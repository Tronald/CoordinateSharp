using System;
using System.Collections.Generic;
using System.IO;
using CoordinateSharp;
namespace CoordinateSharp_TestProj
{
    public class CelestialTests
    {
        public void Populate_CelestialTests()
        {
            //Test will run against N39, W72
            //Sun Times and Moon Times will range for 1 Mar-2018 to 30-Mar-2018
            //Sun/Moon Alt, Az, pergiee, apogee eclispes, fraction and distance with be tested against 15-Mar-2018 UTC
            Coordinate c = new Coordinate(39, -72, new DateTime(2018, 03, 15));
            SunAlts = new List<double>();
            SunAzs = new List<double>();
            MoonAlts = new List<double>();
            MoonAzs = new List<double>();
            MoonAlts = new List<double>();
            MoonDistances = new List<double>();
            MoonFraction = new List<double>();
            MoonPhase = new List<double>();
            MoonPhaseName = new List<string>();

            for (int x = 0; x < 144; x++)
            {
                if (x != 0)
                {
                    c.GeoDate = c.GeoDate.AddMinutes(10);
                }
                SunAlts.Add(c.CelestialInfo.SunAltitude);
                SunAzs.Add(c.CelestialInfo.SunAzimuth);
                MoonAlts.Add(c.CelestialInfo.MoonAltitude);
                MoonAzs.Add(c.CelestialInfo.MoonAzimuth);
                MoonFraction.Add(c.CelestialInfo.MoonIllum.Fraction);
               
            }

            c.GeoDate = new DateTime(2018, 3, 1);

            SunRises = new List<DateTime?>();
            SunSets = new List<DateTime?>();
            MoonRises = new List<DateTime?>();
            MoonSets = new List<DateTime?>();

            CivilDawn = new List<DateTime?>();
            CivilDusk = new List<DateTime?>();
            NauticalDawn = new List<DateTime?>();
            NauticalDusk = new List<DateTime?>();
            AstroDawn = new List<DateTime?>();
            AstroDusk = new List<DateTime?>();
            BottomSolarDiscRise = new List<DateTime?>();
            BottomSolarDiscSet = new List<DateTime?>();
           
            for (int x = 0; x < 31; x++)
            {
                if (x != 0)
                {
                    c.GeoDate = c.GeoDate.AddDays(1);
                }
                SunRises.Add(c.CelestialInfo.SunRise);
                SunSets.Add(c.CelestialInfo.SunSet);
                MoonRises.Add(c.CelestialInfo.MoonRise);
                MoonSets.Add(c.CelestialInfo.MoonSet);

                MoonDistances.Add(c.CelestialInfo.MoonDistance.Kilometers);
                CivilDawn.Add(c.CelestialInfo.AdditionalSolarTimes.CivilDawn);
                CivilDusk.Add(c.CelestialInfo.AdditionalSolarTimes.CivilDusk);
                NauticalDawn.Add(c.CelestialInfo.AdditionalSolarTimes.NauticalDawn);
                NauticalDusk.Add(c.CelestialInfo.AdditionalSolarTimes.NauticalDusk);
                AstroDawn.Add(c.CelestialInfo.AdditionalSolarTimes.AstronomicalDawn);
                AstroDusk.Add(c.CelestialInfo.AdditionalSolarTimes.AstronomicalDusk);
                BottomSolarDiscRise.Add(c.CelestialInfo.AdditionalSolarTimes.SunriseBottomDisc);
                BottomSolarDiscSet.Add(c.CelestialInfo.AdditionalSolarTimes.SunsetBottomDisc);

                MoonPhase.Add(c.CelestialInfo.MoonIllum.Phase);
                MoonPhaseName.Add(c.CelestialInfo.MoonIllum.PhaseName);
            }

            //Set Dates and Finish
            this.SolarEclispe = new List<DateTime>();
            this.LunarEclispe = new List<DateTime>();
            this.Perigee = new List<DateTime>();
            this.Apogee = new List<DateTime>();

        }

        public bool Check_Values(object prop, string file)
        {
            string[] lines = File.ReadAllLines(file);

            if (prop.GetType() == typeof(List<DateTime?>))
            {
                List<DateTime?> d = (List<DateTime?>)prop;
                for (int x = 0; x < lines.Length; x++)
                {
                    DateTime date;
                    if(lines[x] != "*")
                    {
                        if (DateTime.TryParse(lines[x], out date))
                        {
                            TimeSpan span = date - d[x].Value;
                            if(Math.Abs(span.TotalMinutes)> 1)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }                 
                    }
                    else
                    {
                        if (!d[x].HasValue) { return false; }
                    }
                }
            }
            if (prop.GetType() == typeof(List<double>))
            {
                List<double?> d = (List<double?>)prop;
                for (int x = 0; x < lines.Length; x++)
                {
                }
            }
            if (prop.GetType() == typeof(List<string>))
            {
                List<DateTime?> d = (List<DateTime?>)prop;
                for (int x = 0; x < lines.Length; x++)
                {
                }
            }
            return true;
        }
        public List<DateTime?> SunRises { get; set; }
        public List<DateTime?> MoonRises { get; set; }
        public List<DateTime?> SunSets { get; set; }
        public List<DateTime?> MoonSets { get; set; }

        public List<DateTime?> CivilDawn { get; set; }
        public List<DateTime?> CivilDusk { get; set; }
        public List<DateTime?> NauticalDawn { get; set; }
        public List<DateTime?> NauticalDusk { get; set; }
        public List<DateTime?> AstroDawn { get; set; }
        public List<DateTime?> AstroDusk { get; set; }
        public List<DateTime?> BottomSolarDiscRise { get; set; }
        public List<DateTime?> BottomSolarDiscSet { get; set; }

        public List<double> SunAlts { get; set; }
        public List<double> SunAzs { get; set; }
        public List<double> MoonAlts { get; set; }
        public List<double> MoonAzs { get; set; }
        public List<double> MoonDistances { get; set; }
        public List<double> MoonFraction{ get; set; }
        public List<double> MoonPhase { get; set; }
        public List<string> MoonPhaseName { get; set; }

        public List<DateTime> SolarEclispe { get; set; }
        public List<DateTime> LunarEclispe { get; set; }
        public List<DateTime> Perigee { get; set; }
        public List<DateTime> Apogee { get; set; }
    }
}
