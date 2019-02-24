using System;
using System.Linq;
using System.Diagnostics;
using System.ComponentModel;

namespace CoordinateSharp
{
    public partial class MilitaryGridReferenceSystem
    {
        /// <summary>
        /// Create an MGRS object with WGS84 datum
        /// </summary>
        /// <param name="latz">Lat Zone</param>
        /// <param name="longz">Long Zone</param>
        /// <param name="d">Digraph</param>
        /// <param name="e">Easting</param>
        /// <param name="n">Northing</param>
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
        /// Create an MGRS object with custom datum
        /// </summary>
        /// <param name="latz">Lat Zone</param>
        /// <param name="longz">Long Zone</param>
        /// <param name="d">Digraph</param>
        /// <param name="e">Easting</param>
        /// <param name="n">Northing</param>
        /// <param name="rad">Equatorial Radius</param>
        /// <param name="flt">Inverse Flattening</param>
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
        /// MGRS Default String Format
        /// </summary>
        /// <returns>MGRS Formatted Coordinate String</returns>
        public override string ToString()
        {
            if (!withinCoordinateSystemBounds) { return ""; }//MGRS Coordinate is outside its reliable boundaries. Return empty.
            return longZone.ToString() + LatZone + " " + digraph + " " + ((int)easting).ToString("00000") + " " + ((int)northing).ToString("00000");
        }      
    }
}
