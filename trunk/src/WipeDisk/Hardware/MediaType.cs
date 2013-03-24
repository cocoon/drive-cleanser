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

namespace WipeDisk.Hardware
{
    /// <summary>
    /// 	An enumeration representing the various types of media
    /// </summary>
    /// <remarks>
    /// 	See http://msdn.microsoft.com/en-us/library/windows/desktop/aa365231%28v=vs.85%29.aspx for more information.
    /// </remarks>
    public enum MEDIA_TYPE
    {
        /// <summary>
        /// 	Format is unknown
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// 	A 5.25" floppy, with 1.2MB and 512 bytes/sector.
        /// </summary>
        F5_1Pt2_512 = 1,

        /// <summary>
        /// 	A 3.5" floppy, with 1.44MB and 512 bytes/sector.
        /// </summary>
        F3_1Pt44_512 = 2,

        /// <summary>
        /// 	A 3.5" floppy, with 2.88MB and 512 bytes/sector.
        /// </summary>
        F3_2Pt88_512 = 3,

        /// <summary>
        /// 	A 3.5" floppy, with 20.8MB and 512 bytes/sector.
        /// </summary>
        F3_20Pt8_512 = 4,

        /// <summary>
        /// 	A 3.5" floppy, with 720KB and 512 bytes/sector.
        /// </summary>
        F3_720_512 = 5,

        /// <summary>
        /// 	A 5.25" floppy, with 360KB and 512 bytes/sector.
        /// </summary>
        F5_360_512 = 6,

        /// <summary>
        /// 	A 5.25" floppy, with 320KB and 512 bytes/sector.
        /// </summary>
        F5_320_512 = 7,

        ///<summary>
        ///	A 5.25" floppy, with 320KB and 1024 bytes/sector.
        ///</summary>
        F5_320_1024 = 8,

        /// <summary>
        /// 	A 5.25" floppy, with 180KB and 512 bytes/sector.
        /// </summary>
        F5_180_512 = 9,

        /// <summary>
        /// 	A 5.25" floppy, with 160KB and 512 bytes/sector.
        /// </summary>
        F5_160_512 = 10,

        /// <summary>
        /// 	Removable media other than floppy.
        /// </summary>
        RemovableMedia = 11,

        /// <summary>
        /// 	Fixed hard disk media.
        /// </summary>
        FixedMedia = 12,

        /// <summary>
        /// 	A 3.5" floppy, with 120MB and 512 bytes/sector.
        /// </summary>
        F3_120M_512 = 13,

        /// <summary>
        /// 	A 3.5" floppy, with 640KB and 512 bytes/sector.
        /// </summary>
        F3_640_512 = 14,

        /// <summary>
        /// 	A 5.25" floppy, with 640KB and 512 bytes/sector.
        /// </summary>
        F5_640_512 = 15,

        /// <summary>
        /// 	A 5.25" floppy, with 720KB and 512 bytes/sector.
        /// </summary>
        F5_720_512 = 16,

        /// <summary>
        /// 	A 3.5" floppy, with 1.2MB and 512 bytes/sector.
        /// </summary>
        F3_1Pt2_512 = 17,

        /// <summary>
        /// 	A 3.5" floppy, with 1.23MB and 1024 bytes/sector.
        /// </summary>
        F3_1Pt23_1024 = 18,

        /// <summary>
        /// 	A 5.25" floppy, with 1.23MB and 1024 bytes/sector.
        /// </summary>
        F5_1Pt23_1024 = 19,

        /// <summary>
        /// 	A 3.5" floppy, with 128MB and 512 bytes/sector.
        /// </summary>
        F3_128Mb_512 = 20,

        /// <summary>
        /// 	A 3.5" floppy, with 230MB and 512 bytes/sector.
        /// </summary>
        F3_230Mb_512 = 21,

        /// <summary>
        /// 	An 8" floppy, with 256KB and 128 bytes/sector.
        /// </summary>
        F8_256_128 = 22,

        /// <summary>
        /// 	A 3.5" floppy, with 200MB and 512 bytes/sector. (HiFD).
        /// </summary>
        F3_200Mb_512 = 23,

        /// <summary>
        /// 	A 3.5" floppy, with 240MB and 512 bytes/sector. (HiFD).
        /// </summary>
        F3_240M_512 = 24,

        /// <summary>
        /// 	A 3.5" floppy, with 32MB and 512 bytes/sector.
        /// </summary>
        F3_32M_512 = 25
    }
}