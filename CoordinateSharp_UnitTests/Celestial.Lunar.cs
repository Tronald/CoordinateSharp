using System;
using System.Collections.Generic;
using System.Text;
using CoordinateSharp;
using System.Reflection;
using System.Collections;
using System.IO;
using System.Globalization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoordinateSharp_UnitTests
{
    [TestClass]
    public class CelestialLunar
    {
        readonly Lunar_Data  data = new Lunar_Data();

        public CelestialLunar()
        {
            //Test will run against N39, W72
            //Sun Times and Moon Times will range for 1 Mar-2018 to 30-Mar-2018
            //Sun/Moon Alt, Az, Perigee, Apogee, Eclipses, fraction and distance with be tested against 15-Mar-2018 UTC
            Coordinate c = new Coordinate(39, -72, new DateTime(2018, 03, 15));
           
            for (int x = 0; x < 144; x++)
            {
                if (x != 0)
                {
                    c.GeoDate = c.GeoDate.AddMinutes(10);
                }
                data.MoonAlts.Add(c.CelestialInfo.MoonAltitude);
                data.MoonAzs.Add(c.CelestialInfo.MoonAzimuth);
                data.MoonFractions.Add(c.CelestialInfo.MoonIllum.Fraction);
            }

            c.GeoDate = new DateTime(2018, 3, 1);

            data.MoonRises = new List<DateTime?>();
            data.MoonSets = new List<DateTime?>();
        

            for (int x = 0; x < 31; x++)
            {
                if (x != 0)
                {
                    c.GeoDate = c.GeoDate.AddDays(1);
                }
                data.MoonRises.Add(c.CelestialInfo.MoonRise);
                data.MoonSets.Add(c.CelestialInfo.MoonSet);
                data.MoonDistances.Add(c.CelestialInfo.MoonDistance.Kilometers);
                data.MoonPhases.Add(c.CelestialInfo.MoonIllum.Phase);
                data.MoonPhaseNames.Add(c.CelestialInfo.MoonIllum.PhaseName);
                data.MoonPhaseNameEnums.Add(c.CelestialInfo.MoonIllum.PhaseNameEnum);
            }

            //Set Dates and Finish
            //THESE OBJECT ARE TESTED AGAINST SERIALIZED OBJECTS.
            //IF CHANGING THE MODEL YOU WILL HAVE TO CHANGE THE OBJECTS THEY ARE TESTED AGAINST AS WELL
            data.LunarEclispe = c.CelestialInfo.LunarEclipse;
            data.Perigee = c.CelestialInfo.Perigee;
            data.Apogee = c.CelestialInfo.Apogee;

        }

        [TestMethod]
        public void Moonset()
        {
            Check_Values(data.MoonSets, "CelestialData\\MoonSet.txt");
        }
        [TestMethod]
        public void Moonrise()
        {
            Check_Values(data.MoonRises, "CelestialData\\MoonRise.txt");
        }
        [TestMethod]
        public void MoonAltitude()
        {
            Check_Values(data.MoonAlts, "CelestialData\\MoonAlts.txt",.0001);
        }
        [TestMethod]
        public void MoonAzimuth()
        {
            Check_Values(data.MoonAzs, "CelestialData\\MoonAzs.txt",.0001);
        }
        [TestMethod]
        public void MoonDistance()
        {
            Check_Values(data.MoonDistances, "CelestialData\\MoonDistance.txt",.0001);
        }
        [TestMethod]
        public void MoonIllum()
        {
            Check_Values(data.MoonFractions, "CelestialData\\MoonFraction.txt",.01);
        }
        [TestMethod]
        public void MoonPhase()
        {
            Check_Values(data.MoonPhases, "CelestialData\\MoonPhase.txt",.001);
        }
        [TestMethod]
        public void MoonPhaseName()
        {
            Check_Values(data.MoonPhaseNames, "CelestialData\\MoonPhaseName.txt");
        }
        [TestMethod]
        public void MoonPhaseEnum()
        {
            Check_Values(data.MoonPhaseNameEnums, "CelestialData\\MoonPhaseName.txt");
        }
        [TestMethod]
        public void LunarEclipse()
        {     
            //Deserialize     
            using (StreamReader streamReader = new StreamReader("CelestialData\\LunarEclipse.txt"))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                LunarEclipse ev = (LunarEclipse)binaryFormatter.Deserialize(streamReader.BaseStream);

                LunarEclipseDetails lE1 = ev.LastEclipse;
                LunarEclipseDetails nE1 = ev.NextEclipse;
                LunarEclipseDetails lE2 = data.LunarEclispe.LastEclipse;
                LunarEclipseDetails nE2 = data.LunarEclispe.NextEclipse;

                PropertyInfo[] properties = typeof(LunarEclipseDetails).GetProperties();
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
        }

        [TestMethod]
        public void IsMoonUp()
        {
            string[] lines = File.ReadAllLines("CelestialData\\IsMoonUp.txt");
            foreach (string line in lines)
            {
                string[] split = line.Split(',');
                double lat = double.Parse(split[0]);
                double longi = double.Parse(split[1]);
                DateTime geoDate = DateTime.Parse(split[2]).Date;
                
                string condition = split[5];
                EagerLoad el = new EagerLoad(EagerLoadType.Celestial);
                el.Extensions = new EagerLoad_Extensions(EagerLoad_ExtensionsType.Lunar_Cycle);
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
                            Assert.AreNotEqual(true, c.CelestialInfo.IsMoonUp, $"Down All Day Expected on Iteration {x + 1}");
                            break;
                        case "UpAllDay":
                            c.GeoDate = c.GeoDate.AddMinutes(i);
                            Assert.AreNotEqual(false, c.CelestialInfo.IsMoonUp, $"Up All Day Expected on Iteration {x + 1}");
                            break;
                        case "NoRise":
                            c.GeoDate = c.GeoDate.AddMinutes(i);
                            if (c.CelestialInfo.MoonSet > c.GeoDate && c.CelestialInfo.IsMoonUp == false) { Assert.Fail($"NoRise Moonset time is greater than geodate while Moon is down at iteration {x + 1}"); }
                            if (c.CelestialInfo.MoonSet <= c.GeoDate && c.CelestialInfo.IsMoonUp == true) { Assert.Fail($"NoRise Moonset time is less than geodate while Moon is up at iteration {x + 1}"); }
                            break;
                        case "NoSet":
                            c.GeoDate = c.GeoDate.AddMinutes(i);
                            if (c.CelestialInfo.MoonRise >= c.GeoDate && c.CelestialInfo.IsMoonUp == true) { Assert.Fail($"NoSet MoonRise time is greater than geodate while Moon is up at iteration {x + 1}"); }
                            if (c.CelestialInfo.MoonRise < c.GeoDate && c.CelestialInfo.IsMoonUp == false) { Assert.Fail($"NoSet MoonRise time is less than geodate while Moon is down at iteration {x + 1}"); }
                            break;
                        case "RiseAndSet":
                            c.GeoDate = c.GeoDate.AddMinutes(i);
                            //When working in Z time we have to account for rise occuring after set
                            if (c.CelestialInfo.MoonSet > c.CelestialInfo.MoonRise)
                            {
                                //Moon SHOULD BE UP
                                if (c.GeoDate > c.CelestialInfo.MoonRise && c.GeoDate < c.CelestialInfo.MoonSet && c.CelestialInfo.IsMoonUp == false) { Assert.Fail($"Moon up expected {x + 1}"); }
                                //Moon SHOULD BE DOWN
                                if ((c.GeoDate <= c.CelestialInfo.MoonRise || c.GeoDate >= c.CelestialInfo.MoonSet) && c.CelestialInfo.IsMoonUp == true) { Assert.Fail($"Moon down expected {x + 1}"); }
                            }
                            else
                            {
                                //AFTER RISE Moon SHOULD BE UP
                                if (c.GeoDate > c.CelestialInfo.MoonRise && c.CelestialInfo.IsMoonUp == false) { Assert.Fail($"Moon up expected after rise {x + 1}"); }
                                //BETWEEN SET AND RISE Moon SHOULD BE DOWN
                                if (c.GeoDate <= c.CelestialInfo.MoonRise && c.GeoDate >= c.CelestialInfo.MoonSet && c.CelestialInfo.IsMoonUp == true) { Assert.Fail($"Moon down between set and rise expected {x + 1}"); }
                                //BEFORE SET Moon SHOULD BE UP
                                if (c.GeoDate < c.CelestialInfo.MoonSet && c.CelestialInfo.IsMoonUp == false) { Assert.Fail($"Moon up before set expected {x + 1}"); }
                            }

                            break;
                        default:
                            break;
                    }
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
          
            val = Celestial.Get_Next_MoonRise(c);
            Assert.AreEqual(val.ToString(), "2/6/2019 1:10:32 PM");

            val = Celestial.Get_Last_MoonRise(c);
            Assert.AreEqual(val.ToString(), "2/5/2019 12:39:02 PM");

            val = Celestial.Get_Next_MoonSet(c);
            Assert.AreEqual(val.ToString(), "2/7/2019 12:08:33 AM");

            val = Celestial.Get_Last_MoonSet(c);
            Assert.AreEqual(val.ToString(), "2/5/2019 11:11:09 PM");

            //Method 2 UTC
        

            val = Celestial.Get_Next_MoonRise(40.0352, -74.5844, d);
            Assert.AreEqual(val.ToString(), "2/6/2019 1:10:32 PM");

            val = Celestial.Get_Last_MoonRise(40.0352, -74.5844, d);
            Assert.AreEqual(val.ToString(), "2/5/2019 12:39:02 PM");

            val = Celestial.Get_Next_MoonSet(40.0352, -74.5844, d);
            Assert.AreEqual(val.ToString(), "2/7/2019 12:08:33 AM");

            val = Celestial.Get_Last_MoonSet(40.0352, -74.5844, d);
            Assert.AreEqual(val.ToString(), "2/5/2019 11:11:09 PM");

            //Method 3 LOCAL TIMES


            val = Celestial.Get_Next_MoonRise(40.0352, -74.5844, d, -4);
            Assert.AreEqual(val.ToString(), "2/6/2019 9:10:32 AM");

            val = Celestial.Get_Last_MoonRise(40.0352, -74.5844, d, -4);
            Assert.AreEqual(val.ToString(), "2/5/2019 8:39:02 AM");

            val = Celestial.Get_Next_MoonSet(40.0352, -74.5844, d, -4);
            Assert.AreEqual(val.ToString(), "2/6/2019 8:08:33 PM");

            val = Celestial.Get_Last_MoonSet(40.0352, -74.5844, d, -4);
            Assert.AreEqual(val.ToString(), "2/5/2019 7:11:09 PM");

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
            cCel = new Celestial(0, 0, DateTime.Now, 12);
            cCel = new Celestial(0, 0, DateTime.Now, -12, new EagerLoad());
            cCel = new Celestial(0, 0, DateTime.Now, 12, new EagerLoad());
            cCel = Celestial.CalculateCelestialTimes(0, 0, DateTime.Now, -12);
            cCel = Celestial.CalculateCelestialTimes(0, 0, DateTime.Now, 12);
            cCel = Celestial.CalculateCelestialTimes(0, 0, DateTime.Now, new EagerLoad(), 12);
            cCel = Celestial.CalculateCelestialTimes(0, 0, DateTime.Now, new EagerLoad(), -12);

            //OUT OF RANGE
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { cUTC.Offset = -13; });
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { cUTC.Offset = 13; });
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { cCel = new Celestial(0, 0, DateTime.Now, -13); });
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { cCel = new Celestial(0, 0, DateTime.Now, 13); });
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { cCel = new Celestial(0, 0, DateTime.Now, -13, new EagerLoad()); });
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { cCel = new Celestial(0, 0, DateTime.Now, 13, new EagerLoad()); });
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { cCel = Celestial.CalculateCelestialTimes(0, 0, DateTime.Now, -13); });
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { cCel = Celestial.CalculateCelestialTimes(0, 0, DateTime.Now, 13); });
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { cCel = Celestial.CalculateCelestialTimes(0, 0, DateTime.Now, new EagerLoad(), -13); });
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => { cCel = Celestial.CalculateCelestialTimes(0, 0, DateTime.Now, new EagerLoad(), 13); });

        }

        [TestMethod]
        public void Perigee()
        {
            //Deserialize     
            using (StreamReader streamReader = new StreamReader("CelestialData\\Perigee.txt"))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                Perigee ev = (Perigee)binaryFormatter.Deserialize(streamReader.BaseStream);

                PerigeeApogee lE1 = ev.LastPerigee;
                PerigeeApogee nE1 = ev.NextPerigee;
                PerigeeApogee lE2 = data.Perigee.LastPerigee;
                PerigeeApogee nE2 = data.Perigee.NextPerigee;

                PropertyInfo[] properties = typeof(PerigeeApogee).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    var l1 = property.GetValue(lE1);
                    var l2 = property.GetValue(lE2);
                    var n1 = property.GetValue(nE1);
                    var n2 = property.GetValue(nE2);

                    Assert.AreEqual(l1.ToString(), l2.ToString(), $"Last Perigee: {l2} not expected.");

                    if (l1.GetType() == typeof(Distance) && l2.GetType() == typeof(Distance))
                    {
                        PropertyInfo[] propertiesSub = typeof(Distance).GetProperties();
                        foreach (PropertyInfo propertySub in propertiesSub)
                        {
                            var l1Sub = propertySub.GetValue(l1);
                            var l2Sub = propertySub.GetValue(l2);
                            Assert.AreEqual(l1Sub.ToString(), l2Sub.ToString(), $"Last Perigee {propertySub.Name}: {l2Sub} not expected.");                        
                        }
                    }
                    Assert.AreEqual(n1.ToString(), n2.ToString(), $"Next Perigee: {n2} not expected.");

                    if (n1.GetType() == typeof(Distance) && n2.GetType() == typeof(Distance))
                    {
                        PropertyInfo[] propertiesSub = typeof(Distance).GetProperties();
                        foreach (PropertyInfo propertySub in propertiesSub)
                        {
                            var n1Sub = propertySub.GetValue(n1);
                            var n2Sub = propertySub.GetValue(n2);
                            Assert.AreEqual(n1Sub.ToString(), n2Sub.ToString(), $"Next Perigee {propertySub.Name}: {n2Sub} not expected.");
                        }
                    }

                }
            }
 
        }

        [TestMethod]
        public void Apogee()
        {          
            //Deserialize     
            using (StreamReader streamReader = new StreamReader("CelestialData\\Apogee.txt"))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                Apogee ev = (Apogee)binaryFormatter.Deserialize(streamReader.BaseStream);

                PerigeeApogee lE1 = ev.LastApogee;
                PerigeeApogee nE1 = ev.NextApogee;
                PerigeeApogee lE2 = data.Apogee.LastApogee;
                PerigeeApogee nE2 = data.Apogee.NextApogee;

                PropertyInfo[] properties = typeof(PerigeeApogee).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    var l1 = property.GetValue(lE1);
                    var l2 = property.GetValue(lE2);
                    var n1 = property.GetValue(nE1);
                    var n2 = property.GetValue(nE2);

                    Assert.AreEqual(l1.ToString(), l2.ToString(), $"Last Apogee: {l2} not expected.");
                    if (l1.GetType() == typeof(Distance) && l2.GetType() == typeof(Distance))
                    {
                        PropertyInfo[] propertiesSub = typeof(Distance).GetProperties();
                        foreach (PropertyInfo propertySub in propertiesSub)
                        {
                            var l1Sub = propertySub.GetValue(l1);
                            var l2Sub = propertySub.GetValue(l2);
                            Assert.AreEqual(l1Sub.ToString(), l2Sub.ToString(), $"Last Apogee {propertySub.Name}: {l2Sub} not expected.");
                        }
                    }
                    Assert.AreEqual(n1.ToString(), n2.ToString(), $"Next Apogee: {l2} not expected.");
                    if (n1.GetType() == typeof(Distance) && n2.GetType() == typeof(Distance))
                    {
                        PropertyInfo[] propertiesSub = typeof(Distance).GetProperties();
                        foreach (PropertyInfo propertySub in propertiesSub)
                        {
                            var n1Sub = propertySub.GetValue(n1);
                            var n2Sub = propertySub.GetValue(n2);
                            Assert.AreEqual(n1Sub.ToString(), n2Sub.ToString(), $"Next Apogee {propertySub.Name}: {n2Sub} not expected.");
                        }
                    }

                }
            }
        }

        /// <summary>
        /// Ensures lunar coordinate accuracy
        /// </summary>
        [TestMethod]
        public void Lunar_Coordinate_Accuracy_Check()
        {
            //https://www.timeanddate.com/worldclock/sunearth.html
            var lc1 = Celestial.Get_Lunar_Coordinate(new DateTime(2020, 11, 19, 0, 0, 0));
            var lc2 = Celestial.Get_Lunar_Coordinate(new DateTime(2020, 11, 19, 0, 26, 0));
            var lc3 = Celestial.Get_Lunar_Coordinate(new DateTime(2020, 11, 19, 12, 26, 0));
            var lc4 = Celestial.Get_Lunar_Coordinate(new DateTime(1992, 10, 13, 1, 0, 0));


          
            Assert.AreEqual(288.404, lc1.Longitude, .8, "Longitude for lc1 exceeds delta.");
            Assert.AreEqual(-2.490, lc1.Latitude, .1, "Latitude for lc1 exceeds delta.");
            Assert.AreEqual(290.305, lc1.RightAscension, .1, "Right Ascension for lc1 exceeds delta.");
            Assert.AreEqual(-24.639, lc1.Declination, .1, "Declination for lc1 exceeds delta.");
            Assert.AreEqual(281.280, lc1.GeometricMeanLongitude, .001, "Geometric Mean Longitude for lc1 exceeds delta.");
            Assert.AreEqual(-24.39, lc1.SublunarLatitude, .4, "Sublunar Latitude for lc1 exceeds delta.");
            Assert.AreEqual(-128.09, lc1.SublunarLongitude, .4, "Sublunar Longitude for lc1 exceeds delta.");

           
            Assert.AreEqual(288.653, lc2.Longitude, .8, "Longitude for lc2 exceeds delta.");
            Assert.AreEqual(-2.511, lc2.Latitude, .1, "Latitude for lc2 exceeds delta.");
            Assert.AreEqual(290.579, lc2.RightAscension, .1, "Right Ascension for lc2 exceeds delta.");
            Assert.AreEqual(-24.624, lc2.Declination, .1, "Declination for lc2 exceeds delta.");
            Assert.AreEqual(281.518, lc2.GeometricMeanLongitude, .001, "Geometric Mean Longitude for lc2 exceeds delta.");
            Assert.AreEqual(-24.38, lc2.SublunarLatitude, .4, "Sublunar Latitude for lc2 exceeds delta.");
            Assert.AreEqual(-134.24, lc2.SublunarLongitude, .4, "Sublunar Longitude for lc2 exceeds delta.");

           
            Assert.AreEqual(295.493, lc3.Longitude, .8, "Longitude for lc3 exceeds delta.");
            Assert.AreEqual(-3.043, lc3.Latitude, .1, "Latitude for lc3 exceeds delta.");
            Assert.AreEqual(298.072, lc3.RightAscension, .1, "Right Ascension for lc3 exceeds delta.");
            Assert.AreEqual(-24.030, lc3.Declination, .1, "Declination for lc3 exceeds delta.");
            Assert.AreEqual(288.106, lc3.GeometricMeanLongitude, .001, "Geometric Mean Longitude for lc3 exceeds delta.");
            Assert.AreEqual(-24.02, lc3.SublunarLatitude, .4, "Sublunar Latitude for lc3 exceeds delta.");
            Assert.AreEqual(52.36, lc3.SublunarLongitude, .4, "Sublunar Longitude for lc3 exceeds delta.");

          
            Assert.AreEqual(34.832, lc4.Longitude, .8, "Longitude for lc4 exceeds delta.");
            Assert.AreEqual(3.776, lc4.Latitude, .1, "Latitude for lc4 exceeds delta.");
            Assert.AreEqual(31.235, lc4.RightAscension, .1, "Right Ascension for lc4 exceeds delta.");
            Assert.AreEqual(16.686, lc4.Declination, .1, "Declination for lc4 exceeds delta.");
            Assert.AreEqual(39.296, lc4.GeometricMeanLongitude, .001, "Geometric Mean Longitude for lc4 exceeds delta.");
            Assert.AreEqual(16.420, lc4.SublunarLatitude, .4, "Sublunar Latitude for lc4 exceeds delta.");
            Assert.AreEqual(-5.37, lc4.SublunarLongitude, .4, "Sublunar Longitude for lc4 exceeds delta.");
        }
        /// <summary>
        /// Ensures lunar coordinate accuracy in local time
        /// </summary>
        [TestMethod]
        public void Lunar_Coordinate_Accuracy_Local_Time_Check()
        {     
            var lcZ = Celestial.Get_Lunar_Coordinate(new DateTime(2020, 11, 19, 0, 0, 0));
            var lcL = Celestial.Get_Lunar_Coordinate(new DateTime(2020, 11, 18, 13, 0, 0),-11);

            //Local float precision loss will occur
            Assert.AreEqual(lcZ.SublunarLatitude, lcL.SublunarLatitude, .0001, "Sublunar Latitude");
            Assert.AreEqual(lcZ.SublunarLongitude, lcL.SublunarLongitude, .0001, "Sublunar Longitude");
            Assert.AreEqual(lcZ.RightAscension, lcL.RightAscension,.00001, "Right Ascension");
        }

        private void Local_Time_Checker(Celestial cCel, Celestial lCel, Celestial sCel, Celestial bCel, double offset)
        {


            //LUNAR (PORT TO LUNAR)
            Time_Compare(cCel.MoonRise, lCel.MoonRise, sCel.MoonRise, offset);
            Time_Compare(cCel.MoonSet, lCel.MoonSet, sCel.MoonSet, offset);
            Time_Compare(cCel.LunarEclipse.LastEclipse.MidEclipse, lCel.LunarEclipse.LastEclipse.MidEclipse, sCel.LunarEclipse.LastEclipse.MidEclipse, offset);
            Time_Compare(cCel.LunarEclipse.NextEclipse.MidEclipse, lCel.LunarEclipse.NextEclipse.MidEclipse, sCel.LunarEclipse.NextEclipse.MidEclipse, offset);
            Time_Compare(cCel.Perigee.LastPerigee.Date, lCel.Perigee.LastPerigee.Date, sCel.Perigee.LastPerigee.Date, offset);
            Time_Compare(cCel.Perigee.NextPerigee.Date, lCel.Perigee.NextPerigee.Date, sCel.Perigee.NextPerigee.Date, offset);
            Time_Compare(cCel.Apogee.LastApogee.Date, lCel.Apogee.LastApogee.Date, sCel.Apogee.LastApogee.Date, offset);
            Time_Compare(cCel.Apogee.NextApogee.Date, lCel.Apogee.NextApogee.Date, sCel.Apogee.NextApogee.Date, offset);

            Assert.AreEqual(lCel.MoonAltitude, bCel.MoonAltitude, .00001);
            Assert.AreEqual(lCel.MoonAzimuth, bCel.MoonAzimuth, .00001);
            Assert.AreEqual(lCel.Perigee.LastPerigee.Distance.Meters, bCel.Perigee.LastPerigee.Distance.Meters, .00001);
            Assert.AreEqual(lCel.Perigee.NextPerigee.Distance.Meters, bCel.Perigee.NextPerigee.Distance.Meters, .00001);
            Assert.AreEqual(lCel.Apogee.LastApogee.Distance.Meters, bCel.Apogee.LastApogee.Distance.Meters, .00001);
            Assert.AreEqual(lCel.Apogee.NextApogee.Distance.Meters, bCel.Apogee.NextApogee.Distance.Meters, .00001);
            Assert.AreEqual(lCel.Apogee.LastApogee.HorizontalParallax, bCel.Apogee.LastApogee.HorizontalParallax, .00001);
            Assert.AreEqual(lCel.Apogee.NextApogee.HorizontalParallax, bCel.Apogee.NextApogee.HorizontalParallax, .00001);
            Assert.AreEqual(lCel.MoonDistance.Meters, bCel.MoonDistance.Meters, .2);
            Assert.AreEqual(lCel.MoonIllum.Angle, bCel.MoonIllum.Angle, .00001);
            Assert.AreEqual(lCel.MoonIllum.Fraction, bCel.MoonIllum.Fraction, .00001);
            Assert.AreEqual(lCel.MoonIllum.Phase, bCel.MoonIllum.Phase, .00001);
            //if (lCel.MoonIllum.PhaseName != bCel.MoonIllum.PhaseName) { return false; } Test omitted as time's can changes phases between days. Documentation reflects. Issues will be captured at Phase.
            Assert.AreEqual(lCel.IsMoonUp, bCel.IsMoonUp);
            Assert.AreEqual(lCel.MoonCondition, bCel.MoonCondition);
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
            if (prop.GetType() == typeof(List<PhaseEnum>))
            {
                List<PhaseEnum> d = (List<PhaseEnum>)prop;
                for (int x = 0; x < lines.Length; x++)
                {
                    Assert.AreEqual(d[x].ToString(), lines[x].Replace(" ",""), $"Properties do not match at iteration {x + 1}");
                }
            }
        }
    }

    public class Lunar_Data
    {
        public List<DateTime?> MoonRises { get; set; } = new List<DateTime?>();

        public List<DateTime?> MoonSets { get; set; } = new List<DateTime?>();

        public List<double> MoonFractions { get; set; } = new List<double>();
        public List<double> MoonDistances { get; set; } = new List<double>();
        public List<double> MoonPhases { get; set; } = new List<double>();
        public List<string> MoonPhaseNames { get; set; } = new List<string>();
        public List<PhaseEnum> MoonPhaseNameEnums { get; set; } = new List<PhaseEnum>();


        public List<double> MoonAlts { get; set; } = new List<double>();
        public List<double> MoonAzs { get; set; } = new List<double>();

        public LunarEclipse LunarEclispe { get; set; }

        public List<bool> IsMoonUp { get; set; } = new List<bool>();

        public Perigee Perigee { get; set; }
        public Apogee Apogee { get; set; }

    }
}
