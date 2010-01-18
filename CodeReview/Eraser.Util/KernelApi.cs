/* 
 * $Id$
 * Copyright 2008-2009 The Eraser Project
 * Original Author: Joel Low <lowjoel@users.sourceforge.net>
 * Modified By: Garrett Trant <gtrant@users.sourceforge.net>
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
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.ComponentModel;

namespace Eraser.Util
{
	public static class KernelApi
	{
		/// <summary>
		/// Allocates a new console for the calling process.
		/// </summary>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// 
		/// If the function fails, the return value is zero. To get extended error
		/// information, call Marshal.GetLastWin32Error.</returns>
		/// <remarks>A process can be associated with only one console, so the AllocConsole
		/// function fails if the calling process already has a console. A process can
		/// use the FreeConsole function to detach itself from its current console, then
		/// it can call AllocConsole to create a new console or AttachConsole to attach
		/// to another console.
		/// 
		/// If the calling process creates a child process, the child inherits the
		/// new console.
		/// 
		/// AllocConsole initializes standard input, standard output, and standard error
		/// handles for the new console. The standard input handle is a handle to the
		/// console's input buffer, and the standard output and standard error handles
		/// are handles to the console's screen buffer. To retrieve these handles, use
		/// the GetStdHandle function.
		/// 
		/// This function is primarily used by graphical user interface (GUI) application
		/// to create a console window. GUI applications are initialized without a
		/// console. Console applications are initialized with a console, unless they
		/// are created as detached processes (by calling the CreateProcess function
		/// with the DETACHED_PROCESS flag).</remarks>
		[Obsolete]
		public static bool AllocConsole()
		{
			return NativeMethods.AllocConsole();
		}

		/// <summary>
		/// Detaches the calling process from its console.
		/// </summary>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// 
		/// If the function fails, the return value is zero. To get extended error
		/// information, call Marshal.GetLastWin32Error.</returns>
		/// <remarks>A process can be attached to at most one console. If the calling
		/// process is not already attached to a console, the error code returned is
		/// ERROR_INVALID_PARAMETER (87).
		/// 
		/// A process can use the FreeConsole function to detach itself from its
		/// console. If other processes share the console, the console is not destroyed,
		/// but the process that called FreeConsole cannot refer to it. A console is
		/// closed when the last process attached to it terminates or calls FreeConsole.
		/// After a process calls FreeConsole, it can call the AllocConsole function to
		/// create a new console or AttachConsole to attach to another console.</remarks>
		[Obsolete]
		public static bool FreeConsole()
		{
			return NativeMethods.FreeConsole();
		}

		/// <summary>
		/// Converts a Win32 Error code to a HRESULT.
		/// </summary>
		/// <param name="errorCode">The error code to convert.</param>
		/// <returns>A HRESULT value representing the error code.</returns>
		internal static int GetHRForWin32Error(int errorCode)
		{
			const uint FACILITY_WIN32 = 7;
			return errorCode <= 0 ? errorCode :
				(int)((((uint)errorCode) & 0x0000FFFF) | (FACILITY_WIN32 << 16) | 0x80000000);
		}

		/// <summary>
		/// Gets a Exception for the given Win32 error code.
		/// </summary>
		/// <param name="errorCode">The error code.</param>
		/// <returns>An exception object representing the error code.</returns>
		internal static Exception GetExceptionForWin32Error(int errorCode)
		{
			int HR = GetHRForWin32Error(errorCode);
			return Marshal.GetExceptionForHR(HR);
		}

		/// <summary>
		/// Retrieves the current value of the high-resolution performance counter.
		/// </summary>
		public static long PerformanceCounter
		{
			get
			{
				long result = 0;
				if (NativeMethods.QueryPerformanceCounter(out result))
					return result;
				return 0;
			}
		}

		/// <summary>
		/// Gets the current CPU type of the system.
		/// </summary>
		/// <returns>One of the <see cref="ProcessorTypes"/> enumeration values.</returns>
		public static ProcessorArchitecture ProcessorArchitecture
		{
			get
			{
				NativeMethods.SYSTEM_INFO info = new NativeMethods.SYSTEM_INFO();
				NativeMethods.GetSystemInfo(out info);

				switch (info.processorArchitecture)
				{
					case NativeMethods.SYSTEM_INFO.ProcessorArchitecture.PROCESSOR_ARCHITECTURE_AMD64:
						return ProcessorArchitecture.Amd64;
					case NativeMethods.SYSTEM_INFO.ProcessorArchitecture.PROCESSOR_ARCHITECTURE_IA64:
						return ProcessorArchitecture.IA64;
					case NativeMethods.SYSTEM_INFO.ProcessorArchitecture.PROCESSOR_ARCHITECTURE_INTEL:
						return ProcessorArchitecture.X86;
					default:
						return ProcessorArchitecture.None;
				}
			}
		}

		public class DiskPerformanceInfo
		{
			unsafe internal DiskPerformanceInfo(NativeMethods.DiskPerformanceInfoInternal info)
			{
				BytesRead = info.BytesRead;
				BytesWritten = info.BytesWritten;
				ReadTime = info.ReadTime;
				WriteTime = info.WriteTime;
				IdleTime = info.IdleTime;
				ReadCount = info.ReadCount;
				WriteCount = info.WriteCount;
				QueueDepth = info.QueueDepth;
				SplitCount = info.SplitCount;
				QueryTime = info.QueryTime;
				StorageDeviceNumber = info.StorageDeviceNumber;
				StorageManagerName = new string((char*)info.StorageManagerName);
			}

			public long BytesRead { get; private set; }
			public long BytesWritten { get; private set; }
			public long ReadTime { get; private set; }
			public long WriteTime { get; private set; }
			public long IdleTime { get; private set; }
			public uint ReadCount { get; private set; }
			public uint WriteCount { get; private set; }
			public uint QueueDepth { get; private set; }
			public uint SplitCount { get; private set; }
			public long QueryTime { get; private set; }
			public uint StorageDeviceNumber { get; private set; }
			public string StorageManagerName { get; private set; }
		}

		/// <summary>
		/// Queries the performance information for the given disk.
		/// </summary>
		/// <param name="diskHandle">A read-only handle to a device (disk).</param>
		/// <returns>A DiskPerformanceInfo structure describing the performance
		/// information for the given disk.</returns>
		public static DiskPerformanceInfo QueryDiskPerformanceInfo(SafeFileHandle diskHandle)
		{
			if (diskHandle.IsInvalid)
				throw new ArgumentException("The disk handle must not be invalid.");

			//This only works if the user has turned on the disk performance
			//counters with 'diskperf -y'. These counters are off by default
			NativeMethods.DiskPerformanceInfoInternal result =
				new NativeMethods.DiskPerformanceInfoInternal();
			uint bytesReturned = 0;
			if (NativeMethods.DeviceIoControl(diskHandle, NativeMethods.IOCTL_DISK_PERFORMANCE,
				IntPtr.Zero, 0, out result, (uint)Marshal.SizeOf(result), out bytesReturned, IntPtr.Zero))
			{
				return new DiskPerformanceInfo(result);
			}

			return null;
		}
	}
}