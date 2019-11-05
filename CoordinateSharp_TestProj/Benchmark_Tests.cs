using System;
using CoordinateSharp;
using System.Diagnostics;
namespace CoordinateSharp_TestProj
{
    public class Benchmark_Tests
    {
        public static void Run_Test()
        {
            Coordinate tc = null;
            Console.WriteLine("Starting Benchmarks, this test may take a while to finish...");
            Console.WriteLine();
            //Benchmark Standard Object Initialization
            Benchmark(() => { tc = new Coordinate(39.891219, -74.872435, new DateTime(2018, 7, 26, 15, 49, 0)); }, 100, "Standard Initialization");

            Benchmark(() => {
                tc = new Coordinate();
                tc.Latitude = new CoordinatePart(39, 45, 34, CoordinatesPosition.N);
                tc.Longitude = new CoordinatePart(74, 34, 45, CoordinatesPosition.W);
                tc.GeoDate = new DateTime(2018, 7, 26, 15, 49, 0);

            }, 100, "Secondary Initialization");

            //Benchmark TryParse Object Initialization
            Benchmark(() => { Coordinate.TryParse("39.891219, -74.872435", new DateTime(2010, 7, 26, 15, 49, 0), out tc); }, 100, "TryParse() Initialization");

            //Benchmark with EagerLoad fully off
            Benchmark(() => {
                EagerLoad eg = new EagerLoad(false);
              
                tc = new Coordinate(39.891219, -74.872435, new DateTime(2018, 7, 26, 15, 49, 0), eg);
            }, 100, "EagerLoad Off Initialization");
            tc = new Coordinate(39.891219, -74.872435, new DateTime(2018, 7, 26, 15, 49, 0));

            //Benchmark property change
            Random r = new Random();
            Benchmark(() => { tc.Latitude.DecimalDegree = r.Next(-90, 90); }, 100, "Property Change");

            //Benchmark Celestial Times
            Benchmark(() => { Celestial cel = new Celestial(45, 45, DateTime.Now); }, 100, "Celestial Time Calculations");

            EagerLoad el = new EagerLoad();
            Coordinate c = new Coordinate(45, 45, DateTime.Now, el);
            //Benchmark Local Times
            Benchmark(() => {
               
                Celestial cel = c.Celestial_LocalTime(-7);
            }, 100, "Local Celestial Times From Coordinate");
            el.Extensions = new EagerLoad_Extensions(EagerLoad_ExtensionsType.Solar_Cycle);
            Benchmark(() => {
               
                Celestial cel = c.Celestial_LocalTime(-7);
            }, 100, "Local Solar Cycle Only Times From Coordinate");
            el.Extensions = new EagerLoad_Extensions(EagerLoad_ExtensionsType.Lunar_Cycle);
            Benchmark(() => {              
                Celestial cel = c.Celestial_LocalTime(-7);
            }, 100, "Local Lunar Cycle Only Times From Coordinate");
            el = new EagerLoad();
            Benchmark(() => {
               
                Celestial cel = Celestial.CalculateCelestialTimes(45, 45, DateTime.Now, el, -7);
            }, 100, "Local Celestial Times From Direct Celestial");
            el = new EagerLoad(EagerLoadType.Celestial);
            el.Extensions = new EagerLoad_Extensions(EagerLoad_ExtensionsType.Solar_Cycle);
            Benchmark(() => {             
                Celestial cel = Celestial.CalculateCelestialTimes(45, 45, DateTime.Now, el, -7);
            }, 100, "Local Solar Cycle Only Times From Direct Celestial");
            el.Extensions = new EagerLoad_Extensions(EagerLoad_ExtensionsType.Lunar_Cycle);
            Benchmark(() => {
                Celestial cel = Celestial.CalculateCelestialTimes(45, 45, DateTime.Now, el, -7);              
            }, 100, "Local Lunar Cycle Only Times From Direct Celestial");


            //Benchmark EagerLoading Extension Times
            el = new EagerLoad();
            Benchmark(() => 
            {
                el.Extensions = new EagerLoad_Extensions(EagerLoad_ExtensionsType.Solar_Cycle);
                c = new Coordinate(45, 45, DateTime.Now, el); 
            }, 100, "Coordinate Initialization with Solar Cycle Calculations Only.");

            Benchmark(() =>
            {

                el.Extensions = new EagerLoad_Extensions(EagerLoad_ExtensionsType.Lunar_Cycle);
                c = new Coordinate(45, 45, DateTime.Now, el);
            }, 100, "Coordinate Initialization with Lunar Cycle Calculations Only.");


        }

        private static void Benchmark(Action act, int iterations, string s)
        {
            GC.Collect();
            act.Invoke(); // run once outside of loop to avoid initialization costs
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                act.Invoke();
            }
            sw.Stop();
            Console.Write(s + ": ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write((sw.ElapsedMilliseconds / iterations).ToString() + " ms");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }
    }
}
