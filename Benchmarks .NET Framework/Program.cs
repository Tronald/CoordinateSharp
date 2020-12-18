using System;
using System.Diagnostics;
using CoordinateSharp;
using CoordinateSharp.Debuggers;
using CoordinateSharp.Magnetic;
namespace Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Starting .NET FRAMEWORK 4.5 Benchmarks, this test may take a while to finish...");
            Console.WriteLine();

            Benchmarkers.Run_Benchmarks(OutputOption.Console);
            Magnetic_Calculations(OutputOption.Console);

            Console.WriteLine();
            Console.WriteLine("TEST COMPLETE");
            Console.ReadKey();
        }

        /// <summary>
        /// Benchmark magnetic calculations.
        /// </summary>
        /// <param name="opt"></param>
        static void Magnetic_Calculations(OutputOption opt)
        {
            Coordinate c = new Coordinate(45, 45, new DateTime(2021, 1, 1), new EagerLoad(false));

            Benchmarkers.Benchmark(() => {
                Magnetic m = new Magnetic(c, DataModel.WMM2020);
                }, 100, "Magnetic Calculation Times", opt);
        }


    }
}
