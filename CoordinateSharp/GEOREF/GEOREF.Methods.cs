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
using System.Globalization;
using System.Linq;

namespace CoordinateSharp
{
    public partial class GEOREF
    {
     

        /// <summary>
        /// Creates a GEOREF.
        /// </summary>      
        /// <param name="quad_15">15 Degree Quadrangle</param>
        /// <param name="quad_1">1 Degree Quadrangle</param>
        /// <param name="easting">Easting</param>
        /// <param name="northing">Northing</param>
        /// <example>
        /// <code>
        /// GEOREF geo = new GEOREF("SK","FB", 145200, 367200);
        /// </code>
        /// </example>
        public GEOREF(string quad_15, string quad_1, string easting, string northing)       
        {
            if (quad_1 == null) { quad_1 = "AA"; }

            if (easting == null) { easting = ""; }
            if (northing == null) { northing = ""; }

            Validate_GEOREF(quad_15,quad_1,easting,northing);
            
            Quad_15 = quad_15.ToUpper();
            Quad_1 = quad_1.ToUpper();

         

            Easting = easting.PadRight(maxprec, '0');
            Northing = northing.PadRight(maxprec, '0');
        }

        /// <summary>
        /// Validates GEOREF parameters
        /// </summary>
        /// <param name="quad_15">15 Degree Quadrangle</param>
        /// <param name="quad_1">1 Degree Quadrangle</param>
        /// <param name="easting">Easting</param>
        /// <param name="northing">Northing</param>
        /// <exception cref="FormatException">Format Exception</exception>
        private static void Validate_GEOREF(string quad_15, string quad_1, string easting, string northing)
        {
            //ADD REGEX VALIDATION


            if (quad_15.Length != 2) { throw new FormatException(); }
            if (quad_1.Length != 2) { throw new FormatException(); }

            if(easting.Length != northing.Length) { throw new FormatException("Easting and Northing length/precision must match."); }

            if (!lngTile.Contains(quad_15[0].ToString())) { throw new FormatException("15 degree quadrangle is invalid."); }
            if (!latTile.Contains(quad_15[1].ToString())) { throw new FormatException("15 degree quadrangle is invalid."); }
            if (!degrees.Contains(quad_1[0].ToString())) { throw new FormatException("1 degree quadrangle is invalid."); }
            if (!degrees.Contains(quad_1[1].ToString())) { throw new FormatException("1 degree quadrangle is invalid."); }

            //Ensure easting and northing are ints
            if (!string.IsNullOrWhiteSpace(easting) && !long.TryParse(easting, out long i)) { throw new FormatException("Easting value is invalid."); }
            if (!string.IsNullOrWhiteSpace(easting) && !long.TryParse(northing, out long n)) { throw new FormatException("Northing value is invalid."); }

            //Ensure easting and northing are at least 2 digits if not empty
            if (easting.Length == 1) { throw new FormatException("Easting value is invalid and must begin with a 2 digit value or be omitted."); } //Must be 2 digits for degree per spec.
            if (northing.Length == 1) { throw new FormatException("Northing value is invalid and must begin with a 2 digit value or be omitted."); } //Must be 2 digits for degree per spec.

            //Ensure Minutes less than 60
            int firstEDigit = 0;
            int firstNDigit = 0;

            if (!string.IsNullOrWhiteSpace(easting)) { firstEDigit = int.Parse(easting[0].ToString()); }
            if (!string.IsNullOrWhiteSpace(northing)) { firstNDigit = int.Parse(northing[0].ToString()); }

            //Restrict to 59.9~ degrees
            if (firstEDigit >= 6) { throw new FormatException("Easting value is invalid."); }
            if (firstNDigit >= 6) { throw new FormatException("Northing value is invalid."); }

            //Ensure max precision not exceeded
            if (easting.Length > maxprec) { throw new FormatException("Max precision allowed exceeded."); }
            if (northing.Length > maxprec) { throw new FormatException("Max precision allowed exceeded."); }
        }

        /// <summary>
        /// Constructs a GEOREF object based off signed lat/long
        /// </summary>
        /// <param name="lat">Signed Latitude</param>
        /// <param name="lng">Signed Longitude</param>
        internal GEOREF(double lat, double lng)
        {
            ToGEOREF(lat, lng, this);
        }

