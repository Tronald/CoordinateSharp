using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoordinateSharp;
using System.Threading;
using System.Globalization;
using System.IO;
namespace CoordinateSharp_UnitTests
{
    [TestClass]
    public class CoordinatePart_Parsers
    {
        /// <summary>
        /// Verifies that Parse is wrapping Try_Parse correctly
        /// </summary>
        [TestMethod]
        public void CoordinatePart_Parse_Wrap_Check()
        {
            CoordinatePart cp = CoordinatePart.Parse("45");
            Assert.AreEqual(45, cp.ToDouble());
            Assert.AreEqual(CoordinatesPosition.N, cp.Position);
         
            cp = CoordinatePart.Parse("45", CoordinateType.Long);
            Assert.AreEqual(45, cp.ToDouble());
            Assert.AreEqual(CoordinatesPosition.E, cp.Position);
        }

        /// <summary>
        /// Ensures parse
        /// </summary>
        [TestMethod]
        public void Signed_Parse()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var lines = File.ReadAllLines("CoordinatePartData\\Signed.txt");
            foreach (string cs in lines)
            {
                TryParse_Check(cs);
                Parse_Check(cs);
            }
        }

        /// <summary>
        /// Ensures parses work in Dutch Culture formatting
        /// </summary>
        [TestMethod]
        public void Signed_Parse_Dutch_Culture()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("nl");
            var lines = File.ReadAllLines("CoordinatePartData\\Signed.txt");
            foreach (string cs in lines)
            {
                TryParse_Check(cs);
                Parse_Check(cs);
            }
        }

        /// <summary>
        /// Ensures parse
        /// </summary>
        [TestMethod]
        public void Signed_DDM_Parse()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var lines = File.ReadAllLines("CoordinatePartData\\SignedDDM.txt");
            foreach (string cs in lines)
            {
                TryParse_Check(cs);
                Parse_Check(cs);
            }
        }

        /// <summary>
        /// Ensures parses work in Dutch Culture formatting
        /// </summary>
        [TestMethod]
        public void Signed_DDM_Parse_Dutch_Culture()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("nl");
            var lines = File.ReadAllLines("CoordinatePartData\\SignedDDM.txt");
            foreach (string cs in lines)
            {
                TryParse_Check(cs);
                Parse_Check(cs);
            }
        }
        /// <summary>
        /// Ensures parse
        /// </summary>
        [TestMethod]
        public void Signed_DMS_Parse()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var lines = File.ReadAllLines("CoordinatePartData\\SignedDMS.txt");
            foreach (string cs in lines)
            {
                TryParse_Check(cs);
                Parse_Check(cs);
            }
        }

        /// <summary>
        /// Ensures parses work in Dutch Culture formatting
        /// </summary>
        [TestMethod]
        public void Signed_DMS_Parse_Dutch_Culture()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("nl");
            var lines = File.ReadAllLines("CoordinatePartData\\SignedDMS.txt");
            foreach (string cs in lines)
            {
                TryParse_Check(cs);
                Parse_Check(cs);
            }
        }

        /// <summary>
        /// Ensures parse
        /// </summary>
        [TestMethod]
        public void Decimal_Degree_Parse()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var lines = File.ReadAllLines("CoordinatePartData\\DD.txt");
            foreach (string cs in lines)
            {
                TryParse_Check(cs);
                Parse_Check(cs);
            }
        }

        /// <summary>
        /// Ensures parses work in Dutch Culture formatting
        /// </summary>
        [TestMethod]
        public void Decimal_Degree_Parse_Dutch_Culture()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("nl");
            var lines = File.ReadAllLines("CoordinatePartData\\DD.txt");
            foreach (string cs in lines)
            {
                TryParse_Check(cs);
                Parse_Check(cs);
            }
        }

        /// <summary>
        /// Ensures parse
        /// </summary>
        [TestMethod]
        public void Degree_Decimal_Minute_Parse()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var lines = File.ReadAllLines("CoordinatePartData\\DDM.txt");
            foreach (string cs in lines)
            {
                TryParse_Check(cs);
                Parse_Check(cs);
            }
        }

        /// <summary>
        /// Ensures parses work in Dutch Culture formatting
        /// </summary>
        [TestMethod]
        public void Degree_Decimal_Minute_Dutch_Culture()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("nl");
            var lines = File.ReadAllLines("CoordinatePartData\\DDM.txt");
            foreach (string cs in lines)
            {
                TryParse_Check(cs);
                Parse_Check(cs);
            }
        }

        /// <summary>
        /// Ensures parse
        /// </summary>
        [TestMethod]
        public void Degree_Minute_Seconds_Parse()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var lines = File.ReadAllLines("CoordinatePartData\\DMS.txt");
            foreach (string cs in lines)
            {
                TryParse_Check(cs);
                Parse_Check(cs);
            }
        }

        /// <summary>
        /// Ensures parses work in Dutch Culture formatting
        /// </summary>
        [TestMethod]
        public void Degree_Minute_Seconds_Dutch_Culture()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("nl");
            var lines = File.ReadAllLines("CoordinatePartData\\DMS.txt");
            foreach (string cs in lines)
            {
                TryParse_Check(cs);
                Parse_Check(cs);
            }
        }

      
        /// <summary>
        /// Splits Coordinate test string to get value to parse and expected output.
        /// Checks proper parsing and value
        /// </summary>
        /// <param name="val">Test string value</param>
        private void Parse_Check(string val)
        {
            var vals = val.Split('#'); //First part of string is value to parse, seconds is expected string value after parse
            string coordString = vals[0];
            string expected = vals[1];

            CoordinatePart c = CoordinatePart.Parse(coordString);//Will throw exception and fail if incorrect (intended behavior)

            //CHECK STRING COMPARISON, BUT REPLACE , and . with * to avoid cultural mismatch
            Assert.AreEqual(expected.Replace(",", "*").Replace(".", "*"), c.ToString().Replace(",", "*").Replace(".", "*"), $"{vals} parsed as {c.ToString().Replace(",", "*").Replace(".", "*")} but {expected.Replace(",", "*").Replace(".", "*")} was expected.");

        }

        /// <summary>
        /// Splits Coordinate test string to get value to parse and expected output.
        /// Checks proper parsing and value
        /// </summary>
        /// <param name="val">Test string value</param>
        private void TryParse_Check(string val)
        {
            var vals = val.Split('#'); //First part of string is value to parse, seconds is expected string value after parse
            string coordString = vals[0];
            string expected = vals[1];
            CoordinatePart c;

            Assert.IsTrue(CoordinatePart.TryParse(coordString, out c), $"{coordString} cannot be parsed.");

            //CHECK STRING COMPARISON, BUT REPLACE , and . with * to avoid cultural mismatch
            Assert.AreEqual(expected.Replace(",", "*").Replace(".", "*"), c.ToString().Replace(",", "*").Replace(".", "*"), $"{vals} parsed as {c.ToString().Replace(",", "*").Replace(".", "*")} but {expected.Replace(",", "*").Replace(".", "*")} was expected.");
        }
    }
}
