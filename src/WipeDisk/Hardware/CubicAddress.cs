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
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public class CubicAddress
    {
        /// <summary>
        /// Gets or sets the cylinder.
        /// </summary>
        /// <value>The cylinder.</value>
        /// <remarks></remarks>
        public virtual long Cylinder { get; set; }

        /// <summary>
        /// Gets or sets the head.
        /// </summary>
        /// <value>The head.</value>
        /// <remarks></remarks>
        public virtual uint Head { get; set; }

        /// <summary>
        /// Gets or sets the sector.
        /// </summary>
        /// <value>The sector.</value>
        /// <remarks></remarks>
        public virtual uint Sector { get; set; }

        /// <summary>
        /// Transforms the specified linear address.
        /// </summary>
        /// <param name="linearAddress">The linear address.</param>
        /// <param name="geometry">The geometry.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static CubicAddress Transform(long linearAddress, CubicAddress geometry)
        {
            var cubicAddress = new CubicAddress();
            uint sectorsPerCylinder = geometry.Sector*geometry.Head;
            long remainder;
            cubicAddress.Cylinder = Math.DivRem(linearAddress, sectorsPerCylinder, out remainder);
            cubicAddress.Head = (uint) Math.DivRem(remainder, geometry.Sector, out remainder);
            cubicAddress.Sector = 1 + (uint) remainder;
            return cubicAddress;
        }
    }
}