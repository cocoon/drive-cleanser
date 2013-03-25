// Copyright 2013 Brian David Patterson <pattersonbriandavid@gmail.com>
// 
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
// 
//     You should have received a copy of the GNU General Public License
//     along with this program.  If not, see <http://www.gnu.org/licenses/>

#region

using System;
using System.Collections.Generic;
using System.Management;

#endregion

namespace WipeDisk
{
    /// <summary>
    /// Wraps WMI and provides several methods for querying and returning information from it.
    /// </summary>
    /// <remarks></remarks>
    internal class WMIWrapper
    {
        /// <summary>
        /// Returns all logical drives for a physical drive.
        /// </summary>
        /// <param name="deviceId">The device id.</param>
        /// <returns>String list of drive letters.</returns>
        /// <remarks></remarks>
        public static List<string> GetDevices(string deviceId)
        {
            var driveLetters = new List<string>();
            string queryString = "ASSOCIATORS OF {Win32_DiskDrive.DeviceID='" + deviceId +
                                 "'} WHERE AssocClass = Win32_DiskDriveToDiskPartition";
            var diskSearcher = new ManagementObjectSearcher("root\\CIMV2", queryString);
            ManagementObjectCollection diskMoc = diskSearcher.Get();
            foreach (ManagementObject diskMo in diskMoc)
            {
                queryString = "ASSOCIATORS OF {Win32_DiskPartition.DeviceID='" + diskMo["DeviceID"] +
                              "'} WHERE AssocClass = Win32_LogicalDiskToPartition";
                var driveSearcher = new ManagementObjectSearcher("root\\CIMV2", queryString);

                ManagementObjectCollection driveMoc = driveSearcher.Get();
                foreach (ManagementObject driveMo in driveMoc)
                {
                    driveLetters.Add(driveMo["DeviceID"].ToString());
                }
            }
            return driveLetters;
        }

        /// <summary>
        /// Returns the name of the physical drive that was used to boot the system.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetSystemDrive()
        {
            // Get the logical drive letter using environment variables. 
            string systemLogicalDiskDeviceId = Environment.GetFolderPath(Environment.SpecialFolder.System).Substring(0,
                                                                                                                     2);

            using (var diskDriveClass = new ManagementClass("Win32_DiskDrive"))
            {
                // Iterate our physical drives.
                foreach (ManagementObject diskDrive in diskDriveClass.GetInstances())
                {
                    string diskDriveDeviceId = diskDrive["DeviceID"].ToString();

                    // Loop through all partitions and logical disks on the drive
                    foreach (ManagementObject partition in diskDrive.GetRelated("Win32_DiskPartition"))
                        foreach (ManagementObject logicalDisk in partition.GetRelated("Win32_LogicalDisk"))
                        {
                            string logicalDiskDeviceId = logicalDisk["DeviceID"].ToString();

                            // See if this one matches the drive letter obtained from the environment var.
                            if (
                                String.Compare(systemLogicalDiskDeviceId, logicalDiskDeviceId,
                                               StringComparison.OrdinalIgnoreCase) == 0)
                                return diskDriveDeviceId;
                        }
                }
            }
            // Our attempts have failed - return a NULL as an indication to our failure.
            return null;
        }

        /// <summary>
        /// Formats the specified drive
        /// </summary>
        /// <param name="driveLetter">The drive letter to format</param>
        /// <param name="fileSystem">The file system to format the drive as (Default = NTFS)</param>
        /// <param name="quickFormat"><c>True</c> to perform a quick format, otherwise <c>False</c></param>
        /// <param name="clusterSize">The cluster size to use (Default = 8192)</param>
        /// <param name="label">The label to use on the hard drive. (Default = HDD)</param>
        /// <param name="enableCompression"><c>True</c> to enable compression on this drive. (Default = false)</param>
        /// <returns><c>True</c> if the format completes without error, otherwise <c>False</c> is returned.</returns>
        /// <remarks></remarks>
        public static bool FormatDrive(string driveLetter,
                                      string fileSystem = "NTFS", bool quickFormat = true,
                                      int clusterSize = 8192, string label = "HDD", bool enableCompression = false)
        {
            if (driveLetter.Length != 2 || driveLetter[1] != ':' || !char.IsLetter(driveLetter[0]))
                return false;

            //query and format given drive         
            var searcher = new ManagementObjectSearcher
                (@"select * from Win32_Volume WHERE DriveLetter = '" + driveLetter + "'");
            foreach (ManagementObject vi in searcher.Get())
            {
                vi.InvokeMethod("Format", new object[] { fileSystem, quickFormat, clusterSize, label, enableCompression });
            }

            return true;
        }
    }
}