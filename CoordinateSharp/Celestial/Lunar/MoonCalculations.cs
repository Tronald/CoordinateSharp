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
using CoordinateSharp.Formatters;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CoordinateSharp
{
    internal class MoonCalc
    {
        static readonly double rad = Math.PI / 180; //For converting radians

        //obliquity of the ecliptic in radians based on standard equinox 2000.
        static readonly double e = rad * 23.4392911; 
        /// <summary>
        /// Gets Moon Times, Altitude and Azimuth
        /// </summary>
        /// <param name="date">Date</param>
        /// <param name="lat">Latitude</param>
        /// <param name="lng">Longitude</param>
        /// <param name="c">Celestial</param>
        /// <param name="offset">Offset hours</param>
        public static void GetMoonTimes(DateTime date, double lat, double lng, Celestial c, double offset)
        {
            //Get current Moon Position to populate passed Alt / Azi for user specified date

            offset *= -1; //REVERSE OFFSET FOR POSITIONING

            //Get current moon position and coordinate
            MoonPosition mp = GetMoonPosition(date, lat, lng, c, offset, true);

            double altRad = mp.Altitude / Math.PI*180; //Convert alt to degrees
            c.moonAltitude = (altRad - mp.ParallaxCorection); //Set altitude with adjusted parallax                
            c.moonAzimuth = mp.Azimuth / Math.PI*180 + 180;  //Azimuth in degrees + 180 for E by N.
   
            ////New Iterations for Moon set / rise
            bool moonRise = false;
            bool moonSet = false;

            //Start at beginning of day
            DateTime t = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);

            //Get start of day Moon Pos
            MoonPosition moonPos = GetMoonPosition(t, lat, lng, c, offset,false);
            double alt1 = moonPos.Altitude - (moonPos.ParallaxCorection * rad);

            DateTime? setTime = null;
            DateTime? riseTime = null;
            double hz = -.3 * rad;//Horizon degrees at -.3 for appearant rise / set

            //Iterate for each hour of the day
            for(int x = 1;x<=24;x++)
            {               
                moonPos = GetMoonPosition(t.AddHours(x), lat, lng, c, offset, false);//Get the next hours altitude for comparison
                double alt2 = moonPos.Altitude - (moonPos.ParallaxCorection * rad);
                //If hour 1 is below horizon and hour 2 is above
                if(alt1 <hz && alt2 >=hz)
                {
                    //Moon Rise Occurred
                    moonRise = true;
                    DateTime dt1 = t.AddHours(x - 1);
                    moonPos = GetMoonPosition(dt1, lat, lng, c, offset, false);//Get the next hours altitude for comparison
                    double altM1 = moonPos.Altitude - (moonPos.ParallaxCorection * rad);
                    //Iterate through each minute to determine at which minute the horizon is crossed.
                    //Interpolation is more efficient, but yielded results with deviations up to 5 minutes. 
                    //Investigate formula efficiency 
                    for (int y = 1;y<=60;y++)
                    {
                        DateTime dt2 = t.AddHours(x-1).AddMinutes(y);
                        moonPos = GetMoonPosition(dt2, lat, lng, c, offset, false);//Get the next hours altitude for comparison
                        double altM2 = moonPos.Altitude - (moonPos.ParallaxCorection * rad);
                        if (altM1<hz && altM2>=hz)
                        {
                            //interpolate seconds
                            double p = 60 * ((hz - altM1) / (altM2 - altM1));
                            riseTime = dt1.AddMinutes(y-1).AddSeconds(p);
                            break;
                        }
                        altM1 = altM2;
                        
                    }             
                }
                //if hour 2 is above horizon and hour 1 below
                if(alt1>=hz && alt2 <hz)
                {
                    //Moon Set Occurred
                    moonSet = true;
                    DateTime dt1 = t.AddHours(x - 1);
                    moonPos = GetMoonPosition(dt1, lat, lng, c, offset, false);//Get the next hours altitude for comparison
                    double altM1 = moonPos.Altitude - (moonPos.ParallaxCorection * rad);
                    //Iterate through each minute to determine at which minute the horizon is crossed.
                    //Interpolation is more efficient, but yielded results with deviations up to 5 minutes. 
                    //Investigate formula efficiency 
                    for (int y = 1; y <= 60; y++)
                    {
                        DateTime dt2 = t.AddHours(x - 1).AddMinutes(y);
                        moonPos = GetMoonPosition(dt2, lat, lng, c, offset, false);//Get the next hours altitude for comparison
                        double altM2 = moonPos.Altitude - (moonPos.ParallaxCorection * rad);
                        if (altM1 >= hz && altM2 < hz)
                        {
                            //Interpolate seconds 
                            double p = 60 * ((hz - altM2) / (altM1 - altM2));
                            setTime = dt1.AddMinutes(y).AddSeconds(-p);
                            break;
                        }
                        altM1 = altM2;

                    }
                }
                alt1 = alt2;
                if(moonRise && moonSet) { break; }
            }

            c.moonSet = setTime;
            c.moonRise = riseTime;
            if (moonRise && moonSet) { c.moonCondition = CelestialStatus.RiseAndSet; }
            else
            {
                if (!moonRise && !moonSet)
                {
                    if (alt1 >= 0) { c.moonCondition = CelestialStatus.UpAllDay; }
                    else { c.moonCondition = CelestialStatus.DownAllDay; }
                }
                if (!moonRise && moonSet) { c.moonCondition = CelestialStatus.NoRise; }
                if (moonRise && !moonSet) { c.moonCondition = CelestialStatus.NoSet; }
            }          
        }

        private static MoonPosition GetMoonPosition(DateTime date, double lat, double lng, Celestial cel, double offset, bool setCelC)
        {
            //Set UTC date integrity
            date = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, DateTimeKind.Utc);
           
            double julianOffset = offset * .04166667;
           
            //Ch 47
            double JDE = JulianConversions.GetJulian(date);//Get julian 
            double oJDE = JulianConversions.GetJulian(date) + julianOffset;//Get julian 
            double T = (oJDE - 2451545) / 36525; //Get dynamic time.

            double[] LDMNF = Get_Moon_LDMNF(T);
            LunarCoordinates celC = GetMoonCoords(LDMNF, T, oJDE);
            if (setCelC) { cel.lunarCoordinates = celC; }
            Distance dist = GetMoonDistance(date.AddHours(offset));
            double lw = rad * -lng;
            double phi = rad * lat;
       
            double H = rad * MeeusFormulas.Get_Sidereal_Time(oJDE) - lw - celC.rightAscension.ToRadians();
            
            double ra = celC.rightAscension.ToRadians(); //Adjust current RA formula to avoid needless RAD conversions
            double dec = celC.declination.ToRadians(); //Adjust current RA formula to avoid needless RAD conversions

            //Adjust for parallax (low accuracy increases may not be worth cost)
            //Investigate
            double pSinE = Get_pSinE(dec, dist.Meters) * Math.PI / 180;
            double pCosE = Get_pCosE(dec, dist.Meters) * Math.PI / 180;
            double cRA = Parallax_RA(dist.Meters, H, pCosE, dec);
            double tDEC = Parallax_Dec(dist.Meters, H, pCosE, pSinE, dec, cRA);
            double tRA = ra - cRA;
            dec = tDEC;
            ra = tRA;

            //Get true altitude
            double h = Altitude(H, phi, dec);

            // formula 14.1 of "Astronomical Algorithms" 2nd edition by Jean Meeus (Willmann-Bell, Richmond) 1998.
            double pa = Math.Atan2(Math.Sin(H), Math.Tan(phi) * Math.Cos(dec) - Math.Sin(dec) * Math.Cos(H));
           
            //altitude correction for refraction
            h += AstroRefraction(h);

            MoonPosition mp = new MoonPosition();
            mp.Azimuth = Azimuth(H, phi, dec);
            mp.Altitude = h / Math.PI * 180;
            mp.Distance = dist; 
            mp.ParallacticAngle = pa;

            double horParal = 8.794 / (dist.Meters / 149.59787E6); // horizontal parallax (arcseconds), Meeus S. 263  
            double p = Math.Asin(Math.Cos(h) * Math.Sin(horParal/3600)); // parallax in altitude (degrees)
            p *= 1000;
          
            mp.ParallaxCorection = p;          
            mp.Altitude *= rad;
           
            return mp;
        }

        public static LunarCoordinates GetMoonCoords(DateTime date, double offset)
        {
            double julianOffset = offset * .04166667;

            //Ch 47
            double oJDE = JulianConversions.GetJulian(date) + julianOffset;//Get julian 
            double T = (oJDE - 2451545) / 36525; //Get dynamic time.

            double[] LDMNF = Get_Moon_LDMNF(T);

            return GetMoonCoords(LDMNF, T, oJDE);
        }

        private static LunarCoordinates GetMoonCoords(double[] LDMNF, double t, double JD)
        {
            LunarCoordinates cel = new LunarCoordinates();
            // Legacy function. Updated with Meeus Calcs for increased accuracy.
            // geocentric ecliptic coordinates of the moon
            // Meeus Ch 47
            double[] cs = Get_Moon_Coordinates(LDMNF, t);

            double l = cs[0]; // longitude
            double b = cs[1]; // latitude 
           

        
            double geocentricLongitude = l.ToDegrees();
            cel.julianDayDecimal = JD-.5 - Math.Floor(JD-.5);
            cel.latitude = b.ToDegrees();
            cel.rightAscension = RightAscension(l, b).ToDegrees().NormalizeDegrees360();
            cel.declination = Declination(l, b).ToDegrees();
            cel.geometricMeanLongitude = LDMNF[0].NormalizeDegrees360();

            

            //APPARENTS
            //Get longitutde of the acsencing node of the moon's mean orbit on the ecliptic, measure from the mean equinox of the date
            double lngAscendingNode = 125.04452 - 1934.136261 * t;
            double la = lngAscendingNode.ToRadians();

            //Mean Longitudes
            double sunL = 280.4665 + 36000.7698 * t; 
            double moonL = 218.3165 + 481267.8813 * t;
            cel.sunMeanLongitude = sunL.NormalizeDegrees360();
            //nutations in longitude
            double lngNutation = (-17.20).ToSeconds() * Math.Sin(la) - (1.32).ToSeconds() * Math.Sin(2 * sunL) - (.23).ToSeconds() * Math.Sin(2 * moonL) + (.21).ToSeconds() * Math.Sin(2*la);
            cel.longitude = geocentricLongitude + lngNutation;

            ////nutations in obliquity
            //double obNutation = (9.20).ToSeconds() * Math.Cos(la) + (.57).ToSeconds() * Math.Cos(2 * sunL) + (.10).ToSeconds() * Math.Cos(2 * moonL) - (.09).ToSeconds() * Math.Cos(2 * la);

            ////Obliquity of the ecliptic 22.2 (MAY NEED TO SWITCH TO 22.3 in future as it is higher accuracy)
            //double U = t / 100;
            //double E0 = Format.ToDegrees(23, 23, 21.488) - (46.8150).ToSeconds() * t - (.00059).ToSeconds() * Math.Pow(t, 2) + (.001813).ToSeconds() * Math.Pow(t, 3);
            //double E = E0 + obNutation; //true Obliquity


            return cel;
        }
       
        public static void GetMoonIllumination(DateTime date, Celestial c, double lat, double lng, EagerLoad el, double offset)
        {
       
            //Moon Illum must be done for both the Moon Name and Phase so either will trigger it to load
            if (el.Extensions.Lunar_Cycle)
            {
                date = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, DateTimeKind.Utc);
                offset *= -1;
                double julianOffset = offset * .04166667;
               
                SolarCoordinates s = SunCalc.Get_Solar_Coordinates(date, offset);
                double JDE = JulianConversions.GetJulian(date) + julianOffset;//Get julian 
                double T = (JDE - 2451545) / 36525; //Get dynamic time.
                double[] LDMNF = Get_Moon_LDMNF(T);

                LunarCoordinates m = GetMoonCoords(LDMNF, T, JDE);

                double sdist = 149598000,
                phi = Math.Acos(Math.Sin(s.declination.ToRadians()) * Math.Sin(m.declination.ToRadians()) + Math.Cos(s.declination.ToRadians()) * Math.Cos(m.declination.ToRadians()) * Math.Cos(s.rightAscension.ToRadians() - m.rightAscension.ToRadians())),
                inc = Math.Atan2(sdist * Math.Sin(phi), 0 - sdist * Math.Cos(phi)),
                angle = Math.Atan2(Math.Cos(s.declination.ToRadians()) * Math.Sin(s.rightAscension.ToRadians() - m.rightAscension.ToRadians()), Math.Sin(s.declination.ToRadians()) * Math.Cos(m.declination.ToRadians()) -
                        Math.Cos(s.declination.ToRadians()) * Math.Sin(m.declination.ToRadians()) * Math.Cos(s.rightAscension.ToRadians() - m.rightAscension.ToRadians()));


                MoonIllum mi = new MoonIllum();

                mi.Fraction = (1 + Math.Cos(inc)) / 2;
                mi.Phase = 0.5 + 0.5 * inc * (angle < 0 ? -1 : 1) / Math.PI;
                mi.Angle = angle;
           

                c.moonIllum = mi;

                MoonName moonName = MoonName.None;
                int moonDate = 0;
                //GET PHASE NAME

                //CHECK MOON AT BEGINNING AT END OF DAY TO GET DAY PHASE
                DateTime dMon = new DateTime(date.Year, date.Month, 1);
                for (int x = 1; x <= date.Day; x++)
                {
                    DateTime nDate = new DateTime(dMon.Year, dMon.Month, x, 0, 0, 0, DateTimeKind.Utc);
                    
                    s = SunCalc.Get_Solar_Coordinates(date, offset);
                    JDE = JulianConversions.GetJulian(nDate) + julianOffset;//Get julian 
                    T = (JDE - 2451545) / 36525; //Get dynamic time.
                    LDMNF = Get_Moon_LDMNF(T);
                    m = GetMoonCoords(LDMNF, T,JDE);

                    phi = Math.Acos(Math.Sin(s.declination.ToRadians()) * Math.Sin(m.declination.ToRadians()) + Math.Cos(s.declination.ToRadians()) * Math.Cos(m.declination.ToRadians()) * Math.Cos(s.rightAscension.ToRadians() - m.rightAscension.ToRadians()));
                    inc = Math.Atan2(sdist * Math.Sin(phi), 0 - sdist * Math.Cos(phi));
                    angle = Math.Atan2(Math.Cos(s.declination.ToRadians()) * Math.Sin(s.rightAscension.ToRadians() - m.rightAscension.ToRadians()), Math.Sin(s.declination.ToRadians()) * Math.Cos(m.declination.ToRadians()) -
                            Math.Cos(s.declination.ToRadians()) * Math.Sin(m.declination.ToRadians()) * Math.Cos(s.rightAscension.ToRadians() - m.rightAscension.ToRadians()));

                    double startPhase = 0.5 + 0.5 * inc * (angle < 0 ? -1 : 1) / Math.PI;

                    nDate = new DateTime(dMon.Year, dMon.Month, x, 23, 59, 59, DateTimeKind.Utc);
                   
                    s = SunCalc.Get_Solar_Coordinates(date, offset);
                    JDE = JulianConversions.GetJulian(nDate) + julianOffset;//Get julian 
                    T = (JDE - 2451545) / 36525; //Get dynamic time.
                    LDMNF = Get_Moon_LDMNF(T);
                    m = GetMoonCoords(LDMNF, T,JDE);

                    phi = Math.Acos(Math.Sin(s.declination.ToRadians()) * Math.Sin(m.declination.ToRadians()) + Math.Cos(s.declination.ToRadians()) * Math.Cos(m.declination.ToRadians()) * Math.Cos(s.rightAscension.ToRadians() - m.rightAscension.ToRadians()));
                    inc = Math.Atan2(sdist * Math.Sin(phi), 0 - sdist * Math.Cos(phi));
                    angle = Math.Atan2(Math.Cos(s.declination.ToRadians()) * Math.Sin(s.rightAscension.ToRadians() - m.rightAscension.ToRadians()), Math.Sin(s.declination.ToRadians()) * Math.Cos(m.declination.ToRadians()) -
                            Math.Cos(s.declination.ToRadians()) * Math.Sin(m.declination.ToRadians()) * Math.Cos(s.rightAscension.ToRadians() - m.rightAscension.ToRadians()));

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
                            mi.PhaseNameEnum = PhaseEnum.NewMoon;
                            break;
                        }
                        if (startPhase <= .25 && endPhase >= .25)
                        {
                            mi.PhaseName = "First Quarter";
                            mi.PhaseNameEnum = PhaseEnum.FirstQuarter;
                            break;
                        }
                        if (startPhase <= .5 && endPhase >= .5)
                        {
                            mi.PhaseName = "Full Moon";
                            mi.PhaseNameEnum = PhaseEnum.FullMoon;
                            break;
                        }
                        if (startPhase <= .75 && endPhase >= .75)
                        {
                            mi.PhaseName = "Last Quarter";
                            mi.PhaseNameEnum = PhaseEnum.LastQuarter;
                            break;
                        }

                        if (startPhase > 0 && startPhase < .25 && endPhase > 0 && endPhase < .25)
                        {
                            mi.PhaseName = "Waxing Crescent";
                            mi.PhaseNameEnum = PhaseEnum.WaxingCrescent;
                            break;
                        }
                        if (startPhase > .25 && startPhase < .5 && endPhase > .25 && endPhase < .5)
                        {
                            mi.PhaseName = "Waxing Gibbous";
                            mi.PhaseNameEnum = PhaseEnum.WaxingGibbous;
                            break;
                        }
                        if (startPhase > .5 && startPhase < .75 && endPhase > .5 && endPhase < .75)
                        {
                            mi.PhaseName = "Waning Gibbous";
                            mi.PhaseNameEnum = PhaseEnum.WaningGibbous;
                            break;
                        }
                        if (startPhase > .75 && startPhase < 1 && endPhase > .75 && endPhase < 1)
                        {
                            mi.PhaseName = "Waning Crescent";
                            mi.PhaseNameEnum = PhaseEnum.WaningCrescent;
                            break;
                        }
                    }

                }
               

                if (date.Day == moonDate)
                {
                    c.AlmanacMoonName.emoonName = moonName;
                }               
                else 
                {
                    c.AlmanacMoonName.emoonName = MoonName.None;                   
                }
             

            }
            if (el.Extensions.Lunar_Eclipse) { CalculateLunarEclipse(date, lat, lng, c); }

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
                //string date = values[0];
                //// Ensure year is 4 digits
                //string[] parts = input.Split('-');
                //if (parts[0].Length == 3)
                //{
                //    parts[0] = "0" + parts[0];
                //}
                //date = string.Join("-", parts);
                string[] formats = { "yyyy-MMM-dd", "yyy-MMM-dd", "yy-MMM-dd", "y-MMM-dd" };
                DateTime ld = DateTime.ParseExact(values[0], formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
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

        private static MoonName GetMoonName(int month, MoonName name)
        {
            if (name != MoonName.None) { return MoonName.Blue; }
            switch (month)
            {
                case 1:
                    return MoonName.Wolf;
                case 2:
                    return MoonName.Snow;
                case 3:
                    return MoonName.Worm;
                case 4:
                    return MoonName.Pink;
                case 5:
                    return MoonName.Flower;
                case 6:
                    return MoonName.Strawberry;
                case 7:
                    return MoonName.Buck;
                case 8:
                    return MoonName.Sturgeon;
                case 9:
                    return MoonName.Corn;
                case 10:
                    return MoonName.Hunters;
                case 11:
                    return MoonName.Beaver;
                case 12:
                    return MoonName.Cold;
                default:
                    return MoonName.None;
            }
        }
      

        //v1.1.3 Formulas
        //The following formulas are either additions 
        //or conversions of SunCalcs formulas into Meeus

        /// <summary>
        /// Grabs Perigee or Apogee of Moon based on specified time.
        /// Results will return event just before, or just after specified DateTime
        /// </summary>
        /// <param name="d">DateTime</param>
        /// <param name="md">Event Type</param>
        /// <returns>PerigeeApogee</returns>
        private static PerigeeApogee MoonPerigeeOrApogee(DateTime d, MoonDistanceType md)
        {
            //Perigee & Apogee Algorithms from Jean Meeus Astronomical Algorithms Ch. 50

            //50.1
            //JDE = 2451534.6698 + 27.55454989 * k 
            //                     -0.0006691 * Math.Pow(T,2)
            //                     -0.000.01098 * Math.Pow(T,3)
            //                     -0.0000000052 * Math.Pow(T,4)

            //50.2
            //K approx = (yv - 1999.97)*13.2555
            //yv is the year + percentage of days that have occured in the year. 1998 Oct 1 is approx 1998.75
            //k ending in .0 represent perigee and .5 apogee. Anything > .5 is an error.

            //50.3
            //T = k/1325.55

            double yt = 365; //days in year
            if (DateTime.IsLeapYear(d.Year)) { yt = 366; } //days in year if leap year
            double f = d.DayOfYear / yt; //Get percentage of year that as passed
            double yv = d.Year + f; //add percentage of year passed to year.
            double k = (yv - 1999.97) * 13.2555; //find approximate k using formula 50.2

            //Set k decimal based on apogee or perigee
            if (md == MoonDistanceType.Apogee)
            {
                k = Math.Floor(k) + .5;
            }
            else
            {
                k = Math.Floor(k);
            }

            //Find T using formula 50.3
            double T = k / 1325.55;
            //Find JDE using formula 50.1
            double JDE = 2451534.6698 + 27.55454989 * k -
                0.0006691 * Math.Pow(T, 2) -
                0.00001098 * Math.Pow(T, 3) -
                0.0000000052 * Math.Pow(T, 4);

            //Find Moon's mean elongation at time JDE.
            double D = 171.9179 + 335.9106046 * k -
                0.0100383 * Math.Pow(T, 2) -
                0.00001156 * Math.Pow(T, 3) +
                0.000000055 * Math.Pow(T, 4);

            //Find Sun's mean anomaly at time JDE
            double M = 347.3477 + 27.1577721 * k -
                0.0008130 * Math.Pow(T, 2) -
                0.0000010 * Math.Pow(T, 3);


            //Find Moon's argument of latitude at Time JDE
            double F = 316.6109 + 364.5287911 * k -
                0.0125053 * Math.Pow(T, 2) -
                0.0000148 * Math.Pow(T, 3);

            //Normalize DMF to a 0-360 degree number
            D %= 360;
            if (D < 0) { D += 360; }
            M %= 360;
            if (M < 0) { M += 360; }
            F %= 360;
            if (F < 0) { F += 360; }

            //Convert DMF to radians
            D = D * Math.PI / 180;
            M = M * Math.PI / 180;
            F = F * Math.PI / 180;
            double termsA;
            //Find Terms A from Table 50.A 
            if (md == MoonDistanceType.Apogee)
            {
                termsA = MeeusTables.ApogeeTermsA(D, M, F, T);
            }
            else
            {
                termsA = MeeusTables.PerigeeTermsA(D, M, F, T);
            }
            JDE += termsA;
            double termsB;
            if (md == MoonDistanceType.Apogee)
            {
                termsB = MeeusTables.ApogeeTermsB(D, M, F, T);
            }
            else
            {
                termsB = MeeusTables.PerigeeTermsB(D, M, F, T);
            }
            //Convert julian back to date
            DateTime date = JulianConversions.GetDate_FromJulian(JDE).Value;
            //Obtain distance
            Distance dist = GetMoonDistance(date);

            PerigeeApogee ap = new PerigeeApogee(date, termsB, dist);
            return ap;
        }

        public static Perigee GetPerigeeEvents(DateTime d)
        {
            //Iterate in 15 day increments due to formula variations.
            //Determine closest events to date.
            //per1 is last date
            //per2 is next date

            //integrity for new date.
            if (d.Year <= 0001) { return new Perigee(new PerigeeApogee(new DateTime(), 0, new Distance(0)), new PerigeeApogee(new DateTime(), 0, new Distance(0))); }
            //Start at lowest increment
            PerigeeApogee per1 = MoonPerigeeOrApogee(d.AddDays(-45), MoonDistanceType.Perigee);
            PerigeeApogee per2 = MoonPerigeeOrApogee(d.AddDays(-45), MoonDistanceType.Perigee);

            for (int x = -30; x <= 45; x+=15)
            {
                //used for comparison 
                PerigeeApogee t = MoonPerigeeOrApogee(d.AddDays(x), MoonDistanceType.Perigee);
             
                //Find the next pergiee after specified date           
                if (t.Date > per2.Date && t.Date >= d)
                {
                    per2 = t; 
                    break;
                }
                //Find last perigee before specified date
                if (t.Date > per1.Date && t.Date < d)
                {
                    per1 = t;
                    per2 = t;
                }

            }
            return new Perigee(per1, per2);
        }
        public static Apogee GetApogeeEvents(DateTime d)
        {
            //Iterate in 5 month increments due to formula variations.
            //Determine closest events to date.
            //apo1 is last date
            //apo2 is next date

            //integrity for new date.
            if (d.Year <= 0001) { return new Apogee(new PerigeeApogee(new DateTime(), 0, new Distance(0)), new PerigeeApogee(new DateTime(), 0, new Distance(0))); }

            PerigeeApogee apo1 = MoonPerigeeOrApogee(d.AddDays(-45), MoonDistanceType.Apogee);
            PerigeeApogee apo2 = MoonPerigeeOrApogee(d.AddDays(-45), MoonDistanceType.Apogee);
            for (int x = -30; x <= 45; x+=15)
            {
                PerigeeApogee t = MoonPerigeeOrApogee(d.AddDays(x), MoonDistanceType.Apogee);
                //Find next apogee after specified date
                if (t.Date > apo2.Date && t.Date >= d)
                {
                    apo2 = t;
                    break;
                }
                //Find last apogee before specified date
                if (t.Date > apo1.Date && t.Date < d)
                {
                    apo1 = t;
                    apo2 = t;
                }
                
            }
            return new Apogee(apo1, apo2);

        }

        public static void GetMoonDistance(DateTime date, Celestial c)
        {
            date = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, DateTimeKind.Utc);

            c.moonDistance = GetMoonDistance(date,0);      //Updating distance formula    
        }
        public static void GetMoonDistance(DateTime date, Celestial c, double offset)
        {
            date = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, DateTimeKind.Utc);

            c.moonDistance = GetMoonDistance(date, offset);      //Updating distance formula    
        }
        /// <summary>
        /// Gets moon distance (Ch 47).
        /// </summary>
        /// <param name="d">DateTime</param>
        /// <returns>Distance</returns>
        public static Distance GetMoonDistance(DateTime d)
        {
            return GetMoonDistance(d, 0);
        }
        /// <summary>
        /// Gets moon distance (Ch 47).
        /// </summary>
        /// <param name="d">DateTime</param>
        /// <param name="offset">UTC offset in hours</param>
        /// <returns>Distance</returns>
        public static Distance GetMoonDistance(DateTime d, double offset)
        { 
            //Ch 47
            offset *= -1;
            double julianOffset = offset * .04166667;
            double JDE = JulianConversions.GetJulian(d)+julianOffset;//Get julian 
            double T = (JDE - 2451545) / 36525; //Get dynamic time.
          
            double[] values = Get_Moon_LDMNF(T);

            double D = values[1];
            double M = values[2];
            double N = values[3];
            double F = values[4];

            //Ch 47 distance formula
            double dist = 385000.56 + (MeeusTables.Moon_Periodic_Er(D, M, N, F, T) / 1000);
            return new Distance(dist);
        }      

        /// <summary>
        /// Gets Moon L, D, M, N, F values
        /// Ch. 47 
        /// </summary>
        /// <param name="T">Dynamic Time</param>
        /// <returns>double[] containing L,D,M,N,F</returns>
        static double[] Get_Moon_LDMNF(double T)
        {
            //T = dynamic time

            //Moon's mean longitude
            double L = 218.3164477 + 481267.88123421 * T -
                 .0015786 * Math.Pow(T, 2) + Math.Pow(T, 3) / 538841 -
                 Math.Pow(T, 4) / 65194000;

            //Moon's mean elongation 
            double D = 297.8501921 + 445267.1114034 * T -
                0.0018819 * Math.Pow(T, 2) + Math.Pow(T, 3) / 545868 - Math.Pow(T, 4) / 113065000;
            //Sun's mean anomaly
            double M = 357.5291092 + 35999.0502909 * T -
                .0001536 * Math.Pow(T, 2) + Math.Pow(T, 3) / 24490000;
            //Moon's mean anomaly
            double N = 134.9633964 + 477198.8675055 * T + .0087414 * Math.Pow(T, 2) +
                Math.Pow(T, 3) / 69699 - Math.Pow(T, 4) / 14712000;
            //Moon's argument of latitude
            double F = 93.2720950 + 483202.0175233 * T - .0036539 * Math.Pow(T, 2) - Math.Pow(T, 3) /
                3526000 + Math.Pow(T, 4) / 863310000;

            //Normalize DMF to a 0-360 degree number      
            D %= 360;
            if (D < 0) { D += 360; }
            M %= 360;
            if (M < 0) { M += 360; }
            N %= 360;
            if (N < 0) { N += 360; }
            F %= 360;
            if (F < 0) { F += 360; }
           
            //Convert DMF to radians
          
            D = D * Math.PI / 180;
            M = M * Math.PI / 180;
            N = N * Math.PI / 180;
            F = F * Math.PI / 180;

            return new double[] { L, D, M, N, F };
        }
        /// <summary>
        /// Get moons lat/long in radians (Ch 47).
        /// </summary>
        /// <param name="LDMNF">L,D,M,N,F</param>
        /// <param name="T">Dynamic Time</param>
        /// <returns>Lat[0], Long[1]</returns>
        private static double[] Get_Moon_Coordinates(double[] LDMNF,double T)
        {
            //Reference Ch 47.
            double lat = LDMNF[0] + (MeeusTables.Moon_Periodic_El(LDMNF[0], LDMNF[1], LDMNF[2], LDMNF[3], LDMNF[4],T)/1000000);
            double longi = MeeusTables.Moon_Periodic_Eb(LDMNF[0], LDMNF[1], LDMNF[2], LDMNF[3], LDMNF[4], T) / 1000000;
            lat %= 360;
            if (lat < 0) { lat += 360; }
           
            //Convert to radians
            double l = rad *  lat; // longitude
            double b = rad * longi; // latitude
           
            return new double[] { l, b };
        }
        
        /// <summary>
        /// Gets right Ascension of celestial object (Ch 13 Fig 13.3)
        /// </summary>
        /// <param name="l">latitude in radians</param>
        /// <param name="b">longitude in radian</param>
        /// <returns>Right Ascension</returns>
        private static double RightAscension(double l, double b)
        {
            //Ch 13 Fig 13.3
            //tan a = ( sin(l) * cos(e) - tan(b)-sin(e) ) / cons(l)
            //Converts to the following using Atan2 for 4 quadriatic regions
            return Math.Atan2(Math.Sin(l) * Math.Cos(e) - Math.Tan(b) * Math.Sin(e), Math.Cos(l));
        }
        /// <summary>
        /// Gets declination of celestial object (Ch 13 Fig 13.4)
        /// </summary>
        /// <param name="l">latitude in radians</param>
        /// <param name="b">longitude in radian</param>
        /// <returns>Declination</returns>
        private static double Declination(double l, double b)
        {
            //Ch 13 Fig 13.4
            //sin o =  sin(b) * cos(e) + cos(b)*sin(e) * sin(l)
            //Converts to the following using Asin
            return Math.Asin(Math.Sin(b) * Math.Cos(e) + Math.Cos(b) * Math.Sin(e) * Math.Sin(l));
        }
        
        static double Parallax_Dec(double distance, double H, double pCosE, double pSinE, double dec, double cRA)
        {
            //Ch 40 (Correction for parallax
            //H - geocentric hour angle of the body (sidereal) IAW Ch 12
            double pi = Math.Asin((Math.Sin(8.794 / distance))) * Math.PI / 180; // 40.1 in radians
            H = H * Math.PI / 180;
            //Directly to topocencric dec
            double tDEC = Math.Atan2((Math.Sin(dec) - pSinE * Math.Sin(pi)) * Math.Cos(cRA), Math.Cos(dec) - pCosE * Math.Sin(pi) * Math.Cos(H));
            return tDEC;

        }
        static double Parallax_RA(double distance, double H, double pCosE, double dec)
        {
            //ENSURE RADIANS

            //Ch 40 (Correction for parallax
            //H - geocentric hour angle of the body (sidereal) IAW Ch 12

            double pi = Math.Asin((Math.Sin(8.794 / distance))) * Math.PI / 180; // 40.1


            //Convert to Radian
            double t = -pCosE * Math.Sin(pi) * Math.Sin(H);
            double b = Math.Cos(dec) - pCosE * Math.Sin(pi) * Math.Cos(H);
            double cRA = Math.Atan2(t, b);
            return cRA;
            //Topocencric RA = RA - cRA
        }
        static double Get_pSinE(double dec, double H)
        {
            //ASSUME WGS 84 FOR NOW         
            double ba = .99664719; // or 1-f
            double u = (ba * dec) * Math.PI / 180;

            double ps = ba * Math.Sin(u) + (H / 6378140) * Math.Sin(dec);
            return ps;

        }
        static double Get_pCosE(double dec, double H)
        {
            //ASSUME WGS 84 FOR NOW        
            double ba = .99664719; // or 1-f
            double u = (ba * dec) * Math.PI / 180;

            double ps = Math.Cos(u) + (H / 6378140) * Math.Cos(dec);
            return ps;
        }

        static double Azimuth(double H, double phi, double dec) { return Math.Atan2(Math.Sin(H), Math.Cos(H) * Math.Sin(phi) - Math.Tan(dec) * Math.Cos(phi)); }
        static double Altitude(double H, double phi, double dec)
        {
            return Math.Asin(Math.Sin(phi) * Math.Sin(dec) + Math.Cos(phi) * Math.Cos(dec) * Math.Cos(H));
        }
        static double AstroRefraction(double h)
        {
            //CH 16
            double P = 1013.25; //Average pressure of earth
            double T = 16; //Average temp of earth
            double alt = h / Math.PI * 180;
            double Ref = P * (.1594 + .0196 * alt + .00002 * Math.Pow(alt, 2)) / ((273 + T) * (1 + .505 * alt + .0845 * Math.Pow(alt, 2)));
            return Ref / 60;         
        }
    }
}
