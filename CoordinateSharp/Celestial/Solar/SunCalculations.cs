/*
CoordinateSharp is a .NET standard library that is intended to ease geographic coordinate 
format conversions and location based celestial calculations.
https://github.com/Tronald/CoordinateSharp

Many celestial formulas in this library are based on Jean Meeus's 
Astronomical Algorithms (2nd Edition). Comments that reference only a chapter
are referring to this work.

License

CoordinateSharp is split licensed and may be licensed under the GNU Affero General Public License version 3 or a commercial use license as stated.

Copyright (C) 2023, Signature Group, LLC
  
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

Organizations or use cases that fall under the following conditions may receive a free commercial use license upon request on a case by case basis.


	-Open source contributors to this library.
	-Scholarly or scientific research.
	-Emergency response / management uses.

Please visit http://coordinatesharp.com/licensing or contact Signature Group, LLC to purchase a commercial license, or for any questions regarding the AGPL 3.0 license requirements or free use license: sales@signatgroup.com.
*/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using CoordinateSharp.Formatters;
namespace CoordinateSharp
{
    internal class SunCalc
    {     
        public static void CalculateSunTime(double lat, double longi, DateTime date, Celestial c, EagerLoad el, double offset)
        {
           
            if (date.Year == 0001) { return; } //Return if date value hasn't been established.
            if (el.Extensions.Solar_Cycle)
            {
                DateTime actualDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);
              
                ////Sun Time Calculations
                //Get solar coordinate info and feed
                //Get Julian     
                double lw = rad * -longi;
                double phi = rad * lat;

                //Rise Set        
                DateTime?[] evDate = Get_Event_Time(lw, phi, -.8333, actualDate, offset,true); //ADDED OFFSET TO ALL Get_Event_Time calls.
               
                c.sunRise = evDate[0];
                c.sunSet = evDate[1];
                c.solarNoon = evDate[2];
               
                c.sunCondition = CelestialStatus.RiseAndSet;

                //Get Solar Coordinate
                var celC = Get_Solar_Coordinates(date, -offset);
                c.solarCoordinates = celC;
                //Azimuth and Altitude
                CalculateSunAngle(date.AddHours(-offset), longi, lat, c, celC); //SUBTRACT OFFSET TO CALC IN Z TIME AND ADJUST SUN ANGLE DURING LOCAL CALCULATIONS.
                // neither sunrise nor sunset
                if ((!c.SunRise.HasValue) && (!c.SunSet.HasValue))
                {
                    if (c.SunAltitude < 0)
                    {
                        c.sunCondition = CelestialStatus.DownAllDay;
                    }
                    else
                    {
                        c.sunCondition = CelestialStatus.UpAllDay;
                    }
                }
                // sunrise or sunset
                else
                {
                    if (!c.SunRise.HasValue)
                    {
                        // No sunrise this date
                        c.sunCondition = CelestialStatus.NoRise;

                    }
                    else if (!c.SunSet.HasValue)
                    {
                        // No sunset this date
                        c.sunCondition = CelestialStatus.NoSet;
                    }
                }
                //Additional Times
                c.additionalSolarTimes = new AdditionalSolarTimes();
                //Dusk and Dawn
                //Civil
                evDate = Get_Event_Time(lw, phi, -6, actualDate, offset,false);
                c.AdditionalSolarTimes.civilDawn = evDate[0];
                c.AdditionalSolarTimes.civilDusk = evDate[1];


                //Nautical
                evDate = Get_Event_Time(lw, phi, -12, actualDate, offset, false);
                c.AdditionalSolarTimes.nauticalDawn = evDate[0];
                c.AdditionalSolarTimes.nauticalDusk = evDate[1];

                //Astronomical
                evDate = Get_Event_Time(lw, phi, -18, actualDate, offset, false);

                c.AdditionalSolarTimes.astronomicalDawn = evDate[0];
                c.AdditionalSolarTimes.astronomicalDusk = evDate[1];

                //BottomDisc
                evDate = Get_Event_Time(lw, phi, -.2998, actualDate, offset, false);
                c.AdditionalSolarTimes.sunriseBottomDisc = evDate[0];
                c.AdditionalSolarTimes.sunsetBottomDisc = evDate[1];             
            }
            if (el.Extensions.Solstice_Equinox){ Calculate_Soltices_Equinoxes(date, c, offset); }         
            if (el.Extensions.Solar_Eclipse) { CalculateSolarEclipse(date, lat, longi, c); }
        }
        /// <summary>
        /// Gets time of event based on specified degree below specified altitude
        /// </summary>
        /// <param name="lw">Observer Longitude in radians</param>
        /// <param name="phi">Observer Latitude in radians</param>
        /// <param name="h">Angle in Degrees</param>
        /// <param name="date">Date of Event</param>
        /// <param name="offset">Offset hours</param>
        /// <param name="calculateNoon">Should solar noon iterate and return value</param>
        /// <returns>DateTime?[]{rise, set}</returns> 
        internal static DateTime?[] Get_Event_Time(double lw, double phi, double h, DateTime date, double offset, bool calculateNoon)
        {
            double julianOffset = offset * .04166667;
           
            //Create arrays. Index 0 = Day -1, 1 = Day, 2 = Day + 1;
            //These will be used to find exact day event occurs for comparison
            DateTime?[] sets = new DateTime?[] { null, null, null, null, null };
            DateTime?[] rises = new DateTime?[] { null, null, null,null, null };
            DateTime?[] solarNoons = new DateTime?[] { null, null, null, null, null };

            //Iterate starting with day -1;
            for (int x = 0; x < 5; x++)
            {
                double d = JulianConversions.GetJulian(date.AddDays(x-2)) - j2000 + .5; //LESS PRECISE JULIAN NEEDED
                double n = julianCycle(d, lw);

                //var celC = Get_Solar_Coordinates(date); //Change and Test locs!

                double ds = approxTransit(0, lw, n);

                //M = celC.MeanAnomaly.ToRadians();

                double M = solarMeanAnomaly(ds);

               

                double L = eclipticLongitude(M);

                double dec = declination(L, 0);

                double Jnoon = solarTransitJ(ds, M, L);

             
                //Rise Set
                double Jset = GetTime(h * rad, lw, phi, dec, n, M, L);
                double Jrise = Jnoon - (Jset - Jnoon);
                   
                DateTime? rise = JulianConversions.GetDate_FromJulian(Jrise + julianOffset); //Adjusting julian for DT OFFSET MAY HELP WITH LOCAL TIME
                DateTime? set = JulianConversions.GetDate_FromJulian(Jset + julianOffset); //Adjusting julian for DT OFFSET MAY HELP WITH LOCAL TIME
               
                rises[x] = rise;
                sets[x] = set;

                if (calculateNoon)
                {
                    solarNoons[x] = JulianConversions.GetDate_FromJulian(Jnoon + julianOffset);
                }
            }

            //Compare and send           
            DateTime? tRise = Get_Event_Target_Date(rises, date);
            DateTime? tSet = Get_Event_Target_Date(sets, date);

            DateTime? tNoon = null;
            if (calculateNoon){tNoon = Get_Event_Target_Date(solarNoons, date); }

            return new DateTime?[] { tRise, tSet, tNoon};
        }

