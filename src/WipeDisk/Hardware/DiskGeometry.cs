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

#endregion

namespace WipeDisk.Hardware
{
    public class DiskGeometry : CubicAddress
    {
        private readonly UInt32 m_BytesPerCylinder;
        private readonly Int64 m_DiskSize;
        private readonly CubicAddress m_MaximumCubicAddress;
        private readonly long m_MaximumLinearAddress;
        private Win32.DISK_GEOMETRY m_Geometry;

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

        public MEDIA_TYPE MediaType
        {
            get { return m_Geometry.MediaType; }
        }

        public String MediaTypeName
        {
            get { return Enum.GetName(typeof (MEDIA_TYPE), MediaType); }
        }

        public override long Cylinder
        {
            get { return m_Geometry.Cylinders; }
        }

        public override uint Head
        {
            get { return m_Geometry.TracksPerCylinder; }
        }

        public override uint Sector
        {
            get { return m_Geometry.SectorsPerTrack; }
        }

        public UInt32 BytesPerSector
        {
            get { return m_Geometry.BytesPerSector; }
        }

        public long DiskSize
        {
            get { return m_DiskSize; }
        }

        public long MaximumLinearAddress
        {
            get { return m_MaximumLinearAddress; }
        }

        public CubicAddress MaximumCubicAddress
        {
            get { return m_MaximumCubicAddress; }
        }

        public UInt32 BytesPerCylinder
        {
            get { return m_BytesPerCylinder; }
        }

        internal static void ThrowIfDiskSizeOutOfIntegrity(long remainder)
        {
            if (0 != remainder)
            {
                string message = "DiskSize is not an integral multiple of a sector size";
                throw new ArithmeticException(message);
            }
        }

        public static DiskGeometry FromDevice(String deviceName)
        {
            return new DiskGeometry(deviceName);
        }
    }
}