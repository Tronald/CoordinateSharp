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
            Check_UPS_MGRS_Polar();
            Check_UTM_MGRS_Grid_Zone_Lock();
        }
        private static void Run_Conversion_Passes(List<List<string>> Conversions)
        {
            //List of coordinates to test conversions against.
            List<double[]> coords = new List<double[]>();
            coords.Add(new double[] { 39.5768, 72.4859 });
            coords.Add(new double[] { -15.5768, 100.4859 });
            coords.Add(new double[] { 65.25, -15.1859 });
            coords.Add(new double[] { -80.659, -152.49 });
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
                            if (c.UTM.ToString() != coordList[y]) { pass = false; }
                            UniversalTransverseMercator utm = new UniversalTransverseMercator(c.UTM.LatZone, c.UTM.LongZone, c.UTM.Easting, c.UTM.Northing);
                            rc = UniversalTransverseMercator.ConvertUTMtoLatLong(utm);
                          
                            if (Math.Abs(rc.Latitude.ToDouble() - c.Latitude.ToDouble()) >= .00001 ) { pass = false; Debug.WriteLine("...UTM Conversion Outside Limits: " + rc.Latitude.ToDouble() + " - " + c.Latitude.ToDouble()); }
                            if (Math.Abs(rc.Longitude.ToDouble() - c.Longitude.ToDouble()) >= .00001) { pass = false; Debug.WriteLine("...UTM Conversion Outside Limits: " + rc.Longitude.ToDouble() + " - " + c.Longitude.ToDouble()); }

                            //SIGNED DEGREE METHOD                                                      
                            double[] sd = UniversalTransverseMercator.ConvertUTMtoSignedDegree(utm);
                            if (Math.Abs(sd[0] - c.Latitude.ToDouble()) >= .00001) { pass = false; Debug.WriteLine("...UTM Conversion Outside Limits: " + rc.Latitude.ToDouble() + " - " + c.Latitude.ToDouble()); }
                            if (Math.Abs(sd[1] - c.Longitude.ToDouble()) >= .00001) { pass = false; Debug.WriteLine("...UTM Conversion Outside Limits: " + rc.Longitude.ToDouble() + " - " + c.Longitude.ToDouble()); }


                            //OVERLOAD METHOD
                            utm = new UniversalTransverseMercator(c.UTM.LongZone + c.UTM.LatZone.ToString(), c.UTM.Easting, c.UTM.Northing);
                            rc = UniversalTransverseMercator.ConvertUTMtoLatLong(utm);
                            if (Math.Abs(rc.Latitude.ToDouble() - c.Latitude.ToDouble()) >= .00001 ) { pass = false; Debug.WriteLine("...UTM Conversion Outside Limits: " + rc.Latitude.ToDouble() + " - " + c.Latitude.ToDouble()); }
                            if (Math.Abs(rc.Longitude.ToDouble() - c.Longitude.ToDouble()) >= .00001)  { pass = false; Debug.WriteLine("...UTM Conversion Outside Limits: " + rc.Longitude.ToDouble() + " - " + c.Longitude.ToDouble()); }

                            

                            break;
                        case 5:
                            if (c.MGRS.ToString() != coordList[y] ) { pass = false; }
                            MilitaryGridReferenceSystem mgrs = new MilitaryGridReferenceSystem(c.MGRS.LatZone, c.MGRS.LongZone, c.MGRS.Digraph, c.MGRS.Easting, c.MGRS.Northing);
                            rc = MilitaryGridReferenceSystem.MGRStoLatLong(mgrs);
                            if (Math.Abs(rc.Latitude.ToDouble() - c.Latitude.ToDouble()) >= .0001) { pass = false; Debug.WriteLine("...MGRS Conversion Outside Limits: " + rc.Latitude.ToDouble() + " - " + c.Latitude.ToDouble()); }
                            if (Math.Abs(rc.Longitude.ToDouble() - c.Longitude.ToDouble()) >= .0001 ) { pass = false; Debug.WriteLine("...MGRS Conversion Outside Limits: " + rc.Longitude.ToDouble() + " - " + c.Longitude.ToDouble()); }

                            //SIGNED DEGREE METHOD                                                      
                            double[] sdM = MilitaryGridReferenceSystem.MGRStoSignedDegree(mgrs);
                            if (Math.Abs(sdM[0] - c.Latitude.ToDouble()) >= .0001 ) { pass = false; Debug.WriteLine("...MGRS Conversion Outside Limits: " + rc.Latitude.ToDouble() + " - " + c.Latitude.ToDouble()); }
                            if (Math.Abs(sdM[1] - c.Longitude.ToDouble()) >= .0001 ) { pass = false; Debug.WriteLine("...MGRS Conversion Outside Limits: " + rc.Longitude.ToDouble() + " - " + c.Longitude.ToDouble()); }


                            //OVERLOAD METHOD
                            mgrs = new MilitaryGridReferenceSystem(c.MGRS.LongZone + c.MGRS.LatZone.ToString(), c.MGRS.Digraph, c.MGRS.Easting, c.MGRS.Northing);
                            rc = MilitaryGridReferenceSystem.MGRStoLatLong(mgrs);
                            if (Math.Abs(rc.Latitude.ToDouble() - c.Latitude.ToDouble()) >= .0001) { pass = false; Debug.WriteLine("...MGRS Conversion Outside Limits: " + rc.Latitude.ToDouble() + " - " + c.Latitude.ToDouble()); }
                            if (Math.Abs(rc.Longitude.ToDouble() - c.Longitude.ToDouble()) >= .0001) { pass = false; Debug.WriteLine("...MGRS Conversion Outside Limits: " + rc.Longitude.ToDouble() + " - " + c.Longitude.ToDouble()); }

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
       
        private static void Check_UPS_MGRS_Polar()
        {
            bool pass = true;
            EagerLoad el = new EagerLoad(EagerLoadType.UTM_MGRS);

            Console.WriteLine();
            Console.WriteLine("...Checking Polar Regions (this may take a minute)");
            Console.WriteLine();

            using (StreamReader sr = new StreamReader("CoordinateData\\UPS.csv"))
            {
                string currentLine;
                // currentLine will be null when the StreamReader reaches the end of file
                while ((currentLine = sr.ReadLine()) != null)
                {
                    string[] parts = currentLine.Split(',');

                    double lat = Convert.ToDouble(parts[5]);
                    double lng = Convert.ToDouble(parts[6]);
                  
                    string utm = parts[0];
                    string mgrs = parts[4];
                    string zone = parts[1];
                    int easting = Convert.ToInt32(parts[2]);
                    int northing = Convert.ToInt32(parts[3]);

                    Coordinate c = new Coordinate(lat, lng, el);

                    //skip 80-84 due to Earthpoint using different UTM zone returns. Both methods are accurate and test against EarthPoint, but will cause test to fail.
                    if(lat>=80 && lat <= 84) { continue; }
                    if (Math.Abs(lat) >= 89.99999) { continue; }//Dont test as long doesn't exist at pole.
                    if (string.Format("{0}{1}",c.UTM.LongZone, c.UTM.LatZone) != zone) { pass = false; break; }
                    if (Math.Abs(c.UTM.Easting-easting)>1) { pass = false; break; }
                    if (Math.Abs(c.UTM.Northing - northing) > 1) { pass = false; break; }
                    string nMgrs = c.MGRS.ToString().Replace(" ", "");
                    if (nMgrs!= mgrs) { pass = false; break; }
                    if (Math.Abs(c.UTM.Easting - easting) > 1) { pass = false; break; }
                    if (Math.Abs(c.UTM.Northing - northing) > 1) { pass = false; break; }

                    //CONVERT BACK TEST
                    double precision = .0000001; //1.1 CM Convert Back Precision
                    
                    Coordinate bc = UniversalTransverseMercator.ConvertUTMtoLatLong(c.UTM, new EagerLoad(false));
                    double l = c.Latitude.ToDouble();
                    double bL = bc.Latitude.ToDouble();
                    //IGNORE 360 values as that equals 0 degrees
                    if (Math.Abs(bL - l) > precision && Math.Abs(bL - l) != 360) { pass = false; break; }
                    l = c.Longitude.ToDouble();
                    bL = bc.Longitude.ToDouble();
                    if (Math.Abs(bL - l) > precision && Math.Abs(bL - l) != 360) { pass = false; break; }

                    precision = .0003;
                    if (Math.Abs(c.Latitude.ToDouble()) > 89) { precision = .002; }
                    else if (Math.Abs(c.Latitude.ToDouble()) > 88) { precision = .0006; }

                    bc = MilitaryGridReferenceSystem.MGRStoLatLong(c.MGRS, new EagerLoad(false));
                    l = c.Latitude.ToDouble();
                    bL = bc.Latitude.ToDouble();
                    if (Math.Abs(bL - l) > precision && Math.Abs(bL - l) != 360) { pass = false; break; }
                    l = c.Longitude.ToDouble();
                    bL = bc.Longitude.ToDouble();
                    if (Math.Abs(bL - l) > precision && Math.Abs(bL - l) != 360) { pass = false; break; }
                }
            }


            Pass.Write("UPS MGRS Polar Conversions: ", pass);
        }
        private static void Check_UTM_MGRS_Grid_Zone_Lock()
        {
            bool pass = true;
            Coordinate coord1 = new Coordinate(51.5074, 1);
            Coordinate coord2 = new Coordinate(51.5074, 1);
            coord1.Lock_UTM_MGRS_Zone(30); //Lock first coord to zone 30

            //2 degree change tested at 1.1 Meter precision
            //Try UTM
            Coordinate coordVal;
            if(!Coordinate.TryParse(coord1.UTM.ToString(), out coordVal))
            {
                pass = false; return;
            }
            if(Math.Abs(coordVal.Latitude.ToDouble() - coord2.Latitude.ToDouble()) > .00001) { pass = false; }
            if(Math.Abs(coordVal.Longitude.ToDouble() - coord2.Longitude.ToDouble()) > .00001) { pass = false; }

            //TRY MGRS
            if (!Coordinate.TryParse(coord1.MGRS.ToString(), out coordVal))
            {
                pass = false; return;
            }
            if (Math.Abs(coordVal.Latitude.ToDouble() - coord2.Latitude.ToDouble()) > .00001) { pass = false; }
            if (Math.Abs(coordVal.Longitude.ToDouble() - coord2.Longitude.ToDouble()) > .00001) { pass = false; }

            coord1.Unlock_UTM_MGRS_Zone();
            if(coord1.UTM.LongZone!=31 || coord1.MGRS.LongZone != 31) { pass = false; }

            //Test Validation
            try
            {
                coord1.Lock_UTM_MGRS_Zone(0);
                pass = false;
            }
            catch {//Intentional fail 
            }
            try
            {
                coord1.Lock_UTM_MGRS_Zone(61);
                pass = false;
            }
            catch
            {//Intentional fail 
            }



            Pass.Write("UTM MGRS Zone Lock Test", pass);
        }
    }
}
