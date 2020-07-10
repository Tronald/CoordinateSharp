using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CoordinateSharp;
using System.Reflection;

namespace CoordinateSharp_UnitTests
{
    [TestClass]
    public class EagerLoading_Extensions
    {
        /// <summary>
        /// Tests to determine if eager load extension properly intializes
        /// </summary>
        [TestMethod]
        public void EagerLoading_Ext_Intializes_On()
        {
            EagerLoad e = new EagerLoad();
            e.Extensions = new EagerLoad_Extensions();
            Coordinate c = new Coordinate(45, 75, new DateTime(2008, 1, 2), e);

            //Check extension properties to ensure proper loading
            Assert.AreNotEqual(null, c.CelestialInfo.SunSet);//Solar Cycle
            Assert.AreNotEqual(null, c.CelestialInfo.MoonSet);//Lunar Cycle
            Assert.AreNotEqual(null, c.CelestialInfo.AstrologicalSigns.MoonName);//Zodiac
            Assert.AreNotEqual(null, c.CelestialInfo.LunarEclipse.LastEclipse.Type);//Lunar Cycle
            Assert.AreNotEqual(null, c.CelestialInfo.SolarEclipse.LastEclipse.Type);//Solar Cycle
            Assert.AreNotEqual(null, c.CelestialInfo.Solstices.Summer);//Lunar Cycle
            Assert.AreNotEqual(null, c.MGRS); //MGRS                  
        }
        /// <summary>
        /// Tests to determine if eager load extensions properly turns off.
        /// </summary>
        [TestMethod]
        public void EagerLoading_Ext_Off()
        {
            EagerLoad e = new EagerLoad(true);
            e.Extensions = new EagerLoad_Extensions(false);
            Coordinate c = new Coordinate(45, 75, new DateTime(2008, 1, 2), e);
            c.Latitude.DecimalDegree++; //Trigger proper changes to confirm objects remain unloaded
            c.Longitude.DecimalDegree++; //Trigger proper changes to confirm objects remain unloaded
            Assert.AreEqual(null, c.CelestialInfo.SunSet);//Solar Cycle
            Assert.AreEqual(null, c.CelestialInfo.MoonSet);//Lunar Cycle
            Assert.AreEqual(null, c.CelestialInfo.AstrologicalSigns.MoonName);//Zodiac
            Assert.AreEqual(new DateTime(), c.CelestialInfo.LunarEclipse.LastEclipse.Date);//Lunar Cycle
            Assert.AreEqual(new DateTime(), c.CelestialInfo.SolarEclipse.LastEclipse.Date);//Solar Eclipse
            Assert.AreEqual(new DateTime(), c.CelestialInfo.Solstices.Summer);//Solstice Equinox
            Assert.AreEqual(null, c.MGRS); //MGRS                   
        }

        /// <summary>
        /// Ensures Solar Cycle Extension properly turns back on without turning other extensions on
        /// </summary>
        [TestMethod]
        public void Solar_Cycle_Extension_On()
        {
            EagerLoad e = new EagerLoad(true);
            e.Extensions = new EagerLoad_Extensions(false);
            Coordinate c = new Coordinate(45, 75, new DateTime(2008, 1, 2), e);
            e.Extensions.Solar_Cycle = true;
            c.Latitude.DecimalDegree++; //Trigger proper changes to confirm objects remain unloaded
            c.Longitude.DecimalDegree++; //Trigger proper changes to confirm objects remain unloaded

            Assert.AreNotEqual(null, c.CelestialInfo.SunSet);//Solar Cycle
           
            Assert.AreEqual(null, c.CelestialInfo.MoonSet);//Lunar Cycle
            Assert.AreEqual(null, c.CelestialInfo.AstrologicalSigns.MoonName);//Zodiac
            Assert.AreEqual(new DateTime(), c.CelestialInfo.LunarEclipse.LastEclipse.Date);//Lunar Cycle
            Assert.AreEqual(new DateTime(), c.CelestialInfo.SolarEclipse.LastEclipse.Date);//Solar Eclipse
            Assert.AreEqual(new DateTime(), c.CelestialInfo.Solstices.Summer);//Solstice Equinox
            Assert.AreEqual(null, c.MGRS); //MGRS                   
        }

