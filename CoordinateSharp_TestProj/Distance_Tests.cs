using System;
using CoordinateSharp;
using System.Diagnostics;
using System.Collections.Generic;
namespace CoordinateSharp_TestProj
{
    public class Distance_Tests
    {
        public static void Run_Test()
        {
            Distance_Init_Tests();
            Coordinate_Move_Test();
            Distance_Value_Tests();
        
        }
        private static void Distance_Init_Tests()
        {
            //Conversions should be equal to these numbers within .0001 tolerance

            double m = 1000; //meters
            double km = 1; //Kilometers
            double ft = 3280.84; //Feet
            double sm = 0.6213712; //Nautical Miles
            double nm = 0.5399565; //Statute Miles

            double[] distances = new double[] { m, km, ft, nm, sm };


            Distance d = new Distance(km);

            Pass.Write("Distance(double km)", Check_Distance(d, distances));
            d = new Distance(distances[0], DistanceType.Meters);
            Console.WriteLine();
            Pass.Write("Distance(double distance, DistanceType Meters)", Check_Distance(d, distances));
            d = new Distance(distances[1], DistanceType.Kilometers);
            Pass.Write("Distance(double distance, DistanceType Kilometers)", Check_Distance(d, distances));
            d = new Distance(distances[2], DistanceType.Feet);
            Pass.Write("Distance(double distance, DistanceType Feet)", Check_Distance(d, distances));
            d = new Distance(distances[3], DistanceType.NauticalMiles);
            Pass.Write("Distance(double distance, DistanceType Nautical Miles)", Check_Distance(d, distances));
            d = new Distance(distances[4], DistanceType.Miles);
            Pass.Write("Distance(double distance, DistanceType Statute Miles)", Check_Distance(d, distances));
            Console.WriteLine();

            //KILOMETERS Between specified points above should be as follows in defined tolerance .000001
            double kmSphere = 412.0367538058125;
            double kmWGS84 = 412.1977393206501; //Default datum WGS84

            Coordinate c1 = new Coordinate(45, 72);
            Coordinate c2 = new Coordinate(42, 75);

            d = new Distance(c1, c2);

            if (Math.Abs(d.Kilometers - kmSphere) > .000001)
            {
                Pass.Write("Distance(Coordinate c1, Coordinate c2)", false);
                Debug.WriteLine("...Mismatch: " + d.Kilometers + " - " + kmSphere);
            }
            else { Pass.Write("Distance(Coordinate c1, Coordinate c2)", true); }
            d = new Distance(c1, c2, Shape.Sphere);
            if (Math.Abs(d.Kilometers - kmSphere) > .000001)
            {
                Pass.Write("Distance(Coordinate c1, Coordinate c2, Shape.Sphere)", false);
                Debug.WriteLine("...Mismatch: " + d.Kilometers + " - " + kmSphere);
            }
            else { Pass.Write("Distance(Coordinate c1, Coordinate c2, Shape.Sphere)", true); }
            d = new Distance(c1, c2, Shape.Ellipsoid);
            if (Math.Abs(d.Kilometers - kmWGS84) > .000001)
            {
                Pass.Write("Distance(Coordinate c1, Coordinate c2, Shape.Ellipsoid)", false);
                Debug.WriteLine("...Mismatch: " + d.Kilometers + " - " + kmWGS84);
            }
            else { Pass.Write("Distance(Coordinate c1, Coordinate c2, Shape.Ellipsoid)", true); }
        }
        private static void Coordinate_Move_Test()
        {
            bool pass = true;
            Console.WriteLine();
            string[] lines = System.IO.File.ReadAllLines(@"CoordinateData\MoveCoords.txt");
            int line = 1;
            //TEST MOVE TOWARDS TARGET LOGIC
            foreach(string s in lines)
            {
                Shape shape;
                string[] data = s.Split(',');
                if (data[5] == "S") { shape = Shape.Sphere; }
                else{ shape = Shape.Ellipsoid; }
                double lat1 = double.Parse(data[0]);
                double long1 = double.Parse(data[1]);
                double lat2 = double.Parse(data[2]);
                double long2 = double.Parse(data[3]);
                double dist = double.Parse(data[4]);
                double bearing = double.Parse(data[6]);
                Coordinate coord = new Coordinate(lat1, long1);
                Coordinate target = new Coordinate(lat2, long2);
                coord.FormatOptions.Format = CoordinateFormatType.Decimal;
                
                //MOVE TO TARGET
                //Method 1
                coord.Move(target, new Distance(dist), shape);
                coord.FormatOptions.Format = CoordinateFormatType.Decimal;
                //Console.WriteLine(coord);
                if (Math.Abs(coord.Latitude.ToDouble() - target.Latitude.ToDouble()) > .0001) { pass = false; Debug.WriteLine("Coordinate Move Test Failed On Iteration " + line); break;  }
                if (Math.Abs(coord.Longitude.ToDouble() - target.Longitude.ToDouble()) > .0001) { pass = false; Debug.WriteLine("Coordinate Move Test Failed On Iteration " + line); break; }

                //Method 2
                coord = new Coordinate(lat1, long1);
                coord.Move(target, dist*1000, shape);
                if (Math.Abs(coord.Latitude.ToDouble() - target.Latitude.ToDouble()) > .0001) { pass = false; Debug.WriteLine("Coordinate Move Test Failed On Iteration " + line); break; }
                if (Math.Abs(coord.Longitude.ToDouble() - target.Longitude.ToDouble()) > .0001) { pass = false; Debug.WriteLine("Coordinate Move Test Failed On Iteration " + line); break; }

                //MOVE TOWARD BEARING
                coord = new Coordinate(lat1, long1);
                coord.Move(new Distance(dist), bearing, shape);
                if (Math.Abs(coord.Latitude.ToDouble() - target.Latitude.ToDouble()) > .0001) { pass = false; Debug.WriteLine("Coordinate Move Test Failed On Iteration " + line); break; }
                if (Math.Abs(coord.Longitude.ToDouble() - target.Longitude.ToDouble()) > .0001) { pass = false; Debug.WriteLine("Coordinate Move Test Failed On Iteration " + line); break; }

                coord = new Coordinate(lat1, long1);
                coord.Move(dist*1000, bearing, shape);
                if (Math.Abs(coord.Latitude.ToDouble() - target.Latitude.ToDouble()) > .0001) { pass = false; Debug.WriteLine("Coordinate Move Test Failed On Iteration " + line); break; }
                if (Math.Abs(coord.Longitude.ToDouble() - target.Longitude.ToDouble()) > .0001) { pass = false; Debug.WriteLine("Coordinate Move Test Failed On Iteration " + line); break; }

                line++;
            }          
          
          

            Pass.Write("Coordinate Move Tests ", pass);


        }
        private static bool Check_Distance(Distance d, double[] distances)
        {
            bool pass = true;
            //Round to avoid float point issues
            double m = d.Meters;
            double km = d.Kilometers;
            double ft = d.Feet;
            double sm = d.Miles;
            double nm = d.NauticalMiles;
            if (Math.Abs(m - distances[0]) > .0001) { Debug.WriteLine("...METERS MISMATCH: " + d.Meters + " - " + distances[0]); return false; }
            if (Math.Abs(km - distances[1]) > .0001) { Debug.WriteLine("...KILOMETERS MISMATCH: " + d.Kilometers + " - " + distances[1]); return false; }
            if (Math.Abs(ft - distances[2]) > .0001) { Debug.WriteLine("...FEET MISMATCH: " + d.Feet + " - " + distances[2]); return false; }
            if (Math.Abs(nm - distances[3]) > .0001) { Debug.WriteLine("...NAUTICAL MILES MISMATCH: " + d.NauticalMiles + " - " + distances[3]); return false; }
            if (Math.Abs(sm - distances[4]) > .001) { Debug.WriteLine("...STATUTE MILE MISMATCH: " + d.Miles + " - " + distances[4]); return false; }
            return pass;

        }
        private static void Distance_Value_Tests()
        {
            Coordinate c1;
            Coordinate c2;
            double distanceBuf = .0000001; //Fault tolerance for distance variations
            double bearingBuf = .0000001; //Fault tolerance for bearing variations
            Distance d;
            double[] check;
            bool pass = true;

            //COMAPRISON VALUES PULLED FROM ED WILLIAMS GREAT CIRCLE CALCULATOR 
            //http://edwilliams.org/gccalc.htm

            /* ELLIPSOID CHECKS */
            //Check 1
            c1 = new Coordinate(45.02258, 7.63489);
            c2 = new Coordinate(45.02092, 7.6332);
            d = new Distance(c1, c2, Shape.Ellipsoid);
            check = new double[]{215.83122136519, 0.22754143255301168};
            if(Math.Abs(d.Bearing - check[0]) > bearingBuf) { pass = false; }
            if (Math.Abs(d.Kilometers - check[1]) > distanceBuf) { pass = false; }
            //Check 2
            c1 = new Coordinate(45.02258, -7.63489);
            c2 = new Coordinate(45.02092, -7.6332);
            d = new Distance(c1, c2, Shape.Ellipsoid);
            check = new double[] { 144.16877863481, 0.22754143255301168 };
            if (Math.Abs(d.Bearing - check[0]) > bearingBuf) { pass = false; }
            if (Math.Abs(d.Kilometers - check[1]) > distanceBuf) { pass = false; }
            //Check 3
            c1 = new Coordinate(-45.02258, -7.63489);
            c2 = new Coordinate(-45.02092, -7.6332);
            d = new Distance(c1, c2, Shape.Ellipsoid);
            check = new double[] { 35.83122136518998, 0.22754143255301168 };
            if (Math.Abs(d.Bearing - check[0]) > bearingBuf) { pass = false; }
            if (Math.Abs(d.Kilometers - check[1]) > distanceBuf) { pass = false; }
            //Check 4
            c1 = new Coordinate(-45.02258, 7.63489);
            c2 = new Coordinate(-45.02092, 7.6332);
            d = new Distance(c1, c2, Shape.Ellipsoid);
            check = new double[] {324.16877863481, 0.22754143255301168 };
            if (Math.Abs(d.Bearing - check[0]) > bearingBuf) { pass = false; }
            if (Math.Abs(d.Kilometers - check[1]) > distanceBuf) { pass = false; }
            //Check 5
            c1 = new Coordinate(-45.02258, -7.63489);
            c2 = new Coordinate(45.02092, 7.6332);
            d = new Distance(c1, c2, Shape.Ellipsoid);
            check = new double[] { 10.7750299, 10087.874457727042 };
            if (Math.Abs(d.Bearing - check[0]) > bearingBuf) { pass = false; }
            if (Math.Abs(d.Kilometers - check[1]) > distanceBuf) { pass = false; }
            //Check 6
            c1 = new Coordinate(-76.02258, -120.63489);
            c2 = new Coordinate(12.2569, 7.6332);
            d = new Distance(c1, c2, Shape.Ellipsoid);
            check = new double[] { 125.0102282873087, 12292.331977781124 };
            if (Math.Abs(d.Bearing - check[0]) > bearingBuf) { pass = false; }
            if (Math.Abs(d.Kilometers - check[1]) > distanceBuf) { pass = false; }
            //Check 7
            c1 = new Coordinate(7.689, 91.6998);
            c2 = new Coordinate(8.656, 90.658);
            d = new Distance(c1, c2, Shape.Ellipsoid);
            check = new double[] { 313.0440365804527, 156.8980064612199 };
            if (Math.Abs(d.Bearing - check[0]) > bearingBuf) { pass = false; }
            if (Math.Abs(d.Kilometers - check[1]) > distanceBuf) { pass = false; }
            //Check 8
            c1 = new Coordinate(-7.689, 91.6998);
            c2 = new Coordinate(-8.656, 90.658);
            d = new Distance(c1, c2, Shape.Ellipsoid);
            check = new double[] { 226.9559634195473, 156.8980064612199 };
            if (Math.Abs(d.Bearing - check[0]) > bearingBuf) { pass = false; }
            if (Math.Abs(d.Kilometers - check[1]) > distanceBuf) { pass = false; }
            //Check 9
            c1 = new Coordinate(-25.6965, -91.6998);
            c2 = new Coordinate(-22.3656, -90.658);
            d = new Distance(c1, c2, Shape.Ellipsoid);
            check = new double[] { 16.242229103528945, 383.8404829840529 };
            if (Math.Abs(d.Bearing - check[0]) > bearingBuf) { pass = false; }
            if (Math.Abs(d.Kilometers - check[1]) > distanceBuf) { pass = false; }
            //Check 10
            c1 = new Coordinate(25.6965, -91.6998);
            c2 = new Coordinate(22.3656, -90.658);
            d = new Distance(c1, c2, Shape.Ellipsoid);
            check = new double[] { 163.75777089647104, 383.8404829840529 };
            if (Math.Abs(d.Bearing - check[0]) > bearingBuf) { pass = false; }
            if (Math.Abs(d.Kilometers - check[1]) > distanceBuf) { pass = false; }

            Pass.Write("Coordinate Distance / Bearing Value (ELLIPSE) Tests", pass);

            //DISTANCE VALUES COMPARED https://www.movable-type.co.uk/scripts/latlong.html
            distanceBuf = .0001; //Fault tolerance for distance variations
            bearingBuf = .0001; //Fault tolerance for bearing variations
            pass = true;
            /* SPHERE CHECKS */
            //Check 1
            c1 = new Coordinate(45.02258, 7.63489);
            c2 = new Coordinate(45.02092, 7.6332);
            d = new Distance(c1, c2, Shape.Sphere);
            check = new double[] { 215.73985977231427, 0.2274 };
            if (Math.Abs(d.Bearing - check[0]) > bearingBuf) { pass = false; }
            if (Math.Abs(d.Kilometers - check[1]) > distanceBuf) { pass = false; }
            //Check 2
            c1 = new Coordinate(45.02258, -7.63489);
            c2 = new Coordinate(45.02092, -7.6332);
            d = new Distance(c1, c2, Shape.Sphere);
            check = new double[] { 144.26014022768572, 0.2274 };
            if (Math.Abs(d.Bearing - check[0]) > bearingBuf) { pass = false; }
            if (Math.Abs(d.Kilometers - check[1]) > distanceBuf) { pass = false; }
            //Check 3
            c1 = new Coordinate(-45.02258, -7.63489);
            c2 = new Coordinate(-45.02092, -7.6332);
            d = new Distance(c1, c2, Shape.Sphere);
            check = new double[] { 35.73985977231427, 0.2274 };
            if (Math.Abs(d.Bearing - check[0]) > bearingBuf) { pass = false; }
            if (Math.Abs(d.Kilometers - check[1]) > distanceBuf) { pass = false; }
            //Check 4
            c1 = new Coordinate(-45.02258, 7.63489);
            c2 = new Coordinate(-45.02092, 7.6332);
            d = new Distance(c1, c2, Shape.Sphere);
            check = new double[] { 324.2601402276857, 0.2274 };
            if (Math.Abs(d.Bearing - check[0]) > bearingBuf) { pass = false; }
            if (Math.Abs(d.Kilometers - check[1]) > distanceBuf) { pass = false; }
            //Check 5
            c1 = new Coordinate(-45.02258, -7.63489);
            c2 = new Coordinate(45.02092, 7.6332);
            d = new Distance(c1, c2, Shape.Sphere);
            check = new double[] { 10.72935562256683, 10124.7363 };
            if (Math.Abs(d.Bearing - check[0]) > bearingBuf) { pass = false; }
            if (Math.Abs(d.Kilometers - check[1]) > distanceBuf) { pass = false; }
            //Check 6
            c1 = new Coordinate(-76.02258, -120.63489);
            c2 = new Coordinate(12.2569, 7.6332);
            d = new Distance(c1, c2, Shape.Sphere);
            check = new double[] { 124.94031333870444, 12300.5645 };
            if (Math.Abs(d.Bearing - check[0]) > bearingBuf) { pass = false; }
            if (Math.Abs(d.Kilometers - check[1]) > distanceBuf) { pass = false; }
            //Check 7
            c1 = new Coordinate(7.689, 91.6998);
            c2 = new Coordinate(8.656, 90.658);
            d = new Distance(c1, c2, Shape.Sphere);
            check = new double[] { 313.2321507309865, 157.1934 };
            if (Math.Abs(d.Bearing - check[0]) > bearingBuf) { pass = false; }
            if (Math.Abs(d.Kilometers - check[1]) > distanceBuf) { pass = false; }
            //Check 8
            c1 = new Coordinate(-7.689, 91.6998);
            c2 = new Coordinate(-8.656, 90.658);
            d = new Distance(c1, c2, Shape.Sphere);
            check = new double[] { 226.76784926901342, 157.1934 };
            if (Math.Abs(d.Bearing - check[0]) > bearingBuf) { pass = false; }
            if (Math.Abs(d.Kilometers - check[1]) > distanceBuf) { pass = false; }
            //Check 9
            c1 = new Coordinate(-25.6965, -91.6998);
            c2 = new Coordinate(-22.3656, -90.658);
            d = new Distance(c1, c2, Shape.Sphere);
            check = new double[] { 16.157241979509656, 385.1879 };
            if (Math.Abs(d.Bearing - check[0]) > bearingBuf) { pass = false; }
            if (Math.Abs(d.Kilometers - check[1]) > distanceBuf) { pass = false; }
            //Check 10
            c1 = new Coordinate(25.6965, -91.6998);
            c2 = new Coordinate(22.3656, -90.658);
            d = new Distance(c1, c2, Shape.Sphere);
            check = new double[] { 163.84275802049035, 385.1879 };
            if (Math.Abs(d.Bearing - check[0]) > bearingBuf) { pass = false; }
            if (Math.Abs(d.Kilometers - check[1]) > distanceBuf) { pass = false; }

            Pass.Write("Coordinate Distance / Bearing Value (SPHERE) Tests", pass);
            //Console.WriteLine(d.Kilometers);

            //Console.WriteLine(d.Bearing);

            //c1.Move(new Distance(.2274), 144, Shape.Ellipsoid);
            //c1.FormatOptions.Format = CoordinateFormatType.Degree_Decimal_Minutes;
            //c1.FormatOptions.Round = 4;
        }
    }
}
