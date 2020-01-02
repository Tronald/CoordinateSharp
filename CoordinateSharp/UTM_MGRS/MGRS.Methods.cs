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
            Construct_MGRS(latz, longz, d, e, n, 6378137.0, 298.257223563);
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
            Construct_MGRS(latz, longz, d, e, n, rad, flt);
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
        /// MilitaryGridReferenceSystem mgrs = new MilitaryGridReferenceSystem("21N", "SA", 66037, 61982);
        /// </code>
        /// </example>
        public MilitaryGridReferenceSystem(string gridZone, string d, double e, double n)
        {
            string resultString = Regex.Match(gridZone, @"\d+").Value;
            int longz;
            if(!int.TryParse(resultString, out longz))
            {
                //Check Polar First
                if (!Regex.Match(gridZone.ToUpper(), "[ABZY]").Success)
                {
                    throw new FormatException("The MGRS Grid Zone Designator format is invalid.");
                }
                else{ systemType = MGRS_Type.MGRS_Polar; resultString = "0"; }
            }

            string latz = gridZone.Replace(resultString, "");

            Construct_MGRS(latz, longz, d, e, n, 6378137.0, 298.257223563);
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
        /// MilitaryGridReferenceSystem mgrs = new MilitaryGridReferenceSystem("21N", "SA", 66037, 61982);
        /// </code>
        /// </example>
        public MilitaryGridReferenceSystem(string gridZone, string d, double e, double n, double rad, double flt)
        {        
            string resultString = Regex.Match(gridZone, @"\d+").Value;
            int longz;
            if (!int.TryParse(resultString, out longz))
            {
                //Check Polar First
                if (!Regex.Match(gridZone.ToUpper(), "[ABZY]").Success)
                {
                    throw new FormatException("The MGRS Grid Zone Designator format is invalid.");
                }
                else { systemType = MGRS_Type.MGRS_Polar; resultString = "0"; }
            }

            string latz = gridZone.Replace(resultString, "");

            Construct_MGRS(latz, longz, d, e, n, flt, rad);
        }

        /// <summary>
        /// Construct MGRS
        /// </summary>       
        private void Construct_MGRS(string latz, int longz, string d, double e, double n, double rad, double flt)
        {
            Regex rg = new Regex("[aAbByYzZ]");
            Match m = rg.Match(latz);                    

            if (m.Success)
            {
                systemType = MGRS_Type.MGRS_Polar;
                if (longz != 0)
                {
                    Debug.WriteLine("MGRS Polar Longitudinal Zone Invalid", "You passed an MGRS Polar coordinate. The longitudinal zone should be set to 0.");
                }
            }

            Digraphs ds = new Digraphs(systemType, latz);

            if (longz < 1 || longz > 60) { Debug.WriteLine("Longitudinal zone out of range", "UTM longitudinal zones must be between 1-60."); }
            if (!Verify_Lat_Zone(latz)) { throw new ArgumentException("Latitudinal zone invalid", "UTM latitudinal zone was unrecognized."); }
            if (n < 0 || n > 10000000) { throw new ArgumentOutOfRangeException("Northing out of range", "Northing must be between 0-10,000,000."); }
            if (d.Count() < 2 || d.Count() > 2) { throw new ArgumentException("Digraph invalid", "MGRS Digraph was unrecognized."); }
            if (ds.digraph1.Where(x => x.Letter == d.ToUpper()[0].ToString()).Count() == 0) { throw new ArgumentException("Digraph invalid", "MGRS Digraph was unrecognized."); }
            if (ds.digraph2.Where(x => x.Letter == d.ToUpper()[1].ToString()).Count() == 0) { throw new ArgumentException("Digraph invalid", "MGRS Digraph was unrecognized."); }
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
            Regex rg = new Regex("[ABYZ]");
            Match m = rg.Match(utm.LatZone.ToUpper());

            string digraph1;
            string digraph2;

            Digraphs digraphs;
       
            if (m.Success)
            {
                systemType = MGRS_Type.MGRS_Polar;
                digraphs = new Digraphs(systemType, utm.LatZone);
                digraph1 = digraphs.getDigraph1(utm.LongZone, utm.Easting);
                digraph2 = digraphs.getDigraph2_Polar(utm.LongZone, utm.Northing);

            }
            else
            {
                systemType = MGRS_Type.MGRS;
                digraphs = new Digraphs(systemType, utm.LatZone);
                digraph1 = digraphs.getDigraph1(utm.LongZone, utm.Easting);
                digraph2 = digraphs.getDigraph2(utm.LongZone, utm.Northing);
            }
 

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
            return MGRStoLatLong(mgrs, new EagerLoad());
        }
       
        /// <summary>
        /// Creates a Coordinate object from an MGRS/NATO UTM Coordinate
        /// </summary>
        /// <param name="mgrs">MilitaryGridReferenceSystem</param>
        /// <param name="eagerLoad">EagerLoad</param>
        /// <returns>Coordinate object</returns>
        /// <example>
        /// The following example creates (converts to) a geodetic Coordinate object based on a MGRS object.
        /// <code>
        /// MilitaryGridReferenceSystem mgrs = new MilitaryGridReferenceSystem("N", 21, "SA", 66037, 61982);
        /// Coordinate c = MilitaryGridReferenceSystem.MGRStoLatLong(mgrs, new EagerLoad(false));
        /// Console.WriteLine(c); //N 0º 33' 35.988" W 60º 0' 0.01"
        /// </code>
        /// </example>
        public static Coordinate MGRStoLatLong(MilitaryGridReferenceSystem mgrs, EagerLoad eagerLoad)
        {
            if (mgrs.systemType == MGRS_Type.MGRS_Polar) { return MGRS_Polar_ToLatLong(mgrs); }

            string latz = mgrs.LatZone;
            string digraph = mgrs.Digraph;

            char eltr = digraph[0];
            char nltr = digraph[1];

            string digraphLettersE = "ABCDEFGHJKLMNPQRSTUVWXYZ";
            string digraphLettersN = "ABCDEFGHJKLMNPQRSTUV";

            string digraphLettersAll = "";

            for (int lt = 1; lt < 25; lt++)
            {
                digraphLettersAll += digraphLettersN;
            }

            var eidx = digraphLettersE.IndexOf(eltr);

            var pbase = 100000;

            double fl = Math.Floor(Convert.ToDouble(eidx) / 8);

            double subbase = 1 + eidx - 8 * fl;

            var ebase = pbase * subbase;
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

            if (mgrs.systemType != MGRS_Type.MGRS_Polar)
            {
                if (y > 10000000)
                {
                    y = y - 10000000;
                }
                if (nbase >= 10000000)
                {
                    y = nbase + mgrs.northing - 10000000;
                }
            }

            UniversalTransverseMercator utm = new UniversalTransverseMercator(mgrs.LatZone, mgrs.LongZone, x, y);
            utm.equatorial_radius = mgrs.equatorialRadius;
            utm.inverse_flattening = mgrs.inverseFlattening;
            Coordinate c = UniversalTransverseMercator.ConvertUTMtoLatLong(utm, eagerLoad);

            c.Set_Datum(mgrs.equatorialRadius, mgrs.inverseFlattening);

            return c;
        }
        private static Coordinate MGRS_Polar_ToLatLong(MilitaryGridReferenceSystem mgrs)
        {
            //WORKING
            bool isNorth = true;
            if (mgrs.latZone.ToUpper() == "A" || mgrs.latZone.ToUpper() == "B") { isNorth = false; }

            string latz = mgrs.LatZone;
            string digraph = mgrs.Digraph;

            char eltr = digraph[0];
            char nltr = digraph[1];


            string digraphLettersE;
            if (!isNorth) 
            {
               digraphLettersE = "KLPQRSTUXYZABCFGH"; 
            }
            else { digraphLettersE = "RSTUXYZABCFGHJ"; }

            string digraphLettersN;
            if (!isNorth) { digraphLettersN = "VWXYBCDEFGHJKLMNPQRSTUVWXYZ"; }
            else { digraphLettersN = "ABCDEFGHJKLMNP"; }


            string digraphLettersAll = "";

            for (int lt = 1; lt < 31; lt++)
            {
                digraphLettersAll += digraphLettersN;
            }

            var eidx = digraphLettersE.IndexOf(eltr);
         
            //Offsets are set due to less Easting Identifiers.
            //North has 4 less than S
            double offset = 9;
            if (isNorth) { offset = 13; }

            if (mgrs.latZone == "B" && eidx < offset && mgrs.easting!=0) { eidx += 18; }
     

            double subbase = eidx + offset;

            var ebase = 100000 * subbase;
            var latBand = digraphLettersE.IndexOf(latz);
            var latBandLow = 8 * latBand - 96;
            var latBandHigh = 8 * latBand - 88;

            if (!isNorth)
            {
                latBandLow = -90;
                latBandHigh = -80;
            }
            else
            {
                latBandLow = 84;
                latBandHigh = 90;
            }

            var lowLetter = Math.Floor(100 + 1.11 * latBandLow);
            var highLetter = Math.Round(100 + 1.11 * latBandHigh);

            string latBandLetters = null;
            int l = Convert.ToInt32(lowLetter);          
            int h = Convert.ToInt32(highLetter+7);

            if (mgrs.LongZone / 2.0 == Math.Floor(mgrs.LongZone / 2.0))
            {
                latBandLetters = digraphLettersAll.Substring(l + 5, h + 5).ToString();
            }
            else
            {
                latBandLetters = digraphLettersAll.Substring(l, h).ToString();
            }

            //North offset + 4 due to lower band count.
            double nOffset = 13;
            if (!isNorth) { nOffset = 10; }
            else { latBandLetters = digraphLettersN; }
            int index = latBandLetters.IndexOf(nltr);
           
            if (index == -1 && nltr=='A') { index -=1; } //ALPHA PATCH

            //int subset = 0;
            //if ((latz == "Y" || latz == "Z") && (nOffset+index)>25 && (ebase> 2100000 || ebase<2000000) && ebase!= 2000000) { subset = -14; }
            var nbase = 100000 * (index+nOffset);
            
            var x = ebase + mgrs.Easting;
            var y = nbase + mgrs.Northing;

            if (mgrs.systemType != MGRS_Type.MGRS_Polar)
            {
                if (y > 10000000)
                {
                    y = y - 10000000;
                }
                if (nbase >= 10000000)
                {
                    y = nbase + mgrs.northing - 10000000;
                }
            }

           // Debug.WriteLine("MGRS {0} {1}", x, y);
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

            Regex upsCheck = new Regex("[AaBbYyZz]");
            if (upsCheck.IsMatch(mgrs.latZone))
            {
                Coordinate c = MGRStoLatLong(mgrs);
                return new double[] { c.Latitude.ToDouble(), c.Longitude.ToDouble() };
            }

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
            if (systemType == MGRS_Type.MGRS)
            {
                return longZone.ToString() + LatZone + " " + digraph + " " + ((int)easting).ToString("00000") + " " + ((int)northing).ToString("00000");
            }
            else
            {
                return LatZone + " " + digraph + " " + ((int)easting).ToString("00000") + " " + ((int)northing).ToString("00000");
            }
        }      
    }
}
