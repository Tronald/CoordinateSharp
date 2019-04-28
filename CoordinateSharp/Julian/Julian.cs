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
namespace CoordinateSharp
{
    /// <summary>
    /// Julian date conversions
    /// </summary>
    public class JulianConversions
    {
        //1.1.3.1
        private static double J1970 = 2440588, J2000 = 2451545;

        /// <summary>
        /// Returns JD.
        /// Meeus Ch 7.
        /// </summary>
        /// <param name="d">DateTime</param>
        /// <returns>Julian date</returns>
        /// <example>
        /// <code>
        /// DateTime date = new DateTime(2019,1,1);
        /// double jul = JulianConversions.GetJulian(date);
        /// Console.WriteLine(jul); //2458484.5
        /// </code>
        /// </example>
        public static double GetJulian(DateTime d)
        {
            double y = d.Year;
            double m = d.Month;
            double dy = d.Day + (d.TimeOfDay.TotalHours / 24);

            //If month is Jan or Feb add 12 to month and reduce year by 1.
            if (m <= 2) { m += 12; y -= 1; }

            double A = (int)(d.Year / 100.0);
            double B = 0;

            //Gregorian Start Date
            if (d >= new DateTime(1582, 10, 15))
            {
                B = 2 - A + (int)(A / 4.0);
            }
            double JD = (int)(365.25 * (y + 4716)) + (int)(30.6001 * (m + 1)) + dy + B - 1524.5;
            return JD;
        }
        /// <summary>
        /// Returns JD from epoch 2000.
        /// Meeus Ch 7.
        /// </summary>
        /// <param name="d">DateTime</param>
        /// <returns>Julian date from epoch 2000</returns>
        /// <example>
        /// <code>
        /// DateTime date = new DateTime(2019,1,1);
        /// double jul = JulianConversions.GetJulian_Epoch2000(date);
        /// Console.WriteLine(jul); //6939.5
        /// </code>
        /// </example>
        public static double GetJulian_Epoch2000(DateTime d)
        {
            return GetJulian(d) - J2000;
        }
        /// <summary>
        /// Returns JD from epoch 1970.
        /// Meeus Ch 7.
        /// </summary>
        /// <param name="d">DateTime</param>
        /// <returns>Julian date from epoch 1970</returns>
        /// <example>
        /// <code>
        /// DateTime date = new DateTime(2019,1,1);
        /// double jul = JulianConversions.GetJulian_Epoch1970(date);
        /// Console.WriteLine(jul); //17896.5
        /// </code>
        /// </example>
        public static double GetJulian_Epoch1970(DateTime d)
        {
            return GetJulian(d) - J1970;
        }

        /// <summary>
        /// Returns date from Julian
        /// Meeus ch. 7
        /// </summary>
        /// <param name="j">Julian date</param>
        /// <returns>DateTime</returns>
        /// <example>
        /// <code>
        /// double jul = 2458484.5;
        /// DateTime? date = JulianConversions.GetDate_FromJulian(jul);
        /// Console.WriteLine(date); //1/1/2019 12:00:00 AM
        /// </code>
        /// </example>
        public static DateTime? GetDate_FromJulian(double j)
        {
            if (Double.IsNaN(j)) { return null; } //No Event Occured

            j = j + .5;
            double Z = Math.Floor(j);
            double F = j - Z;
            double A = Z;
            if (Z >= 2299161)
            {
                double a = (int)((Z - 1867216.25) / 36524.25);
                A = Z + 1 + a - (int)(a / 4.0);
            }
            double B = A + 1524;
            double C = (int)((B - 122.1) / 365.25);
            double D = (int)(365.25 * C);
            double E = (int)((B - D) / 30.6001);

            double day = B - D - (int)(30.6001 * E) + F;

            //Month is E-1 if month is < 14 or E-13 if month is 14 or 15
            double month = E - 1;
            if (E > 13) { month -= 12; }

            //year is C-4716 if month>2 and C-4715 if month is 1 or 2
            double year = C - 4715;
            if (month > 2)
            {
                year -= 1;
            }

            double hours = day - Math.Floor(day);
            hours *= 24;
            double minutes = hours - Math.Floor(hours);
            minutes *= 60;
            double seconds = minutes - Math.Floor(minutes);
            seconds *= 60;

            day = Math.Floor(day);
            hours = Math.Floor(hours);
            minutes = Math.Floor(minutes);

            DateTime? date = new DateTime?(new DateTime((int)year, (int)month, (int)day, (int)hours, (int)minutes, (int)seconds));
            return date;
        }
        /// <summary>
        /// Returns date from Julian based on epoch 2000
        /// Meeus ch. 7
        /// </summary>
        /// <param name="j">Julian date (epoch 2000)</param>
        /// <returns>DateTime</returns>
        /// <example>
        /// <code>
        /// double jul = 6939.5;
        /// DateTime? date = JulianConversions.GetDate_FromJulian_Epoch2000(jul);
        /// Console.WriteLine(date); //1/1/2019 12:00:00 AM
        /// </code>
        /// </example>
        public static DateTime? GetDate_FromJulian_Epoch2000(double j)
        {
            return GetDate_FromJulian(j + J2000);
        }
        /// <summary>
        /// Returns date from Julian based on epoch 1970
        /// Meeus ch. 7
        /// </summary>
        /// <param name="j">Julian date (epoch 1970)</param>
        /// <returns>DateTime</returns>
        /// <example>
        /// <code>
        /// double jul = 17896.5;
        /// DateTime? date = JulianConversions.GetDate_FromJulian_Epoch1970(jul);
        /// Console.WriteLine(date); //1/1/2019 12:00:00 AM
        /// </code>
        /// </example>
        public static DateTime? GetDate_FromJulian_Epoch1970(double j)
        {
            return GetDate_FromJulian(j + J1970);
        }
    }
}
