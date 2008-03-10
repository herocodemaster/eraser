using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;

namespace Eraser.Util
{
	public static class Drives
	{
		/// <summary>
		/// Determines whether a disk drive is a removable, fixed, CD-ROM, RAM disk,
		/// or network drive.
		/// 
		/// To determine whether a drive is a USB-type drive, call
		/// SetupDiGetDeviceRegistryProperty and specify the SPDRP_REMOVAL_POLICY property.
		/// </summary>
		/// <param name="rootPathName">The root directory for the drive.</param>
		/// <remarks>A trailing backslash is required. If this parameter is NULL, the
		/// function uses the root of the current directory.</remarks>
		/// <returns>The return value specifies the type of drive, which can be one
		/// of the values in the DriveTypes enumeration</returns>
		[DllImport("Kernel32.dll")]
		public static extern DriveTypes GetDriveType(string rootPathName);

		/// <summary>
		/// Retrieves information about the specified disk, including the amount
		/// of free space on the disk.
		/// 
		/// The GetDiskFreeSpace function cannot report volume sizes that are
		/// greater than 2 gigabytes (GB). To ensure that your application works
		/// with large capacity hard drives, use the GetDiskFreeSpaceEx function.
		/// </summary>
		/// <param name="lpRootPathName">The root directory of the disk for which
		/// information is to be returned. If this parameter is NULL, the function
		/// uses the root of the current disk. If this parameter is a UNC name,
		/// it must include a trailing backslash (for example, \\MyServer\MyShare\).
		/// Furthermore, a drive specification must have a trailing backslash
		/// (for example, C:\). The calling application must have FILE_LIST_DIRECTORY
		/// access rights for this directory.</param>
		/// <param name="lpSectorsPerCluster">A pointer to a variable that receives
		/// the number of sectors per cluster.</param>
		/// <param name="lpBytesPerSector">A pointer to a variable that receives
		/// the number of bytes per sector.</param>
		/// <param name="lpNumberOfFreeClusters">A pointer to a variable that
		/// receives the total number of free clusters on the disk that are
		/// available to the user who is associated with the calling thread.
		/// 
		/// If per-user disk quotas are in use, this value may be less than the 
		/// total number of free clusters on the disk.</param>
		/// <param name="lpTotalNumberOfClusters">A pointer to a variable that
		/// receives the total number of clusters on the disk that are available
		/// to the user who is associated with the calling thread.
		/// 
		/// If per-user disk quotas are in use, this value may be less than the
		/// total number of clusters on the disk.</param>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// 
		/// If the function fails, the return value is zero. To get extended
		/// error information, call GetLastError.</returns>
		[DllImport("Kernel32.dll")]
		internal static extern uint GetDiskFreeSpace(
			[MarshalAs(UnmanagedType.LPStr)]string lpRootPathName,
			out UInt32 lpSectorsPerCluster, out UInt32 lpBytesPerSector,
			out UInt32 lpNumberOfFreeClusters, out UInt32 lpTotalNumberOfClusters);

		/// <summary>
		/// Determines the cluster size of the given volume.
		/// </summary>
		/// <param name="driveName">The path to the root of the drive, including
		/// the trailing \</param>
		/// <returns>The size of one cluster, in bytes.</returns>
		public static uint GetDriveClusterSize(string driveName)
		{
			UInt32 clusterSize = 0, sectorSize, freeClusters, totalClusters;
			if (GetDiskFreeSpace(driveName, out clusterSize, out sectorSize, 
				out freeClusters, out totalClusters) != 0)
				return clusterSize * sectorSize;

			throw new Exception("Unknown error calling GetDiskFreeSpace()");
		}
	}

	/// <summary>
	/// The types returned by the GetDriveType function.
	/// </summary>
	public enum DriveTypes
	{
		/// <summary>
		/// The drive type cannot be determined.
		/// </summary>
		DRIVE_UNKNOWN = 0,

		/// <summary>
		/// The root path is invalid; for example, there is no volume is mounted at the path.
		/// </summary>
		DRIVE_NO_ROOT_DIR = 1,

		/// <summary>
		/// The drive has removable media; for example, a floppy drive, thumb drive, or flash card reader.
		/// </summary>
		DRIVE_REMOVABLE = 2,

		/// <summary>
		/// The drive has fixed media; for example, a hard drive or flash drive.
		/// </summary>
		DRIVE_FIXED = 3,

		/// <summary>
		/// The drive is a remote (network) drive.
		/// </summary>
		DRIVE_REMOTE = 4,

		/// <summary>
		/// The drive is a CD-ROM drive.
		/// </summary>
		DRIVE_CDROM = 5,

		/// <summary>
		/// The drive is a RAM disk.
		/// </summary>
		DRIVE_RAMDISK = 6
	}
}
