using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CoordinateSharp;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using System.Diagnostics;
using System.IO;

namespace CoordinateSharp_UnitTests
{
    [TestClass]
    public class EagerLoading_Tests
    {

        /// <summary>
        /// Tests to determine if eager load properly intializes
        /// </summary>
        [TestMethod]
        public void EagerLoading_Intializes_On()
        {
            EagerLoad e = new EagerLoad();
            Coordinate c = new Coordinate(45, 75, new DateTime(2008, 1, 2), e);

            Assert.AreNotEqual(null, c.CelestialInfo);
            Assert.AreNotEqual(null, c.UTM);
            Assert.AreNotEqual(null, c.MGRS);
            Assert.AreNotEqual(null, c.ECEF);
            Assert.AreNotEqual(null, c.Cartesian);
            Assert.AreNotEqual(null, c.WebMercator);

            //Check Polar Regions
            c = new Coordinate(86, 75, new DateTime(2008, 1, 2), e);
            Assert.AreNotEqual(null, c.MGRS);
            Assert.AreNotEqual(null, c.ECEF);
        }
        /// <summary>
        /// Tests to determine if eager load properly turns off.
        /// </summary>
        [TestMethod]
        public void EagerLoading_Off()
        {
            EagerLoad e = new EagerLoad(false);
            Coordinate c = new Coordinate(45, 75, new DateTime(2008, 1, 2), e);
            c.Latitude.DecimalDegree++; //Trigger proper changes to confirm objects remain unloaded
            c.Longitude.DecimalDegree++; //Trigger proper changes to confirm objects remain unloaded

            Assert.AreEqual(null, c.CelestialInfo);
            Assert.AreEqual(null, c.UTM);
            Assert.AreEqual(null, c.MGRS);
            Assert.AreEqual(null, c.ECEF);
            Assert.AreEqual(null, c.Cartesian);
            Assert.AreEqual(null, c.WebMercator);

            //Check Polar Regions
            c = new Coordinate(86, 75, new DateTime(2008, 1, 2), e);
            Assert.AreEqual(null, c.MGRS);
            Assert.AreEqual(null, c.ECEF);
        }

        /// <summary>
        /// Ensures celestial properties load when called.
        /// </summary>
        [TestMethod]
        public void Celestial_Load_Call()
        {
            EagerLoad e = new EagerLoad(false);
            Coordinate c = new Coordinate(45, 75, new DateTime(2008, 1, 2), e);
            c.LoadCelestialInfo();
            Assert.AreNotEqual(null, c.CelestialInfo);
        }

        /// <summary>
        /// Ensures UTM_MGRS properties load when called.
        /// </summary>
        [TestMethod]
        public void UTM_MGRS_Load_Call()
        {
            EagerLoad e = new EagerLoad(false);
            Coordinate c = new Coordinate(45, 75, new DateTime(2008, 1, 2), e);
            c.LoadUTM_MGRS_Info();
            Assert.AreNotEqual(null, c.MGRS, "MGRS data did not load.");
            Assert.AreNotEqual(null, c.UTM, "UTM data did not load.");
        }