        /// <summary>
        /// Ensures Lunar Cycle Extension properly turns back on without turning other extensions on
        /// </summary>
        [TestMethod]
        public void Lunar_Cycle_Extension_On()
        {
            EagerLoad e = new EagerLoad(true);
            e.Extensions = new EagerLoad_Extensions(false);
            Coordinate c = new Coordinate(45, 75, new DateTime(2008, 1, 2), e);
            e.Extensions.Lunar_Cycle = true;
            c.Latitude.DecimalDegree++; //Trigger proper changes to confirm objects remain unloaded
            c.Longitude.DecimalDegree++; //Trigger proper changes to confirm objects remain unloaded

            Assert.AreNotEqual(null, c.CelestialInfo.MoonSet);//Lunar Cycle

            Assert.AreEqual(null, c.CelestialInfo.SunSet);//Solar Cycle
            Assert.AreEqual(null, c.CelestialInfo.AstrologicalSigns.MoonName);//Zodiac
            Assert.AreEqual(new DateTime(), c.CelestialInfo.LunarEclipse.LastEclipse.Date);//Lunar Cycle
            Assert.AreEqual(new DateTime(), c.CelestialInfo.SolarEclipse.LastEclipse.Date);//Solar Eclipse
            Assert.AreEqual(new DateTime(), c.CelestialInfo.Solstices.Summer);//Solstice Equinox
            Assert.AreEqual(null, c.MGRS); //MGRS                   
        }

        /// <summary>
        /// Ensures Solar Eclipse Extension properly turns back on without turning other extensions on
        /// </summary>
        [TestMethod]
        public void Solar_Eclipse_Extension_On()
        {
            EagerLoad e = new EagerLoad(true);
            e.Extensions = new EagerLoad_Extensions(false);
            Coordinate c = new Coordinate(45, 75, new DateTime(2008, 1, 2), e);
            e.Extensions.Solar_Eclipse = true;
            c.Latitude.DecimalDegree++; //Trigger proper changes to confirm objects remain unloaded
            c.Longitude.DecimalDegree++; //Trigger proper changes to confirm objects remain unloaded

            Assert.AreNotEqual(new DateTime(), c.CelestialInfo.SolarEclipse.LastEclipse.Date);//Solar Eclipse

            Assert.AreEqual(null, c.CelestialInfo.SunSet);//Solar Cycle
            Assert.AreEqual(null, c.CelestialInfo.MoonSet);//Lunar Cycle
            Assert.AreEqual(null, c.CelestialInfo.AstrologicalSigns.MoonName);//Zodiac
            Assert.AreEqual(new DateTime(), c.CelestialInfo.LunarEclipse.LastEclipse.Date);//Lunar Cycle
            Assert.AreEqual(new DateTime(), c.CelestialInfo.Solstices.Summer);//Solstice Equinox
            Assert.AreEqual(null, c.MGRS); //MGRS                   
        }

        /// <summary>
        /// Ensures Lunar Eclipse Extension properly turns back on without turning other extensions on
        /// </summary>
        [TestMethod]
        public void Lunar_Eclipse_Extension_On()
        {
            EagerLoad e = new EagerLoad(true);
            e.Extensions = new EagerLoad_Extensions(false);
            Coordinate c = new Coordinate(45, 75, new DateTime(2008, 1, 2), e);
            e.Extensions.Lunar_Eclipse = true;
            c.Latitude.DecimalDegree++; //Trigger proper changes to confirm objects remain unloaded
            c.Longitude.DecimalDegree++; //Trigger proper changes to confirm objects remain unloaded

            Assert.AreNotEqual(new DateTime(), c.CelestialInfo.LunarEclipse.LastEclipse.Date);//Lunar Cycle

            Assert.AreEqual(null, c.CelestialInfo.SunSet);//Solar Cycle
            Assert.AreEqual(null, c.CelestialInfo.MoonSet);//Lunar Cycle
            Assert.AreEqual(null, c.CelestialInfo.AstrologicalSigns.MoonName);//Zodiac
            Assert.AreEqual(new DateTime(), c.CelestialInfo.SolarEclipse.LastEclipse.Date);//Solar Eclipse
            Assert.AreEqual(new DateTime(), c.CelestialInfo.Solstices.Summer);//Solstice Equinox
            Assert.AreEqual(null, c.MGRS); //MGRS                   
        }

