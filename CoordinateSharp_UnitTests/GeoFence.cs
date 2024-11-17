using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoordinateSharp;
using System.IO;
using System.Text.RegularExpressions;
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

            Assert.IsTrue(gf.IsPointInRangeOfLine(c1, 1000));
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
        /// Tests densify logic to ensure proper point placement.
        /// </summary>
        [TestMethod]
        public void Densify()
        {
            //Create a four point GeoFence around Utah

            List<GeoFence.Point> points = new List<GeoFence.Point>();

            points.Add(new GeoFence.Point(41.003444, -109.045223));
            points.Add(new GeoFence.Point(41.003444, -102.041524));
            points.Add(new GeoFence.Point(36.993076, -102.041524));
            points.Add(new GeoFence.Point(36.993076, -109.045223));
            points.Add(new GeoFence.Point(41.003444, -109.045223));

            GeoFence ellipseTest = new GeoFence(points);
            GeoFence sphereTest = new GeoFence(new List<GeoFence.Point>(points));
            GeoFence customTest = new GeoFence(new List<GeoFence.Point>(points));

            ellipseTest.Densify(new Distance(5, DistanceType.Kilometers));
            sphereTest.Densify(new Distance(5, DistanceType.Kilometers), Shape.Sphere);
            customTest.Densify(new Distance(5, DistanceType.Kilometers), Earth_Ellipsoid.Get_Ellipsoid(Earth_Ellipsoid_Spec.WGS84_1984));

            string[] ellipseTestPoints = File.ReadAllText("GeoFenceData\\ColoradoEllipse.txt").Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            string[] sphereTestPoints = File.ReadAllText("GeoFenceData\\ColoradoSphere.txt").Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            for (int x = 0; x < ellipseTestPoints.Length; x++)
            {
                string[] point = ellipseTestPoints[x].Split(',');
                double lat = double.Parse(point[0]);
                double lng = double.Parse(point[1]);
                Assert.AreEqual(ellipseTest.Points[x].Latitude, lat, .00000000001);
                Assert.AreEqual(ellipseTest.Points[x].Longitude, lng, .00000000001);
                Assert.AreEqual(customTest.Points[x].Latitude, lat, .00000000001);
                Assert.AreEqual(customTest.Points[x].Longitude, lng, .00000000001);
            }

            for (int x = 0; x < sphereTestPoints.Length; x++)
            {
                string[] point = sphereTestPoints[x].Split(',');
                double lat = double.Parse(point[0]);
                double lng = double.Parse(point[1]);
                Assert.AreEqual(sphereTest.Points[x].Latitude, lat, .00000000001);
                Assert.AreEqual(sphereTest.Points[x].Longitude, lng, .00000000001);
            }


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
            var gd = new GeoFence.Drawer(c, Shape.Ellipsoid, 0);

            gd.Draw(new Distance(20), 0);
            gd.Draw(new Distance(20), 90);
            gd.Draw(new Distance(20), 90);
            gd.Close();

            Assert.AreEqual(0, c.Latitude.ToDouble() - gd.Last.Latitude.ToDouble());
            Assert.AreEqual(0, c.Longitude.ToDouble() - gd.Last.Longitude.ToDouble());
            Assert.AreEqual(5, gd.Points.Count);

        }

        /// <summary>
        /// Ensure left handed, right handed ordering and GeoJson output in SE hemisphere
        /// </summary>
        [TestMethod]
        public void GeoJsonBuilderSE_Test()
        {
            List<Coordinate> outerPoints = new List<Coordinate>()
            {
                new Coordinate(-23.6980, 133.8807),  // Alice Springs (center)
                new Coordinate(-23.5980, 133.7807),  // North-west
                new Coordinate(-23.4980, 133.8807),  // North-east
                new Coordinate(-23.6980, 133.9807),  // South-east
                new Coordinate(-23.7980, 133.8807),  // South-west
                new Coordinate(-23.6980, 134.0807),  // Far east
                new Coordinate(-23.4980, 133.6807),  // Far north-west
                new Coordinate(-23.7980, 133.6807),  // Far south-west
                new Coordinate(-23.6980, 133.7807),  // North-west again
                new Coordinate(-23.7980, 134.0807),  // South-east
                new Coordinate(-23.4980, 134.0807),  // Far north-east
                new Coordinate(-23.4980, 134.1807),  // Very far north-east
                new Coordinate(-23.5980, 134.0807),  // North-east
                new Coordinate(-23.7980, 134.1807),  // South-east again
                new Coordinate(-23.6980, 134.2807)   // Far far east
            };

            List<Coordinate> innerPointsBraitling = new List<Coordinate>()
            {
                new Coordinate(-23.6856, 133.8662),  // Braitling center
                new Coordinate(-23.6880, 133.8700),  // North-east
                new Coordinate(-23.6830, 133.8630),  // West
                new Coordinate(-23.6870, 133.8600),  // South-west
                new Coordinate(-23.6820, 133.8650)   // East
            };


            List<Coordinate> innerPointsRoss = new List<Coordinate>()
            {
                new Coordinate(-23.7415, 133.8804),  // Ross center
                new Coordinate(-23.7430, 133.8850),  // North-east
                new Coordinate(-23.7400, 133.8780),  // West
                new Coordinate(-23.7450, 133.8760),  // South-west
                new Coordinate(-23.7420, 133.8820)   // East
            };


            GeoFence outerFence = new GeoFence(outerPoints);
            outerFence.OrderPoints_RightHanded();
            outerFence.ClosePolygon();
            string result = GeoFence.GeoJsonPolygonBuilder(outerFence);

            GeoFence braitlingFence = new GeoFence(innerPointsBraitling);
            braitlingFence.OrderPoints_LeftHanded();
            braitlingFence.ClosePolygon();

            GeoFence rossFence = new GeoFence(innerPointsRoss);
            rossFence.OrderPoints_LeftHanded();
            rossFence.ClosePolygon();

            // geojson.io (for plotting tests)
            result = GeoFence.GeoJsonPolygonBuilder(outerFence, new List<GeoFence>() { braitlingFence, rossFence });

            var json = File.ReadAllText(@"GeoFenceData\GeoJsonSE.json");
            string NormalizeWhitespace(string input) => Regex.Replace(input, @"\s+", "");

            Assert.AreEqual(NormalizeWhitespace(json), NormalizeWhitespace(result));
          

        }
        /// <summary>
        /// Ensure left handed, right handed ordering and GeoJson output in NW hemisphere
        /// </summary>
        [TestMethod]
        public void GeoJsonBuilderNW_Test()
        {
            List<Coordinate> outerPoints = new List<Coordinate>()
            {
                 new Coordinate(47.6062, -122.3321),  // Seattle
                 new Coordinate(48.7519, -122.4787),  // Bellingham
                 new Coordinate(47.2529, -122.4443),  // Tacoma
                 new Coordinate(48.0419, -122.9025),  // Port Townsend
                 new Coordinate(47.6588, -117.4260),  // Spokane
                 new Coordinate(46.6021, -120.5059),  // Yakima
                 new Coordinate(46.7324, -117.0002),  // Pullman
                 new Coordinate(48.3102, -122.6290),  // Anacortes
                 new Coordinate(47.8225, -122.3123),  // Edmonds
                 new Coordinate(46.9787, -123.8313),  // Aberdeen
                 new Coordinate(47.0379, -122.9007),  // Olympia
                 new Coordinate(47.6091, -122.2015),  // Bellevue
                 new Coordinate(47.6787, -120.7141),  // Leavenworth
                 new Coordinate(48.0812, -123.2643),  // Port Angeles
                 new Coordinate(46.7152, -122.9522)   // Centralia
            };


            List<Coordinate> innerPoints = new List<Coordinate>()
            {
                new Coordinate(46.8523, -121.7603),  // Mount Rainier (center point)
                new Coordinate(46.8625, -121.7401),  // Slightly north-east
                new Coordinate(46.8421, -121.7805),  // Slightly south-west
                new Coordinate(46.8650, -121.7850),  // North-west
                new Coordinate(46.8400, -121.7500)   // South-east
            };

            List<Coordinate> quincyPoints = new List<Coordinate>()
            {
                new Coordinate(47.2331, -119.8529),  // Quincy (center point)
                new Coordinate(47.2400, -119.8700),  // Slightly north-west
                new Coordinate(47.2280, -119.8350),  // Slightly south-east
                new Coordinate(47.2200, -119.8800),  // South-west
                new Coordinate(47.2450, -119.8400),  // North-east
                new Coordinate(47.2355, -119.8600),  // Near center, west
                new Coordinate(47.2220, -119.8450),  // South-east
                new Coordinate(47.2300, -119.8700),  // Slightly north-west
                new Coordinate(47.2150, -119.8650),  // South-west
                new Coordinate(47.2400, -119.8500)   // North-east
            };

            GeoFence outerFence = new GeoFence(outerPoints);
            outerFence.OrderPoints_RightHanded();
            outerFence.ClosePolygon();
            string result = GeoFence.GeoJsonPolygonBuilder(outerFence);

            GeoFence rainierFence = new GeoFence(innerPoints);
            rainierFence.OrderPoints_LeftHanded();
            rainierFence.ClosePolygon();

            GeoFence quincyFence = new GeoFence(quincyPoints);
            quincyFence.OrderPoints_LeftHanded();
            quincyFence.ClosePolygon();

            //geojson.io (for plotting tests)
            result = GeoFence.GeoJsonPolygonBuilder(outerFence, new List<GeoFence>() { rainierFence, quincyFence });

            var json = File.ReadAllText(@"GeoFenceData\GeoJsonNW.json");
            string NormalizeWhitespace(string input) => Regex.Replace(input, @"\s+", "");

            Assert.AreEqual(NormalizeWhitespace(json), NormalizeWhitespace(result));


        }


    }

}