        /// <summary>
        /// Iterates stored events and extracts the one that occurs on the target date.
        /// </summary>
        /// <param name="dates">Date Array</param>
        /// <param name="targetDate">Target</param>
        /// <returns>DateTime?</returns>
        private static DateTime? Get_Event_Target_Date(DateTime?[] dates, DateTime targetDate)
        {
            DateTime? target = null;
            for (int x = 0; x < 5; x++)
            {
                if (dates[x].HasValue)
                {
                    if (dates[x].Value.Day == targetDate.Day)
                    {
                        target = dates[x];
                        break;
                    }
                }
            }
            return target;
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
            if (date >= new DateTime(date.Year, 1, 1) && date <= new DateTime(date.Year, 1, 19, 23, 59, 59))
            {
                c.AstrologicalSigns.ezodiacSign = AstrologicalSignType.Capricorn;
                return;
            }
            if (date >= new DateTime(date.Year, 1, 20) && date <= new DateTime(date.Year, 2, 18, 23, 59, 59))
            {
                c.AstrologicalSigns.ezodiacSign = AstrologicalSignType.Aquarius;
                return;
            }
            if (date >= new DateTime(date.Year, 2, 19) && date <= new DateTime(date.Year, 3, 20, 23, 59, 59))
            {
                c.AstrologicalSigns.ezodiacSign = AstrologicalSignType.Pisces;
                return;
            }
            if (date >= new DateTime(date.Year, 3, 21) && date <= new DateTime(date.Year, 4, 19, 23, 59, 59))
            {
                c.AstrologicalSigns.ezodiacSign = AstrologicalSignType.Aries;
            }
            if (date >= new DateTime(date.Year, 4, 20) && date <= new DateTime(date.Year, 5, 20, 23, 59, 59))
            {
                c.AstrologicalSigns.ezodiacSign = AstrologicalSignType.Taurus;
                return;
            }
            if (date >= new DateTime(date.Year, 5, 21) && date <= new DateTime(date.Year, 6, 20,23,59,59))
            {
                c.AstrologicalSigns.ezodiacSign = AstrologicalSignType.Gemini;
                return;
            }
            if (date >= new DateTime(date.Year, 6, 21) && date <= new DateTime(date.Year, 7, 22, 23, 59, 59))
            {
                c.AstrologicalSigns.ezodiacSign = AstrologicalSignType.Cancer;
                return;
            }
            if (date >= new DateTime(date.Year, 7, 23) && date <= new DateTime(date.Year, 8, 22, 23, 59, 59))
            {
                c.AstrologicalSigns.ezodiacSign = AstrologicalSignType.Leo;
                return;
            }
            if (date >= new DateTime(date.Year, 8, 23) && date <= new DateTime(date.Year, 9, 22, 23, 59, 59))
            {
                c.AstrologicalSigns.ezodiacSign = AstrologicalSignType.Virgo;
                return;
            }
            if (date >= new DateTime(date.Year, 9, 23) && date <= new DateTime(date.Year, 10, 22, 23, 59, 59))
            {
                c.AstrologicalSigns.ezodiacSign = AstrologicalSignType.Libra;
                return;
            }
            if (date >= new DateTime(date.Year, 9, 23) && date <= new DateTime(date.Year, 11, 21, 23, 59, 59))
            {
                c.AstrologicalSigns.ezodiacSign = AstrologicalSignType.Scorpio;
                return;
            }
            if (date >= new DateTime(date.Year, 11, 21) && date <= new DateTime(date.Year, 12, 21, 23, 59, 59))
            {
                c.AstrologicalSigns.ezodiacSign = AstrologicalSignType.Sagittarius;
                return;
            }
            if (date >= new DateTime(date.Year, 12, 22) && date <= new DateTime(date.Year, 12, 31, 23, 59, 59))
            {
                c.AstrologicalSigns.ezodiacSign = AstrologicalSignType.Capricorn;
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
                DateTime ld = DateTime.ParseExact(values[0], "yyyy-MMM-dd", System.Globalization.CultureInfo.InvariantCulture);
             
                if (ld < date && ld>lastDate) { lastDate = ld;lastE = currentE; }
                if(ld>= date && ld < nextDate) { nextDate = ld;nextE = currentE; }
                currentE++;
            }
            //SET ECLIPSE DATA
            if (lastE >= 0)
            {
                c.SolarEclipse.lastEclipse = new SolarEclipseDetails(se[lastE]);
            }
            if (nextE >= 0)
            {
                c.SolarEclipse.nextEclipse = new SolarEclipseDetails(se[nextE]);
            }
        }

