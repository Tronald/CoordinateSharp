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
using System.Diagnostics;
using System.Collections.Generic;

namespace CoordinateSharp
{
    internal partial class FormatFinder
    {
        public static bool TryMGRS(string ns, out string[] mgrs)
        {
            //Prepare string for regex validation
            string s = ns.Replace(" ", "").ToUpper();
          
            if(Regex.IsMatch(s, @"[0-9]{1,2}[a-z,A-Z]{3}\d+ME\d+MN"))
            {
                int found = 0;
                List<char> msc = new List<char>();
                foreach (var c in s.ToArray())
                {
                    bool isLetter = char.IsLetter(c);
                    if(isLetter)
                    { found++; }

                    if(found>3 && isLetter) { continue; }
                    msc.Add(c);
                }
                s = string.Join("", msc);
            }

            mgrs = null;
            //Add easting northing at 0,0 if not provided

           
            //Attempt Regex Match
            Regex regex = new Regex(@"[0-9]{1,2}[a-z,A-Z]{3}\d+");

            //Add default easting/northing if not provided
            if (!regex.Match(s).Success && regex.Match(s + "00").Success) { s += "00"; }

            Match match = regex.Match(s);

            if (match.Success && match.ToString().Trim()== s)
            {
                //Extract Numbers for one string MGRS
                regex = new Regex("\\d+");
                MatchCollection matches = regex.Matches(s);

                //IF character count of Easting Northing aren't even return false as precision is unknown.
                int splitSpot = matches[1].Value.Count();
                if (splitSpot % 2 == 0)
                {

                    string longZone = matches[0].Value;
                    string eastingNorthing = matches[1].Value;

                    //Extract Letters

                    regex = new Regex("[a-z,A-Z]");
                    matches = regex.Matches(s);
                    string latZone = matches[0].Value;
                    string identifier = matches[1].Value + matches[2].Value;

                    //Split Easting and Northing Values
                    string easting = eastingNorthing.Substring(0, (int)(eastingNorthing.Length / 2));
                    string northing = eastingNorthing.Substring((int)(eastingNorthing.Length / 2), (int)(eastingNorthing.Length / 2));

                    mgrs = new string[] { longZone, latZone, identifier, easting.PadRight(5, '0'), northing.PadRight(5, '0') };
                    return true;
                }

            }
      
            return false;
        }
        public static bool TryMGRS_Polar(string ns, out string[] mgrs)
        {
            //Prepare string for regex validation
            string s = ns.Replace(" ", "").ToUpper();

            if (Regex.IsMatch(s, @"\d?\d?[a,b,y,z,A,B,Y,Z]{1}[a-z,A-Z]{2}\d+ME\d+MN"))
            {
                int found = 0;
                List<char> msc = new List<char>();
                foreach (var c in s.ToArray())
                {
                    bool isLetter = char.IsLetter(c);
                    if (isLetter)
                    { found++; }

                    if (found > 3 && isLetter) { continue; }
                    msc.Add(c);
                }
                s = string.Join("", msc);
            }
            if (s.Count() > 0 && char.IsLetter(s[0])) { s = "0" + s; }//For validation only add 0 to front of string
            mgrs = null;

            int i;
            if (!int.TryParse(s.Trim()[0].ToString(), out i))
            {
                s = "0" + s;
            }

            //Attempt Regex Match
            Regex regex = new Regex(@"\d?\d?[a,b,y,z,A,B,Y,Z]{1}[a-z,A-Z]{2}\d+");

            //Add default easting/northing if not provided
            if (!regex.Match(s).Success && regex.Match(s + "00").Success) { s += "00"; }

            Match match = regex.Match(s);
        

            if (match.Success && match.ToString().Trim()==s)
            {
                //Extract Numbers for one string MGRS
                regex = new Regex("\\d+");
                MatchCollection matches = regex.Matches(s);

                //IF character count of Easting Northing aren't even return false as precisions is unknown.
                int splitSpot = matches[1].Value.Count();
                if (splitSpot % 2 == 0)
                {

                    string longZone = matches[0].Value;
                    string eastingNorthing = matches[1].Value;

                    //Extract Letters

                    regex = new Regex("[a-z,A-Z]");
                    matches = regex.Matches(s);
                    string latZone = matches[0].Value;
                    string identifier = matches[1].Value + matches[2].Value;

                    //Split Easting and Northing Values
                    string easting = eastingNorthing.Substring(0, (int)(eastingNorthing.Length / 2));
                    string northing = eastingNorthing.Substring((int)(eastingNorthing.Length / 2), (int)(eastingNorthing.Length / 2));

                    mgrs = new string[] { longZone, latZone, identifier, easting.PadRight(5, '0'), northing.PadRight(5, '0') };

                    return true;
                }

            }
           
            return false;
        }
    }
}
