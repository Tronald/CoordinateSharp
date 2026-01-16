// Ignore Spelling: Azs Astro

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CoordinateSharp;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using Newtonsoft.Json;
using System.Threading.Tasks;
using CoordinateSharp.UnitTests;
using System.Diagnostics;

namespace CoordinateSharp_UnitTests
{
    [TestClass]
    public class CelestialSolar
    {

        Solar_Data data = new Solar_Data();

        public CelestialSolar()
        {
            //Test will run against N39, W72
            //Sun Times and Moon Times will range for 1 Mar-2018 to 30-Mar-2018
            //Sun/Moon Alt, Az, Perigee, Apogee, Eclipses, fraction and distance with be tested against 15-Mar-2018 UTC
            Coordinate c = new Coordinate(39, -72, new DateTime(2018, 03, 15));
            data.SunAlts = new List<double>();
            data.SunAzs = new List<double>();


            for (int x = 0; x < 144; x++)
            {
                if (x != 0)
                {
                    c.GeoDate = c.GeoDate.AddMinutes(10);
                }
                data.SunAlts.Add(c.CelestialInfo.SunAltitude);
                data.SunAzs.Add(c.CelestialInfo.SunAzimuth);


            }

            c.GeoDate = new DateTime(2018, 3, 1);

            data.SunRises = new List<DateTime?>();
            data.SunSets = new List<DateTime?>();

            data.CivilDawn = new List<DateTime?>();
            data.CivilDusk = new List<DateTime?>();
            data.NauticalDawn = new List<DateTime?>();
            data.NauticalDusk = new List<DateTime?>();
            data.AstroDawn = new List<DateTime?>();
            data.AstroDusk = new List<DateTime?>();
            data.BottomSolarDiscRise = new List<DateTime?>();
            data.BottomSolarDiscSet = new List<DateTime?>();

            for (int x = 0; x < 31; x++)
            {
                if (x != 0)
                {
                    c.GeoDate = c.GeoDate.AddDays(1);
                }
                data.SunRises.Add(c.CelestialInfo.SunRise);
                data.SunSets.Add(c.CelestialInfo.SunSet);



                data.CivilDawn.Add(c.CelestialInfo.AdditionalSolarTimes.CivilDawn);
                data.CivilDusk.Add(c.CelestialInfo.AdditionalSolarTimes.CivilDusk);
                data.NauticalDawn.Add(c.CelestialInfo.AdditionalSolarTimes.NauticalDawn);
                data.NauticalDusk.Add(c.CelestialInfo.AdditionalSolarTimes.NauticalDusk);
                data.AstroDawn.Add(c.CelestialInfo.AdditionalSolarTimes.AstronomicalDawn);
                data.AstroDusk.Add(c.CelestialInfo.AdditionalSolarTimes.AstronomicalDusk);
                data.BottomSolarDiscRise.Add(c.CelestialInfo.AdditionalSolarTimes.SunriseBottomDisc);
                data.BottomSolarDiscSet.Add(c.CelestialInfo.AdditionalSolarTimes.SunsetBottomDisc);

            }

            //Set Dates and Finish
            //THESE OBJECT ARE TESTED AGAINST SERIALIZED OBJECTS.
            //IF CHANGING THE MODEL YOU WILL HAVE TO CHANGE THE OBJECTS THEY ARE TESTED AGAINST AS WELL

            data.SolarEclipse = c.CelestialInfo.SolarEclipse;

        }
        /// <summary>
        /// Ensures solstice/equinox accuracy is within error limits (100 seconds maximum delta).
        /// Tested together due to algorithm configuration of values.
        /// Table 27A Accurate below 1000AD
        /// </summary>
        [TestMethod]
        public void Solstice_Accuracy_Test_27A()
        {
            Coordinate c = new Coordinate(45, 75, new DateTime(650, 1, 1));
            int delta = 100;


            //1000AD-(SECONDS NOT SPECIFIED IN DATA)
            var ts = new DateTime(650, 3, 18, 4, 55, 0) - c.CelestialInfo.Equinoxes.Spring;
            Assert.AreEqual(0, ts.TotalSeconds, delta);
            //Summer
            ts = new DateTime(650, 6, 19, 21, 36, 0) - c.CelestialInfo.Solstices.Summer;
            Assert.AreEqual(0, ts.TotalSeconds, delta);
            //Fall
            ts = new DateTime(650, 9, 20, 20, 55, 0) - c.CelestialInfo.Equinoxes.Fall;
            Assert.AreEqual(0, ts.TotalSeconds, delta);
            //Winter
            ts = new DateTime(650, 12, 18, 18, 37, 0) - c.CelestialInfo.Solstices.Winter;
            Assert.AreEqual(0, ts.TotalSeconds, delta);
        }

        [TestMethod]
        public void Sunset()
        {
            Check_Values(data.SunSets, "CelestialData\\SunSet.txt");
        }
        [TestMethod]
        public void Sunrise()
        {
            Check_Values(data.SunRises, "CelestialData\\SunRise.txt");
        }
        [TestMethod]
        public void AstroDawn()
        {
            Check_Values(data.AstroDawn, "CelestialData\\AstroDawn.txt");
        }
        [TestMethod]
        public void AstroDusk()
        {
            Check_Values(data.AstroDusk, "CelestialData\\AstroDusk.txt");
        }
        [TestMethod]
        public void CivilDawn()
        {
            Check_Values(data.CivilDawn, "CelestialData\\CivilDawn.txt");
        }
        [TestMethod]
        public void CivilDusk()
        {
            Check_Values(data.CivilDusk, "CelestialData\\CivilDusk.txt");
        }
        [TestMethod]
        public void NauticalDawn()
        {
            Check_Values(data.NauticalDawn, "CelestialData\\NauticalDawn.txt");
        }
        [TestMethod]
        public void NauticalDusk()
        {
            Check_Values(data.NauticalDusk, "CelestialData\\NauticalDusk.txt");
        }
        [TestMethod]
        public void BottomSolarDiscRise()
        {
            Check_Values(data.BottomSolarDiscRise, "CelestialData\\BottomDiscRise.txt");
        }
        [TestMethod]
        public void BottomSolarDiscSet()
        {
            Check_Values(data.BottomSolarDiscSet, "CelestialData\\BottomDiscSet.txt");
        }
        [TestMethod]
        public void SunAltitude()
        {
            Check_Values(data.SunAlts, "CelestialData\\SunAlts.txt",.3);
        }
        [TestMethod]
        public void SunAzimuth()
        {
            Check_Values(data.SunAzs, "CelestialData\\SunAzs.txt",.5);
        }