        /// <summary>
        /// Ensures Solstice Equinox Extension properly turns back on without turning other extensions on
        /// </summary>
        [TestMethod]
        public void Solstice_Equinox_Extension_On()
        {
            EagerLoad e = new EagerLoad(true);
            e.Extensions = new EagerLoad_Extensions(false);
            Coordinate c = new Coordinate(45, 75, new DateTime(2008, 1, 2), e);
            e.Extensions.Solstice_Equinox = true;
            c.Latitude.DecimalDegree++; //Trigger proper changes to confirm objects remain unloaded
            c.Longitude.DecimalDegree++; //Trigger proper changes to confirm objects remain unloaded

            Assert.AreNotEqual(new DateTime(), c.CelestialInfo.Solstices.Summer);//Solstice Equinox
        
            Assert.AreEqual(new DateTime(), c.CelestialInfo.SolarEclipse.LastEclipse.Date);//Solar Eclipse
            Assert.AreEqual(null, c.CelestialInfo.SunSet);//Solar Cycle
            Assert.AreEqual(null, c.CelestialInfo.MoonSet);//Lunar Cycle
            Assert.AreEqual(null, c.CelestialInfo.AstrologicalSigns.MoonName);//Zodiac
            Assert.AreEqual(new DateTime(), c.CelestialInfo.LunarEclipse.LastEclipse.Date);//Lunar Eclipse      
            Assert.AreEqual(null, c.MGRS); //MGRS                   
        }

        /// <summary>
        /// Ensures Zodiac Extension properly turns back on without turning other extensions on
        /// </summary>
        [TestMethod]
        public void Zodiac_Extension_On()
        {
            EagerLoad e = new EagerLoad(true);
            e.Extensions = new EagerLoad_Extensions(false);
            Coordinate c = new Coordinate(45, 75, new DateTime(2008, 1, 2), e);
            e.Extensions.Zodiac = true;
            c.Latitude.DecimalDegree++; //Trigger proper changes to confirm objects remain unloaded
            c.Longitude.DecimalDegree++; //Trigger proper changes to confirm objects remain unloaded

            Assert.AreNotEqual(null, c.CelestialInfo.AstrologicalSigns.MoonName);//Zodiac

            Assert.AreEqual(null, c.CelestialInfo.SunSet);//Solar Cycle
            Assert.AreEqual(null, c.CelestialInfo.MoonSet);//Lunar Cycle         
            Assert.AreEqual(new DateTime(), c.CelestialInfo.SolarEclipse.LastEclipse.Date);//Solar Eclipse
            Assert.AreEqual(new DateTime(), c.CelestialInfo.LunarEclipse.LastEclipse.Date);//Lunar Eclipse  
            Assert.AreEqual(new DateTime(), c.CelestialInfo.Solstices.Summer);//Solstice Equinox
            Assert.AreEqual(null, c.MGRS); //MGRS                   
        }

        /// <summary>
        /// Ensures MGRS Extension properly turns back on without turning other extensions on
        /// </summary>
        [TestMethod]
        public void MGRS_Extension_On()
        {
            EagerLoad e = new EagerLoad(true);
            e.Extensions = new EagerLoad_Extensions(false);
            Coordinate c = new Coordinate(45, 75, new DateTime(2008, 1, 2), e);
            e.Extensions.MGRS = true;
            c.Latitude.DecimalDegree++; //Trigger proper changes to confirm objects remain unloaded
            c.Longitude.DecimalDegree++; //Trigger proper changes to confirm objects remain unloaded

            Assert.AreNotEqual(null, c.MGRS); //MGRS     

            Assert.AreEqual(null, c.CelestialInfo.AstrologicalSigns.MoonName);//Zodiac
            Assert.AreEqual(null, c.CelestialInfo.SunSet);//Solar Cycle
            Assert.AreEqual(null, c.CelestialInfo.MoonSet);//Lunar Cycle         
            Assert.AreEqual(new DateTime(), c.CelestialInfo.SolarEclipse.LastEclipse.Date);//Solar Eclipse
            Assert.AreEqual(new DateTime(), c.CelestialInfo.LunarEclipse.LastEclipse.Date);//Lunar Eclipse  
            Assert.AreEqual(new DateTime(), c.CelestialInfo.Solstices.Summer);//Solstice Equinox
                         
        }
    }
}
