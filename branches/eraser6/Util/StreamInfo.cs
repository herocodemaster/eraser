/* 
 * $Id$
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

using Microsoft.Win32.SafeHandles;
using System.IO;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Eraser.Util
{
	public class StreamInfo
	{
		/// <summary>
		/// Initializes a new instance of the Eraser.Util.StreamInfo class, which
		/// acts as a wrapper for a file path.
		/// </summary>
		/// <param name="path">The fully qualified name (with :ADSName for ADSes)
		/// of the new file, or the relative file name.</param>
		public StreamInfo(string path)
		{
			//Separate the path into the ADS and the file.
			if (path.IndexOf(':') != path.LastIndexOf(':'))
			{
				int streamNameColon = path.IndexOf(':', path.IndexOf(':') + 1);
				fileName = path.Substring(0, streamNameColon);
				streamName = path.Substring(streamNameColon + 1);
			}
			else
			{
				fileName = path;
			}
		}

		/// <summary>
		/// Gets an instance of the parent directory.
		/// </summary>
		public DirectoryInfo Directory
		{
			get
			{
				return new DirectoryInfo(DirectoryName);
			}
		}

		/// <summary>
		/// Gets a string representing the containing directory's full path.
		/// </summary>
		public string DirectoryName
		{
			get
			{
				return fileName.Substring(0, fileName.LastIndexOf(Path.DirectorySeparatorChar) + 1);
			}
		}

		/// <summary>
		/// Gets the full name of the file, including the stream name.
		/// </summary>
		public string FullName
		{
			get
			{
				if (streamName != null)
					return fileName + ':' + streamName;
				return fileName;
			}
		}

		/// <summary>
		/// Gets the name of the file.
		/// </summary>
		public string Name
		{
			get { return fileName; }
		}

		/// <summary>
		/// Gets an instance of the main file. If this object refers to an ADS, the
		/// result is null.
		/// </summary>
		public FileInfo File
		{
			get
			{
				if (streamName == null)
					return new FileInfo(fileName);
				return null;
			}
		}

		/// <summary>
		/// Gets or sets the file attributes on this stream.
		/// </summary>
		public FileAttributes Attributes
		{
			get { return System.IO.File.GetAttributes(fileName); }
			set { System.IO.File.SetAttributes(fileName, value); }
		}
		
		/// <summary>
		/// Gets a value indicating whether the stream exists.
		/// </summary>
		public bool Exists
		{
			get
			{
				using (SafeFileHandle handle = fileHandle)
					return !(Marshal.GetLastWin32Error() == 2 /*ERROR_FILE_NOT_FOUND*/ &&
						handle.IsInvalid);
			}
		}

		/// <summary>
		/// Gets or sets a value that determines if the current file is read only.
		/// </summary>
		public bool IsReadOnly
		{
			get
			{
				return (Attributes & FileAttributes.ReadOnly) != 0;
			}

			set
			{
				if (value)
					Attributes |= FileAttributes.ReadOnly;
				else
					Attributes &= ~FileAttributes.ReadOnly;
			}
		}

		/// <summary>
		/// Gets the size of the current stream.
		/// </summary>
		public long Length
		{
			get
			{
				long fileSize;
				using (SafeFileHandle handle = fileHandle)
					if (KernelApi.NativeMethods.GetFileSizeEx(handle, out fileSize))
						return fileSize;

				return 0;
			}
		}

		public DateTime LastAccessTime
		{
			get
			{
				DateTime creationTime, lastAccess, lastWrite;
				using (SafeFileHandle handle = fileHandle)
					KernelApi.GetFileTime(handle, out creationTime, out lastAccess,
						out lastWrite);
				return lastAccess;
			}
			set
			{
				using (SafeFileHandle handle = fileHandle)
					KernelApi.SetFileTime(handle, DateTime.MinValue, value, DateTime.MinValue);
			}
		}

		public DateTime LastWriteTime
		{
			get
			{
				DateTime creationTime, lastAccess, lastWrite;
				using (SafeFileHandle handle = fileHandle)
					KernelApi.GetFileTime(handle, out creationTime, out lastAccess,
						out lastWrite);
				return lastWrite;
			}
			set
			{
				using (SafeFileHandle handle = fileHandle)
					KernelApi.SetFileTime(handle, DateTime.MinValue, DateTime.MinValue, value);
			}
		}

		public DateTime CreationTime
		{
			get
			{
				DateTime creationTime, lastAccess, lastWrite;
				using (SafeFileHandle handle = fileHandle)
					KernelApi.GetFileTime(handle, out creationTime, out lastAccess,
						out lastWrite);
				return creationTime;
			}
			set
			{
				using (SafeFileHandle handle = fileHandle)
					KernelApi.SetFileTime(handle, value, DateTime.MinValue, DateTime.MinValue);
			}
		}

		/// <summary>
		/// Permanently deletes a file.
		/// </summary>
		public void Delete()
		{
			if (streamName == null)
				File.Delete();
			else
				throw new InvalidOperationException("Deleting streams are not implemented");
		}

		/// <summary>
		/// Opens a file in the specified mode.
		/// </summary>
		/// <param name="mode">A System.IO.FileMode constant specifying the mode
		/// (for example, Open or Append) in which to open the file.</param>
		/// <returns>A file opened in the specified mode, with read/write access,
		/// unshared, and no special file options.</returns>
		public FileStream Open(FileMode mode)
		{
			return Open(mode, FileAccess.ReadWrite, FileShare.None, FileOptions.None);
		}

		/// <summary>
		/// Opens a file in the specified mode with read, write, or read/write access.
		/// </summary>
		/// <param name="mode">A System.IO.FileMode constant specifying the mode
		/// (for example, Open or Append) in which to open the file.</param>
		/// <param name="access">A System.IO.FileAccess constant specifying whether
		/// to open the file with Read, Write, or ReadWrite file access.</param>
		/// <returns>A System.IO.FileStream object opened in the specified mode
		/// and access, unshared, and no special file options.</returns>
		public FileStream Open(FileMode mode, FileAccess access)
		{
			return Open(mode, access, FileShare.None, FileOptions.None);
		}

		/// <summary>
		/// Opens a file in the specified mode with read, write, or read/write access
		/// and the specified sharing option.
		/// </summary>
		/// <param name="mode">A System.IO.FileMode constant specifying the mode
		/// (for example, Open or Append) in which to open the file.</param>
		/// <param name="access">A System.IO.FileAccess constant specifying whether
		/// to open the file with Read, Write, or ReadWrite file access.</param>
		/// <param name="share">A System.IO.FileShare constant specifying the type
		/// of access other FileStream objects have to this file.</param>
		/// <returns>A System.IO.FileStream object opened with the specified mode,
		/// access, sharing options, and no special file options.</returns>
		public FileStream Open(FileMode mode, FileAccess access, FileShare share)
		{
			return Open(mode, access, share, FileOptions.None);
		}

		/// <summary>
		/// Opens a file in the specified mode with read, write, or read/write access,
		/// the specified sharing option, and other advanced options.
		/// </summary>
		/// <param name="mode">A System.IO.FileMode constant specifying the mode
		/// (for example, Open or Append) in which to open the file.</param>
		/// <param name="access">A System.IO.FileAccess constant specifying whether
		/// to open the file with Read, Write, or ReadWrite file access.</param>
		/// <param name="share">A System.IO.FileShare constant specifying the type
		/// of access other FileStream objects have to this file.</param>
		/// <param name="options">The System.IO.FileOptions constant specifying
		/// the advanced file options to use when opening the file.</param>
		/// <returns>A System.IO.FileStream object opened with the specified mode,
		/// access, sharing options, and special file options.</returns>
		public FileStream Open(FileMode mode, FileAccess access, FileShare share,
			FileOptions options)
		{
			SafeFileHandle handle = OpenHandle(mode, access, share, options);

			//Check that the handle is valid
			if (handle.IsInvalid)
				throw Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());

			//Return the FileStream
			return new FileStream(handle, access);
		}

		private SafeFileHandle OpenHandle(FileMode mode, FileAccess access, FileShare share,
			FileOptions options)
		{
			//Access mode
			uint iAccess = 0;
			switch (access)
			{
				case FileAccess.Read:
					iAccess = KernelApi.NativeMethods.GENERIC_READ;
					break;
				case FileAccess.ReadWrite:
					iAccess = KernelApi.NativeMethods.GENERIC_READ |
						KernelApi.NativeMethods.GENERIC_WRITE;
					break;
				case FileAccess.Write:
					iAccess = KernelApi.NativeMethods.GENERIC_WRITE;
					break;
			}

			//Sharing mode
			if ((share & FileShare.Inheritable) != 0)
				throw new NotSupportedException("Inheritable handles are not supported.");

			//Advanced options
			if ((options & FileOptions.Asynchronous) != 0)
				throw new NotSupportedException("Asynchronous handles are not implemented.");
			
			//Create the handle
			SafeFileHandle result = KernelApi.NativeMethods.CreateFile(FullName, iAccess,
				(uint)share, IntPtr.Zero, (uint)mode, (uint)options, IntPtr.Zero);
			if (result.IsInvalid)
				throw new Win32Exception(Marshal.GetLastWin32Error());
			return result;
		}

		/// <summary>
		/// Returns the path as a string.
		/// </summary>
		/// <returns>A string representing the path.</returns>
		public override string ToString()
		{
			return FullName;
		}

		/// <summary>
		/// Retrieves a file handle with read access and with all sharing enabled.
		/// </summary>
		private SafeFileHandle fileHandle
		{
			get
			{
				return OpenHandle(FileMode.Open, FileAccess.Read, FileShare.ReadWrite |
					FileShare.Delete, FileOptions.None);
			}
		}

		private string fileName;
		private string streamName;
	}
}
