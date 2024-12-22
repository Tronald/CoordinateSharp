using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoordinateSharp;
using CoordinateSharp.Magnetic;
using System.Globalization;

namespace CoordinateSharp_UnitTests
{
    

    /// <summary>
    /// Ensure magnetic values
    /// Values for comparison
    /// https://www.ngdc.noaa.gov/geomag/calculators/magcalc.shtml#igrfwmm
    /// TEST CASES INCLUDED IN WMM DOWNLOAD
    /// ENSURE LOCALS AND OFFSETS TEST CORRECTLY
    /// </summary>
    [TestClass]
    public class MagneticTests
    {
        private List<string> Magnetic2020TestValues;
        private List<string> Magnetic2025TestValues; 

        [TestInitialize]
        public void InitializeValues()
        {
            Magnetic2020TestValues = File.ReadAllText("MagneticData\\MagneticFields2020.txt").Split("\n").ToList();
            Magnetic2025TestValues = File.ReadAllText("MagneticData\\MagneticFields2025.txt").Split("\n").ToList();
        }
        /// <summary>
        /// Check magnetic field accuracy for WMM2020.
        /// </summary>
        [TestMethod]
        public void MagneticFields2020()
        {
           
            int x = 1;
            foreach(var line in Magnetic2020TestValues.ToList())
            {
                var items = line.Replace("\r","").Split(" ").Where(x => !string.IsNullOrEmpty(x)).ToList();
                System.Diagnostics.Debug.WriteLine(x);
                DateTime d = Get_Date_From_Year_Fraction(double.Parse(items[0]));
                Coordinate c = new Coordinate(double.Parse(items[2]), double.Parse(items[3]), d, new EagerLoad(false));
                var m = new Magnetic(c, new Distance(double.Parse(items[1]), DistanceType.Kilometers), DataModel.WMM2020).MagneticFieldElements;

                Assert.AreEqual(double.Parse(items[4]), m.Declination, .01, $"Declination exceed delta on iteration {x}.");
                Assert.AreEqual(double.Parse(items[5]), m.Inclination, .01, $"Inclination exceed delta on iteration {x}.");
                Assert.AreEqual(double.Parse(items[6]), m.HorizontalIntensity, 1, $"Horizontal Intensity exceed delta on iteration {x}.");
                Assert.AreEqual(double.Parse(items[7]), m.NorthComponent, 1, $"North Component exceed delta on iteration {x}.");
                Assert.AreEqual(double.Parse(items[8]), m.EastComponent, 1, $"East Component exceed delta on iteration {x}.");
                Assert.AreEqual(double.Parse(items[9]), m.DownComponent, 1, $"Down Component exceed delta on iteration {x}.");
                Assert.AreEqual(double.Parse(items[10]), m.TotalIntensity, 1, $"Total Intensity exceed delta on iteration {x}.");              
                x++;
            }
        }
        /// <summary>
        /// Check magnetic field accuracy for WMM2025.
        /// </summary>
        [TestMethod]
        public void MagneticFields2025()
        {

            int x = 1;
            foreach (var line in Magnetic2025TestValues.ToList())
            {
                var items = line.Replace("\r", "").Split(" ").Where(x => !string.IsNullOrEmpty(x)).ToList();
                System.Diagnostics.Debug.WriteLine(x);
                DateTime d = Get_Date_From_Year_Fraction(double.Parse(items[0]));
                Coordinate c = new Coordinate(double.Parse(items[2]), double.Parse(items[3]), d, new EagerLoad(false));
                var m = new Magnetic(c, new Distance(double.Parse(items[1]), DistanceType.Kilometers), DataModel.WMM2025).MagneticFieldElements;

                Assert.AreEqual(double.Parse(items[4]), m.Declination, .01, $"Declination exceed delta on iteration {x}.");
                Assert.AreEqual(double.Parse(items[5]), m.Inclination, .01, $"Inclination exceed delta on iteration {x}.");
                Assert.AreEqual(double.Parse(items[6]), m.HorizontalIntensity, 1, $"Horizontal Intensity exceed delta on iteration {x}.");
                Assert.AreEqual(double.Parse(items[7]), m.NorthComponent, 1, $"North Component exceed delta on iteration {x}.");
                Assert.AreEqual(double.Parse(items[8]), m.EastComponent, 1, $"East Component exceed delta on iteration {x}.");
                Assert.AreEqual(double.Parse(items[9]), m.DownComponent, 1, $"Down Component exceed delta on iteration {x}.");
                Assert.AreEqual(double.Parse(items[10]), m.TotalIntensity, 1, $"Total Intensity exceed delta on iteration {x}.");
                x++;
            }
        }