        [TestMethod]
        public void SolarEclipses()
        {
            string json = File.ReadAllText("CelestialData\\Coordinate.txt");

            Coordinate c = JsonConvert.DeserializeObject<Coordinate>(json);
            SolarEclipse ev = c.CelestialInfo.SolarEclipse;
            SolarEclipseDetails lE1 = ev.LastEclipse;
            SolarEclipseDetails nE1 = ev.NextEclipse;
            SolarEclipseDetails lE2 = data.SolarEclipse.LastEclipse;
            SolarEclipseDetails nE2 = data.SolarEclipse.NextEclipse;

            PropertyInfo[] properties = typeof(SolarEclipseDetails).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var l1 = property.GetValue(lE1);
                var l2 = property.GetValue(lE2);
                var n1 = property.GetValue(nE1);
                var n2 = property.GetValue(nE2);

                Assert.AreEqual(l1.ToString(), l2.ToString(), "Last Eclipse data does not match.");
                Assert.AreEqual(n1.ToString(), n2.ToString(), "Next Eclipse data does not match.");
            }
        }
        [TestMethod]
        public void SolarEclipses_Detailed()
        {
            //Ensures test data matches calculation at test data date, location and TZ offset.           
            string json;
            List<SolarEclipseTestModel> eclipses;

            for (int x = 0; x <= 29; x++)
            {
                //All tests are ran at coord 42, -112, TZ offset -7
                string d = x.ToString("D2");
                json = File.ReadAllText($"CelestialData\\Solar Eclipse\\42N 112W TZ-7\\SE{d}01.json");
                eclipses = JsonConvert.DeserializeObject<List<SolarEclipseTestModel>>(json);

                foreach (var expected in eclipses)
                {
                    Coordinate c = new Coordinate(42, -112, expected.Date.AddDays(-1), new EagerLoad(EagerLoadType.Celestial));
                    c.Offset = -7;
                    SolarEclipseDetails actual = c.CelestialInfo.SolarEclipse.NextEclipse;

                    SolarEclipseValueCheck(expected, actual);
                }
            }

            //Region integrity check
            json = File.ReadAllText($"CelestialData\\Solar Eclipse\\32S 80E TZ6\\SE2001.json");
            eclipses = JsonConvert.DeserializeObject<List<SolarEclipseTestModel>>(json);
            foreach (var expected in eclipses)
            {
                Coordinate c = new Coordinate(-32, 80, expected.Date.AddDays(-1), new EagerLoad(EagerLoadType.Celestial));
                c.Offset = 6;
                SolarEclipseDetails actual = c.CelestialInfo.SolarEclipse.NextEclipse;
                SolarEclipseValueCheck(expected, actual);
            }

            json = File.ReadAllText($"CelestialData\\Solar Eclipse\\58N 68E TZ0\\SE2001.json");
            eclipses = JsonConvert.DeserializeObject<List<SolarEclipseTestModel>>(json);
            foreach (var expected in eclipses)
            {
                Coordinate c = new Coordinate(58, 68, expected.Date.AddDays(-1), new EagerLoad(EagerLoadType.Celestial));
              
                SolarEclipseDetails actual = c.CelestialInfo.SolarEclipse.NextEclipse;
                SolarEclipseValueCheck(expected, actual);
            }

        }
        public void SolarEclipseValueCheck(SolarEclipseTestModel expected, SolarEclipseDetails actual)
        {
            TimeSpan ts = expected.Date - actual.Date;

            Assert.AreEqual(ts.TotalDays, 0);
            Assert.AreEqual((expected.PartialEclipseBegin - actual.PartialEclipseBegin).TotalMinutes, 0, 1);
            Assert.AreEqual((expected.PartialEclipseEnd - actual.PartialEclipseEnd).TotalMinutes, 0, 1);
            Assert.AreEqual((expected.AorTEclipseBegin - actual.AorTEclipseBegin).TotalMinutes, 0, 1);
            Assert.AreEqual((expected.AorTEclipseEnd - actual.AorTEclipseEnd).TotalMinutes, 0, 1);
            Assert.AreEqual(expected.Magnitude, actual.Magnitude, .002);
            Assert.AreEqual((expected.MaximumEclipse - actual.MaximumEclipse).TotalMinutes, 0, 1);
            Assert.AreEqual(expected.Coverage, actual.Coverage, .002);
            Assert.AreEqual(expected.Type, actual.Type);

            TimeSpan expectedTrimmed = new TimeSpan(0, expected.AorTDuration.Hours, expected.AorTDuration.Minutes, 0);
            TimeSpan actualTrimmed = new TimeSpan(0, actual.AorTDuration.Hours, actual.AorTDuration.Minutes, 0);

            Assert.AreEqual((expectedTrimmed - actualTrimmed).Minutes, 0, 1);
        }

