/*
CoordinateSharp is a .NET standard library that is intended to ease geographic coordinate 
format conversions and location based celestial calculations.
https://github.com/Tronald/CoordinateSharp

Many celestial formulas in this library are based on Jean Meeus's 
Astronomical Algorithms (2nd Edition). Comments that reference only a chapter
are referring to this work.

License

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

For more information, please contact Signature Group, LLC at this address: sales@signatgroup.com
*/
using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.Globalization;

namespace CoordinateSharp
{
    internal class FormatFinder
    {
        //Add main to Coordinate and tunnel to Format class. Add private methods to format.
        //WHEN PARSING NO EXCPETIONS FOR OUT OF RANGE ARGS WILL BE THROWN
        public static bool TryParse(string coordString, CartesianType ct, out Coordinate c)
        {
            //Turn of eagerload for efficiency
            EagerLoad eg = new EagerLoad();
            eg.Cartesian = false;
            eg.Celestial = false;
            eg.UTM_MGRS = false;

            c = new Coordinate(eg);
            string s = coordString;
            s = s.Trim(); //Trim all spaces before and after string
            double[] d;
            //Try Signed Degree
            if (TrySignedDegree(s, out d))
            {
                try
                {
                    c = new Coordinate(d[0], d[1], eg);
                    c.Parse_Format = Parse_Format_Type.Signed_Degree;
                    return true;
                }
                catch
                {//Parser failed try next method 
                }
            }

            //Try Decimal Degree
            if (TryDecimalDegree(s, out d))
            {
                try
                {
                    c = new Coordinate(d[0], d[1], eg);
                    c.Parse_Format = Parse_Format_Type.Decimal_Degree;
                    return true;
                }
                catch
                {//Parser failed try next method 
                }
            }
            //Try DDM
            if (TryDegreeDecimalMinute(s, out d))
            {
                try
                {
                    //0 Lat Degree
                    //1 Lat Minute
                    //2 Lat Direction (0 = N, 1 = S)
                    //3 Long Degree
                    //4 Long Minute
                    //5 Long Direction (0 = E, 1 = W)
                    CoordinatesPosition latP = CoordinatesPosition.N;
                    CoordinatesPosition lngP = CoordinatesPosition.E;
                    if (d[2] != 0) { latP = CoordinatesPosition.S; }
                    if (d[5] != 0) { lngP = CoordinatesPosition.W; }
                    CoordinatePart lat = new CoordinatePart((int)d[0], d[1], latP);
                    CoordinatePart lng = new CoordinatePart((int)d[3], d[4], lngP);
                    c = new Coordinate(eg);
                    c.Latitude = lat;
                    c.Longitude = lng;
                    c.Parse_Format = Parse_Format_Type.Degree_Decimal_Minute;
                    return true;
                }
                catch
                {//Parser failed try next method 
                }
            }
            //Try DMS
            if (TryDegreeMinuteSecond(s, out d))
            {
                try
                {
                    //0 Lat Degree
                    //1 Lat Minute
                    //2 Lat Second
                    //3 Lat Direction (0 = N, 1 = S)
                    //4 Long Degree
                    //5 Long Minute
                    //6 Long Second
                    //7 Long Direction (0 = E, 1 = W)
                    CoordinatesPosition latP = CoordinatesPosition.N;
                    CoordinatesPosition lngP = CoordinatesPosition.E;
                    if (d[3] != 0) { latP = CoordinatesPosition.S; }
                    if (d[7] != 0) { lngP = CoordinatesPosition.W; }

                    CoordinatePart lat = new CoordinatePart((int)d[0], (int)d[1], d[2], latP);
                    CoordinatePart lng = new CoordinatePart((int)d[4], (int)d[5], d[6], lngP);
                    c = new Coordinate(eg);
                    c.Latitude = lat;
                    c.Longitude = lng;
                    c.Parse_Format = Parse_Format_Type.Degree_Minute_Second;
                    return true;
                }
                catch
                {//Parser failed try next method 
                }
            }

            string[] um;
            //Try MGRS
            if (TryMGRS(s, out um))
            {
                try
                {
                    double zone = Convert.ToDouble(um[0]);
                    double easting = Convert.ToDouble(um[3]);
                    double northing = Convert.ToDouble(um[4]);
                    MilitaryGridReferenceSystem mgrs = new MilitaryGridReferenceSystem(um[1], (int)zone, um[2], easting, northing);
                    c = MilitaryGridReferenceSystem.MGRStoLatLong(mgrs);
                    c.Parse_Format = Parse_Format_Type.MGRS;
                    return true;
                }
                catch
                {//Parser failed try next method 
                }
            }
            //Try UTM
            if (TryUTM(s, out um))
            {
                try
                {
                    double zone = Convert.ToDouble(um[0]);
                    double easting = Convert.ToDouble(um[2]);
                    double northing = Convert.ToDouble(um[3]);
                    UniversalTransverseMercator utm = new UniversalTransverseMercator(um[1], (int)zone, easting, northing);
                    c = UniversalTransverseMercator.ConvertUTMtoLatLong(utm);
                    c.Parse_Format = Parse_Format_Type.UTM;
                    return true;
                }
                catch
                {//Parser failed try next method 
                }
            }
            //Try Cartesian
            if (TryCartesian(s.ToUpper().Replace("KM", "").Replace("X", "").Replace("Y", "").Replace("Z", ""), out d))
            {
                if (ct == CartesianType.Cartesian)
                {
                    try
                    {
                        Cartesian cart = new Cartesian(d[0], d[1], d[2]);
                        c = Cartesian.CartesianToLatLong(cart);
                        c.Parse_Format = Parse_Format_Type.Cartesian_Spherical;
                        return true;
                    }
                    catch
                    {//Parser failed try next method 
                    }
                }
                if (ct == CartesianType.ECEF)
                {
                    try
                    {
                        ECEF ecef = new ECEF(d[0], d[1], d[2]);
                        c = ECEF.ECEFToLatLong(ecef);
                        c.Parse_Format = Parse_Format_Type.Cartesian_ECEF;
                        return true;
                    }
                    catch
                    {//Parser failed try next method 
                    }
                }
            }

            c = null;
            return false;
        }