        /// <summary>
        /// Check variation accuracy for WMM2020.
        /// </summary>
        [TestMethod]
        public void SecularVariation2020()
        {
      
            int x = 1;
            foreach (var line in Magnetic2020TestValues.ToList())
            {
                var items = line.Replace("\r", "").Split(" ").Where(x => !string.IsNullOrEmpty(x)).ToList();
                System.Diagnostics.Debug.WriteLine(x);
                DateTime d = Get_Date_From_Year_Fraction(double.Parse(items[0]));
                Coordinate c = new Coordinate(double.Parse(items[2]), double.Parse(items[3]), d, new EagerLoad(false));
                var m = new Magnetic(c, new Distance(double.Parse(items[1]), DistanceType.Kilometers), DataModel.WMM2020).SecularVariations;

                Assert.AreEqual(double.Parse(items[11]), m.Declination, .1, $"Declination exceed delta on iteration {x}.");
                Assert.AreEqual(double.Parse(items[12]), m.Inclination, .1, $"Inclination exceed delta on iteration {x}.");
                Assert.AreEqual(double.Parse(items[13]), m.HorizontalIntensity, 1, $"Horizontal Intensity exceed delta on iteration {x}.");
                Assert.AreEqual(double.Parse(items[14]), m.NorthComponent, 1, $"North Component exceed delta on iteration {x}.");
                Assert.AreEqual(double.Parse(items[15]), m.EastComponent, 1, $"East Component exceed delta on iteration {x}.");
                Assert.AreEqual(double.Parse(items[16]), m.DownComponent, 1, $"Down Component exceed delta on iteration {x}.");
                Assert.AreEqual(double.Parse(items[17]), m.TotalIntensity, 1, $"Total Intensity exceed delta on iteration {x}.");
                x++;
            }
        }
        /// <summary>
        /// Check variation accuracy for WMM2025.
        /// </summary>
        [TestMethod]
        public void SecularVariation2025()
        {

            int x = 1;
            foreach (var line in Magnetic2025TestValues.ToList())
            {
                var items = line.Replace("\r", "").Split(" ").Where(x => !string.IsNullOrEmpty(x)).ToList();
                System.Diagnostics.Debug.WriteLine(x);
                DateTime d = Get_Date_From_Year_Fraction(double.Parse(items[0]));
                Coordinate c = new Coordinate(double.Parse(items[2]), double.Parse(items[3]), d, new EagerLoad(false));
                var m = new Magnetic(c, new Distance(double.Parse(items[1]), DistanceType.Kilometers), DataModel.WMM2025).SecularVariations;

                Assert.AreEqual(double.Parse(items[11]), m.Declination, .1, $"Declination exceed delta on iteration {x}.");
                Assert.AreEqual(double.Parse(items[12]), m.Inclination, .1, $"Inclination exceed delta on iteration {x}.");
                Assert.AreEqual(double.Parse(items[13]), m.HorizontalIntensity, 1, $"Horizontal Intensity exceed delta on iteration {x}.");
                Assert.AreEqual(double.Parse(items[14]), m.NorthComponent, 1, $"North Component exceed delta on iteration {x}.");
                Assert.AreEqual(double.Parse(items[15]), m.EastComponent, 1, $"East Component exceed delta on iteration {x}.");
                Assert.AreEqual(double.Parse(items[16]), m.DownComponent, 1, $"Down Component exceed delta on iteration {x}.");
                Assert.AreEqual(double.Parse(items[17]), m.TotalIntensity, 1, $"Total Intensity exceed delta on iteration {x}.");
                x++;
            }
        }

