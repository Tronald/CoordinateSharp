using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoordinateSharp;
using System.IO;
using System.Timers;
using System.Diagnostics;
namespace CoordinateSharp_Playground
{
    class Playground
    {
        static void Main(string[] args)
        {

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
