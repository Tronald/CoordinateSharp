using System;
using System.Diagnostics;
using CoordinateSharp;
namespace Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
           
            Console.WriteLine("Starting .NET CORE 3.1 Benchmarks, this test may take a while to finish...");
            Console.WriteLine();

            Standard_Initialization();
            Secondary_Initialization();
            EagerLoadOff_Initilization();
            TryParse();
            TryParse_EagerLoad_Off();
            Property_Change();

            Celestial_Calculations();
            Celestial_Calculations_Local_Time_From_Coordinate();
            Solar_Cycle_Calculations();
            Lunar_Cycle_Calculations();

            Console.WriteLine();
            Console.WriteLine("TEST COMPLETE");
            Console.ReadKey();
        }

        public static void Standard_Initialization()
        {
            Benchmark(() => { var tc = new Coordinate(39.891219, -74.872435, new DateTime(2018, 7, 26, 15, 49, 0)); }, 100, "Standard Initialization");
        }

        public static void Secondary_Initialization()
        {
            Benchmark(() => {
                var tc = new Coordinate();
                tc.Latitude = new CoordinatePart(39, 45, 34, CoordinatesPosition.N);
                tc.Longitude = new CoordinatePart(74, 34, 45, CoordinatesPosition.W);
                tc.GeoDate = new DateTime(2018, 7, 26, 15, 49, 0);

            }, 100, "Secondary Initialization");
        }

        public static void EagerLoadOff_Initilization()
        {
            Benchmark(() => {
                EagerLoad eg = new EagerLoad(false);

                var tc = new Coordinate(39.891219, -74.872435, new DateTime(2018, 7, 26, 15, 49, 0), eg);
            }, 100, "Eager Load Off Initialization");
            var tc = new Coordinate(39.891219, -74.872435, new DateTime(2018, 7, 26, 15, 49, 0));

        }
        public static void TryParse()
        {
            Benchmark(() => { Coordinate.TryParse("39.891219, -74.872435", new DateTime(2010, 7, 26, 15, 49, 0), out var tc); }, 100, "TryParse() Initialization");
        }

        public static void TryParse_EagerLoad_Off()
        {
            Benchmark(() => { Coordinate.TryParse("39.891219, -74.872435", new DateTime(2010, 7, 26, 15, 49, 0), new EagerLoad(false), out var tc); }, 100, "TryParse() Eager Load Off Initialization");
        }

        public static void Property_Change()
        {
            var tc = new Coordinate(39.891219, -74.872435, new DateTime(2018, 7, 26, 15, 49, 0));

            //Benchmark property change
            Random r = new Random();
            Benchmark(() => { tc.Latitude.DecimalDegree = r.Next(-90, 90); }, 100, "Property Change");
        }

        public static void Celestial_Calculations()
        {
            Benchmark(() => { Celestial cel = new Celestial(45, 45, DateTime.Now); }, 100, "Celestial Time Calculations");
        }
        public static void Celestial_Calculations_Local_Time_From_Coordinate()
        {
            EagerLoad el = new EagerLoad();
            Coordinate c = new Coordinate(45, 45, DateTime.Now, el);
            //Benchmark Local Times
            Benchmark(() => {

                Celestial cel = c.Celestial_LocalTime(-7);
            }, 100, "Local Celestial Times From Coordinate");
        }

        public static void Solar_Cycle_Calculations()
        {
            EagerLoad el = new EagerLoad();
            Coordinate c = new Coordinate(45, 45, DateTime.Now, el);

            el.Extensions = new EagerLoad_Extensions(EagerLoad_ExtensionsType.Solar_Cycle);
            Benchmark(() => {

                Celestial cel = c.Celestial_LocalTime(-7);
            }, 100, "Local Solar Cycle Only Times From Coordinate");
        
        }

        public static void Lunar_Cycle_Calculations()
        {
            EagerLoad el = new EagerLoad();
            Coordinate c = new Coordinate(45, 45, DateTime.Now, el);

            el.Extensions = new EagerLoad_Extensions(EagerLoad_ExtensionsType.Lunar_Cycle);
            Benchmark(() => {
                Celestial cel = c.Celestial_LocalTime(-7);
            }, 100, "Local Lunar Cycle Only Times From Coordinate");
        }  

    


        private static void Benchmark(Action act, int iterations, string bechmarkName)
        {
            GC.Collect();
            act.Invoke(); // run once outside of loop to avoid initialization costs
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                act.Invoke();
            }
            sw.Stop();
            Console.Write(bechmarkName + ": ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write((sw.ElapsedMilliseconds / iterations).ToString() + " ms");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }
    }
}
