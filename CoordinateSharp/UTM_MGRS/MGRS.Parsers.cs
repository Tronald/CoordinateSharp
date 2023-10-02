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
using System.Globalization;
namespace CoordinateSharp
{
    public partial class MilitaryGridReferenceSystem
    {
        /// <summary>
        /// Parses a string into an MGRS coordinate.
        /// </summary>
        /// <param name="value">string</param>
        /// <returns>MilitaryGridReferenceSystem</returns>
        /// <example>
        /// The following example parses an MGRS coordinate set with the default WGS84 ellipsoid.
        /// <code>
        /// MilitaryGridReferenceSystem mgrs = MilitaryGridReferenceSystem.Parse("16U EA 00872 05009");    
        /// </code>
        /// </example>
        public static MilitaryGridReferenceSystem Parse(string value)
        {
            MilitaryGridReferenceSystem mgrs;
            Earth_Ellipsoid ee = Earth_Ellipsoid.Get_Ellipsoid(Earth_Ellipsoid_Spec.WGS84_1984);
            if (TryParse(value, ee.Equatorial_Radius, ee.Inverse_Flattening, out mgrs)) { return mgrs; }

            throw new FormatException(string.Format("Input Coordinate \"{0}\" was not in a correct format.", value));
        }
        /// <summary>
        /// Parses a string into an MGRS coordinate.
        /// </summary>
        /// <param name="value">string</param>
        /// <param name="spec">Earth_Ellipsoid_Spec</param>
        /// <returns>MilitaryGridReferenceSystem</returns>
        /// <example>
        /// The following example parses an MGRS coordinate set with a GRS80 system ellipsoid.
        /// <code>
        /// MilitaryGridReferenceSystem mgrs = MilitaryGridReferenceSystem.Parse("16U EA 00872 05009", Earth_Ellipsoid_Spec.GRS80_1979);    
        /// </code>
        /// </example>
        public static MilitaryGridReferenceSystem Parse(string value, Earth_Ellipsoid_Spec spec)
        {
            MilitaryGridReferenceSystem mgrs;
            Earth_Ellipsoid ee = Earth_Ellipsoid.Get_Ellipsoid(spec);
            if (TryParse(value, ee.Equatorial_Radius, ee.Inverse_Flattening, out mgrs)) { return mgrs; }
            throw new FormatException(string.Format("Input Coordinate \"{0}\" was not in a correct format.", value));
        }
        /// <summary>
        /// Parses a string into an MGRS coordinate.
        /// </summary>
        /// <param name="value">string</param>
        /// <param name="radius">Equatorial Radius (Semi-Major Axis)</param>
        /// <param name="flattening">Inverse Flattening</param>
        /// <returns>MilitaryGridReferenceSystem</returns>
        /// <example>
        /// The following example parses an MGRS coordinate set with GRS80 system ellipsoid values.
        /// <code>
        /// MilitaryGridReferenceSystem mgrs = MilitaryGridReferenceSystem.Parse("16U EA 00872 05009", 6378137.0, 298.257222101);    
        /// </code>
        /// </example>
        public static MilitaryGridReferenceSystem Parse(string value, double radius, double flattening)
        {
            MilitaryGridReferenceSystem mgrs;
            if (TryParse(value, radius, flattening, out mgrs)) { return mgrs; }
            throw new FormatException(string.Format("Input Coordinate \"{0}\" was not in a correct format.", value));
        }

        /// <summary>
        /// Attempts to parse a string into an MGRS coordinate.
        /// </summary>
        /// <param name="value">string</param>
        /// <param name="mgrs">MilitaryGridReferenceSystem</param>
        /// <returns>MilitaryGridReferenceSystem</returns>
        /// <example>
        /// The following example attempts to parse an MGRS coordinate set with the default WGS84 ellipsoid.
        /// <code>
        /// MilitaryGridReferenceSystem mgrs;
        /// if(!MilitaryGridReferenceSystem.TryParse("16U EA 00872 05009", out mgrs))
        /// {
        ///     Console.WriteLine(mgrs);//16U EA 00872 05009
        /// }
        /// </code>
        /// </example>
        public static bool TryParse(string value, out MilitaryGridReferenceSystem mgrs)
        {
            Earth_Ellipsoid ee = Earth_Ellipsoid.Get_Ellipsoid(Earth_Ellipsoid_Spec.WGS84_1984);
            return TryParse(value, ee.Equatorial_Radius, ee.Inverse_Flattening, out mgrs);
        }
        /// <summary>
        /// Attempts to parse a string into an MGRS coordinate.
        /// </summary>
        /// <param name="value">string</param>
        /// <param name="spec">Earth_Ellipsoid_Spec</param>
        /// <param name="mgrs">MilitaryGridReferenceSystem</param>
        /// <returns>MilitaryGridReferenceSystem</returns>
        /// <example>
        /// The following example attempts to parse an MGRS coordinate set with a GRS80 system ellipsoid.
        /// <code>
        /// MilitaryGridReferenceSystem mgrs;
        /// if(!MilitaryGridReferenceSystem.TryParse("16U EA 00872 05009", Earth_Ellipsoid_Spec.GRS80_1979, out mgrs))
        /// {
        ///     Console.WriteLine(mgrs);//16U EA 00872 05009
        /// }
        /// </code>
        /// </example>
        public static bool TryParse(string value, Earth_Ellipsoid_Spec spec, out MilitaryGridReferenceSystem mgrs)
        {
            Earth_Ellipsoid ee = Earth_Ellipsoid.Get_Ellipsoid(spec);
            return TryParse(value, ee.Equatorial_Radius, ee.Inverse_Flattening, out mgrs);
        }
        /// <summary>
        /// Attempts to parse a string into an MGRS coordinate.
        /// </summary>
        /// <param name="value">string</param>
        /// <param name="radius">Equatorial Radius (Semi-Major Axis)</param>
        /// <param name="flattening">Inverse Flattening</param>
        /// <param name="mgrs">MilitaryGridReferenceSystem</param>
        /// <returns>MilitaryGridReferenceSystem</returns>
        /// <example>
        /// The following example attempts to parse an MGRS coordinate set with GRS80 system ellipsoid values.
        /// <code>
        /// MilitaryGridReferenceSystem mgrs;
        /// if(!MilitaryGridReferenceSystem.TryParse("16U EA 00872 05009", 6378137.0, 298.257222101, out mgrs))
        /// {
        ///     Console.WriteLine(mgrs);//16U EA 00872 05009
        /// }
        /// </code>
        /// </example>
        public static bool TryParse(string value, double radius, double flattening, out MilitaryGridReferenceSystem mgrs)
        {
            string[] vals = null;

            if (FormatFinder.TryMGRS(value, out vals) || FormatFinder.TryMGRS_Polar(value, out vals))
            {
                try
                {
                    double zone = Convert.ToDouble(vals[0], CultureInfo.InvariantCulture);
                    double easting = Convert.ToDouble(vals[3], CultureInfo.InvariantCulture);
                    double northing = Convert.ToDouble(vals[4], CultureInfo.InvariantCulture);
                    mgrs = new MilitaryGridReferenceSystem(vals[1], (int)zone, vals[2], easting, northing, radius, flattening);
                  
                    return true;
                }
                catch
                {//Parser failed try next method 
                }
             
            }

            mgrs = null;
            return false;
        }
    }
}
