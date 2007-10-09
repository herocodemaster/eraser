// stdafx.h : include file for standard system include files,
//  or project specific include files that are used frequently, but
//      are changed infrequently
//
// Eraser. Secure data removal. For Windows.
// Copyright � 1997-2001  Sami Tolvanen (sami@tolvanen.com).
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

#if !defined(AFX_STDAFX_H__70E9C856_F0D1_11D2_BBF3_00105AAF62C4__INCLUDED_)
#define AFX_STDAFX_H__70E9C856_F0D1_11D2_BBF3_00105AAF62C4__INCLUDED_

#pragma once

#ifndef VC_EXTRALEAN
#define VC_EXTRALEAN	// Exclude rarely-used items from Windows headers.
#endif

// Modify the following defines if you have to target an OS before the ones 
// specified in the following code. See MSDN for the latest information
// about corresponding values for different operating systems.
#ifndef WINVER		// Permit use of features specific to Windows 95 and Windows NT 4.0 or later.
#define WINVER 0x0500	// Change this to the appropriate value to target 
#endif                     // Windows 98 and Windows 2000 or later.

#ifndef _WIN32_WINNT	// Permit use of features specific to Windows NT 4.0 or later.
#define _WIN32_WINNT 0x0500	// Change this to the appropriate value to target 
#endif		         // Windows 98 and Windows 2000 or later.			

#ifndef _WIN32_WINDOWS	      // Permit use of features specific to Windows 98 or later.
#define _WIN32_WINDOWS 0x0500    // Change this to the appropriate value to target 
#endif			      // Windows Millennium Edition or later.

#ifndef _WIN32_IE		// Permit use of features specific to Internet Explorer 4.0 or later.
#define _WIN32_IE 0x0500   // Change this to the appropriate value to target 
#endif			// Internet Explorer 5.0 or later.

#define _ATL_CSTRING_EXPLICIT_CONSTRUCTORS  // Some CString constructors will be explicit.

// Turns off MFC feature that hides of some common warning messages
// that are frequently and safely ignored .
#define _AFX_ALL_WARNINGS

#include <afxwin.h>         // MFC core and standard components
#include <afxext.h>         // MFC extensions
#include <afxdtctl.h>       // MFC support for Internet Explorer 4 Common Controls
#ifndef _AFX_NO_AFXCMN_SUPPORT
#include <afxcmn.h>         // MFC support for Windows Common Controls
#endif // _AFX_NO_AFXCMN_SUPPORT
#include <afxcview.h>
#include <afxmt.h>
#include <afxcoll.h>
#include <afxole.h>

#include "DateTimeInit.h"

#include <eh.h>                 // structured exception
#include "shared\SeException.h" // handling
#include <afxdisp.h>
#include <afxtempl.h>
#include <afxmt.h>
#include <shlobj.h>
#include <shlwapi.h>
#include <lm.h>
#include <eh.h>             // structured exception
#include "EraserUI\FileDialogEx.h"

#ifndef _AFXDLL
#define AfxLoadLibrary  ::LoadLibrary
#define AfxFreeLibrary  ::FreeLibrary
#endif

#define REPORT_ERROR(e) \
    (((e)->IsKindOf(RUNTIME_CLASS(CSeException))) ?\
        ReportExceptionError((CSeException*)(e), __FILE__, __LINE__, __DATE__, __TIME__) :\
        (e)->ReportError(MB_ICONERROR))

inline void ReportExceptionError(CSeException *e, LPCTSTR szFile,
                                 const int iLine, LPCTSTR szDate,
                                 LPCTSTR szTime)
{
    try
    {
        CString strException;
        CString strError;

        e->GetErrorMessage(strException);

        strError.Format("%s\n\nFile: %s (%i)\nCompiled: %s, %s",
                        (LPCTSTR)strException,
                        szFile, iLine,
                        szDate, szTime);

        AfxMessageBox(strError, MB_ICONERROR, 0);
    }
    catch (...)
    {
    }
}

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_STDAFX_H__70E9C856_F0D1_11D2_BBF3_00105AAF62C4__INCLUDED_)
