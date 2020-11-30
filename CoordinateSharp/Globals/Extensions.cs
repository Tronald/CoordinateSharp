/*
CoordinateSharp is a .NET standard library that is intended to ease geographic coordinate 
format conversions and location based celestial calculations.
https://github.com/Tronald/CoordinateSharp

Many celestial formulas in this library are based on Jean Meeus's 
Astronomical Algorithms (2nd Edition). Comments that reference only a chapter
are referring to this work.

License

CoordinateSharp is split licensed and may be licensed under the GNU Affero General Public License version 3 or a commercial use license as stated.

Copyright (C) 2019, Signature Group, LLC
  
This program is free software; you can redistribute it and/or modify it under the terms of the GNU Affero General Public License version 3 
as published by the Free Software Foundation with the addition of the following permission added to Section 15 as permitted in Section 7(a): 
FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY Signature Group, LLC. Signature Group, LLC DISCLAIMS THE WARRANTY OF 
NON INFRINGEMENT OF THIRD PARTY RIGHTS.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Affero General Public License for more details. You should have received a copy of the GNU 
Affero General Public License along with this program; if not, see http://www.gnu.org/licenses or write to the 
Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA, 02110-1301 USA, or download the license from the following URL:

https://www.gnu.org/licenses/agpl-3.0.html

The interactive user interfaces in modified source and object code versions of this program must display Appropriate Legal Notices, 
as required under Section 5 of the GNU Affero General Public License.

You can be released from the requirements of the license by purchasing a commercial license. Buying such a license is mandatory 
as soon as you develop commercial activities involving the CoordinateSharp software without disclosing the source code of your own applications. 
These activities include: offering paid services to customers as an ASP, on the fly location based calculations in a web application, 
or shipping CoordinateSharp with a closed source product.

Organizations or use cases that fall under the following conditions may receive a free commercial use license upon request.
-Department of Defense
-Department of Homeland Security
-Open source contributors to this library
-Scholarly or scientific uses on a case by case basis.
-Emergency response / management uses on a case by case basis.

For more information, please contact Signature Group, LLC at this address: sales@signatgroup.com
*/
using CoordinateSharp.Formatters;
using System;

namespace CoordinateSharp.Formatters
{
    /// <summary>
    /// Extensions methods
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Convert degree double to radians.
        /// </summary>
        /// <param name="d">double</param>
        /// <returns>Radians double</returns>
        public static double ToRadians(this double d)
        {
            return Format.ToRadians(d);
        }

        /// <summary>
        /// Convert radians double to degrees.
        /// </summary>
        /// <param name="d">double</param>
        /// <returns>Degrees double</returns>
        public static double ToDegrees(this double d)
        {
            return Format.ToDegrees(d);
        }

        /// <summary>
        /// Converts double to DDM.
        /// </summary>
        /// <param name="d">double</param>
        /// <returns>DDM double[]</returns>
        public static double[] ToDegreeDecimalMinutes(this double d)
        {
            return Format.ToDegreeDecimalMinutes(d);
        }

        /// <summary>
        /// Converts double to DMS.
        /// </summary>
        /// <param name="d">double</param>
        /// <returns>DMS double[]</returns>
        public static double[] ToDegreesMinutesSeconds(this double d)
        {

            return Format.ToDegreesMinutesSeconds(d);
        }

        /// <summary>
        /// Convert double to normalized 360 Degrees.
        /// </summary>
        /// <param name="d">double</param>
        /// <returns>Normalize 360 degree double</returns>
        public static double NormalizeDegrees360(this double d)
        {
            return Format.NormalizeDegrees360(d);
        }

        /// <summary>
        /// Convert double to HMS
        /// </summary>
        /// <param name="d">double</param>
        /// <returns>HMS double[]</returns>
        public static double[] ToHoursMinutesSeconds(this double d)
        {
            return Format.ToHoursMinutesSeconds(d);
        }

        /// <summary>
        /// Converts decimal minutes to fractional degree minutes.
        /// </summary>
        /// <param name="d">Minutes</param>
        /// <returns>double</returns>
        public static double ToMinutes(this double d)
        {
            return Format.ToMinutes(d);
        }


        /// <summary>
        /// Converts decimal seconds to fractional degree seconds.
        /// </summary>
        /// <param name="d">Seconds</param>
        /// <returns>double</returns>
        public static double ToSeconds(this double d)
        {
            return Format.ToSeconds(d);
        }
    }
}
