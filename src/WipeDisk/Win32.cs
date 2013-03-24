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
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;
using WipeDisk.Hardware;

#endregion

namespace WipeDisk
{
    #region

    using LPSECURITY_ATTRIBUTES = IntPtr;
    using LPOVERLAPPED = IntPtr;
    using LPVOID = IntPtr;
    using HANDLE = IntPtr;
    using LARGE_INTEGER = Int64;
    using DWORD = UInt32;
    using LPCTSTR = String;

    #endregion

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    public static class Win32
    {
        #region EMoveMethod enum

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        public enum EMoveMethod : uint
        {
            /// <summary>
            /// </summary>
            Begin = 0,

            /// <summary>
            /// </summary>
            Current = 1,

            /// <summary>
            /// </summary>
            End = 2
        }

        #endregion

        /// <summary>
        /// </summary>
        public const uint OPEN_EXISTING = 3;

        /// <summary>
        /// </summary>
        public const uint FILE_SHARE_WRITE = 0x00000002;

        /// <summary>
        /// </summary>
        public const uint GENERIC_READ = (0x80000000);

        /// <summary>
        /// </summary>
        public const uint FILE_SHARE_READ = 0x00000001;

        /// <summary>
        /// </summary>
        public const uint GENERIC_WRITE = (0x40000000);

        /// <summary>
        /// </summary>
        public const uint FSCTL_LOCK_VOLUME = 0x00090018;

        /// <summary>
        /// </summary>
        public const uint FSCTL_UNLOCK_VOLUME = 0x0009001c;

        /// <summary>
        /// </summary>
        public const uint FSCTL_DISMOUNT_VOLUME = 0x00090020;

        /// <summary>
        /// </summary>
        private const uint FILE_SHARE_DELETE = 0x00000004;

        /// <summary>
        /// </summary>
        private const uint FILE_FLAG_NO_BUFFERING = 0x20000000;

        /// <summary>
        /// </summary>
        private const uint FILE_READ_ATTRIBUTES = (0x0080);

        /// <summary>
        /// </summary>
        private const uint FILE_WRITE_ATTRIBUTES = 0x0100;

        /// <summary>
        /// </summary>
        private const uint ERROR_INSUFFICIENT_BUFFER = 122;

        /// <summary>
        /// </summary>
        private const uint INVALID_SET_FILE_POINTER = 0xFFFFFFFF;

        /// <summary>
        /// 	Devices the io control.
        /// </summary>
        /// <param name = "hDevice">The h device.</param>
        /// <param name = "dwIoControlCode">The dw io control code.</param>
        /// <param name = "lpInBuffer">The lp in buffer.</param>
        /// <param name = "nInBufferSize">Size of the n in buffer.</param>
        /// <param name = "lpOutBuffer">The lp out buffer.</param>
        /// <param name = "nOutBufferSize">Size of the n out buffer.</param>
        /// <param name = "lpBytesReturned">The lp bytes returned.</param>
        /// <param name = "lpOverlapped">The lp overlapped.</param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern DWORD DeviceIoControl(
            SafeFileHandle hDevice,
            DWORD dwIoControlCode,
            LPVOID lpInBuffer,
            DWORD nInBufferSize,
            LPVOID lpOutBuffer,
            int nOutBufferSize,
            ref DWORD lpBytesReturned,
            LPOVERLAPPED lpOverlapped
            );

        /// <summary>
        /// 	Flushes the file buffers.
        /// </summary>
        /// <param name = "hFile">The h file.</param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FlushFileBuffers(SafeFileHandle hFile);

