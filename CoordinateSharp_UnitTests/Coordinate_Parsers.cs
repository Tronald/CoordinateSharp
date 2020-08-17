using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using CoordinateSharp;
namespace CoordinateSharp_UnitTests
{
    [TestClass]
    public class Coordinate_Parsers
    {
        /// <summary>
        /// Ensures coordinate does not parse null or empty.
        /// </summary>
        [TestMethod]
        public void Does_Not_Parse_Null_Or_Empty_Strings()
        {
            Assert.ThrowsException<FormatException>(() => Coordinate.Parse(null));
            Assert.ThrowsException<FormatException>(() => Coordinate.Parse(""));
            Coordinate c;
            Assert.IsFalse(Coordinate.TryParse(null, out c));
            Assert.IsFalse(Coordinate.TryParse("", out c));
        }

        /// <summary>
        /// Check that MGRS will parse if Easting and Northing values are not provided
        /// </summary>
        [TestMethod]
        public void MGRS_Parses_With_No_Easting_Northing_Provided()
        {
            Coordinate c = null;

            Assert.IsTrue(Coordinate.TryParse("38TNG", out c));
            Assert.IsTrue(Coordinate.TryParse("38T NG", out c));
            Assert.IsTrue(Coordinate.TryParse("38 T NG", out c));

            Assert.IsFalse(Coordinate.TryParse("38TNG0", out c));
            Assert.IsFalse(Coordinate.TryParse("38T NG 0", out c));
            Assert.IsFalse(Coordinate.TryParse("38 T NG 0", out c));
        }

        /// <summary>
        /// Check that MGRS Polar will parse if Easting and Northing values are not provided
        /// </summary>
        [TestMethod]
        public void MGRS_Polar_Parses_With_No_Easting_Northing_Provided()
        {
            Coordinate c = null;

            Assert.IsTrue(Coordinate.TryParse("ZFG", out c));
            Assert.IsTrue(Coordinate.TryParse("Z FG", out c));
           
            Assert.IsFalse(Coordinate.TryParse("ZFG0", out c));
            Assert.IsFalse(Coordinate.TryParse("Z FG 0", out c));          
        }
    }
}
