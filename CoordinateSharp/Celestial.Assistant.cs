using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
namespace CoordinateSharp
{
    internal partial class MeeusTables
    {
      
        /// <summary>
        /// Returns Moon Periodic Value Er
        /// </summary>
        /// <param name="D">Moon's mean elongation</param>
        /// <param name="M">Sun's mean anomaly</param>
        /// <param name="N">Moon's mean anomaly</param>
        /// <param name="F">Moon's argument of latitude</param>
        /// <param name="T">Dynamic time</param>
        /// <returns>Er</returns>
        public static double Moon_Periodic_Er(double D, double M, double N, double F, double T)
        {
            //Table 47A contains 60 lines to sum
            double[] values = new double[] { D, M, N, F };
            double sum = 0;
            for (int x = 0; x < 60; x++)
            {
                sum += Get_Table47A_Values(values, x, T, false);
            }

            return sum;
        }
        /// <summary>
        /// Returns Moon Periodic Value El
        /// </summary>
        /// <param name="L">Moon's mean longitude</param>
        /// <param name="D">Moon's mean elongation</param>
        /// <param name="M">Sun's mean anomaly</param>
        /// <param name="N">Moon's mean anomaly</param>
        /// <param name="F">Moon's argument of latitude</param>
        /// <param name="T">Dynamic time</param>
        /// <returns>El</returns>
        public static double Moon_Periodic_El(double L, double D, double M, double N, double F, double T)
        {
            //Table 47A contains 60 lines to sum
            double[] values = new double[] { D, M, N, F };
            double sum = 0;
            for (int x = 0; x < 60; x++)
            {
                sum += Get_Table47A_Values(values, x, T, true);
            }

            //Planetary adjustments
            double A1 = 119.75 + 131.849 * T;
            double A2 = 53.09 + 479264.290 * T;

            //Normalize 0-360 degree number
            A1 %= 360;
            if (A1 < 0) { A1 += 360; }
            A2 %= 360;
            if (A2 < 0) { A2 += 360; }

            //Convert DMF to radians
            A1 = A1 * Math.PI / 180;
            A2 = A2 * Math.PI / 180;

            //L TO RADIANS
            L %= 360;
            if (L < 0) { L += 360; }

            //Convert DMF to radians
            L = L * Math.PI / 180;

            sum += 3958 * Math.Sin(A1);
            sum += 1962 * Math.Sin(L - F);
            sum += 318 * Math.Sin(A2);

            return sum;
        }
        /// <summary>
        /// Returns Moon Periodic Value Eb
        /// </summary>
        /// <param name="L">Moon's mean longitude</param>
        /// <param name="D">Moon's mean elongation</param>
        /// <param name="M">Sun's mean anomaly</param>
        /// <param name="N">Moon's mean anomaly</param>
        /// <param name="F">Moon's argument of latitude</param>
        /// <param name="T">Dynamic time</param>
        /// <returns>Eb</returns>
        public static double Moon_Periodic_Eb(double L, double D, double M, double N, double F, double T)
        {
            //Table 47B contains 60 lines to sum
            double[] values = new double[] { D, M, N, F };
            double sum = 0;
            for (int x = 0; x < 60; x++)
            {
                sum += Get_Table47B_Values(values, x, T);
            }

            //Planetary adjustments     
            double A1 = 119.75 + 131.849 * T;
            double A3 = 313.45 + 481266.484 * T;

            //Normalize 0-360 degree number   
            A1 %= 360;
            if (A1 < 0) { A1 += 360; }
            A3 %= 360;
            if (A3 < 0) { A3 += 360; }

            //Convert DMF to radians
            A1 = A1 * Math.PI / 180;
            A3 = A3 * Math.PI / 180;

            //L TO RADIANS
            L %= 360;
            if (L < 0) { L += 360; }

            //Convert DMF to radians
            L = L * Math.PI / 180;

            sum += -2235 * Math.Sin(L);
            sum += 382 * Math.Sin(A3);
            sum += 175 * Math.Sin(A1 - F);
            sum += 175 * Math.Sin(A1 + F);
            sum += 127 * Math.Sin(L - M);
            sum += -115 * Math.Sin(L + M);

            return sum;
        }
        //Ch 50
        /// <summary>
        /// Sum of Apogee Terms from Jean Meeus Astronomical Algorithms Table 50.A
        /// </summary>
        /// <param name="D">Moom's mean elongation at time JDE</param>
        /// <param name="M">Sun's mean anomaly</param>
        /// <param name="F">Moon's arguement f latitude</param>
        /// <param name="T">Time in Julian centuries since epoch 2000</param>
        /// <returns>double</returns>
        public static double ApogeeTermsA(double D, double M, double F, double T)
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
        public static double PerigeeTermsA(double D, double M, double F, double T)
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
        public static double ApogeeTermsB(double D, double M, double F, double T)
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
        public static double PerigeeTermsB(double D, double M, double F, double T)
        {
            //Sum of Perigee Terms from Jean Meeus Astronomical Algorithms Table 50.B          
            double sum = 3629.215;

            sum += Math.Cos(2 * D) * 63.224;
            sum += Math.Cos(4 * D) * -6.990;
            sum += Math.Cos(2 * D - M) * 2.834 - .0071 * T;
            sum += Math.Cos(6 * D) * 1.927;
            sum += Math.Cos(D) * -1.263;
            sum += Math.Cos(8 * D) * -.702;
            sum += Math.Cos(M) * .696 - .0017 * T;
            sum += Math.Cos(2 * F) * -.690;
            sum += Math.Cos(4 * D - M) * -.629 + .0016 * T;
            sum += Math.Cos(2 * D - 2 * F) * -.392;
            sum += Math.Cos(10 * D) * .297;
            sum += Math.Cos(6 * D - M) * .260;
            sum += Math.Cos(3 * D) * .201;
            sum += Math.Cos(2 * D + M) * -.161;
            sum += Math.Cos(D + M) * .157;
            sum += Math.Cos(12 * D) * -.138;
            sum += Math.Cos(8 * D - M) * -.127;
            sum += Math.Cos(2 * D + 2 * F) * .104;
            sum += Math.Cos(2 * D - 2 * M) * .104;
            sum += Math.Cos(5 * D) * -.079;
            sum += Math.Cos(14 * D) * .068;

            sum += Math.Cos(10 * D - M) * .067;
            sum += Math.Cos(4 * D + M) * .054;
            sum += Math.Cos(12 * D - M) * -.038;
            sum += Math.Cos(4 * D - 2 * M) * -.038;
            sum += Math.Cos(7 * D) * .037;
            sum += Math.Cos(4 * D + 2 * F) * -.037;
            sum += Math.Cos(16 * D) * -.035;
            sum += Math.Cos(3 * D + M) * -.030;
            sum += Math.Cos(D - M) * .029;
            sum += Math.Cos(6 * D + M) * -.025;
            sum += Math.Cos(2 * M) * .023;
            sum += Math.Cos(14 * D - M) * .023;
            sum += Math.Cos(2 * D + 2 * M) * -.023;
            sum += Math.Cos(6 * D - 2 * M) * .022;
            sum += Math.Cos(2 * D - 2 * F - M) * -.021;
            sum += Math.Cos(9 * D) * -.020;
            sum += Math.Cos(18 * D) * .019;
            sum += Math.Cos(6 * D + 2 * F) * .017;
            sum += Math.Cos(2 * F - M) * .014;
            sum += Math.Cos(16 * D - M) * -.014;
            sum += Math.Cos(4 * D - 2 * F) * .013;
            sum += Math.Cos(8 * D + M) * .012;
            sum += Math.Cos(11 * D) * .011;
            sum += Math.Cos(5 * D + M) * .010;
            sum += Math.Cos(20 * D) * -.010;

            return sum;
        }

    }
    internal class MeeusFormulas
    {
        public static double Get_Sidereal_Time(double JD)
        {
            //Ch. 12
            //T = Dynamic Time
            //Oo = mean sidereal time at Greenwich at 0h UT
            double T = (JD - 2451545) / 36525;
            double Oo = 280.46061837 + 360.98564736629 * (JD - 2451545) +
                .000387933 * Math.Pow(T, 2) - Math.Pow(T, 3) / 38710000;
            return Oo;
        }
    }
    /// <summary>
    /// Used to display a celestial condition for a specified date.
    /// </summary>
    [Serializable]
    public enum CelestialStatus
    {
        /// <summary>
        /// Celestial body rises and sets on the set day.
        /// </summary>
        RiseAndSet,
        /// <summary>
        /// Celestial body is down all day
        /// </summary>
        DownAllDay,
        /// <summary>
        /// Celestial body is up all day
        /// </summary>
        UpAllDay,
        /// <summary>
        /// Celestial body rises, but does not set on the set day
        /// </summary>
        NoRise,
        /// <summary>
        /// Celestial body sets, but does not rise on the set day
        /// </summary>
        NoSet
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
    /// Moon Illumination Information
    /// </summary>
    [Serializable]
    public class MoonIllum
    {

