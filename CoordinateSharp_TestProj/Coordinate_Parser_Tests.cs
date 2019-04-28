using System;
using System.IO;
using CoordinateSharp;
using System.Diagnostics;
namespace CoordinateSharp_TestProj
{
    public class Coordinate_Parser_Tests
    {
        public static void Run_Test()
        {
            Coordinate_Parses_Test();
            CoordinatePart_Parses_Test();
            ECEF_Parse_Options_Test();
            Parse_Type_Enumerator_Test();            
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
                        if (coordinate.ToString() != cc[1])
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
    }
}
