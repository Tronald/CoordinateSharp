using System;
using System.Diagnostics;
using CoordinateSharp;
using CoordinateSharp.Debuggers;
namespace Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Starting .NET FRAMEWORK 4.5 Benchmarks, this test may take a while to finish...");
            Console.WriteLine();

            Benchmarkers.Run_Benchmarks(OutputOption.Console);

            Console.WriteLine();
            Console.WriteLine("TEST COMPLETE");
            Console.ReadKey();
        }

     
    }
}
