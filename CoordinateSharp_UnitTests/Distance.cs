using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoordinateSharp;
using System.Linq;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Globalization;

namespace CoordinateSharp_UnitTests
{
    [TestClass]
    public class DistanceTests
    {
        static readonly double m = 1000; //meters
        static readonly double km = 1; //Kilometers
        static readonly double ft = 3280.84; //Feet
        static readonly double sm = 0.6213712; //Nautical Miles
        static readonly double nm = 0.5399565; //Statute Miles

        double[] distances = new double[] { m, km, ft, nm, sm };

        static readonly double kmSphere = 412.0367538058125;
        static readonly double kmWGS84 = 412.1977393206501; //Default datum WGS84

        static readonly List<string> lines = System.IO.File.ReadAllLines(@"CoordinateData\MoveCoords.txt").ToList();

        /// <summary>
        /// Check Distance Initializer Method
        /// </summary>
        [TestMethod]
        public void Distance_Initializes_Double()
        {
            Distance d = new Distance(km);
            Check_Distance(d, distances);
        }

        /// <summary>
        /// Check Distance Initializer Method
        /// </summary>
        [TestMethod]
        public void Distance_Initializes_Meters()
        {
            Distance d = new Distance(m, DistanceType.Meters);
            Check_Distance(d, distances);
        }

        /// <summary>
        /// Check Distance Initializer Method
        /// </summary>
        [TestMethod]
        public void Distance_Initializes_Kilometers()
        {
            Distance d = new Distance(km, DistanceType.Kilometers);
            Check_Distance(d, distances);
        }

        /// <summary>
        /// Check Distance Initializer Method
        /// </summary>
        [TestMethod]
        public void Distance_Initializes_Feet()
        {
            Distance d = new Distance(ft, DistanceType.Feet);
            Check_Distance(d, distances);
        }

        /// <summary>
        /// Check Distance Initializer Method
        /// </summary>
        [TestMethod]
        public void Distance_Initializes_Nautical_Miles()
        {
            Distance d = new Distance(nm, DistanceType.NauticalMiles);
            Check_Distance(d, distances);
        }

        /// <summary>
        /// Check Distance Initializer Method
        /// </summary>
        [TestMethod]
        public void Distance_Initializes_Statute_Miles()
        {
            Distance d = new Distance(sm, DistanceType.Miles);
            Check_Distance(d, distances);
        }

        /// <summary>
        /// Check Distance Initializer Method
        /// </summary>
        [TestMethod]
        public void Distance_Initializes_Two_Coordinates()
        {
            Coordinate c1 = new Coordinate(45, 72);
            Coordinate c2 = new Coordinate(42, 75);

            Distance d = new Distance(c1,c2);
            Assert.AreEqual(0,d.Kilometers-kmSphere,.000001, "Coordinate distance exceeds delta");
          
        }

        /// <summary>
        /// Check Distance Initializer Method
        /// </summary>
        [TestMethod]
        public void Distance_Initializes_Two_Coordinates_Sphere()
        {
            Coordinate c1 = new Coordinate(45, 72);
            Coordinate c2 = new Coordinate(42, 75);

            Distance d = new Distance(c1, c2, Shape.Sphere);
            Assert.AreEqual(0, d.Kilometers - kmSphere, .000001, "Coordinate distance exceeds delta");
           
        }

        /// <summary>
        /// Check Distance Initializer Method
        /// </summary>
        [TestMethod]
        public void Distance_Initializes_Two_Coordinates_Ellispoid()
        {
            Coordinate c1 = new Coordinate(45, 72);
            Coordinate c2 = new Coordinate(42, 75);

            Distance d = new Distance(c1, c2, Shape.Ellipsoid);
            Assert.AreEqual(0, d.Kilometers - kmWGS84, .000001, "Coordinate distance exceeds delta");
           
        }