        /// <summary>
        /// Moon's fraction
        /// </summary>
        public double Fraction { get; internal set; }
        /// <summary>
        /// Moon's Angle
        /// </summary>
        public double Angle { get; internal set; }
        /// <summary>
        /// Moon's phase
        /// </summary>
        public double Phase { get; internal set; }
        /// <summary>
        /// Moon's phase name for the specified day
        /// </summary>
        public string PhaseName { get; internal set; }

    }
    /// <summary>
    /// Stores Perigee or Apogee values
    /// </summary>
    [Serializable]
    public class PerigeeApogee
    {
        private DateTime date;
        private double horizontalParallax;
        private Distance distance;

        /// <summary>
        /// Initializes a Perigee or Apogee object
        /// </summary>
        /// <param name="d">Date of Event</param>
        /// <param name="p">Horizontal Parallax</param>
        /// <param name="dist">Distance</param>
        public PerigeeApogee(DateTime d, double p, Distance dist)
        {
            date = d;
            horizontalParallax = p;
            distance = dist;
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
        /// <summary>
        /// Moon's distance at event.
        /// </summary>
        public Distance Distance
        {
            get { return distance; }
        }

        internal void Convert_To_Local_Time(double offset)
        {
            FieldInfo[] fields = typeof(PerigeeApogee).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType == typeof(DateTime))
                {
                    DateTime d = (DateTime)field.GetValue(this);
                    if (d > new DateTime())
                    {
                        d = d.AddHours(offset);
                        field.SetValue(this, d);
                    }
                }
            }
        }

    }
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
            if(m <= 2) { m += 12; y -= 1; }

