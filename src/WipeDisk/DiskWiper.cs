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
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

#endregion

namespace WipeDisk
{
    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    internal class DiskWiper
    {
        //*******************************************************************
        // NOTE
        // The following number of the number of 512 byte blocks that is 
        // written to the disk.
        /// <summary>
        /// </summary>
        private const int MULTIPLIER = 512;

        //*******************************************************************


        /// <summary>
        /// 	Wipes the disk.
        /// </summary>
        /// <param name = "pd">The pd.</param>
        /// <remarks>
        /// </remarks>
        public void wipeDisk(PhysicalDrive pd)
        {
            DateTime dtStart = DateTime.Now;

            bool success = false;
            int intOut;
            List<string> drives = WMIWrapper.GetDevices(pd.DeviceID);

            long DiskSize = pd.Geometry.DiskSize; 

            SafeFileHandle diskHandle = Win32.CreateFile(pd.DeviceID, Win32.GENERIC_WRITE, 0, IntPtr.Zero,
                                                         Win32.OPEN_EXISTING, 0,
                                                         IntPtr.Zero);
            if (diskHandle.IsInvalid)
            {
                Console.WriteLine(pd.DeviceID + " open error.");
                return;
            }
#if VERBOSE
            Console.WriteLine(pd.DeviceID + " " + Marshal.GetHRForLastWin32Error().ToString() + ": opened.");
#endif
            // Now do the same for all the drive letters...

            foreach (string s in drives)
            {
                int i = LockDrives(@"\\.\" + s);
                if (i < 0)
                    return;
            }

            success = Win32.DeviceIoControl(diskHandle, Win32.FSCTL_LOCK_VOLUME, null, 0, null, 0, out intOut,
                                            IntPtr.Zero);
            if (!success)
            {
                Console.WriteLine(pd.DeviceID + " lock error.");
                Win32.CloseHandle(diskHandle);
                return;
            }

#if VERBOSE
            Console.WriteLine(pd.DeviceID + " " + Marshal.GetHRForLastWin32Error().ToString() + ": locked.");
#endif

            success = Win32.DeviceIoControl(diskHandle, Win32.FSCTL_DISMOUNT_VOLUME, null, 0, null, 0, out intOut,
                                            IntPtr.Zero);
            if (!success)
            {
                Console.WriteLine(pd.DeviceID + " " + Marshal.GetHRForLastWin32Error().ToString() + ": dismount error.");
                Win32.DeviceIoControl(diskHandle, Win32.FSCTL_UNLOCK_VOLUME, null, 0, null, 0, out intOut, IntPtr.Zero);
                Win32.CloseHandle(diskHandle);
                return;
            }

#if VERBOSE
            Console.WriteLine(pd.DeviceID + " " + Marshal.GetHRForLastWin32Error().ToString() + ": unmounted.");
#endif
            int numBytesPerSector = (int) pd.Geometry.BytesPerSector*MULTIPLIER;
            long numTotalSectors = (DiskSize/(pd.Geometry.BytesPerSector*MULTIPLIER));
            long missingData = DiskSize - ((pd.Geometry.BytesPerSector*MULTIPLIER)*(numTotalSectors));
            long cleanupStart = numTotalSectors*numBytesPerSector;
#if VERBOSE
            Console.WriteLine("Assert: Excess Data:"+missingData);
            Console.WriteLine("Assert: OrigBytePerSector:" + pd.Geometry.BytesPerSector);
            Console.WriteLine("Assert: BytePerSector:"+numBytesPerSector);
            Console.WriteLine("Assert: TotalSectors:" + numTotalSectors);
            Console.WriteLine("Assert: DiskSize:" + DiskSize);
           
            Console.WriteLine("Cleanup start: "+cleanupStart);
#endif
            var bytesToWrite = new byte[numBytesPerSector];
            for (int x = 0; x < numBytesPerSector; x++)
            {
                bytesToWrite[x] = 0xFF;
            }
            Console.WriteLine("**************************************************************");
            Console.WriteLine("Ready to security wipe drive. This action CANNOT be undone and");
            Console.WriteLine("all information on the drive will be permanently erased!");
            Console.WriteLine("Press 'Y' to begin or any other key to cancel.");
            ConsoleKeyInfo readChar = Console.ReadKey();
            if (readChar.KeyChar == 'y' || readChar.KeyChar == 'Y')
            {
                for (long sectorNum = 0; sectorNum < numTotalSectors; sectorNum++)
                {
                    int numBytesWritten = 0;

                    try
                    {
                        long offset = (sectorNum*(numBytesPerSector));
                        var lo = (int) (offset & 0xffffffff);
                        var hi = (int) (offset >> 32);
                        uint rvalsfp = Win32.SetFilePointer(diskHandle, lo, out hi,
                                                            Win32.EMoveMethod.Begin);


                        int rval = Win32.WriteFile(diskHandle, bytesToWrite, bytesToWrite.Length, out numBytesWritten,
                                                   IntPtr.Zero);

                        if (numBytesWritten != bytesToWrite.Length)
                        {
                            Console.WriteLine("Write error on track " + sectorNum.ToString() + " from " +
                                              (sectorNum*numBytesPerSector).ToString() + "-" +
                                              Marshal.GetHRForLastWin32Error().ToString() + ": Only " +
                                              numBytesWritten.ToString() + "/" + bytesToWrite.Length.ToString() +
                                              " bytes written.");
                            break;
                        }
                        else
                        {
                            if (sectorNum < 5000 || (sectorNum%1000 == 0) || numTotalSectors <= 10000)
                                Console.Write("\r" + sectorNum + " of " + numTotalSectors + "  (" +
                                              StringHelper.BytesToString((ulong) (sectorNum*(numBytesPerSector))) +
                                              ")                      ");
                            //Console.WriteLine("Write success " + Marshal.GetHRForLastWin32Error().ToString() + ": " + numBytesWritten.ToString() + "/" + junkBytes.Length.ToString() + " bytes written.");
                        }
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine("******************  CAUGHT EXCEPTION!! " + exc.Message);
                    }
                    
                }
                StartCleanup(cleanupStart, missingData, pd.Geometry.BytesPerSector, diskHandle);
                Console.WriteLine("\r                                                ");

                bool flushResult = Win32.FlushFileBuffers(diskHandle);
#if VERBOSE
                if (flushResult)
                    Console.WriteLine("Successfully flushed file buffers.");
#endif
                success = Win32.DeviceIoControl(diskHandle, Win32.FSCTL_UNLOCK_VOLUME, null, 0, null, 0, out intOut,
                                                IntPtr.Zero);
                if (success)
                {
#if VERBOSE
                    Console.WriteLine(pd.DeviceID + " " + Marshal.GetHRForLastWin32Error().ToString() + ": unlocked.");
#endif
                }
                else
                {
                    Console.WriteLine(pd.DeviceID + " " + Marshal.GetHRForLastWin32Error().ToString() +
                                      ": unlock error: " +
                                      Marshal.GetHRForLastWin32Error().ToString());
                }

                success = Win32.CloseHandle(diskHandle);
                if (success)
                {
#if VERBOSE
                    Console.WriteLine(pd.DeviceID + " " + Marshal.GetHRForLastWin32Error().ToString() + ": handle closed.");
#endif
                }
                else
                {
                    Console.WriteLine(pd.DeviceID + " " + Marshal.GetHRForLastWin32Error().ToString() +
                                      ": close handle error: " + Marshal.GetHRForLastWin32Error().ToString());
                }

                TimeSpan tsFinished = DateTime.Now.Subtract(dtStart);
                Console.WriteLine("Total Time: " + tsFinished.TotalMinutes.ToString() + " minutes.");
            }
        }

        /// <summary>
        /// 	Wipes all remaining bytes on the drive that didn't get wiped during our "Block Wipe" process.
        /// </summary>
        /// <param name = "startOffset">The start offset.</param>
        /// <param name = "excessData">The excess data.</param>
        /// <param name = "bytesPerSector">The bytes per sector.</param>
        /// <param name = "diskHandle">The disk handle.</param>
        /// <remarks>
        /// </remarks>
        private void StartCleanup(long startOffset, long excessData, uint bytesPerSector, SafeFileHandle diskHandle)
        {
            var junkBytes = new byte[bytesPerSector];
            for (int x = 0; x < bytesPerSector; x++)
            {
                junkBytes[x] = 66; //0xFF
            }

            long numTotalSectors = excessData/bytesPerSector;

            for (long sectorNum = 0; sectorNum < numTotalSectors; sectorNum++)
            {
                int numBytesWritten = 0;
               

                try
                {
                    long offset = (sectorNum*(bytesPerSector)) + startOffset;
                    var lo = (int) (offset & 0xffffffff);
                    var hi = (int) (offset >> 32);
                    uint rvalsfp = Win32.SetFilePointer(diskHandle, lo, out hi,
                                                        Win32.EMoveMethod.Begin);

                    int rval = Win32.WriteFile(diskHandle, junkBytes, junkBytes.Length, out numBytesWritten, IntPtr.Zero);

                    if (numBytesWritten != junkBytes.Length)
                    {
                        Console.WriteLine("Write error on track " + sectorNum.ToString());
                        break;
                    }
                    else
                    {
                        //if (sectorNum < 10000 || (sectorNum % 1000 == 0))
                        Console.Write("\r" + sectorNum + " of " + numTotalSectors + "  (" +
                                      StringHelper.BytesToString((ulong) (sectorNum*(bytesPerSector)) +
                                                                 (ulong) startOffset) + ")                      ");
                    }
                }
                catch (Exception exc)
                {
                    Console.WriteLine("******************  CAUGHT EXCEPTION!! " + exc.Message);
                  
                }
            }
        }

        /// <summary>
        /// 	Locks the drives.
        /// </summary>
        /// <param name = "deviceId">The device id.</param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        private int LockDrives(string deviceId)
        {
            bool success = false;
            int intOut;
            SafeFileHandle diskHandle = Win32.CreateFile(deviceId, Win32.GENERIC_READ | Win32.GENERIC_WRITE,
                                                         Win32.FILE_SHARE_WRITE,
                                                         IntPtr.Zero,
                                                         Win32.OPEN_EXISTING,
                                                         0,
                                                         IntPtr.Zero);
            if (diskHandle.IsInvalid)
            {
                Console.WriteLine(deviceId + " open error.");
                return -1;
            }
#if VERBOSE
            Console.WriteLine(deviceId + " " + Marshal.GetHRForLastWin32Error().ToString() + ": opened.");
#endif
            success = Win32.DeviceIoControl(diskHandle, Win32.FSCTL_LOCK_VOLUME, null, 0, null, 0, out intOut,
                                            IntPtr.Zero);
            if (!success)
            {
                Console.WriteLine(deviceId + " lock error.");
                Win32.CloseHandle(diskHandle);
                return -1;
            }
#if VERBOSE
            Console.WriteLine(deviceId + " " + Marshal.GetHRForLastWin32Error().ToString() + ": locked.");
#endif
            success = Win32.DeviceIoControl(diskHandle, Win32.FSCTL_DISMOUNT_VOLUME, null, 0, null, 0, out intOut,
                                            IntPtr.Zero);
            if (!success)
            {
                Console.WriteLine(deviceId + " " + Marshal.GetHRForLastWin32Error().ToString() + ": dismount error.");
                Win32.DeviceIoControl(diskHandle, Win32.FSCTL_UNLOCK_VOLUME, null, 0, null, 0, out intOut, IntPtr.Zero);
                Win32.CloseHandle(diskHandle);
                return -1;
            }
#if VERBOSE
            Console.WriteLine(deviceId + " " + ": Dismounted.");
#endif
            return 0;
        }
    }
}