        /// <summary>
        /// Check Movement Logic
        /// </summary>
        [TestMethod]
        public void Move_To_Target_Coordinate_Specified_Distance()
        {
            int line = 1;

            foreach (string s in lines)
            {
                Movement_Data md = new Movement_Data(s);

                md.Coord.Move(md.Target, new Distance(md.Dist), md.Shape);
                md.Coord.FormatOptions.Format = CoordinateFormatType.Decimal;

                Assert.AreEqual(0, md.Coord.Latitude.ToDouble() - md.Target.Latitude.ToDouble(), .0001, $"Latitude delta exceeded on iteration/line {line}");
                Assert.AreEqual(0, md.Coord.Longitude.ToDouble() - md.Target.Longitude.ToDouble(), .0001, $"Longitude delta exceeded on iteration/line {line}");

                line++;
            }


        }
        /// <summary>
        /// Check Movement Logic
        /// </summary>
        [TestMethod]
        public void Move_To_Target_Coordinate_Specified_Distance_Double()
        {         
            int line = 1;

            foreach (string s in lines)
            {

                Movement_Data md = new Movement_Data(s);

                md.Coord.Move(md.Target, md.Dist * 1000, md.Shape);

                Assert.AreEqual(0, md.Coord.Latitude.ToDouble() - md.Target.Latitude.ToDouble(), .0001, $"Latitude delta exceeded on iteration/line {line}");
                Assert.AreEqual(0, md.Coord.Longitude.ToDouble() - md.Target.Longitude.ToDouble(), .0001, $"Longitude delta exceeded on iteration/line {line}");

                line++;

            }

        }

        /// <summary>
        /// Check Movement Logic
        /// </summary>
        [TestMethod]
        public void Move_To_Target_Towards_Bearing()
        {        
            int line = 1;

            foreach (string s in lines)
            {
                Movement_Data md = new Movement_Data(s);
              
                md.Coord.Move(new Distance(md.Dist), md.Bearing, md.Shape);

                Assert.AreEqual(0, md.Coord.Latitude.ToDouble() - md.Target.Latitude.ToDouble(), .0001, $"Latitude delta exceeded on iteration/line {line}");
                Assert.AreEqual(0, md.Coord.Longitude.ToDouble() - md.Target.Longitude.ToDouble(), .0001, $"Longitude delta exceeded on iteration/line {line}");

                line++;
            }

        }

        /// <summary>
        /// Check Movement Logic
        /// </summary>
        [TestMethod]
        public void Move_To_Target_Towards_Bearing_Double()
        {           
            int line = 1;

            foreach (string s in lines)
            {
                Movement_Data md = new Movement_Data(s);

              
                md.Coord.Move(md.Dist * 1000, md.Bearing, md.Shape);

                Assert.AreEqual(0, md.Coord.Latitude.ToDouble() - md.Target.Latitude.ToDouble(), .0001, $"Latitude delta exceeded on iteration/line {line}");
                Assert.AreEqual(0, md.Coord.Longitude.ToDouble() - md.Target.Longitude.ToDouble(), .0001, $"Longitude delta exceeded on iteration/line {line}");

                line++;
            }

        }

