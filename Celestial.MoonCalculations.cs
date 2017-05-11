using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoordinateSharp
{
    internal class MoonCalc
    {
        static double rad = Math.PI / 180;
        static double dayMs = 1000 * 60 * 60 * 24, J1970 = 2440588, J2000 = 2451545;
        static double e = rad * 23.4397;

        public static void GetMoonTimes(DateTime date, double lat, double lng, Celestial c)
        {
            c.MoonRise = null;
            c.MoonSet = null;
            DateTime t = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);
            c.MoonSet = null;
            c.MoonSet = null;
            double hc = 0.133 * rad,
            h0 = GetMoonPosition(t, lat, lng).Altitude - hc,
            h1, h2, a, b, xe, ye, d, roots, dx;
            double? x1 = null, x2 = null, rise = null, set = null;

            bool isRise = false;
            bool isSet = false;
            bool isNeg;
            if (h0 < 0)
            {
                isNeg = true;
            }
            else
            {
                isNeg = false;
            }
            // go in 2-hour chunks, each time seeing if a 3-point quadratic curve crosses zero (which means rise or set)
            for (var i = 1; i <= 24; i += 2)
            {

                h1 = GetMoonPosition(hoursLater(t, i), lat, lng).Altitude - hc;
                h2 = GetMoonPosition(hoursLater(t, i + 1), lat, lng).Altitude - hc;
                if (isNeg && h1 >= 0 || isNeg && h2 >= 0) { isNeg = false; isRise = true; }
                if (!isNeg && h1 < 0 || !isNeg && h2 < 0) { isNeg = true; isSet = true; }

                a = (h0 + h2) / 2 - h1;
                b = (h2 - h0) / 2;
                xe = -b / (2 * a);
                ye = (a * xe + b) * xe + h1;
                d = b * b - 4 * a * h1;
                roots = 0;

                if (d >= 0)
                {
                    dx = Math.Sqrt(d) / (Math.Abs(a) * 2);
                    x1 = xe - dx;
                    x2 = xe + dx;
                    if (Math.Abs(x1.Value) <= 1) roots++;
                    if (Math.Abs(x2.Value) <= 1) roots++;
                    if (x1 < -1) x1 = x2;
                }

                if (roots == 1)
                {
                    if (h0 < 0) rise = i + x1;
                    else set = i + x1;

                }
                else if (roots == 2)
                {
                    rise = i + (ye < 0 ? x2 : x1);
                    set = i + (ye < 0 ? x1 : x2);
                }

                if (rise != null && set != null) break;

                h0 = h2;
            }


            if (rise != null) { c.MoonRise = hoursLater(t, rise.Value); }
            if (set != null) { c.MoonSet = hoursLater(t, set.Value); }

            if (isRise && isSet) { c.MoonCondition = CelestialStatus.RiseAndSet; }
            else
            {
                if (!isRise && !isSet)
                {
                    if (h0 >= 0) { c.MoonCondition = CelestialStatus.UpAllDay; }
                    else { c.MoonCondition = CelestialStatus.DownAllDay; }
                }
                if (!isRise && isSet) { c.MoonCondition = CelestialStatus.NoRise; }
                if (isRise && !isSet) { c.MoonCondition = CelestialStatus.NoSet; }
            }

        }
        static MoonPosition GetMoonPosition(DateTime date, double lat, double lng)
        {
            double d = toDays(date);

            CelCoords c = GetMoonCoords(d);
            double lw = rad * -lng;
            double phi = rad * lat;
            double H = siderealTime(d, lw) - c.ra;
            double h = altitude(H, phi, c.dec);

            // formula 14.1 of "Astronomical Algorithms" 2nd edition by Jean Meeus (Willmann-Bell, Richmond) 1998.
            double pa = Math.Atan2(Math.Sin(H), Math.Tan(phi) * Math.Cos(c.dec) - Math.Sin(c.dec) * Math.Cos(H));

            h = h + astroRefraction(h); // altitude correction for refraction

            MoonPosition mp = new MoonPosition();
            mp.Azimuth = azimuth(H, phi, c.dec);
            mp.Altitude = h;
            mp.Distance = c.dist;
            mp.ParallacticAngle = pa;
            return mp;
        }
        static CelCoords GetMoonCoords(double d)
        { // geocentric ecliptic coordinates of the moon

            double L = rad * (218.316 + 13.176396 * d), // ecliptic longitude
                M = rad * (134.963 + 13.064993 * d), // mean anomaly
                F = rad * (93.272 + 13.229350 * d),  // mean distance

                l = L + rad * 6.289 * Math.Sin(M), // longitude
                b = rad * 5.128 * Math.Sin(F),     // latitude
                dt = 385001 - 20905 * Math.Cos(M);  // distance to the moon in km
            CelCoords mc = new CelCoords();
            mc.ra = rightAscension(l, b);
            mc.dec = declination(l, b);
            mc.dist = dt;
            return mc;
        }

        public static void GetMoonIllumination(DateTime date, Celestial c)
        {
            date = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);
            double d = toDays(date);
            CelCoords s = GetSunCoords(d);
            CelCoords m = GetMoonCoords(d);

            double sdist = 149598000,
            phi = Math.Acos(Math.Sin(s.dec) * Math.Sin(m.dec) + Math.Cos(s.dec) * Math.Cos(m.dec) * Math.Cos(s.ra - m.ra)),
            inc = Math.Atan2(sdist * Math.Sin(phi), m.dist - sdist * Math.Cos(phi)),
            angle = Math.Atan2(Math.Cos(s.dec) * Math.Sin(s.ra - m.ra), Math.Sin(s.dec) * Math.Cos(m.dec) -
                    Math.Cos(s.dec) * Math.Sin(m.dec) * Math.Cos(s.ra - m.ra));
            MoonIllum mi = new MoonIllum();
            mi.Fraction = (1 + Math.Cos(inc)) / 2;
            mi.Phase = 0.5 + 0.5 * inc * (angle < 0 ? -1 : 1) / Math.PI;
            mi.Angle = angle;
            c.MoonPhase = mi.Phase;

        }

        //Moon Time Functions
        private static CelCoords GetSunCoords(double d)
        {
            double M = solarMeanAnomaly(d),
                L = eclipticLongitude(M);
            CelCoords c = new CelCoords();
            c.dec = declination(L, 0);
            c.ra = rightAscension(L, 0);
            return c;
        }
        private static double solarMeanAnomaly(double d) { return rad * (357.5291 + 0.98560028 * d); }

        private static double eclipticLongitude(double M)
        {
            double C = rad * (1.9148 * Math.Sin(M) + 0.02 * Math.Sin(2 * M) + 0.0003 * Math.Sin(3 * M)), // equation of center
                P = rad * 102.9372; // perihelion of the Earth

            return M + C + P + Math.PI;
        }
        private static DateTime hoursLater(DateTime date, double h)
        {
            return date.AddHours(h);
        }
        static double toJulian(DateTime date)
        {
            DateTime d = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime t = date;
            double l = (double)(t - d).TotalMilliseconds;

            return l / dayMs - 0.5 + J1970;
        }
        static double toDays(DateTime date)
        {
            return toJulian(date) - J2000;
        }
        static double rightAscension(double l, double b) { return Math.Atan2(Math.Sin(l) * Math.Cos(e) - Math.Tan(b) * Math.Sin(e), Math.Cos(l)); }
        static double declination(double l, double b) { return Math.Asin(Math.Sin(b) * Math.Cos(e) + Math.Cos(b) * Math.Sin(e) * Math.Sin(l)); }
        static double azimuth(double H, double phi, double dec) { return Math.Atan2(Math.Sin(H), Math.Cos(H) * Math.Sin(phi) - Math.Tan(dec) * Math.Cos(phi)); }
        static double altitude(double H, double phi, double dec) { return Math.Asin(Math.Sin(phi) * Math.Sin(dec) + Math.Cos(phi) * Math.Cos(dec) * Math.Cos(H)); }
        static double siderealTime(double d, double lw) { return rad * (280.16 + 360.9856235 * d) - lw; }
        static double astroRefraction(double h)
        {
            // the following formula works for positive altitudes only.
            if (h < 0)
            {
                h = 0; // if h = -0.08901179 a div/0 would occur.
            }

            // formula 16.4 of "Astronomical Algorithms" 2nd edition by Jean Meeus (Willmann-Bell, Richmond) 1998.
            // 1.02 / tan(h + 10.26 / (h + 5.10)) h in degrees, result in arc minutes -> converted to rad:
            return 0.0002967 / Math.Tan(h + 0.00312536 / (h + 0.08901179));
        }

        public class MoonTimes
        {
            public DateTime set { get; set; }
            public DateTime rise { get; set; }
            public CelestialStatus status { get; set; }
        }
        public class MoonPosition
        {
            public double Azimuth { get; set; }
            public double Altitude { get; set; }
            public double Distance { get; set; }
            public double ParallacticAngle { get; set; }
        }
        public class CelCoords
        {
            public double ra { get; set; }
            public double dec { get; set; }
            public double dist { get; set; }
        }
        public class MoonIllum
        {
            public double Fraction { get; set; }
            public double Angle { get; set; }
            public double Phase { get; set; }
        }
    }
}
