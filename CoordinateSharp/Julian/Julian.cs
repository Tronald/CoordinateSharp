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
        /// <returns>JDE</returns>
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
        /// <returns>JDE</returns>
        public static double GetJulian_Epoch2000(DateTime d)
        {
            return GetJulian(d) - J2000;
        }
        /// <summary>
        /// Returns JD from epoch 1970.
        /// Meeus Ch 7.
        /// </summary>
        /// <param name="d">DateTime</param>
        /// <returns>JDE</returns>
        public static double GetJulian_Epoch1970(DateTime d)
        {
            return GetJulian(d) - J1970;
        }

        /// <summary>
        /// Returns date from Julian
        /// Meeus ch. 7
        /// </summary>
        /// <param name="j">Julian</param>
        /// <returns>DateTime</returns>
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
        /// <param name="j">Julian</param>
        /// <returns>DateTime</returns>
        public static DateTime? GetDate_FromJulian_Epoch2000(double j)
        {
            return GetDate_FromJulian(j + J2000);
        }
        /// <summary>
        /// Returns date from Julian based on epoch 1970
        /// Meeus ch. 7
        /// </summary>
        /// <param name="j">Julian</param>
        /// <returns>DateTime</returns>
        public static DateTime? GetDate_FromJulian_Epoch1970(double j)
        {
            return GetDate_FromJulian(j + J1970);
        }
    }
}