        [TestMethod]
        public void SolarEclipses_LeapYearIntegrityCheck()
        {
            EagerLoad el = new EagerLoad(EagerLoadType.Celestial);
            el.Extensions = new EagerLoad_Extensions(EagerLoad_ExtensionsType.Solar_Eclipse | EagerLoad_ExtensionsType.Lunar_Eclipse);

            DateTime eclipseDate = new DateTime(2351, 1, 1, 0, 1, 0);

            Coordinate c = new Coordinate(36.162603, -86.799343, eclipseDate, el);//Will throw if failed
        }
        [TestMethod]
        public void IsSunUp()
        {
            string[] lines = File.ReadAllLines("CelestialData\\IsSunUp.txt");
            foreach (string line in lines)
            {
                string[] split = line.Split(',');
                double lat = double.Parse(split[0]);
                double longi = double.Parse(split[1]);
                DateTime geoDate = DateTime.Parse(split[2]).Date;
                string sR = split[3];
                string SS = split[4];
                string condition = split[5];
                EagerLoad el = new EagerLoad(EagerLoadType.Celestial);
                el.Extensions = new EagerLoad_Extensions(EagerLoad_ExtensionsType.Solar_Cycle);
                Coordinate c = new Coordinate(lat, longi, geoDate, el);
                //Iterate each minute in the day
                for (int x = 0; x < 1440; x++)
                {
                    int i = 1;
                    if (x == 0) { i = 0; }
                    switch (condition)
                    {
                        case "DownAllDay":
                            c.GeoDate = c.GeoDate.AddMinutes(i);
                            Assert.AreNotEqual(true, c.CelestialInfo.IsSunUp, $"Down All Day Expected on Iteration {x + 1}");
                            break;
                        case "UpAllDay":
                            c.GeoDate = c.GeoDate.AddMinutes(i);
                            Assert.AreNotEqual(false, c.CelestialInfo.IsSunUp, $"Up All Day Expected on Iteration {x + 1}");
                            break;
                        case "NoRise":
                            c.GeoDate = c.GeoDate.AddMinutes(i);
                            if (c.CelestialInfo.SunSet > c.GeoDate && c.CelestialInfo.IsSunUp == false) { Assert.Fail($"NoRise Sunset time is greater than geodate while sun is down at iteration {x + 1}"); }
                            if (c.CelestialInfo.SunSet <= c.GeoDate && c.CelestialInfo.IsSunUp == true) { Assert.Fail($"NoRise Sunset time is less than geodate while sun is up at iteration {x + 1}"); }
                            break;
                        case "NoSet":
                            c.GeoDate = c.GeoDate.AddMinutes(i);
                            if (c.CelestialInfo.SunRise >= c.GeoDate && c.CelestialInfo.IsSunUp == true) { Assert.Fail($"NoSet SunRise time is greater than geodate while sun is up at iteration {x + 1}"); }
                            if (c.CelestialInfo.SunRise < c.GeoDate && c.CelestialInfo.IsSunUp == false) { Assert.Fail($"NoSet SunRise time is less than geodate while sun is down at iteration {x + 1}"); }
                            break;
                        case "RiseAndSet":
                            c.GeoDate = c.GeoDate.AddMinutes(i);
                            //When working in Z time we have to account for rise occuring after set
                            if (c.CelestialInfo.SunSet > c.CelestialInfo.SunRise)
                            {
                                //SUN SHOULD BE UP
                                if (c.GeoDate > c.CelestialInfo.SunRise && c.GeoDate < c.CelestialInfo.SunSet && c.CelestialInfo.IsSunUp == false) { Assert.Fail($"Sun up expected {x + 1}"); }
                                //SUN SHOULD BE DOWN
                                if ((c.GeoDate <= c.CelestialInfo.SunRise || c.GeoDate >= c.CelestialInfo.SunSet) && c.CelestialInfo.IsSunUp == true) { Assert.Fail($"Sun down expected {x + 1}"); }
                            }
                            else
                            {
                                //AFTER RISE SUN SHOULD BE UP
                                if (c.GeoDate > c.CelestialInfo.SunRise && c.CelestialInfo.IsSunUp == false) { Assert.Fail($"Sun up expected after rise {x + 1}"); }
                                //BETWEEN SET AND RISE SUN SHOULD BE DOWN
                                if (c.GeoDate <= c.CelestialInfo.SunRise && c.GeoDate >= c.CelestialInfo.SunSet && c.CelestialInfo.IsSunUp == true) { Assert.Fail($"Sun down between set and rise expected {x + 1}"); }
                                //BEFORE SET SUN SHOULD BE UP
                                if (c.GeoDate < c.CelestialInfo.SunSet && c.CelestialInfo.IsSunUp == false) { Assert.Fail($"Sun up before set expected {x + 1}"); }
                            }

                            break;
                        default:
                            break;
                    }
                }

            }

        }
        [TestMethod]
        public void SunCondition_NoEventCheck()
        {
            Coordinate c = new Coordinate(69.68331, 9.955951, new DateTime(2026, 1, 14, 18, 48, 0));
            SunCondition_NoEventCheck(c, CelestialStatus.UpAllDay);

            c = new Coordinate(69.716046, 11.373193, new DateTime(2026, 1, 14, 18, 48, 0));
            SunCondition_NoEventCheck(c, CelestialStatus.UpAllDay);

            c = new Coordinate(69.718731, 18.484865, new DateTime(2026, 1, 14, 18, 48, 0));
            SunCondition_NoEventCheck(c, CelestialStatus.UpAllDay);

            c = new Coordinate(69.687064, 19.902523, new DateTime(2026, 1, 14, 18, 48, 0));
            SunCondition_NoEventCheck(c, CelestialStatus.UpAllDay);

            c = new Coordinate(-69.68331, -9.955951, new DateTime(2026, 1, 14, 18, 48, 0));
            SunCondition_NoEventCheck(c, CelestialStatus.DownAllDay);

            c = new Coordinate(-69.716046, -11.373193, new DateTime(2026, 1, 14, 18, 48, 0));
            SunCondition_NoEventCheck(c, CelestialStatus.DownAllDay);

            c = new Coordinate(-69.718731, -18.484865, new DateTime(2026, 1, 14, 18, 48, 0));
            SunCondition_NoEventCheck(c, CelestialStatus.DownAllDay);

            c = new Coordinate(-69.687064, -19.902523, new DateTime(2026, 1, 14, 18, 48, 0));
            SunCondition_NoEventCheck(c, CelestialStatus.DownAllDay);

        }
        private void SunCondition_NoEventCheck(Coordinate c, CelestialStatus status)
        {
            while (c.GeoDate < new DateTime(2026, 1, 17))
            {
                c.GeoDate = c.GeoDate.AddHours(.1);
                if (c.CelestialInfo.SunCondition == status)
                {
                    Console.WriteLine(c.GeoDate.ToString());
                    Console.WriteLine(c.CelestialInfo.SunAltitude);
                    throw new InvalidOperationException();
                }
            }
        }
        /// <summary>
        /// Ensures static celestial logic works correctly.
        /// </summary>
        [TestMethod]
        public void Static_Checks()
        {
            DateTime d = new DateTime(2019, 2, 6);
            Coordinate c = new Coordinate(40.0352, -74.5844, d);
            DateTime val;


            //Method 1 UTC
            val = Celestial.Get_Next_SunRise(c);
            Assert.AreEqual(val.ToString(), "2/6/2019 12:03:33 PM");

            val = Celestial.Get_Last_SunRise(c);
            Assert.AreEqual(val.ToString(), "2/5/2019 12:04:35 PM");

            val = Celestial.Get_Next_SunSet(c);
            Assert.AreEqual(val.ToString(), "2/6/2019 10:23:54 PM");

            val = Celestial.Get_Last_SunSet(c);
            Assert.AreEqual(val.ToString(), "2/5/2019 10:22:41 PM");



            //Method 2 UTC
            val = Celestial.Get_Next_SunRise(40.0352, -74.5844, d);
            Assert.AreEqual(val.ToString(), "2/6/2019 12:03:33 PM");

            val = Celestial.Get_Last_SunRise(40.0352, -74.5844, d);
            Assert.AreEqual(val.ToString(), "2/5/2019 12:04:35 PM");

            val = Celestial.Get_Next_SunSet(40.0352, -74.5844, d);
            Assert.AreEqual(val.ToString(), "2/6/2019 10:23:54 PM");

            val = Celestial.Get_Last_SunSet(40.0352, -74.5844, d);
            Assert.AreEqual(val.ToString(), "2/5/2019 10:22:41 PM");



            //Method 3 LOCAL TIMES
            val = Celestial.Get_Next_SunRise(40.0352, -74.5844, d, -4);
            Assert.AreEqual(val.ToString(), "2/6/2019 8:03:33 AM");

            val = Celestial.Get_Last_SunRise(40.0352, -74.5844, d, -4);
            Assert.AreEqual(val.ToString(), "2/5/2019 8:04:35 AM");

            val = Celestial.Get_Next_SunSet(40.0352, -74.5844, d, -4);
            Assert.AreEqual(val.ToString(), "2/6/2019 6:23:54 PM");

            val = Celestial.Get_Last_SunSet(40.0352, -74.5844, d, -4);
            Assert.AreEqual(val.ToString(), "2/5/2019 6:22:41 PM");



        }

