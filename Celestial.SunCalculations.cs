using System;
using System.Collections.Generic;
namespace CoordinateSharp
{
    internal class SunCalc
    {     
        public static void CalculateSunTime(double lat, double longi, DateTime date, Celestial c)
        {
            if (date.Year == 1900) { return; } //Return if date vaue hasn't been established.
            DateTime actualDate = new DateTime(date.Year,date.Month,date.Day,date.Hour,date.Minute, date.Second, DateTimeKind.Utc);
           
            //Sun Time Calculations
            date = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);      
            double zone = -(int)Math.Round(TimeZone.CurrentTimeZone.GetUtcOffset(date).TotalSeconds / 3600);     
            double jd = GetJulianDay(date) - 2451545;  // Julian day relative to Jan 1.5, 2000

            double lon = longi / 360;
            double tz = zone / 24;
            double ct = jd / 36525 + 1; // centuries since 1900.0
            double t0 = LocalSiderealTimeForTimeZone(lon, jd, tz);      // local sidereal time

            // get sun position at start of day
            jd += tz;
            CalculateSunPosition(jd, ct);
            double ra0 = mSunPositionInSkyArr[0];
            double dec0 = mSunPositionInSkyArr[1];

            // get sun position at end of day
            jd += 1;
            CalculateSunPosition(jd, ct);
            double ra1 = mSunPositionInSkyArr[0];
            double dec1 = mSunPositionInSkyArr[1];

            // make continuous 
            if (ra1 < ra0)
                ra1 += 2 * Math.PI;

            mIsSunrise = false;
            mIsSunset = false;

            mRightAscentionArr[0] = ra0;
            mDecensionArr[0] = dec0;

            // check each hour of this day
            for (int k = 0; k < 25; k++)
            {
                mRightAscentionArr[2] = ra0 + (k + 1) * (ra1 - ra0) / 24;
                mDecensionArr[2] = dec0 + (k + 1) * (dec1 - dec0) / 24;
                mVHzArr[2] = TestHour(k, zone, t0, lat);
                // advance to next hour
                mRightAscentionArr[0] = mRightAscentionArr[2];
                mDecensionArr[0] = mDecensionArr[2];
                mVHzArr[0] = mVHzArr[2];
            }
            //Times returned for 00:00 may create an hour value of 24 which will throw a DateTime Exception.
            //Reset to 0 and keep same day month as this library is designed to return same day event in Z day only.
            if (mRiseTimeArr[0] >= 24) { mRiseTimeArr[0] -= 24; }
            if (mSetTimeArr[0] >= 24) { mSetTimeArr[0] -= 24; }

            c.SunRise = new DateTime(date.Year, date.Month, date.Day, mRiseTimeArr[0], mRiseTimeArr[1], 0);
            c.SunSet = new DateTime(date.Year, date.Month, date.Day, mSetTimeArr[0], mSetTimeArr[1], 0);          
            c.SunCondition = CelestialStatus.RiseAndSet;

            // neither sunrise nor sunset
            if ((!mIsSunrise) && (!mIsSunset))
            {
                if (mVHzArr[2] < 0)
                {
                    c.SunCondition = CelestialStatus.DownAllDay;
                    c.SunRise = null;
                    c.SunSet = null;
                    // Sun down all day
                }
                else
                {
                    c.SunCondition = CelestialStatus.UpAllDay;
                    c.SunRise = null;
                    c.SunSet = null;
                    // Sun up all day
                }
            }
            // sunrise or sunset
            else
            {
                if (!mIsSunrise)
                {
                    // No sunrise this date
                    c.SunCondition = CelestialStatus.NoRise;
                    c.SunRise = null;

                }
                else if (!mIsSunset)
                {
                    // No sunset this date
                    c.SunCondition = CelestialStatus.NoSet;
                    c.SunSet = null;

                }
            }
            
