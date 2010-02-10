/* 
 * $Id$
 * Copyright 2008-2010 The Eraser Project
 * Original Author: Joel Low <lowjoel@users.sourceforge.net>
 * Modified By: Kasra Nassiri <cjax@users.sourceforge.net> @10/18/2008
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Eraser.Manager;
using Eraser.Manager.Plugin;
using Microsoft.Win32;
using System.Globalization;
using Eraser.Util;
using System.Threading;

namespace Eraser
{
	internal partial class SettingsPanel : BasePanel
	{
		public SettingsPanel()
		{
			InitializeComponent();

			//For new plugins, register the callback.
			Host.Instance.PluginLoaded += OnNewPluginLoaded;
			ManagerLibrary.Instance.ErasureMethodManager.Registered += OnMethodRegistered;
			ManagerLibrary.Instance.ErasureMethodManager.Unregistered += OnMethodUnregistered;

			//Load the values
			LoadPluginDependantValues();
			LoadSettings();
		}

		private void OnNewPluginLoaded(object sender, PluginLoadedEventArgs e)
		{
			ListViewItem item = new ListViewItem();
			if (e.Instance.Plugin == null)
			{
				item.Text = System.IO.Path.GetFileNameWithoutExtension(e.Instance.Assembly.Location);
				item.SubItems.Add(e.Instance.AssemblyInfo.Author);
			}
			else
			{
				item.Text = e.Instance.Plugin.Name;
				item.SubItems.Add(e.Instance.Plugin.Author);
			}

			//The item is checked if the plugin was given the green light to load
			item.Checked = e.Instance.Plugin != null ||
				(Manager.ManagerLibrary.Settings.PluginApprovals.ContainsKey(
					e.Instance.AssemblyInfo.Guid) && Manager.ManagerLibrary.
					Settings.PluginApprovals[e.Instance.AssemblyInfo.Guid]
				);

			//Visually display the other metadata associated with the assembly
			item.ImageIndex = e.Instance.AssemblyAuthenticode == null ? -1 : 0;
			item.Group = e.Instance.IsCore ? pluginsManager.Groups[0] :
				pluginsManager.Groups[1];
			item.SubItems.Add(e.Instance.Assembly.GetName().Version.ToString());
			item.SubItems.Add(e.Instance.Assembly.Location);
			item.Tag = e.Instance;
			pluginsManager.Items.Add(item);
		}

		private void OnMethodRegistered(object sender, EventArgs e)
		{
			ErasureMethod method = (ErasureMethod)sender;
			eraseFilesMethod.Items.Add(method);
			if (method is UnusedSpaceErasureMethod)
				eraseUnusedMethod.Items.Add(method);
		}

		private void OnMethodUnregistered(object sender, EventArgs e)
		{
			ErasureMethod method = (ErasureMethod)sender;
			foreach (object obj in eraseFilesMethod.Items)
				if (((ErasureMethod)obj).Guid == method.Guid)
				{
					eraseFilesMethod.Items.Remove(obj);
					break;
				}

			foreach (object obj in eraseUnusedMethod.Items)
				if (((ErasureMethod)obj).Guid == method.Guid)
				{
					eraseUnusedMethod.Items.Remove(obj);
					break;
				}

			if (eraseFilesMethod.SelectedIndex == -1)
				eraseFilesMethod.SelectedIndex = 0;
			if (eraseUnusedMethod.SelectedIndex == -1)
				eraseUnusedMethod.SelectedIndex = 0;
		}

		private void LoadPluginDependantValues()
		{
			//Load the list of plugins
			Host instance = Host.Instance;
			IEnumerator<PluginInstance> i = instance.Plugins.GetEnumerator();
			while (i.MoveNext())
				OnNewPluginLoaded(this, new PluginLoadedEventArgs(i.Current));

			//Refresh the list of languages
			IList<CultureInfo> languages = S.Localisations;
			foreach (CultureInfo culture in languages)
				uiLanguage.Items.Add(culture);

			//Refresh the list of erasure methods
			foreach (ErasureMethod method in ManagerLibrary.Instance.ErasureMethodManager)
			{
				eraseFilesMethod.Items.Add(method);
				if (method is UnusedSpaceErasureMethod)
					eraseUnusedMethod.Items.Add(method);
			}

			//Refresh the list of PRNGs
			foreach (Prng prng in ManagerLibrary.Instance.PRNGManager)
				erasePRNG.Items.Add(prng);
		}

		private void LoadSettings()
		{
			EraserSettings settings = EraserSettings.Get();
			foreach (CultureInfo lang in uiLanguage.Items)
				if (lang.Name == settings.Language)
				{
					uiLanguage.SelectedItem = lang;
					break;
				}

			foreach (ErasureMethod method in eraseFilesMethod.Items)
				if (method.Guid == ManagerLibrary.Settings.DefaultFileErasureMethod)
				{
					eraseFilesMethod.SelectedItem = method;
					break;
				}

			foreach (ErasureMethod method in eraseUnusedMethod.Items)
				if (method.Guid == ManagerLibrary.Settings.DefaultUnusedSpaceErasureMethod)
				{
					eraseUnusedMethod.SelectedItem = method;
					break;
				}

			foreach (Prng prng in erasePRNG.Items)
				if (prng.Guid == ManagerLibrary.Settings.ActivePrng)
				{
					erasePRNG.SelectedItem = prng;
					break;
				}

			foreach (string path in ManagerLibrary.Settings.PlausibleDeniabilityFiles)
				plausibleDeniabilityFiles.Items.Add(path);

			uiContextMenu.Checked = settings.IntegrateWithShell;
			lockedForceUnlock.Checked =
				ManagerLibrary.Settings.ForceUnlockLockedFiles;
			schedulerMissedImmediate.Checked =
				ManagerLibrary.Settings.ExecuteMissedTasksImmediately;
			schedulerMissedIgnore.Checked =
				!ManagerLibrary.Settings.ExecuteMissedTasksImmediately;
			plausibleDeniability.Checked =
				ManagerLibrary.Settings.PlausibleDeniability;
			plausibleDeniability_CheckedChanged(plausibleDeniability, new EventArgs());
			schedulerClearCompleted.Checked = settings.ClearCompletedTasks;

			List<string> defaultsList = new List<string>();

			//Select an intelligent default if the settings are invalid.
			if (uiLanguage.SelectedIndex == -1)
			{
				foreach (CultureInfo lang in uiLanguage.Items)
					if (lang.Name == "en")
					{
						uiLanguage.SelectedItem = lang;
						break;
					}
			}
			if (eraseFilesMethod.SelectedIndex == -1)
			{
				if (eraseFilesMethod.Items.Count > 0)
				{
					eraseFilesMethod.SelectedIndex = 0;
					ManagerLibrary.Settings.DefaultFileErasureMethod =
						((ErasureMethod)eraseFilesMethod.SelectedItem).Guid;
				}
				defaultsList.Add(S._("Default file erasure method"));
			}
			if (eraseUnusedMethod.SelectedIndex == -1)
			{
				if (eraseUnusedMethod.Items.Count > 0)
				{
					eraseUnusedMethod.SelectedIndex = 0;
					ManagerLibrary.Settings.DefaultUnusedSpaceErasureMethod =
						((ErasureMethod)eraseUnusedMethod.SelectedItem).Guid;
				}
				defaultsList.Add(S._("Default unused space erasure method"));
			}
			if (erasePRNG.SelectedIndex == -1)
			{
				if (erasePRNG.Items.Count > 0)
				{
					erasePRNG.SelectedIndex = 0;
					ManagerLibrary.Settings.ActivePrng =
						((Prng)erasePRNG.SelectedItem).Guid;
				}
				defaultsList.Add(S._("Randomness data source"));
			}

			//Remind the user.
			if (defaultsList.Count != 0)
			{
				string defaults = string.Empty;
				foreach (string item in defaultsList)
					defaults += "\t" + item + "\n";
				MessageBox.Show(S._("The following settings held invalid values:\n\n" +
					"{0}\nThese settings have now been set to naive defaults.\n\n" +
					"Please check that the new settings suit your required level of security.",
					defaults), S._("Eraser"), MessageBoxButtons.OK, MessageBoxIcon.Warning,
					MessageBoxDefaultButton.Button1,
					S.IsRightToLeft(this) ?
						MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign : 0);
				saveSettings_Click(null, null);
			}
		}

		private void plausableDeniabilityFilesRemoveUpdate()
		{
			plausibleDeniabilityFilesRemove.Enabled = plausibleDeniability.Checked &&
				plausibleDeniabilityFiles.SelectedIndices.Count > 0;
		}

		private void plausibleDeniability_CheckedChanged(object sender, EventArgs e)
		{
			plausibleDeniabilityFiles.Enabled = plausibleDeniabilityFilesAddFile.Enabled =
				plausibleDeniabilityFilesAddFolder.Enabled = plausibleDeniability.Checked;
			plausableDeniabilityFilesRemoveUpdate();
		}

		private void plausibleDeniabilityFiles_SelectedIndexChanged(object sender, EventArgs e)
		{
			plausableDeniabilityFilesRemoveUpdate();
		}

		private void plausibleDeniabilityFilesAddFile_Click(object sender, EventArgs e)
		{
			if (openFileDialog.ShowDialog() == DialogResult.OK)
				plausibleDeniabilityFiles.Items.AddRange(openFileDialog.FileNames);

			plausableDeniabilityFilesRemoveUpdate();
		}

		private void plausibleDeniabilityFilesAddFolder_Click(object sender, EventArgs e)
		{
			try
			{
				if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
					plausibleDeniabilityFiles.Items.Add(folderBrowserDialog.SelectedPath);
				plausableDeniabilityFilesRemoveUpdate();
			}
			catch (NotSupportedException)
			{
				MessageBox.Show(this, S._("The path you selected is invalid."), S._("Eraser"),
					MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1,
					S.IsRightToLeft(this) ?
						MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign : 0);
			}
		}

		private void plausibleDeniabilityFilesRemove_Click(object sender, EventArgs e)
		{
			if (plausibleDeniabilityFiles.SelectedIndex != -1)
			{
				ListBox.SelectedObjectCollection items =
					plausibleDeniabilityFiles.SelectedItems;

				while (items.Count > 0)
					plausibleDeniabilityFiles.Items.Remove(items[0]);
				plausableDeniabilityFilesRemoveUpdate();
			}
		}

		private void pluginsManager_ItemCheck(object sender, ItemCheckEventArgs e)
		{
			ListViewItem item = pluginsManager.Items[e.Index];
			PluginInstance instance = (PluginInstance)item.Tag;
			if (instance.IsCore)
				e.NewValue = CheckState.Checked;
		}

		private void pluginsMenu_Opening(object sender, CancelEventArgs e)
		{
			if (pluginsManager.SelectedItems.Count == 1)
			{
				PluginInstance instance = (PluginInstance)pluginsManager.SelectedItems[0].Tag;
				e.Cancel = instance.Plugin == null || !instance.Plugin.Configurable;
			}
			else
				e.Cancel = true;
		}

		private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (pluginsManager.SelectedItems.Count != 1)
				return;

			PluginInstance instance = (PluginInstance)pluginsManager.SelectedItems[0].Tag;
			instance.Plugin.DisplaySettings(this);
		}

		private void saveSettings_Click(object sender, EventArgs e)
		{
			EraserSettings settings = EraserSettings.Get();
			ManagerSettings managerSettings = ManagerLibrary.Settings;

			//Save the settings that don't fail first.
			managerSettings.ForceUnlockLockedFiles = lockedForceUnlock.Checked;
			managerSettings.ExecuteMissedTasksImmediately = schedulerMissedImmediate.Checked;
			settings.ClearCompletedTasks = schedulerClearCompleted.Checked;

			bool pluginApprovalsChanged = false;
			IDictionary<Guid, bool> pluginApprovals = managerSettings.PluginApprovals;
			foreach (ListViewItem item in pluginsManager.Items)
			{
				PluginInstance plugin = (PluginInstance)item.Tag;
				Guid guid = plugin.AssemblyInfo.Guid;
				if (!pluginApprovals.ContainsKey(guid))
				{
					if (plugin.Loaded != item.Checked)
					{
						pluginApprovals.Add(guid, item.Checked);
						pluginApprovalsChanged = true;
					}
				}
				else if (pluginApprovals[guid] != item.Checked)
				{
					pluginApprovals[guid] = item.Checked;
					pluginApprovalsChanged = true;
				}
			}

			if (pluginApprovalsChanged)
			{
				MessageBox.Show(this, S._("Plugins which have just been approved will only be loaded " +
					"the next time Eraser is started."), S._("Eraser"), MessageBoxButtons.OK,
					MessageBoxIcon.Information, MessageBoxDefaultButton.Button1,
					S.IsRightToLeft(this) ?
						MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign : 0);
			}

			//Error checks for the rest that do.
			errorProvider.Clear();
			if (uiLanguage.SelectedIndex == -1)
			{
				errorProvider.SetError(uiLanguage, S._("An invalid language was selected."));
				return;
			}
			else if (eraseFilesMethod.SelectedIndex == -1)
			{
				errorProvider.SetError(eraseFilesMethod, S._("An invalid file erasure method " +
					"was selected."));
				return;
			}
			else if (eraseUnusedMethod.SelectedIndex == -1)
			{
				errorProvider.SetError(eraseUnusedMethod, S._("An invalid unused disk space " +
					"erasure method was selected."));
				return;
			}
			else if (erasePRNG.SelectedIndex == -1)
			{
				errorProvider.SetError(erasePRNG, S._("An invalid randomness data " +
					"source was selected."));
				return;
			}
			else if (plausibleDeniability.Checked && plausibleDeniabilityFiles.Items.Count == 0)
			{
				errorProvider.SetError(plausibleDeniabilityFiles, S._("Erasures with plausible deniability " +
					"was selected, but no files were selected to be used as decoys."));
				errorProvider.SetIconPadding(plausibleDeniabilityFiles, -16);
				return;
			}

			if (CultureInfo.CurrentUICulture.Name != ((CultureInfo)uiLanguage.SelectedItem).Name)
			{
				settings.Language = ((CultureInfo)uiLanguage.SelectedItem).Name;
				MessageBox.Show(this, S._("The new UI language will take only effect when " +
					"Eraser is restarted."), S._("Eraser"), MessageBoxButtons.OK,
					MessageBoxIcon.Information, MessageBoxDefaultButton.Button1,
					S.IsRightToLeft(this) ?
						MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign : 0);
			}
			settings.IntegrateWithShell = uiContextMenu.Checked;

			managerSettings.DefaultFileErasureMethod =
				((ErasureMethod)eraseFilesMethod.SelectedItem).Guid;
			managerSettings.DefaultUnusedSpaceErasureMethod =
				((ErasureMethod)eraseUnusedMethod.SelectedItem).Guid;

			Prng newPRNG = (Prng)erasePRNG.SelectedItem;
			if (newPRNG.Guid != managerSettings.ActivePrng)
			{
				MessageBox.Show(this, S._("The new randomness data source will only be used when " +
					"the next task is run.\nCurrently running tasks will use the old source."),
					S._("Eraser"), MessageBoxButtons.OK, MessageBoxIcon.Information,
					MessageBoxDefaultButton.Button1,
					S.IsRightToLeft(this) ?
						MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign : 0);
				managerSettings.ActivePrng = newPRNG.Guid;
			}
			
			managerSettings.PlausibleDeniability = plausibleDeniability.Checked;
			IList<string> plausibleDeniabilityFilesList = managerSettings.PlausibleDeniabilityFiles;
			plausibleDeniabilityFilesList.Clear();
			foreach (string str in this.plausibleDeniabilityFiles.Items)
				plausibleDeniabilityFilesList.Add(str);
		}
	}
}

