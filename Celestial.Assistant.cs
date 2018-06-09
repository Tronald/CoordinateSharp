using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoordinateSharp
{
    internal class MeeusTables
    {
        /// <summary>
        /// Sum of Apogee Terms from Jean Meeus Astronomical Algorithms Table 50.A
        /// </summary>
        /// <param name="D">Moom's mean elongation at time JDE</param>
        /// <param name="M">Sun's mean anomaly</param>
        /// <param name="F">Moon's arguement f latitude</param>
        /// <param name="T">Time in Julian centuries since epoch 2000</param>
        /// <returns>double</returns>
        static double ApogeeTermsA(double D, double M, double F, double T)
        {
            double sum;
         
            sum = Math.Sin(2 * D) * 0.4392;
            sum += Math.Sin(4 * D) * 0.0684;
            sum += Math.Sin(M) * .0456 - 0.00011 * T;
            sum += Math.Sin(2 * D - M) * .0426 - 0.00011 * T;
            sum += Math.Sin(2 * F) * .0212;
            sum += Math.Sin(D) * -0.0189;
            sum += Math.Sin(6 * D) * .0144;
            sum += Math.Sin(4 * D - M) * .0113;
            sum += Math.Sin(2 * D + 2 * F) * .0047;
            sum += Math.Sin(D + M) * .0036;
            sum += Math.Sin(8 * D) * .0035;
            sum += Math.Sin(6 * D - M) * .0034;
            sum += Math.Sin(2 * D - 2 * F) * -0.0034;
            sum += Math.Sin(2 * D - 2 * M) * .0022;
            sum += Math.Sin(3 * D) * -.0017;
            sum += Math.Sin(4 * D + 2 * F) * 0.0013;

            sum += Math.Sin(8 * D - M) * .0011;
            sum += Math.Sin(4 * D - 2 * M) * .0010;
            sum += Math.Sin(10 * D) * .0009;
            sum += Math.Sin(3 * D + M) * .0007;
            sum += Math.Sin(2 * M) * .0006;
            sum += Math.Sin(2 * D + M) * .0005;
            sum += Math.Sin(2 * D + 2 * M) * .0005;
            sum += Math.Sin(6 * D + 2 * F) * .0004;
            sum += Math.Sin(6 * D - 2 * M) * .0004;
            sum += Math.Sin(10 * D - M) * .0004;
            sum += Math.Sin(5 * D) * -0.0004;
            sum += Math.Sin(4 * D - 2 * F) * -0.0004;
            sum += Math.Sin(2 * F + M) * .0003;
            sum += Math.Sin(12 * D) * .0003;
            sum += Math.Sin(2 * D + 2 * F - M) * 0.0003;
            sum += Math.Sin(D - M) * -0.0003;
            return sum;
        }
        /// <summary>
        /// Sum of Perigee Terms from Jean Meeus Astronomical Algorithms Table 50.A
        /// </summary>
        /// <param name="D">Moom's mean elongation at time JDE</param>
        /// <param name="M">Sun's mean anomaly</param>
        /// <param name="F">Moon's arguement f latitude</param>
        /// <param name="T">Time in Julian centuries since epoch 2000</param>
        /// <returns>double</returns>
        static double PerigeeTermsA(double D, double M, double F, double T)
        {
            double sum;

            sum = Math.Sin(2 * D) * -1.6769;
            sum += Math.Sin(4 * D) * .4589;
            sum += Math.Sin(6 * D) * -.1856;
            sum += Math.Sin(8 * D) * .0883;
            sum += Math.Sin(2 * D - M) * -.0773 + .00019 * T;
            sum += Math.Sin(M) * .0502 - .00013 * T;
            sum += Math.Sin(10 * D) * -.0460;
            sum += Math.Sin(4 * D - M) * .0422 - .00011 * T;
            sum += Math.Sin(6 * D - M) * -.0256;
            sum += Math.Sin(12 * D) * .0253;
            sum += Math.Sin(D) * .0237;
            sum += Math.Sin(8 * D - M) * .0162;
            sum += Math.Sin(14 * D) * -.0145;
            sum += Math.Sin(2 * F) * .0129;
            sum += Math.Sin(3 * D) * -.0112;
            sum += Math.Sin(10 * D - M) * -.0104;
            sum += Math.Sin(16 * D) * .0086;
            sum += Math.Sin(12 * D - M) * .0069;
            sum += Math.Sin(5 * D) * .0066;
            sum += Math.Sin(2 * D + 2 * F) * -.0053;
            sum += Math.Sin(18 * D) * -.0052;
            sum += Math.Sin(14 * D - M) * -.0046;
            sum += Math.Sin(7 * D) * -.0041;
            sum += Math.Sin(2 * D + M) * .0040;
            sum += Math.Sin(20 * D) * .0032;
            sum += Math.Sin(D + M) * -.0032;
            sum += Math.Sin(16 * D - M) * .0031;
            sum += Math.Sin(4 * D + M) * -.0029;
            sum += Math.Sin(9 * D) * .0027;
            sum += Math.Sin(4 * D + 2 * F) * .0027;

            sum += Math.Sin(2 * D - 2 * M) * -.0027;
            sum += Math.Sin(4 * D - 2 * M) * .0024;
            sum += Math.Sin(6 * D - 2 * M) * -.0021;
            sum += Math.Sin(22 * D) * -.0021;
            sum += Math.Sin(18 * D - M) * -.0021;
            sum += Math.Sin(6 * D + M) * .0019;
            sum += Math.Sin(11 * D) * -.0018;
            sum += Math.Sin(8 * D + M) * -.0014;
            sum += Math.Sin(4 * D - 2 * F) * -.0014;
            sum += Math.Sin(6 * D + 2 * F) * -.0014;
            sum += Math.Sin(3 * D + M) * .0014;
            sum += Math.Sin(5 * D + M) * -.0014;
            sum += Math.Sin(13 * D) * .0013;
            sum += Math.Sin(20 * D - M) * .0013;
            sum += Math.Sin(3 * D + 2 * M) * .0011;
            sum += Math.Sin(4 * D + 2 * F - 2 * M) * -.0011;
            sum += Math.Sin(D + 2 * M) * -.0010;
            sum += Math.Sin(22 * D - M) * -.0009;
            sum += Math.Sin(4 * F) * -.0008;
            sum += Math.Sin(6 * D - 2 * F) * .0008;
            sum += Math.Sin(2 * D - 2 * F + M) * .0008;
            sum += Math.Sin(2 * M) * .0007;
            sum += Math.Sin(2 * F - M) * .0007;
            sum += Math.Sin(2 * D + 4 * F) * .0007;
            sum += Math.Sin(2 * F - 2 * M) * -.0006;
            sum += Math.Sin(2 * D - 2 * F + 2 * M) * -.0006;
            sum += Math.Sin(24 * D) * .0006;
            sum += Math.Sin(4 * D - 4 * F) * .0005;
            sum += Math.Sin(2 * D + 2 * M) * .0005;
            sum += Math.Sin(D - M) * -.0004;

            return sum;
        }
        /// <summary>
        /// Sum of Apogee Terms from Jean Meeus Astronomical Algorithms Table 50.B
        /// </summary>
        /// <param name="D">Moom's mean elongation at time JDE</param>
        /// <param name="M">Sun's mean anomaly</param>
        /// <param name="F">Moon's arguement f latitude</param>
        /// <param name="T">Time in Julian centuries since epoch 2000</param>
        /// <returns>double</returns>
        static double ApogeeTermsB(double D, double M, double F, double T)
        {
            double sum = 3245.251;

            sum += Math.Cos(2 * D) * -9.147;
            sum += Math.Cos(D) * -.841;
            sum += Math.Cos(2 * F) * .697;
            sum += Math.Cos(M) * -0.656 + .0016 * T;
            sum += Math.Cos(4 * D) * .355;
            sum += Math.Cos(2 * D - M) * .159;
            sum += Math.Cos(D + M) * .127;
            sum += Math.Cos(4 * D - M) * .065;

            sum += Math.Cos(6 * D) * .052;
            sum += Math.Cos(2 * D + M) * .043;
            sum += Math.Cos(2 * D + 2 * F) * .031;
            sum += Math.Cos(2 * D - 2 * F) * -.023;
            sum += Math.Cos(2 * D - 2 * M) * .022;
            sum += Math.Cos(2 * D + 2 * M) * .019;
            sum += Math.Cos(2 * M) * -.016;
            sum += Math.Cos(6 * D - M) * .014;
            sum += Math.Cos(8 * D) * .010;

            return sum;
        }
        /// <summary>
        /// Sum of Perigee Terms from Jean Meeus Astronomical Algorithms Table 50.B
        /// </summary>
        /// <param name="D">Moom's mean elongation at time JDE</param>
        /// <param name="M">Sun's mean anomaly</param>
        /// <param name="F">Moon's arguement f latitude</param>
        /// <param name="T">Time in Julian centuries since epoch 2000</param>
        /// <returns>double</returns>
        static double PerigeeTermsB(double D, double M, double F, double T)
        {
            //Sum of Perigee Terms from Jean Meeus Astronomical Algorithms Table 50.B          
            double sum = 3629.215;

            sum += Math.Cos(2 * D) * 63.224;
            return sum;
        }
    }
    /// <summary>
    ///  moon perigee or apogee indicator
    /// </summary>
    internal enum MoonDistanceType
    {
        /// <summary>
        /// Moon's perigee
        /// </summary>
        Perigee,
        /// <summary>
        /// Moon's apogee
        /// </summary>
        Apogee
    }
    /// <summary>
    /// Stores Perigee or Apogee values
    /// </summary>
    public class PerigeeApogee
    {
        private DateTime date;
        private double horizontalParallax;

        /// <summary>
        /// Initializes a Perigee or Apogee object
        /// </summary>
        /// <param name="d">Date of Event</param>
        /// <param name="p">Horizontal Parallax</param>
        public PerigeeApogee(DateTime d, double p)
        {
            date = d;
            horizontalParallax = p;
        }

        /// <summary>
        /// Date of event.
        /// </summary>
        public DateTime Date
        {
            get { return date; }
        }
        /// <summary>
        /// Horizontal Parallax.
        /// </summary>
        public double HorizontalParallax
        {
            get { return horizontalParallax; }
        }
    }
    /// <summary>
    /// Used for Julian date conversions
    /// </summary>
    internal class JulianConversions
    {
        private static double dayMs = 1000 * 60 * 60 * 24, J1970 = 2440588, J2000 = 2451545;
        public static double toJulian(DateTime date)
        {
            DateTime d = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime t = date;
            double l = (double)(t - d).TotalMilliseconds;
            double tj = l / dayMs - 0.5 + J1970;

            return tj;

        }
        public static double toDays(DateTime date)
        {
            double d = toJulian(date) - J2000;

            return d;
        }
        public static double GetJulianDay(DateTime date)
        {
            int month = date.Month;
            int day = date.Day;
            int year = date.Year;

            bool gregorian = (year < 1583) ? false : true;

            if ((month == 1) || (month == 2))
            {
                year = year - 1;
                month = month + 12;
            }

            double a = Math.Truncate((double)year / 100);
            double b = 0;

            if (gregorian)
                b = 2 - a + Math.Truncate(a / 4);
            else
                b = 0.0;

            double jd = Math.Truncate(365.25 * (year + 4716))
                       + Math.Truncate(30.6001 * (month + 1))
                       + day + b - 1524.5;

            return jd;
        }
        public static DateTime? fromJulian(double j)
        {
            if (Double.IsNaN(j)) { return null; } //No Event Occured

            double unixTime = (j + 0.5 - J1970) * 86400;

            System.DateTime dtDateTime = new DateTime(1970, 1, 1);
            dtDateTime = dtDateTime.AddSeconds(unixTime);

            return dtDateTime;

        }
    }
}