            //Azimuth and Altitude
            CalculateSunAngle(actualDate, longi, lat, c);   
           
           
        }
        public static void CalculateAdditionSolarTimes(DateTime date, double longi, double lat, Celestial c)
        {   
            if (c.SunCondition == CelestialStatus.RiseAndSet && date.Year>1900)
            {          
                getTimes(date, longi, lat, c);
            }
            else
            {
                c.AdditionalSolarTimes = new AdditionalSolarTimes();
            }
            CalculateSolarEclipse(date, lat, longi, c);
        }
        public static void CalculateZodiacSign(DateTime date, Celestial c)
        {
            //Aquarius (January 20 to February 18)
            //Pisces (February 19 to March 20)
            //Aries (March 21-April 19)
            //Taurus (April 20-May 20)
            //Gemini (May 21-June 20)
            //Cancer (June 21-July 22)
            //Leo (July 23-August 22)
            //Virgo (August 23-September 22)
            //Libra (September 23-October 22)
            //Scorpio (October 23-November 21)
            //Sagittarius (November 22-December 21)
            //Capricorn (December 22-January 19)
           
            if (date >= new DateTime(date.Year, 1, 1) && date <= new DateTime(date.Year, 1, 19))
            {
                c.AstrologicalSigns.ZodiacSign = "Capricorn";
                return;
            }
            if (date >= new DateTime(date.Year, 1, 20) && date <= new DateTime(date.Year, 2, 18))
            {
                c.AstrologicalSigns.ZodiacSign = "Aquarius";
                return;
            }
            if (date >= new DateTime(date.Year, 2, 19) && date <= new DateTime(date.Year, 3, 20))
            {
                c.AstrologicalSigns.ZodiacSign = "Pisces";
                return;
            }
            if (date >= new DateTime(date.Year, 3, 21) && date <= new DateTime(date.Year, 4, 19))
            {
                c.AstrologicalSigns.ZodiacSign = "Aries";
                return;
            }
            if (date >= new DateTime(date.Year, 4, 20) && date <= new DateTime(date.Year, 5, 20))
            {
                c.AstrologicalSigns.ZodiacSign = "Taurus";
                return;
            }
            if (date >= new DateTime(date.Year, 5, 21) && date <= new DateTime(date.Year, 6, 20))
            {
                c.AstrologicalSigns.ZodiacSign = "Gemini";
                return;
            }
            if (date >= new DateTime(date.Year, 6, 21) && date <= new DateTime(date.Year, 6, 22))
            {
                c.AstrologicalSigns.ZodiacSign = "Cancer";
                return;
            }
            if (date >= new DateTime(date.Year, 7, 23) && date <= new DateTime(date.Year, 8, 22))
            {
                c.AstrologicalSigns.ZodiacSign = "Leo";
                return;
            }
            if (date >= new DateTime(date.Year, 8, 23) && date <= new DateTime(date.Year, 9, 22))
            {
                c.AstrologicalSigns.ZodiacSign = "Virgo";
                return;
            }
            if (date >= new DateTime(date.Year, 9, 23) && date <= new DateTime(date.Year, 10, 22))
            {
                c.AstrologicalSigns.ZodiacSign = "Libra";
                return;
            }
            if (date >= new DateTime(date.Year, 9, 23) && date <= new DateTime(date.Year, 11, 21))
            {
                c.AstrologicalSigns.ZodiacSign = "Scorpio";
                return;
            }
            if (date >= new DateTime(date.Year, 11, 21) && date <= new DateTime(date.Year, 12, 21))
            {
                c.AstrologicalSigns.ZodiacSign = "Sagittarius";
                return;
            }
            if (date >= new DateTime(date.Year, 12, 22) && date <= new DateTime(date.Year, 12, 31))
            {
                c.AstrologicalSigns.ZodiacSign = "Capricorn";
                return;
            }
        }
        public static void CalculateSolarEclipse(DateTime date, double lat, double longi, Celestial c)
        {
            //Convert to Radian
            double latR = lat * Math.PI / 180;
            double longR = longi * Math.PI / 180;
            List<List<string>> se = SolarEclipseCalc.CalculateSolarEclipse(date, latR, longR);
            //RETURN FIRST AND LAST
            if (se.Count == 0) { return; }
            //FIND LAST AND NEXT ECLIPSE
            int lastE = -1;
            int nextE = -1;
            int currentE = 0;
            DateTime lastDate = new DateTime();
            DateTime nextDate = new DateTime(3300, 1, 1);
            //Iterate to get last and next eclipse
            foreach(List<string> values in se)
            {               
                DateTime ld = Convert.ToDateTime(values[0], System.Globalization.CultureInfo.InvariantCulture);
                if (ld < date && ld>lastDate) { lastDate = ld;lastE = currentE; }
                if(ld>= date && ld < nextDate) { nextDate = ld;nextE = currentE; }
                currentE++;
            }
            //SET ECLIPSE DATA
            if (lastE >= 0)
            {
                c.SolarEclipse.LastEclipse = new SolarEclipseDetails(se[lastE]);
            }
            if (nextE >= 0)
            {
                c.SolarEclipse.NextEclipse = new SolarEclipseDetails(se[nextE]);
            }
        }

