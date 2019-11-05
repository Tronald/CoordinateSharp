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
    }
}