        #region PARSERS
        private static bool TrySignedDegree(string s, out double[] d)
        {
            d = null;
            if (Regex.Matches(s, @"[a-zA-Z]").Count != 0) { return false; } //Should contain no letters

            string[] sA = SpecialSplit(s, false);
            double lat;
            double lng;

            double degLat;
            double minLat; //Minutes & MinSeconds
            double secLat;

            int signLat = 1;

            double degLng;
            double minLng; //Minutes & MinSeconds
            double secLng;

            int signLng = 1;

            switch (sA.Count())
            {
                case 2:
                    if (!double.TryParse(sA[0], NumberStyles.Any, CultureInfo.InvariantCulture, out lat))
                    { return false; }
                    if (!double.TryParse(sA[1], NumberStyles.Any, CultureInfo.InvariantCulture, out lng))
                    { return false; }
                    d = new double[] { lat, lng };
                    return true;
                case 4:
                    if (!double.TryParse(sA[0], NumberStyles.Any, CultureInfo.InvariantCulture, out degLat))
                    { return false; }
                    if (!double.TryParse(sA[1], NumberStyles.Any, CultureInfo.InvariantCulture, out minLat))
                    { return false; }
                    if (!double.TryParse(sA[2], NumberStyles.Any, CultureInfo.InvariantCulture, out degLng))
                    { return false; }
                    if (!double.TryParse(sA[3], NumberStyles.Any, CultureInfo.InvariantCulture, out minLng))
                    { return false; }

                    if (degLat < 0) { signLat = -1; }
                    if (degLng < 0) { signLng = -1; }
                    if (minLat >= 60 || minLat < 0) { return false; } //Handle in parser as degree will be incorrect.
                    if (minLng >= 60 || minLng < 0) { return false; } //Handle in parser as degree will be incorrect.
                    lat = (Math.Abs(degLat) + (minLat / 60.0)) * signLat;
                    lng = (Math.Abs(degLng) + (minLng / 60.0)) * signLng;
                    d = new double[] { lat, lng };
                    return true;
                case 6:
                    if (!double.TryParse(sA[0], NumberStyles.Any, CultureInfo.InvariantCulture, out degLat))
                    { return false; }
                    if (!double.TryParse(sA[1], NumberStyles.Any, CultureInfo.InvariantCulture, out minLat))
                    { return false; }
                    if (!double.TryParse(sA[2], NumberStyles.Any, CultureInfo.InvariantCulture, out secLat))
                    { return false; }
                    if (!double.TryParse(sA[3], NumberStyles.Any, CultureInfo.InvariantCulture, out degLng))
                    { return false; }
                    if (!double.TryParse(sA[4], NumberStyles.Any, CultureInfo.InvariantCulture, out minLng))
                    { return false; }
                    if (!double.TryParse(sA[5], NumberStyles.Any, CultureInfo.InvariantCulture, out secLng))
                    { return false; }
                    if (degLat < 0) { signLat = -1; }
                    if (degLng < 0) { signLng = -1; }
                    if (minLat >= 60 || minLat < 0) { return false; } //Handle in parser as degree will be incorrect.
                    if (minLng >= 60 || minLng < 0) { return false; } //Handle in parser as degree will be incorrect.
                    if (secLat >= 60 || secLat < 0) { return false; } //Handle in parser as degree will be incorrect.
                    if (secLng >= 60 || secLng < 0) { return false; } //Handle in parser as degree will be incorrect.
                    lat = (Math.Abs(degLat) + (minLat / 60.0) + (secLat / 3600)) * signLat;
                    lng = (Math.Abs(degLng) + (minLng / 60.0) + (secLng / 3600)) * signLng;
                    d = new double[] { lat, lng };
                    return true;
                default:
                    return false;
            }
        }
        private static bool TryDecimalDegree(string s, out double[] d)
        {
            d = null;
            if (Regex.Matches(s, @"[a-zA-Z]").Count != 2) { return false; } //Should only contain 1 letter.

            string[] sA = SpecialSplit(s, true);
            if (sA.Count() == 2 || sA.Count() == 4)
            {
                double lat;
                double lng;

                double latR = 1; //Sets negative if South
                double lngR = 1; //Sets negative if West

                //Contact get brin directional indicator together with string
                if (sA.Count() == 4)
                {
                    sA[0] += sA[1];
                    sA[1] = sA[2] + sA[3];
                }

                //Find Directions
                if (!sA[0].Contains("N") && !sA[0].Contains("n"))
                {
                    if (!sA[0].Contains("S") && !sA[0].Contains("s"))
                    {
                        return false;//No Direction Found
                    }
                    latR = -1;
                }
                if (!sA[1].Contains("E") && !sA[1].Contains("e"))
                {
                    if (!sA[1].Contains("W") && !sA[1].Contains("w"))
                    {
                        return false;//No Direction Found
                    }
                    lngR = -1;
                }

                sA[0] = Regex.Replace(sA[0], "[^0-9.]", "");
                sA[1] = Regex.Replace(sA[1], "[^0-9.]", "");

                if (!double.TryParse(sA[0], NumberStyles.Any, CultureInfo.InvariantCulture, out lat))
                { return false; }
                if (!double.TryParse(sA[1], NumberStyles.Any, CultureInfo.InvariantCulture, out lng))
                { return false; }
                lat *= latR;
                lng *= lngR;
                d = new double[] { lat, lng };
                return true;
            }

            return false;
        }
        private static bool TryDegreeDecimalMinute(string s, out double[] d)
        {
            d = null;
            if (Regex.Matches(s, @"[a-zA-Z]").Count != 2) { return false; } //Should only contain 1 letter.

            string[] sA = SpecialSplit(s, true);
            if (sA.Count() == 4 || sA.Count() == 6)
            {
                double latD;
                double latMS;
                double lngD;
                double lngMS;

                double latR = 0; //Sets 1 if South
                double lngR = 0; //Sets 1 if West

                //Contact get in order to combine directional indicator together with string
                //Should reduce 6 items to 4
                if (sA.Count() == 6)
                {
                    if (char.IsLetter(sA[0][0])) { sA[0] += sA[1]; sA[1] = sA[2]; }
                    else if (char.IsLetter(sA[1][0])) { sA[0] += sA[1]; sA[1] = sA[2]; }
                    else if (char.IsLetter(sA[2][0])) { sA[0] += sA[2]; }
                    else { return false; }

                    if (char.IsLetter(sA[3][0])) { sA[3] += sA[4]; sA[4] = sA[5]; }
                    else if (char.IsLetter(sA[4][0])) { sA[3] += sA[4]; sA[4] = sA[5]; }
                    else if (char.IsLetter(sA[5][0])) { sA[3] += sA[5]; }
                    else { return false; }

                    //Shift values for below logic
                    sA[2] = sA[3];
                    sA[3] = sA[4];
                }

                //Find Directions
                if (!sA[0].Contains("N") && !sA[0].Contains("n") && !sA[1].Contains("N") && !sA[1].Contains("n"))
                {
                    if (!sA[0].Contains("S") && !sA[0].Contains("s") && !sA[1].Contains("S") && !sA[1].Contains("s"))
                    {
                        return false;//No Direction Found
                    }
                    latR = 1;
                }
                if (!sA[2].Contains("E") && !sA[2].Contains("e") && !sA[3].Contains("E") && !sA[3].Contains("e"))
                {
                    if (!sA[2].Contains("W") && !sA[2].Contains("w") && !sA[3].Contains("W") && !sA[3].Contains("w"))
                    {
                        return false;//No Direction Found
                    }
                    lngR = 1;
                }

                sA[0] = Regex.Replace(sA[0], "[^0-9.]", "");
                sA[1] = Regex.Replace(sA[1], "[^0-9.]", "");
                sA[2] = Regex.Replace(sA[2], "[^0-9.]", "");
                sA[3] = Regex.Replace(sA[3], "[^0-9.]", "");

                if (!double.TryParse(sA[0], NumberStyles.Any, CultureInfo.InvariantCulture, out latD))
                { return false; }
                if (!double.TryParse(sA[1], NumberStyles.Any, CultureInfo.InvariantCulture, out latMS))
                { return false; }
                if (!double.TryParse(sA[2], NumberStyles.Any, CultureInfo.InvariantCulture, out lngD))
                { return false; }
                if (!double.TryParse(sA[3], NumberStyles.Any, CultureInfo.InvariantCulture, out lngMS))
                { return false; }

                d = new double[] { latD, latMS, latR, lngD, lngMS, lngR };
                return true;
            }
            return false;
        }
        private static bool TryDegreeMinuteSecond(string s, out double[] d)
        {
            d = null;
            if (Regex.Matches(s, @"[a-zA-Z]").Count != 2) { return false; } //Should only contain 1 letter.

            string[] sA = SpecialSplit(s, true);
            if (sA.Count() == 6 || sA.Count() == 8)
            {
                double latD;
                double latM;
                double latS;
                double lngD;
                double lngM;
                double lngS;

                double latR = 0; //Sets 1 if South
                double lngR = 0; //Sets 1 if West

                //Contact get in order to combine directional indicator together with string
                //Should reduce 8 items to 6
                if (sA.Count() == 8)
                {
                    if (char.IsLetter(sA[0][0])) { sA[0] += sA[1]; sA[1] = sA[2]; sA[2] = sA[3]; }
                    else if (char.IsLetter(sA[1][0])) { sA[0] += sA[1]; sA[1] = sA[2]; sA[2] = sA[3]; }
                    else if (char.IsLetter(sA[3][0])) { sA[0] += sA[3]; }
                    else { return false; }

                    if (char.IsLetter(sA[4][0])) { sA[4] += sA[5]; sA[5] = sA[6]; sA[6] = sA[7]; }
                    else if (char.IsLetter(sA[5][0])) { sA[4] += sA[5]; sA[5] = sA[6]; sA[6] = sA[7]; }
                    else if (char.IsLetter(sA[7][0])) { sA[4] += sA[7]; }
                    else { return false; }

                    //Shift values for below logic
                    sA[3] = sA[4];
                    sA[4] = sA[5];
                    sA[5] = sA[6];
                }

                //Find Directions
                if (!sA[0].Contains("N") && !sA[0].Contains("n") && !sA[2].Contains("N") && !sA[2].Contains("n"))
                {
                    if (!sA[0].Contains("S") && !sA[0].Contains("s") && !sA[2].Contains("S") && !sA[2].Contains("s"))
                    {
                        return false;//No Direction Found
                    }
                    latR = 1;
                }
                if (!sA[3].Contains("E") && !sA[3].Contains("e") && !sA[5].Contains("E") && !sA[5].Contains("e"))
                {
                    if (!sA[3].Contains("W") && !sA[3].Contains("w") && !sA[5].Contains("W") && !sA[5].Contains("w"))
                    {
                        return false;//No Direction Found
                    }
                    lngR = 1;
                }
                sA[0] = Regex.Replace(sA[0], "[^0-9.]", "");
                sA[1] = Regex.Replace(sA[1], "[^0-9.]", "");
                sA[2] = Regex.Replace(sA[2], "[^0-9.]", "");
                sA[3] = Regex.Replace(sA[3], "[^0-9.]", "");
                sA[4] = Regex.Replace(sA[4], "[^0-9.]", "");
                sA[5] = Regex.Replace(sA[5], "[^0-9.]", "");

                if (!double.TryParse(sA[0], NumberStyles.Any, CultureInfo.InvariantCulture, out latD))
                { return false; }
                if (!double.TryParse(sA[1], NumberStyles.Any, CultureInfo.InvariantCulture, out latM))
                { return false; }
                if (!double.TryParse(sA[2], NumberStyles.Any, CultureInfo.InvariantCulture, out latS))
                { return false; }
                if (!double.TryParse(sA[3], NumberStyles.Any, CultureInfo.InvariantCulture, out lngD))
                { return false; }
                if (!double.TryParse(sA[4], NumberStyles.Any, CultureInfo.InvariantCulture, out lngM))
                { return false; }
                if (!double.TryParse(sA[5], NumberStyles.Any, CultureInfo.InvariantCulture, out lngS))
                { return false; }

                d = new double[] { latD, latM, latS, latR, lngD, lngM, lngS, lngR };
                return true;
            }
            return false;
        }
        private static bool TryMGRS(string s, out string[] mgrs)
        {
            mgrs = null;
            string[] sA = SpecialSplit(s, false);
            if (sA.Count() == 4 || sA.Count() == 5)
            {
                double zone;
                string zoneL;
                string diagraph;
                double easting;
                double northing;

                if (sA.Count() == 5)
                {
                    if (char.IsLetter(sA[0][0])) { sA[0] += sA[1]; sA[1] = sA[2]; sA[2] = sA[3]; }
                    else if (char.IsLetter(sA[1][0])) { sA[0] += sA[1]; sA[1] = sA[2]; sA[2] = sA[3]; }
                    else { return false; }
                }
                zoneL = new string(sA[0].Where(Char.IsLetter).ToArray());
                if (zoneL == string.Empty) { return false; }
                if (zoneL.Count() != 1) { return false; }
                sA[0] = Regex.Replace(sA[0], "[^0-9.]", "");
                diagraph = sA[1];
                if(diagraph.Count() != 2) { return false; }
                if (!double.TryParse(sA[0], NumberStyles.Any, CultureInfo.InvariantCulture, out zone))
                { return false; }
                if (!double.TryParse(sA[2], NumberStyles.Any, CultureInfo.InvariantCulture, out easting))
                { return false; }
                if (!double.TryParse(sA[3], NumberStyles.Any, CultureInfo.InvariantCulture, out northing))
                { return false; }

                mgrs = new string[] { zone.ToString(), zoneL, diagraph, easting.ToString(), northing.ToString() };
                return true;
            }
            return false;
        }
        private static bool TryUTM(string s, out string[] utm)
        {
            utm = null;
            string[] sA = SpecialSplit(s, false);
            if (sA.Count() == 3 || sA.Count() == 4)
            {
                double zone;
                string zoneL;
                double easting;
                double northing;

                if (sA.Count() == 4)
                {

                    if (char.IsLetter(sA[0][0])) { sA[0] += sA[1]; sA[1] = sA[2]; sA[2] = sA[3]; }
                    else if (char.IsLetter(sA[1][0])) { sA[0] += sA[1]; sA[1] = sA[2]; sA[2] = sA[3]; }
                    else { return false; }
                }
               
                zoneL = new string(sA[0].Where(Char.IsLetter).ToArray());
                if (zoneL == string.Empty) { return false; }
                if(zoneL.Count() != 1) { return false; }
                sA[0] = Regex.Replace(sA[0], "[^0-9.]", "");

                if (!double.TryParse(sA[0], NumberStyles.Any, CultureInfo.InvariantCulture, out zone))
                { return false; }
                if (!double.TryParse(sA[1], NumberStyles.Any, CultureInfo.InvariantCulture, out easting))
                { return false; }
                if (!double.TryParse(sA[2], NumberStyles.Any, CultureInfo.InvariantCulture, out northing))
                { return false; }

                utm = new string[] { zone.ToString(), zoneL, easting.ToString(), northing.ToString() };
                return true;
            }
            return false;
        }
     
        private static bool TryCartesian(string s, out double[] d)
        {
            d = null;
            string[] sA = SpecialSplit(s, false);

            if (sA.Count() == 3)
            {
                double x;
                double y;
                double z;
                if (!double.TryParse(sA[0], NumberStyles.Any, CultureInfo.InvariantCulture, out x))
                { return false; }
                if (!double.TryParse(sA[1], NumberStyles.Any, CultureInfo.InvariantCulture, out y))
                { return false; }
                if (!double.TryParse(sA[2], NumberStyles.Any, CultureInfo.InvariantCulture, out z))
                { return false; }
                d = new double[] { x, y, z };
                return true;
            }
            return false;
        }
        #endregion

        //KEEP DASHES FOR SIGNED AND CARTESIAN AS THEY ARE USED FOR NEGATVE VALUES
        private static string[] SpecialSplit(string s, bool removeDashes)
        {
            s = s.Replace("°", " ");
            s = s.Replace("º", " ");
            s = s.Replace("'", " ");
            s = s.Replace("\"", " ");
            s = s.Replace(",", " ");
            s = s.Replace("mE", " ");
            s = s.Replace("mN", " ");
            if (removeDashes)
            {
                s = s.Replace("-", " ");
            }
            return s.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
