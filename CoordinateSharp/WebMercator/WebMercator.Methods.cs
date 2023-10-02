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
using CoordinateSharp.Formatters;
using System.Diagnostics;


namespace CoordinateSharp
{  
    public partial class WebMercator
    {

        /// <summary>
        /// Creates a Web Mercator (EPSG:3857).
        /// </summary>      
        /// <param name="est">Easting</param>
        /// <param name="nrt">Northing</param>
        /// <example>
        /// <code>
        /// WebMercator wm = new WebMercator(8284118.2, 6339892.6);
        /// </code>
        /// </example>
        public WebMercator(double est, double nrt)
        {
            //https://epsg.io/3857

            if (est < -DataValues.WebMercatorEastingLimit || est > DataValues.WebMercatorEastingLimit)
            {
                throw new ArgumentOutOfRangeException("Web Mercator Easting Boundary Exceeded", $"Web Mercator Easting must be between -{DataValues.WebMercatorEastingLimit} and {DataValues.WebMercatorEastingLimit}.");
            }
            if (nrt < -DataValues.WebMercatorNorthingLimit || nrt > DataValues.WebMercatorNorthingLimit)
            {
                throw new ArgumentOutOfRangeException("Web Mercator Northing Boundary Exceeded", $"Web Mercator Northing must be between -{DataValues.WebMercatorNorthingLimit} and {DataValues.WebMercatorNorthingLimit}.");
            }

            easting = est;
            northing = nrt;

        }

       

        /// <summary>
        /// Constructs a Web Mercator object based off signed Lat/Long.
        /// </summary>
        /// <param name="lat">Signed Latitude</param>
        /// <param name="lng">Signed Longitude</param>
        /// <param name="c">Parent Coordinate Object</param>
        internal WebMercator(double lat, double lng, Coordinate c)
        {
            ToWebMercator(lat, lng, this);
            coordinate = c;
        }
     

        /// <summary>
        /// Assigns Web Mercator values based on Lat/Long.
        /// </summary>
        /// <param name="lat">Signed Latitude</param>
        /// <param name="lng">Signed longitude</param>     
        /// <param name="wmc">Web Mercator</param>     
        internal void ToWebMercator(double lat, double lng, WebMercator wmc)
        {
            //E = Easting
            //FE = False Easting
            //a= Semi-Major Axis
            //lng = Longitude
            //lngNO = Longitude of Natural Origin
            //N = Northing
            //FN = False Northing
            //ln = NATURAL LOGARITHM

            //Easting = FE + a * (lng - lngNO)
            //Northing = FN + a * ln[tan(pi/4*latitude/2]

            double latRad = Extensions.ToRadians(lat);
            double lngRad = Extensions.ToRadians(lng);

            double FE = wmc.false_easting;
            double FN = wmc.false_northing;

            double a = DataValues.DefaultSemiMajorAxis; //Must use WGS 84

            double E = FE + a * (lngRad - 0);
            double N = FN + a * Math.Log(Math.Tan(Math.PI / 4 + latRad / 2));

          
            //Corrected needed at 180 degrees
            if( E > DataValues.WebMercatorEastingLimit) { E = DataValues.WebMercatorEastingLimit; }
            if (E < -DataValues.WebMercatorEastingLimit) { E = -DataValues.WebMercatorEastingLimit; }

            //Truncate above /below 85.06 degrees
            if (N > DataValues.WebMercatorNorthingLimit) { N = DataValues.WebMercatorNorthingLimit;  }
            if (N < -DataValues.WebMercatorNorthingLimit) { N = -DataValues.WebMercatorNorthingLimit; }

            wmc.easting = E;
            wmc.northing = N;
           
        }

        /// <summary>
        /// Default formatted Web Mercator string.
        /// </summary>
        /// <returns>UTM Formatted Coordinate String</returns>
        public override string ToString()
        {
            return $"{Math.Round(easting, 3)}mE {Math.Round(northing, 3)}mN";
        }   
    
        private static Coordinate WebMercatortoLatLong(double x, double y, EagerLoad el)
        {
            double[] d = WebMercatortoSigned(x, y);
            return new Coordinate(d[0], d[1], el);
        }

        private static double[] WebMercatortoSigned(double x, double y)
        {
            //x easting
            //y northing

            //E = Easting
            //FE = False Easting
            //a= Semi-Major Axis
            //lng = Longitude
            //lngNO = Longitude of Natural Origin
            //N = Northing
            //FN = False Northing

            //D = -(FN - N) / a = (FN-n)/a
            //lat = pi/2 - 2 * atan(e^D) where e=base of natural log 2.7182818
            //lng = [(E-FE)/a] +lngNO

            double E = x;
            double N = y;

            double FE = 0;
            double FN = 0;

            double a = DataValues.DefaultSemiMajorAxis; //Must use WGS 84
            double lngNO = 0;
            double D = -(N - FN) / a;

            double lat = Extensions.ToDegrees(Math.PI / 2 - 2 * Math.Atan(Math.Pow(Math.E, D)));
            double lng = Extensions.ToDegrees(((E - FE) / a) + lngNO);

            return new double[] { lat, lng };
        }


