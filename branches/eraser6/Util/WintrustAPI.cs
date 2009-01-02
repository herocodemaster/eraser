﻿/* 
 * $Id: UserAPI.cs 461 2008-11-06 10:16:32Z lowjoel $
 * Copyright 2008 The Eraser Project
 * Original Author: Joel Low <lowjoel@users.sourceforge.net>
 * Modified By:
 * 
 * This file is part of Eraser.
 * 
 * Eraser is free software: you can redistribute it and/or modify it under the
 * terms of the GNU General Public License as published by the Free Software
 * Foundation, either version 3 of the License, or (at your option) any later
 * version.
 * 
 * Eraser is distributed in the hope that it will be useful, but WITHOUT ANY
 * WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
 * A PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * 
 * A copy of the GNU General Public License can be found at
 * <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Eraser.Util
{
	public static class WintrustAPI
	{
		/// <summary>
		/// Verifies the Authenticode signature in a file.
		/// </summary>
		/// <param name="pathToFile">The file to verify.</param>
		/// <returns>True if the file contains a valid Authenticode certificate.</returns>
		public static bool VerifyAuthenticode(string pathToFile)
		{
			WINTRUST_FILE_INFO fileinfo = new WINTRUST_FILE_INFO();
			fileinfo.cbStruct = (uint)Marshal.SizeOf(typeof(WINTRUST_FILE_INFO));
			fileinfo.pcwszFilePath = pathToFile;
			fileinfo.hFile = IntPtr.Zero;

			WINTRUST_DATA data = new WINTRUST_DATA();
			data.cbStruct = (uint)Marshal.SizeOf(typeof(WINTRUST_DATA));
			data.dwUIChoice = WINTRUST_DATA.UIChoices.WTD_UI_NONE;
			data.fdwRevocationChecks = WINTRUST_DATA.RevocationChecks.WTD_REVOKE_NONE;
			data.dwUnionChoice = WINTRUST_DATA.UnionChoices.WTD_CHOICE_FILE;
			data.pUnion = Marshal.AllocHGlobal((int)data.cbStruct);
			Marshal.StructureToPtr(fileinfo, data.pUnion, false);

			int result = WinVerifyTrust(IntPtr.Zero, WINTRUST_ACTION_GENERIC_VERIFY_V2, data);
			Marshal.FreeHGlobal(data.pUnion);
			return result == 0;
		}

		/// <summary>
		/// The WinVerifyTrust function performs a trust verification action on a
		/// specified object. The function passes the inquiry to a trust provider
		/// that supports the action identifier, if one exists.
		/// 
		/// For certificate verification, use the CertGetCertificateChain and
		/// CertVerifyCertificateChainPolicy functions.
		/// </summary>
		/// <param name="hWnd">Handle to a caller window. A trust provider can use
		/// this value to determine whether it can interact with the user. However,
		/// trust providers typically perform verification actions with input from
		/// the user.
		/// 
		/// This parameter can be one of the following values.
		/// Value					Meaning
		/// INVALID_HANDLE_VALUE	There is no interactive user. The trust provider
		///							performs the verification action without the
		///							user's assistance.
		///	Zero					The trust provider can use the interactive desktop
		///							to display its user interface.
		///	A valid window handle	A trust provider can treat any value other than
		///							INVALID_HANDLE_VALUE or zero as a valid window
		///							handle that it can use to interact with the user.</param>
		/// <param name="pgActionID">A pointer to a GUID structure that identifies an
		/// action and the trust provider that supports that action. This value indicates
		/// the type of verification action to be performed on the structure pointed to
		/// by pWinTrustData.
		/// 
		/// The WinTrust service is designed to work with trust providers implemented
		/// by third parties. Each trust provider provides its own unique set of action
		/// identifiers. For information about the action identifiers supported by a
		/// trust provider, see the documentation for that trust provider.
		/// 
		/// or example, Microsoft provides a Software Publisher Trust Provider that can
		/// establish the trustworthiness of software being downloaded from the Internet
		/// or some other public network. The Software Publisher Trust Provider supports
		/// the following action identifiers. These constants are defined in Softpub.h.</param>
		/// <param name="pWVTData">A pointer that, when cast as a WINTRUST_DATA structure,
		/// contains information that the trust provider needs to process the specified
		/// action identifier. Typically, the structure includes information that
		/// identifies the object that the trust provider must evaluate.
		/// 
		/// The format of the structure depends on the action identifier. For information
		/// about the data required for a specific action identifier, see the documentation
		/// for the trust provider that supports that action.</param>
		/// <returns>If the trust provider verifies that the subject is trusted for the
		/// specified action, the return value is zero. No other value besides zero
		/// should be considered a successful return.
		/// 
		/// If the trust provider does not verify that the subject is trusted for the
		/// specified action, the function returns a status code from the trust provider.
		/// 
		/// For example, a trust provider might indicate that the subject is not trusted,
		/// or is trusted but with limitations or warnings. The return value can be a
		/// trust-provider-specific value described in the documentation for an individual
		/// trust provider, or it can be one of the following error codes.
		/// 
		/// Return code						Description
		/// TRUST_E_SUBJECT_NOT_TRUSTED		The subject failed the specified verification
		///									action. Most trust providers return a more
		///									detailed error code that describes the reason
		///									for the failure.
		/// TRUST_E_PROVIDER_UNKNOWN		The trust provider is not recognized on this
		///									system.
		/// TRUST_E_ACTION_UNKNOWN			The trust provider does not support the
		///									specified action.
		/// TRUST_E_SUBJECT_FORM_UNKNOWN	The trust provider does not support the form
		///									specified for the subject.</returns>
		[DllImport("Wintrust.dll", CharSet = CharSet.Unicode)]
		private static extern int WinVerifyTrust(IntPtr hWnd, Guid pgActionID,
			WINTRUST_DATA pWVTData);

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		private struct WINTRUST_FILE_INFO
		{
			public uint cbStruct;			// = sizeof(WINTRUST_FILE_INFO)
			public string pcwszFilePath;	// required, file name to be verified
			public IntPtr hFile;			// optional, open handle to pcwszFilePath
			public Guid pgKnownSubject;		// optional: fill if the subject type is known.
		}

		private struct WINTRUST_DATA
		{
			public uint cbStruct;						// = sizeof(WINTRUST_DATA)

			public IntPtr pPolicyCallbackData;			// optional: used to pass data between the app and policy
			public IntPtr pSIPClientData;				// optional: used to pass data between the app and SIP.

			public enum UIChoices : uint
			{
				WTD_UI_ALL = 1,
				WTD_UI_NONE = 2,
				WTD_UI_NOBAD = 3,
				WTD_UI_NOGOOD = 4,
			}
			public UIChoices dwUIChoice;				// required: UI choice.  One of the following.

			public enum RevocationChecks : uint
			{
				WTD_REVOKE_NONE = 0x00000000,
				WTD_REVOKE_WHOLECHAIN = 0x00000001
			}
			public RevocationChecks fdwRevocationChecks;// required: certificate revocation check options

			public enum UnionChoices : uint
			{
				WTD_CHOICE_FILE = 1,
				WTD_CHOICE_CATALOG = 2,
				WTD_CHOICE_BLOB = 3,
				WTD_CHOICE_SIGNER = 4,
				WTD_CHOICE_CERT = 5
			}
			public UnionChoices dwUnionChoice;			// required: which structure is being passed in?

			public IntPtr pUnion;

			public enum StateActions : uint
			{
				WTD_STATEACTION_IGNORE = 0x00000000,
				WTD_STATEACTION_VERIFY = 0x00000001,
				WTD_STATEACTION_CLOSE = 0x00000002,
				WTD_STATEACTION_AUTO_CACHE = 0x00000003,
				WTD_STATEACTION_AUTO_CACHE_FLUSH = 0x00000004
			}
			public StateActions dwStateAction;		// optional (Catalog File Processing)
			IntPtr hWVTStateData;					// optional (Catalog File Processing)
			string pwszURLReference;				// optional: (future) used to determine zone.

			public enum ProviderFlags : uint
			{
				WTD_PROV_FLAGS_MASK = 0x0000FFFF,
				WTD_USE_IE4_TRUST_FLAG = 0x00000001,
				WTD_NO_IE4_CHAIN_FLAG = 0x00000002,
				WTD_NO_POLICY_USAGE_FLAG = 0x00000004,
				WTD_REVOCATION_CHECK_NONE = 0x00000010,
				WTD_REVOCATION_CHECK_END_CERT = 0x00000020,
				WTD_REVOCATION_CHECK_CHAIN = 0x00000040,
				WTD_REVOCATION_CHECK_CHAIN_EXCLUDE_ROOT = 0x00000080,
				WTD_SAFER_FLAG = 0x00000100,
				WTD_HASH_ONLY_FLAG = 0x00000200,
				WTD_USE_DEFAULT_OSVER_CHECK = 0x00000400,
				WTD_LIFETIME_SIGNING_FLAG = 0x00000800,
				WTD_CACHE_ONLY_URL_RETRIEVAL = 0x00001000
			}
			public ProviderFlags dwProvFlags;


			// 17-Dec-2004 JSchwart: re-added to fix build break in other depots
			public enum UIContexts
			{
				WTD_UICONTEXT_EXECUTE = 0,
				WTD_UICONTEXT_INSTALL = 1
			}
			public UIContexts dwUIContext;
		}

		private static readonly Guid WINTRUST_ACTION_GENERIC_VERIFY_V2 = new Guid(0xaac56b,                                         
			unchecked((short)0xcd44), 0x11d0, new byte[] { 0x8c, 0xc2, 0x0, 0xc0, 0x4f, 0xc2, 0x95, 0xee });
	}
}
