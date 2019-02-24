using System;
using System.ComponentModel;
using System.Collections.Generic;
namespace CoordinateSharp
{ 
    public partial class Celestial
    {
        //When a rise or a set does not occur, the DateTime will return null
        /// <summary>
        /// Creates an empty Celestial.
        /// </summary>
        public Celestial()
        {
            astrologicalSigns = new AstrologicalSigns();
            lunarEclipse = new LunarEclipse();
            solarEclipse = new SolarEclipse();
            CalculateCelestialTime(0, 0, new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc));                     
        }

        private Celestial(bool hasCalcs)
        {

            astrologicalSigns = new AstrologicalSigns();
            lunarEclipse = new LunarEclipse();
            solarEclipse = new SolarEclipse();
            if (hasCalcs) { CalculateCelestialTime(0, 0, new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)); }
        }

        /// <summary>
        /// Creates a Celestial based on a location and specified date
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="longi">longitude</param>
        /// <param name="geoDate">DateTime (UTC)</param>
        public Celestial(double lat, double longi, DateTime geoDate)
        {
            DateTime d = new DateTime(geoDate.Year, geoDate.Month, geoDate.Day, geoDate.Hour, geoDate.Minute, geoDate.Second, DateTimeKind.Utc);
            astrologicalSigns = new AstrologicalSigns();
            lunarEclipse = new LunarEclipse();
            solarEclipse = new SolarEclipse();
            CalculateCelestialTime(lat, longi, d);
        }

        /// <summary>
        /// Creates a Celestial based on a location and date in the provided Coordinate.
        /// </summary>
        /// <param name="c">Coordinate</param>
        /// <returns>Celestial</returns>
        public static Celestial LoadCelestial(Coordinate c)
        {
            DateTime geoDate = c.GeoDate;

            DateTime d = new DateTime(geoDate.Year, geoDate.Month, geoDate.Day, geoDate.Hour, geoDate.Minute, geoDate.Second, DateTimeKind.Utc);
            Celestial cel = new Celestial(c.Latitude.ToDouble(), c.Longitude.ToDouble(), c.GeoDate);

            return cel;
        }
      
        /// <summary>
        /// Converts Celestial values to local times.
        /// </summary>
        /// <param name="c">Coordinate</param>
        /// <param name="offset">UTC offset</param>
        /// <returns></returns>
        public static Celestial Celestial_LocalTime(Coordinate c, double offset)
        {
            if(offset < -12 || offset > 12) { throw new ArgumentOutOfRangeException("Time offsets cannot be greater 12 or less than -12."); }
            //Probably need to offset initial UTC date so user can op in local
            //Determine best way to do this.
            DateTime d = c.GeoDate.AddHours(offset);

            //Get 3 objects for comparison
            Celestial cel = new Celestial(c.Latitude.ToDouble(), c.Longitude.ToDouble(), c.GeoDate);
            Celestial celPre = new Celestial(c.Latitude.ToDouble(), c.Longitude.ToDouble(), c.GeoDate.AddDays(-1));    
            Celestial celPost = new Celestial(c.Latitude.ToDouble(), c.Longitude.ToDouble(), c.GeoDate.AddDays(1));
           
            //Slip objects for comparison. Compare with slipped date. 
            celPre.Local_Convert(c, offset);
            cel.Local_Convert(c, offset);
            celPost.Local_Convert(c, offset);

            //Get SunSet
            int i = Determine_Slipped_Event_Index(cel.SunSet, celPre.SunSet, celPost.SunSet, d);
            cel.sunSet = Get_Correct_Slipped_Date(cel.SunSet, celPre.SunSet, celPost.SunSet, i);
            cel.AdditionalSolarTimes.civilDusk = Get_Correct_Slipped_Date(cel.AdditionalSolarTimes.CivilDusk, 
                celPre.AdditionalSolarTimes.CivilDusk, celPost.AdditionalSolarTimes.CivilDusk, i);
            cel.AdditionalSolarTimes.nauticalDusk = Get_Correct_Slipped_Date(cel.AdditionalSolarTimes.NauticalDusk,
               celPre.AdditionalSolarTimes.NauticalDusk, celPost.AdditionalSolarTimes.NauticalDusk, i);
            //Get SunRise
            i = Determine_Slipped_Event_Index(cel.SunRise, celPre.SunRise, celPost.SunRise, d);
            cel.sunRise = Get_Correct_Slipped_Date(cel.SunRise, celPre.SunRise, celPost.SunRise, i);
            cel.AdditionalSolarTimes.civilDawn = Get_Correct_Slipped_Date(cel.AdditionalSolarTimes.CivilDawn,
                celPre.AdditionalSolarTimes.CivilDawn, celPost.AdditionalSolarTimes.CivilDawn, i);
            cel.AdditionalSolarTimes.nauticalDawn = Get_Correct_Slipped_Date(cel.AdditionalSolarTimes.NauticalDawn,
               celPre.AdditionalSolarTimes.NauticalDawn, celPost.AdditionalSolarTimes.NauticalDawn, i);

            //MoonRise
            i = Determine_Slipped_Event_Index(cel.MoonRise, celPre.MoonRise, celPost.MoonRise, d);
            cel.moonRise = Get_Correct_Slipped_Date(cel.MoonRise, celPre.MoonRise, celPost.MoonRise, i);
        
            //MoonSet
            i = Determine_Slipped_Event_Index(cel.MoonSet, celPre.MoonSet, celPost.MoonSet, d);
            cel.moonSet = Get_Correct_Slipped_Date(cel.MoonSet, celPre.MoonSet, celPost.MoonSet, i);

            //Local Conditions
            CelestialStatus[] cels = new CelestialStatus[]
            {
                celPre.MoonCondition,cel.MoonCondition,celPost.MoonCondition
            };
            cel.moonCondition = Celestial.GetStatus(cel.MoonRise, cel.MoonSet, cels);
            cels = new CelestialStatus[]
            {
                celPre.SunCondition, cel.SunCondition, celPost.SunCondition
            };
            cel.sunCondition = Celestial.GetStatus(cel.SunRise, cel.SunSet, cels);

            //Load IsUp values based on local time with populated Celestial
            Celestial.Calculate_Celestial_IsUp_Booleans(d, cel);

            return cel;
        }

        private static CelestialStatus GetStatus(DateTime? rise, DateTime? set,  CelestialStatus[] cels)
        {  
            if (set.HasValue && rise.HasValue) { return CelestialStatus.RiseAndSet; }
            if (set.HasValue && !rise.HasValue) { return CelestialStatus.NoRise; }
            if (!set.HasValue && rise.HasValue) { return CelestialStatus.NoSet; }
            for (int x=0; x < 3;x++)
            {
                if(cels[x] == CelestialStatus.DownAllDay || cels[x] == CelestialStatus.UpAllDay)
                {
                    return cels[x];
                }
            }
            return cels[1];
        }

        /// <summary>
        /// In place time slip
        /// </summary>
        /// <param name="c">Coordinate</param>
        /// <param name="offset">hour offset</param>
        private void Local_Convert(Coordinate c, double offset)
        {
            //Find new lunar set rise times
            if (MoonSet.HasValue) { moonSet = moonSet.Value.AddHours(offset); }
            if (MoonRise.HasValue) { moonRise = moonRise.Value.AddHours(offset); }
            //Perigee
            Perigee.ConvertTo_Local_Time(offset);
            //Apogee
            Apogee.ConvertTo_Local_Time(offset);
            //Eclipse
            LunarEclipse.ConvertTo_LocalTime(offset);

            ////Solar
            if (sunSet.HasValue) { sunSet = sunSet.Value.AddHours(offset); }
            if (SunRise.HasValue) { sunRise = SunRise.Value.AddHours(offset); }
            AdditionalSolarTimes.Convert_To_Local_Time(offset);

            //Eclipse
            SolarEclipse.ConvertTo_LocalTime(offset);
            SunCalc.CalculateZodiacSign(c.GeoDate.AddHours(offset), this);
            MoonCalc.GetMoonSign(c.GeoDate.AddHours(offset), this);
            
        }

        private PerigeeApogee Get_Correct_Slipped_Date(PerigeeApogee actual, PerigeeApogee pre, PerigeeApogee post, int i)
        {
            switch (i)
            {
                case 0:
                    return pre;
                case 1:
                    return actual;
                case 2:
                    return post;
                default:
                    return actual;
            }
        }
        private static DateTime? Get_Correct_Slipped_Date(DateTime? actual, DateTime? pre, DateTime? post, int i)
        {
            switch(i)
            {
                case 0:
                    return pre;
                case 1:
                    return actual;
                case 2:
                    return post;
                default:
                    return null;
            }
        }
        private static int Determine_Slipped_Event_Index(DateTime? actual, DateTime? pre, DateTime? post, DateTime d)
        {
           
            if (actual.HasValue)
            {
                if (actual.Value.Day != d.Day)
                {
                    if (pre.HasValue)
                    {                        
                        if (pre.Value.Day == d.Day) { return 0; }
                    }
                    if (post.HasValue)
                    {
                        if (post.Value.Day == d.Day) { return 2; }
                    }
                    return 3;
                }
            }
            else
            {
                if (pre.HasValue)
                {
                    if (pre.Value.Day == d.Day) { return 0; }
                }
                if (post.HasValue)
                {
                    if (post.Value.Day == d.Day) { return 2; }
                }
            }         
            return 1;
        }


        /// <summary>
        /// Calculates all celestial data. Coordinates will notify as changes occur
        /// </summary>
        /// <param name="lat">Decimal format latitude</param>
        /// <param name="longi">Decimal format longitude</param>
        /// <param name="date">Geographic DateTime</param>
        internal void CalculateCelestialTime(double lat, double longi, DateTime date)
        {
            date = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, DateTimeKind.Utc);
            SunCalc.CalculateSunTime(lat, longi, date, this);
            MoonCalc.GetMoonTimes(date, lat, longi, this);
            MoonCalc.GetMoonDistance(date, this);
            
            SunCalc.CalculateZodiacSign(date, this);
            MoonCalc.GetMoonSign(date, this);

            MoonCalc.GetMoonIllumination(date, this,lat,longi);

           
            perigee = MoonCalc.GetPerigeeEvents(date);
            apogee = MoonCalc.GetApogeeEvents(date);

            Calculate_Celestial_IsUp_Booleans(date, this);

        }
        /// <summary>
        /// Calculate celestial data based on lat/long and date.
        /// </summary>
        /// <param name="lat">Decimal format latitude</param>
        /// <param name="longi">Decimal format longitude</param>
        /// <param name="date">Geographic DateTime</param>
        /// <returns>Fully populated Celestial object</returns>
        public static Celestial CalculateCelestialTimes(double lat, double longi, DateTime date)
        {
            date = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, DateTimeKind.Utc);

            Celestial c = new Celestial(false);

            SunCalc.CalculateSunTime(lat, longi, date, c);
            MoonCalc.GetMoonTimes(date, lat, longi, c);
            MoonCalc.GetMoonDistance(date, c);
            SunCalc.CalculateZodiacSign(date, c);
            MoonCalc.GetMoonSign(date, c);
            MoonCalc.GetMoonIllumination(date, c,lat,longi);
           
            c.perigee = MoonCalc.GetPerigeeEvents(date);
            c.apogee = MoonCalc.GetApogeeEvents(date);

            Calculate_Celestial_IsUp_Booleans(date, c);

            return c;
        }

        /// <summary>
        /// Calculate sun data based on lat/long and date.
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="longi">longitude</param>
        /// <param name="date">DateTime</param>
        /// <returns>Celestial (Partially Populated)</returns>
        public static Celestial CalculateSunData(double lat, double longi, DateTime date)
        {
            date = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, DateTimeKind.Utc);

            Celestial c = new Celestial(false);
            SunCalc.CalculateSunTime(lat, longi, date, c);
            SunCalc.CalculateZodiacSign(date, c);
           
            return c;
        }
        /// <summary>
        /// Calculate moon data based on lat/long and date.
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="longi">longitude</param>
        /// <param name="date">DateTime</param>
        /// <returns>Celestial (Partially Populated)</returns>
        public static Celestial CalculateMoonData(double lat, double longi, DateTime date)
        {
            date = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, DateTimeKind.Utc);

            Celestial c = new Celestial(false);

            MoonCalc.GetMoonTimes(date, lat, longi, c);
            MoonCalc.GetMoonDistance(date, c);
            MoonCalc.GetMoonSign(date, c);
            MoonCalc.GetMoonIllumination(date, c,lat,longi);

            c.perigee = MoonCalc.GetPerigeeEvents(date);
            c.apogee = MoonCalc.GetApogeeEvents(date);

            return c;
        }

        /// <summary>
        /// Returns a List containing solar eclipse data for the century.
        /// Century return is based on the date passed.
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="longi">longitude</param>
        /// <param name="date">DateTime</param>
        /// <returns></returns>
        public static List<SolarEclipseDetails> Get_Solar_Eclipse_Table(double lat, double longi, DateTime date)
        {
            //Convert to Radians
            double latR = lat * Math.PI / 180;
            double longR = longi * Math.PI / 180;
            //Get solar data based on date
            double[] events = Eclipse.SolarData.SolarDateData_100Year(date);
            //Return list of solar data.
            return SolarEclipseCalc.CalculateSolarEclipse(date, latR, longR, events);
        }
        /// <summary>
        /// Returns a List containing solar eclipse data for the century.
        /// Century return is based on the date passed.
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="longi">longitude</param>
        /// <param name="date">DateTime</param>
        /// <returns></returns>
        public static List<LunarEclipseDetails> Get_Lunar_Eclipse_Table(double lat, double longi, DateTime date)
        {
            //Convert to Radians
            double latR = lat * Math.PI / 180;
            double longR = longi * Math.PI / 180;
            //Get solar data based on date
            double[] events = Eclipse.LunarData.LunarDateData_100Year(date);
            //Return list of solar data.
            return LunarEclipseCalc.CalculateLunarEclipse(date, latR, longR, events);
        }
        
        /// <summary>
        /// Set bool SunIsUp and MoonIsUp values
        /// </summary>
        /// <param name="date">Coordinate GeoDate</param>
        /// <param name="cel">Celestial Object</param>
        private static void Calculate_Celestial_IsUp_Booleans(DateTime date, Celestial cel)
        {
            //SUN
            switch (cel.SunCondition)
            {
                case CelestialStatus.DownAllDay:
                    cel.isSunUp = false;
                    break;
                case CelestialStatus.UpAllDay:
                    cel.isSunUp = true;
                    break;
                case CelestialStatus.NoRise:
                    if(date<cel.SunSet)
                    {
                        cel.isSunUp = true;
                    }
                    else { cel.isSunUp = false; }
                    break;
                case CelestialStatus.NoSet:
                    if (date > cel.SunRise)
                    {
                        cel.isSunUp = true;
                    }
                    else { cel.isSunUp = false; }
                    break;
                case CelestialStatus.RiseAndSet:
                    if (cel.SunRise < cel.SunSet)
                    {
                        if (date > cel.SunRise && date < cel.SunSet)
                        {
                            cel.isSunUp = true;
                        }
                        else
                        {
                            cel.isSunUp = false;
                        }
                    }
                    else
                    {
                        if (date > cel.SunRise || date < cel.SunSet)
                        {
                            cel.isSunUp = true;
                        }
                        else
                        {
                            cel.isSunUp = false;
                        }
                    }
                    break;
                default:
                    //Should never be reached. If reached, previous calculations failed somewhere.
                    break;
            }

            //MOON
            switch (cel.MoonCondition)
            {
                case CelestialStatus.DownAllDay:
                    cel.isMoonUp = false;
                    break;
                case CelestialStatus.UpAllDay:
                    cel.isMoonUp = true;
                    break;
                case CelestialStatus.NoRise:
                    if (date < cel.MoonSet)
                    {
                        cel.isMoonUp = true;
                    }
                    else { cel.isMoonUp = false; }
                    break;
                case CelestialStatus.NoSet:
                    if (date > cel.MoonRise)
                    {
                        cel.isMoonUp = true;
                    }
                    else { cel.isMoonUp = false; }
                    break;
                case CelestialStatus.RiseAndSet:
                    if (cel.MoonRise < cel.MoonSet)
                    {
                        if (date > cel.MoonRise && date < cel.MoonSet)
                        {
                            cel.isMoonUp = true;
                        }
                        else
                        {
                            cel.isMoonUp = false;
                        }
                    }
                    else
                    {
                        if (date > cel.MoonRise || date < cel.MoonSet)
                        {
                            cel.isMoonUp = true;
                        }
                        else
                        {
                            cel.isMoonUp = false;
                        }
                    }
                    break;
                default:
                    //Should never be reached. If reached, previous calculations failed somewhere.
                    break;
            }
        }
        
        /// <summary>
        /// Returns Apogee object containing last and next apogee based on the specified date.
        /// </summary>
        /// <param name="d">DateTime</param>
        /// <returns>Apogee</returns>
        public static Apogee GetApogees(DateTime d)
        {
            return MoonCalc.GetApogeeEvents(d);
        }
        /// <summary>
        /// Returns Perigee object containing last and next perigee based on the specified date.
        /// </summary>
        /// <param name="d">DateTime</param>
        /// <returns>Perigee</returns>
        public static Perigee GetPerigees(DateTime d)
        {
            return MoonCalc.GetPerigeeEvents(d);
        }    
    }

}