        /// <summary>
        /// Assigns GEOREF values based on Lat/Long.
        /// </summary>
        /// <param name="lat">Signed Latitude</param>
        /// <param name="lng">Signed longitude</param>  
        /// <param name="georef">GEOREF</param>
        internal void ToGEOREF(double lat, double lng, GEOREF georef)
        {
            string easting = "";
            string northing = "";

            if (lat == 90)
            {
                lat = lat - double.Epsilon;
            }
            if (lat == -90)
            {
                lat = lat + double.Epsilon;
            }

            long m = 60000000000;

            long x = (long)(Math.Floor(lng * m) - lngorig * m);
            long y = (long)(Math.Floor(lat * m) - latorig * m);
            long ilon = (long)(x / m);
            long ilat = (long)(y / m);

            georef.Quad_15 = lngTile[(int)(ilon / tile)].ToString() + latTile[(int)(ilat / tile)].ToString();

            georef.Quad_1 = degrees[(int)(ilon % tile)].ToString() + degrees[(int)(ilat % tile)].ToString();


            x = (long)(x - m * ilon);
            y = (long)(y - m * ilat);
          
            int c = maxprec;

            while (c > 0)
            {
                easting = easting.Insert(0, digits[Math.Abs((int)(x % based))].ToString());
                x = (long)(x / based);

                northing = northing.Insert(0, digits[Math.Abs((int)(y % based))].ToString());
                y = (long)(y / based);

                c -= 1;
            }

            georef.Easting = easting;
            georef.Northing = northing;
        }

        /// <summary>
        /// Default formatted GEOREF string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            //Default output precision
            int easting_precision = 6;
            int northing_precision = 6;

            if (easting.Length < easting_precision) { easting_precision = easting.Length; }
            if (northing.Length < northing_precision) { northing_precision = northing.Length; }

            return $"{quad_15}{quad_1}{easting.Substring(0, easting_precision)}{northing.Substring(0, northing_precision)}";
        }

        /// <summary>
        /// GEOREF string with desired precision.
        /// </summary>
        /// <param name="precision">Precision value</param>
        /// <returns>string</returns>
        /// <exception cref="ArgumentOutOfRangeException">ArgumentOutOfRangeException</exception>
        public string ToString(int precision)
        {
            
            if (precision < 0) { throw new ArgumentOutOfRangeException("Precision may not be less than 0."); }
            if (precision >= maxprec) { throw new ArgumentOutOfRangeException($"Precision may not be greater than {maxprec-1}."); }
            //if(max_precision % 2 != 0) { throw new ArgumentOutOfRangeException($"Max be an equal number."); }

            if (precision == 1) { precision = 2; }//Minute must be 2 digit.

            int easting_precision = precision;
            int northing_precision = precision;

            if (easting.Length < easting_precision) { easting_precision = easting.Length; }
            if (northing.Length < northing_precision) { northing_precision = northing.Length; }

            return $"{quad_15}{quad_1}{easting.Substring(0, easting_precision)}{northing.Substring(0, northing_precision)}";
        }

        private static Coordinate GEOREFtoLatLong(string quad_15, string quad_1, string easting, string northing, EagerLoad el)
        {
            double[] d = GEOREFtoSigned(quad_15, quad_1, easting, northing);
            return new Coordinate(d[0], d[1], el);

        }

        private static double[] GEOREFtoSigned(string quad_15, string quad_1, string easting, string northing)
        {
            if (easting == null) { easting = ""; }
            if (northing == null) { northing = ""; }

            easting = easting.PadRight(maxprec, '0');
            northing = northing.PadRight(maxprec, '0');

            double lon1 = lngTile.IndexOf(quad_15[0]) + lngorig / tile;
            double lat1 = latTile.IndexOf(quad_15[1]) + latorig / tile;

            double unit = 1;

            unit = unit * tile;

            lon1 = lon1 * tile + degrees.IndexOf(quad_1[0]);
            lat1 = lat1 * tile + degrees.IndexOf(quad_1[1]);

            int i = 0;

            while (i < maxprec)
            {
                double m = based;
                if (i == 0) { m = 6; }

                unit = unit * m;
                int x = digits.IndexOf(easting[i].ToString());
                int y = digits.IndexOf(northing[i].ToString());

                lon1 = m * lon1 + x;
                lat1 = m * lat1 + y;

                i++;
            }

            double lat = (tile * lat1) / unit;
            double lon = (tile * lon1) / unit;

            if (lat > 90) { lat = 90; }
            if (lat < -90) { lat = -90; }
            if(lon > 180) { lngorig = 180; }
            if (lon < -180) { lngorig = -180; }

            return new double[] { lat, lon };
        }

