using System;
using CoordinateSharp;
using System.Diagnostics;

namespace CoordinateSharp_TestProj
{
    public class Distance_Tests
    {
        public static void Run_Test()
        {
            Distance_Init_Tests();
            Coordinate_Move_Test();          
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
            //TURN OFF EAGERLOAD TO ENSURE COORD STILL MOVES

            Coordinate c;
            Coordinate c2 = new Coordinate(25, 25);
            Console.WriteLine();

            try
            {
                bool pass = true;

                double lat = 0.993933103786722;
                double longi = 0.993337111127846;

                c = new Coordinate(1, 1, new EagerLoad(false));
                c.Move(c2, 1000, Shape.Ellipsoid);
                if (Math.Abs(lat - c.Latitude.DecimalDegree) > .000001 || Math.Abs(longi - c.Longitude.DecimalDegree) > .000001)
                {
                    pass = false;
                }

                lat = 1.00667823028963;
                longi = 0.993966812478288;

                c = new Coordinate(1, 1, new EagerLoad(false));
                c.Move(c2, 1000, Shape.Sphere);
                if (Math.Abs(lat - c.Latitude.DecimalDegree) > .000001 || Math.Abs(longi - c.Longitude.DecimalDegree) > .000001)
                {
                    pass = false;
                }

                c = new Coordinate(1, 1, new EagerLoad(false));
                c.Move(1000, 25, Shape.Sphere);
                lat = 1.00815611080085;
                longi = 0.996196153222444;

                if (Math.Abs(lat - c.Latitude.DecimalDegree) > .000001 || Math.Abs(longi - c.Longitude.DecimalDegree) > .000001)
                {
                    pass = false;
                }

                c = new Coordinate(1, 1, new EagerLoad(false));
                c.Move(1000, 25, Shape.Ellipsoid);
                lat = 1.00819634348146;
                longi = 0.996202971693992;

                if (Math.Abs(lat - c.Latitude.DecimalDegree) > .000001 || Math.Abs(longi - c.Longitude.DecimalDegree) > .000001)
                {
                    pass = false;
                }

                Pass.Write("Coordinate Move Tests", pass);
            }
            catch (Exception ex)
            {
                Pass.Write("Coordinate Move Tests", false);
                Console.WriteLine();
                Console.WriteLine(ex.ToString());
                Console.WriteLine();
            }
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
    }
}