        public static void Calculate_Soltices_Equinoxes(DateTime d, Celestial c, double offset)
        {
            double springEquinoxJDE;
            double fallEquinoxJDE;
            double summerSolsticeJDE;
            double winterSolsticeJDE;
            //Table 27A
            if (d.Year <= 1000)
            {
                double Y = d.Year / 1000.0; //Determine if actual int
                springEquinoxJDE = 1721139.29189 + 365242.13740 * Y + 0.06134 * Math.Pow(Y, 2) + 0.00111 * Math.Pow(Y, 3) - 0.00071 * Math.Pow(Y, 4);
                fallEquinoxJDE = 1721325.70455 + 365242.49558 * Y + 0.11677 * Math.Pow(Y, 2) + 0.00297 * Math.Pow(Y, 3) - 0.00074 * Math.Pow(Y, 4);
                summerSolsticeJDE = 1721233.25401 + 365241.72562 * Y + 0.05323 * Math.Pow(Y, 2) + 0.00907 * Math.Pow(Y, 3) - 0.00025 * Math.Pow(Y, 4);
                winterSolsticeJDE = 1721414.39987 + 365242.88257 * Y + 0.00769 * Math.Pow(Y, 2) + 0.00933 * Math.Pow(Y, 3) - 0.00006 * Math.Pow(Y, 4);
            }
            //Table 27B
            else
            {
                double Y = (d.Year - 2000) / 1000.0; //Determine if actual in
                springEquinoxJDE = 2451623.80984 + 365242.37404* Y + 0.05169 * Math.Pow(Y, 2) + 0.00411 * Math.Pow(Y, 3) - 0.00057 * Math.Pow(Y, 4);
                fallEquinoxJDE = 2451810.21715 + 365242.01767 * Y + 0.11575 * Math.Pow(Y, 2) + 0.00337 * Math.Pow(Y, 3) - 0.00078 * Math.Pow(Y, 4);
                summerSolsticeJDE = 2451716.56767 + 365241.62603* Y + 0.00325 * Math.Pow(Y, 2) + 0.00888 * Math.Pow(Y, 3) - 0.00030 * Math.Pow(Y, 4);
                winterSolsticeJDE = 2451900.05952+ 365242.74049 * Y + 0.06233 * Math.Pow(Y, 2) + 0.00823 * Math.Pow(Y, 3) - 0.00032 * Math.Pow(Y, 4);
            }

            c.Solstices.Summer = Get_Soltice_Equinox_From_JDE0(summerSolsticeJDE, offset);
            c.Solstices.Winter = Get_Soltice_Equinox_From_JDE0(winterSolsticeJDE, offset);
            c.Equinoxes.Spring = Get_Soltice_Equinox_From_JDE0(springEquinoxJDE, offset);
            c.Equinoxes.Fall = Get_Soltice_Equinox_From_JDE0(fallEquinoxJDE, offset);
        }
        private static DateTime Get_Soltice_Equinox_From_JDE0(double JDE0, double offset)
        {
            //Get Event Ch 27.
            double T = (JDE0 - 2451545.0) / 36525;
            double W = (35999.373 * Math.PI / 180) * T - (2.47 * Math.PI / 180);
            double ang = 1 + .0334 * Math.Cos(W) + .0007 * Math.Cos(2 * W);
            double sum = MeeusTables.Equinox_Solstice_Sum_of_S(T);
            double JDE = JDE0 + ((.00001) * sum / ang);
            DateTime? d = JulianConversions.GetDate_FromJulian(JDE);
            if (d.HasValue)
            {
                return JulianConversions.GetDate_FromJulian(JDE).Value.AddHours(offset);
            }
            return new DateTime(); //Julian limit exceeded, return empty DateTime
        }