        /// <summary>
        /// Ensures ECEF properties load when called.
        /// </summary>
        [TestMethod]
        public void ECEF_Load_Call()
        {
            EagerLoad e = new EagerLoad(false);
            Coordinate c = new Coordinate(45, 75, new DateTime(2008, 1, 2), e);
            c.LoadECEFInfo();
            Assert.AreNotEqual(null, c.ECEF);
        }
        /// <summary>
        /// Ensures Cartesian properties load when called.
        /// </summary>
        [TestMethod]
        public void Cartesian_Load_Call()
        {
            EagerLoad e = new EagerLoad(false);
            Coordinate c = new Coordinate(45, 75, new DateTime(2008, 1, 2), e);
            c.LoadCartesianInfo();
            Assert.AreNotEqual(null, c.Cartesian);
        }
        /// <summary>
        /// Ensures WebMercator properties load when called.
        /// </summary>
        [TestMethod]
        public void WebMercator_Load_Call()
        {
            EagerLoad e = new EagerLoad(false);
            Coordinate c = new Coordinate(45, 75, new DateTime(2008, 1, 2), e);
            c.LoadWebMercatorInfo();
            Assert.AreNotEqual(null, c.WebMercator);
        }
        /// <summary>
        /// Ensures celestial values do not change with coordinate when eagerloading is off.
        /// </summary>
        [TestMethod]
        public void Celestial_EagerLoad_Off_Does_Not_Update_With_Coordinate_Change()
        {
          
            Coordinate c = new Coordinate(45, 75, new DateTime(2008, 1, 2));
            var val = c.CelestialInfo; //For comparison
            c.EagerLoadSettings = new EagerLoad(false); //Turn off eager loading
            c.Latitude.DecimalDegree = 44;
            c.Longitude.DecimalDegree = 74;
            Assert.IsTrue(Helpers.ReflectiveEquals(val, c.CelestialInfo));
        }
        /// <summary>
        /// Ensures UTM values do not change with coordinate when eagerloading is off.
        /// </summary>
        [TestMethod]
        public void UTM_EagerLoad_Off_Does_Not_Update_With_Coordinate_Change()
        {

            Coordinate c = new Coordinate(45, 75, new DateTime(2008, 1, 2));
            var val = c.UTM;//For comparison
            c.EagerLoadSettings = new EagerLoad(false); //Turn off eager loading
            c.Latitude.DecimalDegree = 44;
            c.Longitude.DecimalDegree = 74;
            Assert.IsTrue(Helpers.ReflectiveEquals(val, c.UTM));
        }
        /// <summary>
        /// Ensures MGRS values do not change with coordinate when eagerloading is off.
        /// </summary>
        [TestMethod]
        public void MGRS_EagerLoad_Off_Does_Not_Update_With_Coordinate_Change()
        {

            Coordinate c = new Coordinate(45, 75, new DateTime(2008, 1, 2));
            var val = c.MGRS;//For comparison
            c.EagerLoadSettings = new EagerLoad(false); //Turn off eager loading
            c.Latitude.DecimalDegree = 44;
            c.Longitude.DecimalDegree = 74;
            Assert.IsTrue(Helpers.ReflectiveEquals(val, c.MGRS));
        }
        /// <summary>
        /// Ensures ECEFvalues do not change with coordinate when eagerloading is off.
        /// </summary>
        [TestMethod]
        public void ECEF_EagerLoad_Off_Does_Not_Update_With_Coordinate_Change()
        {

            Coordinate c = new Coordinate(45, 75, new DateTime(2008, 1, 2));
            var val = c.ECEF;//For comparison
            c.EagerLoadSettings = new EagerLoad(false); //Turn off eager loading
            c.Latitude.DecimalDegree = 44;
            c.Longitude.DecimalDegree = 74;
            Assert.IsTrue(Helpers.ReflectiveEquals(val, c.ECEF));
        }
        /// <summary>
        /// Ensures Cartesian values do not change with coordinate when eagerloading is off.
        /// </summary>
        [TestMethod]
        public void Cartesian_EagerLoad_Off_Does_Not_Update_With_Coordinate_Change()
        {

            Coordinate c = new Coordinate(45, 75, new DateTime(2008, 1, 2));
            var val = c.Cartesian;//For comparison
            c.EagerLoadSettings = new EagerLoad(false); //Turn off eager loading
            c.Latitude.DecimalDegree = 44;
            c.Longitude.DecimalDegree = 74;
            Assert.IsTrue(Helpers.ReflectiveEquals(val, c.Cartesian));
        }
        /// <summary>
        /// Ensures Web Mercator values do not change with coordinate when eagerloading is off.
        /// </summary>
        [TestMethod]
        public void WebMercator_EagerLoad_Off_Does_Not_Update_With_Coordinate_Change()
        {
            Coordinate c = new Coordinate(45, 75, new DateTime(2008, 1, 2));
            var val = c.WebMercator;//For comparison
            c.EagerLoadSettings = new EagerLoad(false); //Turn off eager loading
            c.Latitude.DecimalDegree = 44;
            c.Longitude.DecimalDegree = 74;
            Assert.IsTrue(Helpers.ReflectiveEquals(val, c.WebMercator));
        }

