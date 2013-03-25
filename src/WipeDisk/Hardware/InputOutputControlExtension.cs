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
using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

#endregion

namespace WipeDisk.Hardware
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public static class InputOutputControlExtension
    {
        /// <summary>
        /// 
        /// </summary>
        public const UInt32
            DISK_BASE = 0x00000007,
            METHOD_BUFFERED = 0,
            FILE_ANY_ACCESS = 0;


        /// <summary>
        /// 
        /// </summary>
        public static readonly UInt32 DISK_GET_DRIVE_GEOMETRY_EX =
            CTL_CODE(DISK_BASE, 0x0028, METHOD_BUFFERED, FILE_ANY_ACCESS);

        /// <summary>
        /// 
        /// </summary>
        public static readonly UInt32 DISK_GET_DRIVE_GEOMETRY =
            CTL_CODE(DISK_BASE, 0, METHOD_BUFFERED, FILE_ANY_ACCESS);


        /// <summary>
        /// CTs the l_ CODE.
        /// </summary>
        /// <param name="DeviceType">Type of the device.</param>
        /// <param name="Function">The function.</param>
        /// <param name="Method">The method.</param>
        /// <param name="Access">The access.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static UInt32 CTL_CODE(UInt32 DeviceType, UInt32 Function, UInt32 Method, UInt32 Access)
        {
            return (((DeviceType) << 16) | ((Access) << 14) | ((Function) << 2) | (Method));
        }

        /// <summary>
        /// Executes the specified x.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x">The x.</param>
        /// <param name="dwIoControlCode">The dw io control code.</param>
        /// <param name="lpFileName">Name of the lp file.</param>
        /// <param name="dwDesiredAccess">The dw desired access.</param>
        /// <param name="dwShareMode">The dw share mode.</param>
        /// <param name="lpSecurityAttributes">The lp security attributes.</param>
        /// <param name="dwCreationDisposition">The dw creation disposition.</param>
        /// <param name="dwFlagsAndAttributes">The dw flags and attributes.</param>
        /// <param name="hTemplateFile">The h template file.</param>
        /// <remarks></remarks>
        public static void Execute<T>(
            ref T x,
            UInt32 dwIoControlCode,
            String lpFileName,
            UInt32 dwDesiredAccess = Win32.GENERIC_READ,
            UInt32 dwShareMode = Win32.FILE_SHARE_WRITE | Win32.FILE_SHARE_READ,
            IntPtr lpSecurityAttributes = default(IntPtr),
            UInt32 dwCreationDisposition = Win32.OPEN_EXISTING,
            UInt32 dwFlagsAndAttributes = 0,
            IntPtr hTemplateFile = default(IntPtr)
            )
        {
            using (
                SafeFileHandle hDevice =
                    Win32.CreateFile(
                        lpFileName,
                        dwDesiredAccess, dwShareMode,
                        lpSecurityAttributes,
                        dwCreationDisposition, dwFlagsAndAttributes,
                        hTemplateFile
                        )
                )
            {
                if (null == hDevice || hDevice.IsInvalid)
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                int nOutBufferSize = Marshal.SizeOf(typeof (T));
                IntPtr lpOutBuffer = Marshal.AllocHGlobal(nOutBufferSize);
                uint lpBytesReturned = default(UInt32);
                IntPtr NULL = IntPtr.Zero;

                uint result =
                    Win32.DeviceIoControl(
                        hDevice, dwIoControlCode,
                        NULL, 0,
                        lpOutBuffer, nOutBufferSize,
                        ref lpBytesReturned, NULL
                        );

                if (0 == result)
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                x = (T) Marshal.PtrToStructure(lpOutBuffer, typeof (T));
                Marshal.FreeHGlobal(lpOutBuffer);
            }
        }
    }
}