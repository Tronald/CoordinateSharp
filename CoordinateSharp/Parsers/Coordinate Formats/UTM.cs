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
using System.Text.RegularExpressions;
using System.Linq;
using System.Globalization;

namespace CoordinateSharp
{
    internal partial class FormatFinder
    {
        private static bool TryUTM(string s, out string[] utm)
        {
            utm = null;       

            //Attempt Regex Match
            Regex regex = new Regex("[0-9]{1,2}[a-z,A-Z]{1}\\d+");
            Match match = regex.Match(s);
            if (match.Success)
            {
                //Extract Numbers for one string UTM
                regex = new Regex("\\d+");
                MatchCollection matches = regex.Matches(s);

                //IF character count of Easting Northing aren't even return false as precisions is unknown.
                int splitSpot = matches[1].Value.Count();
                if (splitSpot % 2 != 0)
                {

                    string longZone = matches[0].Value;
                    string eastingNorthing = matches[1].Value;

                    //Extract Letters
                    regex = new Regex("[a-z,A-Z]");
                    matches = regex.Matches(s);
                    string latZone = matches[0].Value;

                    //Split Easting and Northing Values
                    string easting = eastingNorthing.Substring(0, (int)(eastingNorthing.Length / 2));
                    string northing = eastingNorthing.Substring((int)(eastingNorthing.Length / 2), (int)(eastingNorthing.Length / 2) + 1);

                    utm = new string[] { longZone, latZone, easting, northing };
                    return true;
                }

            }

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
                if (zoneL.Count() != 1) { return false; }
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
        private static bool TryUPS(string s, out string[] utm)
        {
            utm = null;

            int i;
            if(!int.TryParse(s.Trim()[0].ToString(), out i))
            {
                s = "0" + s;
            }

            //Attempt Regex Match
            Regex regex = new Regex("[0]{1,2}[a,b,y,z,A,B,Y,Z]{1}\\d+");
        
            Match match = regex.Match(s);
            if (match.Success)
            {
                
                //Extract Numbers for one string UTM
                regex = new Regex("\\d+");
                MatchCollection matches = regex.Matches(s);
  
                //IF character count of Easting Northing aren't even return false as precisions is unknown.
                int splitSpot = matches[1].Value.Count();
   
                if (splitSpot % 2 == 0)
                {
         
                    string longZone = "0";
                    string eastingNorthing = matches[1].Value;

                    //Extract Letters
                    regex = new Regex("[a-z,A-Z]");
                    matches = regex.Matches(s);
                    string latZone = matches[0].Value;

                    //Split Easting and Northing Values
                    string easting = eastingNorthing.Substring(0, (int)(eastingNorthing.Length / 2));
                    string northing = eastingNorthing.Substring((int)(eastingNorthing.Length / 2), (int)(eastingNorthing.Length / 2));

                    utm = new string[] { longZone, latZone, easting, northing };
         
                    return true;
                }

            }

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
                if (zoneL.Count() != 1) { return false; }
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
    }
}
