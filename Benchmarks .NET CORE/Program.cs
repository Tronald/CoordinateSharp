using System;
using System.Diagnostics;
using CoordinateSharp;
using CoordinateSharp.Debuggers;
namespace Benchmarks
{
    class Program
    {
        static void Main()
        {
           
            Console.WriteLine("Starting .NET CORE 3.1 Benchmarks, this test may take a while to finish...");
            Console.WriteLine();

            Benchmarkers.Run_Benchmarks(OutputOption.Console);
          
            Console.WriteLine();
            Console.WriteLine("TEST COMPLETE");
            Console.ReadKey();
        }
    }
}
