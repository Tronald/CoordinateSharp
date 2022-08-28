using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CoordinateSharp
{
    internal partial class FormatFinder
    {
        public static bool TryWebMercator(string ns, out string[] webMercator)
        {
            //X,Y
            //mE,mN
            //E,N
            webMercator = null;
            Regex r = new Regex(@"-?([0-9]+\.?\,?[0-9]*|\.[0-9]+)\s?((?i)me|y|e|) -?([0-9]+\.?\,?[0-9]*|\.[0-9]+)\s?((?i)mn|x|n|)");
            Match match = r.Match(ns);
            if (!match.Success || match.Value != ns) { return false; }
          
            string s = ns.ToUpper().Replace("ME", "").Replace("MN", "").Replace("E", "")
                .Replace("N", "").Replace("X", "").Replace("Y", "").Trim();
            string[] sA = s.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            if (sA.Count() != 2) { return false; }

            double easting;
            double northing;


            if (!double.TryParse(sA[0], NumberStyles.Any, CultureInfo.InvariantCulture, out easting))
            { return false; }
            if (!double.TryParse(sA[1], NumberStyles.Any, CultureInfo.InvariantCulture, out northing))
            { return false; }

            if (easting < -DataValues.WebMercatorEastingLimit || easting > DataValues.WebMercatorEastingLimit) { return false; }
            if (northing < -DataValues.WebMercatorNorthingLimit || easting > DataValues.WebMercatorEastingLimit) { return false; }

            webMercator = new string[] { easting.ToString(), northing.ToString() };
            return true;

        }
    }
}
