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

namespace CoordinateSharp.Formatters
{
    /// <summary>
    /// Static formatters designed to assist with format conversions of coordinates and celestial data points.
    /// </summary>
    public static class Format
    {
        /// <summary>
        /// Converts degrees to radians.
        /// </summary>
        /// <param name="degrees">Degrees</param>
        /// <returns>double</returns>
        public static double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }

        /// <summary>
        /// Converts radians to degrees.
        /// </summary>
        /// <param name="radians">Radians</param>
        /// <returns>double</returns>
        public static double ToDegrees(double radians)
        {
            return radians * 180 / Math.PI;
        }
        /// <summary>
        /// Converts DDM format to signed degrees format.
        /// </summary>
        /// <param name="degrees">Degrees</param>
        /// <param name="decimalMinutes">Decimal Minutes</param>
        /// <returns>double</returns>
        public static double ToDegrees(int degrees, double decimalMinutes)
        {
            return degrees + (Math.Sign(degrees) * (decimalMinutes / 60));
        }

        /// <summary>
        /// Converts DMS format to signed degrees format.
        /// </summary>
        /// <param name="degrees">Degrees</param>
        /// <param name="minutes">Minutes</param>
        /// <param name="seconds">Seconds</param>
        /// <returns>double</returns>
        public static double ToDegrees(int degrees, int minutes, double seconds)
        {
            double decimalMinutes = minutes + (seconds / 60);
            return degrees + (Math.Sign(degrees) * (decimalMinutes / 60));
        }

        /// <summary>
        /// Converts signed degrees to DDM format.
        /// </summary>
        /// <param name="signedDegrees">Signed Degrees</param>
        /// <returns>double[Degrees, Decimal Minutes]</returns>
        public static double[] ToDegreeDecimalMinutes(double signedDegrees)
        {
            //Update the Degree & Decimal Minute
            double degFloor = Math.Truncate(signedDegrees); //Truncate the number left to extract the degree          
            double ddm = signedDegrees - degFloor; //Extract decimalMinute value from decimalDegree
            ddm *= 60; //Multiply by 60 to get readable decimalMinute
            double dm = Convert.ToDouble(ddm); //Convert decimalMinutes back to double for storage
            int df = Convert.ToInt32(degFloor); //Convert degrees to int for storage

            return new double[] {df,dm};
        }

        /// <summary>
        /// Converts signed degrees to DMS format.
        /// </summary>
        /// <param name="signedDegrees">Signed Degrees</param>
        /// <returns>double[Degrees, Minutes, Seconds]</returns>
        public static double[] ToDegreesMinutesSeconds(double signedDegrees)
        {
            var ddm = ToDegreeDecimalMinutes(signedDegrees);

            double dmFloor = Math.Truncate(ddm[1]); //Get number left of decimal to grab minute value
          
            double s = ddm[1] - dmFloor; //Get seconds from minutes
            s *= 60; //Multiply by 60 to get readable seconds
            double secs = Convert.ToDouble(s); //Convert back to double for storage

            return new double[] { ddm[0], dmFloor, secs };
        }

        /// <summary>
        /// Normalizes degrees to a value between 0 and 360.
        /// </summary>
        /// <param name="degree">Signed Degrees</param>
        /// <returns>double</returns>
        public static double NormalizeDegrees360(double degree)
        {
            double normalized = degree % 360;
            if (normalized < 0)
            {
                normalized = 360 + normalized;
            }

            return normalized;
        }

        /// <summary>
        /// Converts signed degrees to HMS format.
        /// </summary>
        /// <param name="degrees">Signed Degrees</param>
        /// <returns>double[Hours, Minutes, Seconds]</returns>
        public static double[] ToHoursMinutesSeconds(double degrees)
        {
            double hoursDecimal = degrees / 15;
            double hours = Math.Truncate(hoursDecimal);
            double minutesDecimal = (hoursDecimal - hours)*60;
            double minutes = Math.Truncate(minutesDecimal);
            double seconds = (minutesDecimal - minutes) * 60;
           
            return new double[] { hours, minutes ,seconds };
        }

        /// <summary>
        /// Converts decimal minutes to fractional degree minutes.
        /// </summary>
        /// <param name="d">Minutes</param>
        /// <returns>double</returns>
        public static double ToMinutes(double d)
        {
            return d / 60;
        }

        /// <summary>
        /// Converts decimal seconds to fractional degree seconds.
        /// </summary>
        /// <param name="d">Seconds</param>
        /// <returns>double</returns>
        public static double ToSeconds(double d)
        {
            return d / 3600;
        }
    }
}