        /// <summary>
        /// Check uncertainty accuracy WMM2020.
        /// </summary>
        [TestMethod]
        public void Uncertainty2020()
        {
            //All values static accept dec. Grab Dec variations for 2020 2022 and 2024
            Coordinate c = new Coordinate(-45, 20, new DateTime(2020, 1, 1), new EagerLoad(false));
            Magnetic m = new Magnetic(c, DataModel.WMM2020);
            Assert.AreEqual(.51, m.Uncertainty.Declination, .01, "Declination attempt 1 exceeds delta.");
            Assert.AreEqual(.21, m.Uncertainty.Inclination, "Inclination incorrect.");
            Assert.AreEqual(128, m.Uncertainty.HorizontalIntensity, "Inclination incorrect.");
            Assert.AreEqual(131, m.Uncertainty.NorthComponent, "North Component incorrect.");
            Assert.AreEqual(94, m.Uncertainty.EastComponent, "East Component incorrect.");
            Assert.AreEqual(157, m.Uncertainty.DownComponent, "Down Component incorrect.");
            Assert.AreEqual(145, m.Uncertainty.TotalIntensity, "Total Intensity incorrect.");


            

            c = new Coordinate(45, -120, new DateTime(2022, 1, 1), new EagerLoad(false));
            m = new Magnetic(c, DataModel.WMM2020);
            Assert.AreEqual(.38, m.Uncertainty.Declination, .01, "Declination attempt 2 exceeds delta.");

            c = new Coordinate(82, 120, new DateTime(2024, 1, 1), new EagerLoad(false));
            m = new Magnetic(c, DataModel.WMM2020);
            Assert.AreEqual(5.13, m.Uncertainty.Declination, .01, "Declination attempt 3 exceeds delta.");

        }
        /// <summary>
        /// Check uncertainty accuracy WMM2025.
        /// </summary>
        [TestMethod]
        public void Uncertainty2025()
        {
          
            Coordinate c = new Coordinate(-45, 20, new DateTime(2025, 1, 1), new EagerLoad(false));
            Magnetic m = new Magnetic(c, DataModel.WMM2025);

            Assert.AreEqual(.49, m.Uncertainty.Declination, .01, "Declination attempt 1 exceeds delta.");

            Assert.AreEqual(.2, m.Uncertainty.Inclination, "Inclination incorrect.");
            Assert.AreEqual(133, m.Uncertainty.HorizontalIntensity, "Inclination incorrect.");
            Assert.AreEqual(137, m.Uncertainty.NorthComponent, "North Component incorrect.");
            Assert.AreEqual(89, m.Uncertainty.EastComponent, "East Component incorrect.");
            Assert.AreEqual(141, m.Uncertainty.DownComponent, "Down Component incorrect.");
            Assert.AreEqual(138, m.Uncertainty.TotalIntensity, "Total Intensity incorrect.");


           

            c = new Coordinate(45, -120, new DateTime(2027, 1, 1), new EagerLoad(false));
            m = new Magnetic(c, DataModel.WMM2025);
            Assert.AreEqual(.38, m.Uncertainty.Declination, .01, "Declination attempt 2 exceeds delta.");

            c = new Coordinate(82, 120, new DateTime(2029, 1, 1), new EagerLoad(false));
            m = new Magnetic(c, DataModel.WMM2025);
            Assert.AreEqual(6.04, m.Uncertainty.Declination, .01, "Declination attempt 3 exceeds delta.");

        }

        /// <summary>
        /// Check Grid Variation WMM2020.
        /// </summary>
        [TestMethod]
        public void GridVariation2020()
        {
            Coordinate c = new Coordinate(89, -121, new DateTime(2020, 1, 1), new EagerLoad(false));
            Magnetic m = new Magnetic(c, DataModel.WMM2020);
            Assert.AreEqual(9.08, m.MagneticFieldElements.GridVariation,.01, "Attempt 1 exceeds delta.");

            c = new Coordinate(80, 0, new DateTime(2020, 1, 1), new EagerLoad(false));
            m = new Magnetic(c, DataModel.WMM2020);
            Assert.AreEqual(-1.28, m.MagneticFieldElements.GridVariation, .01, "Attempt 2 exceeds delta.");

            c = new Coordinate(-80, -120, new DateTime(2020, 1, 1), new EagerLoad(false));
            m = new Magnetic(c, DataModel.WMM2020);
            Assert.AreEqual(-50.64, m.MagneticFieldElements.GridVariation, .01, "Attempt 3 exceeds delta.");

            c = new Coordinate(20, -121, new DateTime(2020, 1, 1), new EagerLoad(false));
            m = new Magnetic(c, DataModel.WMM2020);
            Assert.AreEqual(m.MagneticFieldElements.Declination, m.MagneticFieldElements.GridVariation, "Attempt 4 exceeds delta.");
        }
        /// <summary>
        /// Check Grid Variation WMM2025.
        /// </summary>
        [TestMethod]
        public void GridVariation2025()
        {
            Coordinate c = new Coordinate(89, -121, new DateTime(2025, 1, 1), new EagerLoad(false));
            Magnetic m = new Magnetic(c, DataModel.WMM2025);
            Assert.AreEqual(21.52, m.MagneticFieldElements.GridVariation, .01, "Attempt 1 exceeds delta.");

            c = new Coordinate(80, 0, new DateTime(2025, 1, 1), new EagerLoad(false));
            m = new Magnetic(c, DataModel.WMM2025);
            Assert.AreEqual(1.28, m.MagneticFieldElements.GridVariation, .01, "Attempt 2 exceeds delta.");

            c = new Coordinate(-80, -120, new DateTime(2025, 1, 1), new EagerLoad(false));
            m = new Magnetic(c, DataModel.WMM2025);
            Assert.AreEqual(-51.22, m.MagneticFieldElements.GridVariation, .01, "Attempt 3 exceeds delta.");

            c = new Coordinate(20, -121, new DateTime(2025, 1, 1), new EagerLoad(false));
            m = new Magnetic(c, DataModel.WMM2025);
            Assert.AreEqual(m.MagneticFieldElements.Declination, m.MagneticFieldElements.GridVariation, "Attempt 4 exceeds delta.");
        }

