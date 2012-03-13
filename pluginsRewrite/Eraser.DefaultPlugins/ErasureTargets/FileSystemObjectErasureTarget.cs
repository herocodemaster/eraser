﻿/* 
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
using System.Linq;
using System.Text;

using System.Runtime.Serialization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.IO;

using Eraser.Util;
using Eraser.Util.ExtensionMethods;
using Eraser.Plugins;
using Eraser.Plugins.ExtensionPoints;
using Eraser.Plugins.Registrars;

namespace Eraser.DefaultPlugins
{
	/// <summary>
	/// Class representing a tangible object (file/folder) to be erased.
	/// </summary>
	[Serializable]
	abstract class FileSystemObjectErasureTarget : ErasureTargetBase
	{
		#region Serialization code
		protected FileSystemObjectErasureTarget(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			Path = (string)info.GetValue("Path", typeof(string));
		}

		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("Path", Path);
		}
		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		protected FileSystemObjectErasureTarget()
			: base()
		{
		}

		/// <summary>
		/// The Progress of this target's erasure. This property must be set
		/// before <see cref="FileSystemObjectErasureTarget.Execute"/> is called.
		/// </summary>
		protected new SteppedProgressManager Progress
		{
			get
			{
				return (SteppedProgressManager)base.Progress;
			}
			set
			{
				base.Progress = value;
			}
		}

		/// <summary>
		/// Retrieves the list of files/folders to erase as a list.
		/// </summary>
		/// <returns>A list containing the paths to all the files to be erased.</returns>
		protected abstract List<StreamInfo> GetPaths();

		/// <summary>
		/// Gets all files in the provided directory.
		/// </summary>
		/// <param name="info">The directory to look files in.</param>
		/// <returns>A list of files found in the directory.</returns>
		/// <remarks>This function does not recurse into directories which are
		/// reparse points.</remarks>
		protected static FileInfo[] GetFiles(DirectoryInfo info)
		{
			List<FileInfo> result = new List<FileInfo>();
			if (info.Exists && (info.Attributes & FileAttributes.ReparsePoint) == 0)
			{
				try
				{
					foreach (DirectoryInfo dir in info.GetDirectories())
						result.AddRange(GetFiles(dir));
					result.AddRange(info.GetFiles());
				}
				catch (UnauthorizedAccessException e)
				{
					Logger.Log(S._("Could not erase files and subfolders in {0} because {1}",
						info.FullName, e.Message), LogLevel.Error);
				}
			}

			return result.ToArray();
		}

		/// <summary>
		/// Adds ADSes of the given file to the list, forcing the open handles to the
		/// files closed if necessary.
		/// </summary>
		/// <param name="file">The file to look for ADSes</param>
		protected static StreamInfo[] GetPathADSes(FileInfo file)
		{
			try
			{
				return file.GetADSes().ToArray();
			}
			catch (FileNotFoundException)
			{
			}
			catch (SharingViolationException)
			{
				//The system cannot open the file, try to force the file handle to close.
				if (!ManagerLibrary.Settings.ForceUnlockLockedFiles)
					throw;

				StringBuilder processStr = new StringBuilder();
				foreach (OpenHandle handle in OpenHandle.Close(file.FullName))
				{
					try
					{
						processStr.AppendFormat(
							System.Globalization.CultureInfo.InvariantCulture,
							"{0}, ", handle.Process.MainModule.FileName);
					}
					catch (System.ComponentModel.Win32Exception)
					{
						processStr.AppendFormat(
							System.Globalization.CultureInfo.InvariantCulture,
							"Process ID {0}, ", handle.Process.Id);
					}
				}

				if (processStr.Length == 0)
					return GetPathADSes(file);
				else
					throw;
			}
			catch (UnauthorizedAccessException e)
			{
				//The system cannot read the file, assume no ADSes for lack of
				//more information.
				Logger.Log(e.Message, LogLevel.Error);
			}

			return new StreamInfo[0];
		}

		/// <summary>
		/// The path to the file or folder referred to by this object.
		/// </summary>
		public string Path { get; set; }

		public sealed override IErasureMethod EffectiveMethod
		{
			get
			{
				if (Method != ErasureMethodRegistrar.Default)
					return base.EffectiveMethod;

				return Host.Instance.ErasureMethods[
					Manager.Settings.DefaultFileErasureMethod];
			}
		}

		public override string UIText
		{
			get
			{
				return Path;
			}
		}

		/// <summary>
		/// Erases the streams returned by the <see cref="GetPaths"/> function.
		/// </summary>
		/// <remarks>The <see cref="Progress"/> property must be defined prior
		/// to the execution of this function.</remarks>
		public override void Execute()
		{
			//Retrieve the list of files to erase.
			List<StreamInfo> paths = GetPaths();
			long dataTotal = paths.Sum(x => x.Length);

			//Set the event's current target status.
			if (Progress == null)
				throw new InvalidOperationException("The Progress property must not be null.");
			Task.Progress.Steps.Add(new SteppedProgressManagerStep(Progress,
				1.0f / Task.Targets.Count));

			//Iterate over every path, and erase the path.
			for (int i = 0; i < paths.Count; ++i)
			{
				ProgressManager step = new ProgressManager();
				Progress.Steps.Add(new SteppedProgressManagerStep(step,
					dataTotal == 0 ? 0.0f : paths[i].Length / (float)dataTotal,
					S._("Erasing files...")));
				EraseStream(paths[i], step);
				step.MarkComplete();
			}
		}

		/// <summary>
		/// Erases the provided stream, and updates progress using the provided
		/// progress manager.
		/// </summary>
		/// <param name="info">The information regarding the stream that needs erasure.</param>
		/// <param name="progress">The progress manager for the erasure of the current
		/// stream.</param>
		protected void EraseStream(StreamInfo info, ProgressManager progress)
		{
			//Check that the file exists - we do not want to bother erasing nonexistant files
			if (!info.Exists)
			{
				Logger.Log(S._("The file {0} was not erased as the file does not exist.",
					info.FileName), LogLevel.Notice);
				return;
			}

			//Get the filesystem provider to handle the secure file erasures
			IFileSystem fsManager = Host.Instance.FileSystems[
				VolumeInfo.FromMountPoint(info.DirectoryName)];

			bool isReadOnly = false;

			try
			{
				//Update the task progress
				IErasureMethod method = EffectiveMethod;
				OnProgressChanged(this, new ProgressChangedEventArgs(progress,
					new TaskProgressChangedEventArgs(info.FullName, 0, method.Passes)));

				//Remove the read-only flag, if it is set.
				if (isReadOnly = info.IsReadOnly)
					info.IsReadOnly = false;

				//Define the callback function for progress reporting.
				ErasureMethodProgressFunction callback =
					delegate(long lastWritten, long totalData, int currentPass)
					{
						if (Task.Canceled)
							throw new OperationCanceledException(S._("The task was cancelled."));

						progress.Total = totalData;
						progress.Completed += lastWritten;
						OnProgressChanged(this, new ProgressChangedEventArgs(progress,
							new TaskProgressChangedEventArgs(info.FullName, currentPass, method.Passes)));
					};

				TryEraseStream(fsManager, method, info, callback);

				//Remove the file.
				FileInfo fileInfo = info.File;
				if (fileInfo != null)
					fsManager.DeleteFile(fileInfo);
				progress.MarkComplete();
			}
			catch (UnauthorizedAccessException)
			{
				Logger.Log(S._("The file {0} could not be erased because the file's " +
					"permissions prevent access to the file.", info.FullName), LogLevel.Error);
			}
			catch (SharingViolationException e)
			{
				Logger.Log(e.Message, LogLevel.Error);
			}
			finally
			{
				//Re-set the read-only flag if the file exists (i.e. there was an error)
				if (isReadOnly && info.Exists && !info.IsReadOnly)
					info.IsReadOnly = isReadOnly;
			}
		}

		/// <summary>
		/// Attempts to erase a stream, trying to close all open handles if a process has
		/// a lock on the file.
		/// </summary>
		/// <param name="fsManager">The file system provider used to erase the stream.</param>
		/// <param name="method">The erasure method to use to erase the stream.</param>
		/// <param name="info">The stream to erase.</param>
		/// <param name="callback">The erasure progress callback.</param>
		private void TryEraseStream(IFileSystem fsManager, IErasureMethod method, StreamInfo info,
			ErasureMethodProgressFunction callback)
		{
			for (int i = 0; ; ++i)
			{
				try
				{
					//Make sure the file does not have any attributes which may affect
					//the erasure process
					if ((info.Attributes & FileAttributes.Compressed) != 0 ||
						(info.Attributes & FileAttributes.Encrypted) != 0 ||
						(info.Attributes & FileAttributes.SparseFile) != 0)
					{
						//Log the error
						Logger.Log(S._("The file {0} could not be erased because the file was " +
							"either compressed, encrypted or a sparse file.", info.FullName),
							LogLevel.Error);
						return;
					}

					//Do not erase reparse points, as they will cause other references to the file
					//to be to garbage.
					if ((info.Attributes & FileAttributes.ReparsePoint) == 0)
						fsManager.EraseFileSystemObject(info, method, callback);
					else
						Logger.Log(S._("The file {0} is a hard link or a symbolic link thus the " +
							"contents of the file was not erased.", LogLevel.Notice));
					return;
				}
				catch (SharingViolationException)
				{
					if (!ManagerLibrary.Settings.ForceUnlockLockedFiles)
						throw;

					//Try closing all open handles. If it succeeds, we can run the erase again.
					//To prevent Eraser from deadlocking, we will only attempt this once. Some
					//programs may be aggressive and keep a handle open in a tight loop.
					List<OpenHandle> remainingHandles = OpenHandle.Close(info.FullName);
					if (i == 0 && remainingHandles.Count == 0)
						continue;

					//Either we could not close all instances, or we already tried twice. Report
					//the error.
					StringBuilder processStr = new StringBuilder();
					foreach (OpenHandle handle in remainingHandles)
					{
						try
						{
							processStr.AppendFormat(
								System.Globalization.CultureInfo.InvariantCulture,
								"{0}, ", handle.Process.MainModule.FileName);
						}
						catch (System.ComponentModel.Win32Exception)
						{
							processStr.AppendFormat(
								System.Globalization.CultureInfo.InvariantCulture,
								"Process ID {0}, ", handle.Process.Id);
						}
					}

					throw new SharingViolationException(S._(
						"Could not force closure of file \"{0}\" {1}", info.FileName,
						S._("(locked by {0})",
							processStr.ToString().Remove(processStr.Length - 2)).Trim()),
						info.FileName);
				}
			}
		}

		/// <summary>
		/// Writes a file for plausible deniability over the current stream.
		/// </summary>
		/// <param name="stream">The stream to write the data to.</param>
		protected static void CopyPlausibleDeniabilityFile(Stream stream)
		{
			//Get the template file to copy
			FileInfo shadowFileInfo;
			{
				string shadowFile = null;
				List<string> entries = new List<string>(
					ManagerLibrary.Settings.PlausibleDeniabilityFiles);
				IPrng prng = Host.Instance.Prngs.ActivePrng;
				do
				{
					if (entries.Count == 0)
						throw new FatalException(S._("Plausible deniability was selected, " +
							"but no decoy files were found. The current file has been only " +
							"replaced with random data."));

					//Get an item from the list of files, and then check that the item exists.
					int index = prng.Next(entries.Count - 1);
					shadowFile = entries[index];
					if (File.Exists(shadowFile) || Directory.Exists(shadowFile))
					{
						if ((File.GetAttributes(shadowFile) & FileAttributes.Directory) != 0)
						{
							DirectoryInfo dir = new DirectoryInfo(shadowFile);
							FileInfo[] files = dir.GetFiles("*", SearchOption.AllDirectories);
							entries.Capacity += files.Length;
							foreach (FileInfo f in files)
								entries.Add(f.FullName);
						}
						else
							shadowFile = entries[index];
					}
					else
						shadowFile = null;

					entries.RemoveAt(index);
				}
				while (string.IsNullOrEmpty(shadowFile));
				shadowFileInfo = new FileInfo(shadowFile);
			}

			//Dump the copy (the first 4MB, or less, depending on the file size and size of
			//the original file)
			long amountToCopy = Math.Min(stream.Length,
				Math.Min(4 * 1024 * 1024, shadowFileInfo.Length));
			using (FileStream shadowFileStream = shadowFileInfo.OpenRead())
			{
				while (stream.Position < amountToCopy)
				{
					byte[] buf = new byte[524288];
					int bytesRead = shadowFileStream.Read(buf, 0, buf.Length);

					//Stop bothering if the input stream is at the end
					if (bytesRead == 0)
						break;

					//Dump the read contents onto the file to be deleted
					stream.Write(buf, 0,
						(int)Math.Min(bytesRead, amountToCopy - stream.Position));
				}
			}
		}
	}
}