        /// <summary>
        /// Ensures Ellipsoidal calculates do not exceed delta
        /// </summary>
        [TestMethod]
        public void Ellipsoidal_Delta_Checks()
        {
            Coordinate c1;
            Coordinate c2;
            double distanceBuf = .0000001; //Fault tolerance for distance variations
            double bearingBuf = .0000001; //Fault tolerance for bearing variations
            Distance d;
            double[] check;
        

            //COMAPRISON VALUES PULLED FROM ED WILLIAMS GREAT CIRCLE CALCULATOR 
            //http://edwilliams.org/gccalc.htm

            int checkNum = 1;
            /* ELLIPSOID CHECKS */
            //Check 1
            c1 = new Coordinate(45.02258, 7.63489);
            c2 = new Coordinate(45.02092, 7.6332);
            d = new Distance(c1, c2, Shape.Ellipsoid);
            check = new double[] { 215.83122136519, 0.22754143255301168 };
            Assert.AreEqual(0, d.Bearing - check[0], bearingBuf, $"Bearing delta exceed at check {checkNum}.");
            Assert.AreEqual(0, d.Kilometers - check[1], distanceBuf, $"distance delta exceed at check {checkNum}.");
            checkNum++;
            //Check 2
            c1 = new Coordinate(45.02258, -7.63489);
            c2 = new Coordinate(45.02092, -7.6332);
            d = new Distance(c1, c2, Shape.Ellipsoid);
            check = new double[] { 144.16877863481, 0.22754143255301168 };
            Assert.AreEqual(0, d.Bearing - check[0], bearingBuf, $"Bearing delta exceed at check {checkNum}.");
            Assert.AreEqual(0, d.Kilometers - check[1], distanceBuf, $"distance delta exceed at check {checkNum}.");
            checkNum++;
            //Check 3
            c1 = new Coordinate(-45.02258, -7.63489);
            c2 = new Coordinate(-45.02092, -7.6332);
            d = new Distance(c1, c2, Shape.Ellipsoid);
            check = new double[] { 35.83122136518998, 0.22754143255301168 };
            Assert.AreEqual(0, d.Bearing - check[0], bearingBuf, $"Bearing delta exceed at check {checkNum}.");
            Assert.AreEqual(0, d.Kilometers - check[1], distanceBuf, $"distance delta exceed at check {checkNum}.");
            checkNum++;
            //Check 4
            c1 = new Coordinate(-45.02258, 7.63489);
            c2 = new Coordinate(-45.02092, 7.6332);
            d = new Distance(c1, c2, Shape.Ellipsoid);
            check = new double[] { 324.16877863481, 0.22754143255301168 };
            Assert.AreEqual(0, d.Bearing - check[0], bearingBuf, $"Bearing delta exceed at check {checkNum}.");
            Assert.AreEqual(0, d.Kilometers - check[1], distanceBuf, $"distance delta exceed at check {checkNum}.");
            checkNum++;
            //Check 5
            c1 = new Coordinate(-45.02258, -7.63489);
            c2 = new Coordinate(45.02092, 7.6332);
            d = new Distance(c1, c2, Shape.Ellipsoid);
            check = new double[] { 10.7750299, 10087.874457727042 };
            Assert.AreEqual(0, d.Bearing - check[0], bearingBuf, $"Bearing delta exceed at check {checkNum}.");
            Assert.AreEqual(0, d.Kilometers - check[1], distanceBuf, $"distance delta exceed at check {checkNum}.");
            checkNum++;
            //Check 6
            c1 = new Coordinate(-76.02258, -120.63489);
            c2 = new Coordinate(12.2569, 7.6332);
            d = new Distance(c1, c2, Shape.Ellipsoid);
            check = new double[] { 125.0102282873087, 12292.331977781124 };
            Assert.AreEqual(0, d.Bearing - check[0], bearingBuf, $"Bearing delta exceed at check {checkNum}.");
            Assert.AreEqual(0, d.Kilometers - check[1], distanceBuf, $"distance delta exceed at check {checkNum}.");
            checkNum++;
            //Check 7
            c1 = new Coordinate(7.689, 91.6998);
            c2 = new Coordinate(8.656, 90.658);
            d = new Distance(c1, c2, Shape.Ellipsoid);
            check = new double[] { 313.0440365804527, 156.8980064612199 };
            Assert.AreEqual(0, d.Bearing - check[0], bearingBuf, $"Bearing delta exceed at check {checkNum}.");
            Assert.AreEqual(0, d.Kilometers - check[1], distanceBuf, $"distance delta exceed at check {checkNum}.");
            checkNum++;
            //Check 8
            c1 = new Coordinate(-7.689, 91.6998);
            c2 = new Coordinate(-8.656, 90.658);
            d = new Distance(c1, c2, Shape.Ellipsoid);
            check = new double[] { 226.9559634195473, 156.8980064612199 };
            Assert.AreEqual(0, d.Bearing - check[0], bearingBuf, $"Bearing delta exceed at check {checkNum}.");
            Assert.AreEqual(0, d.Kilometers - check[1], distanceBuf, $"distance delta exceed at check {checkNum}.");
            checkNum++;
            //Check 9
            c1 = new Coordinate(-25.6965, -91.6998);
            c2 = new Coordinate(-22.3656, -90.658);
            d = new Distance(c1, c2, Shape.Ellipsoid);
            check = new double[] { 16.242229103528945, 383.8404829840529 };
            Assert.AreEqual(0, d.Bearing - check[0], bearingBuf, $"Bearing delta exceed at check {checkNum}.");
            Assert.AreEqual(0, d.Kilometers - check[1], distanceBuf, $"distance delta exceed at check {checkNum}.");
            checkNum++;
            //Check 10
            c1 = new Coordinate(25.6965, -91.6998);
            c2 = new Coordinate(22.3656, -90.658);
            d = new Distance(c1, c2, Shape.Ellipsoid);
            check = new double[] { 163.75777089647104, 383.8404829840529 };
            Assert.AreEqual(0, d.Bearing - check[0], bearingBuf, $"Bearing delta exceed at check {checkNum}.");
            Assert.AreEqual(0, d.Kilometers - check[1], distanceBuf, $"distance delta exceed at check {checkNum}.");
        }