        /// <summary>
        /// Ensures celestial values change with coordinate when eagerloading is turned back on.
        /// </summary>
        [TestMethod]
        public void Celestial_EagerLoad_On_Updates_With_Coordinate_Change()
        {
            EagerLoad el = new EagerLoad(false);
            Coordinate c = new Coordinate(45, 75, new DateTime(2008, 1, 2), el);
            var val = c.CelestialInfo; //For comparison
            c.EagerLoadSettings = new EagerLoad(true); //Turn on eager loading      
            c.Latitude.DecimalDegree = 44;
            c.Longitude.DecimalDegree = 74;
            Assert.IsFalse(Helpers.ReflectiveEquals(val, c.CelestialInfo));
        }
        /// <summary>
        /// Ensures UTM values change with coordinate when eagerloading is turned back on.
        /// </summary>
        [TestMethod]
        public void UTM_EagerLoad_On_Updates_With_Coordinate_Change()
        {
            EagerLoad el = new EagerLoad(false);
            Coordinate c = new Coordinate(45, 75, new DateTime(2008, 1, 2), el);
            var val = c.UTM;//For comparison
            c.EagerLoadSettings = new EagerLoad(true); //Turn on eager loading          
            c.Latitude.DecimalDegree = 44;
            c.Longitude.DecimalDegree = 74;
            Assert.IsFalse(Helpers.ReflectiveEquals(val, c.UTM));
        }
        /// <summary>
        /// Ensures MGRS values change with coordinate when eagerloading is turned back on.
        /// </summary>
        [TestMethod]
        public void MGRS_EagerLoad_On_Updates_With_Coordinate_Change()
        {

            EagerLoad el = new EagerLoad(false);
            Coordinate c = new Coordinate(45, 75, new DateTime(2008, 1, 2), el);
            var val = c.MGRS;//For comparison
            c.EagerLoadSettings = new EagerLoad(true); //Turn on eager loading      
            c.Latitude.DecimalDegree = 44;
            c.Longitude.DecimalDegree = 74;
            Assert.IsFalse(Helpers.ReflectiveEquals(val, c.MGRS));
        }
        /// <summary>
        /// Ensures ECEF values change with coordinate when eagerloading turned back on.
        /// </summary>
        [TestMethod]
        public void ECEF_EagerLoad_On_Updates_With_Coordinate_Change()
        {

            EagerLoad el = new EagerLoad(false);
            Coordinate c = new Coordinate(45, 75, new DateTime(2008, 1, 2), el);
            var val = c.ECEF;//For comparison
            c.EagerLoadSettings = new EagerLoad(true); //Turn on eager loading      
            c.Latitude.DecimalDegree = 44;
            c.Longitude.DecimalDegree = 74;
            Assert.IsFalse(Helpers.ReflectiveEquals(val, c.ECEF));
        }
        /// <summary>
        /// Ensures Cartesian values change with coordinate when eagerloading is turned back on.
        /// </summary>
        [TestMethod]
        public void Cartesian_EagerLoad_On_Updates_With_Coordinate_Change()
        {

            EagerLoad el = new EagerLoad(false);
            Coordinate c = new Coordinate(45, 75, new DateTime(2008, 1, 2), el);
            var val = c.Cartesian;//For comparison
            c.EagerLoadSettings = new EagerLoad(true); //Turn on eager loading      
            c.Latitude.DecimalDegree = 44;
            c.Longitude.DecimalDegree = 74;
            Assert.IsFalse(Helpers.ReflectiveEquals(val, c.Cartesian));
        }

        /// <summary>
        /// Ensures WebMercator values change with coordinate when eagerloading is turned back on.
        /// </summary>
        [TestMethod]
        public void WebMercator_EagerLoad_On_Updates_With_Coordinate_Change()
        {

            EagerLoad el = new EagerLoad(false);
            Coordinate c = new Coordinate(45, 75, new DateTime(2008, 1, 2), el);
            var val = c.WebMercator;//For comparison
            c.EagerLoadSettings = new EagerLoad(true); //Turn on eager loading      
            c.Latitude.DecimalDegree = 44;
            c.Longitude.DecimalDegree = 74;
            Assert.IsFalse(Helpers.ReflectiveEquals(val, c.WebMercator));
        }

