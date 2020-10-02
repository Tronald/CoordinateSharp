using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoordinateSharp;
namespace CoordinateSharp_UnitTests
{
    [TestClass]
    public class GeoFenceTests
    {
        /// <summary>
        /// Ensure points remain outside polygon.
        /// </summary>
        [TestMethod]
        public void Outside_Polygon()
        {
            // CHANGE LATS e LONS
            Coordinate c1 = new Coordinate(31.66, -106.52, new EagerLoad(false));
            Coordinate c2 = new Coordinate(31.64, -106.53, new EagerLoad(false));
            Coordinate c3 = new Coordinate(42.02, -106.52, new EagerLoad(false));
            Coordinate c4 = new Coordinate(42.03, -106.53, new EagerLoad(false));

            List<GeoFence.Point> points = new List<GeoFence.Point>();

            points.Add(new GeoFence.Point(31.65, -106.52));
            points.Add(new GeoFence.Point(31.65, -84.02));
            points.Add(new GeoFence.Point(42.03, -84.02));
            points.Add(new GeoFence.Point(42.03, -106.52));
            points.Add(new GeoFence.Point(31.65, -106.52));

            GeoFence gf = new GeoFence(points);

            Assert.IsFalse(gf.IsPointInPolygon(c1));
            Assert.IsFalse(gf.IsPointInPolygon(c2));
            Assert.IsFalse(gf.IsPointInPolygon(c3));
            Assert.IsFalse(gf.IsPointInPolygon(c4));         
        }

        /// <summary>
        /// Ensure points remain inside polygon.
        /// </summary>
        [TestMethod]
        public void Inside_Polygon()
        {
          
            Coordinate c1 = new Coordinate(31.67, -106.51, new EagerLoad(false));
            Coordinate c2 = new Coordinate(31.67, -84.03, new EagerLoad(false));
            Coordinate c3 = new Coordinate(42.01, -106.51, new EagerLoad(false));
            Coordinate c4 = new Coordinate(42.01, -84.03, new EagerLoad(false));

            List<GeoFence.Point> points = new List<GeoFence.Point>();

            points.Add(new GeoFence.Point(31.65, -106.52));
            points.Add(new GeoFence.Point(31.65, -84.02));
            points.Add(new GeoFence.Point(42.03, -84.02));
            points.Add(new GeoFence.Point(42.03, -106.52));
            points.Add(new GeoFence.Point(31.65, -106.52));

            GeoFence gf = new GeoFence(points);

            Assert.IsTrue(gf.IsPointInPolygon(c1));
            Assert.IsTrue(gf.IsPointInPolygon(c2));
            Assert.IsTrue(gf.IsPointInPolygon(c3));
            Assert.IsTrue(gf.IsPointInPolygon(c4));
        }

        /// <summary>
        /// Ensures polyline in range.
        /// </summary>
        [TestMethod]
        public void Polyline_In_Range()
        {

            Coordinate c1 = new Coordinate(31.67, -106.51, new EagerLoad(false));        

            List<GeoFence.Point> points = new List<GeoFence.Point>();

            points.Add(new GeoFence.Point(31.65, -106.52));
            points.Add(new GeoFence.Point(31.65, -84.02));
            points.Add(new GeoFence.Point(42.03, -84.02));
            points.Add(new GeoFence.Point(42.03, -106.52));
            points.Add(new GeoFence.Point(31.65, -106.52));

            GeoFence gf = new GeoFence(points);
            Distance d = new Distance(1000, DistanceType.Meters);

            Assert.IsTrue(gf.IsPointInRangeOfLine(c1,1000));
            Assert.IsTrue(gf.IsPointInRangeOfLine(c1, d));
        }

        /// <summary>
        /// Ensures polyline out of range.
        /// </summary>
        [TestMethod]
        public void Polyline_Out_Range()
        {

            Coordinate c1 = new Coordinate(31.67, -106.51, new EagerLoad(false));

            List<GeoFence.Point> points = new List<GeoFence.Point>();

            points.Add(new GeoFence.Point(31.65, -106.52));
            points.Add(new GeoFence.Point(31.65, -84.02));
            points.Add(new GeoFence.Point(42.03, -84.02));
            points.Add(new GeoFence.Point(42.03, -106.52));
            points.Add(new GeoFence.Point(31.65, -106.52));

            GeoFence gf = new GeoFence(points);
            Distance d = new Distance(900, DistanceType.Meters);

            Assert.IsFalse(gf.IsPointInRangeOfLine(c1, 900));
            Assert.IsFalse(gf.IsPointInRangeOfLine(c1, d));
        }

        /// <summary>
        /// Ensures Haversine precision withing bounds
        /// </summary>
        [TestMethod]
        public void Haversine_Precision()
        {
            Coordinate c = new Coordinate(25, 45, new EagerLoad(false));

            //Check Haversine
            var gd = new GeoFence.Drawer(c, Shape.Sphere, 0);

            gd.Draw(new Distance(20), 0);
            gd.Draw(new Distance(20), 90);
            gd.Draw(new Distance(20), 90);
            gd.Draw(new Distance(20), 90);

            Assert.AreEqual(0, c.Latitude.ToDouble() - gd.Last.Latitude.ToDouble(), .000001);
            Assert.AreEqual(0, c.Longitude.ToDouble() - gd.Last.Longitude.ToDouble(), .000001);            
        }


        /// <summary>
        /// Ensures Vincenty precision withing bounds
        /// </summary>
        [TestMethod]
        public void Vincenty_Precision()
        {
            Coordinate c = new Coordinate(25, 45, new EagerLoad(false));

            //Check Vincenty
            var gd = new GeoFence.Drawer(c, Shape.Ellipsoid, 0);


            gd.Draw(new Distance(20), 0);
            gd.Draw(new Distance(20), 90);
            gd.Draw(new Distance(20), 90);
            gd.Draw(new Distance(20), 90);

            Assert.AreEqual(0, c.Latitude.ToDouble() - gd.Last.Latitude.ToDouble(), .000001);
            Assert.AreEqual(0, c.Longitude.ToDouble() - gd.Last.Longitude.ToDouble(), .000001);
        }

        /// <summary>
        /// Ensures shape closes when called
        /// </summary>
        [TestMethod]
        public void Shape_Closing()
        {
            Coordinate c = new Coordinate(25, 45, new EagerLoad(false));
            var  gd = new GeoFence.Drawer(c, Shape.Ellipsoid, 0);

            gd.Draw(new Distance(20), 0);
            gd.Draw(new Distance(20), 90);
            gd.Draw(new Distance(20), 90);
            gd.Close();

            Assert.AreEqual(0, c.Latitude.ToDouble() - gd.Last.Latitude.ToDouble());
            Assert.AreEqual(0, c.Longitude.ToDouble() - gd.Last.Longitude.ToDouble());
            Assert.AreEqual(5, gd.Points.Count);
           
        }
    }
}