        /// <summary>
        /// Converts GEOREF to Lat/Long Coordinate.
        /// </summary>
        /// <param name="georef">GEOREF</param>
        /// <returns>Coordinate</returns>
        /// <example>
        /// The following example creates (converts to) a geodetic Coordinate object based on a GEOREF object.
        /// <code>
        /// GEOREF geo = new GEOREF("SK", "FB", 145200, 367200);
        /// c = GEOREF.ConvertGEOREFtoLatLong(geo);
        /// Console.WriteLine(c); //N 46º 36' 43.2" E 65º 14' 31.2"
        /// </code>
        /// </example>
        public static Coordinate ConvertGEOREFtoLatLong(GEOREF georef)
        {
            return GEOREFtoLatLong(georef.quad_15, georef.quad_1, georef.easting, georef.northing, GlobalSettings.Default_EagerLoad);
        }


        /// <summary>
        /// Converts GEOREF to Lat/Long Coordinate.
        /// </summary>
        /// <param name="georef">GEOREF</param>
        /// <param name="el">EagerLoad</param>
        /// <returns>Coordinate</returns>
        /// <example>
        /// The following example creates (converts to) a geodetic Coordinate object based on a GEOREF object.
        /// Performance is maximized by turning off EagerLoading.
        /// <code>
        /// EagerLoad el = new EagerLoad(false);
        /// GEOREF geo = new GEOREF("SK", "FB", 145200, 367200);
        /// c = GEOREF.ConvertGEOREFtoLatLong(geo, el);
        /// Console.WriteLine(c); //N 46º 36' 43.2" E 65º 14' 31.2"
        /// </code>
        /// </example>
        public static Coordinate ConvertGEOREFtoLatLong(GEOREF georef, EagerLoad el)
        {
            return GEOREFtoLatLong(georef.quad_15, georef.quad_1, georef.easting, georef.northing, el);
        }

        /// <summary>
        /// Converts GEOREF to Lat/Long Coordinate.
        /// </summary>
        /// <param name="quad_15">15 Degree Quadrangle</param>
        /// <param name="quad_1">1 Degree Quadrangle</param>
        /// <param name="easting">Easting</param>
        /// <param name="northing">1Northing</param>
        /// <returns>Coordinate</returns>
        /// <example>
        /// The following example creates (converts to) a geodetic Coordinate object based on a GEOREF object.
        /// <code>
        /// c = GEOREF.ConvertGEOREFtoLatLong("SK", "FB", 145200, 367200);
        /// Console.WriteLine(c); //N 46º 36' 43.2" E 65º 14' 31.2"
        /// </code>
        /// </example>
        public static Coordinate ConvertGEOREFtoLatLong(string quad_15, string quad_1, string easting, string northing)
        {
            Validate_GEOREF(quad_15, quad_1, easting, northing);
            return GEOREFtoLatLong(quad_15, quad_1, easting, northing, GlobalSettings.Default_EagerLoad);
        }

        /// <summary>
        /// Converts GEOREF to Lat/Long Coordinate.
        /// </summary>
        /// <param name="quad_15">15 Degree Quadrangle</param>
        /// <param name="quad_1">1 Degree Quadrangle</param>
        /// <param name="easting">Easting</param>
        /// <param name="northing">1Northing</param>
        /// <param name="el">EagerLoad</param>
        /// <returns>Coordinate</returns>
        /// <example>
        /// The following example creates (converts to) a geodetic Coordinate object based on a GEOREF object.
        /// Performance is maximized by turning off EagerLoading.
        /// <code>
        /// EagerLoad el = new EagerLoad(false);
        /// c = GEOREF.ConvertGEOREFtoLatLong("SK", "FB", 145200, 367200, el);
        /// Console.WriteLine(c); //N 46º 36' 43.2" E 65º 14' 31.2"
        /// </code>
        /// </example>
        public static Coordinate ConvertGEOREFtoLatLong(string quad_15, string quad_1, string easting, string northing, EagerLoad el)
        {
            Validate_GEOREF(quad_15, quad_1, easting, northing);
            return GEOREFtoLatLong(quad_15, quad_1, easting, northing, el);
        }

