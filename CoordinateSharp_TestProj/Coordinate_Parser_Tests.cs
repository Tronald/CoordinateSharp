using System;
using System.IO;
using CoordinateSharp;
using System.Diagnostics;
using System.Threading;
using System.Globalization;
namespace CoordinateSharp_TestProj
{
    public class Coordinate_Parser_Tests
    {
        public static void Run_Test()
        {
            //RUN IN BOTH ENGLISH AND DUTCH AS PERIODS CHANGE TO COMMAS
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("*****PARSING WITH US CULTURE*****");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Coordinate_Parses_Test();         
            CoordinatePart_Parses_Test();
            Coordinate_Parses_EagerLoad_Test();
            Parse_Wrap_Tests();
            ECEF_Parse_Options_Test();
            Parse_Type_Enumerator_Test();

            Thread.CurrentThread.CurrentCulture = new CultureInfo("nl");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("*****PARSING WITH DUTCH CULTURE*****");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Coordinate_Parses_Test();            
            CoordinatePart_Parses_Test();
            Coordinate_Parses_EagerLoad_Test();
            Parse_Wrap_Tests();
            ECEF_Parse_Options_Test();
            Parse_Type_Enumerator_Test();

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US"); //RESET CULTURE
        }
        private static void Coordinate_Parses_Test()
        {
            Coordinate coordinate;
            //Parse Coordinate Formats
            string[] coordStrings = File.ReadAllLines("CoordinateData\\Coordinates.txt");
            bool pass = true;
            string lastType = "";
            Console.WriteLine("Coordinate Parses...");
            foreach (string c in coordStrings)
            {
                if (c.Contains("\\"))
                {
                    if (lastType != "")
                    {
                        Pass.Write(lastType, pass);
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
                        //CHECK STRING COMPARISON, BUT REPLACE , and . with * to avoid cultural mismatch
                        if (coordinate.ToString().Replace(",","*").Replace(".", "*") != cc[1].Replace(",","*").Replace(".", "*"))
                        {

                            Debug.WriteLine("...MISMATCH: " + coordinate.ToString() + " - " + cc[1]);
                            pass = false;
                        }
                    }
                    
                }
            }
            if (lastType != "")
            {
                Pass.Write(lastType.Split(',')[0], pass);
            }
            //MGRS LESS PRECISE PARSE
            Console.WriteLine();
            pass = true;
            var precise = Coordinate.Parse("16REU6070");
            if(precise.MGRS.ToString() != "16R EU 60000 70000") { pass = false; }
            precise = Coordinate.Parse("ZBE6611");
            if (precise.MGRS.ToString() != "Z BE 65999 11000") { pass = false; Debug.WriteLine(precise.MGRS); }
            Pass.Write("\\MGRS Less Precise Parse", pass);
            //Attempt Forces Param
            pass = true;
            try
            {
                //Parse wraps TryParse, so Parse does not need to be added to tests unless this structure changes.

                if (Coordinate.TryParse("95F, 54", out coordinate)) { pass = false; }//Intentional Fail
                if (Coordinate.TryParse("E 181 30, 56 76", out coordinate)) { pass = false; } //Intentional Fail
                if (Coordinate.TryParse("N 95 45, E 45", out coordinate)) { pass = false; } //Intentional Fail
                if (Coordinate.TryParse("95 87 46 78 D", out coordinate)) { pass = false; } //Intentional Fail
                if (Coordinate.TryParse("W24 45, W45", out coordinate)) { pass = false; } //Intentional Fail
             
            }
            catch { pass = false; }
            Console.WriteLine();
            Pass.Write("\\\\Intentional Fails", pass);
            Console.WriteLine();
           
          
            
        }
        private static void CoordinatePart_Parses_Test()
        {
            //Parse CoordinatePart Formats
            Console.WriteLine("CoordinatePart Parses...");
            string[] coordStrings = File.ReadAllLines("CoordinateData\\CoordinateParts.txt");
            CoordinatePart cp;
            bool pass = true;
            string lastType = "";
            foreach (string c in coordStrings)
            {
                c.Trim();
                if (c.Contains("\\"))
                {
                    if (lastType != "")
                    {
                        Pass.Write(lastType.Split('#')[0], pass);
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
                        if (cp.ToString().Replace(",", "*").Replace(".", "*") != cc[1].Replace(",", "*").Replace(".", "*"))
                        {
                            Debug.WriteLine("...MISMATCH: " + cp.ToString() + " - " + cc[1]);
                            pass = false;
                        }
                    }
                }
            }
            if (lastType != "")
            {
                Pass.Write(lastType.Split(',')[0], pass);
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
            Pass.Write("\\\\Intentional Fails", pass);
        }
        private static void ECEF_Parse_Options_Test()
        {
            Coordinate coordinate;
            bool pass = true;
            Coordinate.TryParse("5242.118   km 2444.44  km 2679.085   km", CartesianType.ECEF, out coordinate);
            if (Math.Abs(coordinate.Latitude.DecimalDegree - 25) > .001) { pass = false; }
            if (Math.Abs(coordinate.Longitude.DecimalDegree - 25) > .001) { pass = false; }
            if (Math.Abs(coordinate.ECEF.GeoDetic_Height.Meters - 25) > 1) { pass = false; }

            Coordinate.TryParse("-96.867, -1107.196 , 6261.02   km", new DateTime(2019, 1, 1), CartesianType.ECEF, out coordinate);
            if (Math.Abs(coordinate.Latitude.DecimalDegree - 80) > .001) { pass = false; }
            if (Math.Abs(coordinate.Longitude.DecimalDegree - -95) > .001) { pass = false; }
            if (Math.Abs(coordinate.ECEF.GeoDetic_Height.Meters - 1500) > 1) { pass = false; }
            Console.WriteLine();
            Pass.Write("ECEF PARSE OPTION", pass);
        }
        private static void Parse_Type_Enumerator_Test()
        {
            bool pass = true;
            Coordinate coordinate = new Coordinate(25, 25);
            if (coordinate.Parse_Format != Parse_Format_Type.None) { pass = false; }
            Coordinate.TryParse("25,25", out coordinate);
            if (coordinate.Parse_Format != Parse_Format_Type.Signed_Degree) { pass = false; }
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
            Pass.Write("PARSE FORMAT ENUM", pass);
        }
        private static void Coordinate_Parses_EagerLoad_Test()
        {
            Coordinate coordinate;
            //Parse Coordinate Formats
            string[] coordStrings = File.ReadAllLines("CoordinateData\\Coordinates.txt");
            bool pass = true;
          
            foreach (string c in coordStrings)
            {
                EagerLoad el = new EagerLoad(false);
                if(!Coordinate.TryParse("12,12", el, out coordinate)){ pass = false; }
                if(coordinate.CelestialInfo != null) { pass = false; } //Simple check to see if eager loading is off.

                el = new EagerLoad(EagerLoadType.Celestial);
                if (!Coordinate.TryParse("12,12", DateTime.Now, el, out coordinate)) { pass = false; }
                if (coordinate.CelestialInfo == null || coordinate.ECEF != null) { pass = false; } //Simple check to see if eager loading is specified

                el = new EagerLoad(EagerLoadType.ECEF);
                if (!Coordinate.TryParse("12,12", CartesianType.ECEF, el, out coordinate)) { pass = false; }
                if (coordinate.ECEF == null || coordinate.CelestialInfo != null) { pass = false; } //Simple check to see if eager loading is off.

                el = new EagerLoad(EagerLoadType.ECEF | EagerLoadType.Celestial);
                if (!Coordinate.TryParse("12,12", DateTime.Now, CartesianType.ECEF, el, out coordinate)) { pass = false; }
                if (coordinate.CelestialInfo == null || coordinate.ECEF == null) { pass = false; } //Simple check to see if eager loading is off.


            }

            Pass.Write("Coordinate Eager Load Parses", pass);
           
            Console.WriteLine();
            
        }
        //Verifies that Parse is wrapping Try_Parse correctly
        private static void Parse_Wrap_Tests()
        {
            bool pass = true;

            string coord = "45.6, 22.4";
            EagerLoad el = new EagerLoad(EagerLoadType.Celestial | EagerLoadType.Cartesian | EagerLoadType.ECEF);
            CartesianType cType = CartesianType.ECEF;
            DateTime geoDate = new DateTime(2020, 3, 10, 10, 10, 12);

            Coordinate parseCoord;
            Coordinate tryParseCoord;

            parseCoord = Coordinate.Parse(coord);
            Coordinate.TryParse(coord, out tryParseCoord);
            if(!Parse_Wrap_Check(parseCoord, tryParseCoord, false)) { pass = false; }

            parseCoord = Coordinate.Parse(coord, geoDate);
            Coordinate.TryParse(coord, geoDate, out tryParseCoord);
            if (!Parse_Wrap_Check(parseCoord, tryParseCoord, false)) { pass = false; }

            parseCoord = Coordinate.Parse(coord, cType);
            Coordinate.TryParse(coord, cType, out tryParseCoord);
            if (!Parse_Wrap_Check(parseCoord, tryParseCoord, false)) { pass = false; }

            parseCoord = Coordinate.Parse(coord, geoDate, cType);
            Coordinate.TryParse(coord, geoDate, cType, out tryParseCoord);
            if (!Parse_Wrap_Check(parseCoord, tryParseCoord, false)) { pass = false; }

            parseCoord = Coordinate.Parse(coord, el);
            Coordinate.TryParse(coord, el, out tryParseCoord);
            if (!Parse_Wrap_Check(parseCoord, tryParseCoord, false)) { pass = false; }

            parseCoord = Coordinate.Parse(coord, geoDate, el);
            Coordinate.TryParse(coord, geoDate, el, out tryParseCoord);
            if (!Parse_Wrap_Check(parseCoord, tryParseCoord, false)) { pass = false; }

            parseCoord = Coordinate.Parse(coord, cType, el);
            Coordinate.TryParse(coord, cType, el, out tryParseCoord);
            if (!Parse_Wrap_Check(parseCoord, tryParseCoord, false)) { pass = false; }

            parseCoord = Coordinate.Parse(coord, geoDate, cType, el);
            Coordinate.TryParse(coord, geoDate, cType, el, out tryParseCoord);
            if (!Parse_Wrap_Check(parseCoord, tryParseCoord, false)) { pass = false; }

            //CoordinatePart Check

            CoordinatePart cp = CoordinatePart.Parse("45");
            if(cp.ToDouble()!= 45 || cp.Position != CoordinatesPosition.N) { pass = false; }

            cp = CoordinatePart.Parse("45", CoordinateType.Long);
            if (cp.ToDouble() != 45 || cp.Position != CoordinatesPosition.E) { pass = false; }

            Pass.Write("Parse Wrapper Test", pass);
        }
        private static bool Parse_Wrap_Check(Coordinate parseCoord, Coordinate tryParseCoord, bool eagerLoadCheck)
        {
            bool pass = true;

            if (parseCoord.Latitude.ToDouble() != tryParseCoord.Latitude.ToDouble()) { pass = false; }
            if (parseCoord.Longitude.ToDouble() != tryParseCoord.Longitude.ToDouble()) { pass = false; }
            if (parseCoord.GeoDate != tryParseCoord.GeoDate) { pass = false; }
            if (parseCoord.EagerLoadSettings != tryParseCoord.EagerLoadSettings) { pass = false; }
            if (parseCoord.Cartesian.X != tryParseCoord.Cartesian.X) { pass = false; }
            if (parseCoord.Cartesian.Y != tryParseCoord.Cartesian.Y) { pass = false; }
            if (parseCoord.Cartesian.Z != tryParseCoord.Cartesian.Z) { pass = false; }
            if (parseCoord.MGRS != null && eagerLoadCheck == true) { pass = false; }
            if (parseCoord.Parse_Format != tryParseCoord.Parse_Format) { pass = false; }

            return pass;
        }
    }
}
