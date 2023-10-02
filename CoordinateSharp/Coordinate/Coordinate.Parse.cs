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
    public partial class Coordinate
    {

        /*PARSE METHODS*/

        /// <summary>
        /// Parses a string into a Coordinate.
        /// </summary>
        /// <param name="value">Coordinate string</param>
        /// <returns>Coordinate</returns>
        /// <example>
        /// The following example parses a decimal degree formatted geodetic coordinate string.
        /// <code>
        /// Coordinate c = Coordinate.Parse("N 32.891º W 64.872º", out c);
        /// </code>
        /// </example>
        public static Coordinate Parse(string value)
        {
            Coordinate coordinate = null;
            if(TryParse(value, out coordinate))
            {
                return coordinate;
            }

            throw new FormatException(string.Format("Input Coordinate \"{0}\" was not in a correct format.", value));
        }
        /// <summary>
        /// Parses a string into a Coordinate with specified date.
        /// </summary>
        /// <param name="value">Coordinate string</param>
        /// <param name="geoDate">GeoDate</param>
        /// <returns>Coordinate</returns>
        /// <example>
        /// The following example parses a decimal degree formatted geodetic coordinate string, with a provided GeoDate. 
        /// <code>
        /// Coordinate c = Coordinate.Parse("N 32.891º W 64.872º", new DateTime(2018,7,7));
        /// </code>
        /// </example>
        public static Coordinate Parse(string value, DateTime geoDate)
        {
            Coordinate coordinate = null;
            if (TryParse(value, geoDate, out coordinate))
            {
                return coordinate;
            }

            throw new FormatException(string.Format("Input Coordinate \"{0}\" was not in a correct format.", value));
        }
        /// <summary>
        /// Parses a string into a Coordinate with a specified Cartesian system type.
        /// </summary>
        /// <param name="value">Coordinate string</param>
        /// <param name="cartesianType">Cartesian Type</param>
        /// <returns>Coordinate</returns>
        /// <example>
        /// The following example parses an ECEF formatted coordinate string. 
        /// Because this is an ECEF Cartesian type coordinate, we will specify the Cartesian system type.
        /// <code>
        /// Coordinate c = Coordinate.Parse("5242.097 km, 2444.43 km, 2679.074 km", CartesianType.Cartesian);
        /// </code>
        /// </example>
        public static Coordinate Parse(string value, CartesianType cartesianType)
        {
            Coordinate coordinate = null;
            if (TryParse(value, cartesianType, out coordinate))
            {
                return coordinate;
            }

            throw new FormatException(string.Format("Input Coordinate \"{0}\" was not in a correct format.", value));
        }
        /// <summary>
        /// Parses a string into a Coordinate with specified DateTime and Cartesian system type.
        /// </summary>
        /// <param name="value">Coordinate string</param>
        /// <param name="geoDate">GeoDate</param>
        /// <param name="cartesianType">Cartesian Type</param>
        /// <returns>Coordinate</returns>
        /// <example>
        /// The following example parses an ECEF formatted coordinate string, with an included GeoDate. 
        /// Because this is an ECEF Cartesian type coordinate, we will specify the Cartesian system type.
        /// <code>
        /// Coordinate c = Coordinate.Parse("5242.097 km, 2444.43 km, 2679.074 km", new DateTime(2018,7,7), CartesianType.ECEF);
        /// </code>
        /// </example>
        public static Coordinate Parse(string value, DateTime geoDate, CartesianType cartesianType)
        {
            Coordinate coordinate = null;
            if (TryParse(value, geoDate, cartesianType, out coordinate))
            {
                return coordinate;
            }

            throw new FormatException(string.Format("Input Coordinate \"{0}\" was not in a correct format.", value));
        }
        /// <summary>
        /// Parses a string into a Coordinate with specified eager loading settings.
        /// </summary>
        /// <param name="value">Coordinate string</param>
        /// <param name="eagerLoad">Eager loading options</param>
        /// <returns>Coordinate</returns>
        /// <example>
        /// The following example parses a decimal degree formatted geodetic coordinate string.
        /// Eager loading is turned off for improved efficiency.
        /// <code>
        /// EagerLoad el = new EagerLoad(false);
        /// Coordinate c = Coordinate.Parse("N 32.891º W 64.872º", el);
        /// </code>
        /// </example>
        public static Coordinate Parse(string value, EagerLoad eagerLoad)
        {
            Coordinate coordinate = null;
            if (TryParse(value, eagerLoad, out coordinate))
            {
                return coordinate;
            }

            throw new FormatException(string.Format("Input Coordinate \"{0}\" was not in a correct format.", value));
        }
        /// <summary>
        /// Parses a string into a Coordinate with a specified date and eager loading settings.
        /// </summary>
        /// <param name="value">Coordinate string</param>
        /// <param name="geoDate">GeoDate</param>
        /// <param name="eagerLoad">Eager loading options</param>
        /// <returns>Coordinate</returns>
        /// <example>
        /// The following example parses a decimal degree formatted geodetic coordinate string with a provided GeoDate. 
        /// Eager loading is set to load celestial calculations only.
        /// <code>
        /// EagerLoad el = new EagerLoad(EagerLoadType.Celestial);
        /// Coordinate c = Coordinate.Parse("N 32.891º W 64.872º", new DateTime(2018,7,7), el);       
        /// </code>
        /// </example>
        public static Coordinate Parse(string value, DateTime geoDate, EagerLoad eagerLoad)
        {
            Coordinate coordinate = null;
            if (TryParse(value, geoDate, eagerLoad, out coordinate))
            {
                return coordinate;
            }

            throw new FormatException(string.Format("Input Coordinate \"{0}\" was not in a correct format.", value));
        }
        /// <summary>
        /// Parses a string into a Coordinate with a specified Cartesian system type and eager loading settings.
        /// </summary>
        /// <param name="value">Coordinate string</param>    
        /// <param name="cartesianType">Cartesian Type</param>
        /// <param name="eagerLoad">Eager loading options</param>
        /// <returns>Coordinate</returns>
        /// <example>
        /// The following example parses an ECEF formatted coordinate string. 
        /// Because this is an ECEF Cartesian type coordinate, we will specify the Cartesian system type.
        /// Eager loading options have been specified for efficiency.
        /// <code>      
        /// EagerLoad el = new EagerLoad(EagerLoadType.Cartesian);
        /// Coordinate c = Coordinate.Parse("5242.097 km, 2444.43 km, 2679.074 km", CartesianType.Cartesian, el);
        /// </code>
        /// </example>
        public static Coordinate Parse(string value, CartesianType cartesianType, EagerLoad eagerLoad)
        {
            Coordinate coordinate = null;
            if (TryParse(value, cartesianType, eagerLoad, out coordinate))
            {
                return coordinate;
            }

            throw new FormatException(string.Format("Input Coordinate \"{0}\" was not in a correct format.", value));
        }
        /// <summary>
        /// Parses a string into a Coordinate with a specified date, Cartesian system type and eager loading settings.
        /// </summary>
        /// <param name="value">Coordinate string</param>
        /// <param name="geoDate">GeoDate</param>
        /// <param name="cartesianType">Cartesian Type</param>
        /// <param name="eagerLoad">Eager loading options</param>
        /// <returns>Coordinate</returns>
        /// <example>
        /// The following example parses an ECEF formatted coordinate string, with an included GeoDate. 
        /// Because this is an ECEF Cartesian type coordinate, we will specify the Cartesian system type.
        /// Eager loading options have been specified for efficiency.
        /// <code>
        /// EagerLoad el = new EagerLoad(EagerLoadType.Cartesian);
        /// Coordinate c = Coordinate.Parse("5242.097 km, 2444.43 km, 2679.074 km", new DateTime(2018,7,7), CartesianType.ECEF, el);    
        /// </code>
        /// </example>
        public static Coordinate Parse(string value, DateTime geoDate, CartesianType cartesianType, EagerLoad eagerLoad)
        {
            Coordinate coordinate = null;
            if (TryParse(value, geoDate, cartesianType, eagerLoad, out coordinate))
            {
                return coordinate;
            }

            throw new FormatException(string.Format("Input Coordinate \"{0}\" was not in a correct format.", value));
        }

        /// <summary>
        /// Parses a string into a Coordinate.
        /// </summary>
        /// <param name="value">Coordinate string</param>
        /// <param name="formats">Allowable parse formats</param>
        /// <returns>Coordinate</returns>
        /// <example>
        /// The following example parses a decimal degree formatted geodetic coordinate string. Parsable format is restricted to Lat/Long and MGRS.
        /// <code>
        /// Allowed_Parse_Format formats = Allowed_Parse_Format.Lat_Long | Allowed_Parse_Format.MGRS;
        /// Coordinate c = Coordinate.Parse("N 32.891º W 64.872º", formats);
        /// </code>
        /// </example>
        public static Coordinate Parse(string value, Allowed_Parse_Format formats)
        {
            Coordinate coordinate = null;
            if (TryParse(value, formats, out coordinate))
            {
                return coordinate;
            }

            throw new FormatException(string.Format("Input Coordinate \"{0}\" was not in a correct format.", value));
        }
        /// <summary>
        /// Parses a string into a Coordinate with specified date.
        /// </summary>
        /// <param name="value">Coordinate string</param>
        /// <param name="geoDate">GeoDate</param>
        /// <param name="formats">Allowable parse formats</param>
        /// <returns>Coordinate</returns>
        /// <example>
        /// The following example parses a decimal degree formatted geodetic coordinate string, with a provided GeoDate. Parsable format is restricted to Lat/Long and MGRS.
        /// <code>
        /// Allowed_Parse_Format formats = Allowed_Parse_Format.Lat_Long | Allowed_Parse_Format.MGRS;
        /// Coordinate c = Coordinate.Parse("N 32.891º W 64.872º", new DateTime(2018,7,7), formats);
        /// </code>
        /// </example>
        public static Coordinate Parse(string value, DateTime geoDate, Allowed_Parse_Format formats)
        {
            Coordinate coordinate = null;
            if (TryParse(value, geoDate, formats, out coordinate))
            {
                return coordinate;
            }

            throw new FormatException(string.Format("Input Coordinate \"{0}\" was not in a correct format.", value));
        }
        /// <summary>
        /// Parses a string into a Coordinate with a specified Cartesian system type.
        /// </summary>
        /// <param name="value">Coordinate string</param>
        /// <param name="cartesianType">Cartesian Type</param>
        /// <param name="formats">Allowable parse formats</param>
        /// <returns>Coordinate</returns>
        /// <example>
        /// The following example parses an ECEF formatted coordinate string. 
        /// Because this is an ECEF Cartesian type coordinate, we will specify the Cartesian system type. Parsable format is restricted to Lat/Long and MGRS.
        /// <code>
        /// Allowed_Parse_Format formats = Allowed_Parse_Format.Lat_Long | Allowed_Parse_Format.MGRS;
        /// Coordinate c = Coordinate.Parse("5242.097 km, 2444.43 km, 2679.074 km", CartesianType.Cartesian, formats);
        /// </code>
        /// </example>
        public static Coordinate Parse(string value, CartesianType cartesianType, Allowed_Parse_Format formats)
        {
            Coordinate coordinate = null;
            if (TryParse(value, cartesianType, formats, out coordinate))
            {
                return coordinate;
            }

            throw new FormatException(string.Format("Input Coordinate \"{0}\" was not in a correct format.", value));
        }
        /// <summary>
        /// Parses a string into a Coordinate with specified DateTime and Cartesian system type.
        /// </summary>
        /// <param name="value">Coordinate string</param>
        /// <param name="geoDate">GeoDate</param>
        /// <param name="cartesianType">Cartesian Type</param>
        /// <param name="formats">Allowed parse formats</param>
        /// <returns>Coordinate</returns>
        /// <example>
        /// The following example parses an ECEF formatted coordinate string, with an included GeoDate. 
        /// Because this is an ECEF Cartesian type coordinate, we will specify the Cartesian system type. Parsable format is restricted to Lat/Long and Cartesian/ECEF.
        /// <code>
        /// Allowed_Parse_Format formats = Allowed_Parse_Format.Lat_Long | Allowed_Parse_Format.Cartesian_ECEF;
        /// Coordinate c = Coordinate.Parse("5242.097 km, 2444.43 km, 2679.074 km", new DateTime(2018,7,7), CartesianType.ECEF, formats);
        /// </code>
        /// </example>
        public static Coordinate Parse(string value, DateTime geoDate, CartesianType cartesianType, Allowed_Parse_Format formats)
        {
            Coordinate coordinate = null;
            if (TryParse(value, geoDate, cartesianType, formats, out coordinate))
            {
                return coordinate;
            }

            throw new FormatException(string.Format("Input Coordinate \"{0}\" was not in a correct format.", value));
        }
        /// <summary>
        /// Parses a string into a Coordinate with specified eager loading settings.
        /// </summary>
        /// <param name="value">Coordinate string</param>
        /// <param name="eagerLoad">Eager loading options</param>
        /// <param name="formats">Allowed parse formats</param>
        /// <returns>Coordinate</returns>
        /// <example>
        /// The following example parses a decimal degree formatted geodetic coordinate string. Parsable format is restricted to Lat/Long and Cartesian/ECEF.
        /// Eager loading is turned off for improved efficiency.
        /// <code>
        /// EagerLoad el = new EagerLoad(false);
        /// Allowed_Parse_Format formats = Allowed_Parse_Format.Lat_Long | Allowed_Parse_Format.Cartesian_ECEF;
        /// Coordinate c = Coordinate.Parse("N 32.891º W 64.872º", el, formats);
        /// </code>
        /// </example>
        public static Coordinate Parse(string value, EagerLoad eagerLoad, Allowed_Parse_Format formats)
        {
            Coordinate coordinate = null;
            if (TryParse(value, eagerLoad, formats, out coordinate))
            {
                return coordinate;
            }

            throw new FormatException(string.Format("Input Coordinate \"{0}\" was not in a correct format.", value));
        }
        /// <summary>
        /// Parses a string into a Coordinate with a specified date and eager loading settings.
        /// </summary>
        /// <param name="value">Coordinate string</param>
        /// <param name="geoDate">GeoDate</param>
        /// <param name="eagerLoad">Eager loading options</param>
        /// <param name="formats">Allowed parse formats</param>
        /// <returns>Coordinate</returns>
        /// <example>
        /// The following example parses a decimal degree formatted geodetic coordinate string with a provided GeoDate. Parsable format is restricted to Lat/Long and Cartesian/ECEF.
        /// Eager loading is set to load celestial calculations only.
        /// <code>
        /// EagerLoad el = new EagerLoad(EagerLoadType.Celestial);
        /// Allowed_Parse_Format formats = Allowed_Parse_Format.Lat_Long | Allowed_Parse_Format.Cartesian_ECEF;
        /// Coordinate c = Coordinate.Parse("N 32.891º W 64.872º", new DateTime(2018,7,7), el, formats);       
        /// </code>
        /// </example>
        public static Coordinate Parse(string value, DateTime geoDate, EagerLoad eagerLoad, Allowed_Parse_Format formats)
        {
            Coordinate coordinate = null;
            if (TryParse(value, geoDate, eagerLoad, formats, out coordinate))
            {
                return coordinate;
            }

            throw new FormatException(string.Format("Input Coordinate \"{0}\" was not in a correct format.", value));
        }
        /// <summary>
        /// Parses a string into a Coordinate with a specified Cartesian system type and eager loading settings.
        /// </summary>
        /// <param name="value">Coordinate string</param>    
        /// <param name="cartesianType">Cartesian Type</param>
        /// <param name="eagerLoad">Eager loading options</param>
        /// <param name="formats">Allowed parse formats</param>
        /// <returns>Coordinate</returns>
        /// <example>
        /// The following example parses an ECEF formatted coordinate string. 
        /// Because this is an ECEF Cartesian type coordinate, we will specify the Cartesian system type. Parsable format is restricted to Lat/Long and Cartesian/ECEF.
        /// Eager loading options have been specified for efficiency.
        /// <code>      
        /// EagerLoad el = new EagerLoad(EagerLoadType.Cartesian);
        /// Allowed_Parse_Format formats = Allowed_Parse_Format.Lat_Long | Allowed_Parse_Format.Cartesian_ECEF;
        /// Coordinate c = Coordinate.Parse("5242.097 km, 2444.43 km, 2679.074 km", CartesianType.Cartesian, el, formats);
        /// </code>
        /// </example>
        public static Coordinate Parse(string value, CartesianType cartesianType, EagerLoad eagerLoad, Allowed_Parse_Format formats)
        {
            Coordinate coordinate = null;
            if (TryParse(value, cartesianType, eagerLoad, formats, out coordinate))
            {
                return coordinate;
            }

            throw new FormatException(string.Format("Input Coordinate \"{0}\" was not in a correct format.", value));
        }
        /// <summary>
        /// Parses a string into a Coordinate with a specified date, Cartesian system type and eager loading settings.
        /// </summary>
        /// <param name="value">Coordinate string</param>
        /// <param name="geoDate">GeoDate</param>
        /// <param name="cartesianType">Cartesian Type</param>
        /// <param name="eagerLoad">Eager loading options</param>
        /// <param name="formats">Allowed parse formats</param>
        /// <returns>Coordinate</returns>
        /// <example>
        /// The following example parses an ECEF formatted coordinate string, with an included GeoDate. 
        /// Because this is an ECEF Cartesian type coordinate, we will specify the Cartesian system type. Parsable format is restricted to Lat/Long and Cartesian/ECEF.
        /// Eager loading options have been specified for efficiency.
        /// <code>
        /// EagerLoad el = new EagerLoad(EagerLoadType.Cartesian);
        /// Allowed_Parse_Format formats = Allowed_Parse_Format.Lat_Long | Allowed_Parse_Format.Cartesian_ECEF;
        /// Coordinate c = Coordinate.Parse("5242.097 km, 2444.43 km, 2679.074 km", new DateTime(2018,7,7), CartesianType.ECEF, el);    
        /// </code>
        /// </example>
        public static Coordinate Parse(string value, DateTime geoDate, CartesianType cartesianType, EagerLoad eagerLoad, Allowed_Parse_Format formats)
        {
            Coordinate coordinate = null;
            if (TryParse(value, geoDate, cartesianType, eagerLoad, formats, out coordinate))
            {
                return coordinate;
            }

            throw new FormatException(string.Format("Input Coordinate \"{0}\" was not in a correct format.", value));
        }

        /*TRYPARSE METHODS*/

        /// <summary>
        /// Attempts to parse a string into a Coordinate.
        /// </summary>
        /// <param name="value">Coordinate string</param>
        /// <param name="coordinate">Coordinate</param>
        /// <returns>boolean</returns>
        /// <example>
        /// The following example parses a decimal degree formatted geodetic coordinate string.
        /// <code>
        /// Coordinate c;
        /// if(Coordinate.TryParse("N 32.891º W 64.872º", out c))
        /// {
        ///     Console.WriteLine(c); //N 32º 53' 28.212" W 64º 52' 20.914"
        /// }
        /// </code>
        /// </example>
        public static bool TryParse(string value, out Coordinate coordinate)
        {
            coordinate = null;
            if (FormatFinder.TryParse(value, GlobalSettings.Default_Cartesian_Type, GlobalSettings.Default_Parsable_Formats, out coordinate))
            {
                Parse_Format_Type pft = coordinate.Parse_Format;
                coordinate = new Coordinate(coordinate.Latitude.ToDouble(), coordinate.Longitude.ToDouble()); //Reset with EagerLoad default settings
                coordinate.parse_Format = pft;

                return true;
            }
            return false;
        }
        /// <summary>
        /// Attempts to parse a string into a Coordinate with specified date.
        /// </summary>
        /// <param name="value">Coordinate string</param>
        /// <param name="geoDate">GeoDate</param>
        /// <param name="coordinate">Coordinate</param>
        /// <returns>boolean</returns>
        /// <example>
        /// The following example parses a decimal degree formatted geodetic coordinate string, with a provided GeoDate. 
        /// <code>
        /// Coordinate c;
        /// if(Coordinate.TryParse("N 32.891º W 64.872º", new DateTime(2018,7,7), out c))
        /// {
        ///     Console.WriteLine(c); //N 32º 53' 28.212" W 64º 52' 20.914"
        /// }
        /// </code>
        /// </example>
        public static bool TryParse(string value, DateTime geoDate, out Coordinate coordinate)
        {
            coordinate = null;
            if (FormatFinder.TryParse(value, GlobalSettings.Default_Cartesian_Type, GlobalSettings.Default_Parsable_Formats, out coordinate))
            {
                Parse_Format_Type pft = coordinate.Parse_Format;
                coordinate = new Coordinate(coordinate.Latitude.ToDouble(), coordinate.Longitude.ToDouble(), geoDate); //Reset with EagerLoad default settings
                coordinate.parse_Format = pft;

                return true;
            }
            return false;
        }
        /// <summary>
        /// Attempts to parse a string into a Coordinate with a specified Cartesian system type.
        /// </summary>
        /// <param name="value">Coordinate string</param>
        /// <param name="cartesianType">Cartesian Type</param>
        /// <param name="coordinate">Coordinate</param>
        /// <returns>boolean</returns>
        /// <example>
        /// The following example parses an ECEF formatted coordinate string. 
        /// Because this is an ECEF Cartesian type coordinate, we will specify the Cartesian system type.
        /// <code>
        /// Coordinate c;
        /// if(Coordinate.TryParse("5242.097 km, 2444.43 km, 2679.074 km", CartesianType.Cartesian, out c))
        /// {
        ///     Console.WriteLine(c); //N 24º 59' 59.987" E 25º 0' 0.001"
        /// }
        /// </code>
        /// </example>
        public static bool TryParse(string value, CartesianType cartesianType, out Coordinate coordinate)
        {
            coordinate = null;
            if (FormatFinder.TryParse(value, cartesianType, GlobalSettings.Default_Parsable_Formats, out coordinate))
            {
                Parse_Format_Type pft = coordinate.Parse_Format;
                if (cartesianType == CartesianType.ECEF)
                {
                    Distance h = coordinate.ecef.GeoDetic_Height;
                    coordinate = new Coordinate(coordinate.Latitude.ToDouble(), coordinate.Longitude.ToDouble()); //Reset with EagerLoad default settings
                    if (coordinate.ecef != null)
                    {
                        coordinate.ecef.Set_GeoDetic_Height(coordinate, h);
                    }
                }
                else
                {
                    coordinate = new Coordinate(coordinate.Latitude.ToDouble(), coordinate.Longitude.ToDouble()); //Reset with EagerLoad default settings
                }
                coordinate.parse_Format = pft;

                return true;
            }
            return false;
        }
        /// <summary>
        /// Attempts to parse a string into a Coordinate with specified DateTime and Cartesian system type
        /// </summary>
        /// <param name="value">Coordinate string</param>
        /// <param name="geoDate">GeoDate</param>       
        /// <param name="cartesianType">Cartesian Type</param>
        /// <param name="coordinate">Coordinate</param>
        /// <returns>boolean</returns>
        /// <example>
        /// The following example parses an ECEF formatted coordinate string, with an included GeoDate. 
        /// Because this is an ECEF Cartesian type coordinate, we will specify the Cartesian system type.
        /// <code>
        /// Coordinate c;
        /// if(Coordinate.TryParse("5242.097 km, 2444.43 km, 2679.074 km", new DateTime(2018,7,7), CartesianType.ECEF, out c))
        /// {
        ///     Console.WriteLine(c); //N 24º 59' 59.987" E 25º 0' 0.001"
        /// }
        /// </code>
        /// </example>
        public static bool TryParse(string value, DateTime geoDate, CartesianType cartesianType, out Coordinate coordinate)
        {
            coordinate = null;
            if (FormatFinder.TryParse(value, cartesianType,     GlobalSettings.Default_Parsable_Formats, out coordinate))
            {
                Parse_Format_Type pft = coordinate.Parse_Format;
                if (cartesianType == CartesianType.ECEF)
                {
                    Distance h = coordinate.ecef.GeoDetic_Height;
                    coordinate = new Coordinate(coordinate.Latitude.ToDouble(), coordinate.Longitude.ToDouble(), geoDate); //Reset with EagerLoad default settings
                    if (coordinate.ecef != null)
                    {
                        coordinate.ecef.Set_GeoDetic_Height(coordinate, h);
                    }
                }
                else
                {
                    coordinate = new Coordinate(coordinate.Latitude.ToDouble(), coordinate.Longitude.ToDouble(), geoDate); //Reset with EagerLoad default setting
                }
                coordinate.parse_Format = pft;

                return true;
            }
            return false;
        }
        /// <summary>
        /// Attempts to parse a string into a Coordinate with specified eager loading settings.
        /// </summary>
        /// <param name="value">Coordinate string</param>
        /// <param name="eagerLoad">Eager loading options</param>
        /// <param name="coordinate">Coordinate</param>
        /// <returns>boolean</returns>
        /// <example>
        /// The following example parses a decimal degree formatted geodetic coordinate string.
        /// Eager loading is turned off for improved efficiency.
        /// <code>
        /// Coordinate c;
        /// EagerLoad el = new EagerLoad(false);
        /// if(Coordinate.TryParse("N 32.891º W 64.872º", el, out c))
        /// {
        ///     Console.WriteLine(c); //N 32º 53' 28.212" W 64º 52' 20.914"
        /// }
        /// </code>
        /// </example>
        public static bool TryParse(string value, EagerLoad eagerLoad, out Coordinate coordinate)
        {
            coordinate = null;
            if (FormatFinder.TryParse(value, GlobalSettings.Default_Cartesian_Type, GlobalSettings.Default_Parsable_Formats, out coordinate))
            {
                Parse_Format_Type pft = coordinate.Parse_Format;
                coordinate = new Coordinate(coordinate.Latitude.ToDouble(), coordinate.Longitude.ToDouble(), eagerLoad); //Reset with specified eager load options.
                coordinate.parse_Format = pft;
                coordinate.parse_Format = pft;

                return true;
            }
            return false;
        }
        /// <summary>
        /// Attempts to parse a string into a Coordinate with a specified date and eager loading settings.
        /// </summary>
        /// <param name="value">Coordinate string</param>
        /// <param name="geoDate">GeoDate</param>
        /// <param name="eagerLoad">Eager loading options</param>
        /// <param name="coordinate">Coordinate</param>
        /// <returns>boolean</returns>
        /// <example>
        /// The following example parses a decimal degree formatted geodetic coordinate string, with a provided GeoDate. 
        /// Eager loading is set to load celestial calculations only.
        /// <code>
        /// Coordinate c;
        /// EagerLoad el = new EagerLoad(EagerLoadType.Celestial);
        /// if(Coordinate.TryParse("N 32.891º W 64.872º", new DateTime(2018,7,7), el, out c))
        /// {
        ///     Console.WriteLine(c); //N 32º 53' 28.212" W 64º 52' 20.914"
        /// }
        /// </code>
        /// </example>
        public static bool TryParse(string value, DateTime geoDate, EagerLoad eagerLoad, out Coordinate coordinate)
        {
            coordinate = null;
            if (FormatFinder.TryParse(value, GlobalSettings.Default_Cartesian_Type, GlobalSettings.Default_Parsable_Formats, out coordinate))
            {
                Parse_Format_Type pft = coordinate.Parse_Format;
                coordinate = new Coordinate(coordinate.Latitude.ToDouble(), coordinate.Longitude.ToDouble(), geoDate, eagerLoad); //Reset with specified eager load options.
                coordinate.parse_Format = pft;

                return true;
            }
            return false;
        }
        /// <summary>
        /// Attempts to parse a string into a Coordinate with a specified Cartesian system type and eager loading settings.
        /// </summary>
        /// <param name="value">Coordinate string</param>    
        /// <param name="cartesianType">Cartesian Type</param>
        /// <param name="eagerLoad">Eager loading options</param>
        /// <param name="coordinate">Coordinate</param>
        /// <returns>boolean</returns>
        /// <example>
        /// The following example parses an ECEF formatted coordinate string. 
        /// Because this is an ECEF Cartesian type coordinate, we will specify the Cartesian system type.
        /// Eager loading options have been specified for efficiency.
        /// <code>
        /// Coordinate c;
        /// EagerLoad el = new EagerLoad(EagerLoadType.Cartesian);
        /// if(Coordinate.TryParse("5242.097 km, 2444.43 km, 2679.074 km", CartesianType.Cartesian, el, out c))
        /// {
        ///     Console.WriteLine(c); //N 24º 59' 59.987" E 25º 0' 0.001"
        /// }
        /// </code>
        /// </example>
        public static bool TryParse(string value, CartesianType cartesianType, EagerLoad eagerLoad, out Coordinate coordinate)
        {
            coordinate = null;
            if (FormatFinder.TryParse(value, cartesianType, GlobalSettings.Default_Parsable_Formats, out coordinate))
            {
                Parse_Format_Type pft = coordinate.Parse_Format;
                if (cartesianType == CartesianType.ECEF)
                {
                    Distance h = coordinate.ecef.GeoDetic_Height;
                    coordinate = new Coordinate(coordinate.Latitude.ToDouble(), coordinate.Longitude.ToDouble(), eagerLoad); //Reset with eager load options specified.
                    if (coordinate.ecef != null) 
                    { 
                        coordinate.ecef.Set_GeoDetic_Height(coordinate, h); 
                    }
                }
                else
                {
                    coordinate = new Coordinate(coordinate.Latitude.ToDouble(), coordinate.Longitude.ToDouble(), eagerLoad); //Reset with eager load options specified.
                }
                coordinate.parse_Format = pft;

                return true;
            }
            return false;
        }
        /// <summary>
        /// Attempts to parse a string into a Coordinate with a specified date, Cartesian system type and eager loading settings.
        /// </summary>
        /// <param name="value">Coordinate string</param>
        /// <param name="geoDate">GeoDate</param>
        /// <param name="cartesianType">Cartesian Type</param>
        /// <param name="eagerLoad">Eager loading options</param>
        /// <param name="coordinate">Coordinate</param>
        /// <returns>boolean</returns>
        /// <example>
        /// The following example parses an ECEF formatted coordinate string, with an included GeoDate. 
        /// Because this is an ECEF Cartesian type coordinate, we will specify the Cartesian system type.
        /// Eager loading options have been specified for efficiency.
        /// <code>
        /// Coordinate c;
        /// EagerLoad el = new EagerLoad(EagerLoadType.Cartesian);
        /// if(Coordinate.TryParse("5242.097 km, 2444.43 km, 2679.074 km", new DateTime(2018,7,7), CartesianType.ECEF, el, out c))
        /// {
        ///     Console.WriteLine(c); //N 24º 59' 59.987" E 25º 0' 0.001"
        /// }
        /// </code>
        /// </example>
        public static bool TryParse(string value, DateTime geoDate, CartesianType cartesianType, EagerLoad eagerLoad, out Coordinate coordinate)
        {
            coordinate = null;
            if (FormatFinder.TryParse(value, cartesianType, GlobalSettings.Default_Parsable_Formats, out coordinate))
            {
                Parse_Format_Type pft = coordinate.Parse_Format;
                if (cartesianType == CartesianType.ECEF)
                {
                    Distance h = coordinate.ecef.GeoDetic_Height;
                    coordinate = new Coordinate(coordinate.Latitude.ToDouble(), coordinate.Longitude.ToDouble(), geoDate, eagerLoad); //Reset with eager load options specified.
                    if (coordinate.ecef != null)
                    {
                        coordinate.ecef.Set_GeoDetic_Height(coordinate, h);
                    }
                }
                else
                {
                    coordinate = new Coordinate(coordinate.Latitude.ToDouble(), coordinate.Longitude.ToDouble(), geoDate, eagerLoad); //Reset with eager load options specified.
                }
                coordinate.parse_Format = pft;

                return true;
            }
            return false;
        }

        /// <summary>
        /// Attempts to parse a string into a Coordinate.
        /// </summary>
        /// <param name="value">Coordinate string</param>
        /// <param name="formats">Allowed parse formats</param>
        /// <param name="coordinate">Coordinate</param>
        /// <returns>boolean</returns>
        /// <example>
        /// The following example parses a decimal degree formatted geodetic coordinate string. Parsable format is restricted to Lat/Long and Cartesian/ECEF.
        /// <code>
        /// Coordinate c;
        /// Allowed_Parse_Format formats = Allowed_Parse_Format.Lat_Long | Allowed_Parse_Format.Cartesian_ECEF;
        /// if(Coordinate.TryParse("N 32.891º W 64.872º", formats, out c))
        /// {
        ///     Console.WriteLine(c); //N 32º 53' 28.212" W 64º 52' 20.914"
        /// }
        /// </code>
        /// </example>
        public static bool TryParse(string value, Allowed_Parse_Format formats, out Coordinate coordinate)
        {
            coordinate = null;
            if (FormatFinder.TryParse(value, GlobalSettings.Default_Cartesian_Type, formats, out coordinate))
            {
                Parse_Format_Type pft = coordinate.Parse_Format;
                coordinate = new Coordinate(coordinate.Latitude.ToDouble(), coordinate.Longitude.ToDouble()); //Reset with EagerLoad default settings
                coordinate.parse_Format = pft;

                return true;
            }
            return false;
        }
        /// <summary>
        /// Attempts to parse a string into a Coordinate with specified date.
        /// </summary>
        /// <param name="value">Coordinate string</param>
        /// <param name="geoDate">GeoDate</param>
        /// <param name="formats">Allowed parse formats</param>
        /// <param name="coordinate">Coordinate</param>
        /// <returns>boolean</returns>
        /// <example>
        /// The following example parses a decimal degree formatted geodetic coordinate string, with a provided GeoDate. Parsable format is restricted to Lat/Long and Cartesian/ECEF.
        /// <code>
        /// Coordinate c;
        /// Allowed_Parse_Format formats = Allowed_Parse_Format.Lat_Long | Allowed_Parse_Format.Cartesian_ECEF;
        /// if(Coordinate.TryParse("N 32.891º W 64.872º", new DateTime(2018,7,7), out c))
        /// {
        ///     Console.WriteLine(c); //N 32º 53' 28.212" W 64º 52' 20.914"
        /// }
        /// </code>
        /// </example>
        public static bool TryParse(string value, DateTime geoDate, Allowed_Parse_Format formats, out Coordinate coordinate)
        {
            coordinate = null;
            if (FormatFinder.TryParse(value, GlobalSettings.Default_Cartesian_Type, formats, out coordinate))
            {
                Parse_Format_Type pft = coordinate.Parse_Format;
                coordinate = new Coordinate(coordinate.Latitude.ToDouble(), coordinate.Longitude.ToDouble(), geoDate); //Reset with EagerLoad default settings
                coordinate.parse_Format = pft;

                return true;
            }
            return false;
        }

        /// <summary>
        /// Attempts to parse a string into a Coordinate with a specified Cartesian system type.
        /// </summary>
        /// <param name="value">Coordinate string</param>
        /// <param name="cartesianType">Cartesian Type</param>
        /// <param name="formats">Allowed parse formats</param>
        /// <param name="coordinate">Coordinate</param>
        /// <returns>boolean</returns>
        /// <example>
        /// The following example parses an ECEF formatted coordinate string. Parsable format is restricted to Lat/Long and Cartesian/ECEF.
        /// Because this is an ECEF Cartesian type coordinate, we will specify the Cartesian system type.
        /// <code>
        /// Coordinate c;
        /// Allowed_Parse_Format formats = Allowed_Parse_Format.Lat_Long | Allowed_Parse_Format.Cartesian_ECEF;
        /// if(Coordinate.TryParse("5242.097 km, 2444.43 km, 2679.074 km", CartesianType.Cartesian, out c))
        /// {
        ///     Console.WriteLine(c); //N 24º 59' 59.987" E 25º 0' 0.001"
        /// }
        /// </code>
        /// </example>
        public static bool TryParse(string value, CartesianType cartesianType, Allowed_Parse_Format formats, out Coordinate coordinate)
        {
            coordinate = null;
            if (FormatFinder.TryParse(value, cartesianType, formats, out coordinate))
            {
                Parse_Format_Type pft = coordinate.Parse_Format;
                if (cartesianType == CartesianType.ECEF)
                {
                    Distance h = coordinate.ecef.GeoDetic_Height;
                    coordinate = new Coordinate(coordinate.Latitude.ToDouble(), coordinate.Longitude.ToDouble()); //Reset with EagerLoad default settings
                    if (coordinate.ecef != null)
                    {
                        coordinate.ecef.Set_GeoDetic_Height(coordinate, h);
                    }
                }
                else
                {
                    coordinate = new Coordinate(coordinate.Latitude.ToDouble(), coordinate.Longitude.ToDouble()); //Reset with EagerLoad default settings
                }
                coordinate.parse_Format = pft;

                return true;
            }
            return false;
        }

        /// <summary>
        /// Attempts to parse a string into a Coordinate with specified DateTime and Cartesian system type
        /// </summary>
        /// <param name="value">Coordinate string</param>
        /// <param name="geoDate">GeoDate</param>       
        /// <param name="cartesianType">Cartesian Type</param>
        /// <param name="formats">Allowed parse formats</param>
        /// <param name="coordinate">Coordinate</param>
        /// <returns>boolean</returns>
        /// <example>
        /// The following example parses an ECEF formatted coordinate string, with an included GeoDate. Parsable format is restricted to Lat/Long and Cartesian/ECEF.
        /// Because this is an ECEF Cartesian type coordinate, we will specify the Cartesian system type.
        /// <code>
        /// Coordinate c;
        /// Allowed_Parse_Format formats = Allowed_Parse_Format.Lat_Long | Allowed_Parse_Format.Cartesian_ECEF;
        /// if(Coordinate.TryParse("5242.097 km, 2444.43 km, 2679.074 km", new DateTime(2018,7,7), CartesianType.ECEF, out c))
        /// {
        ///     Console.WriteLine(c); //N 24º 59' 59.987" E 25º 0' 0.001"
        /// }
        /// </code>
        /// </example>
        public static bool TryParse(string value, DateTime geoDate, CartesianType cartesianType, Allowed_Parse_Format formats, out Coordinate coordinate)
        {
            coordinate = null;
            if (FormatFinder.TryParse(value, cartesianType, formats, out coordinate))
            {
                Parse_Format_Type pft = coordinate.Parse_Format;
                if (cartesianType == CartesianType.ECEF)
                {
                    Distance h = coordinate.ecef.GeoDetic_Height;
                    coordinate = new Coordinate(coordinate.Latitude.ToDouble(), coordinate.Longitude.ToDouble(), geoDate); //Reset with EagerLoad default settings
                    if (coordinate.ecef != null)
                    {
                        coordinate.ecef.Set_GeoDetic_Height(coordinate, h);
                    }
                }
                else
                {
                    coordinate = new Coordinate(coordinate.Latitude.ToDouble(), coordinate.Longitude.ToDouble(), geoDate); //Reset with EagerLoad default setting
                }
                coordinate.parse_Format = pft;

                return true;
            }
            return false;
        }
        /// <summary>
        /// Attempts to parse a string into a Coordinate with specified eager loading settings.
        /// </summary>
        /// <param name="value">Coordinate string</param>
        /// <param name="eagerLoad">Eager loading options</param>
        /// <param name="formats">Allowed parse formats</param>
        /// <param name="coordinate">Coordinate</param>
        /// <returns>boolean</returns>
        /// <example>
        /// The following example parses a decimal degree formatted geodetic coordinate string. Parsable format is restricted to Lat/Long and Cartesian/ECEF.
        /// Eager loading is turned off for improved efficiency.
        /// <code>
        /// Coordinate c;
        /// EagerLoad el = new EagerLoad(false);
        /// Allowed_Parse_Format formats = Allowed_Parse_Format.Lat_Long | Allowed_Parse_Format.Cartesian_ECEF;
        /// if(Coordinate.TryParse("N 32.891º W 64.872º", el, out c))
        /// {
        ///     Console.WriteLine(c); //N 32º 53' 28.212" W 64º 52' 20.914"
        /// }
        /// </code>
        /// </example>
        public static bool TryParse(string value, EagerLoad eagerLoad, Allowed_Parse_Format formats, out Coordinate coordinate)
        {
            coordinate = null;
            if (FormatFinder.TryParse(value, GlobalSettings.Default_Cartesian_Type, formats, out coordinate))
            {
                Parse_Format_Type pft = coordinate.Parse_Format;
                coordinate = new Coordinate(coordinate.Latitude.ToDouble(), coordinate.Longitude.ToDouble(), eagerLoad); //Reset with specified eager load options.
                coordinate.parse_Format = pft;
                coordinate.parse_Format = pft;

                return true;
            }
            return false;
        }
        /// <summary>
        /// Attempts to parse a string into a Coordinate with a specified date and eager loading settings.
        /// </summary>
        /// <param name="value">Coordinate string</param>
        /// <param name="geoDate">GeoDate</param>
        /// <param name="eagerLoad">Eager loading options</param>
        /// <param name="formats">Allowed parse formats</param>
        /// <param name="coordinate">Coordinate</param>
        /// <returns>boolean</returns>
        /// <example>
        /// The following example parses a decimal degree formatted geodetic coordinate string, with a provided GeoDate. Parsable format is restricted to Lat/Long and Cartesian/ECEF. 
        /// Eager loading is set to load celestial calculations only.
        /// <code>
        /// Coordinate c;
        /// EagerLoad el = new EagerLoad(EagerLoadType.Celestial);
        /// Allowed_Parse_Format formats = Allowed_Parse_Format.Lat_Long | Allowed_Parse_Format.Cartesian_ECEF;
        /// if(Coordinate.TryParse("N 32.891º W 64.872º", new DateTime(2018,7,7), el, out c))
        /// {
        ///     Console.WriteLine(c); //N 32º 53' 28.212" W 64º 52' 20.914"
        /// }
        /// </code>
        /// </example>
        public static bool TryParse(string value, DateTime geoDate, EagerLoad eagerLoad, Allowed_Parse_Format formats, out Coordinate coordinate)
        {
            coordinate = null;
            if (FormatFinder.TryParse(value, GlobalSettings.Default_Cartesian_Type, formats, out coordinate))
            {
                Parse_Format_Type pft = coordinate.Parse_Format;
                coordinate = new Coordinate(coordinate.Latitude.ToDouble(), coordinate.Longitude.ToDouble(), geoDate, eagerLoad); //Reset with specified eager load options.
                coordinate.parse_Format = pft;

                return true;
            }
            return false;
        }
        /// <summary>
        /// Attempts to parse a string into a Coordinate with a specified Cartesian system type and eager loading settings.
        /// </summary>
        /// <param name="value">Coordinate string</param>    
        /// <param name="cartesianType">Cartesian Type</param>
        /// <param name="eagerLoad">Eager loading options</param>
        /// <param name="formats">Allowed parse formats</param>
        /// <param name="coordinate">Coordinate</param>
        /// <returns>boolean</returns>
        /// <example>
        /// The following example parses an ECEF formatted coordinate string. 
        /// Because this is an ECEF Cartesian type coordinate, we will specify the Cartesian system type. Parsable format is restricted to Lat/Long and Cartesian/ECEF.
        /// Eager loading options have been specified for efficiency.
        /// <code>
        /// Coordinate c;
        /// EagerLoad el = new EagerLoad(EagerLoadType.Cartesian);
        /// Allowed_Parse_Format formats = Allowed_Parse_Format.Lat_Long | Allowed_Parse_Format.Cartesian_ECEF;
        /// if(Coordinate.TryParse("5242.097 km, 2444.43 km, 2679.074 km", CartesianType.Cartesian, el, out c))
        /// {
        ///     Console.WriteLine(c); //N 24º 59' 59.987" E 25º 0' 0.001"
        /// }
        /// </code>
        /// </example>
        public static bool TryParse(string value, CartesianType cartesianType, EagerLoad eagerLoad, Allowed_Parse_Format formats, out Coordinate coordinate)
        {
            coordinate = null;
            if (FormatFinder.TryParse(value, cartesianType, formats, out coordinate))
            {
                Parse_Format_Type pft = coordinate.Parse_Format;
                if (cartesianType == CartesianType.ECEF)
                {
                    Distance h = coordinate.ecef.GeoDetic_Height;
                    coordinate = new Coordinate(coordinate.Latitude.ToDouble(), coordinate.Longitude.ToDouble(), eagerLoad); //Reset with eager load options specified.
                    if (coordinate.ecef != null)
                    {
                        coordinate.ecef.Set_GeoDetic_Height(coordinate, h);
                    }
                }
                else
                {
                    coordinate = new Coordinate(coordinate.Latitude.ToDouble(), coordinate.Longitude.ToDouble(), eagerLoad); //Reset with eager load options specified.
                }
                coordinate.parse_Format = pft;

                return true;
            }
            return false;
        }
        /// <summary>
        /// Attempts to parse a string into a Coordinate with a specified date, Cartesian system type and eager loading settings.
        /// </summary>
        /// <param name="value">Coordinate string</param>
        /// <param name="geoDate">GeoDate</param>
        /// <param name="cartesianType">Cartesian Type</param>
        /// <param name="eagerLoad">Eager loading options</param>
        /// <param name="formats">Allowed parse formats</param>
        /// <param name="coordinate">Coordinate</param>
        /// <returns>boolean</returns>
        /// <example>
        /// The following example parses an ECEF formatted coordinate string, with an included GeoDate. Parsable format is restricted to Lat/Long and Cartesian/ECEF.
        /// Because this is an ECEF Cartesian type coordinate, we will specify the Cartesian system type.
        /// Eager loading options have been specified for efficiency.
        /// <code>
        /// Coordinate c;
        /// EagerLoad el = new EagerLoad(EagerLoadType.Cartesian);
        /// Allowed_Parse_Format formats = Allowed_Parse_Format.Lat_Long | Allowed_Parse_Format.Cartesian_ECEF;
        /// if(Coordinate.TryParse("5242.097 km, 2444.43 km, 2679.074 km", new DateTime(2018,7,7), CartesianType.ECEF, el, out c))
        /// {
        ///     Console.WriteLine(c); //N 24º 59' 59.987" E 25º 0' 0.001"
        /// }
        /// </code>
        /// </example>
        public static bool TryParse(string value, DateTime geoDate, CartesianType cartesianType, EagerLoad eagerLoad, Allowed_Parse_Format formats, out Coordinate coordinate)
        {
            coordinate = null;
            if (FormatFinder.TryParse(value, cartesianType, formats, out coordinate))
            {
                Parse_Format_Type pft = coordinate.Parse_Format;
                if (cartesianType == CartesianType.ECEF)
                {
                    Distance h = coordinate.ecef.GeoDetic_Height;
                    coordinate = new Coordinate(coordinate.Latitude.ToDouble(), coordinate.Longitude.ToDouble(), geoDate, eagerLoad); //Reset with eager load options specified.
                    if (coordinate.ecef != null)
                    {
                        coordinate.ecef.Set_GeoDetic_Height(coordinate, h);
                    }
                }
                else
                {
                    coordinate = new Coordinate(coordinate.Latitude.ToDouble(), coordinate.Longitude.ToDouble(), geoDate, eagerLoad); //Reset with eager load options specified.
                }
                coordinate.parse_Format = pft;

                return true;
            }
            return false;
        }
    }
}