        [TestMethod]
        public void Local_Time()
        {

            DateTime d = new DateTime(2019, 10, 31, 14, 10, 22);
            double offset = -7;

            //INSTANCE CHECK

            //Coordinate in UTC
            Coordinate cUTC = new Coordinate(47.60357, -122.32945, d);
            //Coordinate in UTC - offset added to account for UTC date differing
            Coordinate sUTC = new Coordinate(47.60357, -122.32945, d.AddDays(offset / Math.Abs(offset) * -1));
            //Coordinate in Local
            Coordinate lLoc = new Coordinate(47.60357, -122.32945, d);
            //Coordinate in local + offset (for cel coord comparision)
            Coordinate simUTC = new Coordinate(47.60357, -122.32945, d.AddHours(-offset));
            lLoc.Offset = offset;


            Celestial cCel = cUTC.CelestialInfo;
            Celestial sCel = sUTC.CelestialInfo;
            Celestial lCel = lLoc.CelestialInfo;
            Celestial bCel = simUTC.CelestialInfo;

            Local_Time_Checker(cCel, lCel, sCel, bCel, offset);

            //STATIC CHECK

            cCel = Celestial.CalculateCelestialTimes(47.60357, -122.32945, d);
            sCel = Celestial.CalculateCelestialTimes(47.60357, -122.32945, d.AddDays(offset / Math.Abs(offset) * -1));
            lCel = Celestial.CalculateCelestialTimes(47.60357, -122.32945, d, offset);
            bCel = Celestial.CalculateCelestialTimes(47.60357, -122.32945, d.AddHours(-offset));

            Local_Time_Checker(cCel, lCel, sCel, bCel, offset);


            //With EagerLoad
            EagerLoad el = new EagerLoad(EagerLoadType.Celestial);
            cCel = Celestial.CalculateCelestialTimes(47.60357, -122.32945, d, el);
            sCel = Celestial.CalculateCelestialTimes(47.60357, -122.32945, d.AddDays(offset / Math.Abs(offset) * -1), el);
            lCel = Celestial.CalculateCelestialTimes(47.60357, -122.32945, d, el, offset);
            bCel = Celestial.CalculateCelestialTimes(47.60357, -122.32945, d.AddHours(-offset), el);

            Local_Time_Checker(cCel, lCel, sCel, bCel, offset);

            //VALIDATIONS
            //IN RANGE
            cUTC.Offset = -12;
            cUTC.Offset = 12;
            cCel = new Celestial(0, 0, DateTime.Now, -12);
            cCel = new Celestial(0, 0, DateTime.Now, 14);
            cCel = new Celestial(0, 0, DateTime.Now, -12, new EagerLoad());
            cCel = new Celestial(0, 0, DateTime.Now, 14, new EagerLoad());
            cCel = Celestial.CalculateCelestialTimes(0, 0, DateTime.Now, -12);
            cCel = Celestial.CalculateCelestialTimes(0, 0, DateTime.Now, 14);
            cCel = Celestial.CalculateCelestialTimes(0, 0, DateTime.Now, new EagerLoad(), 14);
            cCel = Celestial.CalculateCelestialTimes(0, 0, DateTime.Now, new EagerLoad(), -12);
            //OUT OF RANGE
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { cUTC.Offset = -13; });
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { cUTC.Offset = 15; });
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { cCel = new Celestial(0, 0, DateTime.Now, -13); });
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { cCel = new Celestial(0, 0, DateTime.Now, 15); });
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { cCel = new Celestial(0, 0, DateTime.Now, -13, new EagerLoad()); });
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { cCel = new Celestial(0, 0, DateTime.Now, 15, new EagerLoad()); });
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { cCel = Celestial.CalculateCelestialTimes(0, 0, DateTime.Now, -13); });
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { cCel = Celestial.CalculateCelestialTimes(0, 0, DateTime.Now, 15); });
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { cCel = Celestial.CalculateCelestialTimes(0, 0, DateTime.Now, new EagerLoad(), -13); });
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { cCel = Celestial.CalculateCelestialTimes(0, 0, DateTime.Now, new EagerLoad(), 15); });

        }

        private void Local_Time_Checker(Celestial cCel, Celestial lCel, Celestial sCel, Celestial bCel, double offset)
        {
            //SOLAR          
            Time_Compare(cCel.SunRise, lCel.SunRise, sCel.SunRise, offset);
            Time_Compare(cCel.SunSet, lCel.SunSet, sCel.SunSet, offset);
            Time_Compare(cCel.AdditionalSolarTimes.CivilDawn, lCel.AdditionalSolarTimes.CivilDawn, sCel.AdditionalSolarTimes.CivilDawn, offset);
            Time_Compare(cCel.AdditionalSolarTimes.CivilDusk, lCel.AdditionalSolarTimes.CivilDusk, sCel.AdditionalSolarTimes.CivilDusk, offset);
            Time_Compare(cCel.AdditionalSolarTimes.NauticalDawn, lCel.AdditionalSolarTimes.NauticalDawn, sCel.AdditionalSolarTimes.NauticalDawn, offset);
            Time_Compare(cCel.AdditionalSolarTimes.NauticalDusk, lCel.AdditionalSolarTimes.NauticalDusk, sCel.AdditionalSolarTimes.NauticalDusk, offset);
            Time_Compare(cCel.AdditionalSolarTimes.AstronomicalDawn, lCel.AdditionalSolarTimes.AstronomicalDawn, sCel.AdditionalSolarTimes.AstronomicalDawn, offset);
            Time_Compare(cCel.AdditionalSolarTimes.AstronomicalDusk, lCel.AdditionalSolarTimes.AstronomicalDusk, sCel.AdditionalSolarTimes.AstronomicalDusk, offset);
            Time_Compare(cCel.SolarEclipse.LastEclipse.MaximumEclipse, lCel.SolarEclipse.LastEclipse.MaximumEclipse, sCel.SolarEclipse.LastEclipse.MaximumEclipse, offset);
            Time_Compare(cCel.SolarEclipse.NextEclipse.MaximumEclipse, lCel.SolarEclipse.NextEclipse.MaximumEclipse, sCel.SolarEclipse.NextEclipse.MaximumEclipse, offset);

            Assert.AreEqual(0, lCel.SunAltitude - bCel.SunAltitude, .00001);
            Assert.AreEqual(0, lCel.SunAzimuth - bCel.SunAzimuth, .00001);
            Assert.AreEqual(0, lCel.SunAzimuth - bCel.SunAzimuth, .00001);

            Assert.AreEqual(lCel.IsSunUp, bCel.IsSunUp);
            Assert.AreEqual(lCel.SunCondition, bCel.SunCondition);
        }
        private void Time_Compare(DateTime? utc, DateTime? local, DateTime? backup, double offset)
        {
            //Cannot run tests with null dates
            if (utc == null || local == null || backup == null) { Assert.Fail("Null value not expected"); }

            TimeSpan ts = local.Value - utc.Value.AddHours(offset);
            //5ms float point change account
            if (Math.Abs(ts.TotalMilliseconds) > 5)
            {
                ts = local.Value - backup.Value.AddHours(offset);
                Assert.AreEqual(0, ts.TotalMilliseconds, 5);
            }

        }

        private void Check_Values(object prop, string file, double doubleDelta=0)
        {
            string[] lines = File.ReadAllLines(file);

            if (prop.GetType() == typeof(List<DateTime?>))
            {
                List<DateTime?> d = (List<DateTime?>)prop;
                for (int x = 0; x < lines.Length; x++)
                {

                    if (lines[x] != "*")
                    {

                        DateTime date = DateTime.Parse(lines[x]);
                        TimeSpan span = date - d[x].Value;
                        Assert.AreEqual(0, span.TotalMinutes, 1, $"DateTime exceeded delta at iteration {x + 1}");
                    }
                    else
                    {
                        Assert.IsNull(d[x]);
                    }
                }
            }
            if (prop.GetType() == typeof(List<double>))
            {
               
                List<double> d = (List<double>)prop;
                for (int x = 0; x < lines.Length; x++)
                {
                    double dub = double.Parse(lines[x]);
                    Assert.AreEqual(0, dub - d[x], doubleDelta, $"Double exceeded delta at iteration {x + 1}");
                }
            }
            if (prop.GetType() == typeof(List<string>))
            {
                List<string> d = (List<string>)prop;
                for (int x = 0; x < lines.Length; x++)
                {
                    Assert.AreEqual(d[x], lines[x], $"String properties do not match at iteration {x + 1}");
                }
            }
        }

        /// <summary>
        /// Ensures solstice/equinox accuracy is within error limits (100 seconds maximum delta).
        /// Tested together due to algorithm configuration of values.
        /// Table 27B Accurate between 1000-2999AD
        /// </summary>
        [TestMethod]
        public void Solstice_Accuracy_Test_27B()
        {
            Coordinate c = new Coordinate(45, 75, new DateTime(2000, 1, 1));
            int delta = 100;
            //1000AD+

            //Spring
            var ts = new DateTime(2000, 3, 20, 7, 36, 19) - c.CelestialInfo.Equinoxes.Spring;
            Assert.AreEqual(0, ts.TotalSeconds, delta);
            //Summer
            ts = new DateTime(2000, 6, 21, 1, 48, 46) - c.CelestialInfo.Solstices.Summer;
            Assert.AreEqual(0, ts.TotalSeconds, delta);
            //Fall
            ts = new DateTime(2000, 9, 22, 17, 28, 40) - c.CelestialInfo.Equinoxes.Fall;
            Assert.AreEqual(0, ts.TotalSeconds, delta);
            //Winter
            ts = new DateTime(2000, 12, 21, 13, 38, 30) - c.CelestialInfo.Solstices.Winter;
            Assert.AreEqual(0, ts.TotalSeconds, delta);
        }

        /// <summary>
        /// Ensures accuracy of solar noon times.
        /// </summary>
        [TestMethod]
        public void Solar_Noon_Times()
        {
            Coordinate c = new Coordinate(39.833, -98.583, new DateTime(2020, 8, 11));
            c.Offset = -5; //Chicago Time

            //Solar noon based on NOAA Data.
            var ts = new DateTime(2020, 8, 11, 13, 39, 25) - c.CelestialInfo.SolarNoon;
            Assert.AreEqual(0, Math.Abs(ts.Value.TotalSeconds), 90);

            c = new Coordinate(47.608, -122.335, new DateTime(2020, 8, 11));
            c.Offset = -7; //Seattle Time

            //Solar noon based on NOAA Data.
            ts = new DateTime(2020, 8, 11, 13, 14, 25) - c.CelestialInfo.SolarNoon;
            Assert.AreEqual(0, Math.Abs(ts.Value.TotalSeconds), 90);

            c = new Coordinate(-32.608, 125.335, new DateTime(2020, 8, 11));
            c.Offset = 8; //Perth Time

            //Solar noon based on NOAA Data.
            ts = new DateTime(2020, 8, 11, 11, 43, 51) - c.CelestialInfo.SolarNoon;
            Assert.AreEqual(0, Math.Abs(ts.Value.TotalSeconds), 90);

            //Zulu check
            c.Offset = 0;
            ts = new DateTime(2020, 8, 11, 03, 43, 51) - c.CelestialInfo.SolarNoon;
            Assert.AreEqual(0, Math.Abs(ts.Value.TotalSeconds), 90);
        }

        /// <summary>
        /// Ensures correct time is returned at specified point and date
        /// </summary>
        [TestMethod]
        public void Time_at_Altitude_Tests()
        {
            //Expected values based off suncalc data          
            Coordinate c = new Coordinate(47.40615, -122.24517, new DateTime(2020, 8, 11, 11, 29, 0));
            c.Offset = -7;
            var t = Celestial.Get_Time_at_Solar_Altitude(c, 50.94);
            var ts = c.GeoDate - t.Rising;
            Assert.AreEqual(0, Math.Abs(ts.Value.TotalSeconds), 60, $"Time: {t.Rising}");
            Assert.AreEqual(CelestialStatus.RiseAndSet, t.Condition);

            t = Celestial.Get_Time_at_Solar_Altitude(c.Latitude.ToDouble(), c.Longitude.ToDouble(), c.GeoDate.AddHours(7), 50.94);
            ts = c.GeoDate.AddHours(7) - t.Rising;
            Assert.AreEqual(0, Math.Abs(ts.Value.TotalSeconds), 60, $"Time: {t.Rising}");
            Assert.AreEqual(CelestialStatus.RiseAndSet, t.Condition);

            t = Celestial.Get_Time_at_Solar_Altitude(c.Latitude.ToDouble(), c.Longitude.ToDouble(), c.GeoDate, 50.94, c.Offset);
            ts = c.GeoDate - t.Rising;
            Assert.AreEqual(0, Math.Abs(ts.Value.TotalSeconds), 60, $"Time: {t.Rising}");
            Assert.AreEqual(CelestialStatus.RiseAndSet, t.Condition);

            //Expected values based on NOAA data
            c.GeoDate = new DateTime(2020, 8, 11, 18, 02, 10);
            t = Celestial.Get_Time_at_Solar_Altitude(c, 23.09);
            ts = c.GeoDate - t.Setting;
            Assert.AreEqual(0, Math.Abs(ts.Value.TotalSeconds), 240, $"Time: {t.Setting}");
            Assert.AreEqual(CelestialStatus.RiseAndSet, t.Condition);
        }

        /// <summary>
        /// Ensures correct conditions are returned at specified point and date
        /// </summary>
        [TestMethod]
        public void Time_at_Altitude_Condition_Tests()
        {
            Coordinate c = new Coordinate(47.40615, -122.24517, new DateTime(2020, 8, 11, 11, 29, 0));
            var t = Celestial.Get_Time_at_Solar_Altitude(c, 57.8);
            Assert.AreEqual(CelestialStatus.DownAllDay, t.Condition);

            t = Celestial.Get_Time_at_Solar_Altitude(c, -57.8);
            Assert.AreEqual(CelestialStatus.UpAllDay, t.Condition);

            c = new Coordinate(75, 45, new DateTime(2020, 11, 5, 10, 7, 0));
            c.Offset = -9;
            t = Celestial.Get_Time_at_Solar_Altitude(c, -.8);
            Assert.AreEqual(CelestialStatus.NoRise, t.Condition);

            c = new Coordinate(76, -45, new DateTime(2021, 8, 18, 10,7,0));           
            t = Celestial.Get_Time_at_Solar_Altitude(c, -.8);
            Assert.AreEqual(CelestialStatus.NoSet, t.Condition);
        }

        [TestMethod]
        public void Time_at_Azimuth_Tests()
        {
            Random r = new Random();
            for (int x = 0; x < 100; x++)
            {
                double lat = r.Next(-80, 80);
                double lng = r.Next(-180, 180);
                DateTime d = new DateTime(r.Next(2000, 2050), r.Next(1, 12), r.Next(1, 28));
                double az = r.Next(90, 270);
                double offset = r.Next(-12, 12);
                Coordinate c = new Coordinate(lat, lng, d, new EagerLoad(false));
                c.Offset = offset;
                DateTime? t = Celestial.Get_Time_At_Solar_Azimuth(az, c);
                if (t == null) { continue; }

                c = new Coordinate(lat,lng,t.Value, new EagerLoad(EagerLoadType.Celestial));
                c.Offset = offset;
                Assert.AreEqual(az, c.CelestialInfo.SunAzimuth, .8);
            }
           
           
        }

        /// <summary>
        /// Ensure solar coordinate accuracy
        /// </summary>
        [TestMethod]
        public void Solar_Coordinate_Accuracy_Check()
        {
            var sc1 = Celestial.Get_Solar_Coordinate(new DateTime(2020, 11, 19, 0, 0, 0));
            var sc2 = Celestial.Get_Solar_Coordinate(new DateTime(2020, 11, 19, 0, 26, 0));
            var sc3 = Celestial.Get_Solar_Coordinate(new DateTime(2020, 11, 19, 12, 26, 0));
            var sc4 = Celestial.Get_Solar_Coordinate(new DateTime(1992, 10, 13, 1, 0, 0));
            var sc5 = Celestial.Get_Solar_Coordinate(new DateTime(2020, 9, 1, 11, 24, 0)); //Normalization subsolar check


            Assert.AreEqual(0.988293, sc1.RadiusVector, .000001, "Radius vector for sc1 exceeds delta.");
            Assert.AreEqual(237.122, sc1.Longitude, .8, "Longitude for sc1 exceeds delta.");
            Assert.AreEqual(0, sc1.Latitude, .001, "Latitude for sc1 exceeds delta.");
            Assert.AreEqual(234.835, sc1.RightAscension, .1, "Right Ascension for sc1 exceeds delta.");
            Assert.AreEqual(-19.514, sc1.Declination, .1, "Declination for sc1 exceeds delta.");
            Assert.AreEqual(238.491, sc1.GeometricMeanLongitude, .001, "Geometric Mean Longitude for sc1 exceeds delta.");
            Assert.AreEqual(-19.514, sc1.SubsolarLatitude, .01, "Subsolar Latitude for sc1 exceeds delta.");
            Assert.AreEqual(176.344, sc1.SubsolarLongitude, .03, "Subsolar Longitude for sc1 exceeds delta.");

            Assert.AreEqual(0.988289, sc2.RadiusVector, .000001, "Radius vector for sc2 exceeds delta.");
            Assert.AreEqual(237.141, sc2.Longitude, .8, "Longitude for sc2 exceeds delta.");
            Assert.AreEqual(0, sc2.Latitude, .001, "Latitude for sc2 exceeds delta.");
            Assert.AreEqual(234.854, sc2.RightAscension, .1, "Right Ascension for sc2 exceeds delta.");
            Assert.AreEqual(-19.518, sc2.Declination, .1, "Declination for sc2 exceeds delta.");
            Assert.AreEqual(238.509, sc2.GeometricMeanLongitude, .001, "Geometric Mean Longitude for sc2 exceeds delta.");
            Assert.AreEqual(-19.518, sc2.SubsolarLatitude, .01, "Subsolar Latitude for sc2 exceeds delta.");
            Assert.AreEqual(169.845, sc2.SubsolarLongitude, .03, "Subsolar Longitude for sc2 exceeds delta.");

            Assert.AreEqual(0.988186, sc3.RadiusVector, .000001, "Radius vector for sc3 exceeds delta.");
            Assert.AreEqual(237.141, sc3.Longitude, .8, "Longitude for sc3 exceeds delta.");
            Assert.AreEqual(0, sc3.Latitude, .001, "Latitude for sc3 exceeds delta.");
            Assert.AreEqual(235.376, sc3.RightAscension, .1, "Right Ascension for sc3 exceeds delta.");
            Assert.AreEqual(-19.632, sc3.Declination, .1, "Declination for sc3 exceeds delta.");
            Assert.AreEqual(239.002, sc3.GeometricMeanLongitude, .001, "Geometric Mean Longitude for sc3 exceeds delta.");
            Assert.AreEqual(-19.632, sc3.SubsolarLatitude, .01, "Subsolar Latitude for sc3 exceeds delta.");
            Assert.AreEqual(-10.126, sc3.SubsolarLongitude, .03, "Subsolar Longitude for sc3 exceeds delta.");

            Assert.AreEqual(0.997650, sc4.RadiusVector, .000001, "Radius vector for sc4 exceeds delta.");
            Assert.AreEqual(199.950, sc4.Longitude, .8, "Longitude for sc4 exceeds delta.");
            Assert.AreEqual(0, sc4.Latitude, .001, "Latitude for sc4 exceeds delta.");
            Assert.AreEqual(198.420, sc4.RightAscension, .1, "Right Ascension for sc4 exceeds delta.");
            Assert.AreEqual(-7.801, sc4.Declination, .1, "Declination for sc4 exceeds delta.");
            Assert.AreEqual(201.848, sc4.GeometricMeanLongitude, .001, "Geometric Mean Longitude for sc4 exceeds delta.");
            Assert.AreEqual(-7.801, sc4.SubsolarLatitude, .01, "Subsolar Latitude for sc4 exceeds delta.");
            Assert.AreEqual(161.572, sc4.SubsolarLongitude, .03, "Subsolar Longitude for sc4 exceeds delta.");

          
            Assert.AreEqual(8.016, sc5.SubsolarLatitude, .01, "Subsolar Latitude for sc5 exceeds delta.");
            Assert.AreEqual(8.966, sc5.SubsolarLongitude, .03, "Subsolar Longitude for sc5 exceeds delta.");
        }

        /// <summary>
        /// Ensures solar coordinate accuracy in local time
        /// </summary>
        [TestMethod]
        public void Solar_Coordinate_Accuracy_Local_Time_Check()
        {
            var lcZ = Celestial.Get_Solar_Coordinate(new DateTime(2020, 11, 19, 0, 0, 0));
            var lcL = Celestial.Get_Solar_Coordinate(new DateTime(2020, 11, 18, 13, 0, 0), -11);

            //Local float precision loss will occur
            Assert.AreEqual(lcZ.SubsolarLatitude, lcL.SubsolarLatitude, .000001, "Subsolar Latitude");
            Assert.AreEqual(lcZ.SubsolarLongitude, lcL.SubsolarLongitude, .0001, "Subsolar Longitude");
            Assert.AreEqual(lcZ.RightAscension, lcL.RightAscension, .0000001, "Right Ascension");
        }
        /// <summary>
        /// Ensures day and night time spans are calculating correctly
        /// </summary>
        [TestMethod]
        public void Check_DayNight_Times()
        {
            //Rise and set at Z
            Coordinate c = new Coordinate(45, 112, new DateTime(2024, 9, 30));
            Assert.AreEqual(new TimeSpan(24, 0, 0), c.CelestialInfo.DaySpan + c.CelestialInfo.NightSpan);           

            //Rise and set at local
            Coordinate c2 = new Coordinate(45, 112, new DateTime(2024, 9, 30));
            for (int i = -12; i <=14;  i++) 
            {
                c2.Offset = i;
                Assert.AreEqual(c.CelestialInfo.DaySpan.TotalMinutes, c2.CelestialInfo.DaySpan.TotalMinutes ,3.2);     //3.2 minute delta accounts for day shift. Still accurate as long as sum of span = 24hrs
                Assert.AreEqual(new TimeSpan(24, 0, 0), c2.CelestialInfo.DaySpan + c2.CelestialInfo.NightSpan);
                Assert.AreEqual(new TimeSpan(11, 45, 0).TotalMinutes, c2.CelestialInfo.DaySpan.TotalMinutes, 3.8); //3.8 minute delta accounts for local day shift. Still accurate as long as sum of span = 24hrs
                Assert.AreEqual(new TimeSpan(12, 15, 0).TotalMinutes, c2.CelestialInfo.NightSpan.TotalMinutes, 3.8); //3.8 minute delta accounts for local day shift. Still accurate as long as sum of span = 24hrs
                if (c2.CelestialInfo.SunRise < c2.CelestialInfo.SunSet)
                {
                    Assert.AreEqual(c2.CelestialInfo.DaySpan, c2.CelestialInfo.SunSet - c2.CelestialInfo.SunRise); //Checks timespan is calculating correctly
                }
                else
                {

                    Assert.AreEqual(c2.CelestialInfo.NightSpan, c2.CelestialInfo.SunRise - c2.CelestialInfo.SunSet); //Checks timespan is calculating correctly
                }
            }
           

            //Up all day
            c = new Coordinate(89, 112, new DateTime(2024, 12, 12));
            Assert.AreEqual(new TimeSpan(24, 0, 0), c.CelestialInfo.DaySpan + c.CelestialInfo.NightSpan);
            Assert.AreEqual(new TimeSpan(0, 0, 0), c.CelestialInfo.DaySpan);

            //Down all day
            c = new Coordinate(89, 112, new DateTime(2024, 6, 21));
            Assert.AreEqual(new TimeSpan(24, 0, 0), c.CelestialInfo.DaySpan + c.CelestialInfo.NightSpan);
            Assert.AreEqual(new TimeSpan(24, 0, 0), c.CelestialInfo.DaySpan);

            //No Set
            c = new Coordinate(66.771832, 22.339500, new DateTime(2024, 6, 2));
            c.Offset = 2;
            Assert.AreEqual(new TimeSpan(24, 0, 0), c.CelestialInfo.DaySpan + c.CelestialInfo.NightSpan);
            Assert.AreEqual(c.CelestialInfo.SunRise.Value.TimeOfDay, c.CelestialInfo.NightSpan);

            //No Rise
            c = new Coordinate(78.2232, 15.6469, new DateTime(2024, 10, 26));
            c.Offset = -11;
            Assert.AreEqual(new TimeSpan(24, 0, 0), c.CelestialInfo.DaySpan + c.CelestialInfo.NightSpan);
            Assert.AreEqual(c.CelestialInfo.SunSet.Value.TimeOfDay, c.CelestialInfo.DaySpan);
        }
        [TestMethod]
        public void Check_Horizon_Hang_Correction()
        {
            EagerLoad el = new EagerLoad(EagerLoadType.Celestial);

            //No Rise Reported, Correct to Up All Day
            DateTime d = new DateTime(2021, 4, 24, 0, 0, 0);
            var c = Celestial.CalculateCelestialTimes(76, 45, d, el, 0);
            Assert.AreEqual(c.SunCondition, CelestialStatus.UpAllDay);

            //Next day is up all day, correct to No Set
            d = new DateTime(2021, 4, 24, 0, 0, 0);
            c = Celestial.CalculateCelestialTimes(76, 45, d, el, 3);
            Assert.AreEqual(c.SunCondition, CelestialStatus.NoSet);

            //Ensure up all day
            d = new DateTime(2021, 4, 25, 0, 0, 0);
            c = Celestial.CalculateCelestialTimes(76, 45, d, el, 3);
            Assert.AreEqual(c.SunCondition, CelestialStatus.UpAllDay);
      
            //No set correct to up all day
            d = new DateTime(2024, 6, 4);
            c = Celestial.CalculateCelestialTimes(66.771832, 22.339500, d, el, 2);
            Assert.AreEqual(c.SunCondition, CelestialStatus.UpAllDay);

            //No rise correct to up all day
            d = new DateTime(2024, 7, 9);
            c = Celestial.CalculateCelestialTimes(66.771832, 22.339500, d, el, 2);
            Assert.AreEqual(c.SunCondition, CelestialStatus.UpAllDay);

            //Correct Up All Day
            d = new DateTime(2024, 1, 8);
            c = Celestial.CalculateCelestialTimes(-66.771832, 22.339500, d, el, 2);
            Assert.AreEqual(c.SunCondition, CelestialStatus.UpAllDay);

            //Strip Set correction
            d = new DateTime(2024, 12, 4);
            c = Celestial.CalculateCelestialTimes(-66.771832, 22.339500, d, el, 2);
            Assert.AreEqual(c.SunCondition, CelestialStatus.NoSet);

        }
       
    }



    public class Solar_Data
    {
        public List<DateTime?> SunRises { get; set; }

        public List<DateTime?> SunSets { get; set; }
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

        public SolarEclipse SolarEclipse { get; set; }

        public List<bool> IsSunUp { get; set; }

    }
}
