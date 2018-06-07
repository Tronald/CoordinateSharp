using System;
using System.Collections.Generic;
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
            h0 = GetMoonPosition(t, lat, lng, c).Altitude - hc,
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
                h1 = GetMoonPosition(hoursLater(t, i), lat, lng, c).Altitude - hc;
                h2 = GetMoonPosition(hoursLater(t, i + 1), lat, lng, c).Altitude - hc;
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
        static MoonPosition GetMoonPosition(DateTime date, double lat, double lng, Celestial cel)
        {
            date = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, DateTimeKind.Utc);
            double d = toDays(date);

            CelCoords c = GetMoonCoords(d, cel);
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
        static CelCoords GetMoonCoords(double d, Celestial c)
        {   
            // geocentric ecliptic coordinates of the moon
            //Formulas used from http://aa.quae.nl/en/reken/hemelpositie.html#1_3
            double L = rad * (218.316 + 13.176396 * d), // ecliptic longitude
                M = rad * (134.963 + 13.064993 * d), // mean anomaly
                F = rad * (93.272 + 13.229350 * d),  // mean distance

                l = L + rad * 6.289 * Math.Sin(M), // longitude
                b = rad * 5.128 * Math.Sin(F),     // latitude
                dt = 385001 - 20905 * Math.Cos(M);  // distance to the moon in km
            //c.MoonSign = MoonSign(l);
            CelCoords mc = new CelCoords();
            mc.ra = rightAscension(l, b);
            mc.dec = declination(l, b);
            mc.dist = dt;
            return mc;
        }
      
        public static void GetMoonIllumination(DateTime date, Celestial c, double lat, double lng)
        {
            date = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, DateTimeKind.Utc);
         
            double d = toDays(date);
            CelCoords s = GetSunCoords(d);
            CelCoords m = GetMoonCoords(d, c);

            double sdist = 149598000,
            phi = Math.Acos(Math.Sin(s.dec) * Math.Sin(m.dec) + Math.Cos(s.dec) * Math.Cos(m.dec) * Math.Cos(s.ra - m.ra)),
            inc = Math.Atan2(sdist * Math.Sin(phi), m.dist - sdist * Math.Cos(phi)),
            angle = Math.Atan2(Math.Cos(s.dec) * Math.Sin(s.ra - m.ra), Math.Sin(s.dec) * Math.Cos(m.dec) -
                    Math.Cos(s.dec) * Math.Sin(m.dec) * Math.Cos(s.ra - m.ra));


            MoonIllum mi = new MoonIllum();

            mi.Fraction = (1 + Math.Cos(inc)) / 2;
            mi.Phase = 0.5 + 0.5 * inc * (angle < 0 ? -1 : 1) / Math.PI;
            mi.Angle = angle;

           
            c.MoonIllum = mi;
            
             string moonName = "";
             int moonDate = 0;
            //GET PHASE NAME

            //CHECK MOON AT BEGINNING AT END OF DAY TO GET DAY PHASE IN UTC
            DateTime dMon = new DateTime(date.Year, date.Month, 1);
            for(int x = 1;x<= date.Day;x++)
            {               
                DateTime nDate = new DateTime(dMon.Year, dMon.Month, x, 0, 0, 0, DateTimeKind.Utc);
                d = toDays(nDate);
                s = GetSunCoords(d);
                m = GetMoonCoords(d, c);

                phi = Math.Acos(Math.Sin(s.dec) * Math.Sin(m.dec) + Math.Cos(s.dec) * Math.Cos(m.dec) * Math.Cos(s.ra - m.ra));
                inc = Math.Atan2(sdist * Math.Sin(phi), m.dist - sdist * Math.Cos(phi));
                angle = Math.Atan2(Math.Cos(s.dec) * Math.Sin(s.ra - m.ra), Math.Sin(s.dec) * Math.Cos(m.dec) -
                        Math.Cos(s.dec) * Math.Sin(m.dec) * Math.Cos(s.ra - m.ra));

                double startPhase = 0.5 + 0.5 * inc * (angle < 0 ? -1 : 1) / Math.PI;

                nDate = new DateTime(dMon.Year, dMon.Month, x, 23, 59, 59, DateTimeKind.Utc);
                d = toDays(nDate);
                s = GetSunCoords(d);
                m = GetMoonCoords(d, c);

                phi = Math.Acos(Math.Sin(s.dec) * Math.Sin(m.dec) + Math.Cos(s.dec) * Math.Cos(m.dec) * Math.Cos(s.ra - m.ra));
                inc = Math.Atan2(sdist * Math.Sin(phi), m.dist - sdist * Math.Cos(phi));
                angle = Math.Atan2(Math.Cos(s.dec) * Math.Sin(s.ra - m.ra), Math.Sin(s.dec) * Math.Cos(m.dec) -
                        Math.Cos(s.dec) * Math.Sin(m.dec) * Math.Cos(s.ra - m.ra));

                double endPhase = 0.5 + 0.5 * inc * (angle < 0 ? -1 : 1) / Math.PI;
                //Determine Moon Name.
                if (startPhase <= .5 && endPhase >= .5)
                {
                    moonDate = x;
                    moonName = GetMoonName(dMon.Month, moonName);
                }
                //Get Moon Name (month, string);
                //Get Moon Phase Name          
                if (date.Day == x)
                {
                    if (startPhase > endPhase)
                    {
                        mi.PhaseName = "New Moon";
                        break;
                    }
                    if (startPhase <= .25 && endPhase >= .25)
                    {
                        mi.PhaseName = "First Quarter";
                        break;
                    }
                    if (startPhase <= .5 && endPhase >= .5)
                    {
                        mi.PhaseName = "Full Moon";                       
                        break;
                    }
                    if (startPhase <= .75 && endPhase >= .75)
                    {
                        mi.PhaseName = "Last Quarter";
                        break;
                    }

                    if (startPhase > 0 && startPhase < .25 && endPhase > 0 && endPhase < .25)
                    {
                        mi.PhaseName = "Waxing Crescent";
                        break;
                    }
                    if (startPhase > .25 && startPhase < .5 && endPhase > .25 && endPhase < .5)
                    {
                        mi.PhaseName = "Waxing Gibbous";
                        break;
                    }
                    if (startPhase > .5 && startPhase < .75 && endPhase > .5 && endPhase < .75)
                    {
                        mi.PhaseName = "Waning Gibbous";
                        break;
                    }
                    if (startPhase > .75 && startPhase < 1 && endPhase > .75 && endPhase < 1)
                    {
                        mi.PhaseName = "Waning Crescent";
                        break;
                    }
                }
               
            }
            if (date.Day == moonDate)
            {
                c.AstrologicalSigns.MoonName = moonName;
            }
            else { c.AstrologicalSigns.MoonName = ""; }
            CalculateLunarEclipse(date, lat, lng, c);

        }
        public static void CalculateLunarEclipse(DateTime date, double lat, double longi, Celestial c)
        {
            //Convert to Radian
            double latR = lat * Math.PI / 180;
            double longR = longi * Math.PI / 180;
            List<List<string>> se = LunarEclipseCalc.CalculateLunarEclipse(date, latR, longR);
            //RETURN FIRST AND LAST
            if (se.Count == 0) { return; }
            //FIND LAST AND NEXT ECLIPSE
            int lastE = -1;
            int nextE = -1;
            int currentE = 0;
            DateTime lastDate = new DateTime();
            DateTime nextDate = new DateTime(3300, 1, 1);
            //Iterate to get last and next eclipse
         
            foreach (List<string> values in se)
            {
                DateTime ld = Convert.ToDateTime(values[0], System.Globalization.CultureInfo.InvariantCulture);
                if (ld < date && ld > lastDate) { lastDate = ld; lastE = currentE; }
                if (ld >= date && ld < nextDate) { nextDate = ld; nextE = currentE; }
                currentE++;
            }
            //SET ECLIPSE DATA
            if (lastE >= 0)
            {
                c.LunarEclipse.LastEclipse = new LunarEclipseDetails(se[lastE]);
            }
            if (nextE >= 0)
            {
                c.LunarEclipse.NextEclipse = new LunarEclipseDetails(se[nextE]);
            }
        }

        private static string GetMoonName(int month, string name)
        {
            if (name != "") { return "Blue Moon"; }
            switch (month)
            {                 
                case 1:
                        return "Wolf Moon";
                case 2:
                        return "Snow Moon";
                case 3:
                        return "Worm Moon";
                case 4:
                        return "Pink Moon";
                case 5:               
                        return "Flower Moon";              
                case 6:                   
                        return "Strawberry Moon";
                case 7:               
                        return "Buck Moon";
                case 8:
                        return "Sturgeon Moon";
                case 9:
                        return "Corn Moon";
                case 10:
                        return "Hunters Moon";
                case 11:
                        return "Beaver Moon";
                case 12:
                        return "Cold Moon";             
                default:
                    return "";
            }
        }
        public static void GetMoonDistance(DateTime date, Celestial c)
        {
            date = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, DateTimeKind.Utc);
           
            double d = toDays(date);
            
            CelCoords cel = GetMoonCoords(d, c);
            c.MoonDistance = cel.dist;          
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
            double tj = l / dayMs - 0.5 + J1970;
           
            return tj;
            
        }
        static double toDays(DateTime date)
        {
            double d = toJulian(date) - J2000;
  
            return d;
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

        public static void GetMoonSign(DateTime date, Celestial c)
        {
            //Formulas taken from https://www.astrocal.co.uk/moon-sign-calculator/
            double d = date.Day;
            double m = date.Month;
            double y = date.Year;
            double hr = date.Hour;
            double mi = date.Minute;

            double f = hr + (mi / 60);
            double im = 12 * (y + 4800) + m - 3;
            double j = (2 * (im - Math.Floor(im / 12) * 12) + 7 + 365 * im) / 12;
            j = Math.Floor(j) + d + Math.Floor(im / 48) - 32083;
            double jd = j + Math.Floor(im / 4800) - Math.Floor(im / 1200) + 38;
            double T = ((jd - 2415020) + f / 24 - .5) / 36525;
            double ob = FNr(23.452294 - .0130125 * T);
            double ll = 973563 + 1732564379 * T - 4 * T * T;
            double g = 1012395 + 6189 * T;
            double n = 933060 - 6962911 * T + 7.5 * T * T;
            double g1 = 1203586 + 14648523 * T - 37 * T * T;
            d = 1262655 + 1602961611 * T - 5 * T * T;
            double M = 3600;
            double l = (ll - g1) / M;
            double l1 = ((ll - d) - g) / M;
            f = (ll - n) / M;
            d = d / M;
            y = 2 * d;
            double ml = 22639.6 * FNs(l) - 4586.4 * FNs(l - y);
            ml = ml + 2369.9 * FNs(y) + 769 * FNs(2 * l) - 669 * FNs(l1);
            ml = ml - 411.6 * FNs(2 * f) - 212 * FNs(2 * l - y);
            ml = ml - 206 * FNs(l + l1 - y) + 192 * FNs(l + y);
            ml = ml - 165 * FNs(l1 - y) + 148 * FNs(l - l1) - 125 * FNs(d);
            ml = ml - 110 * FNs(l + l1) - 55 * FNs(2 * f - y);
            ml = ml - 45 * FNs(l + 2 * f) + 40 * FNs(l - 2 * f);
            double tn = n + 5392 * FNs(2 * f - y) - 541 * FNs(l1) - 442 * FNs(y);
            tn = tn + 423 * FNs(2 * f) - 291 * FNs(2 * l - 2 * f);
            g = FNu(FNp(ll + ml));
            double sign = Math.Floor(g / 30);
            double degree = (g - (sign * 30));
            sign = sign + 1;

            switch (sign.ToString())
            {
                case "1": c.AstrologicalSigns.MoonSign = "Aries"; break;
                case "2": c.AstrologicalSigns.MoonSign = "Taurus"; break;
                case "3": c.AstrologicalSigns.MoonSign = "Gemini"; break;
                case "4": c.AstrologicalSigns.MoonSign = "Cancer"; break;
                case "5": c.AstrologicalSigns.MoonSign = "Leo"; break;
                case "6": c.AstrologicalSigns.MoonSign = "Virgo"; break;
                case "7": c.AstrologicalSigns.MoonSign = "Libra"; break;
                case "8": c.AstrologicalSigns.MoonSign = "Scorpio"; break;
                case "9": c.AstrologicalSigns.MoonSign = "Sagitarius"; break;
                case "10": c.AstrologicalSigns.MoonSign = "Capricorn"; break;
                case "11": c.AstrologicalSigns.MoonSign = "Aquarius"; break;
                case "12": c.AstrologicalSigns.MoonSign = "Pisces"; break;
                default: c.AstrologicalSigns.MoonSign = "Pisces"; break;
            }

        }
        private static double FNp(double x)
        {
            double sgn;
            if (x < 0)
            { sgn = -1; }
            else
            { sgn = 1; }
            return sgn * ((Math.Abs(x) / 3600) / 360 - Math.Floor((Math.Abs(x) / 3600.0) / 360.0)) * 360;
        }
        private static double FNu(double x)
        { return x - (Math.Floor(x / 360) * 360); }

        private static double FNr(double x)
        { return Math.PI / 180 * x; }

        private static double FNs(double x)
        { return Math.Sin(Math.PI / 180 * x); }


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
       
    }
}
