using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CoordinateSharp;
using System.Reflection;

namespace CoordinateSharp_UnitTests
{
    [TestClass]
    public class CelestialSolar
    {
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
        /// <summary>
        /// Ensures solstice/equinox accuracy is within error limits (100 seconds maximum delta).
        /// Tested together due to algorithm configuration of values.
        /// Table 27B Accurate between 1000-3000AD
        /// </summary>
        [TestMethod]
        public void Solstice_Accuracy_Test_27B()
        {
            Coordinate c = new Coordinate(45, 75, new DateTime(2000, 1, 1));
            int delta = 100;
            //1000AD+
            
            //Spring
            var ts = new DateTime(2000, 3, 20, 7, 36, 19) - c.CelestialInfo.Equinoxes.Spring;
            Assert.AreEqual(0,ts.TotalSeconds, delta);
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
        /// Ensures correct time is return at specified point and date
        /// </summary>
        [TestMethod]
        public void Time_at_Altitude_Tests()
        {
            //Expected values based of suncalc Data          
            Coordinate c = new Coordinate(47.40615, -122.24517, new DateTime(2020, 8, 11, 11, 29, 0));
            c.Offset = -7;
            var t = Celestial.Get_Time_at_Solar_Altitude(c, 50.94);
            var ts = c.GeoDate - t.Rising;
            Assert.AreEqual(0, Math.Abs(ts.Value.TotalSeconds), 60, $"Time: {t.Rising}");

            t = Celestial.Get_Time_at_Solar_Altitude(c.Latitude.ToDouble(),c.Longitude.ToDouble(), c.GeoDate.AddHours(7), 50.94);
            ts = c.GeoDate.AddHours(7) - t.Rising;
            Assert.AreEqual(0, Math.Abs(ts.Value.TotalSeconds), 60, $"Time: {t.Rising}");

            t = Celestial.Get_Time_at_Solar_Altitude(c.Latitude.ToDouble(), c.Longitude.ToDouble(), c.GeoDate, 50.94, c.Offset);
            ts = c.GeoDate - t.Rising;
            Assert.AreEqual(0, Math.Abs(ts.Value.TotalSeconds), 60, $"Time: {t.Rising}");

            //Expected values based on NOAA data
            c.GeoDate = new DateTime(2020, 8, 11, 18, 02, 10);
            t = Celestial.Get_Time_at_Solar_Altitude(c, 23.09);
            ts = c.GeoDate - t.Setting;
            Assert.AreEqual(0, Math.Abs(ts.Value.TotalSeconds), 240, $"Time: {t.Setting}");
        }
    }
}
