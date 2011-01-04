/* 
 * $Id$
 * Copyright 2008-2010 The Eraser Project
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
using System.Runtime.Serialization;

using Eraser.Plugins;
using Eraser.Util;

namespace Eraser.Manager
{
	/// <summary>
	/// The library instance which initializes and cleans up data required for the
	/// library to function.
	/// </summary>
	public class ManagerLibrary : IDisposable
	{
		public ManagerLibrary(SettingsManager settings)
		{
			if (Instance != null)
				throw new InvalidOperationException("Only one ManagerLibrary instance can " +
					"exist at any one time");

			Instance = this;
			SettingsManager = settings;

			EntropySourceRegistrar = new EntropySourceRegistrar();
			PrngRegistrar = new PrngRegistrar();
			ErasureMethodRegistrar = new ErasureMethodRegistrar();
			ErasureTargetRegistrar = new ErasureTargetRegistrar();
			FileSystemRegistrar = new FileSystemRegistrar();
		}

		~ManagerLibrary()
		{
			Dispose(false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (SettingsManager == null)
				return;

			if (disposing)
			{
				EntropySourceRegistrar.Poller.Abort();
				Host.Instance.Dispose();
				SettingsManager.Save();
			}

			SettingsManager = null;
			Instance = null;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void OnPluginLoad(object sender, PluginLoadEventArgs e)
		{
			//If the plugin does not have an approval or denial, check for the presence of
			//a valid signature.
			IDictionary<Guid, bool> approvals = ManagerLibrary.Settings.PluginApprovals;
			if (!approvals.ContainsKey(e.Plugin.AssemblyInfo.Guid) &&
				(e.Plugin.Assembly.GetName().GetPublicKey().Length == 0 ||
				!Security.VerifyStrongName(e.Plugin.Assembly.Location) ||
				e.Plugin.AssemblyAuthenticode == null))
			{
				e.Load = false;
			}

			//Is there an approval or denial?
			else if (approvals.ContainsKey(e.Plugin.AssemblyInfo.Guid))
				e.Load = approvals[e.Plugin.AssemblyInfo.Guid];

			//There's no approval or denial, what is the specified loading policy?
			else
				e.Load = e.Plugin.LoadingPolicy != LoadingPolicy.DefaultOff;
		}

		/// <summary>
		/// The global library instance.
		/// </summary>
		public static ManagerLibrary Instance { get; private set; }

		/// <summary>
		/// The global instance of the EntropySource Manager
		/// </summary>
		public EntropySourceRegistrar EntropySourceRegistrar { get; private set; }

		/// <summary>
		/// The global instance of the PRNG Manager.
		/// </summary>
		public PrngRegistrar PrngRegistrar { get; private set; }

		/// <summary>
		/// The global instance of the Erasure method Manager.
		/// </summary>
		public ErasureMethodRegistrar ErasureMethodRegistrar { get; private set; }

		/// <summary>
		/// The global instance of the Erasure target Manager.
		/// </summary>
		public ErasureTargetRegistrar ErasureTargetRegistrar { get; private set; }

		/// <summary>
		/// The global instance of the File System manager.
		/// </summary>
		public FileSystemRegistrar FileSystemRegistrar { get; private set; }

		/// <summary>
		/// Global instance of the Settings manager.
		/// </summary>
		public SettingsManager SettingsManager { get; set; }

		/// <summary>
		/// Gets the settings object representing the settings for the Eraser
		/// Manager. This is just shorthand for the local classes.
		/// </summary>
		public static ManagerSettings Settings
		{
			get
			{
				if (settingsInstance == null)
					settingsInstance = new ManagerSettings();
				return settingsInstance;
			}
		}

		/// <summary>
		/// The singleton instance for <see cref="Settings"/>.
		/// </summary>
		private static ManagerSettings settingsInstance;
	}
}