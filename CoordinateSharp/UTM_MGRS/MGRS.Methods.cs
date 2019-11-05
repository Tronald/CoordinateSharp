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
    public partial class MilitaryGridReferenceSystem
    {
        /// <summary>
        /// Creates an MilitaryGridReferenceSystem (MGRS) object with a default WGS84 datum(ellipsoid).
        /// </summary>
        /// <param name="latz">MGRS Latitude Band Grid Zone Designation (Letter)</param>
        /// <param name="longz">MGRS Longitude Band Grid Zone Designation (Number)</param>
        /// <param name="d">MGRS 100,000 Meter Square Identifier (2 Letter)</param>
        /// <param name="e">Easting</param>
        /// <param name="n">Northing</param>
        /// <example>
        /// <code>
        /// MilitaryGridReferenceSystem mgrs = new MilitaryGridReferenceSystem("N", 21, "SA", 66037, 61982);
        /// </code>
        /// </example>
        public MilitaryGridReferenceSystem(string latz, int longz, string d, double e, double n)
        {
            string digraphLettersE = "ABCDEFGHJKLMNPQRSTUVWXYZ";
            string digraphLettersN = "ABCDEFGHJKLMNPQRSTUV";
            if (longz < 1 || longz > 60) { Debug.WriteLine("Longitudinal zone out of range", "UTM longitudinal zones must be between 1-60."); }
            if (!Verify_Lat_Zone(latz)) { throw new ArgumentException("Latitudinal zone invalid", "UTM latitudinal zone was unrecognized."); }
            if (n < 0 || n > 10000000) { throw new ArgumentOutOfRangeException("Northing out of range", "Northing must be between 0-10,000,000."); }
            if (d.Count() < 2 || d.Count() > 2) { throw new ArgumentException("Digraph invalid", "MGRS Digraph was unrecognized."); }
            if (digraphLettersE.ToCharArray().ToList().Where(x => x.ToString() == d.ToUpper()[0].ToString()).Count() == 0) { throw new ArgumentException("Digraph invalid", "MGRS Digraph was unrecognized."); }
            if (digraphLettersN.ToCharArray().ToList().Where(x => x.ToString() == d.ToUpper()[1].ToString()).Count() == 0) { throw new ArgumentException("Digraph invalid", "MGRS Digraph was unrecognized."); }
            latZone = latz;
            longZone = longz;
            digraph = d;
            easting = e;
            northing = n;
            //WGS84
            equatorialRadius = 6378137.0;
            inverseFlattening = 298.257223563;
          
        }
        /// <summary>
        /// Creates an MilitaryGridReferenceSystem (MGRS) object with a custom datum(ellipsoid).
        /// </summary>
        /// <param name="latz">MGRS Latitude Band Grid Zone Designation (Letter)</param>
        /// <param name="longz">MGRS Longitude Band Grid Zone Designation (Number)</param>
        /// <param name="d">MGRS 100,000 Meter Square Identifier (2 Letter)</param>
        /// <param name="e">Easting</param>
        /// <param name="n">Northing</param>
        /// <param name="rad">Equatorial Radius</param>
        /// <param name="flt">Inverse Flattening</param>
        /// <example>
        /// <code>
        /// MilitaryGridReferenceSystem mgrs = new MilitaryGridReferenceSystem("N", 21, "SA", 66037, 61982, 6378160.000, 298.25);
        /// </code>
        /// </example>
        public MilitaryGridReferenceSystem(string latz, int longz, string d, double e, double n,double rad, double flt)
        {
            string digraphLettersE = "ABCDEFGHJKLMNPQRSTUVWXYZ";
            string digraphLettersN = "ABCDEFGHJKLMNPQRSTUV";
            if (longz < 1 || longz > 60) { Debug.WriteLine("Longitudinal zone out of range", "UTM longitudinal zones must be between 1-60."); }
            if (!Verify_Lat_Zone(latz)) { throw new ArgumentException("Latitudinal zone invalid", "UTM latitudinal zone was unrecognized."); }
            if (n < 0 || n > 10000000) { throw new ArgumentOutOfRangeException("Northing out of range", "Northing must be between 0-10,000,000."); }
            if (d.Count() < 2 || d.Count() > 2) { throw new ArgumentException("Digraph invalid", "MGRS Digraph was unrecognized."); }
            if (digraphLettersE.ToCharArray().ToList().Where(x => x.ToString() == d.ToUpper()[0].ToString()).Count() == 0) { throw new ArgumentException("Digraph invalid", "MGRS Digraph was unrecognized."); }
            if (digraphLettersN.ToCharArray().ToList().Where(x => x.ToString() == d.ToUpper()[1].ToString()).Count() == 0) { throw new ArgumentException("Digraph invalid", "MGRS Digraph was unrecognized."); }
            latZone = latz;
            longZone = longz;
            digraph = d;
            easting = e;
            northing = n;
          
            equatorialRadius = rad;
            inverseFlattening = flt;
        }

        /// <summary>
        /// Creates an MilitaryGridReferenceSystem (MGRS) object with a default WGS84 datum(ellipsoid).
        /// </summary>
        /// <param name="gridZone">MGRS Grid Zone Designation</param>
        /// <param name="d">MGRS 100,000 Meter Square Identifier (2 Letter)</param>
        /// <param name="e">Easting</param>
        /// <param name="n">Northing</param>
        /// <example>
        /// <code>
        /// MilitaryGridReferenceSystem mgrs = new MilitaryGridReferenceSystem("N", 21, "SA", 66037, 61982);
        /// </code>
        /// </example>
        public MilitaryGridReferenceSystem(string gridZone, string d, double e, double n)
        {

            string digraphLettersE = "ABCDEFGHJKLMNPQRSTUVWXYZ";
            string digraphLettersN = "ABCDEFGHJKLMNPQRSTUV";

            string resultString = Regex.Match(gridZone, @"\d+").Value;
            int longz;
            if(!int.TryParse(resultString, out longz))
            {
                throw new FormatException("The MGRS Grid Zone Designator format is invalid.");
            }

            string latz = gridZone.Replace(resultString, "");
        
            if (longz < 1 || longz > 60) { Debug.WriteLine("Longitudinal zone out of range", "UTM longitudinal zones must be between 1-60."); }
            if (!Verify_Lat_Zone(latz)) { throw new ArgumentException("Latitudinal zone invalid", "UTM latitudinal zone was unrecognized."); }
            if (n < 0 || n > 10000000) { throw new ArgumentOutOfRangeException("Northing out of range", "Northing must be between 0-10,000,000."); }
            if (d.Count() < 2 || d.Count() > 2) { throw new ArgumentException("Digraph invalid", "MGRS Digraph was unrecognized."); }
            if (digraphLettersE.ToCharArray().ToList().Where(x => x.ToString() == d.ToUpper()[0].ToString()).Count() == 0) { throw new ArgumentException("Digraph invalid", "MGRS Digraph was unrecognized."); }
            if (digraphLettersN.ToCharArray().ToList().Where(x => x.ToString() == d.ToUpper()[1].ToString()).Count() == 0) { throw new ArgumentException("Digraph invalid", "MGRS Digraph was unrecognized."); }
            latZone = latz;
            longZone = longz;
            digraph = d;
            easting = e;
            northing = n;
            //WGS84
            equatorialRadius = 6378137.0;
            inverseFlattening = 298.257223563;

        }
        /// <summary>
        /// Creates an MilitaryGridReferenceSystem (MGRS) object with a default WGS84 datum(ellipsoid).
        /// </summary>
        /// <param name="gridZone">MGRS Grid Zone Designation</param>
        /// <param name="d">MGRS 100,000 Meter Square Identifier (2 Letter)</param>
        /// <param name="e">Easting</param>
        /// <param name="n">Northing</param>
        /// <param name="rad">Equatorial Radius</param>
        /// <param name="flt">Inverse Flattening</param>
        /// <example>
        /// <code>
        /// MilitaryGridReferenceSystem mgrs = new MilitaryGridReferenceSystem("N", 21, "SA", 66037, 61982);
        /// </code>
        /// </example>
        public MilitaryGridReferenceSystem(string gridZone, string d, double e, double n, double rad, double flt)
        {

            string digraphLettersE = "ABCDEFGHJKLMNPQRSTUVWXYZ";
            string digraphLettersN = "ABCDEFGHJKLMNPQRSTUV";

            string resultString = Regex.Match(gridZone, @"\d+").Value;
            int longz;
            if (!int.TryParse(resultString, out longz))
            {
                throw new FormatException("The MGRS Grid Zone Designator format is invalid.");
            }

            string latz = gridZone.Replace(resultString, "");

            if (longz < 1 || longz > 60) { Debug.WriteLine("Longitudinal zone out of range", "UTM longitudinal zones must be between 1-60."); }
            if (!Verify_Lat_Zone(latz)) { throw new ArgumentException("Latitudinal zone invalid", "UTM latitudinal zone was unrecognized."); }
            if (n < 0 || n > 10000000) { throw new ArgumentOutOfRangeException("Northing out of range", "Northing must be between 0-10,000,000."); }
            if (d.Count() < 2 || d.Count() > 2) { throw new ArgumentException("Digraph invalid", "MGRS Digraph was unrecognized."); }
            if (digraphLettersE.ToCharArray().ToList().Where(x => x.ToString() == d.ToUpper()[0].ToString()).Count() == 0) { throw new ArgumentException("Digraph invalid", "MGRS Digraph was unrecognized."); }
            if (digraphLettersN.ToCharArray().ToList().Where(x => x.ToString() == d.ToUpper()[1].ToString()).Count() == 0) { throw new ArgumentException("Digraph invalid", "MGRS Digraph was unrecognized."); }
            latZone = latz;
            longZone = longz;
            digraph = d;
            easting = e;
            northing = n;
            //WGS84
            equatorialRadius = rad;
            inverseFlattening = flt;

        }

        private bool Verify_Lat_Zone(string l)
        {
            if (LatZones.longZongLetters.Where(x => x == l.ToUpper()).Count() != 1)
            {
                return false;
            }
            return true;
        }
      
        internal MilitaryGridReferenceSystem(UniversalTransverseMercator utm)
        {
            ToMGRS(utm);
        }
        internal void ToMGRS(UniversalTransverseMercator utm)
        {
            Digraphs digraphs = new Digraphs();
       
            string digraph1 = digraphs.getDigraph1(utm.LongZone, utm.Easting);
            string digraph2 = digraphs.getDigraph2(utm.LongZone, utm.Northing);

            digraph = digraph1 + digraph2;
            latZone = utm.LatZone;
            longZone = utm.LongZone;

            //String easting = String.valueOf((int)_easting);
            string e =  ((int)utm.Easting).ToString();
            if (e.Length < 5)
            {
                e = "00000" + ((int)utm.Easting).ToString();
            }
            e = e.Substring(e.Length - 5);
           
            easting = Convert.ToInt32(e);
            
            string n =  ((int)utm.Northing).ToString();
            if (n.Length < 5)
            {
                n = "0000" + ((int)utm.Northing).ToString();
            }
            n = n.Substring(n.Length - 5);
           
            northing = Convert.ToInt32(n);
            equatorialRadius = utm.equatorial_radius;
            inverseFlattening = utm.inverse_flattening;

            withinCoordinateSystemBounds = utm.WithinCoordinateSystemBounds;
        }

        /// <summary>
        /// Creates a Coordinate object from an MGRS/NATO UTM Coordinate
        /// </summary>
        /// <param name="mgrs">MilitaryGridReferenceSystem</param>
        /// <returns>Coordinate object</returns>
        /// <example>
        /// The following example creates (converts to) a geodetic Coordinate object based on a MGRS object.
        /// <code>
        /// MilitaryGridReferenceSystem mgrs = new MilitaryGridReferenceSystem("N", 21, "SA", 66037, 61982);
        /// Coordinate c = MilitaryGridReferenceSystem.MGRStoLatLong(mgrs);
        /// Console.WriteLine(c); //N 0º 33' 35.988" W 60º 0' 0.01"
        /// </code>
        /// </example>
        public static Coordinate MGRStoLatLong(MilitaryGridReferenceSystem mgrs)
        {
            string latz = mgrs.LatZone;
            string digraph = mgrs.Digraph;

            char eltr = digraph[0];
            char nltr = digraph[1];
      
            string digraphLettersE = "ABCDEFGHJKLMNPQRSTUVWXYZ";
            string digraphLettersN = "ABCDEFGHJKLMNPQRSTUV";
            string digraphLettersAll="";
            for (int lt = 1; lt < 25; lt++)
            {
                digraphLettersAll += "ABCDEFGHJKLMNPQRSTUV";
            }
           
            var eidx = digraphLettersE.IndexOf(eltr);
            var nidx = digraphLettersN.IndexOf(nltr);
            if (mgrs.LongZone / 2.0 == Math.Floor(mgrs.LongZone / 2.0))
            {
                nidx -= 5;  // correction for even numbered zones
            }

            var ebase = 100000 * (1 + eidx - 8 * Math.Floor(Convert.ToDouble(eidx) / 8));
            var latBand = digraphLettersE.IndexOf(latz);
            var latBandLow = 8 * latBand - 96;
            var latBandHigh = 8 * latBand - 88;
   
            if (latBand < 2)
            {        
                latBandLow = -90;
                latBandHigh = -80;               
            }
            else if (latBand == 21)
            {
                latBandLow = 72;
                latBandHigh = 84;
            }
            else if (latBand > 21)
            {
                latBandLow = 84;
                latBandHigh = 90;
            }

            var lowLetter = Math.Floor(100 + 1.11 * latBandLow);
            var highLetter = Math.Round(100 + 1.11 * latBandHigh);
  
            string latBandLetters = null;
            int l = Convert.ToInt32(lowLetter);
            int h = Convert.ToInt32(highLetter);
            if (mgrs.LongZone / 2.0 == Math.Floor(mgrs.LongZone / 2.0))
            {            
                latBandLetters = digraphLettersAll.Substring(l + 5, h + 5).ToString();
            }
            else
            {
                latBandLetters = digraphLettersAll.Substring(l, h).ToString();         
            }
            var nbase = 100000 * (lowLetter + latBandLetters.IndexOf(nltr));
            //latBandLetters.IndexOf(nltr) value causing incorrect Northing below -80
            var x = ebase + mgrs.Easting;
            var y = nbase + mgrs.Northing;
            if (y > 10000000)
            {
                y = y - 10000000;
            }
            if (nbase >= 10000000)
            {
                y = nbase + mgrs.northing - 10000000;
            }

            var southern = nbase < 10000000;
            UniversalTransverseMercator utm = new UniversalTransverseMercator(mgrs.LatZone, mgrs.LongZone, x, y);
            utm.equatorial_radius = mgrs.equatorialRadius;
            utm.inverse_flattening = mgrs.inverseFlattening;
            Coordinate c = UniversalTransverseMercator.ConvertUTMtoLatLong(utm);
           
            c.Set_Datum(mgrs.equatorialRadius, mgrs.inverseFlattening);
          
            return c;
        }

        /// <summary>
        /// Creates a Signed Degree double[] object from an MGRS/NATO UTM Coordinate
        /// </summary>
        /// <param name="mgrs">MilitaryGridReferenceSystem</param>
        /// <returns>Coordinate object</returns>
        /// <example>
        /// The following example creates (converts to) a geodetic Coordinate object based on a MGRS object.
        /// <code>
        /// MilitaryGridReferenceSystem mgrs = new MilitaryGridReferenceSystem("N", 21, "SA", 66037, 61982);
        /// double[] sd = MilitaryGridReferenceSystem.MGRStoSignedDegree(mgrs);
        /// Coordinate c = new Coordinate(sd[0],sd[1], new EagerLoad(false));
        /// Console.WriteLine(c); //N 0º 33' 35.988" W 60º 0' 0.01"
        /// </code>
        /// </example>
        public static double[] MGRStoSignedDegree(MilitaryGridReferenceSystem mgrs)
        {
            string latz = mgrs.LatZone;
            string digraph = mgrs.Digraph;

            char eltr = digraph[0];
            char nltr = digraph[1];

            string digraphLettersE = "ABCDEFGHJKLMNPQRSTUVWXYZ";
            string digraphLettersN = "ABCDEFGHJKLMNPQRSTUV";
            string digraphLettersAll = "";
            for (int lt = 1; lt < 25; lt++)
            {
                digraphLettersAll += "ABCDEFGHJKLMNPQRSTUV";
            }

            var eidx = digraphLettersE.IndexOf(eltr);
            var nidx = digraphLettersN.IndexOf(nltr);
            if (mgrs.LongZone / 2.0 == Math.Floor(mgrs.LongZone / 2.0))
            {
                nidx -= 5;  // correction for even numbered zones
            }

            var ebase = 100000 * (1 + eidx - 8 * Math.Floor(Convert.ToDouble(eidx) / 8));
            var latBand = digraphLettersE.IndexOf(latz);
            var latBandLow = 8 * latBand - 96;
            var latBandHigh = 8 * latBand - 88;

            if (latBand < 2)
            {
                latBandLow = -90;
                latBandHigh = -80;
            }
            else if (latBand == 21)
            {
                latBandLow = 72;
                latBandHigh = 84;
            }
            else if (latBand > 21)
            {
                latBandLow = 84;
                latBandHigh = 90;
            }

            var lowLetter = Math.Floor(100 + 1.11 * latBandLow);
            var highLetter = Math.Round(100 + 1.11 * latBandHigh);

            string latBandLetters = null;
            int l = Convert.ToInt32(lowLetter);
            int h = Convert.ToInt32(highLetter);
            if (mgrs.LongZone / 2.0 == Math.Floor(mgrs.LongZone / 2.0))
            {
                latBandLetters = digraphLettersAll.Substring(l + 5, h + 5).ToString();
            }
            else
            {
                latBandLetters = digraphLettersAll.Substring(l, h).ToString();
            }
            var nbase = 100000 * (lowLetter + latBandLetters.IndexOf(nltr));
            //latBandLetters.IndexOf(nltr) value causing incorrect Northing below -80
            var x = ebase + mgrs.Easting;
            var y = nbase + mgrs.Northing;
            if (y > 10000000)
            {
                y = y - 10000000;
            }
            if (nbase >= 10000000)
            {
                y = nbase + mgrs.northing - 10000000;
            }

            var southern = nbase < 10000000;
            UniversalTransverseMercator utm = new UniversalTransverseMercator(mgrs.LatZone, mgrs.LongZone, x, y);
            utm.equatorial_radius = mgrs.equatorialRadius;
            utm.inverse_flattening = mgrs.inverseFlattening;
            double[] sd = UniversalTransverseMercator.ConvertUTMtoSignedDegree(utm);

            return sd;
        }

        /// <summary>
        /// Default formatted MGRS string
        /// </summary>
        /// <returns>MGRS Formatted Coordinate String</returns>
        public override string ToString()
        {
            if (!withinCoordinateSystemBounds) { return ""; }//MGRS Coordinate is outside its reliable boundaries. Return empty.
            return longZone.ToString() + LatZone + " " + digraph + " " + ((int)easting).ToString("00000") + " " + ((int)northing).ToString("00000");
        }      
    }
}
