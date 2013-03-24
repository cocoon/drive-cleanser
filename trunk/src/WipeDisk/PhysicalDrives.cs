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

using System.Collections.Generic;
using System.Management;
using WipeDisk.Hardware;

#endregion

namespace WipeDisk
{
    /// <summary>
    /// 	Maintains a collection of all physical drives currently detected in the system.
    /// </summary>
    /// <remarks>
    /// </remarks>
    internal class PhysicalDrives : List<PhysicalDrive>
    {
        /// <summary>
        /// 	Initializes a new instance of the <see cref = "PhysicalDrives" /> class.
        /// </summary>
        /// <remarks>
        /// </remarks>
        internal PhysicalDrives()
        {
            // Load all physical drive information in to this collection.
            QueryPhysicalDrives();
        }

        /// <summary>
        /// 	Obtains a list of logical drives from physical drives.
        /// </summary>
        /// <remarks>
        /// </remarks>
        private void QueryPhysicalDrives()
        {
            var searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_DiskDrive");
            ManagementObjectCollection driveMoc = searcher.Get();
            foreach (ManagementBaseObject v in driveMoc)
            {
                var pd = new PhysicalDrive();
                var totalSize = (ulong) v.Properties["Size"].Value;
                string driveName = v.Properties["Caption"].Value.ToString();
                pd.DeviceID = v["DeviceID"].ToString();
                pd.DeviceName = driveName;
                pd.DeviceSize = StringHelper.BytesToString(totalSize);
                DiskGeometry diskGeometry = DiskGeometry.FromDevice(pd.DeviceID);
                CubicAddress cubicAddress = diskGeometry.MaximumCubicAddress;
                pd.Geometry = diskGeometry;
                pd.CubicAddressDetail = cubicAddress;

                List<string> allDevices = WMIWrapper.GetDevices(v["DeviceID"].ToString());
                foreach (string s in allDevices)
                {
                    if (!string.IsNullOrEmpty(pd.LogicalDrives))
                        pd.LogicalDrives += ",";
                    pd.LogicalDrives += s;
                }
                Add(pd);
            }
        }
    }
}