        /// <summary>
        /// Check overloads are accurate.
        /// </summary>
        [TestMethod]
        public void Overload_Constructors()
        {
            Coordinate c = new Coordinate(89, -121, new DateTime(2025, 1, 1), new EagerLoad(false));
            Magnetic m1 = new Magnetic(c, DataModel.WMM2025);
            Magnetic m2 = new Magnetic(89,-121, new DateTime(2025,1,1), 0,0 , DataModel.WMM2025);
            Magnetic m3 = new Magnetic(c);
            Magnetic m4 = new Magnetic(89, -121, new DateTime(2025, 1, 1), 0, 0);
            Magnetic m5 = new Magnetic(c, (double)0);
            Magnetic m6 = new Magnetic(c, new Distance(0));
            Magnetic m7 = new Magnetic(c, new Distance(0), DataModel.WMM2025);
            Magnetic m8 = new Magnetic(c, (double)0, DataModel.WMM2025);
          
            Assert.IsTrue(Helpers.ReflectiveEquals(m1.MagneticFieldElements, m2.MagneticFieldElements));
            Assert.IsTrue(Helpers.ReflectiveEquals(m1.MagneticFieldElements, m3.MagneticFieldElements));
            Assert.IsTrue(Helpers.ReflectiveEquals(m1.MagneticFieldElements, m4.MagneticFieldElements));
            Assert.IsTrue(Helpers.ReflectiveEquals(m1.MagneticFieldElements, m5.MagneticFieldElements));
            Assert.IsTrue(Helpers.ReflectiveEquals(m1.MagneticFieldElements, m6.MagneticFieldElements));
            Assert.IsTrue(Helpers.ReflectiveEquals(m1.MagneticFieldElements, m7.MagneticFieldElements));
            Assert.IsTrue(Helpers.ReflectiveEquals(m1.MagneticFieldElements, m8.MagneticFieldElements));
        }

        /// <summary>
        /// Check local time accuracy.
        /// </summary>
        [TestMethod]
        public void Local_Time()
        {
            Coordinate c1 = new Coordinate(89, -121, new DateTime(2020, 1, 1), new EagerLoad(false));
            c1.Offset = -8;
            Coordinate c2 = new Coordinate(89, -121, new DateTime(2020, 1, 1, 8, 0, 0), new EagerLoad(false));

            Magnetic m1 = new Magnetic(c1, DataModel.WMM2020);
            Magnetic m2 = new Magnetic(c2, DataModel.WMM2020);

            Assert.IsTrue(Helpers.ReflectiveEquals(m1.MagneticFieldElements, m2.MagneticFieldElements), "Failed on attempt 1.");

            m1 = new Magnetic(45, 45, new DateTime(2020, 1, 1), -8, 100, DataModel.WMM2020);
            m2 = new Magnetic(45, 45, new DateTime(2020, 1, 1, 8, 0, 0), 0, 100, DataModel.WMM2020);

            Assert.IsTrue(Helpers.ReflectiveEquals(m1.MagneticFieldElements, m2.MagneticFieldElements), "Failed on attempt 2.");
        }

        /// <summary>
        /// Check extensions work.
        /// </summary>
        [TestMethod]
        public void Coordinate_Extensions()
        {
            Coordinate c = new Coordinate(89, -121, new DateTime(2020, 1, 1), new EagerLoad(false));
            Magnetic m1 = new Magnetic(c, DataModel.WMM2020);
            Magnetic m2 = c.GetMagnetic(DataModel.WMM2020);
            Assert.IsTrue(Helpers.ReflectiveEquals(m1.MagneticFieldElements, m2.MagneticFieldElements), "Failed on attempt 1.");

            m1 = new Magnetic(c,10000, DataModel.WMM2020);
            m2 = c.GetMagnetic(10000, DataModel.WMM2020);
            Assert.IsTrue(Helpers.ReflectiveEquals(m1.MagneticFieldElements, m2.MagneticFieldElements), "Failed on attempt 2.");

            m1 = new Magnetic(c, new Distance(100), DataModel.WMM2020);
            m2 = c.GetMagnetic(new Distance(100), DataModel.WMM2020);
            Assert.IsTrue(Helpers.ReflectiveEquals(m1.MagneticFieldElements, m2.MagneticFieldElements), "Failed on attempt 3.");
        }

        private DateTime Get_Date_From_Year_Fraction(double year)
        {
           
            double mf = year - Math.Floor(year);
            int d = 1;
            int m = 1;
            if (mf != 0) { d = 2; m = 7; }
            return new DateTime((int)Math.Floor(year), m, d);
        }
    }
}
