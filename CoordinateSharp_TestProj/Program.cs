/*
 * The following program is used to automate certain aspects of the testing of CoordinateSharp.
 */
using System;
using System.Collections.Generic;
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
                Console.WriteLine("5. Distance Initialization / Move Tests");
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
                        Run_Distance_Test();
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
                CoordinatePart cp = new CoordinatePart(CoordinateType.Lat);
                cp = new CoordinatePart(CoordinateType.Long);
                cp = new CoordinatePart(25, CoordinateType.Lat);
                cp = new CoordinatePart(25, CoordinateType.Long);
                cp = new CoordinatePart(25, 25, CoordinatesPosition.N);
                cp = new CoordinatePart(25, 25, CoordinatesPosition.E);
                cp = new CoordinatePart(25, 25, CoordinatesPosition.S);
                cp = new CoordinatePart(25, 25, CoordinatesPosition.W);
                cp = new CoordinatePart(25, 25, 25, CoordinatesPosition.N);
                cp = new CoordinatePart(25, 25, 25, CoordinatesPosition.E);
                cp = new CoordinatePart(25, 25, 25, CoordinatesPosition.S);
                cp = new CoordinatePart(25, 25, 25, CoordinatesPosition.W);
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
            try
            {
                pass = true;
                Coordinate c = new Coordinate();
                ECEF ecef = new ECEF(c);
                ecef = new ECEF(3194.469, 3194.469, 4487.419);
            }
            catch { pass = false; }
            Write_Pass("ECEF Initialization Error Checks", pass);
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
            Write_Pass("Coordinate Initialization Range Checks", pass);

            pass = true;
            try
            {
                Coordinate c = new Coordinate();
                CoordinatePart cp = new CoordinatePart(90, CoordinateType.Lat);
                cp = new CoordinatePart(-90, CoordinateType.Lat);
                cp = new CoordinatePart(89, 59,  CoordinatesPosition.N);
                cp = new CoordinatePart(89, 59, CoordinatesPosition.S);
                cp = new CoordinatePart(89, 59, 59, CoordinatesPosition.N);
                cp = new CoordinatePart(89, 59, 59, CoordinatesPosition.S);
                cp = new CoordinatePart(180, CoordinateType.Long);
                cp = new CoordinatePart(-180, CoordinateType.Long);
                cp = new CoordinatePart(179, 59, CoordinatesPosition.E);
                cp = new CoordinatePart(179, 59, CoordinatesPosition.W);
                cp = new CoordinatePart(179, 59, 59, CoordinatesPosition.E);
                cp = new CoordinatePart(179, 59, 59, CoordinatesPosition.W);

                //Should fail
                try { cp = new CoordinatePart(91, CoordinateType.Lat); pass = false; } catch { }
                try { cp = new CoordinatePart(-91, CoordinateType.Lat); pass = false; } catch { }
                try { cp = new CoordinatePart(181, CoordinateType.Long); pass = false; } catch { }
                try { cp = new CoordinatePart(-181, CoordinateType.Long); pass = false; } catch { }

                try { cp = new CoordinatePart(91, 0, CoordinatesPosition.N); pass = false; } catch { }
                try { cp = new CoordinatePart(90, 1, CoordinatesPosition.N); pass = false; } catch { }
                try { cp = new CoordinatePart(89, 60, CoordinatesPosition.N); pass = false; } catch { }
                try { cp = new CoordinatePart(91, 0, CoordinatesPosition.N); pass = false; } catch { }
                try { cp = new CoordinatePart(90, 1, CoordinatesPosition.N); pass = false; } catch { }
                try { cp = new CoordinatePart(89, 60, CoordinatesPosition.N); pass = false; } catch { }
                try { cp = new CoordinatePart(-90, 1, CoordinatesPosition.N); pass = false; } catch { }
                try { cp = new CoordinatePart(89, -1, CoordinatesPosition.N); pass = false; } catch { }

                try { cp = new CoordinatePart(91, 0, CoordinatesPosition.S); pass = false; } catch { }
                try { cp = new CoordinatePart(90, 1, CoordinatesPosition.S); pass = false; } catch { }
                try { cp = new CoordinatePart(89, 60, CoordinatesPosition.S); pass = false; } catch { }
                try { cp = new CoordinatePart(91, 0, CoordinatesPosition.S); pass = false; } catch { }
                try { cp = new CoordinatePart(90, 1, CoordinatesPosition.S); pass = false; } catch { }
                try { cp = new CoordinatePart(89, 60, CoordinatesPosition.S); pass = false; } catch { }
                try { cp = new CoordinatePart(-90, 1, CoordinatesPosition.S); pass = false; } catch { }
                try { cp = new CoordinatePart(89, -1, CoordinatesPosition.S); pass = false; } catch { }

                try { cp = new CoordinatePart(91, 0, 0, CoordinatesPosition.N); pass = false; } catch { }
                try { cp = new CoordinatePart(90, 0, 1, CoordinatesPosition.N); pass = false; } catch { }
                try { cp = new CoordinatePart(89, 59, 60, CoordinatesPosition.N); pass = false; } catch { }
                try { cp = new CoordinatePart(90, 0, 1, CoordinatesPosition.N); pass = false; } catch { }
                try { cp = new CoordinatePart(89, 59, 60, CoordinatesPosition.N); pass = false; } catch { }
                try { cp = new CoordinatePart(-90, 0, 0, CoordinatesPosition.N); pass = false; } catch { }
                try { cp = new CoordinatePart(89, -1, 0, CoordinatesPosition.N); pass = false; } catch { }
                try { cp = new CoordinatePart(89, 1, -1, CoordinatesPosition.N); pass = false; } catch { }

                try { cp = new CoordinatePart(91, 0, 0, CoordinatesPosition.S); pass = false; } catch { }
                try { cp = new CoordinatePart(90, 0, 1, CoordinatesPosition.S); pass = false; } catch { }
                try { cp = new CoordinatePart(89, 59, 60, CoordinatesPosition.S); pass = false; } catch { }
                try { cp = new CoordinatePart(90, 0, 1, CoordinatesPosition.S); pass = false; } catch { }
                try { cp = new CoordinatePart(89, 59, 60, CoordinatesPosition.S); pass = false; } catch { }
                try { cp = new CoordinatePart(-90, 0, 0, CoordinatesPosition.S); pass = false; } catch { }
                try { cp = new CoordinatePart(89, -1, 0, CoordinatesPosition.S); pass = false; } catch { }
                try { cp = new CoordinatePart(89, 1, -1, CoordinatesPosition.S); pass = false; } catch { }


                try { cp = new CoordinatePart(181, 0, CoordinatesPosition.E); pass = false; } catch { }
                try { cp = new CoordinatePart(180, 1, CoordinatesPosition.E); pass = false; } catch { }
                try { cp = new CoordinatePart(179, 60, CoordinatesPosition.E); pass = false; } catch { }
                try { cp = new CoordinatePart(181, 0, CoordinatesPosition.E); pass = false; } catch { }
                try { cp = new CoordinatePart(180, 1, CoordinatesPosition.E); pass = false; } catch { }
                try { cp = new CoordinatePart(179, 60, CoordinatesPosition.E); pass = false; } catch { }
                try { cp = new CoordinatePart(-180, 1, CoordinatesPosition.E); pass = false; } catch { }
                try { cp = new CoordinatePart(179, -1, CoordinatesPosition.E); pass = false; } catch { }

                try { cp = new CoordinatePart(181, 0, CoordinatesPosition.W); pass = false; } catch { }
                try { cp = new CoordinatePart(180, 1, CoordinatesPosition.W); pass = false; } catch { }
                try { cp = new CoordinatePart(179, 60, CoordinatesPosition.W); pass = false; } catch { }
                try { cp = new CoordinatePart(181, 0, CoordinatesPosition.W); pass = false; } catch { }
                try { cp = new CoordinatePart(180, 1, CoordinatesPosition.W); pass = false; } catch { }
                try { cp = new CoordinatePart(179, 60, CoordinatesPosition.W); pass = false; } catch { }
                try { cp = new CoordinatePart(-180, 1, CoordinatesPosition.W); pass = false; } catch { }
                try { cp = new CoordinatePart(179, -1, CoordinatesPosition.W); pass = false; } catch { }

                try { cp = new CoordinatePart(181, 0, 0, CoordinatesPosition.E); pass = false; } catch { }
                try { cp = new CoordinatePart(180, 0, 1, CoordinatesPosition.E); pass = false; } catch { }
                try { cp = new CoordinatePart(179, 59, 60, CoordinatesPosition.E); pass = false; } catch { }
                try { cp = new CoordinatePart(180, 0, 1, CoordinatesPosition.E); pass = false; } catch { }
                try { cp = new CoordinatePart(179, 59, 60, CoordinatesPosition.E); pass = false; } catch { }
                try { cp = new CoordinatePart(-180, 0, 0, CoordinatesPosition.E); pass = false; } catch { }
                try { cp = new CoordinatePart(179, -1, 0, CoordinatesPosition.E); pass = false; } catch { }
                try { cp = new CoordinatePart(179, 1, -1, CoordinatesPosition.E); pass = false; } catch { }

                try { cp = new CoordinatePart(181, 0, 0, CoordinatesPosition.W); pass = false; } catch { }
                try { cp = new CoordinatePart(180, 0, 1, CoordinatesPosition.W); pass = false; } catch { }
                try { cp = new CoordinatePart(179, 59, 60, CoordinatesPosition.W); pass = false; } catch { }
                try { cp = new CoordinatePart(180, 0, 1, CoordinatesPosition.W); pass = false; } catch { }
                try { cp = new CoordinatePart(179, 59, 60, CoordinatesPosition.W); pass = false; } catch { }
                try { cp = new CoordinatePart(-180, 0, 0, CoordinatesPosition.W); pass = false; } catch { }
                try { cp = new CoordinatePart(179, -1, 0, CoordinatesPosition.W); pass = false; } catch { }
                try { cp = new CoordinatePart(179, 1, -1, CoordinatesPosition.W); pass = false; } catch { }
            }
            catch
            {
                pass = false;
            }
            Write_Pass("CoordinatePart Initialization Range Checks", pass);

            //TEST NEW/DEPRACTED COORDPART CONTRUCTORS
            pass = true;
            try
            {
               
                Coordinate coord = new Coordinate();

                double lat = coord.Latitude.DecimalDegree;
                double lng = coord.Longitude.DecimalDegree;
                string MGRS = coord.MGRS.ToString();
                string UTM = coord.UTM.ToString();
                string ECEF = coord.ECEF.ToString();
                string Cartesian = coord.Cartesian.ToString();

                CoordinatePart cpLat = new CoordinatePart(25, CoordinateType.Lat);
                CoordinatePart cpLng = new CoordinatePart(25, CoordinateType.Long);

                //PROP CHANGE ERROR CHECK
              
                cpLat.DecimalDegree = 27;
                cpLng.Seconds = 24;           

                coord.Latitude = cpLat;
                if (coord.Latitude.ToDouble() == lat) { pass = false; }
                coord.Longitude = cpLng;
                if (coord.Longitude.ToDouble() == lng) { pass = false; }
                if (MGRS == coord.MGRS.ToString()) { pass = false; }
                if (UTM == coord.UTM.ToString()) { pass = false; }
                if (ECEF == coord.ECEF.ToString()) { pass = false; }
                if (Cartesian == coord.Cartesian.ToString()) { pass = false; }
            }
            catch{ pass = false; }

            Write_Pass("CoordinatePart Constructor Property Change Test: ", pass);         
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

            for (int x = 0; x < Conversions.Count; x++)
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
                //7 = ECEF
                Coordinate c = new Coordinate(lat, lng);
                bool pass = true;
                Coordinate rc = new Coordinate();
                for (int y = 0; y < 8; y++)
                {

                    switch (y)
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
                            rc.Latitude = new CoordinatePart(c.Latitude.Degrees, c.Latitude.DecimalMinute, c.Latitude.Position);
                            rc.Longitude = new CoordinatePart(c.Longitude.Degrees, c.Longitude.DecimalMinute, c.Longitude.Position);
                            if (rc.Latitude.ToDouble() != c.Latitude.ToDouble()) { pass = false; Debug.WriteLine("...Conversion Outside Limits: " + rc.Latitude.ToDouble() + " - " + c.Latitude.ToDouble()); }
                            if (rc.Longitude.ToDouble() != c.Longitude.ToDouble()) { pass = false; Debug.WriteLine("...Conversion Outside Limits: " + rc.Longitude.ToDouble() + " - " + c.Longitude.ToDouble()); }
                            break;
                        case 3:
                            c.FormatOptions.Format = CoordinateFormatType.Degree_Minutes_Seconds;
                            if (c.ToString() != coordList[y]) { pass = false; }
                            rc = new Coordinate();
                            rc.Latitude = new CoordinatePart(c.Latitude.Degrees, c.Latitude.Minutes, c.Latitude.Seconds, c.Latitude.Position);
                            rc.Longitude = new CoordinatePart(c.Longitude.Degrees, c.Longitude.Minutes, c.Longitude.Seconds, c.Longitude.Position);
                            if (rc.Latitude.ToDouble() != c.Latitude.ToDouble()) { pass = false; Debug.WriteLine("...Conversion Outside Limits: " + rc.Latitude.ToDouble() + " - " + c.Latitude.ToDouble()); }
                            if (rc.Longitude.ToDouble() != c.Longitude.ToDouble()) { pass = false; Debug.WriteLine("...Conversion Outside Limits: " + rc.Longitude.ToDouble() + " - " + c.Longitude.ToDouble()); }
                            break;
                        case 4:
                            if (c.UTM.ToString() != coordList[y] && c.UTM.WithinCoordinateSystemBounds) { pass = false; }
                            UniversalTransverseMercator utm = new UniversalTransverseMercator(c.UTM.LatZone, c.UTM.LongZone, c.UTM.Easting, c.UTM.Northing);
                            rc = UniversalTransverseMercator.ConvertUTMtoLatLong(utm);
                            if (Math.Abs(rc.Latitude.ToDouble() - c.Latitude.ToDouble()) >= .00001 && c.UTM.WithinCoordinateSystemBounds) { pass = false; Debug.WriteLine("...UTM Conversion Outside Limits: " + rc.Latitude.ToDouble() + " - " + c.Latitude.ToDouble()); }
                            if (Math.Abs(rc.Longitude.ToDouble() - c.Longitude.ToDouble()) >= .00001 && c.UTM.WithinCoordinateSystemBounds) { pass = false; Debug.WriteLine("...UTM Conversion Outside Limits: " + rc.Longitude.ToDouble() + " - " + c.Longitude.ToDouble()); }
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
                        case 7:
                            string ec = c.ECEF.ToString().Replace(" km", "").Replace(",", "");
                            if (ec != coordList[y]) { pass = false; }
                            ECEF ecef = new ECEF(c.ECEF.X, c.ECEF.Y, c.ECEF.Z);
                            rc = ECEF.ECEFToLatLong(ecef);
                            if (Math.Abs(rc.Latitude.ToDouble() - c.Latitude.ToDouble()) >= .00001) { pass = false; Debug.WriteLine("...ECEF Conversion Outside Limits: " + rc.Latitude.ToDouble() + " - " + c.Latitude.ToDouble()); }
                            if (Math.Abs(rc.Longitude.ToDouble() - c.Longitude.ToDouble()) >= .00001) { pass = false; Debug.WriteLine("...ECEF Conversion Outside Limits: " + rc.Longitude.ToDouble() + " - " + c.Longitude.ToDouble()); }
                            if (Math.Abs(rc.ECEF.GeoDetic_Height.Meters - c.ECEF.GeoDetic_Height.Meters) >= .00001) { pass = false; Debug.WriteLine("...ECEF Conversion Outside Limits: " + rc.Longitude.ToDouble() + " - " + c.Longitude.ToDouble()); }
                            break;
                        default:
                            break;
                    }
                }
                Write_Pass("Conversion Pass " + ((int)(x + 1)).ToString() + ": ", pass);
            }
            //ECEF WITH HEIGHT CHECK
            Console.WriteLine();
            bool passE = true;
            double latE = -80.6586;
            double longE = -152.49;            
            Distance h = new Distance(1500, DistanceType.Meters);
            Coordinate cE = new Coordinate(latE, longE);
            cE.ECEF.Set_GeoDetic_Height(cE, h);
            if (Math.Abs(cE.ECEF.X - -921.443) >= .001) { passE = false; Debug.WriteLine("...Setting GeoDetic Height Conversions Outside Limits"); }
            if (Math.Abs(cE.ECEF.Y - -479.878) >= .001) { passE = false; Debug.WriteLine("...Setting GeoDetic Height Conversions Outside Limits"); }
            if (Math.Abs(cE.ECEF.Z - -6273.377) >= .001) { passE = false; Debug.WriteLine("...Setting GeoDetic Height Conversions Outside Limits"); }

            ECEF ecefE = new ECEF(cE.ECEF.X, cE.ECEF.Y, cE.ECEF.Z);
            Coordinate rcE = ECEF.ECEFToLatLong(ecefE);           
            if (Math.Abs(rcE.Latitude.ToDouble() - cE.Latitude.ToDouble()) >= .00001) { passE = false; Debug.WriteLine("...ECEF Conversion Outside Limits: " + rcE.Latitude.ToDouble() + " - " + cE.Latitude.ToDouble()); }
            if (Math.Abs(rcE.Longitude.ToDouble() - cE.Longitude.ToDouble()) >= .00001) { passE = false; Debug.WriteLine("...ECEF Conversion Outside Limits: " + rcE.Longitude.ToDouble() + " - " + cE.Longitude.ToDouble()); }
            if (Math.Abs(rcE.ECEF.GeoDetic_Height.Meters - cE.ECEF.GeoDetic_Height.Meters) >= .00001) { passE = false; Debug.WriteLine("...ECEF Conversion Outside Limits: " + rcE.Longitude.ToDouble() + " - " + cE.Longitude.ToDouble()); }

            ecefE = new ECEF(cE,cE.ECEF.GeoDetic_Height);
            
            rcE = ECEF.ECEFToLatLong(ecefE);
            if (Math.Abs(rcE.Latitude.ToDouble() - cE.Latitude.ToDouble()) >= .00001) { passE = false; Debug.WriteLine("...ECEF Conversion Outside Limits: " + rcE.Latitude.ToDouble() + " - " + cE.Latitude.ToDouble()); }
            if (Math.Abs(rcE.Longitude.ToDouble() - cE.Longitude.ToDouble()) >= .00001) { passE = false; Debug.WriteLine("...ECEF Conversion Outside Limits: " + rcE.Longitude.ToDouble() + " - " + cE.Longitude.ToDouble()); }
            if (Math.Abs(rcE.ECEF.GeoDetic_Height.Meters - cE.ECEF.GeoDetic_Height.Meters) >= .00001) { passE = false; Debug.WriteLine("...ECEF Conversion Outside Limits: " + rcE.ECEF.GeoDetic_Height.Meters + " - " + cE.ECEF.GeoDetic_Height.Meters); }

            Write_Pass("ECEF WITH HEIGHT CHECK", passE);

            //UTM MGRS BOUNDARY CHECK
            bool p = true;
            Coordinate cr = new Coordinate(-79.99, 0);
            if(!cr.UTM.WithinCoordinateSystemBounds || !cr.MGRS.WithinCoordinateSystemBounds) { p = false; }
            cr.Latitude.DecimalDegree = -80;
            if (cr.UTM.WithinCoordinateSystemBounds || cr.MGRS.WithinCoordinateSystemBounds) { p = false; }
            cr.Latitude.DecimalDegree = 83.99;
            if (!cr.UTM.WithinCoordinateSystemBounds || !cr.MGRS.WithinCoordinateSystemBounds) { p = false; }
            cr.Latitude.DecimalDegree = 84;
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

            pass = true;
            Coordinate.TryParse("5242.118   km 2444.44  km 2679.085   km", CartesianType.ECEF, out coordinate);
            if (Math.Abs(coordinate.Latitude.DecimalDegree - 25) > .001) { pass = false; }
            if (Math.Abs(coordinate.Longitude.DecimalDegree - 25) > .001) { pass = false; }          
            if (Math.Abs(coordinate.ECEF.GeoDetic_Height.Meters - 25) > 1) { pass = false; }
      
            Coordinate.TryParse("-96.867, -1107.196 , 6261.02   km", new DateTime(2019,1,1), CartesianType.ECEF, out coordinate);
            if (Math.Abs(coordinate.Latitude.DecimalDegree - 80) > .001) { pass = false; }
            if (Math.Abs(coordinate.Longitude.DecimalDegree - -95) > .001) { pass = false; }
            if (Math.Abs(coordinate.ECEF.GeoDetic_Height.Meters - 1500) > 1) { pass = false; }
            Console.WriteLine();
            Write_Pass("ECEF PARSE OPTION", pass);

            //PARSE TYPE ENUM TEST
            pass = true;
            coordinate = new Coordinate(25, 25);
            if (coordinate.Parse_Format != Parse_Format_Type.None) { pass = false; }
            Coordinate.TryParse("25,25", out coordinate);
            if(coordinate.Parse_Format != Parse_Format_Type.Signed_Degree) { pass = false; }
            Coordinate.TryParse("N 25º E 25º", out coordinate);
            if (coordinate.Parse_Format != Parse_Format_Type.Decimal_Degree) { pass = false; }
            Coordinate.TryParse("N 25º 0' E 25º 0'", out coordinate);
            if (coordinate.Parse_Format != Parse_Format_Type.Degree_Decimal_Minute) { pass = false; }
            Coordinate.TryParse("N 25º 0' 0\" E 25º 0' 0\"", out coordinate);
            if (coordinate.Parse_Format != Parse_Format_Type.Degree_Minute_Second) { pass = false; }
            Coordinate.TryParse("35R 298154mE 2766437mN", out coordinate);
            if (coordinate.Parse_Format != Parse_Format_Type.UTM) { pass = false; }
            Coordinate.TryParse("35R KH 98154 66437", out coordinate);
            if (coordinate.Parse_Format != Parse_Format_Type.MGRS) { pass = false; }
            Coordinate.TryParse("0.8213938 0.38302222 0.42261826", CartesianType.Cartesian, out coordinate);
            if (coordinate.Parse_Format != Parse_Format_Type.Cartesian_Spherical) { pass = false; }
            Coordinate.TryParse("5242.097 km, 2444.43 km, 2679.074 km", CartesianType.ECEF, out coordinate);
            if (coordinate.Parse_Format != Parse_Format_Type.Cartesian_ECEF) { pass = false; }

            Console.WriteLine();
            Write_Pass("PARSE FORMAT ENUM", pass);
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
            Console.WriteLine();
            Console.WriteLine("***Running IsSunUp Test (This will take a minute)****");
            Console.WriteLine();
            Write_Pass("IsSunUp", ct.Check_IsSunUp());
            Console.WriteLine();
            Console.WriteLine("***Running IsMoonUp Test (This will take a minute)****");
            Console.WriteLine();
            Write_Pass("IsMoonUp", ct.Check_IsMoonUp());
            Console.WriteLine();
        }

        #region Distance Tests
   
        private static void Run_Distance_Test()
        {
            Distance_Init_Tests();
            Coordinate_Move_Test();
            Distance_Value_Tests();
        }
        private static void Distance_Init_Tests()
        {
            //Conversions should be equal to these numbers within .0001 tolerance

            double m = 1000; //meters
            double km = 1; //Kilometers
            double ft = 3280.84; //Feet
            double sm = 0.6213712; //Nautical Miles
            double nm = 0.5399565; //Statute Miles

            double[] distances = new double[] { m, km, ft, nm, sm };


            Distance d = new Distance(km);

            Pass.Write("Distance(double km)", Check_Distance(d, distances));
            d = new Distance(distances[0], DistanceType.Meters);
            Console.WriteLine();
            Pass.Write("Distance(double distance, DistanceType Meters)", Check_Distance(d, distances));
            d = new Distance(distances[1], DistanceType.Kilometers);
            Pass.Write("Distance(double distance, DistanceType Kilometers)", Check_Distance(d, distances));
            d = new Distance(distances[2], DistanceType.Feet);
            Pass.Write("Distance(double distance, DistanceType Feet)", Check_Distance(d, distances));
            d = new Distance(distances[3], DistanceType.NauticalMiles);
            Pass.Write("Distance(double distance, DistanceType Nautical Miles)", Check_Distance(d, distances));
            d = new Distance(distances[4], DistanceType.Miles);
            Pass.Write("Distance(double distance, DistanceType Statute Miles)", Check_Distance(d, distances));
            Console.WriteLine();

            //KILOMETERS Between specified points above should be as follows in defined tolerance .000001
            double kmSphere = 412.0367538058125;
            double kmWGS84 = 412.1977393206501; //Default datum WGS84

            Coordinate c1 = new Coordinate(45, 72);
            Coordinate c2 = new Coordinate(42, 75);

            d = new Distance(c1, c2);

            if (Math.Abs(d.Kilometers - kmSphere) > .000001)
            {
                Pass.Write("Distance(Coordinate c1, Coordinate c2)", false);
                Debug.WriteLine("...Mismatch: " + d.Kilometers + " - " + kmSphere);
            }
            else { Pass.Write("Distance(Coordinate c1, Coordinate c2)", true); }
            d = new Distance(c1, c2, Shape.Sphere);
            if (Math.Abs(d.Kilometers - kmSphere) > .000001)
            {
                Pass.Write("Distance(Coordinate c1, Coordinate c2, Shape.Sphere)", false);
                Debug.WriteLine("...Mismatch: " + d.Kilometers + " - " + kmSphere);
            }
            else { Pass.Write("Distance(Coordinate c1, Coordinate c2, Shape.Sphere)", true); }
            d = new Distance(c1, c2, Shape.Ellipsoid);
            if (Math.Abs(d.Kilometers - kmWGS84) > .000001)
            {
                Pass.Write("Distance(Coordinate c1, Coordinate c2, Shape.Ellipsoid)", false);
                Debug.WriteLine("...Mismatch: " + d.Kilometers + " - " + kmWGS84);
            }
            else { Pass.Write("Distance(Coordinate c1, Coordinate c2, Shape.Ellipsoid)", true); }
        }
        private static void Coordinate_Move_Test()
        {
            bool pass = true;
            Console.WriteLine();
            string[] lines = System.IO.File.ReadAllLines(@"CoordinateData\MoveCoords.txt");
            int line = 1;
            //TEST MOVE TOWARDS TARGET LOGIC
            foreach (string s in lines)
            {
                Shape shape;
                string[] data = s.Split(',');
                if (data[5] == "S") { shape = Shape.Sphere; }
                else { shape = Shape.Ellipsoid; }
                double lat1 = double.Parse(data[0]);
                double long1 = double.Parse(data[1]);
                double lat2 = double.Parse(data[2]);
                double long2 = double.Parse(data[3]);
                double dist = double.Parse(data[4]);
                double bearing = double.Parse(data[6]);
                Coordinate coord = new Coordinate(lat1, long1);
                Coordinate target = new Coordinate(lat2, long2);
                coord.FormatOptions.Format = CoordinateFormatType.Decimal;

                //MOVE TO TARGET
                //Method 1
                coord.Move(target, new Distance(dist), shape);
                coord.FormatOptions.Format = CoordinateFormatType.Decimal;
                //Console.WriteLine(coord);
                if (Math.Abs(coord.Latitude.ToDouble() - target.Latitude.ToDouble()) > .0001) { pass = false; Debug.WriteLine("Coordinate Move Test Failed On Iteration " + line); break; }
                if (Math.Abs(coord.Longitude.ToDouble() - target.Longitude.ToDouble()) > .0001) { pass = false; Debug.WriteLine("Coordinate Move Test Failed On Iteration " + line); break; }

                //Method 2
                coord = new Coordinate(lat1, long1);
                coord.Move(target, dist * 1000, shape);
                if (Math.Abs(coord.Latitude.ToDouble() - target.Latitude.ToDouble()) > .0001) { pass = false; Debug.WriteLine("Coordinate Move Test Failed On Iteration " + line); break; }
                if (Math.Abs(coord.Longitude.ToDouble() - target.Longitude.ToDouble()) > .0001) { pass = false; Debug.WriteLine("Coordinate Move Test Failed On Iteration " + line); break; }

                //MOVE TOWARD BEARING
                coord = new Coordinate(lat1, long1);
                coord.Move(new Distance(dist), bearing, shape);
                if (Math.Abs(coord.Latitude.ToDouble() - target.Latitude.ToDouble()) > .0001) { pass = false; Debug.WriteLine("Coordinate Move Test Failed On Iteration " + line); break; }
                if (Math.Abs(coord.Longitude.ToDouble() - target.Longitude.ToDouble()) > .0001) { pass = false; Debug.WriteLine("Coordinate Move Test Failed On Iteration " + line); break; }

                coord = new Coordinate(lat1, long1);
                coord.Move(dist * 1000, bearing, shape);
                if (Math.Abs(coord.Latitude.ToDouble() - target.Latitude.ToDouble()) > .0001) { pass = false; Debug.WriteLine("Coordinate Move Test Failed On Iteration " + line); break; }
                if (Math.Abs(coord.Longitude.ToDouble() - target.Longitude.ToDouble()) > .0001) { pass = false; Debug.WriteLine("Coordinate Move Test Failed On Iteration " + line); break; }

                line++;
            }



            Pass.Write("Coordinate Move Tests ", pass);


        }
        private static bool Check_Distance(Distance d, double[] distances)
        {
            bool pass = true;
            //Round to avoid float point issues
            double m = d.Meters;
            double km = d.Kilometers;
            double ft = d.Feet;
            double sm = d.Miles;
            double nm = d.NauticalMiles;
            if (Math.Abs(m - distances[0]) > .0001) { Debug.WriteLine("...METERS MISMATCH: " + d.Meters + " - " + distances[0]); return false; }
            if (Math.Abs(km - distances[1]) > .0001) { Debug.WriteLine("...KILOMETERS MISMATCH: " + d.Kilometers + " - " + distances[1]); return false; }
            if (Math.Abs(ft - distances[2]) > .0001) { Debug.WriteLine("...FEET MISMATCH: " + d.Feet + " - " + distances[2]); return false; }
            if (Math.Abs(nm - distances[3]) > .0001) { Debug.WriteLine("...NAUTICAL MILES MISMATCH: " + d.NauticalMiles + " - " + distances[3]); return false; }
            if (Math.Abs(sm - distances[4]) > .001) { Debug.WriteLine("...STATUTE MILE MISMATCH: " + d.Miles + " - " + distances[4]); return false; }
            return pass;

        }
        private static void Distance_Value_Tests()
        {
            Coordinate c1;
            Coordinate c2;
            double distanceBuf = .0000001; //Fault tolerance for distance variations
            double bearingBuf = .0000001; //Fault tolerance for bearing variations
            Distance d;
            double[] check;
            bool pass = true;

            //COMAPRISON VALUES PULLED FROM ED WILLIAMS GREAT CIRCLE CALCULATOR 
            //http://edwilliams.org/gccalc.htm

            /* ELLIPSOID CHECKS */
            //Check 1
            c1 = new Coordinate(45.02258, 7.63489);
            c2 = new Coordinate(45.02092, 7.6332);
            d = new Distance(c1, c2, Shape.Ellipsoid);
            check = new double[] { 215.83122136519, 0.22754143255301168 };
            if (Math.Abs(d.Bearing - check[0]) > bearingBuf) { pass = false; }
            if (Math.Abs(d.Kilometers - check[1]) > distanceBuf) { pass = false; }
            //Check 2
            c1 = new Coordinate(45.02258, -7.63489);
            c2 = new Coordinate(45.02092, -7.6332);
            d = new Distance(c1, c2, Shape.Ellipsoid);
            check = new double[] { 144.16877863481, 0.22754143255301168 };
            if (Math.Abs(d.Bearing - check[0]) > bearingBuf) { pass = false; }
            if (Math.Abs(d.Kilometers - check[1]) > distanceBuf) { pass = false; }
            //Check 3
            c1 = new Coordinate(-45.02258, -7.63489);
            c2 = new Coordinate(-45.02092, -7.6332);
            d = new Distance(c1, c2, Shape.Ellipsoid);
            check = new double[] { 35.83122136518998, 0.22754143255301168 };
            if (Math.Abs(d.Bearing - check[0]) > bearingBuf) { pass = false; }
            if (Math.Abs(d.Kilometers - check[1]) > distanceBuf) { pass = false; }
            //Check 4
            c1 = new Coordinate(-45.02258, 7.63489);
            c2 = new Coordinate(-45.02092, 7.6332);
            d = new Distance(c1, c2, Shape.Ellipsoid);
            check = new double[] { 324.16877863481, 0.22754143255301168 };
            if (Math.Abs(d.Bearing - check[0]) > bearingBuf) { pass = false; }
            if (Math.Abs(d.Kilometers - check[1]) > distanceBuf) { pass = false; }
            //Check 5
            c1 = new Coordinate(-45.02258, -7.63489);
            c2 = new Coordinate(45.02092, 7.6332);
            d = new Distance(c1, c2, Shape.Ellipsoid);
            check = new double[] { 10.7750299, 10087.874457727042 };
            if (Math.Abs(d.Bearing - check[0]) > bearingBuf) { pass = false; }
            if (Math.Abs(d.Kilometers - check[1]) > distanceBuf) { pass = false; }
            //Check 6
            c1 = new Coordinate(-76.02258, -120.63489);
            c2 = new Coordinate(12.2569, 7.6332);
            d = new Distance(c1, c2, Shape.Ellipsoid);
            check = new double[] { 125.0102282873087, 12292.331977781124 };
            if (Math.Abs(d.Bearing - check[0]) > bearingBuf) { pass = false; }
            if (Math.Abs(d.Kilometers - check[1]) > distanceBuf) { pass = false; }
            //Check 7
            c1 = new Coordinate(7.689, 91.6998);
            c2 = new Coordinate(8.656, 90.658);
            d = new Distance(c1, c2, Shape.Ellipsoid);
            check = new double[] { 313.0440365804527, 156.8980064612199 };
            if (Math.Abs(d.Bearing - check[0]) > bearingBuf) { pass = false; }
            if (Math.Abs(d.Kilometers - check[1]) > distanceBuf) { pass = false; }
            //Check 8
            c1 = new Coordinate(-7.689, 91.6998);
            c2 = new Coordinate(-8.656, 90.658);
            d = new Distance(c1, c2, Shape.Ellipsoid);
            check = new double[] { 226.9559634195473, 156.8980064612199 };
            if (Math.Abs(d.Bearing - check[0]) > bearingBuf) { pass = false; }
            if (Math.Abs(d.Kilometers - check[1]) > distanceBuf) { pass = false; }
            //Check 9
            c1 = new Coordinate(-25.6965, -91.6998);
            c2 = new Coordinate(-22.3656, -90.658);
            d = new Distance(c1, c2, Shape.Ellipsoid);
            check = new double[] { 16.242229103528945, 383.8404829840529 };
            if (Math.Abs(d.Bearing - check[0]) > bearingBuf) { pass = false; }
            if (Math.Abs(d.Kilometers - check[1]) > distanceBuf) { pass = false; }
            //Check 10
            c1 = new Coordinate(25.6965, -91.6998);
            c2 = new Coordinate(22.3656, -90.658);
            d = new Distance(c1, c2, Shape.Ellipsoid);
            check = new double[] { 163.75777089647104, 383.8404829840529 };
            if (Math.Abs(d.Bearing - check[0]) > bearingBuf) { pass = false; }
            if (Math.Abs(d.Kilometers - check[1]) > distanceBuf) { pass = false; }

            Pass.Write("Coordinate Distance / Bearing Value (ELLIPSE) Tests", pass);

            //DISTANCE VALUES COMPARED https://www.movable-type.co.uk/scripts/latlong.html
            distanceBuf = .0001; //Fault tolerance for distance variations
            bearingBuf = .0001; //Fault tolerance for bearing variations
            pass = true;
            /* SPHERE CHECKS */
            //Check 1
            c1 = new Coordinate(45.02258, 7.63489);
            c2 = new Coordinate(45.02092, 7.6332);
            d = new Distance(c1, c2, Shape.Sphere);
            check = new double[] { 215.73985977231427, 0.2274 };
            if (Math.Abs(d.Bearing - check[0]) > bearingBuf) { pass = false; }
            if (Math.Abs(d.Kilometers - check[1]) > distanceBuf) { pass = false; }
            //Check 2
            c1 = new Coordinate(45.02258, -7.63489);
            c2 = new Coordinate(45.02092, -7.6332);
            d = new Distance(c1, c2, Shape.Sphere);
            check = new double[] { 144.26014022768572, 0.2274 };
            if (Math.Abs(d.Bearing - check[0]) > bearingBuf) { pass = false; }
            if (Math.Abs(d.Kilometers - check[1]) > distanceBuf) { pass = false; }
            //Check 3
            c1 = new Coordinate(-45.02258, -7.63489);
            c2 = new Coordinate(-45.02092, -7.6332);
            d = new Distance(c1, c2, Shape.Sphere);
            check = new double[] { 35.73985977231427, 0.2274 };
            if (Math.Abs(d.Bearing - check[0]) > bearingBuf) { pass = false; }
            if (Math.Abs(d.Kilometers - check[1]) > distanceBuf) { pass = false; }
            //Check 4
            c1 = new Coordinate(-45.02258, 7.63489);
            c2 = new Coordinate(-45.02092, 7.6332);
            d = new Distance(c1, c2, Shape.Sphere);
            check = new double[] { 324.2601402276857, 0.2274 };
            if (Math.Abs(d.Bearing - check[0]) > bearingBuf) { pass = false; }
            if (Math.Abs(d.Kilometers - check[1]) > distanceBuf) { pass = false; }
            //Check 5
            c1 = new Coordinate(-45.02258, -7.63489);
            c2 = new Coordinate(45.02092, 7.6332);
            d = new Distance(c1, c2, Shape.Sphere);
            check = new double[] { 10.72935562256683, 10124.7363 };
            if (Math.Abs(d.Bearing - check[0]) > bearingBuf) { pass = false; }
            if (Math.Abs(d.Kilometers - check[1]) > distanceBuf) { pass = false; }
            //Check 6
            c1 = new Coordinate(-76.02258, -120.63489);
            c2 = new Coordinate(12.2569, 7.6332);
            d = new Distance(c1, c2, Shape.Sphere);
            check = new double[] { 124.94031333870444, 12300.5645 };
            if (Math.Abs(d.Bearing - check[0]) > bearingBuf) { pass = false; }
            if (Math.Abs(d.Kilometers - check[1]) > distanceBuf) { pass = false; }
            //Check 7
            c1 = new Coordinate(7.689, 91.6998);
            c2 = new Coordinate(8.656, 90.658);
            d = new Distance(c1, c2, Shape.Sphere);
            check = new double[] { 313.2321507309865, 157.1934 };
            if (Math.Abs(d.Bearing - check[0]) > bearingBuf) { pass = false; }
            if (Math.Abs(d.Kilometers - check[1]) > distanceBuf) { pass = false; }
            //Check 8
            c1 = new Coordinate(-7.689, 91.6998);
            c2 = new Coordinate(-8.656, 90.658);
            d = new Distance(c1, c2, Shape.Sphere);
            check = new double[] { 226.76784926901342, 157.1934 };
            if (Math.Abs(d.Bearing - check[0]) > bearingBuf) { pass = false; }
            if (Math.Abs(d.Kilometers - check[1]) > distanceBuf) { pass = false; }
            //Check 9
            c1 = new Coordinate(-25.6965, -91.6998);
            c2 = new Coordinate(-22.3656, -90.658);
            d = new Distance(c1, c2, Shape.Sphere);
            check = new double[] { 16.157241979509656, 385.1879 };
            if (Math.Abs(d.Bearing - check[0]) > bearingBuf) { pass = false; }
            if (Math.Abs(d.Kilometers - check[1]) > distanceBuf) { pass = false; }
            //Check 10
            c1 = new Coordinate(25.6965, -91.6998);
            c2 = new Coordinate(22.3656, -90.658);
            d = new Distance(c1, c2, Shape.Sphere);
            check = new double[] { 163.84275802049035, 385.1879 };
            if (Math.Abs(d.Bearing - check[0]) > bearingBuf) { pass = false; }
            if (Math.Abs(d.Kilometers - check[1]) > distanceBuf) { pass = false; }

            Pass.Write("Coordinate Distance / Bearing Value (SPHERE) Tests", pass);
            //Console.WriteLine(d.Kilometers);

            //Console.WriteLine(d.Bearing);

            //c1.Move(new Distance(.2274), 144, Shape.Ellipsoid);
            //c1.FormatOptions.Format = CoordinateFormatType.Degree_Decimal_Minutes;
            //c1.FormatOptions.Round = 4;
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
                tc.Latitude = new CoordinatePart(39, 45, 34, CoordinatesPosition.N);
                tc.Longitude = new CoordinatePart(74, 34, 45, CoordinatesPosition.W);
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
            if (c.ECEF != null) { pass = false; }
            Write_Pass("Null Properties (Celestial, UTM, MGRS, Cartesian, ECEF)", pass);

            //Check Load_Calls
            pass = true;
            c.LoadCelestialInfo();
            if (c.CelestialInfo == null) { pass = false; }
            c.LoadUTM_MGRS_Info();
            if (c.UTM == null) { pass = false; }
            if (c.MGRS == null) { pass = false; }
            c.LoadCartesianInfo();
            if (c.Cartesian == null) { pass = false; }
            c.LoadECEFInfo();
            if(c.ECEF == null) { pass = false; }
            Write_Pass("Load Calls (Celestial, UTM, MGRS, Cartesian, ECEF)", pass);

            if (pass)
            {
                Celestial cel = c.CelestialInfo;
                MilitaryGridReferenceSystem mgrs = c.MGRS;
                UniversalTransverseMercator utm = c.UTM;
                Cartesian cart = c.Cartesian;
                ECEF ecef = c.ECEF;

                c.Latitude.DecimalDegree = -45;
                c.Longitude.DecimalDegree = -75;

                //Properties should not change.
                if(!ReflectiveEquals(c.CelestialInfo, cel)){ pass = false; }
                if (!ReflectiveEquals(c.MGRS, mgrs)){ pass = false; }
                if (!ReflectiveEquals(c.UTM, utm)){ pass = false; }
                if (!ReflectiveEquals(c.Cartesian, cart)){ pass = false; }
                if (!ReflectiveEquals(c.ECEF, ecef)) { pass = false; }
                //Properties should remain equal as no load calls were made
                Write_Pass("Property State Hold (Celestial, UTM, MGRS, Cartesian, ECEF)", pass);

                //Properties should change
                pass = true;
                c.LoadCelestialInfo();
                c.LoadCartesianInfo();
                c.LoadUTM_MGRS_Info();
                c.LoadECEFInfo();
                if (ReflectiveEquals(c.CelestialInfo, cel)) { pass = false; }
                if (ReflectiveEquals(c.MGRS, mgrs)) { pass = false; }
                if (ReflectiveEquals(c.UTM, utm)) { pass = false; }
                if (ReflectiveEquals(c.Cartesian, cart)) { pass = false; }
                if (ReflectiveEquals(c.ECEF, ecef)) { pass = false; }
                //Properties should not be equal as chages have been made
                Write_Pass("Property State Change (Celestial, UTM, MGRS, Cartesian, ECEF)", pass);

            }
            else
            {
                //Passes auto fail has properties didn't load when called.

                Write_Pass("Property State Hold (Celestial, UTM, MGRS, Cartesian, ECEF)", false);
                Write_Pass("Property State Change (Celestial, UTM, MGRS, Cartesian, ECEF)", false);

            }

            //EagerLoaded Flags Test
          
            EagerLoad eg = new EagerLoad(EagerLoadType.Cartesian | EagerLoadType.Celestial | EagerLoadType.UTM_MGRS | EagerLoadType.ECEF);
            pass = true;
            if(eg.Cartesian==false || eg.Celestial==false || eg.UTM_MGRS == false || eg.ECEF == false) { pass = false; }
            eg = new EagerLoad(EagerLoadType.Celestial);
            if (eg.Cartesian == true || eg.Celestial == false || eg.UTM_MGRS == true || eg.ECEF == true) { pass = false; }
            eg = new EagerLoad(EagerLoadType.Cartesian);
            if (eg.Cartesian == false|| eg.Celestial == true || eg.UTM_MGRS == true || eg.ECEF == true) { pass = false; }
            eg = new EagerLoad(EagerLoadType.UTM_MGRS);
            if (eg.Cartesian == true|| eg.Celestial == true || eg.UTM_MGRS == false || eg.ECEF == true) { pass = false; }
            eg = new EagerLoad(EagerLoadType.ECEF);
            if (eg.Cartesian == true || eg.Celestial == true || eg.UTM_MGRS == true || eg.ECEF == false) { pass = false; }

            eg = new EagerLoad(EagerLoadType.UTM_MGRS | EagerLoadType.Celestial);
            if (eg.Cartesian == true || eg.Celestial == false|| eg.UTM_MGRS == false || eg.ECEF==true) { pass = false; }
            eg = new EagerLoad(EagerLoadType.Cartesian | EagerLoadType.Celestial);
            if (eg.Cartesian == false || eg.Celestial == false || eg.UTM_MGRS == true || eg.ECEF == true) { pass = false; }
            eg = new EagerLoad(EagerLoadType.UTM_MGRS | EagerLoadType.Cartesian);
            if (eg.Cartesian == false|| eg.Celestial == true|| eg.UTM_MGRS == false || eg.ECEF == true) { pass = false; }
            eg = new EagerLoad(EagerLoadType.ECEF | EagerLoadType.Celestial);
            if (eg.Cartesian == true || eg.Celestial == false || eg.UTM_MGRS == true || eg.ECEF == false) { pass = false; }
            eg = new EagerLoad(EagerLoadType.ECEF | EagerLoadType.Cartesian);
            if (eg.Cartesian == false || eg.Celestial == true || eg.UTM_MGRS == true || eg.ECEF == false) { pass = false; }
            eg = new EagerLoad(EagerLoadType.ECEF | EagerLoadType.Cartesian| EagerLoadType.UTM_MGRS);
            if (eg.Cartesian == false|| eg.Celestial == true || eg.UTM_MGRS == false || eg.ECEF == false) { pass = false; }

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
