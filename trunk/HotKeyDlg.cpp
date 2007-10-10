// HotKeyDlg.cpp : implementation file
//

#include "stdafx.h"
#include "Eraser.h"
#include "HotKeyDlg.h"
#include "KeyComboDlg.h"
#include <shared/key.h>


const LPCTSTR szAccelerKey = "Acceler";
static const int iColumnCount = 2;
static const LPTSTR szColumnNames[] =
{
	"Menu command",
	"Hot key combination"	
};

static const LPTSTR szDefAccelerKeys[] =
{
	"E",
	"M",
    "U"
};

static int iColumnWidths[] =
{
	190, 110
};


static LPCTSTR szCommandNames[] =
{
	"Erase",
	"Eraser Secure Move",
    "Erase Unused Space"
};

static void CreateList(CListCtrl& lcHKey)
{
	LVCOLUMN lvc;
	ZeroMemory(&lvc, sizeof(LVCOLUMN));

	lvc.mask        = LVCF_FMT | LVCF_SUBITEM | LVCF_TEXT | LVCF_WIDTH;
	lvc.fmt         = LVCFMT_LEFT;
	lvc.pszText     = szColumnNames[0];
	lvc.cx          = iColumnWidths[0];
	lvc.iSubItem    = 0;
	lcHKey.InsertColumn(0, &lvc);

	lvc.mask        = LVCF_FMT | LVCF_SUBITEM | LVCF_TEXT | LVCF_WIDTH;
	lvc.fmt         = LVCFMT_LEFT;
	lvc.pszText     = szColumnNames[1];
	lvc.cx          = iColumnWidths[1];
	lvc.iSubItem    = 1;
	lcHKey.InsertColumn(1, &lvc);
	
	lcHKey.SetExtendedStyle(LVS_EX_HEADERDRAGDROP | LVS_EX_FULLROWSELECT | LVS_EX_GRIDLINES);
}

__declspec(dllimport) bool no_registry;

void InitRegistry()
{
	CKey kReg_reg;
	CIniKey kReg_ini;
	CKey &kReg = no_registry ? kReg_ini : kReg_reg;
	CString strPath = _T("");
	strPath.Format("%s\\%s", ERASER_REGISTRY_BASE, szAccelerKey);
	
	if (!no_registry) {
		if (!kReg.Open(HKEY_CURRENT_USER, strPath, FALSE))
		{
			if (kReg.Open(HKEY_CURRENT_USER, strPath, TRUE))
			{
				for (int i = 0; i < iCommandCount; i++) {
					CString strCommandKey = szDefAccelerKeys[i];
					kReg.SetValue(strCommandKey,szCommandNames[i]);				
				}
			}
			kReg.Close();
		}
	} else {
		kReg.Open(HKEY_CURRENT_USER, strPath);
		DWORD count = 0;
		kReg.GetValue(count, "__count", 0);
		if (!count) {
			CString temp;
			for (int i = 0; i < iCommandCount; i++) {
				temp.Format("__key_%ld", i);
				kReg.SetValue(szDefAccelerKeys[i], temp);
				temp.Format("__value_%ld", i);
				kReg.SetValue(szCommandNames[i], temp);
			}
			kReg.SetValue(iCommandCount, "__count");
		}
	}
}

void CHotKeyDlg::LoadValuesFromRegistry()
{
	CKey kReg_reg;
	CIniKey kReg_ini;
	CKey &kReg = no_registry ? kReg_ini : kReg_reg;
	CString	strPath =_T("");
	CString strValueName=_T("");
	CString strValue=_T("");
	
	strPath.Format("%s\\%s", ERASER_REGISTRY_BASE, szAccelerKey);
	InitRegistry();

	try
	{
		m_lcHotKeys.DeleteAllItems();
		if (kReg.Open(HKEY_CURRENT_USER, strPath))
		{
			if (!no_registry) {
				DWORD dwId = 0;
				while (kReg.GetNextValueName(strValueName,dwId)) {
					if (kReg.GetValue(strValue,strValueName))
					{
						m_arKeyValues.SetAt(strValueName,strValue);					
					}
					dwId++;
					strValueName="";
					strValueName.ReleaseBuffer();
				}
			} else {
				DWORD count = 0;
				kReg.GetValue(count, "__count", 0);
				for (int i = 0; i < count; i++) {
					CString temp, key, value;
					temp.Format("__key_%ld", i);
					kReg.GetValue(key, temp, "");
					temp.Format("__value_%ld", i);
					kReg.GetValue(value, temp, "");
					if ((key.GetLength() > 0) && (value.GetLength() > 0))
						m_arKeyValues.SetAt(key, value);
				}
			}
		}
	}
	catch(...)
	{
		ASSERT(FALSE);
	}
	kReg.Close();
}