        #region Private Suntime Members

        private static double rad = Math.PI / 180;

        private static double j2000 = 2451545; //Julian from year 2000
        private static double j1970 = 2440588; //Julian from year 1970
        private static double dayMS = 1000 * 60 * 60 * 24; //Day in milliseconds

        private const double mDR = Math.PI / 180;
        private const double mK1 = 15 * mDR * 1.0027379;

        private static int[] mRiseTimeArr = new int[2] { 0, 0 };
        private static int[] mSetTimeArr = new int[2] { 0, 0 };
        private static double mRizeAzimuth = 0.0;
        private static double mSetAzimuth = 0.0;

        private static double[] mSunPositionInSkyArr = new double[2] { 0.0, 0.0 };
        private static double[] mRightAscentionArr = new double[3] { 0.0, 0.0, 0.0 };
        private static double[] mDecensionArr = new double[3] { 0.0, 0.0, 0.0 };
        private static double[] mVHzArr = new double[3] { 0.0, 0.0, 0.0 };

        private static bool mIsSunrise = false;
        private static bool mIsSunset = false;

        #endregion
        #region Private Suntime Functions

        private static double GetJulianDay(DateTime date)
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
        private static DateTime? fromJulian(double j)
        {
            if (Double.IsNaN(j)) { return null; } //No Event Occured

            double unixTime = (j + 0.5 - j1970) * 86400;

            System.DateTime dtDateTime = new DateTime(1970, 1, 1);
            dtDateTime = dtDateTime.AddSeconds(unixTime);

            return dtDateTime;

        }

        private static double LocalSiderealTimeForTimeZone(double lon, double jd, double z)
        {
            double s = 24110.5 + 8640184.812999999 * jd / 36525 + 86636.6 * z + 86400 * lon;
            s = s / 86400;
            s = s - Math.Truncate(s);
            double lst = s * 360 * mDR;
           
            return lst;
        }
        private static double SideRealTime(double d, double lw)
        {
            return rad * (280.16 + 360.9856235 * d) - lw;
        }
        private static double solarTransitJ(double ds, double M, double L)  
        {         
            return j2000 + ds + 0.0053 * Math.Sin(M) - 0.0069 * Math.Sin(2 * L); 
        }

        private static double hourAngle(double h, double phi, double d) 
        {
            //NUMBER RETURNING > and < 1 NaN;
            double d1 = Math.Sin(h) - Math.Sin(phi) * Math.Sin(d);
            double d2 = Math.Cos(phi) * Math.Cos(d);
            double d3 = (d1 / d2);
            
            return Math.Acos(d3); 
        }
        private static double approxTransit(double Ht, double lw, double n)
        {
            return 0.0009 + (Ht + lw) / (2 * Math.PI) + n;
        }
        private static double julianCycle(double d, double lw) { return Math.Round(d - 0.0009 - lw / (2 * Math.PI)); }

