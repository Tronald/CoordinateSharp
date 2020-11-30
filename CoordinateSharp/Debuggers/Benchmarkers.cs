/*
CoordinateSharp is a .NET standard library that is intended to ease geographic coordinate 
format conversions and location based celestial calculations.
https://github.com/Tronald/CoordinateSharp

Many celestial formulas in this library are based on Jean Meeus's 
Astronomical Algorithms (2nd Edition). Comments that reference only a chapter
are referring to this work.

License

CoordinateSharp is split licensed and may be licensed under the GNU Affero General Public License version 3 or a commercial use license as stated.

Copyright (C) 2019, Signature Group, LLC
  
This program is free software; you can redistribute it and/or modify it under the terms of the GNU Affero General Public License version 3 
as published by the Free Software Foundation with the addition of the following permission added to Section 15 as permitted in Section 7(a): 
FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY Signature Group, LLC. Signature Group, LLC DISCLAIMS THE WARRANTY OF 
NON INFRINGEMENT OF THIRD PARTY RIGHTS.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Affero General Public License for more details. You should have received a copy of the GNU 
Affero General Public License along with this program; if not, see http://www.gnu.org/licenses or write to the 
Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA, 02110-1301 USA, or download the license from the following URL:

https://www.gnu.org/licenses/agpl-3.0.html

The interactive user interfaces in modified source and object code versions of this program must display Appropriate Legal Notices, 
as required under Section 5 of the GNU Affero General Public License.

You can be released from the requirements of the license by purchasing a commercial license. Buying such a license is mandatory 
as soon as you develop commercial activities involving the CoordinateSharp software without disclosing the source code of your own applications. 
These activities include: offering paid services to customers as an ASP, on the fly location based calculations in a web application, 
or shipping CoordinateSharp with a closed source product.

Organizations or use cases that fall under the following conditions may receive a free commercial use license upon request.
-Department of Defense
-Department of Homeland Security
-Open source contributors to this library
-Scholarly or scientific uses on a case by case basis.
-Emergency response / management uses on a case by case basis.

For more information, please contact Signature Group, LLC at this address: sales@signatgroup.com
*/
using System;
using System.Diagnostics;


namespace CoordinateSharp.Debuggers
{
    /// <summary>
    /// Built in benchmark tests for CoordinateSharp.
    /// Results write to console or debugger.
    /// </summary>
    public class Benchmarkers
    {
        /// <summary>
        /// Run built in benchmarks.
        /// </summary>
        public static void Run_Benchmarks(OutputOption opt)
        {           
            Standard_Initialization(opt);
            Secondary_Initialization(opt);
            EagerLoadOff_Initilization(opt);
            TryParse(opt);
            TryParse_EagerLoad_Off(opt);
            Property_Change(opt);

            Celestial_Calculations(opt);
            Celestial_Calculations_Local_Time_From_Coordinate(opt);
            Solar_Cycle_Calculations(opt);
            Lunar_Cycle_Calculations(opt);
        }

        /// <summary>
        /// Benchmarks Coordinate initialization method,
        /// </summary>
        public static void Standard_Initialization(OutputOption opt)
        {
            Benchmark(() => { var tc = new Coordinate(39.891219, -74.872435, new DateTime(2018, 7, 26, 15, 49, 0)); }, 100, "Standard Initialization", opt);
        }

        /// <summary>
        /// Benchmarks Coordinate Secondary initialization method.
        /// </summary>
        public static void Secondary_Initialization(OutputOption opt)
        {
            Benchmark(() => {
                var tc = new Coordinate();
                tc.Latitude = new CoordinatePart(39, 45, 34, CoordinatesPosition.N);
                tc.Longitude = new CoordinatePart(74, 34, 45, CoordinatesPosition.W);
                tc.GeoDate = new DateTime(2018, 7, 26, 15, 49, 0);

            }, 100, "Secondary Initialization", opt);
        }