void CHotKeyDlg::UpdateHotKeyList(CListCtrl& lcHKey)
{
	
	lcHKey.SetRedraw(FALSE);
	
	LV_ITEM         lvi;
	POSITION		pos;
	CString			strKey = _T("");
	CString			strCommand =_T("");
		
	ZeroMemory(&lvi, sizeof(LV_ITEM));
	try
	{
		lcHKey.DeleteAllItems();
		BYTE nItem = 0;
		for (pos = m_arKeyValues.GetStartPosition(); pos != NULL; nItem++) 
		{
			m_arKeyValues.GetNextAssoc(pos,strCommand,strKey);
				
			lvi.mask        = LVIF_TEXT ;
			lvi.iItem       = nItem;
			lvi.iSubItem    = 0;
			lvi.pszText     = strCommand.GetBuffer(strCommand.GetLength());
			lvi.iItem       = lcHKey.InsertItem(&lvi);
											
			lvi.mask        = LVIF_TEXT;
			lvi.iSubItem    = 1;
			lvi.pszText     = strKey.GetBuffer(strKey.GetLength()); //strCommandKey.GetBuffer(strTmp.GetLength());
			lcHKey.SetItem(&lvi);										
		}		
	}
	catch (...)
	{
		ASSERT(FALSE);
	}	
	lcHKey.SetRedraw(TRUE);	
}

void CHotKeyDlg::saveListToRegistry()
{
	CKey     kReg_reg;
	CIniKey  kReg_ini;
	CKey    &kReg = no_registry ? kReg_ini : kReg_reg;
	CString  strPath;

	strPath.Format("%s\\%s", ERASER_REGISTRY_BASE, szAccelerKey);

	if (kReg.Open(HKEY_CURRENT_USER, strPath))
	{
		CString strKey=_T(""), strCommand = _T("");
		POSITION pos;
		int cnt = 0;
		for (pos = m_arKeyValues.GetStartPosition(); pos != NULL;) 
		{
			m_arKeyValues.GetNextAssoc(pos,strCommand,strKey);
			if (!no_registry) {
				kReg.SetValue(strKey,strCommand);
			} else {
				CString temp;
				temp.Format("__key_%ld", cnt);
				kReg.SetValue(strKey, temp);
				temp.Format("__value_%ld", cnt);
				kReg.SetValue(strCommand, temp);
			}
			cnt++;
		}
		if (no_registry)
			kReg.SetValue(cnt, "__count");
	}
	kReg.Close();
}

// CHotKeyDlg dialog

IMPLEMENT_DYNAMIC(CHotKeyDlg, CDialog)
CHotKeyDlg::CHotKeyDlg(CWnd* pParent /*=NULL*/, int iValCnt)
	: CDialog(CHotKeyDlg::IDD, pParent),m_arKeyValues(),m_lcHotKeys()
{	
}

CHotKeyDlg::~CHotKeyDlg()
{	
}

void CHotKeyDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_LIST_HOTKEYS, m_lcHotKeys);	
}

BOOL CHotKeyDlg::OnInitDialog()
{	
	try
	{
		CDialog::OnInitDialog();
		CreateList(m_lcHotKeys);
		LoadValuesFromRegistry();
		UpdateHotKeyList(m_lcHotKeys);
		GetDlgItem(IDCHANGE)->EnableWindow(FALSE);		
	}
	catch(...)
	{
		ASSERT(FALSE);
	}
	return TRUE;
}


BEGIN_MESSAGE_MAP(CHotKeyDlg, CDialog)
	ON_BN_CLICKED(IDCHANGE, OnBnClickedChange)
	ON_WM_ACTIVATE()
	ON_NOTIFY(NM_CLICK, IDC_LIST_HOTKEYS, OnNMClickListHotkeys)
	ON_BN_CLICKED(IDC_BUTTON_OKCHANGE, OnBnClickedButtonOk)
END_MESSAGE_MAP()


// CHotKeyDlg message handlers

void CHotKeyDlg::OnBnClickedChange()
{
	// TODO: Add your control notification handler code here
	if (POSITION pos=m_lcHotKeys.GetFirstSelectedItemPosition())
	{
		CString strKey;
		strKey=m_lcHotKeys.GetItemText((int)pos-1,0);
		CKeyComboDlg dlg(m_lcHotKeys.GetItemText((int)pos-1,1),strKey);
		if (IDOK ==dlg.DoModal())
		{
			m_arKeyValues.SetAt(strKey,dlg.m_strValue);
			UpdateHotKeyList(m_lcHotKeys);
		}
		GetDlgItem(IDCHANGE)->EnableWindow(FALSE);			
	}
}

void CHotKeyDlg::OnActivate(UINT nState, CWnd* pWndOther, BOOL bMinimized)
{
	CDialog::OnActivate(nState, pWndOther, bMinimized);

	// TODO: Add your message handler code here
    UpdateHotKeyList(m_lcHotKeys);
	GetDlgItem(IDCHANGE)->EnableWindow(FALSE);
}



void CHotKeyDlg::OnNMClickListHotkeys(NMHDR *pNMHDR, LRESULT *pResult)
{
	// TODO: Add your control notification handler code here
	if (m_lcHotKeys.GetFirstSelectedItemPosition() != NULL)
        GetDlgItem(IDCHANGE)->EnableWindow(TRUE);
	else 
		GetDlgItem(IDCHANGE)->EnableWindow(FALSE);
	*pResult = 0;
}

void CHotKeyDlg::OnBnClickedButtonOk()
{
    saveListToRegistry();	
	this->OnOK();
}
