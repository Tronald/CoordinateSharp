using System;
using System.Collections.Generic;
using System.IO;
using CoordinateSharp;
using System.Diagnostics;
namespace CoordinateSharp_TestProj
{
    public class Coordinate_Conversion_Tests
    {
        public static void Run_Test()
        {
            //GATHER CONVERSIONS
            //Conversion lists must end in //** to signify end of list
            List<List<string>> Conversions = new List<List<string>>();
            string[] coordStrings = File.ReadAllLines("CoordinateData\\Conversions.txt");
            List<string> cList = new List<string>();
            foreach (string c in coordStrings)
            {
                if (c == "//**")
                {
                    Conversions.Add(cList);
                    cList = new List<string>();
                }
                else
                {
                    cList.Add(c);
                }
            }

            Run_Conversion_Passes(Conversions);
            Check_ECEF_Height_Conversions();
            Check_UTM_MGRS_Boundaries();      
        }
        private static void Run_Conversion_Passes(List<List<string>> Conversions)
        {
            //List of coordinates to test conversions against.
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

                            //OVERLOAD
                            utm = new UniversalTransverseMercator(c.UTM.LongZone + c.UTM.LatZone.ToString(), c.UTM.Easting, c.UTM.Northing);
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

                            //OVERLOAD
                            mgrs = new MilitaryGridReferenceSystem(c.MGRS.LongZone + c.MGRS.LatZone.ToString(), c.MGRS.Digraph, c.MGRS.Easting, c.MGRS.Northing);
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
                Pass.Write("Conversion Pass " + ((int)(x + 1)).ToString() + ": ", pass);
            }
        }
        private static void Check_ECEF_Height_Conversions()
        {
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

            ecefE = new ECEF(cE, cE.ECEF.GeoDetic_Height);

            rcE = ECEF.ECEFToLatLong(ecefE);
            if (Math.Abs(rcE.Latitude.ToDouble() - cE.Latitude.ToDouble()) >= .00001) { passE = false; Debug.WriteLine("...ECEF Conversion Outside Limits: " + rcE.Latitude.ToDouble() + " - " + cE.Latitude.ToDouble()); }
            if (Math.Abs(rcE.Longitude.ToDouble() - cE.Longitude.ToDouble()) >= .00001) { passE = false; Debug.WriteLine("...ECEF Conversion Outside Limits: " + rcE.Longitude.ToDouble() + " - " + cE.Longitude.ToDouble()); }
            if (Math.Abs(rcE.ECEF.GeoDetic_Height.Meters - cE.ECEF.GeoDetic_Height.Meters) >= .00001) { passE = false; Debug.WriteLine("...ECEF Conversion Outside Limits: " + rcE.ECEF.GeoDetic_Height.Meters + " - " + cE.ECEF.GeoDetic_Height.Meters); }

            Pass.Write("ECEF WITH HEIGHT CHECK", passE);
        }
        private static void Check_UTM_MGRS_Boundaries()
        {
            //UTM MGRS BOUNDARY CHECK
            bool p = true;
            Coordinate cr = new Coordinate(-79.99, 0);
            if (!cr.UTM.WithinCoordinateSystemBounds || !cr.MGRS.WithinCoordinateSystemBounds) { p = false; }
            cr.Latitude.DecimalDegree = -80;
            if (cr.UTM.WithinCoordinateSystemBounds || cr.MGRS.WithinCoordinateSystemBounds) { p = false; }
            cr.Latitude.DecimalDegree = 83.99;
            if (!cr.UTM.WithinCoordinateSystemBounds || !cr.MGRS.WithinCoordinateSystemBounds) { p = false; }
            cr.Latitude.DecimalDegree = 84;
            if (cr.UTM.WithinCoordinateSystemBounds || cr.MGRS.WithinCoordinateSystemBounds) { p = false; }
            Pass.Write("UTM MGRS BOUNDARY CHECK", p);
        }
    }
}
