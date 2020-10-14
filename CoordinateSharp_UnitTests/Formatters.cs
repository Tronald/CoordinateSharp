using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CoordinateSharp;
using CoordinateSharp.Formatters;

namespace CoordinateSharp_UnitTests
{
    [TestClass]
    public class Formatters
    {
      

        [TestMethod]
        public void DegreesToRadian()
        {
            double degree = 45.258;
            Assert.AreEqual(.7899011, Format.ToRadians(degree), .0000001);
        }

        [TestMethod]
        public void RadiansToDegrees()
        {

            double rad = .7899011;
            Assert.AreEqual(45.258, Format.ToDegrees(rad), .000001);
        }

        [TestMethod]
        public void DDMtoSigned()
        {
            Coordinate c = new Coordinate(45.234, -34.567, new EagerLoad(false));

            double[] ddm1 = new double[] { Math.Truncate(c.Latitude.DecimalDegree), c.Latitude.DecimalMinute };
            double[] ddm2 = new double[] { Math.Truncate(c.Longitude.DecimalDegree), c.Longitude.DecimalMinute };

            Assert.AreEqual(Format.ToDegrees((int)ddm1[0],ddm1[1]), c.Latitude.ToDouble(), .000001);
            Assert.AreEqual(Format.ToDegrees((int)ddm2[0], ddm2[1]), c.Longitude.ToDouble(), .000001);

        }

        [TestMethod]
        public void DMStoSigned()
        {
            Coordinate c = new Coordinate(45.234, -34.567, new EagerLoad(false));

            double[] ddm1 = new double[] { Math.Truncate(c.Latitude.DecimalDegree), c.Latitude.Minutes, c.Latitude.Seconds};
            double[] ddm2 = new double[] { Math.Truncate(c.Longitude.DecimalDegree), c.Longitude.Minutes, c.Latitude.Seconds};

            Assert.AreEqual(Format.ToDegrees((int)ddm1[0], (int)ddm1[1], ddm1[2]), c.Latitude.ToDouble(), .0004);
            Assert.AreEqual(Format.ToDegrees((int)ddm2[0], (int)ddm2[1], ddm2[2]), c.Longitude.ToDouble(), .0004);

        }

        [TestMethod]
        public void SignedtoDDM()
        {
            Coordinate c = new Coordinate(45.234, -34.567, new EagerLoad(false));
            var lat = Format.ToDegreeDecimalMinutes(c.Latitude.ToDouble());
            var lng = Format.ToDegreeDecimalMinutes(c.Longitude.ToDouble());

            Assert.AreEqual(Math.Truncate(c.Latitude.DecimalDegree), lat[0]);
            Assert.AreEqual(c.Latitude.DecimalMinute, lat[1], .0000001);

            Assert.AreEqual(Math.Truncate(c.Longitude.DecimalDegree), lng[0]);       
            Assert.AreEqual(c.Longitude.DecimalMinute, -lng[1], .0000001);
        }

        [TestMethod]
        public void SignedtoDMS()
        {
            Coordinate c = new Coordinate(45.234, -34.567, new EagerLoad(false));
            var lat = Format.ToDegreesMinutesSeconds(c.Latitude.ToDouble());
            var lng = Format.ToDegreesMinutesSeconds(c.Longitude.ToDouble());

            Assert.AreEqual(Math.Truncate(c.Latitude.DecimalDegree), lat[0]);
            Assert.AreEqual(c.Latitude.Minutes, lat[1]);
            Assert.AreEqual(c.Latitude.Seconds, lat[2], .0000001);

            Assert.AreEqual(Math.Truncate(c.Longitude.DecimalDegree), lng[0]);
            Assert.AreEqual(c.Longitude.Minutes, -lng[1]);
            Assert.AreEqual(c.Longitude.Seconds, -lng[2], .0000001);
        }

        [TestMethod]
        public void SignedtoHMS()
        {
            double signed = -161.61917;
            var hms = Format.ToHoursMinutesSeconds(signed);

            Assert.AreEqual(-10, hms[0]);
            Assert.AreEqual(-46, hms[1]);
            Assert.AreEqual(-28.60068, hms[2],.001);
        }

        [TestMethod]
        public void SignedtoHMS_AfterNormailize()
        {
            double signed = -161.61917;
            var hms = Format.ToHoursMinutesSeconds(Format.NormalizeDegrees360(signed));

            Assert.AreEqual(13, hms[0]);
            Assert.AreEqual(13, hms[1]);
            Assert.AreEqual(31.4, hms[2], .001);
        }

        [TestMethod]
        public void Normalize360()
        {
            double signed = -2318.19280;
            Assert.AreEqual(201.80720, Format.NormalizeDegrees360(signed),.000000001);
        }
    }
}
