﻿// Copyright 2013 Brian David Patterson <pattersonbriandavid@gmail.com>
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

#endregion

namespace WipeDisk.Hardware
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public class DiskGeometry : CubicAddress
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly UInt32 m_BytesPerCylinder;
        /// <summary>
        /// 
        /// </summary>
        private readonly Int64 m_DiskSize;
        /// <summary>
        /// 
        /// </summary>
        private readonly CubicAddress m_MaximumCubicAddress;
        /// <summary>
        /// 
        /// </summary>
        private readonly long m_MaximumLinearAddress;
        /// <summary>
        /// 
        /// </summary>
        private Win32.DISK_GEOMETRY m_Geometry;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiskGeometry"/> class.
        /// </summary>
        /// <param name="deviceName">Name of the device.</param>
        /// <remarks></remarks>
        private DiskGeometry(String deviceName)
        {
            var x = new Win32.DISK_GEOMETRY_EX();
            InputOutputControlExtension.Execute(ref x, InputOutputControlExtension.DISK_GET_DRIVE_GEOMETRY_EX,
                                                deviceName);
            m_DiskSize = x.DiskSize;
            m_Geometry = x.Geometry;

            long remainder;
            m_MaximumLinearAddress = Math.DivRem(DiskSize, BytesPerSector, out remainder) - 1;
            ThrowIfDiskSizeOutOfIntegrity(remainder);

            m_BytesPerCylinder = BytesPerSector*Sector*Head;
            m_MaximumCubicAddress = Transform(m_MaximumLinearAddress, this);
        }

        /// <summary>
        /// Gets the type of the media.
        /// </summary>
        /// <remarks></remarks>
        public MEDIA_TYPE MediaType
        {
            get { return m_Geometry.MediaType; }
        }

        /// <summary>
        /// Gets the name of the media type.
        /// </summary>
        /// <remarks></remarks>
        public String MediaTypeName
        {
            get { return Enum.GetName(typeof (MEDIA_TYPE), MediaType); }
        }

        /// <summary>
        /// Gets or sets the cylinder.
        /// </summary>
        /// <value>The cylinder.</value>
        /// <remarks></remarks>
        public override long Cylinder
        {
            get { return m_Geometry.Cylinders; }
        }

        /// <summary>
        /// Gets or sets the head.
        /// </summary>
        /// <value>The head.</value>
        /// <remarks></remarks>
        public override uint Head
        {
            get { return m_Geometry.TracksPerCylinder; }
        }

        /// <summary>
        /// Gets or sets the sector.
        /// </summary>
        /// <value>The sector.</value>
        /// <remarks></remarks>
        public override uint Sector
        {
            get { return m_Geometry.SectorsPerTrack; }
        }

        /// <summary>
        /// Gets the bytes per sector.
        /// </summary>
        /// <remarks></remarks>
        public UInt32 BytesPerSector
        {
            get { return m_Geometry.BytesPerSector; }
        }

        /// <summary>
        /// Gets the size of the disk.
        /// </summary>
        /// <remarks></remarks>
        public long DiskSize
        {
            get { return m_DiskSize; }
        }

        /// <summary>
        /// Gets the maximum linear address.
        /// </summary>
        /// <remarks></remarks>
        public long MaximumLinearAddress
        {
            get { return m_MaximumLinearAddress; }
        }

        /// <summary>
        /// Gets the maximum cubic address.
        /// </summary>
        /// <remarks></remarks>
        public CubicAddress MaximumCubicAddress
        {
            get { return m_MaximumCubicAddress; }
        }

        /// <summary>
        /// Gets the bytes per cylinder.
        /// </summary>
        /// <remarks></remarks>
        public UInt32 BytesPerCylinder
        {
            get { return m_BytesPerCylinder; }
        }

        /// <summary>
        /// Throws if disk size out of integrity.
        /// </summary>
        /// <param name="remainder">The remainder.</param>
        /// <remarks></remarks>
        internal static void ThrowIfDiskSizeOutOfIntegrity(long remainder)
        {
            if (0 != remainder)
            {
                string message = "DiskSize is not an integral multiple of a sector size";
                throw new ArithmeticException(message);
            }
        }

        /// <summary>
        /// Froms the device.
        /// </summary>
        /// <param name="deviceName">Name of the device.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DiskGeometry FromDevice(String deviceName)
        {
            return new DiskGeometry(deviceName);
        }
    }
}