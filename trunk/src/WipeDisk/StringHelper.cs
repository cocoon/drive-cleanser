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

namespace WipeDisk
{
    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    internal static class StringHelper
    {
        /// <summary>
        /// 	Byteses to string.
        /// </summary>
        /// <param name = "byteCount">The byte count.</param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        internal static String BytesToString(ulong byteCount)
        {
            string[] suf = {"B", "KB", "MB", "GB", "TB", "PB", "EB"}; //Longs run out around EB
            if (byteCount == 0)
                return "0" + suf[0];
            ulong bytes = byteCount;
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes/Math.Pow(1024, place), 1);
            return (num).ToString() + suf[place];
        }

        /// <summary>
        /// 	Determines whether the specified value is numeric.
        /// </summary>
        /// <param name = "value">The value.</param>
        /// <returns><c>true</c> if the specified value is numeric; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// </remarks>
        public static bool IsNumeric(string value)
        {
            try
            {
                char[] chars = value.ToCharArray();
                foreach (char c in chars)
                {
                    if (!char.IsNumber(c))
                        return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}