        /// <summary>
        /// Converts GEOREF to Signed Degree Lat/Long double array.
        /// </summary>
        /// <param name="georef">GEOREF</param>
        /// <returns>double[lat, lng]</returns>
        /// <example>
        /// The following example creates (converts to) a geodetic lat / long double array object based on a GEOREF object.
        /// <code>
        /// GEOREF geo = new GEOREF("SK", "FB", 145200, 367200);
        /// double[] signed = GEOREF.ConvertGEOREFtoSignedDegree(geo);
        /// Coordinate c = new Coordinate(signed[0], signed[1], new EagerLoad(false));
        /// Console.WriteLine(c); //N 46º 36' 43.2" E 65º 14' 31.2"
        /// </code>
        /// </example>
        public static double[] ConvertGEOREFtoSignedDegree(GEOREF georef)
        {
            double[] signed = GEOREFtoSigned(georef.quad_15, georef.quad_1, georef.easting, georef.northing);
            return signed;
        }


        #region GeoFence Methods  
        /// <summary>
        /// Creates a GeoFence box based on the specified precision of the GEOREF coordinates. May not be reliable at poles.
        /// </summary>
        /// <remarks>
        /// <para>Precision Levels</para>
        /// <para>0: Quad 15 corners</para>
        /// <para>1-2: Quad 1 corners</para>
        /// <para>3-4: Easting and Northing minute corners</para>
        /// <para>5-11: Easting and Northing seconds corners</para>
        /// </remarks>
        /// <param name="precision">Precision</param>
        /// <returns>GeoFence</returns>
        public GeoFence ToGeoFence(int precision)
        {
            precision = Set_Precision(precision);

            GEOREF bl = Get_BottomLeftCorner(precision);
            GEOREF br = Get_BottomRightCorner(precision);
            GEOREF tr = Get_TopRightCorner(precision);
            GEOREF tl = Get_TopLeftCorner(precision);

            List<Coordinate> coordinates = new List<Coordinate>()
            {
                GEOREF.ConvertGEOREFtoLatLong(bl,new EagerLoad(EagerLoadType.GEOREF)),
                GEOREF.ConvertGEOREFtoLatLong(br,new EagerLoad(EagerLoadType.GEOREF)),
                GEOREF.ConvertGEOREFtoLatLong(tr,new EagerLoad(EagerLoadType.GEOREF)),
                GEOREF.ConvertGEOREFtoLatLong(tl,new EagerLoad(EagerLoadType.GEOREF)),
                GEOREF.ConvertGEOREFtoLatLong(bl,new EagerLoad(EagerLoadType.GEOREF)),//Close box
            };

            GeoFence geoFence = new GeoFence(coordinates);
         
            return geoFence;
        }

        /// <summary>
        /// Gets the Bottom Left corner of the GEOREF coordinate based on the specified precision.        
        /// </summary>
        /// <remarks>
        /// <para>Precision Levels</para>
        /// <para>0: Quad 15 corners</para>
        /// <para>1-2: Quad 1 corners</para>
        /// <para>3-4: Easting and Northing minute corners</para>
        /// <para>5-11: Easting and Northing seconds corners</para>
        /// </remarks>
        /// <param name="precision">Precision</param>
        /// <returns>GEOREF</returns>
        public GEOREF Get_BottomLeftCorner(int precision)
        {
            precision = Set_Precision(precision);
            GEOREF geo = new GEOREF(quad_15, quad_1, easting, northing);
            geo.Shift_GeoRef(precision, false, false);
            
            if (precision < 4)
            {
                geo.easting = "0".PadRight(maxprec, '0');
                geo.northing = "0".PadRight(maxprec, '0');
                if(precision < 2)
                {
                    geo.quad_1 = "AA";
                }

                return geo;
            }
            else
            {
                return new GEOREF(quad_15, quad_1, Easting.Substring(0, precision-2), Northing.Substring(0, precision-2));
            }

           
        }

