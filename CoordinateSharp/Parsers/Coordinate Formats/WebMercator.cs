/*
CoordinateSharp is a .NET standard library that is intended to ease geographic coordinate 
format conversions and location based celestial calculations.
https://github.com/Tronald/CoordinateSharp

Many celestial formulas in this library are based on Jean Meeus's 
Astronomical Algorithms (2nd Edition). Comments that reference only a chapter
are referring to this work.

License

CoordinateSharp is split licensed and may be licensed under the GNU Affero General Public License version 3 or a commercial use license as stated.

Copyright (C) 2023, Signature Group, LLC
  
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