            double A = (int)(d.Year / 100.0);
            double B = 0;

            //Gregorian Start Date
            if(d >= new DateTime(1582,10,15))
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
            double month = E-1;
            if (E > 13) { month -= 12; }

            //year is C-4716 if month>2 and C-4715 if month is 1 or 2
            double year = C - 4715;
            if(month >2)
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
            return GetDate_FromJulian(j+J2000);
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
    /// <summary>
    /// Contains last and next perigee
    /// </summary>
    [Serializable]
    public class Perigee
    {
        private PerigeeApogee lastPerigee;
        private PerigeeApogee nextPerigee;

        /// <summary>
        /// Initializes an Perigee object.
        /// </summary>
        /// <param name="last"></param>
        /// <param name="next"></param>
        public Perigee(PerigeeApogee last, PerigeeApogee next)
        {
            lastPerigee = last;
            nextPerigee = next;
        }

        /// <summary>
        /// Last perigee
        /// </summary>
        public PerigeeApogee LastPerigee { get { return lastPerigee; } }
        /// <summary>
        /// Next perigee
        /// </summary>
        public PerigeeApogee NextPerigee { get { return nextPerigee; } }

        internal void ConvertTo_Local_Time(double offset)
        {
            LastPerigee.Convert_To_Local_Time(offset);
            NextPerigee.Convert_To_Local_Time(offset);
        }

    }
    /// <summary>
    /// Contains last and next apogee
    /// </summary>
    [Serializable]
    public class Apogee
    {
        private PerigeeApogee lastApogee;
        private PerigeeApogee nextApogee;