        /// <summary>
        /// Ensures Spherical calculates do not exceed delta
        /// </summary>
        [TestMethod]
        public void Spherical_Delta_Checks()
        {
            Coordinate c1;
            Coordinate c2;
            double distanceBuf = .0000001; //Fault tolerance for distance variations
            double bearingBuf = .0000001; //Fault tolerance for bearing variations
            Distance d;
            double[] check;
          
            int checkNum = 1;
            //DISTANCE VALUES COMPARED https://www.movable-type.co.uk/scripts/latlong.html
            distanceBuf = .0001; //Fault tolerance for distance variations
            bearingBuf = .0001; //Fault tolerance for bearing variations
          
            /* SPHERE CHECKS */
            //Check 1
            c1 = new Coordinate(45.02258, 7.63489);
            c2 = new Coordinate(45.02092, 7.6332);
            d = new Distance(c1, c2, Shape.Sphere);
            check = new double[] { 215.73985977231427, 0.2274 };
            Assert.AreEqual(0, d.Bearing - check[0], bearingBuf, $"Bearing delta exceed at check {checkNum}.");
            Assert.AreEqual(0, d.Kilometers - check[1], distanceBuf, $"distance delta exceed at check {checkNum}.");
            checkNum++;
            //Check 2
            c1 = new Coordinate(45.02258, -7.63489);
            c2 = new Coordinate(45.02092, -7.6332);
            d = new Distance(c1, c2, Shape.Sphere);
            check = new double[] { 144.26014022768572, 0.2274 };
            Assert.AreEqual(0, d.Bearing - check[0], bearingBuf, $"Bearing delta exceed at check {checkNum}.");
            Assert.AreEqual(0, d.Kilometers - check[1], distanceBuf, $"distance delta exceed at check {checkNum}.");
            checkNum++;
            //Check 3
            c1 = new Coordinate(-45.02258, -7.63489);
            c2 = new Coordinate(-45.02092, -7.6332);
            d = new Distance(c1, c2, Shape.Sphere);
            check = new double[] { 35.73985977231427, 0.2274 };
            Assert.AreEqual(0, d.Bearing - check[0], bearingBuf, $"Bearing delta exceed at check {checkNum}.");
            Assert.AreEqual(0, d.Kilometers - check[1], distanceBuf, $"distance delta exceed at check {checkNum}.");
            checkNum++;
            //Check 4
            c1 = new Coordinate(-45.02258, 7.63489);
            c2 = new Coordinate(-45.02092, 7.6332);
            d = new Distance(c1, c2, Shape.Sphere);
            check = new double[] { 324.2601402276857, 0.2274 };
            Assert.AreEqual(0, d.Bearing - check[0], bearingBuf, $"Bearing delta exceed at check {checkNum}.");
            Assert.AreEqual(0, d.Kilometers - check[1], distanceBuf, $"distance delta exceed at check {checkNum}.");
            checkNum++;
            //Check 5
            c1 = new Coordinate(-45.02258, -7.63489);
            c2 = new Coordinate(45.02092, 7.6332);
            d = new Distance(c1, c2, Shape.Sphere);
            check = new double[] { 10.72935562256683, 10124.7363 };
            Assert.AreEqual(0, d.Bearing - check[0], bearingBuf, $"Bearing delta exceed at check {checkNum}.");
            Assert.AreEqual(0, d.Kilometers - check[1], distanceBuf, $"distance delta exceed at check {checkNum}.");
            checkNum++;
            //Check 6
            c1 = new Coordinate(-76.02258, -120.63489);
            c2 = new Coordinate(12.2569, 7.6332);
            d = new Distance(c1, c2, Shape.Sphere);
            check = new double[] { 124.94031333870444, 12300.5645 };
            Assert.AreEqual(0, d.Bearing - check[0], bearingBuf, $"Bearing delta exceed at check {checkNum}.");
            Assert.AreEqual(0, d.Kilometers - check[1], distanceBuf, $"distance delta exceed at check {checkNum}.");
            checkNum++;
            //Check 7
            c1 = new Coordinate(7.689, 91.6998);
            c2 = new Coordinate(8.656, 90.658);
            d = new Distance(c1, c2, Shape.Sphere);
            check = new double[] { 313.2321507309865, 157.1934 };
            Assert.AreEqual(0, d.Bearing - check[0], bearingBuf, $"Bearing delta exceed at check {checkNum}.");
            Assert.AreEqual(0, d.Kilometers - check[1], distanceBuf, $"distance delta exceed at check {checkNum}.");
            checkNum++;
            //Check 8
            c1 = new Coordinate(-7.689, 91.6998);
            c2 = new Coordinate(-8.656, 90.658);
            d = new Distance(c1, c2, Shape.Sphere);
            check = new double[] { 226.76784926901342, 157.1934 };
            Assert.AreEqual(0, d.Bearing - check[0], bearingBuf, $"Bearing delta exceed at check {checkNum}.");
            Assert.AreEqual(0, d.Kilometers - check[1], distanceBuf, $"distance delta exceed at check {checkNum}.");
            checkNum++;
            //Check 9
            c1 = new Coordinate(-25.6965, -91.6998);
            c2 = new Coordinate(-22.3656, -90.658);
            d = new Distance(c1, c2, Shape.Sphere);
            check = new double[] { 16.157241979509656, 385.1879 };
            Assert.AreEqual(0, d.Bearing - check[0], bearingBuf, $"Bearing delta exceed at check {checkNum}.");
            Assert.AreEqual(0, d.Kilometers - check[1], distanceBuf, $"distance delta exceed at check {checkNum}.");
            checkNum++;
            //Check 10
            c1 = new Coordinate(25.6965, -91.6998);
            c2 = new Coordinate(22.3656, -90.658);
            d = new Distance(c1, c2, Shape.Sphere);
            check = new double[] { 163.84275802049035, 385.1879 };
            Assert.AreEqual(0, d.Bearing - check[0], bearingBuf, $"Bearing delta exceed at check {checkNum}.");
            Assert.AreEqual(0, d.Kilometers - check[1], distanceBuf, $"distance delta exceed at check {checkNum}.");
            checkNum++;
        }

