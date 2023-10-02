/*
CoordinateSharp is a .NET standard library that is intended to ease geographic coordinate 
format conversions and location based celestial calculations.
https://github.com/Tronald/CoordinateSharp

Many celestial formulas in this library are based on Jean Meeus's 
Astronomical Algorithms (2nd Edition). Comments that reference only a chapter
are referring to this work.

License

CoordinateSharp is split licensed and may be licensed under the GNU Affero General Public License version 3 or a commercial use license as stated.

Copyright (C) 2023, Signature Group, LLC
  
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

Organizations or use cases that fall under the following conditions may receive a free commercial use license upon request on a case by case basis.


	-Open source contributors to this library.
	-Scholarly or scientific research.
	-Emergency response / management uses.

Please visit http://coordinatesharp.com/licensing or contact Signature Group, LLC to purchase a commercial license, or for any questions regarding the AGPL 3.0 license requirements or free use license: sales@signatgroup.com.
*/
using System;


namespace CoordinateSharp
{
    public partial class ECEF
    {
        /// <summary>
        /// Parses a string into an ECEF coordinate (kilometer format).
        /// </summary>
        /// <param name="value">string</param>          
        /// <returns>ECEF</returns>
        /// <example>
        /// The following example attempts to parse an ECEF coordinate.
        /// <code>
        /// ECEF ecef = ECEF.Parse("217.206 km, -4127.862 km, 4841.101 km");
        /// </code>
        /// </example>
        public static ECEF Parse(string value)
        {
            ECEF ecef;
            if (TryParse(value, out ecef)) { return ecef; }

            throw new FormatException(string.Format("Input Coordinate \"{0}\" was not in a correct format.", value));
        }

        /// <summary>
        /// Attempts to parse a string into an ECEF coordinate (kilometer format).
        /// </summary>
        /// <param name="value">string</param>    
        /// <param name="ecef">ECEF</param>
        /// <returns>ECEF</returns>
        /// <example>
        /// The following example attempts to parse an ECEF coordinate.
        /// <code>
        /// ECEF ecef;
        /// if(!ECEF.TryParse("217.206 km, -4127.862 km, 4841.101 km", out ecef))
        /// {
        ///     Console.WriteLine(ecef);//217.206 km, -4127.862 km, 4841.101 km
        /// }
        /// </code>
        /// </example>
        public static bool TryParse(string value, out ECEF ecef)
        {
            double[] vals = null;

            if (FormatFinder.TryCartesian(value.ToUpper().Replace("KM", "").Replace("X", "").Replace("Y", "").Replace("Z", ""), out vals))
            {
                try
                {
                    ecef = new ECEF(vals[0], vals[1], vals[2]);

                    return true;
                }
                catch
                {//Parser failed try next method 
                }

            }

            ecef = null;
            return false;
        }
    }
}
