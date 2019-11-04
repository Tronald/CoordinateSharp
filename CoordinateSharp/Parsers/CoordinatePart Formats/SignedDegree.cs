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
    internal partial class FormatFinder_CoordPart
    {
        private static bool TrySignedDegree(string s, int t, out double[] d)
        {
            d = null;
            if (Regex.Matches(s, @"[a-zA-Z]").Count != 0) { return false; } //Should contain no letters

            string[] sA = SpecialSplit(s, false);
            double deg;
            double min; //Minutes & MinSeconds
            double sec;

            int sign = 1;
            switch (sA.Count())
            {
                case 1:
                    if (!double.TryParse(sA[0], NumberStyles.Any, CultureInfo.InvariantCulture, out deg))
                    { return false; }
                    d = new double[] { deg };
                    return true;
                case 2:
                    if (!double.TryParse(sA[0], NumberStyles.Any, CultureInfo.InvariantCulture, out deg))
                    { return false; }
                    if (!double.TryParse(sA[1], NumberStyles.Any, CultureInfo.InvariantCulture, out min))
                    { return false; }

                    if (deg < 0) { sign = -1; }
                    if (min >= 60 || min < 0) { return false; } //Handle in parser as degree will be incorrect.
                    d = new double[] { (Math.Abs(deg) + (min / 60.0)) * sign };
                    return true;
                case 3:
                    if (!double.TryParse(sA[0], NumberStyles.Any, CultureInfo.InvariantCulture, out deg))
                    { return false; }
                    if (!double.TryParse(sA[1], NumberStyles.Any, CultureInfo.InvariantCulture, out min))
                    { return false; }
                    if (!double.TryParse(sA[2], NumberStyles.Any, CultureInfo.InvariantCulture, out sec))
                    { return false; }
                    if (min >= 60 || min < 0) { return false; } //Handle in parser as degree will be incorrect.
                    if (sec >= 60 || sec < 0) { return false; } //Handle in parser as degree will be incorrect.

                    if (deg < 0) { sign = -1; }
                    d = new double[] { (Math.Abs(deg) + (min / 60.0) + (sec / 3600.0)) * sign };
                    return true;
                default:
                    return false;
            }
        }
    }
}
