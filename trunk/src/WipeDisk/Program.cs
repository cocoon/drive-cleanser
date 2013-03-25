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
using System.Security.Cryptography;
using DriveCleanser.CmdLine;


#endregion

namespace WipeDisk
{
    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    internal class Program
    {
        /// <summary>
        /// 	Mains the specified args.
        /// </summary>
        /// <param name = "args">The args.</param>
        /// <remarks>
        /// </remarks>
        private static void Main(string[] args)
        {
           // byte[] returned = DriveCleanser.Cryptography.RNG.FillDoD5200ByteArray(1024);
            int driveCount = 0;
            ShowBanner();
            var pds = new PhysicalDrives();
            foreach (PhysicalDrive pd in pds)
            {
                driveCount++;
                Console.WriteLine(driveCount + ") " + pd);
            }
            Console.WriteLine("\nSelect the drive number to wipe or press q to quit.");
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            if (keyInfo.KeyChar == 'q' || keyInfo.KeyChar == 'Q')
            {
                return;
            }
            if (ValidateKeyInfo(keyInfo, driveCount))
            {
                int number;
                string selectedItem = keyInfo.KeyChar.ToString();
                bool result = int.TryParse(selectedItem, out number);
                if (result)
                {
                    // So far we know the user chose a valid number and within the range of our
                    // physical drive list but now lets check to ensure they didn't choose the drive
                    // that the system was booted with.. 
                    if (pds[number - 1].DeviceID.ToUpper() != WMIWrapper.GetSystemDrive().ToUpper())
                    {
                        var dw = new DiskWiper();
                        dw.wipeDisk(pds[number - 1]);
                    }
                    else
                    {
                        Console.WriteLine("That drive is the current boot device and cannot be wiped.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }

        private static void ShowBanner()
        {
            Console.WriteLine("DriveCleanser Copyright 2013 Brian David Patterson");
            Console.WriteLine("<pattersonbriandavid@gmail.com>\n");
        }

        /// <summary>
        /// 	Validates the key info.
        /// </summary>
        /// <param name = "keyInfo">The key info.</param>
        /// <param name = "driveCount">The drive count.</param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        private static bool ValidateKeyInfo(ConsoleKeyInfo keyInfo, int driveCount)
        {
            string selectedItem = keyInfo.KeyChar.ToString();
            if (StringHelper.IsNumeric(selectedItem))
            {
                int number;

                bool result = int.TryParse(selectedItem, out number);
                if (result && number <= driveCount)
                {
                    return true;
                }
            }
            return false;
        }


        //private static string GetRealPath(string path)
        //{
        //    string realPath;
        //    var pathInformation = new StringBuilder(250);

        //    // Get the drive letter of the 
        //    string driveLetter = Path.GetPathRoot(path).Replace("\\\\?\\", "");
        //    Win32.QueryDosDevice(driveLetter, pathInformation, 250);


        //    return pathInformation.ToString();
        //}
    }
}