        #region Private Suntime Members
        private static readonly double dayMS = 1000 * 60 * 60 * 24, j1970 = 2440588, j2000 = 2451545;
        private static readonly double rad = Math.PI / 180;     

        private static double LocalSiderealTimeForTimeZone(double lon, double jd, double z)
        {
            double s = 24110.5 + 8640184.812999999 * jd / 36525 + 86636.6 * z + 86400 * lon;
            s = s / 86400;
            s = s - Math.Truncate(s);
            double lst = s * 360 *rad;
           
            return lst;
        }
        private static double SideRealTime(double d, double lw)
        {
            double s = rad * (280.16 + 360.9856235 * d) - lw;
            return s;
        }
        private static double solarTransitJ(double ds, double M, double L)  
        {         
            return j2000 + ds + 0.0053 * Math.Sin(M) - 0.0069 * Math.Sin(2 * L); 
        }

        public static SolarCoordinates Get_Solar_Coordinates(DateTime d, double offset)
        {
            double julianOffset = offset * .04166667;
            double JD = JulianConversions.GetJulian(d) + julianOffset;
            double T = (JD - 2451545.0) / 36525; //25.1 Time
            double L0 = 280.46646 + 36000.76983 * T + .0003032 * Math.Pow(T, 2); //25.2 Geometric Mean Longitude
            double M = 357.52911 + 35999.05029 * T - .0001537 * Math.Pow(T, 2);//25.3 Mean Anomaly
            double e = .016708634 - .000042037 * T - .0000001267 * Math.Pow(T, 2); //25.4 Eccentricity
            double C = +(1.914602 - .004817 * T - .000014 * Math.Pow(T, 2)) * Math.Sin(M.ToRadians()) +
                (.019993 - .000101 * T) * Math.Sin(2 * M.ToRadians()) +
                .000289 * Math.Sin(3 * M.ToRadians()); //25.4 Equation of the center

            double trueLongitude = L0 + C; //25.4
            double trueAnomaly = M + C; //25.4 "v" 

            double R = (1.000001018 * (1 - Math.Pow(e, 2))) / (1 + e * Math.Cos(trueAnomaly.ToRadians())); //25.5 Radius Vector

            double ascendingNode = 125.04 - 1934.136 * T;
            double apparentLongitude = trueLongitude - .00569 - .00478 * Math.Sin(ascendingNode.ToRadians());

            double E = Format.ToDegrees(23, 26, 21.488) - (46.8150 / 3600) * T - (.00059 / 3600) * Math.Pow(T, 2) + (.001813 / 3600) * Math.Pow(T, 3);//22.2 Obliquity of the ecliptic

            double tra = Math.Atan2(Math.Cos(E.ToRadians()) * Math.Sin(trueLongitude.ToRadians()), Math.Cos(trueLongitude.ToRadians())); //25.6 True Right Ascensions. Using Atan2 we can move tan to the right side of the function with Numerator, Denominator
          
            double tdec = Math.Asin(Math.Sin(E.ToRadians()) * Math.Sin(trueLongitude.ToRadians())); //25.7 True declination. Asin used in liu of sin.

            double CE = E + .00256 * Math.Cos(ascendingNode.ToRadians());//25.6 & 25.7 Apparent position of the sun.

            double ara = Math.Atan2(Math.Cos(CE.ToRadians()) * Math.Sin(apparentLongitude.ToRadians()), Math.Cos(apparentLongitude.ToRadians())); //25.8 Apparent Right Ascensions. Using Atan2 we can move tan to the right side of the function with Numerator, Denominator           
            double adec = Math.Asin(Math.Sin(CE.ToRadians()) * Math.Sin(apparentLongitude.ToRadians())); //25.8 Apparent declination. Asin used in liu of sin.

            SolarCoordinates celC = new SolarCoordinates();

            //Set to degrees
            //celC.trueRightAscension = tra.ToDegrees();
            //celC.trueDeclination = tdec.ToDegrees();
            var tr = tra.ToDegrees().NormalizeDegrees360();
            celC.rightAscension = ara.ToDegrees().NormalizeDegrees360();
            celC.declination = adec.ToDegrees();
            celC.julianDayDecimal = JD-.5 - Math.Floor(JD-.5);
            celC.trueLongitude = trueLongitude.NormalizeDegrees360();
            celC.longitude = apparentLongitude.NormalizeDegrees360();
            celC.radiusVector = R;
            celC.geometricMeanLongitude = L0.NormalizeDegrees360();
            celC.obliquityOfEcliptic = E.NormalizeDegrees360();
            //Latitude is always 0 for sun as perturbations not accounted for in low accuracy formulas
            celC.latitude = 0;
            celC.trueLatitude = 0;

            return celC;
        }