        /// <summary>
        /// Ensures flags turn on and of eager loaded properties as specified.
        /// </summary>
        [TestMethod]
        public void Flag_Initialization_Tests()
        {
            EagerLoad eg = new EagerLoad(EagerLoadType.Cartesian | EagerLoadType.Celestial | EagerLoadType.UTM_MGRS | EagerLoadType.ECEF | EagerLoadType.WebMercator);

            if (!eg.Cartesian || !eg.Celestial || !eg.UTM_MGRS || !eg.ECEF || !eg.WebMercator) { Assert.Fail("Property values not expected (1)."); }
            eg = new EagerLoad(EagerLoadType.Celestial);
            if (eg.Cartesian || !eg.Celestial || eg.UTM_MGRS || eg.ECEF || eg.WebMercator) { Assert.Fail("Property values not expected (2)."); }
            eg = new EagerLoad(EagerLoadType.Cartesian);
            if (!eg.Cartesian || eg.Celestial || eg.UTM_MGRS || eg.ECEF || eg.WebMercator) { Assert.Fail("Property values not expected (3)."); }
            eg = new EagerLoad(EagerLoadType.UTM_MGRS);
            if (eg.Cartesian || eg.Celestial || !eg.UTM_MGRS || eg.ECEF || eg.WebMercator) { Assert.Fail("Property values not expected (4)."); }
            eg = new EagerLoad(EagerLoadType.ECEF);
            if (eg.Cartesian || eg.Celestial || eg.UTM_MGRS || !eg.ECEF || eg.WebMercator) { Assert.Fail("Property values not expected (5)."); }
            eg = new EagerLoad(EagerLoadType.UTM_MGRS | EagerLoadType.Celestial);
            if (eg.Cartesian || !eg.Celestial || !eg.UTM_MGRS || eg.ECEF || eg.WebMercator) { Assert.Fail("Property values not expected (6)."); }
            eg = new EagerLoad(EagerLoadType.Cartesian | EagerLoadType.Celestial);
            if (!eg.Cartesian || !eg.Celestial || eg.UTM_MGRS || eg.ECEF || eg.WebMercator) { Assert.Fail("Property values not expected (6)."); }
            eg = new EagerLoad(EagerLoadType.UTM_MGRS | EagerLoadType.Cartesian);
            if (!eg.Cartesian || eg.Celestial || !eg.UTM_MGRS || eg.ECEF || eg.WebMercator) { Assert.Fail("Property values not expected (7)."); }
            eg = new EagerLoad(EagerLoadType.ECEF | EagerLoadType.Celestial);
            if (eg.Cartesian || !eg.Celestial || eg.UTM_MGRS || !eg.ECEF || eg.WebMercator) { Assert.Fail("Property values not expected (8)."); }
            eg = new EagerLoad(EagerLoadType.ECEF | EagerLoadType.Cartesian);
            if (!eg.Cartesian || eg.Celestial || eg.UTM_MGRS || !eg.ECEF || eg.WebMercator) { Assert.Fail("Property values not expected (10)."); }
            eg = new EagerLoad(EagerLoadType.ECEF | EagerLoadType.Cartesian | EagerLoadType.UTM_MGRS);
            if (!eg.Cartesian || eg.Celestial || !eg.UTM_MGRS || !eg.ECEF || eg.WebMercator) { Assert.Fail("Property values not expected (11)."); }
            eg = new EagerLoad(EagerLoadType.ECEF | EagerLoadType.WebMercator | EagerLoadType.UTM_MGRS);
            if (eg.Cartesian || eg.Celestial || !eg.UTM_MGRS || !eg.ECEF || !eg.WebMercator) { Assert.Fail("Property values not expected (12)."); }
            eg = new EagerLoad(EagerLoadType.WebMercator | EagerLoadType.Cartesian | EagerLoadType.UTM_MGRS);
            if (!eg.Cartesian || eg.Celestial || !eg.UTM_MGRS || eg.ECEF || !eg.WebMercator) { Assert.Fail("Property values not expected (13)."); }
        }

        /// <summary>
        /// Ensures Eager Loading switches on and off without exceptions.
        /// </summary>
        [TestMethod]
        public void Switch_Check()
        {
            Coordinate c = new Coordinate(1, 1, new EagerLoad(false));
            c.EagerLoadSettings = new EagerLoad(true);
            c.Latitude.DecimalDegree++;
        }

    }
}