        /// <summary>
        /// Initializes an Apogee object.
        /// </summary>
        /// <param name="last"></param>
        /// <param name="next"></param>
        public Apogee(PerigeeApogee last, PerigeeApogee next)
        {
            lastApogee = last;
            nextApogee = next;
        }

        /// <summary>
        /// Last apogee
        /// </summary>
        public PerigeeApogee LastApogee { get { return lastApogee; } }
        /// <summary>
        /// Next apogee
        /// </summary>
        public PerigeeApogee NextApogee { get { return nextApogee; } }

        internal void ConvertTo_Local_Time(double offset)
        {
            LastApogee.Convert_To_Local_Time(offset);
            NextApogee.Convert_To_Local_Time(offset);
        }
    }
    /// <summary>
    /// Astrological Signs
    /// </summary>
    [Serializable]
    public class AstrologicalSigns
    {
        /// <summary>
        /// Astrological Zodiac Sign
        /// </summary>
        public string MoonName { get; internal set; }
        /// <summary>
        /// Astrological Moon Sign
        /// </summary>
        public string MoonSign { get; internal set; }
        /// <summary>
        /// Astrological Zodiac Sign
        /// </summary>
        public string ZodiacSign { get; internal set; }
    }
    /// <summary>
    /// Additional Solar Time Information
    /// </summary>
    [Serializable]
    public class AdditionalSolarTimes
    {
        /// <summary>
        /// Create an AdditionalSolarTimes object.
        /// </summary>
        public AdditionalSolarTimes()
        {
            //Set dates to avoid null errors. If year return 1900 event did not occur.
            CivilDawn = new DateTime();
            CivilDusk = new DateTime();
            NauticalDawn = new DateTime();
            NauticalDusk = new DateTime();

        }
        /// <summary>
        /// Returns Civil Dawn Time
        /// </summary>
        public DateTime? CivilDawn { get; internal set; }
        /// <summary>
        /// Returns Civil Dusk Time
        /// </summary>
        public DateTime? CivilDusk { get; internal set; }
        /// <summary>
        /// Returns Nautical Dawn Time
        /// </summary>
        public DateTime? NauticalDawn { get; internal set; }
        /// <summary>
        /// Returns Nautical Dusk Time
        /// </summary>
        public DateTime? NauticalDusk { get; internal set; }
        /// <summary>
        /// Returns Astronomical Dawn Time
        /// </summary>
        public DateTime? AstronomicalDawn { get; internal set; }
        /// <summary>
        /// Returns Astronomical Dusk Time
        /// </summary>
        public DateTime? AstronomicalDusk { get; internal set; }

        /// <summary>
        /// Returns the time when the bottom of the solar disc touches the horizon after sunrise
        /// </summary>
        public DateTime? SunriseBottomDisc{ get; internal set; }
        /// <summary>
        /// Returns the time when the bottom of the solar disc touches the horizon before sunset
        /// </summary>
        public DateTime? SunsetBottomDisc { get; internal set; }

