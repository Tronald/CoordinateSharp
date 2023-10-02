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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoordinateSharp
{
    /// <summary>
    /// Application wide, global settings for CoordinateSharp. Should be used with caution and only during times where the specified
    /// setting will not have varying changes throughout the application.
    /// Set during application start up.
    /// </summary>
    public static class GlobalSettings
    {
        /// <summary>
        /// Application wide, default EagerLoad settings for CoordinateSharp.
        /// </summary>
        public static EagerLoad Default_EagerLoad { get; set; } = new EagerLoad(true);

        /// <summary>
        /// Application wide, default coordinate formatting output options.
        /// </summary>
        public static CoordinateFormatOptions Default_CoordinateFormatOptions { get; set; } = new CoordinateFormatOptions();

        /// <summary>
        /// Application wide, default parsable formats
        /// </summary>
        public static Allowed_Parse_Format Default_Parsable_Formats { get; set; } = Allowed_Parse_Format.Lat_Long | Allowed_Parse_Format.MGRS | Allowed_Parse_Format.UTM |
            Allowed_Parse_Format.Cartesian_ECEF | Allowed_Parse_Format.Cartesian_Spherical |
            Allowed_Parse_Format.WebMercator | Allowed_Parse_Format.GEOREF;

        /// <summary>
        /// Application wide, default Cartesian parsed format.
        /// </summary>
        public static CartesianType Default_Cartesian_Type { get; set; } = CartesianType.Cartesian;

        /// <summary>
        /// Application wide, default Equatorial Radius (Semi Major Axis).
        /// </summary>
        public static double Default_EquatorialRadius { get; set; } = DataValues.DefaultSemiMajorAxis;
        /// <summary>
        /// Application wide, default Inverse Flattening.
        /// </summary>
        public static double Default_InverseFlattening { get; set; } = DataValues.DefaultInverseFlattening;

        /// <summary>
        /// Sets whether Coordinate calculates in UTC (default) or environment's DateTimeKind specification. Allows Coordinate objects to run in local time automatically if set to true. 
        /// </summary>
        /// <remarks>
        /// If the `Coordinate` object's set `GeoDate` value has a `DateTimeKind` value set as `Local` and `Allow_Coordinate_DateTimeKind_Specification` is set to `true`, then the Coordinate object will initialize and calculate celestial data 
        /// based on the environment's local time by default.
        /// 
        /// If `DateTimeKind` Specification is allowed, then setting the `Coordinate.Offset` property will throw an `InvalidOperationException` as CoordinateSharp is self-handling offsets.
        /// 
        /// While Allowing `DateTimeKind` specification is convenient, it is not recommended if you require control of timezones or manual adjustments of UTC offsets. Use only if applicable. 
        /// </remarks>
        public static bool Allow_Coordinate_DateTimeKind_Specification { get; set; } = false;
    
        /// <summary>
        /// Set the default Equatorial Radius and Inverse Flattening based on a specified ellipsoid.
        /// </summary>
        /// <param name="spec">Earth_Ellipsoid_Spec</param>
        public static void Set_DefaultDatum(Earth_Ellipsoid_Spec spec)
        {
            Set_DefaultDatum(Earth_Ellipsoid.Get_Ellipsoid(spec));         
        }

        /// <summary>
        /// Set the default Equatorial Radius and Inverse Flattening based on a specified ellipsoid.
        /// </summary>
        /// <param name="ee">Earth_Ellipsoid</param>
        public static void Set_DefaultDatum(Earth_Ellipsoid ee)
        {
            Default_EquatorialRadius = ee.Equatorial_Radius;
            Default_InverseFlattening = ee.Inverse_Flattening;
        }
    }
}
