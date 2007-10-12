// FileLockResolver.cpp
// $Id$
//
// Eraser. Secure data removal. For Windows.
// Copyright � 1997-2001  Sami Tolvanen (sami@tolvanen.com).
// Copyright � 2001-2006  Garrett Trant (support@heidi.ie).
// Copyright � 2007 The Eraser Project.
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA
// 02111-1307, USA.
#include "stdafx.h"
#include "FileLockResolver.h"
#include "..\Launcher\Launcher.h"
#include <fstream>
#include <string>
#include <iterator>
#include <atlbase.h>

#define LOCKED_FILE_LIST_NAME _T("lock")
#define RUNONCE  "Software\\Microsoft\\Windows\\CurrentVersion\\RunOnce"
#define LAUNCHER "Eraserl.exe"

CFileLockResolver::CFileLockResolver(BOOL askUser)
: m_bAskUser(askUser), m_hHandle(ERASER_INVALID_CONTEXT)
{

}

CFileLockResolver::CFileLockResolver(ERASER_HANDLE h, BOOL askUser)
: m_bAskUser(askUser)
{
	SetHandle(h);
}

void CFileLockResolver::SetHandle(ERASER_HANDLE h)
{
	m_hHandle = h;
	eraserSetErrorHandler(h, ErrorHandler, this);
}

CFileLockResolver::~CFileLockResolver(void)
{
	Close();
}

struct PathHelper
{
	CString& m_strLockFile;
	PathHelper(CString& lockFile, bool path_only = false)
		:m_strLockFile(lockFile)
	{
		char fullname[MAX_PATH];
		char filename[MAX_PATH];
		char extension[MAX_PATH];
		char pathname[MAX_PATH];
		char drive[10];

		GetModuleFileName(AfxGetInstanceHandle(),fullname,sizeof (fullname));
		_splitpath(fullname,drive, pathname, filename, extension); 

		m_strLockFile = drive;
		m_strLockFile += pathname;
		if (path_only )
			return;
		m_strLockFile.Format("%s%s%d.%s", drive, pathname, time(0), LOCKED_FILE_LIST_NAME);
	}
};

struct FileData
{
	std::string name;
	int method;
	unsigned int passes;

	FileData()
	{
	}

	FileData(const std::string& fname, int m, unsigned int pass)
		:name(fname), method(m), passes(pass)
	{
	}

	void read(std::istream& is)
	{
		is >> std::noskipws;
		std::getline(is, name);
	}

	void write(std::ostream& os) const
	{
		os << std::noskipws;
		os << name << std::endl;
	}
};

std::ostream& operator<< (std::ostream& os, const FileData& data)
{
	data.write(os);
	return os;
}
std::istream& operator>> (std::istream& is, FileData& data)
{
	data.read(is);
	return is;
}

void CFileLockResolver::HandleError(LPCTSTR szFileName, DWORD dwErrorCode, int em, unsigned int passes)
{
	if (ERROR_LOCK_VIOLATION == dwErrorCode 
		|| ERROR_DRIVE_LOCKED == dwErrorCode
		|| ERROR_LOCKED == dwErrorCode
		|| ERROR_SHARING_VIOLATION == dwErrorCode)
	{
		if (TRUE == m_bAskUser)
		{
			if (IDYES == AfxGetMainWnd()->MessageBox(CString("The file ") +
				szFileName + "\nis locked by another process. Do you want to Erase the file after " +
				"you restart your computer?", "File Access Denied", MB_YESNO | MB_ICONQUESTION))
			{
				static PathHelper path(m_strLockFileList);
				std::ofstream os(m_strLockFileList, std::ios_base::out | std::ios_base::app);		
				os << FileData(szFileName, em, passes);
			}
		}
	}
}

void CFileLockResolver::Resolve(LPCTSTR szFileName, CStringArray& ar)
{
	std::ifstream is(szFileName);
	if (is.fail())
		throw std::runtime_error("Unable to resolve locked files. Can't open file list");

	while (!is.eof()) 
	{
		FileData data;
		is >> data;
		if (!data.name.empty())
			ar.Add(data.name.c_str());
	}
	is.close();
	DeleteFile(szFileName);
}

DWORD CFileLockResolver::ErrorHandler(LPCTSTR szFileName, DWORD dwErrorCode, void* ctx, void* param)
{
	CFileLockResolver* self(static_cast<CFileLockResolver*>(param));
	CEraserContext* ectx(static_cast<CEraserContext* >(ctx));
	self->HandleError(szFileName, dwErrorCode, ectx->m_lpmMethod->m_nMethodID, ectx->m_lpmMethod->m_nPasses);
	return 0UL;
}

void CFileLockResolver::Close()
{
	eraserSetErrorHandler(m_hHandle, NULL, NULL);

	if (m_strLockFileList.IsEmpty())
		return;

	CString strPath;
	PathHelper(strPath, true);
	strPath = CString("\"") + strPath + LAUNCHER + "\" " + szResolveLock;
	strPath += " \"" + m_strLockFileList + "\"";

	extern bool no_registry;
	if (!no_registry)
	{
		CRegKey key;
		if (ERROR_SUCCESS == key.Open(HKEY_LOCAL_MACHINE, RUNONCE))
		{
			key.SetStringValue(LAUNCHER, strPath);
			m_strLockFileList = "";
		}
	}
}
