/*
CoordinateSharp is a .NET standard library that is intended to ease geographic coordinate 
format conversions and location based celestial calculations.
https://github.com/Tronald/CoordinateSharp

Many celestial formulas in this library are based on Jean Meeus's 
Astronomical Algorithms (2nd Edition). Comments that reference only a chapter
are referring to this work.

License

CoordinateSharp is split licensed and may be licensed under the GNU Affero General Public License version 3 or a commercial use license as stated.

Copyright (C) 2022, Signature Group, LLC
  
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
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace CoordinateSharp
{
    internal partial class FormatFinder
    {
        //Add main to Coordinate and tunnel to Format class. Add private methods to format.
        //WHEN PARSING NO EXCPETIONS FOR OUT OF RANGE ARGS WILL BE THROWN
        public static bool TryParse(string coordString, CartesianType ct, Allowed_Parse_Format pf, out Coordinate c)
        {
            try
            {
                //Turn off eagerload for efficiency
                EagerLoad eg = new EagerLoad();
                eg.Cartesian = false;
                eg.Celestial = false;
                eg.UTM_MGRS = false;

                c = new Coordinate(eg);
                if (string.IsNullOrEmpty(coordString)) { return false; }

                string s = coordString;
                s = s.Trim(); //Trim all spaces before and after string
                double[] d;
                //Try Signed Degree
                if (pf.HasFlag(Allowed_Parse_Format.Lat_Long) && TrySignedDegree(s, out d))
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
                if (pf.HasFlag(Allowed_Parse_Format.Lat_Long) && TryDecimalDegree(s, out d))
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
                if (pf.HasFlag(Allowed_Parse_Format.Lat_Long) && TryDegreeDecimalMinute(s, out d))
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
                if (pf.HasFlag(Allowed_Parse_Format.Lat_Long) && TryDegreeMinuteSecond(s, out d))
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

                Cartesian cart;
                if (pf.HasFlag(Allowed_Parse_Format.Cartesian_Spherical) && ct == CartesianType.Cartesian && Cartesian.TryParse(s, out cart))
                {
                    try
                    {                    
                        c = Cartesian.CartesianToLatLong(cart);
                        c.Parse_Format = Parse_Format_Type.Cartesian_Spherical;
                        return true;
                    }
                    catch
                    {//Parser failed try next method 
                    }
                }

                ECEF ecef;
                if (pf.HasFlag(Allowed_Parse_Format.Cartesian_ECEF) && ct == CartesianType.ECEF && ECEF.TryParse(s, out ecef))
                {
                    try
                    {                      
                        c = ECEF.ECEFToLatLong(ecef);
                        c.Parse_Format = Parse_Format_Type.Cartesian_ECEF;
                        return true;
                    }
                    catch
                    {//Parser failed try next method 
                    }
                }
                

                //Try MGRS
                MilitaryGridReferenceSystem mgrs;
                if (pf.HasFlag(Allowed_Parse_Format.MGRS) && MilitaryGridReferenceSystem.TryParse(s, out mgrs))
                {
                    try
                    {
                        c = MilitaryGridReferenceSystem.MGRStoLatLong(mgrs);
                        c.Parse_Format = Parse_Format_Type.MGRS;
                        return true;
                    }
                    catch
                    {//Parser failed try next method 
                    }
                }

                //Try UTM
                UniversalTransverseMercator utm;
                if (pf.HasFlag(Allowed_Parse_Format.UTM) && UniversalTransverseMercator.TryParse(s, out utm))
                {
                    try
                    {                      
                        c = UniversalTransverseMercator.ConvertUTMtoLatLong(utm);
                        c.Parse_Format = Parse_Format_Type.UTM;
                        return true;
                    }
                    catch
                    {//Parser failed try next method 
                    }
                }

                //Try Web Mercator
                WebMercator webMercator;
                if(pf.HasFlag(Allowed_Parse_Format.WebMercator) && WebMercator.TryParse(s, out webMercator))
                {
                    try
                    {
                        c = WebMercator.ConvertWebMercatortoLatLong(webMercator);
                        c.Parse_Format = Parse_Format_Type.WebMercator;
                        return true;
                    }
                    catch
                    {//parser failed try next method
                    }
                }

                //Try GEOREF
                GEOREF georef;
                if (pf.HasFlag(Allowed_Parse_Format.GEOREF) && GEOREF.TryParse(s, out georef))
                {
                    try
                    {
                        c = GEOREF.ConvertGEOREFtoLatLong(georef);
                        c.Parse_Format = Parse_Format_Type.GEOREF;
                        return true;
                    }
                    catch
                    {//parser failed try next method
                    }
                }


            }
            catch(Exception ex)
            {
                //Parser exception has occurred
                Debug.WriteLine("PARSER EXCEPTION HANDLED: " + ex.ToString());
            }
            c = null;
            return false;


        }

        //KEEP DASHES FOR SIGNED AND CARTESIAN AS THEY ARE USED FOR NEGATVE VALUES
        private static string[] SpecialSplit(string s, bool removeDashes)
        {
            s=s.ToUpper();
            s = s.Replace("°", " ");
            s = s.Replace("º", " ");
            s = s.Replace("'", " ");
            s = s.Replace("\"", " ");
            s = s.Replace(",", " ");
         
            if (removeDashes)
            {
                s = s.Replace("-", " ");
            }
            return s.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
        }
        private static string Geodetic_Position_Spacer(string s)
        {
            string rs = Regex.Replace(s.ToLower(), @"\d[n,e,s,w]", delegate (Match match)
            {
                string ns = match.ToString();
                return ns[0] + " " + ns[1];
            });
            rs = Regex.Replace(rs, @"[n,e,s,w]\d", delegate (Match match)
            {
                string ns = match.ToString();
                return ns[0] + " " + ns[1];
            });
            return rs;
        }
    }
}
