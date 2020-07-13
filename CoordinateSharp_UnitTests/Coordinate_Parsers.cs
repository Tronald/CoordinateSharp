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
    }
}
