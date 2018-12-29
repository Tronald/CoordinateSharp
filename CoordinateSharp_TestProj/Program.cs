/*
 * The following program is used to automate certain aspects of the testing of CoordinateSharp.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CoordinateSharp;
using System.Drawing;
using System.Diagnostics;
using System.Reflection;
namespace CoordinateSharp_TestProj
{
    class Program
    {
      
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Select Test Module to Run (Enter Test Number):");
                Console.WriteLine();
                Console.WriteLine("1. Coordinate Initialzations");
                Console.WriteLine("2. Coordinate Conversions");
                Console.WriteLine("3. Coordinate Parsers");
                Console.WriteLine("4. Celestial");
                Console.WriteLine("5. Distance Initialization Tests");
                Console.WriteLine("6. Benchmarks");
                Console.WriteLine("7. GeoFence Tests");
                Console.WriteLine("8. EagerLoading Tests");
                Console.WriteLine("9. ..Exit");
                Console.WriteLine();
                Console.Write("Select a Test Number: ");
                ConsoleKeyInfo t = Console.ReadKey();
                Console.WriteLine();
                Console.WriteLine("*********************");
                Console.WriteLine();
                switch(t.Key)
                {
                    case ConsoleKey.D1:                     
                        Coordinate_Initialization_Tests();                       
                        break;
                    case ConsoleKey.D2:
                        Coordinate_Convsersions_Tests();
                        break;
                    case ConsoleKey.D3:
                        Coordinate_Parsers_Tests();
                        break;
                    case ConsoleKey.D4:
                        Celestial_Tests();
                        break;
                    case ConsoleKey.D5:
                        Distance_Initialization_Tests();
                        break;
                    case ConsoleKey.D6:
                        Benchmark_Tests();
                        break;
                    case ConsoleKey.D7:
                        GeoFence_Tests();
                        break;
                    case ConsoleKey.D8:
                        EagerLoading_Tests();
                        break;
                    case ConsoleKey.D9:
                        return;
                    default:
                        Console.WriteLine();                      
                        Colorful.Console.WriteLine("Test choice invalid.", Color.Red);            
                        break;
                }
                Console.WriteLine();
                Console.WriteLine();
            }
            
        }

        #region Coordinate Initialization
        static void Coordinate_Initialization_Tests()
        {
            //Check for errors with initialization as most calculations occur on load
            bool pass = true;
            try
            {
                Coordinate c = new Coordinate();
                c = new Coordinate(25, 25);
                c = new Coordinate(25, 25, new DateTime(2018, 8, 5, 10, 10, 0));

                EagerLoad eg = new EagerLoad();
                eg.Cartesian = false;
                eg.Celestial = false;
                eg.UTM_MGRS = false;

                c = new Coordinate(eg);
                c = new Coordinate(25, 25, eg);
                c = new Coordinate(25, 25, new DateTime(2018, 8, 5, 10, 10, 0), eg);
            }
            catch { pass = false; }
            Write_Pass("Coordinate Initialization Error Checks", pass);

            try
            {
                pass = true;
                Coordinate c = new Coordinate();
                CoordinatePart cp = new CoordinatePart(CoordinateType.Lat, c);
                cp = new CoordinatePart(CoordinateType.Long, c);
                cp = new CoordinatePart(25, CoordinateType.Lat, c);
                cp = new CoordinatePart(25, CoordinateType.Long, c);
                cp = new CoordinatePart(25, 25, CoordinatesPosition.N, c);
                cp = new CoordinatePart(25, 25, CoordinatesPosition.E, c);
                cp = new CoordinatePart(25, 25, CoordinatesPosition.S, c);
                cp = new CoordinatePart(25, 25, CoordinatesPosition.W, c);
                cp = new CoordinatePart(25, 25, 25, CoordinatesPosition.N, c);
                cp = new CoordinatePart(25, 25, 25, CoordinatesPosition.E, c);
                cp = new CoordinatePart(25, 25, 25, CoordinatesPosition.S, c);
                cp = new CoordinatePart(25, 25, 25, CoordinatesPosition.W, c);
            }
            catch { pass = false; }
            Write_Pass("CoordinatePart Initialization Error Checks", pass);
            try
            {
                pass = true;
                UniversalTransverseMercator utm = new UniversalTransverseMercator("Q", 14, 581943.5, 2111989.8);
                utm = new UniversalTransverseMercator("Q", 14, 581943.5, 2111989.8, 6378160.000, 298.25);
            }
            catch { pass = false; }
            Write_Pass("UniversalTransverseMercator Initialization Error Checks", pass);
            try
            {
                pass = true;
                //Outputs 19T CE 51307 93264
                MilitaryGridReferenceSystem mgrs = new MilitaryGridReferenceSystem("T", 19, "CE", 51307, 93264);
                mgrs = new MilitaryGridReferenceSystem("T", 19, "CE", 51307, 93264, 6378160.000, 298.25);
            }
            catch { pass = false; }
            Write_Pass("UniversalTransverseMercator Initialization Error Checks", pass);
            try
            {
                pass = true;
                Coordinate c = new Coordinate();
                Cartesian cart = new Cartesian(c);
                cart = new Cartesian(345, -123, 2839);
            }
            catch { pass = false; }
            Write_Pass("Cartesian Initialization Error Checks", pass);
            Console.WriteLine();

            //Init Range Checks
            try
            {
                pass = true;
                EagerLoad eg = new EagerLoad();

                Coordinate c = new Coordinate(90, 180);            
                c = new Coordinate(-90, -180);
                c = new Coordinate(90, 180, new DateTime());
                c = new Coordinate(-90, -180, new DateTime());
                c = new Coordinate(90, 180, eg);
                c = new Coordinate(-90, -180, eg);
                c = new Coordinate(90, 180, new DateTime(), eg);
                c = new Coordinate(-90, -180, new DateTime(), eg);

                //Should fail as arguments are out of range.
                try { c = new Coordinate(91, 180); pass = false; }
                catch { }
                //Should fail as arguments are out of range.
                try { c = new Coordinate(90, 181); pass = false; }
                catch { }
                //Should fail as arguments are out of range.
                try { c = new Coordinate(-91, -180); pass = false; }
                catch { }
                //Should fail as arguments are out of range.
                try { c = new Coordinate(-90, -181); pass = false; }
                catch { }

                //Should fail as arguments are out of range.
                try { c = new Coordinate(91, 180, new DateTime()); pass = false; }
                catch { }
                //Should fail as arguments are out of range.
                try { c = new Coordinate(90, 181, new DateTime()); pass = false; }
                catch { }
                //Should fail as arguments are out of range.
                try { c = new Coordinate(-91, -180, new DateTime()); pass = false; }
                catch { }
                //Should fail as arguments are out of range.
                try { c = new Coordinate(-90, -181, new DateTime()); pass = false; }
                catch {  }

                //Should fail as arguments are out of range.
                try { c = new Coordinate(91, 180, new DateTime(), eg); pass = false; }
                catch { }
                //Should fail as arguments are out of range.
                try { c = new Coordinate(90, 181, new DateTime(), eg); pass = false; }
                catch { }
                //Should fail as arguments are out of range.
                try { c = new Coordinate(-91, -180, new DateTime(), eg); pass = false; }
                catch { }
                //Should fail as arguments are out of range.
                try { c = new Coordinate(-90, -181, new DateTime(), eg); pass = false; }
                catch { }
            }
            catch { pass = false; }
            Write_Pass("Cordinate Initialization Range Checks", pass);

            pass = true;
            try
            {
                Coordinate c = new Coordinate();
                CoordinatePart cp = new CoordinatePart(90, CoordinateType.Lat, c);
                cp = new CoordinatePart(-90, CoordinateType.Lat, c);
                cp = new CoordinatePart(89, 59,  CoordinatesPosition.N, c);
                cp = new CoordinatePart(89, 59, CoordinatesPosition.S, c);
                cp = new CoordinatePart(89, 59, 59, CoordinatesPosition.N, c);
                cp = new CoordinatePart(89, 59, 59, CoordinatesPosition.S, c);
                cp = new CoordinatePart(180, CoordinateType.Long, c);
                cp = new CoordinatePart(-180, CoordinateType.Long, c);
                cp = new CoordinatePart(179, 59, CoordinatesPosition.E, c);
                cp = new CoordinatePart(179, 59, CoordinatesPosition.W, c);
                cp = new CoordinatePart(179, 59, 59, CoordinatesPosition.E, c);
                cp = new CoordinatePart(179, 59, 59, CoordinatesPosition.W, c);

                //Should fail
                try { cp = new CoordinatePart(91, CoordinateType.Lat, c); pass = false; } catch { }
                try { cp = new CoordinatePart(-91, CoordinateType.Lat, c); pass = false; } catch { }
                try { cp = new CoordinatePart(181, CoordinateType.Long, c); pass = false; } catch { }
                try { cp = new CoordinatePart(-181, CoordinateType.Long, c); pass = false; } catch { }

                try { cp = new CoordinatePart(91, 0, CoordinatesPosition.N, c); pass = false; } catch { }
                try { cp = new CoordinatePart(90, 1, CoordinatesPosition.N, c); pass = false; } catch { }
                try { cp = new CoordinatePart(89, 60, CoordinatesPosition.N, c); pass = false; } catch { }
                try { cp = new CoordinatePart(91, 0, CoordinatesPosition.N, c); pass = false; } catch { }
                try { cp = new CoordinatePart(90, 1, CoordinatesPosition.N, c); pass = false; } catch { }
                try { cp = new CoordinatePart(89, 60, CoordinatesPosition.N, c); pass = false; } catch { }
                try { cp = new CoordinatePart(-90, 1, CoordinatesPosition.N, c); pass = false; } catch { }
                try { cp = new CoordinatePart(89, -1, CoordinatesPosition.N, c); pass = false; } catch { }

                try { cp = new CoordinatePart(91, 0, CoordinatesPosition.S, c); pass = false; } catch { }
                try { cp = new CoordinatePart(90, 1, CoordinatesPosition.S, c); pass = false; } catch { }
                try { cp = new CoordinatePart(89, 60, CoordinatesPosition.S, c); pass = false; } catch { }
                try { cp = new CoordinatePart(91, 0, CoordinatesPosition.S, c); pass = false; } catch { }
                try { cp = new CoordinatePart(90, 1, CoordinatesPosition.S, c); pass = false; } catch { }
                try { cp = new CoordinatePart(89, 60, CoordinatesPosition.S, c); pass = false; } catch { }
                try { cp = new CoordinatePart(-90, 1, CoordinatesPosition.S, c); pass = false; } catch { }
                try { cp = new CoordinatePart(89, -1, CoordinatesPosition.S, c); pass = false; } catch { }

                try { cp = new CoordinatePart(91, 0, 0, CoordinatesPosition.N, c); pass = false; } catch { }
                try { cp = new CoordinatePart(90, 0, 1, CoordinatesPosition.N, c); pass = false; } catch { }
                try { cp = new CoordinatePart(89, 59, 60, CoordinatesPosition.N, c); pass = false; } catch { }
                try { cp = new CoordinatePart(90, 0, 1, CoordinatesPosition.N, c); pass = false; } catch { }
                try { cp = new CoordinatePart(89, 59, 60, CoordinatesPosition.N, c); pass = false; } catch { }
                try { cp = new CoordinatePart(-90, 0, 0, CoordinatesPosition.N, c); pass = false; } catch { }
                try { cp = new CoordinatePart(89, -1, 0, CoordinatesPosition.N, c); pass = false; } catch { }
                try { cp = new CoordinatePart(89, 1, -1, CoordinatesPosition.N, c); pass = false; } catch { }

                try { cp = new CoordinatePart(91, 0, 0, CoordinatesPosition.S, c); pass = false; } catch { }
                try { cp = new CoordinatePart(90, 0, 1, CoordinatesPosition.S, c); pass = false; } catch { }
                try { cp = new CoordinatePart(89, 59, 60, CoordinatesPosition.S, c); pass = false; } catch { }
                try { cp = new CoordinatePart(90, 0, 1, CoordinatesPosition.S, c); pass = false; } catch { }
                try { cp = new CoordinatePart(89, 59, 60, CoordinatesPosition.S, c); pass = false; } catch { }
                try { cp = new CoordinatePart(-90, 0, 0, CoordinatesPosition.S, c); pass = false; } catch { }
                try { cp = new CoordinatePart(89, -1, 0, CoordinatesPosition.S, c); pass = false; } catch { }
                try { cp = new CoordinatePart(89, 1, -1, CoordinatesPosition.S, c); pass = false; } catch { }


                try { cp = new CoordinatePart(181, 0, CoordinatesPosition.E, c); pass = false; } catch { }
                try { cp = new CoordinatePart(180, 1, CoordinatesPosition.E, c); pass = false; } catch { }
                try { cp = new CoordinatePart(179, 60, CoordinatesPosition.E, c); pass = false; } catch { }
                try { cp = new CoordinatePart(181, 0, CoordinatesPosition.E, c); pass = false; } catch { }
                try { cp = new CoordinatePart(180, 1, CoordinatesPosition.E, c); pass = false; } catch { }
                try { cp = new CoordinatePart(179, 60, CoordinatesPosition.E, c); pass = false; } catch { }
                try { cp = new CoordinatePart(-180, 1, CoordinatesPosition.E, c); pass = false; } catch { }
                try { cp = new CoordinatePart(179, -1, CoordinatesPosition.E, c); pass = false; } catch { }

                try { cp = new CoordinatePart(181, 0, CoordinatesPosition.W, c); pass = false; } catch { }
                try { cp = new CoordinatePart(180, 1, CoordinatesPosition.W, c); pass = false; } catch { }
                try { cp = new CoordinatePart(179, 60, CoordinatesPosition.W, c); pass = false; } catch { }
                try { cp = new CoordinatePart(181, 0, CoordinatesPosition.W, c); pass = false; } catch { }
                try { cp = new CoordinatePart(180, 1, CoordinatesPosition.W, c); pass = false; } catch { }
                try { cp = new CoordinatePart(179, 60, CoordinatesPosition.W, c); pass = false; } catch { }
                try { cp = new CoordinatePart(-180, 1, CoordinatesPosition.W, c); pass = false; } catch { }
                try { cp = new CoordinatePart(179, -1, CoordinatesPosition.W, c); pass = false; } catch { }

                try { cp = new CoordinatePart(181, 0, 0, CoordinatesPosition.E, c); pass = false; } catch { }
                try { cp = new CoordinatePart(180, 0, 1, CoordinatesPosition.E, c); pass = false; } catch { }
                try { cp = new CoordinatePart(179, 59, 60, CoordinatesPosition.E, c); pass = false; } catch { }
                try { cp = new CoordinatePart(180, 0, 1, CoordinatesPosition.E, c); pass = false; } catch { }
                try { cp = new CoordinatePart(179, 59, 60, CoordinatesPosition.E, c); pass = false; } catch { }
                try { cp = new CoordinatePart(-180, 0, 0, CoordinatesPosition.E, c); pass = false; } catch { }
                try { cp = new CoordinatePart(179, -1, 0, CoordinatesPosition.E, c); pass = false; } catch { }
                try { cp = new CoordinatePart(179, 1, -1, CoordinatesPosition.E, c); pass = false; } catch { }

                try { cp = new CoordinatePart(181, 0, 0, CoordinatesPosition.W, c); pass = false; } catch { }
                try { cp = new CoordinatePart(180, 0, 1, CoordinatesPosition.W, c); pass = false; } catch { }
                try { cp = new CoordinatePart(179, 59, 60, CoordinatesPosition.W, c); pass = false; } catch { }
                try { cp = new CoordinatePart(180, 0, 1, CoordinatesPosition.W, c); pass = false; } catch { }
                try { cp = new CoordinatePart(179, 59, 60, CoordinatesPosition.W, c); pass = false; } catch { }
                try { cp = new CoordinatePart(-180, 0, 0, CoordinatesPosition.W, c); pass = false; } catch { }
                try { cp = new CoordinatePart(179, -1, 0, CoordinatesPosition.W, c); pass = false; } catch { }
                try { cp = new CoordinatePart(179, 1, -1, CoordinatesPosition.W, c); pass = false; } catch { }
            }
            catch
            {
                pass = false;
            }
            Write_Pass("CordinatePart Initialization Range Checks", pass);
        }
        #endregion

        static void Coordinate_Convsersions_Tests()
        {
            Console.WriteLine();
            //GATHER CONVERSIONS
            //Conversion lists must end in //** to signify end of list
            List<List<string>> Conversions = new List<List<string>>();
            string[] coordStrings = File.ReadAllLines("Conversions.txt");
            List<string> cList = new List<string>();
            foreach (string c in coordStrings)
            {
                if (c == "//**")
                {
                    Conversions.Add(cList);
                    cList = new List<string>(); }
                else
                {
                    cList.Add(c);
                }
            }
            //Conversion coords to test
            List<double[]> coords = new List<double[]>();
            coords.Add(new double[] { 39.5768, 72.4859 });
            coords.Add(new double[] { -15.5768, 100.4859 });
            coords.Add(new double[] { 65.25, -15.1859 });
            coords.Add(new double[] { -80.6586, -152.49 });

            for (int x = 0; x<Conversions.Count;x++)
            {
                List<string> coordList = Conversions[x];
                double lat = coords[x][0];
                double lng = coords[x][1];
                //0 = Decimal / Signed
                //1 = Decimal Degree
                //2 = Degree Decimal Minute
                //3 = Degree Minutes Seconds
                //4 = UTM
                //5 = MGRS
                //6 = Cartesian
                Coordinate c = new Coordinate(lat, lng);
                bool pass = true;
                Coordinate rc = new Coordinate();
                for (int y = 0; y<7;y++)
                {
                    
                    switch(y)
                    {
                        case 0:
                            c.FormatOptions.Format = CoordinateFormatType.Decimal;
                            if (c.ToString() != coordList[y]) { pass = false; }
                            break;
                        case 1:
                            c.FormatOptions.Format = CoordinateFormatType.Decimal_Degree;
                            if (c.ToString() != coordList[y]) { pass = false; }
                            break;
                        case 2:
                            c.FormatOptions.Format = CoordinateFormatType.Degree_Decimal_Minutes;
                            if (c.ToString() != coordList[y]) { pass = false; }
                            rc = new Coordinate();
                            rc.Latitude = new CoordinatePart(c.Latitude.Degrees, c.Latitude.DecimalMinute, c.Latitude.Position, rc);
                            rc.Longitude = new CoordinatePart(c.Longitude.Degrees, c.Longitude.DecimalMinute, c.Longitude.Position, rc);
                            if(rc.Latitude.ToDouble() != c.Latitude.ToDouble()) { pass = false; Debug.WriteLine("...Conversion Outside Limits: " + rc.Latitude.ToDouble() + " - " + c.Latitude.ToDouble()); }
                            if (rc.Longitude.ToDouble() != c.Longitude.ToDouble()) { pass = false; Debug.WriteLine("...Conversion Outside Limits: " + rc.Longitude.ToDouble() + " - " + c.Longitude.ToDouble()); }
                            break;
                        case 3:
                            c.FormatOptions.Format = CoordinateFormatType.Degree_Minutes_Seconds;
                            if (c.ToString() != coordList[y]) { pass = false; }
                            rc = new Coordinate();
                            rc.Latitude = new CoordinatePart(c.Latitude.Degrees, c.Latitude.Minutes, c.Latitude.Seconds, c.Latitude.Position, rc);
                            rc.Longitude = new CoordinatePart(c.Longitude.Degrees, c.Longitude.Minutes, c.Longitude.Seconds, c.Longitude.Position, rc);
                            if (rc.Latitude.ToDouble() != c.Latitude.ToDouble()) { pass = false; Debug.WriteLine("...Conversion Outside Limits: " + rc.Latitude.ToDouble() + " - " + c.Latitude.ToDouble()); }
                            if (rc.Longitude.ToDouble() != c.Longitude.ToDouble()) { pass = false; Debug.WriteLine("...Conversion Outside Limits: " + rc.Longitude.ToDouble() + " - " + c.Longitude.ToDouble()); }
                            break;
                        case 4:
                            if (c.UTM.ToString() != coordList[y] && c.UTM.WithinCoordinateSystemBounds) { pass = false; }
                            UniversalTransverseMercator utm = new UniversalTransverseMercator(c.UTM.LatZone, c.UTM.LongZone, c.UTM.Easting, c.UTM.Northing);
                            rc = UniversalTransverseMercator.ConvertUTMtoLatLong(utm);
                            if (Math.Abs(rc.Latitude.ToDouble() - c.Latitude.ToDouble()) >= .00001 && c.UTM.WithinCoordinateSystemBounds) { pass = false; Debug.WriteLine("...UTM Conversion Outside Limits: " + rc.Latitude.ToDouble() + " - " + c.Latitude.ToDouble()); }
                            if (Math.Abs(rc.Longitude.ToDouble() - c.Longitude.ToDouble()) >=.00001 && c.UTM.WithinCoordinateSystemBounds) { pass = false; Debug.WriteLine("...UTM Conversion Outside Limits: " + rc.Longitude.ToDouble() + " - " + c.Longitude.ToDouble()); }
                            break;
                        case 5:
                            if (c.MGRS.ToString() != coordList[y] && c.MGRS.WithinCoordinateSystemBounds) { pass = false; }
                            MilitaryGridReferenceSystem mgrs = new MilitaryGridReferenceSystem(c.MGRS.LatZone, c.MGRS.LongZone, c.MGRS.Digraph, c.MGRS.Easting, c.MGRS.Northing);
                            rc = MilitaryGridReferenceSystem.MGRStoLatLong(mgrs);
                            if (Math.Abs(rc.Latitude.ToDouble() - c.Latitude.ToDouble()) >= .0001 && c.MGRS.WithinCoordinateSystemBounds) { pass = false; Debug.WriteLine("...MGRS Conversion Outside Limits: " + rc.Latitude.ToDouble() + " - " + c.Latitude.ToDouble()); }
                            if (Math.Abs(rc.Longitude.ToDouble() - c.Longitude.ToDouble()) >= .0001 && c.MGRS.WithinCoordinateSystemBounds) { pass = false; Debug.WriteLine("...MGRS Conversion Outside Limits: " + rc.Longitude.ToDouble() + " - " + c.Longitude.ToDouble()); }

                            break;
                        case 6:
                            if (c.Cartesian.ToString() != coordList[y]) { pass = false; }
                            Cartesian cart = new Cartesian(c.Cartesian.X, c.Cartesian.Y, c.Cartesian.Z);
                            rc = Cartesian.CartesianToLatLong(cart);
                            if (Math.Abs(rc.Latitude.ToDouble() - c.Latitude.ToDouble()) >= .00001) { pass = false; Debug.WriteLine("...Cartesian Conversion Outside Limits: " + rc.Latitude.ToDouble() + " - " + c.Latitude.ToDouble()); }
                            if (Math.Abs(rc.Longitude.ToDouble() - c.Longitude.ToDouble()) >= .00001) { pass = false; Debug.WriteLine("...Cartesian Conversion Outside Limits: " + rc.Longitude.ToDouble() + " - " + c.Longitude.ToDouble()); }
                            break;
                        default:
                            break;
                    }
                }

                Write_Pass("Conversion Pass " + ((int)(x+1)).ToString() + ": ", pass);
                
            }

            //UTM MGRS BOUNDARY CHECK
            bool p = true;
            Coordinate cr = new Coordinate(-79.99, 0);
            if(!cr.UTM.WithinCoordinateSystemBounds || !cr.MGRS.WithinCoordinateSystemBounds) { p = false; }
            cr = new Coordinate(-80, 0);
            if (cr.UTM.WithinCoordinateSystemBounds || cr.MGRS.WithinCoordinateSystemBounds) { p = false; }
            cr = new Coordinate(83.99, 0);
            if (!cr.UTM.WithinCoordinateSystemBounds || !cr.MGRS.WithinCoordinateSystemBounds) { p = false; }
            cr = new Coordinate(84, 0);
            if (cr.UTM.WithinCoordinateSystemBounds || cr.MGRS.WithinCoordinateSystemBounds) { p = false; }
            Write_Pass("UTM MGRS BOUNDARY CHECK", p);
        }
        static void Coordinate_Parsers_Tests()
        {
            CoordinatePart cp;
            Coordinate coordinate;
      
            //Parse Coordinate Formats
            string[] coordStrings = File.ReadAllLines("Coordinates.txt");
            bool pass = true;
            string lastType = "";
            Console.WriteLine("Coordinate Parses...");
            foreach (string c in coordStrings)
            {
                if (c.Contains("\\"))
                {
                    if (lastType!="")
                    {
                        Write_Pass(lastType.Split('#')[0], pass);
                       
                    }
                    lastType = "";
                    pass = true;
                    lastType = c;             
                }
                else
                {
                    string[] cc = c.Split('#');
                    if (!Coordinate.TryParse(cc[0], out coordinate))
                    {
                        pass = false;
                    }
                    else
                    {
                        if(coordinate.ToString() != cc[1])
                        {

                            Debug.WriteLine("...MISMATCH: " + coordinate.ToString() + " - " + cc[1]);
                            pass = false;
                        }
                    }
                }
            }
            if (lastType != "")
            {
                Write_Pass(lastType.Split(',')[0], pass);
            }
            //Attempt Forces Param
            pass = true;
            try
            {

                if (Coordinate.TryParse("95F, 54", out coordinate)) { pass = false; }//Intentional Fail
                if (Coordinate.TryParse("E 181 30, 56 76", out coordinate)) { pass = false; } //Intentional Fail
                if (Coordinate.TryParse("N 95 45, E 45", out coordinate)) { pass = false; } //Intentional Fail
                if (Coordinate.TryParse("95 87 46 78 D", out coordinate)) { pass = false; } //Intentional Fail
                if (Coordinate.TryParse("W24 45, W45", out coordinate)) { pass = false; } //Intentional Fail
            }
            catch { pass = false; }
            Console.WriteLine();
            Write_Pass("\\\\Intentional Fails", pass);
            Console.WriteLine();
            //Parse CoordinatePart Formats
            Console.WriteLine("CoordinatePart Parses...");
            coordStrings = File.ReadAllLines("CoordinateParts.txt");
            pass = true;
            lastType = "";
            foreach (string c in coordStrings)
            {
                c.Trim();
                if (c.Contains("\\"))
                {
                    if (lastType != "")
                    {
                        Write_Pass(lastType.Split('#')[0], pass);
                    }
                    lastType = "";
                    pass = true;
                    lastType = c;
                }
                else
                {
                    string[] cc = c.Split('#');
                    if (!CoordinatePart.TryParse(cc[0], out cp))
                    {
                        pass = false;
                    }
                    else
                    {
                        if (cp.ToString() != cc[1])
                        {
                            Debug.WriteLine("...MISMATCH: " + cp.ToString() + " - " + cc[1]);
                            pass = false;
                        }
                    }
                }
            }
            if (lastType != "")
            {
                Write_Pass(lastType.Split(',')[0], pass);
            }
            //Attempt Forces Param
            pass = true;
            try
            {

                if (CoordinatePart.TryParse("95", CoordinateType.Lat, out cp)) { pass = false; }//Intentional Fail
                if (CoordinatePart.TryParse("E181", CoordinateType.Lat, out cp)) { pass = false; } //Intentional Fail
                if (CoordinatePart.TryParse("N 95 45", CoordinateType.Lat, out cp)) { pass = false; } //Intentional Fail
                if (CoordinatePart.TryParse("95", CoordinateType.Lat, out cp)) { pass = false; } //Intentional Fail
                if (CoordinatePart.TryParse("WD24 45", CoordinateType.Lat, out cp)) { pass = false; } //Intentional Fail
            }
            catch { pass = false; }
            Console.WriteLine();
            Write_Pass("\\\\Intentional Fails", pass);
           
        }
        static void Celestial_Tests()
        {
            Console.WriteLine("Loading Celestial Values...");
            Console.WriteLine();
            CelestialTests ct = new CelestialTests();
            ct.Populate_CelestialTests();
            Write_Pass("Sunset: ", ct.Check_Values(ct.SunSets, "CelestialData\\SunSet.txt"));
            Write_Pass("Sunrise: ", ct.Check_Values(ct.SunRises, "CelestialData\\SunRise.txt"));
            Write_Pass("AstroDawn: ", ct.Check_Values(ct.AstroDawn, "CelestialData\\AstroDawn.txt"));
            Write_Pass("AstroDusk: ", ct.Check_Values(ct.AstroDusk, "CelestialData\\AstroDusk.txt"));
            Write_Pass("CivilDawn: ", ct.Check_Values(ct.CivilDawn, "CelestialData\\CivilDawn.txt"));
            Write_Pass("CivilDusk: ", ct.Check_Values(ct.CivilDusk, "CelestialData\\CivilDusk.txt"));
            Write_Pass("NauticalDawn: ", ct.Check_Values(ct.NauticalDawn, "CelestialData\\NauticalDawn.txt"));
            Write_Pass("NauticalDusk: ", ct.Check_Values(ct.NauticalDusk, "CelestialData\\NauticalDusk.txt"));
            Write_Pass("BottomSolarDiscRise: ", ct.Check_Values(ct.BottomSolarDiscRise, "CelestialData\\BottomDiscRise.txt"));
            Write_Pass("BottomSolarDiscSet: ", ct.Check_Values(ct.BottomSolarDiscSet, "CelestialData\\BottomDiscSet.txt"));
            Write_Pass("Moon Set: ", ct.Check_Values(ct.MoonSets, "CelestialData\\MoonSet.txt"));
            Write_Pass("Moon Rise: ", ct.Check_Values(ct.MoonRises, "CelestialData\\MoonRise.txt"));
            Console.WriteLine();
            Write_Pass("Sun Altitude: ", ct.Check_Values(ct.SunAlts, "CelestialData\\SunAlts.txt"));
            Write_Pass("Sun Azimuth: ", ct.Check_Values(ct.SunAzs, "CelestialData\\SunAzs.txt"));
            Write_Pass("Moon Altitude: ", ct.Check_Values(ct.MoonAlts, "CelestialData\\MoonAlts.txt"));
            Write_Pass("Moon Azimuth: ", ct.Check_Values(ct.MoonAzs, "CelestialData\\MoonAzs.txt"));
            Write_Pass("Moon Distance: ", ct.Check_Values(ct.MoonDistances, "CelestialData\\MoonDistance.txt"));
            Write_Pass("Moon Fraction: ", ct.Check_Values(ct.MoonFraction, "CelestialData\\MoonFraction.txt"));
            Write_Pass("Moon Phase ", ct.Check_Values(ct.MoonPhase, "CelestialData\\MoonPhase.txt"));
            Write_Pass("Moon Phase Name: ", ct.Check_Values(ct.MoonPhaseName, "CelestialData\\MoonPhaseName.txt"));
            Console.WriteLine();
            Write_Pass("Solar Eclipse: ", ct.Check_Solar_Eclipse());
            Write_Pass("Lunar Eclipse: ", ct.Check_Lunar_Eclipse());
            Write_Pass("Perigee: ", ct.Check_Perigee());
            Write_Pass("Apogee: ", ct.Check_Apogee());
        }

        #region Distance Tests
        static void Distance_Initialization_Tests()
        {
            //Conversions should be equal to these numbers within .0001 tolerance

            double m = 1000; //meters
            double km = 1; //Kilometers
            double ft = 3280.84; //Feet
            double sm = 0.6213712; //Nautical Miles
            double nm = 0.5399565; //Statute Miles

            double[] distances = new double[] {m,km,ft,nm,sm };

           
            Distance d = new Distance(km);

            Write_Pass("Distance(double km)", Check_Distance(d, distances));
            d = new Distance(distances[0], DistanceType.Meters);
            Console.WriteLine();
            Write_Pass("Distance(double distance, DistanceType Meters)", Check_Distance(d, distances));
            d = new Distance(distances[1], DistanceType.Kilometers);
            Write_Pass("Distance(double distance, DistanceType Kilometers)", Check_Distance(d, distances));
            d = new Distance(distances[2], DistanceType.Feet);
            Write_Pass("Distance(double distance, DistanceType Feet)", Check_Distance(d, distances));
            d = new Distance(distances[3], DistanceType.NauticalMiles);
            Write_Pass("Distance(double distance, DistanceType Nautical Miles)", Check_Distance(d, distances));
            d = new Distance(distances[4], DistanceType.Miles);
            Write_Pass("Distance(double distance, DistanceType Statute Miles)", Check_Distance(d, distances));
            Console.WriteLine();

            Coordinate c1 = new Coordinate(45, 72);
            Coordinate c2 = new Coordinate(42, 75);

            //KILOMETERS Between specified points above should be as follows in defined tolerance .000001
            double kmSphere = 412.0367538058125; 
            double kmWGS84 = 412.1977393206501; //Default datum WGS84

            d = new Distance(c1, c2);
            if(System.Math.Abs(d.Kilometers - kmSphere) > .000001) { Write_Pass("Distance(Coordinate c1, Coordinate c2)", false);
                Debug.WriteLine("...Mismatch: " + d.Kilometers + " - " + kmSphere);
            }
            else { Write_Pass("Distance(Coordinate c1, Coordinate c2)", true); }
            d = new Distance(c1, c2, Shape.Sphere);
            if (System.Math.Abs(d.Kilometers - kmSphere) > .000001) { Write_Pass("Distance(Coordinate c1, Coordinate c2, Shape.Sphere)", false);
                Debug.WriteLine("...Mismatch: " + d.Kilometers + " - " + kmSphere);
            }
            else { Write_Pass("Distance(Coordinate c1, Coordinate c2, Shape.Sphere)", true); }
            d = new Distance(c1, c2, Shape.Ellipsoid);
            if (System.Math.Abs(d.Kilometers - kmWGS84) > .000001) { Write_Pass("Distance(Coordinate c1, Coordinate c2, Shape.Ellipsoid)", false);
                Debug.WriteLine("...Mismatch: " + d.Kilometers + " - " + kmWGS84);
            }
            else { Write_Pass("Distance(Coordinate c1, Coordinate c2, Shape.Ellipsoid)", true); }
        }     
        static bool Check_Distance(Distance d, double[] distances)
        {
            bool pass = true;
            //Round to avoid float point issues
            double m = d.Meters;
            double km = d.Kilometers;
            double ft = d.Feet;
            double sm = d.Miles;
            double nm = d.NauticalMiles;
            if (System.Math.Abs(m - distances[0]) > .0001) { Debug.WriteLine("...METERS MISMATCH: " + d.Meters + " - " + distances[0]); return false; }
            if (System.Math.Abs(km - distances[1]) > .0001) { Debug.WriteLine("...KILOMETERS MISMATCH: " + d.Kilometers + " - " + distances[1]); return false; }
            if (System.Math.Abs(ft - distances[2]) > .0001) { Debug.WriteLine("...FEET MISMATCH: " + d.Feet + " - " + distances[2]); return false; }
            if (System.Math.Abs(nm - distances[3]) > .0001) { Debug.WriteLine("...NAUTICAL MILES MISMATCH: " + d.NauticalMiles + " - " + distances[3]); return false; }
            if (System.Math.Abs(sm - distances[4]) > .001) { Debug.WriteLine("...STATUTE MILE MISMATCH: " + d.Miles + " - " + distances[4]); return false; }
            return pass;
           
        }
        #endregion
        static void Benchmark_Tests()
        {

            Coordinate tc = null;
            Console.WriteLine("Starting Benchmarks, this test may take a while to finish...");
            Console.WriteLine();
            //Benchmark Standard Object Initialization
            Benchmark(() => { tc = new Coordinate(39.891219, -74.872435, new DateTime(2018, 7, 26, 15, 49, 0)); }, 100, "Standard Initialization");

            Benchmark(() => {
                tc = new Coordinate();
                tc.Latitude = new CoordinatePart(39, 45, 34, CoordinatesPosition.N, tc);
                tc.Longitude = new CoordinatePart(74, 34, 45, CoordinatesPosition.W, tc);
                tc.GeoDate = new DateTime(2018, 7, 26, 15, 49, 0);

            }, 100, "Secondary Initialization");

            //Benchmark TryParse Object Initialization
            Benchmark(() => { Coordinate.TryParse("39.891219, -74.872435", new DateTime(2010, 7, 26, 15, 49, 0), out tc); }, 100, "TryParse() Initialization");

            //Benchmark with EagerLoad fully off
            Benchmark(() => {
                EagerLoad eg = new EagerLoad();
                eg.UTM_MGRS = false;
                eg.Celestial = false;
                eg.Cartesian = false;
                tc = new Coordinate(39.891219, -74.872435, new DateTime(2018, 7, 26, 15, 49, 0), eg);
            }, 100, "EagerLoad Off Initialization");
            tc = new Coordinate(39.891219, -74.872435, new DateTime(2018, 7, 26, 15, 49, 0));

            //Benchmack property change
            Random r = new Random();
            Benchmark(() => { tc.Latitude.DecimalDegree = r.Next(-90, 90); }, 100, "Property Change");

        }
        static void Benchmark(Action act, int iterations, string s)
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
            Colorful.Console.Write((sw.ElapsedMilliseconds / iterations).ToString() + " ms", Color.Lime);
            Console.WriteLine();
        }
        static void GeoFence_Tests()
        {
            // CHANGE LATS e LONS
            Coordinate c1 = new Coordinate(31.66, -106.52);
            Coordinate c2 = new Coordinate(31.64, -106.53);
            Coordinate c3 = new Coordinate(42.02, -106.52);
            Coordinate c4 = new Coordinate(42.03, -106.53);

            List<GeoFence.Point> points = new List<GeoFence.Point>();

            points.Add(new GeoFence.Point(31.65, -106.52));
            points.Add(new GeoFence.Point(31.65, -84.02));
            points.Add(new GeoFence.Point(42.03, -84.02));
            points.Add(new GeoFence.Point(42.03, -106.52));
            points.Add(new GeoFence.Point(31.65, -106.52));

            GeoFence gf = new GeoFence(points);

            bool pass = true;
            if (gf.IsPointInPolygon(c1)) { pass = false; }
            if (gf.IsPointInPolygon(c2)) { pass = false; }
            if (gf.IsPointInPolygon(c3)) { pass = false; }
            if (gf.IsPointInPolygon(c4)) { pass = false; }

            Write_Pass("Outside Polygon: ", pass);

            c1 = new Coordinate(31.67, -106.51);
            c2 = new Coordinate(31.67, -84.03);
            c3 = new Coordinate(42.01, -106.51);
            c4 = new Coordinate(42.01, -84.03);

            pass = true;
            if (!gf.IsPointInPolygon(c1)) { pass = false; }
            if (!gf.IsPointInPolygon(c2)) { pass = false; }
            if (!gf.IsPointInPolygon(c3)) { pass = false; }
            if (!gf.IsPointInPolygon(c4)) { pass = false; }

            Write_Pass("Inside Polygon: ", pass);
           
            pass = true;
            Distance d = new Distance(1000, DistanceType.Meters);
            if (!gf.IsPointInRangeOfLine(c1, 1000)) { pass = false; }
            if (!gf.IsPointInRangeOfLine(c1, d)) { pass = false; }
            Write_Pass("In Range of Polyline: ", pass);

            pass = true;
            d = new Distance(900, DistanceType.Meters);
            if (gf.IsPointInRangeOfLine(c1, 900)) { pass = false; }
            if (gf.IsPointInRangeOfLine(c1, d)) { pass = false; }

            Write_Pass("Out of Range of Polyline: ", pass);

        }
        static void EagerLoading_Tests()
        {
            EagerLoad e = new EagerLoad(false);
            Coordinate c = new Coordinate(45, 75, new DateTime(2008,1,2),e);
            //Check to make sure items don't initialize
            bool pass = true;
            if (c.CelestialInfo != null) { pass = false; }
            if (c.UTM != null) { pass = false; }
            if (c.MGRS != null) { pass = false; }
            if (c.Cartesian != null) { pass = false; }
            Write_Pass("Null Properties (Celestial, UTM, MGRS, Cartesian)", pass);

            //Check Load_Calls
            pass = true;
            c.LoadCelestialInfo();
            if (c.CelestialInfo == null) { pass = false; }
            c.LoadUTM_MGRS_Info();
            if (c.UTM == null) { pass = false; }
            if (c.MGRS == null) { pass = false; }
            c.LoadCartesianInfo();
            if (c.Cartesian == null) { pass = false; }
            Write_Pass("Load Calls (Celestial, UTM, MGRS, Cartesian)", pass);

            if (pass)
            {
                Celestial cel = c.CelestialInfo;
                MilitaryGridReferenceSystem mgrs = c.MGRS;
                UniversalTransverseMercator utm = c.UTM;
                Cartesian cart = c.Cartesian;

                c.Latitude.DecimalDegree = -45;
                c.Longitude.DecimalDegree = -75;

                //Properties should not change.
                if(!ReflectiveEquals(c.CelestialInfo, cel)){ pass = false; }
                if (!ReflectiveEquals(c.MGRS, mgrs)){ pass = false; }
                if (!ReflectiveEquals(c.UTM, utm)){ pass = false; }
                if (!ReflectiveEquals(c.Cartesian, cart)){ pass = false; }
                //Properties should remain equal as no load calls were made
                Write_Pass("Property State Hold (Celestial, UTM, MGRS, Cartesian)", pass);

                //Properties should change
                pass = true;
                c.LoadCelestialInfo();
                c.LoadCartesianInfo();
                c.LoadUTM_MGRS_Info();
                if (ReflectiveEquals(c.CelestialInfo, cel)) { pass = false; }
                if (ReflectiveEquals(c.MGRS, mgrs)) { pass = false; }
                if (ReflectiveEquals(c.UTM, utm)) { pass = false; }
                if (ReflectiveEquals(c.Cartesian, cart)) { pass = false; }
                //Properties should not be equal as chages have been made
                Write_Pass("Property State Change (Celestial, UTM, MGRS, Cartesian)", pass);

            }
            else
            {
                //Passes auto fail has properties didn't load when called.

                Write_Pass("Property State Hold (Celestial, UTM, MGRS, Cartesian)", false);
                Write_Pass("Property State Change (Celestial, UTM, MGRS, Cartesian)", false);

            }

            //EagerLoaded Flags Test
            EagerLoadType et = EagerLoadType.Cartesian | EagerLoadType.Celestial | EagerLoadType.Cartesian;
            EagerLoad eg = new EagerLoad(et);
            pass = true;
            if(eg.Cartesian==false || eg.Celestial==false || eg.UTM_MGRS == false) { pass = false; }
            if (EagerLoad.Create(et).Cartesian == false || EagerLoad.Create(et).Celestial == false || EagerLoad.Create(et).UTM_MGRS == false) { pass = false; }
            Write_Pass("Flags Test", pass);
        }
        public static bool ReflectiveEquals(object first, object second)
        {
            if (first == null && second == null)
            {
                return true;
            }
            if (first == null || second == null)
            {
                return false;
            }
            Type firstType = first.GetType();
            if (second.GetType() != firstType)
            {
                return false; // Or throw an exception
            }
            // This will only use public properties. Is that enough?
            foreach (PropertyInfo propertyInfo in firstType.GetProperties())
            {
                if (propertyInfo.CanRead)
                {
                    object firstValue = propertyInfo.GetValue(first, null);
                    object secondValue = propertyInfo.GetValue(second, null);
                    if (!object.Equals(firstValue, secondValue))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        static void Write_Pass(string method, bool pass)
        {
            Console.Write(method + ": ");
            if (pass)
            { 
                Colorful.Console.Write("PASS", Color.Lime);
            }
            else
            {
                Colorful.Console.Write("FAILED", Color.Red);
            }
            Console.WriteLine();
        }
    }
}
