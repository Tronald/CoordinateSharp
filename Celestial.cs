using System;
using System.ComponentModel;
using System.Collections.Generic;
namespace CoordinateSharp
{
    /// <summary>
    /// The main class for Celestial information
    /// </summary>
    /// <remarks>
    /// This class can calculate various pieces of solar and lunar data, based on location and date
    /// </remarks>
    [Serializable]
    public class Celestial : INotifyPropertyChanged
    {

        //When as rise or a set does not occur, the DateTime will return null
        /// <summary>
        /// Creates an empty Celestial object
        /// </summary>
        public Celestial()
        {
            AstrologicalSigns = new AstrologicalSigns();
            LunarEclipse = new LunarEclipse();
            SolarEclipse = new SolarEclipse();
            CalculateCelestialTime(0, 0, new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc));                     
        }
        private Celestial(bool hasCalcs)
        {

            AstrologicalSigns = new AstrologicalSigns();
            LunarEclipse = new LunarEclipse();
            SolarEclipse = new SolarEclipse();
            if (hasCalcs) { CalculateCelestialTime(0, 0, new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)); }
        }
        /// <summary>
        /// Creates a Celestial object based on a geographic lat/long coordinate and specified date
        /// </summary>
        /// <param name="lat">Decimal Format Latitude</param>
        /// <param name="longi">Decimal Format Longitude</param>
        /// <param name="geoDate">Geographic UTC DateTime</param>
        public Celestial(double lat, double longi, DateTime geoDate)
        {
            DateTime d = new DateTime(geoDate.Year, geoDate.Month, geoDate.Day, geoDate.Hour, geoDate.Minute, geoDate.Second, DateTimeKind.Utc);
            AstrologicalSigns = new AstrologicalSigns();
            LunarEclipse = new LunarEclipse();
            SolarEclipse = new SolarEclipse();
            CalculateCelestialTime(lat, longi, d);
        }
        /// <summary>
        /// Creates a Celestial object based on the location and date in the provided Coordinate object
        /// </summary>
        /// <param name="c">Coordinate</param>
        /// <returns>A populated Celestial object</returns>
        public static Celestial LoadCelestial(Coordinate c)
        {
            DateTime geoDate = c.GeoDate;

            DateTime d = new DateTime(geoDate.Year, geoDate.Month, geoDate.Day, geoDate.Hour, geoDate.Minute, geoDate.Second, DateTimeKind.Utc);
            Celestial cel = new Celestial(c.Latitude.ToDouble(), c.Longitude.ToDouble(), c.GeoDate);

            return cel;
        }
      
        /// <summary>
        /// Converts Celestial Object to Local Times
        /// Calls should be made from empty cel objects.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="offset"></param>
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
            cel.SunSet = Get_Correct_Slipped_Date(cel.SunSet, celPre.SunSet, celPost.SunSet, i);
            cel.AdditionalSolarTimes.CivilDusk = Get_Correct_Slipped_Date(cel.AdditionalSolarTimes.CivilDusk, 
                celPre.AdditionalSolarTimes.CivilDusk, celPost.AdditionalSolarTimes.CivilDusk, i);
            cel.AdditionalSolarTimes.NauticalDusk = Get_Correct_Slipped_Date(cel.AdditionalSolarTimes.NauticalDusk,
               celPre.AdditionalSolarTimes.NauticalDusk, celPost.AdditionalSolarTimes.NauticalDusk, i);
            //Get SunRise
            i = Determine_Slipped_Event_Index(cel.SunRise, celPre.SunRise, celPost.SunRise, d);
            cel.SunRise = Get_Correct_Slipped_Date(cel.SunRise, celPre.SunRise, celPost.SunRise, i);
            cel.AdditionalSolarTimes.CivilDawn = Get_Correct_Slipped_Date(cel.AdditionalSolarTimes.CivilDawn,
                celPre.AdditionalSolarTimes.CivilDawn, celPost.AdditionalSolarTimes.CivilDawn, i);
            cel.AdditionalSolarTimes.NauticalDawn = Get_Correct_Slipped_Date(cel.AdditionalSolarTimes.NauticalDawn,
               celPre.AdditionalSolarTimes.NauticalDawn, celPost.AdditionalSolarTimes.NauticalDawn, i);

            //MoonRise
            i = Determine_Slipped_Event_Index(cel.MoonRise, celPre.MoonRise, celPost.MoonRise, d);
            cel.MoonRise = Get_Correct_Slipped_Date(cel.MoonRise, celPre.MoonRise, celPost.MoonRise, i);
        
            //MoonSet
            i = Determine_Slipped_Event_Index(cel.MoonSet, celPre.MoonSet, celPost.MoonSet, d);
            cel.MoonSet = Get_Correct_Slipped_Date(cel.MoonSet, celPre.MoonSet, celPost.MoonSet, i);

            //Local Conditions
            CelestialStatus[] cels = new CelestialStatus[]
            {
                celPre.MoonCondition,cel.MoonCondition,celPost.MoonCondition
            };
            cel.MoonCondition = Celestial.GetStatus(cel.MoonRise, cel.MoonSet, cels);
            cels = new CelestialStatus[]
            {
                celPre.SunCondition, cel.SunCondition, celPost.SunCondition
            };
            cel.SunCondition = Celestial.GetStatus(cel.SunRise, cel.SunSet, cels);
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
            if (this.MoonSet.HasValue) { this.MoonSet = this.MoonSet.Value.AddHours(offset); }
            if (this.MoonRise.HasValue) { this.MoonRise = this.MoonRise.Value.AddHours(offset); }
            //Perigee
            this.Perigee.ConvertTo_Local_Time(offset);
            //Apogee
            this.Apogee.ConvertTo_Local_Time(offset);
            //Eclipse
            this.LunarEclipse.ConvertTo_LocalTime(offset);

            ////Solar
            if (this.SunSet.HasValue) { this.SunSet = this.SunSet.Value.AddHours(offset); }
            if (this.SunRise.HasValue) { this.SunRise = this.SunRise.Value.AddHours(offset); }
            this.AdditionalSolarTimes.Convert_To_Local_Time(offset);

            //Eclipse
            this.SolarEclipse.ConvertTo_LocalTime(offset);
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
            System.Diagnostics.Debug.Print(actual + " " + pre + " " + post);
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
        /// UTC Sunset time
        /// </summary>
        public DateTime? SunSet { get; set; }
        /// <summary>
        /// UTC Sunrise time
        /// </summary>
        public DateTime? SunRise { get; set; }
        /// <summary>
        /// UTC Moonset time
        /// </summary>
        public DateTime? MoonSet { get; set; }
        /// <summary>
        /// UTC Moonrise time
        /// </summary>
        public DateTime? MoonRise { get; set; }
        /// <summary>
        /// Sun azimuth in degrees
        /// </summary>
        public double SunAzimuth { get; set; }
        /// <summary>
        /// Sun altitude in degrees
        /// </summary>
        public double SunAltitude { get; set; }
        /// <summary>
        /// Estimated moon distance from the earth in kilometers
        /// </summary>
        public Distance MoonDistance { get; set; }
        /// <summary>
        /// Sun's Condition based on the provided UTC date
        /// </summary>
        public CelestialStatus SunCondition { get; set; }
        /// <summary>
        /// Moon's condition based on the provided UTC date
        /// </summary>
        public CelestialStatus MoonCondition { get; set; }
        

        /// <summary>
        /// Moon ilumination details based on the provided UTC date
        /// </summary>
        /// <remarks>
        /// Contains phase, phase name, fraction and angle
        /// </remarks>
        public MoonIllum MoonIllum { get; set; }
        /// <summary>
        /// Moons perigee details based on the provided UTC date.
        /// </summary>
        public Perigee Perigee { get; set; }
        /// <summary>
        /// Moons apogee details based on the provided UTC date.
        /// </summary>
        public Apogee Apogee { get; set; }

        /// <summary>
        /// Additional solar event times based on the provided UTC date and location
        /// </summary>
        /// <remarks>Contains civil and nautical dawn and dusk times.</remarks>
        public AdditionalSolarTimes AdditionalSolarTimes { get; set; }
        /// <summary>
        /// Astrological signs based on the provided UTC date
        /// </summary>
        /// <remarks>
        /// Contains zodiac, moon sign and moon name during full moon events
        /// </remarks>
        public AstrologicalSigns AstrologicalSigns { get; set; }

        /// <summary>
        /// Returns a SolarEclipse object
        /// </summary>
        public SolarEclipse SolarEclipse { get; set; }
        /// <summary>
        /// Returns a LunarEclipse object
        /// </summary>
        public LunarEclipse LunarEclipse { get; set; }

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

            SunCalc.CalculateAdditionSolarTimes(date, longi, lat, this);

            this.Perigee = MoonCalc.GetPerigeeEvents(date);
            this.Apogee = MoonCalc.GetApogeeEvents(date);

        }
        /// <summary>
        /// Calculate celestial data based on lat/long and utc date
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
            SunCalc.CalculateAdditionSolarTimes(date, longi, lat, c);

            c.Perigee = MoonCalc.GetPerigeeEvents(date);
            c.Apogee = MoonCalc.GetApogeeEvents(date);

            return c;
        }
        /// <summary>
        /// Calculate sun data based on lat/long and date
        /// </summary>
        /// <param name="lat">Decimal format latitude</param>
        /// <param name="longi">Decimal format longitude</param>
        /// <param name="date">Geographic DateTime</param>
        /// <returns>Partially populated Celestial object</returns>
        public static Celestial CalculateSunData(double lat, double longi, DateTime date)
        {
            date = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, DateTimeKind.Utc);

            Celestial c = new Celestial(false);
            SunCalc.CalculateSunTime(lat, longi, date, c);
            SunCalc.CalculateZodiacSign(date, c);

            SunCalc.CalculateAdditionSolarTimes(date, longi, lat, c);

            return c;
        }
        /// <summary>
        /// Calculate moon data based on lat/long and date
        /// </summary>
        /// <param name="lat">Decimal format latitude</param>
        /// <param name="longi">Decimal format longitude</param>
        /// <param name="date">Geographic DateTime</param>
        /// <returns>Partially populated Celestial object</returns>
        public static Celestial CalculateMoonData(double lat, double longi, DateTime date)
        {
            date = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, DateTimeKind.Utc);

            Celestial c = new Celestial(false);

            MoonCalc.GetMoonTimes(date, lat, longi, c);
            MoonCalc.GetMoonDistance(date, c);
            MoonCalc.GetMoonSign(date, c);
            MoonCalc.GetMoonIllumination(date, c,lat,longi);

            c.Perigee = MoonCalc.GetPerigeeEvents(date);
            c.Apogee = MoonCalc.GetApogeeEvents(date);

            return c;
        }
        /// <summary>
        /// Returns a List containing solar eclipse data for the century.
        /// Century return is based on the date passed.
        /// </summary>
        /// <param name="lat">Latitude</param>
        /// <param name="longi">Longitude</param>
        /// <param name="date">Date</param>
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
        /// <param name="lat">Latitude</param>
        /// <param name="longi">Longitude</param>
        /// <param name="date">Date</param>
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
        /// Returns Apogee object containing last and next apogee based on the specified date.
        /// </summary>
        /// <param name="d">Date</param>
        /// <returns>Apogee</returns>
        public static Apogee GetApogees(DateTime d)
        {
            return MoonCalc.GetApogeeEvents(d);
        }
        /// <summary>
        /// Returns Perigee object containing last and next perigee based on the specified date.
        /// </summary>
        /// <param name="d">Date</param>
        /// <returns>Perigee</returns>
        public static Perigee GetPerigees(DateTime d)
        {
            return MoonCalc.GetPerigeeEvents(d);
        }
        /// <summary>
        /// Property changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notify property changed
        /// </summary>
        /// <param name="propName">Property name</param>
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }

}