        /// <summary>
        /// Converts Web Mercator coordinate to Lat/Long.
        /// </summary>
        /// <param name="wmt">Web Mercator</param>
        /// <returns>Coordinate</returns>
        /// <example>
        /// The following example creates (converts to) a geodetic Coordinate object based on a Web Mercator object.
        /// <code>
        /// WebMercator wmc = new WebMercator(8284118.2, 6339892.6);
        /// Coordinate c = WebMercator.ConvertWebMercatortoLatLong(wmc);
        /// Console.WriteLine(c); //N 49º 22' 54.431" E 74º 25' 3"
        /// </code>
        /// </example>
        public static Coordinate ConvertWebMercatortoLatLong(WebMercator wmt)
        {
            return ConvertWebMercatortoLatLong(wmt, GlobalSettings.Default_EagerLoad);
        }
     
        /// <summary>
        /// Converts Web Mercator coordinate to Lat/Long.
        /// </summary>
        /// <param name="wmt">Web Mercator</param>
        /// <param name="eagerLoad">EagerLoad</param>
        /// <returns>Coordinate</returns>
        /// <example>
        /// The following example creates (converts to) a geodetic Coordinate object based on a Web Mercator object. 
        /// Performance is maximized by turning off EagerLoading.
        /// <code>
        /// EagerLoad el = new EagerLoad(false);
        /// WebMercator wmc = new WebMercator(8284118.2, 6339892.6);
        /// Coordinate c = WebMercator.ConvertWebMercatortoLatLong(wmc, el);
        /// Console.WriteLine(c); //N 49º 22' 54.431" E 74º 25' 3"
        /// </code>
        /// </example>
        public static Coordinate ConvertWebMercatortoLatLong(WebMercator wmt, EagerLoad eagerLoad)
        {
            return WebMercatortoLatLong(wmt.easting, wmt.northing, eagerLoad);
        }

        /// <summary>
        /// Converts Web Mercator coordinate to Lat/Long.
        /// </summary>
        /// <param name="easting">Easting</param>
        /// <param name="northing">Northing</param>
        /// <returns>Coordinate</returns>
        /// <example>
        /// The following example creates (converts to) a geodetic Coordinate object based on Web Mercator Easting and Northing values.
        /// <code>
        /// Coordinate c = WebMercator.ConvertWebMercatortoLatLong(8284118.2, 6339892.6);
        /// Console.WriteLine(c); //N 49º 22' 54.431" E 74º 25' 3"
        /// </code>
        /// </example>
        public static Coordinate ConvertWebMercatortoLatLong(double easting, double northing)
        {
            WebMercator wmt = new WebMercator(easting, northing);
            return ConvertWebMercatortoLatLong(wmt, GlobalSettings.Default_EagerLoad);
        }

        /// <summary>
        /// Converts Web Mercator coordinate to Lat/Long.
        /// </summary>
        /// <param name="easting">Easting</param>
        /// <param name="northing">Northing</param>
        /// <param name="eagerLoad">EagerLoad</param>
        /// <returns>Coordinate</returns>
        /// <example>
        /// The following example creates (converts to) a geodetic Coordinate object based on Web Mercator Easting and Northing values.
        /// Performance is maximized by turning off EagerLoading.
        /// <code>
        /// EagerLoad el = new EagerLoad(false);
        /// Coordinate c = WebMercator.ConvertWebMercatortoLatLong(8284118.2, 6339892.6, el);
        /// Console.WriteLine(c); //N 49º 22' 54.431" E 74º 25' 3"
        /// </code>
        /// </example>
        public static Coordinate ConvertWebMercatortoLatLong(double easting, double northing, EagerLoad eagerLoad)
        {
            WebMercator wmt = new WebMercator(easting, northing);
            return ConvertWebMercatortoLatLong(wmt, eagerLoad);
        }

        /// <summary>
        /// Converts Web Mercator coordinate to Signed Degree Lat/Long.
        /// </summary>
        /// <param name="wmt">Web Mercator</param>
        /// <returns>double[lat, lng]</returns>
        /// <example>
        /// The following example creates (converts to) a signed degree lat long double array based on a Web Mercator object.
        /// <code>
        /// WebMercator wmc = new WebMercator(8284118.2, 6339892.6);
        /// double[] signed = WebMercator.ConvertWebMercatortoSignedDegree(wmc);
        /// Coordinate c = new Coordinate(signed[0], signed[1], new EagerLoad(false));
        /// Console.WriteLine(c); //N 49º 22' 54.431" E 74º 25' 3"
        /// </code>
        /// </example>
        public static double[] ConvertWebMercatortoSignedDegree(WebMercator wmt)
        {
            double[] signed = WebMercatortoSigned(wmt.easting,wmt.northing);

            return signed;
        }

        internal static bool Datum_Check(double radius, double inverseflattening)
        {
            if (Math.Abs(radius - DataValues.DefaultSemiMajorAxis) > .000000001) { return false; }
            if (Math.Abs(inverseflattening- DataValues.DefaultInverseFlattening) > .000000001) { return false; }
            return true;
        }
    }
}