        /// <summary>
        /// Benchmarks Coordinate initialization with eager loading settings turned off.
        /// </summary>
        public static void EagerLoadOff_Initilization(OutputOption opt)
        {
            Benchmark(() => {
                EagerLoad eg = new EagerLoad(false);

                var tc = new Coordinate(39.891219, -74.872435, new DateTime(2018, 7, 26, 15, 49, 0), eg);
            }, 100, "Eager Load Off Initialization", opt);
           

        }

        /// <summary>
        /// Benchmarks parsers.
        /// </summary>
        public static void TryParse(OutputOption opt)
        {
            Benchmark(() => { Coordinate.TryParse("39.891219, -74.872435", new DateTime(2010, 7, 26, 15, 49, 0), out var tc); }, 100, "TryParse() Initialization", opt);
        }

        /// <summary>
        /// Benchmarks parsers with eagerloading turned off.
        /// </summary>
        public static void TryParse_EagerLoad_Off(OutputOption opt)
        {
            Benchmark(() => { Coordinate.TryParse("39.891219, -74.872435", new DateTime(2010, 7, 26, 15, 49, 0), new EagerLoad(false), out var tc); }, 100, "TryParse() Eager Load Off Initialization", opt);
        }

        /// <summary>
        /// Benchmarks time to change a property of the Coordinate object.
        /// </summary>
        public static void Property_Change(OutputOption opt)
        {
            var tc = new Coordinate(39.891219, -74.872435, new DateTime(2018, 7, 26, 15, 49, 0));

            //Benchmark property change
            Random r = new Random();
            Benchmark(() => { tc.Latitude.DecimalDegree = r.Next(-90, 90); }, 100, "Property Change", opt);
        }

        /// <summary>
        /// Benchmarks all celestial calculations.
        /// </summary>
        public static void Celestial_Calculations(OutputOption opt)
        {
            Benchmark(() => { Celestial cel = new Celestial(45, 45, DateTime.Now); }, 100, "Celestial Time Calculations", opt);
        }
        /// <summary>
        /// Benchmarks all celestial calculations occurring in local time.
        /// </summary>
        public static void Celestial_Calculations_Local_Time_From_Coordinate(OutputOption opt)
        {
            EagerLoad el = new EagerLoad();
            Coordinate c = new Coordinate(45, 45, DateTime.Now, el);
            //Benchmark Local Times
            Benchmark(() => {

                Celestial cel = c.Celestial_LocalTime(-7);
            }, 100, "Local Celestial Times From Coordinate", opt);
        }

        /// <summary>
        /// Benchmark solar cycle calculations.
        /// </summary>
        public static void Solar_Cycle_Calculations(OutputOption opt)
        {
            EagerLoad el = new EagerLoad();
            Coordinate c = new Coordinate(45, 45, DateTime.Now, el);

            el.Extensions = new EagerLoad_Extensions(EagerLoad_ExtensionsType.Solar_Cycle);
            Benchmark(() => {

                Celestial cel = c.Celestial_LocalTime(-7);
            }, 100, "Local Solar Cycle Only Times From Coordinate", opt);

        }

        /// <summary>
        /// Benchmarks lunar cycle calculations.
        /// </summary>
        public static void Lunar_Cycle_Calculations(OutputOption opt)
        {
            EagerLoad el = new EagerLoad();
            Coordinate c = new Coordinate(45, 45, DateTime.Now, el);

            el.Extensions = new EagerLoad_Extensions(EagerLoad_ExtensionsType.Lunar_Cycle);
            Benchmark(() => {
                Celestial cel = c.Celestial_LocalTime(-7);
            }, 100, "Local Lunar Cycle Only Times From Coordinate", opt);
        }




        private static void Benchmark(Action act, int iterations, string bechmarkName, OutputOption opt)
        {
            GC.Collect();
            act.Invoke(); // run once outside of loop to avoid initialization costs
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                act.Invoke();
            }
            sw.Stop();

            if (opt == OutputOption.Console)
            {
                Console.Write(bechmarkName + ": ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write((sw.ElapsedMilliseconds / iterations).ToString() + " ms");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
            }
            else if (opt == OutputOption.Debugger)
            {
                Debug.WriteLine($"{bechmarkName}: {(sw.ElapsedMilliseconds / iterations).ToString()} ms");
            }
        }
    }
}
