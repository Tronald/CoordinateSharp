/*
CoordinateSharp is a .NET standard library that is intended to ease geographic coordinate 
format conversions and location based celestial calculations.
https://github.com/Tronald/CoordinateSharp

Many celestial formulas in this library are based on Jean Meeus's 
Astronomical Algorithms (2nd Edition). Comments that reference only a chapter
are referring to this work.

License

CoordinateSharp is split licensed and may be licensed under the GNU Affero General Public License version 3 or a commercial use license as stated.

Copyright (C) 2021, Signature Group, LLC
  
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
	-United States Department of Defense.
	-United States Department of Homeland Security.
	-Open source contributors to this library.
	-Scholarly or scientific research.
	-Emergency response / management uses.

Please visit http://coordinatesharp.com/licensing or contact Signature Group, LLC to purchase a commercial license, or for any questions regarding the AGPL 3.0 license requirements or free use license: sales@signatgroup.com.
*/
using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.Globalization;

namespace CoordinateSharp
{
    internal partial class FormatFinder
    {
        private static bool TryDecimalDegree(string s, out double[] d)
        {
            d = null;

            string rs = Geodetic_Position_Spacer(s);

            if (Regex.Matches(rs, @"[a-zA-Z]").Count != 2) { return false; } //Should only contain 1 letter per part.

            string[] sA = SpecialSplit(rs, true);
            if (sA.Count() == 4)
            {
                double lat;
                double lng;

                int latR = 1; //Sets negative if South
                int lngR = 1; //Sets negative if West

                //Put both directional indicator together with string

                sA[0] += sA[1];
                sA[1] = sA[2] + sA[3];


                string latString = string.Empty;
                string longString = string.Empty;

                //Find Directions (new struct allows for reverse formatted coordinate)
                DirectionFinder p1 = new DirectionFinder(sA[0]);
                DirectionFinder p2 = new DirectionFinder(sA[1]);

                if (p1.Success)
                {
                    if (p1.CoordinateType.Value == CoordinateType.Lat) { latString = p1.PartString; latR = p1.Rad; }
                    else { longString = p1.PartString; lngR = p1.Rad; }
                }
                if (p2.Success)
                {
                    if (p2.CoordinateType.Value == CoordinateType.Lat) { latString = p2.PartString; latR = p2.Rad; }
                    else { longString = p2.PartString; lngR = p2.Rad; }
                }

                //Either lat or long not provided in this case
                if (string.IsNullOrEmpty(latString) || string.IsNullOrEmpty(longString)) { return false; }

                latString = Regex.Replace(latString, "[^0-9.]", "");
                longString = Regex.Replace(longString, "[^0-9.]", "");

                if (!double.TryParse(latString, NumberStyles.Any, CultureInfo.InvariantCulture, out lat))
                { return false; }
                if (!double.TryParse(longString, NumberStyles.Any, CultureInfo.InvariantCulture, out lng))
                { return false; }

                lat *= latR;
                lng *= lngR;

                d = new double[] { lat, lng };

                return true;
            }

            return false;
        }
    }
}