        /// <summary>
        /// Gets the Top Left corner of the GEOREF coordinate based on the specified precision.
        /// </summary>
        /// <remarks>
        /// <para>Precision Levels</para>
        /// <para>0: Quad 15 corners</para>
        /// <para>1-2: Quad 1 corners</para>
        /// <para>3-4: Easting and Northing minute corners</para>
        /// <para>5-11: Easting and Northing seconds corners</para>
        /// </remarks>
        /// <param name="precision">Precision</param>
        /// <returns>GEOREF</returns>
        public GEOREF Get_TopLeftCorner(int precision)
        {
            precision = Set_Precision(precision);
            GEOREF geo = new GEOREF(quad_15, quad_1, easting, northing);
            geo.Shift_GeoRef(precision, true, false);
            if (precision < 4)
            {
                geo.easting = "0".PadRight(maxprec, '0');
                geo.northing = "0".PadRight(maxprec, '0');
                if (precision < 2)
                {
                    geo.quad_1 = "AA";
                }
            }
            else
            {
                geo.easting = geo.easting.Substring(0, precision - 2).PadRight(maxprec, '0');
                geo.northing = geo.northing.Substring(0, precision - 2).PadRight(maxprec, '0');
            }
            return geo;
        }
        /// <summary>
        /// Gets the Bottom Right corner of the GEOREF coordinate based on the specified precision.
        /// </summary>
        /// <remarks>
        /// <para>Precision Levels</para>
        /// <para>0: Quad 15 corners</para>
        /// <para>1-2: Quad 1 corners</para>
        /// <para>3-4: Easting and Northing minute corners</para>
        /// <para>5-11: Easting and Northing seconds corners</para>
        /// </remarks>
        /// <param name="precision">Precision</param>
        /// <returns>GEOREF</returns>      
        public GEOREF Get_BottomRightCorner(int precision)
        {
            precision = Set_Precision(precision);
            GEOREF geo = new GEOREF(quad_15, quad_1, easting, northing);
            geo.Shift_GeoRef(precision, false, true);
            if (precision < 4)
            {
                geo.easting = "0".PadRight(maxprec, '0');
                geo.northing = "0".PadRight(maxprec, '0');
                if (precision < 2)
                {
                    geo.quad_1 = "AA";
                }
            }
            else
            {
                geo.easting = geo.easting.Substring(0, precision - 2).PadRight(maxprec, '0');
                geo.northing = geo.northing.Substring(0, precision - 2).PadRight(maxprec, '0');
            }
            return geo;
        }
        /// <summary>
        /// Gets the Top Right corner of the GEOREF coordinate based on the specified precision.
        /// </summary>
        /// <remarks>
        /// <para>Precision Levels</para>
        /// <para>0: Quad 15 corners</para>
        /// <para>1-2: Quad 1 corners</para>
        /// <para>3-4: Easting and Northing minute corners</para>
        /// <para>5-11: Easting and Northing seconds corners</para>
        /// </remarks>
        /// <param name="precision">Precision</param>
        /// <returns>GEOREF</returns>
        public GEOREF Get_TopRightCorner(int precision)
        {
            precision = Set_Precision(precision);
            GEOREF geo = new GEOREF(quad_15, quad_1, easting, northing);
            geo.Shift_GeoRef(precision, true, true);
            if (precision < 4)
            {
                geo.easting = "0".PadRight(maxprec, '0');
                geo.northing = "0".PadRight(maxprec, '0');
                if (precision < 2)
                {
                    geo.quad_1 = "AA";
                }
            }
            else
            {
                geo.easting = geo.easting.Substring(0, precision - 2).PadRight(maxprec, '0');
                geo.northing = geo.northing.Substring(0, precision - 2).PadRight(maxprec, '0');
            }
            return geo;
        }
      
       
     