        internal void Convert_To_Local_Time(double offset)
        {
            FieldInfo[] fields = typeof(AdditionalSolarTimes).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType == typeof(DateTime?))
                {
                    DateTime? d = (DateTime?)field.GetValue(this);
                    if (d.HasValue)
                    {
                        if (d > new DateTime())
                        {
                            d = d.Value.AddHours(offset);
                            field.SetValue(this, d);
                        }
                    }
                }
            }
        }
    }
    /// <summary>
    /// Class containing solar eclipse information
    /// </summary>
    [Serializable]
    public class SolarEclipse
    {
        /// <summary>
        /// Initialize a SolarEclipse object
        /// </summary>
        public SolarEclipse()
        {
            LastEclipse = new SolarEclipseDetails();
            NextEclipse = new SolarEclipseDetails();
        }
        /// <summary>
        /// Details about the previous solar eclipse
        /// </summary>
        public SolarEclipseDetails LastEclipse { get; internal set; }
        /// <summary>
        /// Details about the next solar eclipse
        /// </summary>
        public SolarEclipseDetails NextEclipse { get; internal set; }

        internal void ConvertTo_LocalTime(double offset)
        {
            LastEclipse.Convert_To_Local_Time(offset);
            NextEclipse.Convert_To_Local_Time(offset);
        }
    }
    /// <summary>
    /// Class containing lunar eclipse information
    /// </summary>
    [Serializable]
    public class LunarEclipse
    {
        /// <summary>
        /// Initialize a LunarEclipse object
        /// </summary>
        public LunarEclipse()
        {
            LastEclipse = new LunarEclipseDetails();
            NextEclipse = new LunarEclipseDetails();
        }
        /// <summary>
        /// Details about the previous lunar eclipse
        /// </summary>
        public LunarEclipseDetails LastEclipse { get; internal set; }
        /// <summary>
        /// Details about the next lunar eclipse
        /// </summary>
        public LunarEclipseDetails NextEclipse { get; internal set; }

        internal void ConvertTo_LocalTime(double offset)
        {
            LastEclipse.Convert_To_Local_Time(offset);
            NextEclipse.Convert_To_Local_Time(offset);
        }
    }
    /// <summary>
    /// Class containing specific solar eclipse information
    /// </summary>
    [Serializable]
    public class SolarEclipseDetails
    {
        private DateTime date;
        private SolarEclipseType type;
        private DateTime partialEclispeBegin;
        private DateTime aorTEclipseBegin;
        private DateTime maximumEclipse;
        private DateTime aorTEclipseEnd;
        private DateTime partialEclispeEnd;
        private TimeSpan aorTDuration;
        private bool hasEclipseData;

        /// <summary>
        /// Initialize a SolarEclipseDetails object
        /// </summary>
        /// <param name="values">Solar Eclipse String Values</param>
        public SolarEclipseDetails(List<string> values)
        {
            //Eclipse has value
            hasEclipseData = true;
            //Set Eclipse Date
            date = Convert.ToDateTime(values[0]);

            switch (values[1])
            {
                case "P":
                    type = SolarEclipseType.Partial;
                    break;
                case "A":
                    type = SolarEclipseType.Annular;
                    break;
                case "T":
                    type = SolarEclipseType.Total;
                    break;
                default:
                    break;
            }
            TimeSpan ts;
            //Eclipse start
            if (TimeSpan.TryParse(values[2], out ts))
            {
                partialEclispeBegin = date.Add(ts);
            }
            //A or T start
            if (TimeSpan.TryParse(values[4], out ts))
            {
                aorTEclipseBegin = date.Add(ts);
            }
            //Maximum Eclipse
            if (TimeSpan.TryParse(values[5], out ts))
            {
                maximumEclipse = date.Add(ts);
            }
            //A or T ends
            if (TimeSpan.TryParse(values[8], out ts))
            {
                aorTEclipseEnd = date.Add(ts);
            }
            //Eclipse end
            if (TimeSpan.TryParse(values[9], out ts))
            {
                partialEclispeEnd = date.Add(ts);
            }
            //A or T Duration
            if (values[13] != "-")
            {
                string s = values[13].Replace("m", ":").Replace("s", "");
                string[] ns = s.Split(':');
                int mins = 0;
                int secs = 0;

                int.TryParse(ns[0], out mins);
                if (ns.Count() > 0)
                {
                    int.TryParse(ns[1], out secs);
                }

                TimeSpan time = new TimeSpan(0, mins, secs);

                aorTDuration = time;
            }
            else
            {
                aorTDuration = new TimeSpan();
            }
            Adjust_Dates();//Adjust dates if required (needed when eclipse crosses into next day).
        }
        /// <summary>
        /// Initialize an empty SolarEclipseDetails object
        /// </summary>
        public SolarEclipseDetails()
        {
            hasEclipseData = false;
        }
        /// <summary>
        /// JS Eclipse Calc formulas didn't account for Z time calculation.
        /// Iterate through and adjust Z dates where eclipse is passed midnight.
        /// </summary>
        private void Adjust_Dates()
        {
            //Load array in reverse event order
            DateTime[] dateArray = new DateTime[] { partialEclispeBegin, aorTEclipseBegin, maximumEclipse, aorTEclipseEnd, partialEclispeEnd };
            DateTime baseTime = partialEclispeEnd;
            bool multiDay = false; //used to detrmine if eclipse crossed into next Z day

            for (int x = 4; x >= 0; x--)
            {
                DateTime d = dateArray[x];
                //Check if date exist
                if (d > new DateTime())
                {

                    //Adjust if time is less than then baseTime.
                    if (d > baseTime)
                    {
                        switch (x)
                        {
                            case 3:
                                aorTEclipseEnd = aorTEclipseEnd.AddDays(-1);
                                break;
                            case 2:
                                maximumEclipse = maximumEclipse.AddDays(-1);
                                break;
                            case 1:
                                aorTEclipseBegin = aorTEclipseBegin.AddDays(-1);
                                break;
                            case 0:
                                partialEclispeBegin = partialEclispeBegin.AddDays(-1);
                                break;
                            default:
                                break;
                        }

                        multiDay = true;//Set true to change base date value.
                    }
                }
            }
            if (multiDay)
            {
                this.date = this.date.AddDays(-1); //Shave day off base date if multiday.
            }
        }
        /// <summary>
        /// Determine if the SolarEclipseDetails object has been populated
        /// </summary>
        public bool HasEclipseData { get { return hasEclipseData; } }
        /// <summary>
        /// Date of solar eclipse
        /// </summary>
        public DateTime Date { get { return date; } }
        /// <summary>
        /// Solar eclipse type
        /// </summary>
        public SolarEclipseType Type { get { return type; } }
        /// <summary>
        /// DateTime when the partial eclipse begins
        /// </summary>
        public DateTime PartialEclispeBegin { get { return partialEclispeBegin; } }
        /// <summary>
        /// DateTime when an Annular or Total eclipse begins (if applicable)
        /// </summary>
        public DateTime AorTEclipseBegin { get { return aorTEclipseBegin; } }
        /// <summary>
        /// DateTime when eclipse is at Maximum
        /// </summary>
        public DateTime MaximumEclipse { get { return maximumEclipse; } }

        /// <summary>
        /// DateTime when the Annular or Total eclipse ends (if applicable)
        /// </summary>
        public DateTime AorTEclipseEnd { get { return aorTEclipseEnd; } }
        /// <summary>
        /// DateTime when the partial elipse ends
        /// </summary>
        public DateTime PartialEclispeEnd { get { return partialEclispeEnd; } }
        /// <summary>
        /// Duration of Annular or Total eclipse (if applicable)
        /// </summary>
        public TimeSpan AorTDuration { get { return aorTDuration; } }
        /// <summary>
        /// Solat eclipse default string
        /// </summary>
        /// <returns>Solar eclipse base date string</returns>
        public override string ToString()
        {
            return date.ToString("dd-MMM-yyyy");
        }

        internal void Convert_To_Local_Time(double offset)
        {
            FieldInfo[] fields = typeof(SolarEclipseDetails).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType == typeof(DateTime))
                {
                    DateTime d = (DateTime)field.GetValue(this);
                    if (d > new DateTime())
                    {
                        d = d.AddHours(offset);
                        field.SetValue(this, d);
                    }
                }
            }
          
            date = partialEclispeBegin.Date;
        }
    }
    /// <summary>
    /// Class containing specific lunar eclipse information
    /// </summary>
    [Serializable]
    public class LunarEclipseDetails
    {
        private DateTime date;
        private LunarEclipseType type;
        private DateTime penumbralEclipseBegin;
        private DateTime partialEclispeBegin;
        private DateTime totalEclipseBegin;
        private DateTime midEclipse;
        private DateTime totalEclipseEnd;
        private DateTime partialEclispeEnd;
        private DateTime penumbralEclipseEnd;

        private bool hasEclipseData;

        /// <summary>
        /// Initialize a LunarEclipseDetails object
        /// </summary>
        /// <param name="values">Lunar Eclipse String Values</param>
        public LunarEclipseDetails(List<string> values)
        {
            //Eclipse has value
            hasEclipseData = true;
            //Set Eclipse Date
            date = Convert.ToDateTime(values[0]);
            switch (values[1])
            {
                case "T":
                    type = LunarEclipseType.Total;
                    break;
                case "P":
                    type = LunarEclipseType.Partial;
                    break;
                case "N":
                    type = LunarEclipseType.Penumbral;
                    break;
                default:
                    break;
            }
            TimeSpan ts;
            //Penumbral Eclipse start
            if (TimeSpan.TryParse(values[4], out ts))
            {
                penumbralEclipseBegin = date.Add(ts);
            }
            //PartialEclipse start
            if (TimeSpan.TryParse(values[6], out ts))
            {
                partialEclispeBegin = date.Add(ts);
            }
            //Total start
            if (TimeSpan.TryParse(values[8], out ts))
            {
                totalEclipseBegin = date.Add(ts);
            }
            //Mid Eclipse
            if (TimeSpan.TryParse(values[10], out ts))
            {
                midEclipse = date.Add(ts);
            }
            //Total ends
            if (TimeSpan.TryParse(values[12], out ts))
            {
                totalEclipseEnd = date.Add(ts);
            }
            //Partial Eclipse end
            if (TimeSpan.TryParse(values[14], out ts))
            {
                partialEclispeEnd = date.Add(ts);
            }
            //Penumbral Eclipse end
            if (TimeSpan.TryParse(values[16], out ts))
            {
                penumbralEclipseEnd = date.Add(ts);
            }
            Adjust_Dates();
        }
        /// <summary>
        /// Initialize an empty LunarEclipseDetails object
        /// </summary>
        public LunarEclipseDetails()
        {
            hasEclipseData = false;
        }
        /// <summary>
        /// JS Eclipse Calc formulas didn't account for Z time calculation.
        /// Iterate through and adjust Z dates where eclipse is passed midnight.
        /// </summary>
        private void Adjust_Dates()
        {
            //Load array in squential order.
            DateTime[] dateArray = new DateTime[] { penumbralEclipseBegin, partialEclispeBegin, totalEclipseBegin, midEclipse, totalEclipseEnd, partialEclispeEnd, penumbralEclipseEnd };
            DateTime baseTime = partialEclispeEnd;
            bool multiDay = false; //used to detrmine if eclipse crossed into next Z day
            baseTime = penumbralEclipseBegin;
            for (int x = 0; x < dateArray.Count(); x++)
            {
                DateTime d = dateArray[x];
                //Check if date exist
                if (d > new DateTime())
                {
                    if (d < baseTime)
                    {
                        multiDay = true;
                    }
                }
                baseTime = dateArray[x];
                if (multiDay == true)
                {
                    switch (x)
                    {
                        case 1:
                            partialEclispeBegin = partialEclispeBegin.AddDays(1);
                            break;
                        case 2:
                            totalEclipseBegin = totalEclipseBegin.AddDays(1);
                            break;
                        case 3:
                            midEclipse = midEclipse.AddDays(1);
                            break;
                        case 4:
                            totalEclipseEnd = totalEclipseEnd.AddDays(1);
                            break;
                        case 5:
                            partialEclispeEnd = partialEclispeEnd.AddDays(1);
                            break;
                        case 6:
                            penumbralEclipseEnd = penumbralEclipseEnd.AddDays(1);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Determine if the LunarEclipseDetails object has been populated
        /// </summary>
        public bool HasEclipseData { get { return hasEclipseData; } }
        /// <summary>
        /// Date of lunar eclipse
        /// </summary>
        public DateTime Date { get { return date; } }
        /// <summary>
        /// Lunar eclipse type
        /// </summary>
        public LunarEclipseType Type { get { return type; } }
        /// <summary>
        /// DateTime when the penumbral eclipse begins
        /// </summary>
        public DateTime PenumbralEclipseBegin { get { return penumbralEclipseBegin; } }
        /// <summary>
        /// DateTime when the partial eclipse begins (if applicable)
        /// </summary>
        public DateTime PartialEclispeBegin { get { return partialEclispeBegin; } }
        /// <summary>
        /// DateTime when Total eclipse begins (if applicable)
        /// </summary>
        public DateTime TotalEclipseBegin { get { return totalEclipseBegin; } }
        /// <summary>
        /// DateTime when eclipse is at Mid
        /// </summary>
        public DateTime MidEclipse { get { return midEclipse; } }
        /// <summary>
        /// DateTime when Total eclipse ends (if applicable)
        /// </summary>
        public DateTime TotalEclipseEnd { get { return totalEclipseEnd; } }
        /// <summary>
        /// DateTime when the partial elipse ends (if applicable)
        /// </summary>
        public DateTime PartialEclispeEnd { get { return partialEclispeEnd; } }
        /// <summary>
        /// DateTime when the penumbral elipse ends
        /// </summary>
        public DateTime PenumbralEclispeEnd { get { return penumbralEclipseEnd; } }
        /// <summary>
        /// Lunar eclipse default string
        /// </summary>
        /// <returns>Lunar eclipse base date string</returns>
        public override string ToString()
        {
            return date.ToString("dd-MMM-yyyy");
        }

        internal void Convert_To_Local_Time(double offset)
        {       
            FieldInfo[] fields = typeof(LunarEclipseDetails).GetFields(BindingFlags.NonPublic |BindingFlags.Instance);
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType == typeof(DateTime))
                {
                    DateTime d = (DateTime)field.GetValue(this);
                    if (d > new DateTime())
                    {
                        d = d.AddHours(offset);
                        field.SetValue(this, d);
                    }
                }
            }
            date = penumbralEclipseBegin.Date;
           
        }

    }
    internal class MoonTimes
    {
        public DateTime set { get; internal set; }
        public DateTime rise { get; internal set; }
        public CelestialStatus status { get; internal set; }
    }
    internal class MoonPosition
    {
        public double Azimuth { get; internal set; }
        public double Altitude { get; internal set; }
        public Distance Distance { get; internal set; }
        public double ParallacticAngle { get; internal set; }
        public double ParallaxCorection { get; internal set; }
    }
    internal class CelCoords
    {
        public double ra { get; internal set; }
        public double dec { get; internal set; }
        public double dist { get; internal set; }
    }

    /// <summary>
    /// Solar eclipse type
    /// </summary>
    [Serializable]
    public enum SolarEclipseType
    {
        /// <summary>
        /// Partial Eclipse
        /// </summary>
        Partial,
        /// <summary>
        /// Annular Eclipse
        /// </summary>
        Annular,
        /// <summary>
        /// Total Eclipse...of the heart...
        /// </summary>
        Total
    }
    /// <summary>
    /// Lunar eclipse type
    /// </summary>
    [Serializable]
    public enum LunarEclipseType
    {
        /// <summary>
        /// Penumbral Eclipse
        /// </summary>
        Penumbral,
        /// <summary>
        /// Partial Eclipse
        /// </summary>
        Partial,
        /// <summary>
        /// Total Eclipse...of the heart...
        /// </summary>
        Total
    }
}