        //Returns Time of specified event based on suns angle
        private static double GetTime(double h, double lw, double phi, double dec, double n,double M, double L) 
        {
            double w = hourAngle(h, phi, dec);      
            double a = approxTransit(w, lw, n);      
            return solarTransitJ(a, M, L);
        }
        private static double declination(double l, double b)    
        {
            double e = (Math.PI/180) * 23.4397; // obliquity of the Earth
            
            return Math.Asin(Math.Sin(b) * Math.Cos(e) + Math.Cos(b) * Math.Sin(e) * Math.Sin(l)); 
        }
        /// <summary>
        /// Gets times for additional solar times
        /// </summary>
        private static void getTimes(DateTime date, double lng, double lat, Celestial c)
        {
            //Get Julian
            double d = GetJulianDay(date) - j2000 + .5; //LESS PRECISE JULIAN NEEDED
            
            double lw = rad * -lng;
            double phi = rad * lat;

            double n = julianCycle(d, lw);
            double ds = approxTransit(0, lw, n);

            double M = solarMeanAnomaly(ds);

            double L = eclipticLongitude(M);

            double dec = declination(L, 0);

            double Jnoon = solarTransitJ(ds, M, L);

            double Jset;
            double Jrise;

            DateTime? solarNoon = fromJulian(Jnoon);
            DateTime? nadir = fromJulian(Jnoon - 0.5);

            c.AdditionalSolarTimes = new AdditionalSolarTimes();

            //Dusk and Dawn
            Jset = GetTime(-6 * rad, lw, phi, dec, n, M, L); 
            Jrise = Jnoon - (Jset - Jnoon);
           
            c.AdditionalSolarTimes.CivilDawn = fromJulian(Jrise);
            c.AdditionalSolarTimes.CivilDusk = fromJulian(Jset);

            Jset = GetTime(-12 * rad, lw, phi, dec, n, M, L);        
            Jrise = Jnoon - (Jset - Jnoon);
        
            c.AdditionalSolarTimes.NauticalDawn = fromJulian(Jrise);
            c.AdditionalSolarTimes.NauticalDusk = fromJulian(Jset);          
        }
       