        private void Shift_GeoRef(int precision, bool latB, bool lngB)
        {         
            string lat = quad_15[1].ToString();
            string lng = quad_15[0].ToString();

            if (latB)
            {
                bool quad_1_Lat = false;
                if (precision >= 2)
                {
                    quad_1_Lat = Shift_Quad_1(precision, true, false);
                }
                if (quad_1_Lat || precision < 2)
                {
                    lat = Shift_Quad_15_Lat(quad_15[1]);
                    //quad_1 = "A" + quad_1[1];
                   northing = "0".PadRight(maxprec, '0');
                }
            }

            if (lngB)
            {
                bool quad_1_Lng = false;
                if (precision >= 2)
                {
                    quad_1_Lng = Shift_Quad_1(precision, false, true);
                }
                if (quad_1_Lng || precision < 2)
                {
                    lng = Shift_Quad_15_Lng(quad_15[0]);
                    //quad_1 = quad_1[0] + "A";
                    easting = "0".PadRight(maxprec, '0');
                }
            }

            quad_15 = lng + lat;
     
        }
        private bool Shift_Quad_1(int precision, bool latB, bool lngB)
        {
            string lat = quad_1[1].ToString();
            string lng = quad_1[0].ToString();
            
            bool shift = false;
            
           

            if (latB)
            {
                bool northingB = false;
                if(precision >= 4)
                {
                    northingB = Shift_Northing(precision);
                }
                if (northingB || precision < 4)
                {
                    lat = Shift_Quad_1_Degree(quad_1[1]);
                    northing = "0".PadRight(maxprec, '0');
                    if (lat == "A") { shift = true; }//Shift into next quad15
                }
            }

            if (lngB)
            {
                bool eastingB = false;
                if(precision>=4)
                {
                    eastingB = Shift_Easting(precision);
                }
                if (eastingB || precision < 4)
                {
                    lng = Shift_Quad_1_Degree(quad_1[0]);
                    easting= "0".PadRight(maxprec, '0');

                    if (lng == "A") { shift = true; }//Shift into next quad15
                }
            }
           
            quad_1 = lng+lat;
            return shift;
        }
        private bool Shift_Easting(int precision)
        {
            int minutes = int.Parse(easting.Substring(0, 2));
            int seconds = int.Parse(easting.Substring(2, maxprec-2));

            precision = precision - 2;

            if(precision > 2)
            {
                //Shift Seconds First
                seconds = Shift_Second(seconds, precision);              
            }
            
            if(precision<= 2 || seconds == 0)
            {
                minutes = Shift_Minute(minutes);
                seconds = 0;
                if (minutes == 0)
                {
                    easting = "0".PadRight(maxprec, '0');
                    return true;
                }
            }      

            easting = minutes.ToString().PadLeft(2, '0') + seconds.ToString().PadLeft(maxprec - 2, '0');
            return false;
         
        }
        private bool Shift_Northing(int precision)
        {
            int minutes = int.Parse(northing.Substring(0, 2));
            int seconds = int.Parse(northing.Substring(2, maxprec - 2));

            precision = precision - 2;

            if (precision > 2)
            {
                //Shift Seconds First
                seconds = Shift_Second(seconds, precision);
            }

            if (precision <= 2 || seconds == 0)
            {
                minutes = Shift_Minute(minutes);
                seconds = 0;
                if (minutes == 0)
                {
                    northing = "0".PadRight(maxprec, '0');
                    return true;
                }
            }          

            northing = minutes.ToString().PadLeft(2, '0') + seconds.ToString().PadLeft(maxprec - 2, '0');
            return false;

        }
        private int Shift_Minute(int num)
        {
            int a = num + 1;
            if (a >= 60) 
            { 
                //Exceeded, Shift Quad 1
                return 0; 
            }
            return a;
        }
        private int Shift_Second(int seconds, int precision)
        {
            precision = precision - 2;
            decimal dec = 1000000000;
            decimal delt = dec;
            decimal dseconds = seconds / dec; //Decimal keeps leading zeros safe it safe

            for (int x = 0; x < precision; x++)
            {
                delt /= 10;
            }
            decimal a = dseconds + delt/dec;

            if (a>=(decimal)1.0)
            {
                //Exceeded, shift minutes
                return 0;
            }
            return (int)((int)(a * dec / delt) * delt);
        }

        private int Set_Precision(int precision)
        {
            if (precision < 0) { throw new FormatException("The specified precision cannot be less than 0."); }
            if (precision > maxprec) { throw new FormatException($"The specified precision cannot be greater that {maxprec}"); }
            if (precision == 1) { return 2; }
            if (precision == 3) { return 4; }
            return precision;
        }
        private string Shift_Quad_15_Lng(char c)
        {
            if (c == 'Z') { return "A"; }
            int i = (int)c;
            i++;
            if ((char)i == 'I') { i++; } //I does not exist
            return ((char)i).ToString();
        }
        private string Shift_Quad_15_Lat(char c)
        {
            if (c == 'M') { return "A"; }
            int i = (int)c;
            i++;
            if ((char)i == 'I') { i++; } //I does not exist
            return ((char)i).ToString();
        }
        private string Shift_Quad_1_Degree(char c)
        {
            if (c == 'Q') { return "A"; }
            int i = (int)c;
            i++;
            if ((char)i == 'I') { i++; } //I does not exist
            return ((char)i).ToString();
        }

        #endregion
    }
}
