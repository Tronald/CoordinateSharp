/*
 * The following program is used to automate certain aspects of the testing of CoordinateSharp.
 */
using System;

namespace CoordinateSharp_TestProj
{
    class Program
    {
      
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            while (true)
            {
                Console.WriteLine("Select Test Module to Run (Enter Test Number):");
                Console.WriteLine();
                Console.WriteLine("1. Coordinate Initializations");
                Console.WriteLine("2. Coordinate Conversions");
                Console.WriteLine("3. Coordinate Parsers");
                Console.WriteLine("4. Celestial");
                Console.WriteLine("5. Distance Initialization / Move Tests");
                Console.WriteLine("6. Benchmarks");
                Console.WriteLine("7. GeoFence Tests");
                Console.WriteLine("8. EagerLoading Tests");
                Console.WriteLine("ESC. Exit");
                Console.WriteLine();
                Console.Write("Select a Test Number: ");
                ConsoleKeyInfo t = Console.ReadKey();
                Console.WriteLine();
                Console.WriteLine("*********************");
                Console.WriteLine();

                if (t.Key == ConsoleKey.D1) { Coordinate_Initialization.Run_Test(); }
                else if (t.Key == ConsoleKey.D2) { Coordinate_Conversion_Tests.Run_Test(); }
                else if (t.Key == ConsoleKey.D3) { Coordinate_Parser_Tests.Run_Test(); }
                else if (t.Key == ConsoleKey.D4) { CelestialTests.Run_Test(); }
                else if (t.Key == ConsoleKey.D5) { Distance_Tests.Run_Test(); }
                else if (t.Key == ConsoleKey.D6) { Benchmark_Tests.Run_Test(); }
                else if (t.Key == ConsoleKey.D7) { GeoFence_Tests.Run_Test(); }
                else if (t.Key == ConsoleKey.D8) { EagerLoading_Tests.Run_Tests(); }
                else if (t.Key == ConsoleKey.Escape) { return; }
                else
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Test choice invalid.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
               
                Console.WriteLine();
                Console.WriteLine();
            }
            
        }                  
    }
}