        /// <summary>
        /// 	Sets the file pointer.
        /// </summary>
        /// <param name = "hFile">The h file.</param>
        /// <param name = "lDistanceToMove">The l distance to move.</param>
        /// <param name = "lpDistanceToMoveHigh">The lp distance to move high.</param>
        /// <param name = "dwMoveMethod">The dw move method.</param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint SetFilePointer([In] SafeFileHandle hFile, [In] int lDistanceToMove,
                                                 out int lpDistanceToMoveHigh, [In] EMoveMethod dwMoveMethod);

        /// <summary>
        /// 	Creates the file.
        /// </summary>
        /// <param name = "lpFileName">Name of the lp file.</param>
        /// <param name = "dwDesiredAccess">The dw desired access.</param>
        /// <param name = "dwShareMode">The dw share mode.</param>
        /// <param name = "lpSecurityAttributes">The lp security attributes.</param>
        /// <param name = "dwCreationDisposition">The dw creation disposition.</param>
        /// <param name = "dwFlagsAndAttributes">The dw flags and attributes.</param>
        /// <param name = "hTemplateFile">The h template file.</param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern SafeFileHandle CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode,
                                                       IntPtr lpSecurityAttributes, uint dwCreationDisposition,
                                                       uint dwFlagsAndAttributes, IntPtr hTemplateFile);

        /// <summary>
        /// 	Reads the file.
        /// </summary>
        /// <param name = "handle">The handle.</param>
        /// <param name = "bytes">The bytes.</param>
        /// <param name = "numBytesToRead">The num bytes to read.</param>
        /// <param name = "numBytesRead">The num bytes read.</param>
        /// <param name = "overlapped_MustBeZero">The overlapped_ must be zero.</param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        [DllImport("kernel32", SetLastError = true)]
        internal static extern int ReadFile(SafeFileHandle handle, byte[] bytes, int numBytesToRead,
                                            out int numBytesRead, IntPtr overlapped_MustBeZero);

        /// <summary>
        /// 	Writes the file.
        /// </summary>
        /// <param name = "handle">The handle.</param>
        /// <param name = "bytes">The bytes.</param>
        /// <param name = "numBytesToWrite">The num bytes to write.</param>
        /// <param name = "numBytesWritten">The num bytes written.</param>
        /// <param name = "overlapped_MustBeZero">The overlapped_ must be zero.</param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern int WriteFile(SafeFileHandle handle, byte[] bytes, int numBytesToWrite,
                                             out int numBytesWritten, IntPtr overlapped_MustBeZero);

        /// <summary>
        /// 	Devices the io control.
        /// </summary>
        /// <param name = "hDevice">The h device.</param>
        /// <param name = "dwIoControlCode">The dw io control code.</param>
        /// <param name = "lpInBuffer">The lp in buffer.</param>
        /// <param name = "nInBufferSize">Size of the n in buffer.</param>
        /// <param name = "lpOutBuffer">The lp out buffer.</param>
        /// <param name = "nOutBufferSize">Size of the n out buffer.</param>
        /// <param name = "lpBytesReturned">The lp bytes returned.</param>
        /// <param name = "lpOverlapped">The lp overlapped.</param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool DeviceIoControl(SafeFileHandle hDevice, uint dwIoControlCode, byte[] lpInBuffer,
                                                  int nInBufferSize, byte[] lpOutBuffer, int nOutBufferSize,
                                                  out int lpBytesReturned, IntPtr lpOverlapped);

        /// <summary>
        /// 	Closes the handle.
        /// </summary>
        /// <param name = "handle">The handle.</param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool CloseHandle(SafeFileHandle handle);


        [DllImport("kernel32.dll")]
        public static extern uint QueryDosDevice(string lpDeviceName, StringBuilder lpTargetPath, int ucchMax);

        #region Nested type: DISK_GEOMETRY

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [StructLayout(LayoutKind.Sequential)]
        public struct DISK_GEOMETRY
        {
            /// <summary>
            /// </summary>
            public readonly LARGE_INTEGER Cylinders;

            /// <summary>
            /// </summary>
            public readonly MEDIA_TYPE MediaType;

            /// <summary>
            /// </summary>
            public readonly DWORD TracksPerCylinder;

            /// <summary>
            /// </summary>
            public readonly DWORD SectorsPerTrack;

            /// <summary>
            /// </summary>
            public readonly DWORD BytesPerSector;
        }

        #endregion

        #region Nested type: DISK_GEOMETRY_EX

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [StructLayout(LayoutKind.Sequential)]
        public struct DISK_GEOMETRY_EX
        {
            /// <summary>
            /// </summary>
            public readonly DISK_GEOMETRY Geometry;

            /// <summary>
            /// </summary>
            public readonly LARGE_INTEGER DiskSize;

            /// <summary>
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)] internal readonly byte[] Data;
        }

        #endregion
    }
}