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

using System.Text;
using WipeDisk.Hardware;

#endregion

namespace WipeDisk
{
    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    internal class PhysicalDrive
    {
        /// <summary>
        /// 	Gets or sets the device ID.
        /// </summary>
        /// <value>The device ID.</value>
        /// <remarks>
        /// </remarks>
        public string DeviceID { get; set; }

        /// <summary>
        /// 	Gets or sets the size of the device.
        /// </summary>
        /// <value>The size of the device.</value>
        /// <remarks>
        /// </remarks>
        public string DeviceSize { get; set; }

        /// <summary>
        /// 	Gets or sets the name of the device.
        /// </summary>
        /// <value>The name of the device.</value>
        /// <remarks>
        /// </remarks>
        public string DeviceName { get; set; }

        /// <summary>
        /// 	Gets or sets the logical drives.
        /// </summary>
        /// <value>The logical drives.</value>
        /// <remarks>
        /// </remarks>
        public string LogicalDrives { get; set; }

        /// <summary>
        /// 	Gets or sets the geometry.
        /// </summary>
        /// <value>The geometry.</value>
        /// <remarks>
        /// </remarks>
        public DiskGeometry Geometry { get; set; }

        /// <summary>
        /// 	Gets or sets the cubic address detail.
        /// </summary>
        /// <value>The cubic address detail.</value>
        /// <remarks>
        /// </remarks>
        public CubicAddress CubicAddressDetail { get; set; }

        /// <summary>
        /// 	Returns a <see cref = "System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref = "System.String" /> that represents this instance.</returns>
        /// <remarks>
        /// </remarks>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(DeviceID + " (" + DeviceName + " - " + DeviceSize + ")");
            sb.AppendLine("   Logical Drives: " + LogicalDrives);
            return sb.ToString();
        }
    }
}