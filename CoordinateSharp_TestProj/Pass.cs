using System;

namespace CoordinateSharp_TestProj
{
    public class Pass
    {
        public static void Write(string method, bool pass)
        {
            Console.Write(method + ": ");
            if (pass)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("PASS");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("FAILED");
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }
    }
}
