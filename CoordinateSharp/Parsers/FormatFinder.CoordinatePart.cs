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
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace CoordinateSharp
{
    internal partial class FormatFinder_CoordPart
    {
        //Add main to Coordinate and tunnel to Format class. Add private methods to format.
        //WHEN PARSING NO EXCPETIONS FOR OUT OF RANGE ARGS WILL BE THROWN.
        public static bool TryParse(string coordString, out CoordinatePart cp)
        {
            try
            {
                //Turn of eagerload for efficiency
                EagerLoad eg = new EagerLoad();
                int type = 0; //0 = unspecifed, 1 = lat, 2 = long;
                eg.Cartesian = false;
                eg.Celestial = false;
                eg.UTM_MGRS = false;
                cp = null;
                Coordinate c = new Coordinate(eg);
                if (string.IsNullOrEmpty(coordString)) { return false; }

                string s = coordString;
                s = s.Trim(); //Trim all spaces before and after string
                double[] d;

                if (s[0] == ',')
                {
                    type = 2;
                    s = s.Replace(",", "");
                    s = s.Trim();
                }
                if (s[0] == '*')
                {
                    type = 1;
                    s = s.Replace("*", "");
                    s = s.Trim();
                }

                if (TrySignedDegree(s, type, out d))
                {
                    try
                    {
                        switch (type)
                        {
                            case 0:
                                //Attempt Lat first (default for signed)
                                try
                                {
                                    cp = new CoordinatePart(d[0], CoordinateType.Lat);
                                    c.Parse_Format = Parse_Format_Type.Signed_Degree;
                                    return true;
                                }
                                catch
                                {
                                    cp = new CoordinatePart(d[0], CoordinateType.Long);
                                    c.Parse_Format = Parse_Format_Type.Signed_Degree;
                                    return true;
                                }
                            case 1:
                                //Attempt Lat
                                cp = new CoordinatePart(d[0], CoordinateType.Lat);
                                c.Parse_Format = Parse_Format_Type.Signed_Degree;
                                return true;
                            case 2:
                                //Attempt long
                                cp = new CoordinatePart(d[0], CoordinateType.Long);
                                c.Parse_Format = Parse_Format_Type.Signed_Degree;
                                return true;
                        }
                    }
                    catch
                    {
                        //silent fail
                    }
                }
                //SIGNED DEGREE FAILED, REMOVE DASHES FOR OTHER FORMATS
                s = s.Replace("-", " ");

                //All other formats should contain 1 letter.
                if (Regex.Matches(s, @"[a-zA-Z]").Count != 1) { return false; } //Should only contain 1 letter.
                                                                                //Get Coord Direction
                int direction = Find_Position(s);

                if (direction == -1)
                {
                    return false; //No direction found
                }
                //If Coordinate type int specified, look for mismatch
                if (type == 1 && (direction == 1 || direction == 3))
                {
                    return false; //mismatch
                }
                if (type == 2 && (direction == 0 || direction == 2))
                {
                    return false; //mismatch
                }
                CoordinateType t;
                if (direction == 0 || direction == 2) { t = CoordinateType.Lat; }
                else { t = CoordinateType.Long; }

                s = Regex.Replace(s, "[^0-9. ]", ""); //Remove directional character
                s = s.Trim(); //Trim all spaces before and after string

                //Try Decimal Degree with Direction
                if (TryDecimalDegree(s, direction, out d))
                {
                    try
                    {
                        cp = new CoordinatePart(d[0], t);
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
                        //0  Degree
                        //1  Minute
                        //2  Direction (0 = N, 1 = E, 2 = S, 3 = W)                          
                        cp = new CoordinatePart((int)d[0], d[1], (CoordinatesPosition)direction);
                        c.Parse_Format = Parse_Format_Type.Degree_Decimal_Minute;
                        return true;
                    }
                    catch
                    {
                        //Parser failed try next method 
                    }
                }
                //Try DMS
                if (TryDegreeMinuteSecond(s, out d))
                {
                    try
                    {
                        //0 Degree
                        //1 Minute
                        //2 Second
                        //3 Direction (0 = N, 1 = E, 2 = S, 3 = W)                                     
                        cp = new CoordinatePart((int)d[0], (int)d[1], d[2], (CoordinatesPosition)direction);
                        c.Parse_Format = Parse_Format_Type.Degree_Minute_Second;
                        return true;
                    }
                    catch
                    {//Parser failed try next method 
                    }
                }
            }
            catch (Exception ex)
            {
                //Parser exception has occurred
                Debug.WriteLine("PARSER EXCEPTION HANDLED: " + ex.ToString());
            }
            cp = null;
            return false;
        }
        private static int Find_Position(string s)
        {
            //N=0
            //E=1
            //S=2
            //W=3
            //NOPOS = -1
            //Find Directions

            int part = -1;
            if (s.Contains("N") || s.Contains("n"))
            {
                part = 0;
            }
            if (s.Contains("E") || s.Contains("e"))
            {
                part = 1;
            }
            if (s.Contains("S") || s.Contains("s"))
            {
                part = 2;

            }
            if (s.Contains("W") || s.Contains("w"))
            {
                part = 3;
            }
            return part;
        }

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
            if(removeDashes)
            {
                s = s.Replace("-", " ");
            }
            return s.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
