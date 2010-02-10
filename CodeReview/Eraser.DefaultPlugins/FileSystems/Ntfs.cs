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
using System.Text;
using System.Runtime.InteropServices;

using System.IO;
using Eraser.Manager;
using Eraser.Util;

namespace Eraser.DefaultPlugins
{
	/// <summary>
	/// Provides functions to handle erasures specific to NTFS volumes.
	/// </summary>
	[Guid("{34399F62-0AD4-411c-8C71-5E1E6213545C}")]
	public class NtfsFileSystem : WindowsFileSystem
	{
		public override Guid Guid
		{
			get { return GetType().GUID; }
		}

		public override string Name
		{
			get { return "NTFS"; }
		}

		public override void EraseOldFileSystemResidentFiles(VolumeInfo volume,
			DirectoryInfo tempDirectory, ErasureMethod method,
			FileSystemEntriesEraseProgress callback)
		{
			try
			{
				//Squeeze one-byte files until the volume or the MFT is full.
				long oldMFTSize = NtfsApi.GetMftValidSize(volume);

				for ( ; ; )
				{
					//Open this stream
					using (FileStream strm = new FileStream(
						GenerateRandomFileName(tempDirectory, 18), FileMode.CreateNew,
						FileAccess.Write, FileShare.None, 8, FileOptions.WriteThrough))
					{
						long streamSize = 0;
						try
						{
							while (true)
							{
								//Stretch the file size to use up some of the resident space.
								strm.SetLength(++streamSize);

								//Then run the erase task
								method.Erase(strm, long.MaxValue,
									ManagerLibrary.Instance.PrngRegistrar[ManagerLibrary.Settings.ActivePrng],
									null);

								//Call the callback function if one is provided. We'll provide a dummy
								//value since we really have no idea how much of the MFT we can clean.
								if (callback != null)
									callback(0, 1);
							}
						}
						catch (IOException)
						{
							if (streamSize == 1)
								return;
						}
					}

					//We can stop when the MFT has grown.
					if (NtfsApi.GetMftValidSize(volume) > oldMFTSize)
						break;
				}
			}
			catch (IOException)
			{
				//OK, enough squeezing: there isn't enough space to even create a new MFT record.
			}
		}

		public override void EraseDirectoryStructures(VolumeInfo info,
			FileSystemEntriesEraseProgress callback)
		{
			//Create a directory to hold all the temporary files
			DirectoryInfo tempDir = new DirectoryInfo(FileSystem.GenerateRandomFileName(
				new DirectoryInfo(info.MountPoints[0]), 32));
			tempDir.Create();

			try
			{
				//Get the size of the MFT
				long mftSize = NtfsApi.GetMftValidSize(info);
				long mftRecordSegmentSize = NtfsApi.GetMftRecordSegmentSize(info);
				int pollingInterval = (int)Math.Min(Math.Max(1, mftSize / info.ClusterSize / 20), 128);
				int totalFiles = (int)Math.Max(1L, mftSize / mftRecordSegmentSize);
				int filesCreated = 0;

				while (true)
				{
					++filesCreated;
					using (FileStream strm = new FileStream(FileSystem.GenerateRandomFileName(
						tempDir, 220), FileMode.CreateNew, FileAccess.Write))
					{
					}

					if (filesCreated % pollingInterval == 0)
					{
						//Call back to our progress function: this is the first half of the
						//procedure so divide the effective progress by 2.
						if (callback != null)
						{
							int halfFilesCreated = filesCreated / 2;
							callback(halfFilesCreated, Math.Max(halfFilesCreated, totalFiles));
						}

						//Check if the MFT has grown.
						if (mftSize < NtfsApi.GetMftValidSize(info))
							break;
					}
				}
			}
			catch (IOException)
			{
			}
			finally
			{
				//Clear up all the temporary files
				FileInfo[] files = tempDir.GetFiles("*", SearchOption.AllDirectories);
				for (int i = 0; i < files.Length; ++i)
				{
					if (callback != null && i % 50 == 0)
						callback(files.Length + i, files.Length * 2);
					DeleteFile(files[i]);
				}

				DeleteFolder(tempDir);
			}
		}

		public override void EraseFileSystemObject(StreamInfo info, ErasureMethod method,
			ErasureMethodProgressFunction callback)
		{
			//Check if the file fits in one cluster - if it does it may be MFT resident
			//TODO: any more deterministic way of finding out?
			VolumeInfo volume = VolumeInfo.FromMountPoint(info.DirectoryName);
			if (info.Length < Math.Max(volume.ClusterSize, 1024))
			{
				//Yes it does, erase exactly to the file length
				using (FileStream strm = info.Open(FileMode.Open, FileAccess.Write,
					FileShare.None))
				{
					method.Erase(strm, long.MaxValue,
						ManagerLibrary.Instance.PrngRegistrar[ManagerLibrary.Settings.ActivePrng],
						null);
				}
			}

			//Create the file stream, and call the erasure method to write to
			//the stream.
			long fileArea = GetFileArea(info.FullName);

			//If the stream is empty, there's nothing to overwrite. Continue
			//to the next entry
			if (fileArea == 0)
				return;

			using (FileStream strm = info.Open(FileMode.Open, FileAccess.Write,
				FileShare.None, FileOptions.WriteThrough))
			{
				//Set the end of the stream after the wrap-round the cluster size
				strm.SetLength(fileArea);

				//Then erase the file.
				method.Erase(strm, long.MaxValue,
					ManagerLibrary.Instance.PrngRegistrar[ManagerLibrary.Settings.ActivePrng],
					callback
				);

				//Set the length of the file to 0.
				strm.Seek(0, SeekOrigin.Begin);
				strm.SetLength(0);
			}
		}

		protected override DateTime MinTimestamp
		{
			get
			{
				return new DateTime(1601, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			}
		}
	}
}
