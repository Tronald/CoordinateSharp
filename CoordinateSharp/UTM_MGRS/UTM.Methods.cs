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
using System;
using System.Linq;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace CoordinateSharp
{
    public partial class UniversalTransverseMercator 
    {
        /// <summary>
        /// Creates a UniversalTransverMercator (UTM) object with a default WGS84 datum(ellipsoid).
        /// </summary>
        /// <param name="latz">Latitude Band Grid Zone Designation (Letter)</param>
        /// <param name="longz">Longitude Band Grid Zone Designation (Number)</param>
        /// <param name="est">Easting</param>
        /// <param name="nrt">Northing</param>
        /// <example>
        /// <code>
        /// UniversalTransverseMercator utm = new UniversalTransverseMercator("Q", 14, 581943.5, 2111989.8);
        /// </code>
        /// </example>
        public UniversalTransverseMercator(string latz, int longz, double est, double nrt)
        {
            if (longz < 1 || longz > 60) { Debug.WriteLine("Longitudinal zone out of range", "UTM longitudinal zones must be between 1-60."); }
            if (!Verify_Lat_Zone(latz)) { Debug.WriteLine("Latitudinal zone invalid", "UTM latitudinal zone was unrecognized."); }
            if (est < 160000 || est > 834000) { Debug.WriteLine("The Easting value provided is outside the max allowable range. Use with caution."); }
            if (nrt < 0 || nrt > 10000000) { Debug.WriteLine("Northing out of range", "Northing must be between 0-10,000,000."); }

            latZone = latz;
            longZone =longz;
            easting = est;
            northing = nrt;

            equatorial_radius = 6378137.0;
            inverse_flattening = 298.257223563;
        }
        /// <summary>
        /// Creates a UniversalTransverMercator (UTM) object with a custom datum(ellipsoid).
        /// </summary>
        /// <param name="latz">Latitude Band Grid Zone Designation (Letter)</param>
        /// <param name="longz">Longitude Band Grid Zone Designation (Number)</param>
        /// <param name="est">Easting</param>
        /// <param name="nrt">Northing</param>
        /// <param name="radius">Equatorial Radius</param>
        /// <param name="flaten">Inverse Flattening</param>
        /// <example>
        /// <code>
        /// UniversalTransverseMercator utm = new UniversalTransverseMercator("Q", 14, 581943.5, 2111989.8, 6378160.000, 298.25);
        /// </code>
        /// </example>
        public UniversalTransverseMercator(string latz, int longz, double est, double nrt, double radius, double flaten)
        {
            if (longz < 1 || longz > 60) { Debug.WriteLine("Longitudinal zone out of range", "UTM longitudinal zones must be between 1-60."); }
            if (!Verify_Lat_Zone(latz)) { Debug.WriteLine("Latitudinal zone invalid", "UTM latitudinal zone was unrecognized."); }
            if (est < 160000 || est > 834000) { Debug.WriteLine("The Easting value provided is outside the max allowable range. Use with caution."); }
            if (nrt < 0 || nrt > 10000000) { Debug.WriteLine("Northing out of range", "Northing must be between 0-10,000,000."); }

            latZone = latz;
            longZone = longz;
            easting = est;
            northing = nrt;

            equatorial_radius = radius;
            inverse_flattening = flaten;
        }

        /// <summary>
        /// Creates a UniversalTransverMercator (UTM) object with a default WGS84 datum(ellipsoid).
        /// </summary>
        /// <param name="gridZone">UTM Grid Zone Designation</param>
        /// <param name="est">Easting</param>
        /// <param name="nrt">Northing</param>
        /// <example>
        /// <code>
        /// UniversalTransverseMercator utm = new UniversalTransverseMercator("Q", 14, 581943.5, 2111989.8);
        /// </code>
        /// </example>
        public UniversalTransverseMercator(string gridZone, double est, double nrt)
        {
            string resultString = Regex.Match(gridZone, @"\d+").Value;
            int longz;
            if (!int.TryParse(resultString, out longz))
            {
                throw new FormatException("The MGRS Grid Zone Designator format is invalid.");
            }
            string latz = gridZone.Replace(resultString, "");

            if (longz < 1 || longz > 60) { Debug.WriteLine("Longitudinal zone out of range", "UTM longitudinal zones must be between 1-60."); }
            if (!Verify_Lat_Zone(latz)) { Debug.WriteLine("Latitudinal zone invalid", "UTM latitudinal zone was unrecognized."); }
            if (est < 160000 || est > 834000) { Debug.WriteLine("The Easting value provided is outside the max allowable range. Use with caution."); }
            if (nrt < 0 || nrt > 10000000) { Debug.WriteLine("Northing out of range", "Northing must be between 0-10,000,000."); }

            latZone = latz;
            longZone = longz;
            easting = est;
            northing = nrt;

            equatorial_radius = 6378137.0;
            inverse_flattening = 298.257223563;
        }


        /// <summary>
        /// Creates a UniversalTransverMercator (UTM) object with a default WGS84 datum(ellipsoid).
        /// </summary>
        /// <param name="gridZone">UTM Grid Zone Designation</param>
        /// <param name="est">Easting</param>
        /// <param name="nrt">Northing</param>
        /// <param name="radius">Equatorial Radius</param>
        /// <param name="flaten">Inverse Flattening</param>
        /// <example>
        /// <code>
        /// UniversalTransverseMercator utm = new UniversalTransverseMercator("Q", 14, 581943.5, 2111989.8);
        /// </code>
        /// </example>
        public UniversalTransverseMercator(string gridZone, double est, double nrt, double radius, double flaten)
        {
            string resultString = Regex.Match(gridZone, @"\d+").Value;
            int longz;
            if (!int.TryParse(resultString, out longz))
            {
                throw new FormatException("The MGRS Grid Zone Designator format is invalid.");
            }
            string latz = gridZone.Replace(resultString, "");

            if (longz < 1 || longz > 60) { Debug.WriteLine("Longitudinal zone out of range", "UTM longitudinal zones must be between 1-60."); }
            if (!Verify_Lat_Zone(latz)) { Debug.WriteLine("Latitudinal zone invalid", "UTM latitudinal zone was unrecognized."); }
            if (est < 160000 || est > 834000) { Debug.WriteLine("The Easting value provided is outside the max allowable range. Use with caution."); }
            if (nrt < 0 || nrt > 10000000) { Debug.WriteLine("Northing out of range", "Northing must be between 0-10,000,000."); }

            latZone = latz;
            longZone = longz;
            easting = est;
            northing = nrt;

            equatorial_radius = radius;
            inverse_flattening = flaten;
        }

        /// <summary>
        /// Constructs a UTM object based off DD Lat/Long
        /// </summary>
        /// <param name="lat">DD Latitude</param>
        /// <param name="longi">DD Longitide</param>
        /// <param name="c">Parent Coordinate Object</param>
        internal UniversalTransverseMercator(double lat, double longi, Coordinate c)
        {
            //validate coords

            //if (lat > 180) { throw new ArgumentOutOfRangeException("Degrees out of range", "Longitudinal coordinate decimal cannot be greater than 180."); }
            //if (lat < -180) { throw new ArgumentOutOfRangeException("Degrees out of range", "Longitudinal coordinate decimal cannot be less than 180."); }

            //if (longi > 90) { throw new ArgumentOutOfRangeException("Degrees out of range", "Latitudinal coordinate decimal cannot be greater than 90."); }
            //if (longi < -90) { throw new ArgumentOutOfRangeException("Degrees out of range", "Latitudinal coordinate decimal cannot be less than 90."); }
            equatorial_radius = 6378137.0;
            inverse_flattening = 298.257223563;
            ToUTM(lat, longi, this);

            coordinate = c;
        }
        /// <summary>
        /// Constructs a UTM object based off DD Lat/Long
        /// </summary>
        /// <param name="lat">DD Latitude</param>
        /// <param name="longi">DD Longitide</param>
        /// <param name="c">Parent Coordinate Object</param>
        /// <param name="rad">Equatorial Radius</param>
        /// <param name="flt">Flattening</param>
        internal UniversalTransverseMercator(double lat, double longi, Coordinate c,double rad,double flt)
        {
            equatorial_radius = rad;
            inverse_flattening = flt;
            ToUTM(lat, longi, this);

            coordinate = c;
        }
        /// <summary>
        /// Constructs a UTM object based off a UTM coordinate
        /// Not yet implemented
        /// </summary>
        /// <param name="latz">Zone Letter</param>
        /// <param name="longz">Zone Number</param>
        /// <param name="e">Easting</param>
        /// <param name="n">Northing</param>
        /// <param name="c">Parent Coordinate Object</param>
        /// <param name="rad">Equatorial Radius</param>
        /// <param name="flt">Inverse Flattening</param>
        internal UniversalTransverseMercator(string latz, int longz, double e, double n, Coordinate c, double rad, double flt)
        {
            //validate utm
            if (longz < 1 || longz > 60) { Debug.WriteLine("Longitudinal zone out of range", "UTM longitudinal zones must be between 1-60."); }
            if (!Verify_Lat_Zone(latz)) { throw new ArgumentException("Latitudinal zone invalid", "UTM latitudinal zone was unrecognized."); }
            if (e < 160000 || e > 834000) { Debug.WriteLine("The Easting value provided is outside the max allowable range. If this is intentional, use with caution."); }
            if (n < 0 || n > 10000000) { throw new ArgumentOutOfRangeException("Northing out of range", "Northing must be between 0-10,000,000."); }
            equatorial_radius = rad;
            inverse_flattening = flt;
            latZone = latz;
            longZone = longz;

            easting = e;
            northing = n;

            coordinate = c;
            if (c.Latitude.DecimalDegree <= -80 || c.Latitude.DecimalDegree >= 84) { withinCoordinateSystemBounds = false; }
            else { withinCoordinateSystemBounds = true; }
        }

        /// <summary>
        /// Verifies Lat zone when convert from UTM to DD Lat/Long
        /// </summary>
        /// <param name="l">Zone Letter</param>
        /// <returns>boolean</returns>
        private bool Verify_Lat_Zone(string l)
        {
            if (LatZones.longZongLetters.Where(x => x == l.ToUpper()).Count() != 1)
            {
                return false;
            }
            return true;
        }
        private double degreeToRadian(double degree)
        {
            return degree * Math.PI / 180;
        }
        /// <summary>
        /// Assigns UTM values based of Lat/Long
        /// </summary>
        /// <param name="lat">DD Latitude</param>
        /// <param name="longi">DD longitude</param>
        /// <param name="utm">UTM Object to modify</param>
        internal void ToUTM(double lat, double longi, UniversalTransverseMercator utm)
        {
            string letter = "";
            double easting = 0;
            double northing = 0;
            int zone = (int)Math.Floor(longi / 6 + 31);
            if (lat < -72)
                letter = "C";
            else if (lat < -64)
                letter = "D";
            else if (lat < -56)
                letter = "E";
            else if (lat < -48)
                letter = "F";
            else if (lat < -40)
                letter = "G";
            else if (lat < -32)
                letter = "H";
            else if (lat < -24)
                letter = "J";
            else if (lat < -16)
                letter = "K";
            else if (lat < -8)
                letter = "L";
            else if (lat < 0)
                letter = "M";
            else if (lat < 8)
                letter = "N";
            else if (lat < 16)
                letter = "P";
            else if (lat < 24)
                letter = "Q";
            else if (lat < 32)
                letter = "R";
            else if (lat < 40)
                letter = "S";
            else if (lat < 48)
                letter = "T";
            else if (lat < 56)
                letter = "U";
            else if (lat < 64)
                letter = "V";
            else if (lat < 72)
                letter = "W";
            else
                letter = "X";

            double a = utm.equatorial_radius;
            double f = 1.0 / utm.inverse_flattening;
            double b = a * (1 - f);   // polar radius
           
            double e = Math.Sqrt(1 - Math.Pow(b, 2) / Math.Pow(a, 2));
            double e0 = e / Math.Sqrt(1 - Math.Pow(e, 1));

            double drad = Math.PI / 180;
            double k0 = 0.9996;

            double phi = lat * drad;                              // convert latitude to radians
            double lng = longi * drad;                             // convert longitude to radians
            double utmz = 1 + Math.Floor((longi + 180) / 6.0);            // longitude to utm zone
            double zcm = 3 + 6.0 * (utmz - 1) - 180;                     // central meridian of a zone
                                                     // this gives us zone A-B for below 80S
            double esq = (1 - (b / a) * (b / a));
            double e0sq = e * e / (1 - Math.Pow(e, 2));
            double M = 0;

            double N = a / Math.Sqrt(1 - Math.Pow(e * Math.Sin(phi), 2));
            double T = Math.Pow(Math.Tan(phi), 2);
            double C = e0sq * Math.Pow(Math.Cos(phi), 2);
            double A = (longi - zcm) * drad * Math.Cos(phi);

            // calculate M (USGS style)
            M = phi * (1 - esq * (1.0 / 4.0 + esq * (3.0 / 64.0 + 5.0 * esq / 256.0)));
            M = M - Math.Sin(2.0 * phi) * (esq * (3.0 / 8.0 + esq * (3.0 / 32.0 + 45.0 * esq / 1024.0)));
            M = M + Math.Sin(4.0 * phi) * (esq * esq * (15.0 / 256.0 + esq * 45.0 / 1024.0));
            M = M - Math.Sin(6.0 * phi) * (esq * esq * esq * (35.0 / 3072.0));
            M = M * a;//Arc length along standard meridian

            double M0 = 0;// if another point of origin is used than the equator

            // Calculate the UTM values...
            // first the easting
            var x = k0 * N * A * (1 + A * A * ((1 - T + C) / 6 + A * A * (5 - 18 * T + T * T + 72.0 * C - 58 * e0sq) / 120.0)); //Easting relative to CM
            x = x + 500000; // standard easting

            // Northing
        
            double y = k0 * (M - M0 + N * Math.Tan(phi) * (A * A * (1 / 2.0 + A * A * ((5 - T + 9 * C + 4 * C * C) / 24.0 + A * A * (61 - 58 * T + T * T + 600 * C - 330 * e0sq) / 720.0))));    // first from the equator
            double yg = y + 10000000;  //yg = y global, from S. Pole
            if (y < 0)
            {
                y = 10000000 + y;   // add in false northing if south of the equator
            }


            easting = x;
            northing = y; 
               
            utm.latZone = letter;
            utm.longZone = zone;
            utm.easting = easting;
            utm.northing = northing;
            
            if(lat<=-80 || lat >= 84) { withinCoordinateSystemBounds = false; }
            else { withinCoordinateSystemBounds = true; }
        }
       
        /// <summary>
        /// Default formatted UTM string
        /// </summary>
        /// <returns>UTM Formatted Coordinate String</returns>
        public override string ToString()
        {
            if (!withinCoordinateSystemBounds) { return ""; }//MGRS Coordinate is outside its reliable boundaries. Return empty.
            return longZone.ToString() + LatZone + " " + (int)easting + "mE " + (int)northing + "mN";
        }
       
        private static Coordinate UTMtoLatLong(double x, double y, double zone, double equatorialRadius, double flattening)
        {
            //x easting
            //y northing
           
            //http://home.hiwaay.net/~taylorc/toolbox/geography/geoutm.html
            double phif, Nf, Nfpow, nuf2, ep2, tf, tf2, tf4, cf;
            double x1frac, x2frac, x3frac, x4frac, x5frac, x6frac, x7frac, x8frac;
            double x2poly, x3poly, x4poly, x5poly, x6poly, x7poly, x8poly;

            double sm_a = equatorialRadius;
            double sm_b = equatorialRadius * (1 - (1.0 / flattening)); //Polar Radius
          
            /* Get the value of phif, the footpoint latitude. */
            phif = FootpointLatitude(y,equatorialRadius,flattening);

            /* Precalculate ep2 */
            ep2 = (Math.Pow(sm_a, 2.0) - Math.Pow(sm_b, 2.0))
                  / Math.Pow(sm_b, 2.0);

            /* Precalculate cos (phif) */
            cf = Math.Cos(phif);

            /* Precalculate nuf2 */
            nuf2 = ep2 * Math.Pow(cf, 2.0);

            /* Precalculate Nf and initialize Nfpow */
            Nf = Math.Pow(sm_a, 2.0) / (sm_b * Math.Sqrt(1 + nuf2));
            Nfpow = Nf;

            /* Precalculate tf */
            tf = Math.Tan(phif);
            tf2 = tf * tf;
            tf4 = tf2 * tf2;

            /* Precalculate fractional coefficients for x**n in the equations
               below to simplify the expressions for latitude and longitude. */
            x1frac = 1.0 / (Nfpow * cf);

            Nfpow *= Nf;   /* now equals Nf**2) */
            x2frac = tf / (2.0 * Nfpow);

            Nfpow *= Nf;   /* now equals Nf**3) */
            x3frac = 1.0 / (6.0 * Nfpow * cf);

            Nfpow *= Nf;   /* now equals Nf**4) */
            x4frac = tf / (24.0 * Nfpow);

            Nfpow *= Nf;   /* now equals Nf**5) */
            x5frac = 1.0 / (120.0 * Nfpow * cf);

            Nfpow *= Nf;   /* now equals Nf**6) */
            x6frac = tf / (720.0 * Nfpow);

            Nfpow *= Nf;   /* now equals Nf**7) */
            x7frac = 1.0 / (5040.0 * Nfpow * cf);

            Nfpow *= Nf;   /* now equals Nf**8) */
            x8frac = tf / (40320.0 * Nfpow);

            /* Precalculate polynomial coefficients for x**n.
               -- x**1 does not have a polynomial coefficient. */
            x2poly = -1.0 - nuf2;

            x3poly = -1.0 - 2 * tf2 - nuf2;

            x4poly = 5.0 + 3.0 * tf2 + 6.0 * nuf2 - 6.0 * tf2 * nuf2
                - 3.0 * (nuf2 * nuf2) - 9.0 * tf2 * (nuf2 * nuf2);

            x5poly = 5.0 + 28.0 * tf2 + 24.0 * tf4 + 6.0 * nuf2 + 8.0 * tf2 * nuf2;

            x6poly = -61.0 - 90.0 * tf2 - 45.0 * tf4 - 107.0 * nuf2
                + 162.0 * tf2 * nuf2;

            x7poly = -61.0 - 662.0 * tf2 - 1320.0 * tf4 - 720.0 * (tf4 * tf2);

            x8poly = 1385.0 + 3633.0 * tf2 + 4095.0 * tf4 + 1575 * (tf4 * tf2);

            /* Calculate latitude */
            double nLat = phif + x2frac * x2poly * (x * x)
                + x4frac * x4poly * Math.Pow(x, 4.0)
                + x6frac * x6poly * Math.Pow(x, 6.0)
                + x8frac * x8poly * Math.Pow(x, 8.0);

            /* Calculate longitude */
            double nLong = zone + x1frac * x
                + x3frac * x3poly * Math.Pow(x, 3.0)
                + x5frac * x5poly * Math.Pow(x, 5.0)
                + x7frac * x7poly * Math.Pow(x, 7.0);

            double dLat = RadToDeg(nLat);
            double dLong = RadToDeg(nLong);
            if (dLat > 90) { dLat = 90; }
            if (dLat < -90) { dLat = -90; }
            if (dLong > 180) { dLong = 180; }
            if (dLong < -180) { dLong = -180; }

            Coordinate c = new Coordinate(equatorialRadius,flattening, true);
            CoordinatePart cLat = new CoordinatePart(dLat, CoordinateType.Lat);
            CoordinatePart cLng = new CoordinatePart(dLong, CoordinateType.Long);

            c.Latitude = cLat;
            c.Longitude = cLng;
           
            return c;
        }
        private static double[] UTMtoSigned(double x, double y, double zone, double equatorialRadius, double flattening)
        {
            //x easting
            //y northing

            //http://home.hiwaay.net/~taylorc/toolbox/geography/geoutm.html
            double phif, Nf, Nfpow, nuf2, ep2, tf, tf2, tf4, cf;
            double x1frac, x2frac, x3frac, x4frac, x5frac, x6frac, x7frac, x8frac;
            double x2poly, x3poly, x4poly, x5poly, x6poly, x7poly, x8poly;

            double sm_a = equatorialRadius;
            double sm_b = equatorialRadius * (1 - (1.0 / flattening)); //Polar Radius

            /* Get the value of phif, the footpoint latitude. */
            phif = FootpointLatitude(y, equatorialRadius, flattening);

            /* Precalculate ep2 */
            ep2 = (Math.Pow(sm_a, 2.0) - Math.Pow(sm_b, 2.0))
                  / Math.Pow(sm_b, 2.0);

            /* Precalculate cos (phif) */
            cf = Math.Cos(phif);

            /* Precalculate nuf2 */
            nuf2 = ep2 * Math.Pow(cf, 2.0);

            /* Precalculate Nf and initialize Nfpow */
            Nf = Math.Pow(sm_a, 2.0) / (sm_b * Math.Sqrt(1 + nuf2));
            Nfpow = Nf;

            /* Precalculate tf */
            tf = Math.Tan(phif);
            tf2 = tf * tf;
            tf4 = tf2 * tf2;

            /* Precalculate fractional coefficients for x**n in the equations
               below to simplify the expressions for latitude and longitude. */
            x1frac = 1.0 / (Nfpow * cf);

            Nfpow *= Nf;   /* now equals Nf**2) */
            x2frac = tf / (2.0 * Nfpow);

            Nfpow *= Nf;   /* now equals Nf**3) */
            x3frac = 1.0 / (6.0 * Nfpow * cf);

            Nfpow *= Nf;   /* now equals Nf**4) */
            x4frac = tf / (24.0 * Nfpow);

            Nfpow *= Nf;   /* now equals Nf**5) */
            x5frac = 1.0 / (120.0 * Nfpow * cf);

            Nfpow *= Nf;   /* now equals Nf**6) */
            x6frac = tf / (720.0 * Nfpow);

            Nfpow *= Nf;   /* now equals Nf**7) */
            x7frac = 1.0 / (5040.0 * Nfpow * cf);

            Nfpow *= Nf;   /* now equals Nf**8) */
            x8frac = tf / (40320.0 * Nfpow);

            /* Precalculate polynomial coefficients for x**n.
               -- x**1 does not have a polynomial coefficient. */
            x2poly = -1.0 - nuf2;

            x3poly = -1.0 - 2 * tf2 - nuf2;

            x4poly = 5.0 + 3.0 * tf2 + 6.0 * nuf2 - 6.0 * tf2 * nuf2
                - 3.0 * (nuf2 * nuf2) - 9.0 * tf2 * (nuf2 * nuf2);

            x5poly = 5.0 + 28.0 * tf2 + 24.0 * tf4 + 6.0 * nuf2 + 8.0 * tf2 * nuf2;

            x6poly = -61.0 - 90.0 * tf2 - 45.0 * tf4 - 107.0 * nuf2
                + 162.0 * tf2 * nuf2;

            x7poly = -61.0 - 662.0 * tf2 - 1320.0 * tf4 - 720.0 * (tf4 * tf2);

            x8poly = 1385.0 + 3633.0 * tf2 + 4095.0 * tf4 + 1575 * (tf4 * tf2);

            /* Calculate latitude */
            double nLat = phif + x2frac * x2poly * (x * x)
                + x4frac * x4poly * Math.Pow(x, 4.0)
                + x6frac * x6poly * Math.Pow(x, 6.0)
                + x8frac * x8poly * Math.Pow(x, 8.0);

            /* Calculate longitude */
            double nLong = zone + x1frac * x
                + x3frac * x3poly * Math.Pow(x, 3.0)
                + x5frac * x5poly * Math.Pow(x, 5.0)
                + x7frac * x7poly * Math.Pow(x, 7.0);

            double dLat = RadToDeg(nLat);
            double dLong = RadToDeg(nLong);
            if (dLat > 90) { dLat = 90; }
            if (dLat < -90) { dLat = -90; }
            if (dLong > 180) { dLong = 180; }
            if (dLong < -180) { dLong = -180; }

        

            return new double[] { dLat, dLong };
        }

        private static double RadToDeg(double rad)
        {
            double pi = 3.14159265358979;
            return (rad / pi * 180.0);
        }
        private static double DegToRad(double deg)
        {
            double pi = 3.14159265358979;
            return (deg / 180.0 * pi);
        }
        private static double FootpointLatitude(double y, double equatorialRadius, double flattening)
        {
            double y_, alpha_, beta_, gamma_, delta_, epsilon_, n;
            double result;


            /* Ellipsoid model constants (actual values here are for WGS84) */
            double sm_a = equatorialRadius;
            double sm_b = equatorialRadius * (1 - (1.0 / flattening));
           

            /* Precalculate n (Eq. 10.18) */
            n = (sm_a - sm_b) / (sm_a + sm_b);

            /* Precalculate alpha_ (Eq. 10.22) */
            /* (Same as alpha in Eq. 10.17) */
            alpha_ = ((sm_a + sm_b) / 2.0) * (1 + (Math.Pow(n, 2.0) / 4) + (Math.Pow(n, 4.0) / 64));

            /* Precalculate y_ (Eq. 10.23) */
            y_ = y / alpha_;

            /* Precalculate beta_ (Eq. 10.22) */
            beta_ = (3.0 * n / 2.0) + (-27.0 * Math.Pow(n, 3.0) / 32.0)
                + (269.0 * Math.Pow(n, 5.0) / 512.0);

            /* Precalculate gamma_ (Eq. 10.22) */
            gamma_ = (21.0 * Math.Pow(n, 2.0) / 16.0)
                + (-55.0 * Math.Pow(n, 4.0) / 32.0);

            /* Precalculate delta_ (Eq. 10.22) */
            delta_ = (151.0 * Math.Pow(n, 3.0) / 96.0)
                + (-417.0 * Math.Pow(n, 5.0) / 128.0);

            /* Precalculate epsilon_ (Eq. 10.22) */
            epsilon_ = (1097.0 * Math.Pow(n, 4.0) / 512.0);

            /* Now calculate the sum of the series (Eq. 10.21) */
            result = y_ + (beta_ * Math.Sin(2.0 * y_))
                + (gamma_ * Math.Sin(4.0 * y_))
                + (delta_ * Math.Sin(6.0 * y_))
                + (epsilon_ * Math.Sin(8.0 * y_));

            return result;
        }

        /// <summary>
        /// Converts UTM coordinate to Lat/Long
        /// </summary>
        /// <param name="utm">utm</param>
        /// <returns>Coordinate</returns>
        /// <example>
        /// The following example creates (converts to) a geodetic Coordinate object based on a UTM object.
        /// <code>
        /// UniversalTransverseMercator utm = new UniversalTransverseMercator("T", 32, 233434, 234234);
		/// Coordinate c = UniversalTransverseMercator.ConvertUTMtoLatLong(utm);
        /// Console.WriteLine(c); //N 2º 7' 2.332" E 6º 36' 12.653"
        /// </code>
        /// </example>
        public static Coordinate ConvertUTMtoLatLong(UniversalTransverseMercator utm)
        {

            bool southhemi = false;
            if (utm.latZone == "A" || utm.latZone == "B" || utm.latZone == "C" || utm.latZone == "D" || utm.latZone == "E" || utm.latZone == "F" || utm.latZone == "G" || utm.latZone == "H" || utm.latZone == "J" ||
                   utm.latZone == "K" || utm.latZone == "L" || utm.latZone == "M")
            {
                southhemi = true;
            }
     
            double cmeridian;

            double x = utm.Easting - 500000.0;
            double UTMScaleFactor = 0.9996;
            x /= UTMScaleFactor;

            /* If in southern hemisphere, adjust y accordingly. */
            double y = utm.Northing;
            if (southhemi)
            {
                y -= 10000000.0;
            }

            y /= UTMScaleFactor;

            cmeridian = UTMCentralMeridian(utm.LongZone);
           
            Coordinate c = UTMtoLatLong(x, y, cmeridian, utm.equatorial_radius, utm.inverse_flattening);

            if (c.Latitude.ToDouble() > 85 || c.Latitude.ToDouble() < -85)
            {
                Debug.WriteLine("UTM conversions greater than 85 degrees or less than -85 degree latitude contain major deviations and should be used with caution.");
            }
            return c;


        }
        /// <summary>
        /// Converts UTM coordinate to Signed Degree Lat/Long
        /// </summary>
        /// <param name="utm">utm</param>
        /// <returns>Coordinate</returns>
        /// <example>
        /// The following example creates (converts to) a signed degree lat long based on a UTM object.
        /// <code>
        /// UniversalTransverseMercator utm = new UniversalTransverseMercator("T", 32, 233434, 234234);
        /// double[] signed = UniversalTransverseMercator.ConvertUTMtoSignedDegree(utm);
        /// Coordinate c = new Coordinate(signed[0], signed[1], new EagerLoad(false));
        /// Console.WriteLine(c); //N 2º 7' 2.332" E 6º 36' 12.653"
        /// </code>
        /// </example>
        public static double[] ConvertUTMtoSignedDegree(UniversalTransverseMercator utm)
        {

            bool southhemi = false;
            if (utm.latZone == "A" || utm.latZone == "B" || utm.latZone == "C" || utm.latZone == "D" || utm.latZone == "E" || utm.latZone == "F" || utm.latZone == "G" || utm.latZone == "H" || utm.latZone == "J" ||
                   utm.latZone == "K" || utm.latZone == "L" || utm.latZone == "M")
            {
                southhemi = true;
            }

            double cmeridian;

            double x = utm.Easting - 500000.0;
            double UTMScaleFactor = 0.9996;
            x /= UTMScaleFactor;

            /* If in southern hemisphere, adjust y accordingly. */
            double y = utm.Northing;
            if (southhemi)
            {
                y -= 10000000.0;
            }

            y /= UTMScaleFactor;

            cmeridian = UTMCentralMeridian(utm.LongZone);

            double[] signed = UTMtoSigned(x, y, cmeridian, utm.equatorial_radius, utm.inverse_flattening);

            if (signed[0] > 85 || signed[1] < -85)
            {
                Debug.WriteLine("UTM conversions greater than 85 degrees or less than -85 degree latitude contain major deviations and should be used with caution.");
            }
            return signed;


        }
        private static double UTMCentralMeridian(double zone)
        {
            double cmeridian;

            cmeridian = DegToRad(-183.0 + (zone * 6.0));

            return cmeridian;
        }
    
	}  
}