        private static void CalculateSunPosition(double jd, double ct)
        {
            double g, lo, s, u, v, w;

            lo = 0.779072 + 0.00273790931 * jd;
            lo = lo - Math.Truncate(lo);
            lo = lo * 2 * Math.PI;

            g = 0.993126 + 0.0027377785 * jd;
            g = g - Math.Truncate(g);
            g = g * 2 * Math.PI;

            v = 0.39785 * Math.Sin(lo);
            v = v - 0.01 * Math.Sin(lo - g);
            v = v + 0.00333 * Math.Sin(lo + g);
            v = v - 0.00021 * ct * Math.Sin(lo);

            u = 1 - 0.03349 * Math.Cos(g);
            u = u - 0.00014 * Math.Cos(2 * lo);
            u = u + 0.00008 * Math.Cos(lo);

            w = -0.0001 - 0.04129 * Math.Sin(2 * lo);
            w = w + 0.03211 * Math.Sin(g);
            w = w + 0.00104 * Math.Sin(2 * lo - g);
            w = w - 0.00035 * Math.Sin(2 * lo + g);
            w = w - 0.00008 * ct * Math.Sin(g);

            // compute sun's right ascension
            s = w / Math.Sqrt(u - v * v);
            mSunPositionInSkyArr[0] = lo + Math.Atan(s / Math.Sqrt(1 - s * s));

            // ...and declination 
            s = v / Math.Sqrt(u);
            mSunPositionInSkyArr[1] = Math.Atan(s / Math.Sqrt(1 - s * s));
           
        }
        private static void CalculateSunAngle(DateTime date, double longi, double lat, Celestial c)
        {
           
            //C# version of JavaScript date.valueOf();
            TimeSpan ts = date - new DateTime(1970, 1, 1,0,0,0, DateTimeKind.Utc);
            double dms = (ts.TotalMilliseconds / dayMS -.5 + j1970)-j2000;
           

            double lw = rad * -longi;
            double phi = rad * lat;
            double e = rad * 23.4397;
         
            double[] sc = sunCoords(dms);
          
            double H = SideRealTime(dms, lw) - sc[1];

            c.SunAzimuth = Math.Atan2(Math.Sin(H), Math.Cos(H) * Math.Sin(phi) - Math.Tan(sc[0]) * Math.Cos(phi)) * 180 / Math.PI + 180;
            c.SunAltitude = Math.Asin(Math.Sin(phi) * Math.Sin(sc[0]) + Math.Cos(phi) * Math.Cos(sc[0]) * Math.Cos(H)) * 180 / Math.PI;
           
        }
        private static double solarMeanAnomaly(double d) 
        {          
            return rad * (357.5291 + 0.98560028 * d); 
        }
        private static double eclipticLongitude(double m)
        {         
            double c = rad * (1.9148 * Math.Sin(m) + 0.02 * Math.Sin(2 * m) + 0.0003 * Math.Sin(3 * m)); // equation of center
            double p = rad * 102.9372; // perihelion of the Earth
            
            return m + c + p + Math.PI;
        }
        private static double[] sunCoords(double d)
        {

            double m = solarMeanAnomaly(d);
            double l = eclipticLongitude(m);
            double[] sc = new double[2];
            double b = 0;
            double e = rad * 23.4397; // obliquity of the Earth
            sc[0] = Math.Asin(Math.Sin(b) * Math.Cos(e) + Math.Cos(b) * Math.Sin(e) * Math.Sin(l)); //declination
            sc[1] = Math.Atan2(Math.Sin(l) * Math.Cos(e) - Math.Tan(b) * Math.Sin(e), Math.Cos(l)); //rightAscension     
            return sc;
        }
        private static double TestHour(int k, double zone, double t0, double lat)
        {
            double[] ha = new double[3];
            double a, b, c, d, e, s, z;
            double time;
            int hr, min;
            double az, dz, hz, nz;

            ha[0] = t0 - mRightAscentionArr[0] + k * mK1;
            ha[2] = t0 - mRightAscentionArr[2] + k * mK1 + mK1;

            ha[1] = (ha[2] + ha[0]) / 2;    // hour angle at half hour
            mDecensionArr[1] = (mDecensionArr[2] + mDecensionArr[0]) / 2;  // declination at half hour

            s = Math.Sin(lat * mDR);
            c = Math.Cos(lat * mDR);
            z = Math.Cos(90.833 * mDR);    // refraction + sun semidiameter at horizon

            if (k <= 0)
                mVHzArr[0] = s * Math.Sin(mDecensionArr[0]) + c * Math.Cos(mDecensionArr[0]) * Math.Cos(ha[0]) - z;

            mVHzArr[2] = s * Math.Sin(mDecensionArr[2]) + c * Math.Cos(mDecensionArr[2]) * Math.Cos(ha[2]) - z;

            if (Sign(mVHzArr[0]) == Sign(mVHzArr[2]))
                return mVHzArr[2];  // no event this hour

            mVHzArr[1] = s * Math.Sin(mDecensionArr[1]) + c * Math.Cos(mDecensionArr[1]) * Math.Cos(ha[1]) - z;

            a = 2 * mVHzArr[0] - 4 * mVHzArr[1] + 2 * mVHzArr[2];
            b = -3 * mVHzArr[0] + 4 * mVHzArr[1] - mVHzArr[2];
            d = b * b - 4 * a * mVHzArr[0];

            if (d < 0)
                return mVHzArr[2];  // no event this hour

            d = Math.Sqrt(d);
            e = (-b + d) / (2 * a);

            if ((e > 1) || (e < 0))
                e = (-b - d) / (2 * a);

            time = (double)k + e + (double)1 / (double)120; // time of an event

            hr = (int)Math.Floor(time);
            min = (int)Math.Floor((time - hr) * 60);
           
            hz = ha[0] + e * (ha[2] - ha[0]); // azimuth of the sun at the event
            nz = -Math.Cos(mDecensionArr[1]) * Math.Sin(hz);
            dz = c * Math.Sin(mDecensionArr[1]) - s * Math.Cos(mDecensionArr[1]) * Math.Cos(hz);
            az = Math.Atan2(nz, dz) / mDR;
            if (az < 0) az = az + 360;
          
            if ((mVHzArr[0] < 0) && (mVHzArr[2] > 0))
            {
                mRiseTimeArr[0] = hr;
                mRiseTimeArr[1] = min;
                mRizeAzimuth = az;
                mIsSunrise = true;
                
            }

            if ((mVHzArr[0] > 0) && (mVHzArr[2] < 0))
            {
                mSetTimeArr[0] = hr;
                mSetTimeArr[1] = min;
                mSetAzimuth = az;
                mIsSunset = true;
            }

            return mVHzArr[2];
        }
        private static int Sign(double value)
        {
            int rv = 0;

            if (value > 0.0) rv = 1;
            else if (value < 0.0) rv = -1;
            else rv = 0;

            return rv;
        }
        #endregion
    }
}
