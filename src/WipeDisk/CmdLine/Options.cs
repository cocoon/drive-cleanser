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

namespace DriveCleanser.CmdLine
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    internal class Options
    {
        /// <summary>
        /// Gets or sets a value indicating whether [wipe drive].
        /// </summary>
        /// <value><c>true</c> if [wipe drive]; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public bool WipeDrive { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether [wipe free].
        /// </summary>
        /// <value><c>true</c> if [wipe free]; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public bool WipeFree { get; set; }
    }
}