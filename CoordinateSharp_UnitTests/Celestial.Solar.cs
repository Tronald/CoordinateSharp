using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CoordinateSharp;
using System.Reflection;

namespace CoordinateSharp_UnitTests
{
    [TestClass]
    public class Celestial
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
      
    }
}