        private void Check_Distance(Distance d, double[] distances)
        {
           
            //Round to avoid float point issues
            double _m = d.Meters;
            double _km = d.Kilometers;
            double _ft = d.Feet;
            double _nm = d.NauticalMiles;
            double _sm = d.Miles;
            

            Assert.AreEqual(0, _m - distances[0], .0001, $"Meters exceed delta");
            Assert.AreEqual(0, _km - distances[1], .0001, $"Kilometers exceed delta");
            Assert.AreEqual(0, _ft - distances[2], .0001, $"Feet exceed delta");
            Assert.AreEqual(0, _nm - distances[3], .0001, $"Nautical miles exceed delta");
            Assert.AreEqual(0, _sm - distances[4], .001, $"Statue miles exceed delta");
        }
    }

    public class Movement_Data
    {
        public Movement_Data(string line)
        {
          
            string[] data = line.Split(',');
            if (data[5] == "S") { Shape = Shape.Sphere; }
            else { Shape = Shape.Ellipsoid; }

            Lat1 = Convert.ToDouble(data[0], CultureInfo.InvariantCulture);
            Long1 = Convert.ToDouble(data[1], CultureInfo.InvariantCulture);
            Lat2 = Convert.ToDouble(data[2], CultureInfo.InvariantCulture);
            Long2 = Convert.ToDouble(data[3], CultureInfo.InvariantCulture);

            Dist = Convert.ToDouble(data[4], CultureInfo.InvariantCulture);
            Bearing = Convert.ToDouble(data[6], CultureInfo.InvariantCulture);

        
            Coord = new Coordinate(Lat1, Long1, new EagerLoad(false));
            Target = new Coordinate(Lat2, Long2, new EagerLoad(false));
          
        }
        public Shape Shape { get; set; }
        public double Lat1 { get; set; }
        public double Long1 { get; set; }
        public double Lat2 { get; set; }
        public double Long2 { get; set; }
        public double Dist { get; set; }
        public double Bearing { get; set; }
        public Coordinate Coord { get; set; }
        public Coordinate Target { get; set; }
     
    }
   
}