        //CH15 
        //Formula 15.1
        //Returns Approximate Time
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
            double approxTime = hourAngle(h, phi, dec);    //Ch15 Formula 15.1  

            double a = approxTransit(approxTime, lw, n);
            double st = solarTransitJ(a, M, L);
          
            return st;            
        }
        private static double declination(double l, double b)    
        {
            double e = (Math.PI/180) * 23.4392911; // obliquity of the Earth
            
            return Math.Asin(Math.Sin(b) * Math.Cos(e) + Math.Cos(b) * Math.Sin(e) * Math.Sin(l)); 
        }
    
      
        private static void CalculateSunAngle(DateTime date, double longi, double lat, Celestial c, SolarCoordinates solC)
        {
            double[] ang = CalculateSunAngle(date, longi, lat, solC);

            c.sunAzimuth = ang[0];
            c.sunAltitude = ang[1];           
        }
        public static double[] CalculateSunAngle(DateTime date, double longi, double lat, SolarCoordinates solC)
        {
            TimeSpan ts = date - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            double dms = (ts.TotalMilliseconds / dayMS - .5 + j1970) - j2000;

            double lw = rad * -longi;
            double phi = rad * lat;
            double e = rad * 23.4397;        

            double H = SideRealTime(dms, lw) - solC.rightAscension.ToRadians();

            double azimuth = Math.Atan2(Math.Sin(H), Math.Cos(H) * Math.Sin(phi) - Math.Tan(solC.declination.ToRadians()) * Math.Cos(phi)) * 180 / Math.PI + 180;
            double altitude = Math.Asin(Math.Sin(phi) * Math.Sin(solC.declination.ToRadians()) + Math.Cos(phi) * Math.Cos(solC.declination.ToRadians()) * Math.Cos(H)) * 180 / Math.PI;

            return new double[] { azimuth, altitude };
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
       
        //Legacy Coord method
        //private static double[] sunCoords(double d)
        //{

        //    double m = solarMeanAnomaly(d);
        //    double l = eclipticLongitude(m);
        //    double[] sc = new double[2];
        //    double b = 0;
        //    double e = rad * 23.4397; // obliquity of the Earth
        //    sc[0] = Math.Asin(Math.Sin(b) * Math.Cos(e) + Math.Cos(b) * Math.Sin(e) * Math.Sin(l)); //declination
        //    sc[1] = Math.Atan2(Math.Sin(l) * Math.Cos(e) - Math.Tan(b) * Math.Sin(e), Math.Cos(l)); //rightAscension     
        //    return sc;
        //}
        #endregion

    }
}
