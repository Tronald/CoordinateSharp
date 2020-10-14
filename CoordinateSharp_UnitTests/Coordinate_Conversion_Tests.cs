using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CoordinateSharp;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Diagnostics;
using System.IO;

namespace CoordinateSharp_UnitTests
{
    [TestClass]
    public class Coordinate_Conversion_Tests
    {
        //Test Values
        private double[] tc1 = new double[] { 39.5768, 72.4859 };
        private double[] tc2 = new double[] { -15.5768, 100.4859 };
        private double[] tc3 = new double[] { 65.25, -15.1859 };
        private double[] tc4 = new double[] { -80.659, -152.49 };

        private EagerLoad eg = new EagerLoad() { Celestial = false }; //Turn off celestial for efficiency

        private IEnumerable<string> polar_coordinates;

        public Coordinate_Conversion_Tests()
        {
            polar_coordinates = File.ReadLines("CoordinateData\\UPS.csv");
        }

        /// <summary>
        /// Test decimal formatted coordinate conversions
        /// </summary>
        [TestMethod]
        public void Decimal_Format_Conversion()
        {
            string s1 = "39.577 72.486";
            string s2 = "-15.577 100.486";
            string s3 = "65.25 -15.186";
            string s4 = "-80.659 -152.49";

            string[] expecteds = new string[] { s1, s2, s3, s4 };

            CoordinateFormatOptions format = new CoordinateFormatOptions() { Format = CoordinateFormatType.Decimal };

            Assert_Geodetic_Conversion(expecteds, format);
        }

        /// <summary>
        /// Test decimal degree formatted coordinate conversions
        /// </summary>
        [TestMethod]
        public void Decimal_Degree_Format_Conversion()
        {
            string s1 = "N 39.577º E 72.486º";
            string s2 = "S 15.577º E 100.486º";
            string s3 = "N 65.25º W 15.186º";
            string s4 = "S 80.659º W 152.49º";

            string[] expecteds = new string[] { s1, s2, s3, s4 };

            CoordinateFormatOptions format = new CoordinateFormatOptions() { Format = CoordinateFormatType.Decimal_Degree };

            Assert_Geodetic_Conversion(expecteds, format);          
        }

        /// <summary>
        /// Test Degree Decimal Minute formatted coordinate conversions
        /// </summary>
        [TestMethod]
        public void Degree_Decimal_Minute_Format_Conversion()
        {
            string s1 = "N 39º 34.608' E 72º 29.154'";
            string s2 = "S 15º 34.608' E 100º 29.154'";
            string s3 = "N 65º 15' W 15º 11.154'";
            string s4 = "S 80º 39.54' W 152º 29.4'";

            string[] expecteds = new string[] { s1, s2, s3, s4 };

            CoordinateFormatOptions format = new CoordinateFormatOptions() { Format = CoordinateFormatType.Degree_Decimal_Minutes };

            Assert_Geodetic_Conversion(expecteds, format);
        }

        /// <summary>
        /// Test Degrees Minutes Seconds formatted coordinate conversions
        /// </summary>
        [TestMethod]
        public void Degrees_Minutes_Seconds_Conversion()
        {
            string s1 = "N 39º 34' 36.48\" E 72º 29' 9.24\"";
            string s2 = "S 15º 34' 36.48\" E 100º 29' 9.24\"";
            string s3 = "N 65º 15' 0\" W 15º 11' 9.24\"";
            string s4 = "S 80º 39' 32.4\" W 152º 29' 24\"";

            string[] expecteds = new string[] { s1, s2, s3, s4 };

            CoordinateFormatOptions format = new CoordinateFormatOptions() { Format = CoordinateFormatType.Degree_Minutes_Seconds };

            Assert_Geodetic_Conversion(expecteds, format);
        }

        /// <summary>
        /// Test UTM formatted coordinate conversions
        /// </summary>
        [TestMethod]
        public void UTM_Conversion()
        {
            string s1 = "43S 284065mE 4383807mN";
            string s2 = "47L 659324mE 8277320mN";
            string s3 = "28W 491315mE 7236329mN";
            string s4 = "A 1519951mE 1078227mN";

            string[] expecteds = new string[] { s1, s2, s3, s4 };          

            int x = 0;
            List<double[]> coordinates = new List<double[]>() { tc1, tc2, tc3, tc4 };

            foreach (string expected in expecteds)
            {
                Coordinate c = new Coordinate(coordinates[x][0], coordinates[x][1], eg);
                Assert.AreEqual(expected, c.UTM.ToString());
                x++;
            }
        }

        /// <summary>
        /// Test MGRS formatted coordinate conversions
        /// </summary>
        [TestMethod]
        public void MGRS_Conversion()
        {
            string s1 = "43S BD 84065 83807";
            string s2 = "47L PC 59324 77320";
            string s3 = "28W DT 91315 36329";
            string s4 = "A TC 19951 78227";

            string[] expecteds = new string[] { s1, s2, s3, s4 };

            int x = 0;
            List<double[]> coordinates = new List<double[]>() { tc1, tc2, tc3, tc4 };

            foreach (string expected in expecteds)
            {
                Coordinate c = new Coordinate(coordinates[x][0], coordinates[x][1], eg);
                Assert.AreEqual(expected, c.MGRS.ToString());
                x++;
            }
        }

        /// <summary>
        /// Test Cartesian formatted coordinate conversions
        /// </summary>
        [TestMethod]
        public void Cartesian_Conversion()
        {
            string s1 = "0.23195629 0.73504058 0.63711194";
            string s2 = "-0.17530918 0.94718448 -0.2685298";
            string s3 = "0.40404055 -0.10966863 0.90814317";
            string s4 = "-0.14395761 -0.07497152 -0.98673982";

            string[] expecteds = new string[] { s1, s2, s3, s4 };

            int x = 0;
            List<double[]> coordinates = new List<double[]>() { tc1, tc2, tc3, tc4 };

            foreach (string expected in expecteds)
            {
                Coordinate c = new Coordinate(coordinates[x][0], coordinates[x][1], eg);
                Assert.AreEqual(expected, c.Cartesian.ToString(),  string.Format("Value: {0}      Expected: {1}", c.Cartesian.ToString(), expected));
                x++;
            }
        }

        /// <summary>
        /// Test ECEF formatted coordinate conversions
        /// </summary>
        [TestMethod]
        public void ECEF_Conversion()
        {
            string s1 = "1481.463 km, 4694.572 km, 4041.879 km";
            string s2 = "-1118.416 km, 6042.731 km, -1701.665 km";
            string s3 = "2584.169 km, -701.42 km, 5769.435 km";
            string s4 = "-921.188 km, -479.745 km, -6271.904 km";

            string[] expecteds = new string[] { s1, s2, s3, s4 };

            int x = 0;
            List<double[]> coordinates = new List<double[]>() { tc1, tc2, tc3, tc4 };

            foreach (string expected in expecteds)
            {
                Coordinate c = new Coordinate(coordinates[x][0], coordinates[x][1], eg);
                Assert.AreEqual(expected, c.ECEF.ToString());
                x++;
            }
        }

        /// <summary>
        /// Test Geodetic height correctly converts with ECEF
        /// </summary>
        [TestMethod]
        public void Geodetic_To_ECEF_Height_Conversion()
        {
            //ECEF WITH HEIGHT CHECK           
            double latE = -80.6586;
            double longE = -152.49;

            Distance h = new Distance(1500, DistanceType.Meters);
            Coordinate cE = new Coordinate(latE, longE);

            cE.ECEF.Set_GeoDetic_Height(cE, h);
            Assert.AreEqual(0, cE.ECEF.X - -921.443,  .001, "X - GeoDetic Height Conversions Outside Limits");
            Assert.AreEqual(0, cE.ECEF.Y - -479.878,  .001, "Y - Setting GeoDetic Height Conversions Outside Limits");
            Assert.AreEqual(0, cE.ECEF.Z - -6273.377,  .001, "Z - Setting GeoDetic Height Conversions Outside Limits");                  
        }
        /// <summary>
        /// Test ECEF height correctly converts with geodetic 
        /// </summary>
        [TestMethod]
        public void ECEF_To_Geodetic_Height_Conversion()
        {
            //ECEF WITH HEIGHT CHECK           
            double latE = -80.6586;
            double longE = -152.49;

            Distance h = new Distance(1500, DistanceType.Meters);
            Coordinate cE = new Coordinate(latE, longE);

            cE.ECEF.Set_GeoDetic_Height(cE, h);
        
            ECEF ecefE = new ECEF(cE.ECEF.X, cE.ECEF.Y, cE.ECEF.Z);

            Coordinate rcE = ECEF.ECEFToLatLong(ecefE);
        
            Assert.AreEqual(0, rcE.Latitude.ToDouble() - cE.Latitude.ToDouble(), .00001, "Latitude 1 Conversion Outside Limits");
            Assert.AreEqual(0, rcE.Longitude.ToDouble() - cE.Longitude.ToDouble(),  .00001, "Longitude 1 Conversion Outside Limits");
            Assert.AreEqual(0, rcE.ECEF.GeoDetic_Height.Meters - cE.ECEF.GeoDetic_Height.Meters,  .00001, "Height 1 Conversion Outside Limits");
           
            ecefE = new ECEF(cE, cE.ECEF.GeoDetic_Height);

            rcE = ECEF.ECEFToLatLong(ecefE);

            Assert.AreEqual(0, rcE.Latitude.ToDouble() - cE.Latitude.ToDouble(), .00001, "Latitude 2 Conversion Outside Limits");
            Assert.AreEqual(0, rcE.Longitude.ToDouble() - cE.Longitude.ToDouble(), .00001, "Longitude 2 Conversion Outside Limits");
            Assert.AreEqual(0, rcE.ECEF.GeoDetic_Height.Meters - cE.ECEF.GeoDetic_Height.Meters, .00001, "Height 2 Conversion Outside Limits");     
        }
       
        /// <summary>
        /// Tests to ensure UTM gridzone locks during conversions if specified.
        /// </summary>
        [TestMethod]
        public void UTM_Grid_Zone_Conversion_Lock()
        {
            Coordinate coord1 = new Coordinate(51.5074, 1);
            Assert.AreEqual(31, coord1.UTM.LongZone, "Grid zone value not expected.");
            coord1.Lock_UTM_MGRS_Zone(30);
            Assert.AreEqual(30, coord1.UTM.LongZone, "Grid zone did not lock to new value.");
        }
        /// <summary>
        /// Tests to ensure MGRS grid zone locks during conversions if specified.
        /// </summary>
        [TestMethod]
        public void MGRS_Grid_Zone_Conversion_Lock()
        {

            Coordinate coord1 = new Coordinate(51.5074, 1);      
            Assert.AreEqual(31, coord1.MGRS.LongZone,"Grid zone value not expected");
            coord1.Lock_UTM_MGRS_Zone(30);
            Assert.AreEqual(30, coord1.MGRS.LongZone, "Grid zone did not lock to new value.");         
        }

        /// <summary>
        /// Tests to ensure UTM grid zone locked conversions are within limits.
        /// </summary>
        [TestMethod]
        public void UTM_Grid_Zone_Locked_Conversion_Limits()
        {
            Coordinate coord1 = new Coordinate(51.5074, 1);
            Coordinate coord2 = new Coordinate(51.5074, 1);
            coord1.Lock_UTM_MGRS_Zone(30); //Lock first coord to zone 30

            //2 degree change tested at 1.1 Meter precision
            Coordinate coordVal = Coordinate.Parse(coord1.UTM.ToString());

            Assert.AreEqual(0, coordVal.Latitude.ToDouble() - coord2.Latitude.ToDouble(),  .00001, "Latitude 1 Outside Limits");
            Assert.AreEqual(0, coordVal.Longitude.ToDouble() - coord2.Longitude.ToDouble(),  .00001, "Longitude 1 Outside Limits");
        }
        /// <summary>
        /// Tests to ensure MGRS grid zone locked conversions are within limits.
        /// </summary>
        [TestMethod]
        public void MGRS_Grid_Zone_Locked_Conversion_Limits()
        {

            Coordinate coord1 = new Coordinate(51.5074, 1);
            Coordinate coord2 = new Coordinate(51.5074, 1);
            coord1.Lock_UTM_MGRS_Zone(30); //Lock first coord to zone 30

            //2 degree change tested at 1.1 Meter precision
            Coordinate coordVal = Coordinate.Parse(coord1.MGRS.ToString());

            Assert.AreEqual(0, coordVal.Latitude.ToDouble() - coord2.Latitude.ToDouble(), .00001, "Latitude 1 Outside Limits");
            Assert.AreEqual(0, coordVal.Longitude.ToDouble() - coord2.Longitude.ToDouble(), .00001, "Longitude 1 Outside Limits");
        }

        /// <summary>
        /// Tests to ensure UTM grid zone unlocks during conversions if specified.
        /// </summary>
        [TestMethod]
        public void UTM_Grid_Zone_Conversion_Unlock()
        {
            Coordinate coord1 = new Coordinate(51.5074, 1);
           
            coord1.Lock_UTM_MGRS_Zone(30); //Lock first coord to zone 30

            //2 degree change tested at 1.1 Meter precision
            Coordinate coordVal = Coordinate.Parse(coord1.UTM.ToString());          
            coord1.Unlock_UTM_MGRS_Zone();

            Assert.AreEqual(31, coord1.UTM.LongZone);      
        }
        /// <summary>
        /// Tests to ensure MGRS grid zone unlocks during conversions if specified.
        /// </summary>
        [TestMethod]
        public void MGRS_Grid_Zone_Conversion_Unlock()
        {

            Coordinate coord1 = new Coordinate(51.5074, 1); 

            coord1.Lock_UTM_MGRS_Zone(30); //Lock first coord to zone 30

            //2 degree change tested at 1.1 Meter precision
            Coordinate coordVal = Coordinate.Parse(coord1.MGRS.ToString());
            coord1.Unlock_UTM_MGRS_Zone();

            Assert.AreEqual(31, coord1.MGRS.LongZone);
        }
        /// <summary>
        /// Tests to ensure limits are not exceeded during grid zone locks.
        /// </summary>
        [TestMethod]
        public void UTM_MGRS_Grid_Zone_Lock_Conversion_Limits()
        {

            Coordinate coord1 = new Coordinate(51.5074, 1);           

            Assert.ThrowsException<ArgumentOutOfRangeException>(()=>coord1.Lock_UTM_MGRS_Zone(0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(()=>coord1.Lock_UTM_MGRS_Zone(61));       
        }

        /// <summary>
        /// Test to ensure correct UTM system is operated in
        /// </summary>
        [TestMethod]
        public void UTM_System_Conversion_Check()
        {
            Coordinate c = new Coordinate(84, 78, new EagerLoad(EagerLoadType.UTM_MGRS));
            Assert.AreEqual(UTM_Type.UTM, c.UTM.SystemType);
            c = new Coordinate(84.1, 78, new EagerLoad(EagerLoadType.UTM_MGRS));
            Assert.AreEqual(UTM_Type.UPS, c.UTM.SystemType);
            c = new Coordinate(-80, 78, new EagerLoad(EagerLoadType.UTM_MGRS));
            Assert.AreEqual(UTM_Type.UTM, c.UTM.SystemType);
            c = new Coordinate(-80.1, 78, new EagerLoad(EagerLoadType.UTM_MGRS));
            Assert.AreEqual(UTM_Type.UPS, c.UTM.SystemType);

        }
        /// <summary>
        /// Test to ensure correct MGRS system is operated in
        /// </summary>
        [TestMethod]
        public void MGRS_System_Conversion_Check()
        {
            Coordinate c = new Coordinate(84, 78, new EagerLoad(EagerLoadType.UTM_MGRS));
            Assert.AreEqual(MGRS_Type.MGRS, c.MGRS.SystemType);
            c = new Coordinate(84.1, 78, new EagerLoad(EagerLoadType.UTM_MGRS));
            Assert.AreEqual(MGRS_Type.MGRS_Polar, c.MGRS.SystemType);
            c = new Coordinate(-80, 78, new EagerLoad(EagerLoadType.UTM_MGRS));
            Assert.AreEqual(MGRS_Type.MGRS, c.MGRS.SystemType);
            c = new Coordinate(-80.1, 78, new EagerLoad(EagerLoadType.UTM_MGRS));
            Assert.AreEqual(MGRS_Type.MGRS_Polar, c.MGRS.SystemType);
        }

        /// <summary>
        /// Ensures correct MGRS Rounded string.
        /// </summary>
        [TestMethod]
        public void MGRS_Rounded_Check()
        {
            Coordinate c = new Coordinate(45.4596, -45.6986, new EagerLoad(EagerLoadType.UTM_MGRS));
            Assert.AreEqual("23T ML 45382 34246", c.MGRS.ToRoundedString());
           
        }
        /// <summary>
        /// Ensures correct MGRS Centimeter string.
        /// </summary>
        [TestMethod]
        public void MGRS_Centimeter_Check()
        {
            Coordinate c = new Coordinate(45.4596, -45.6986, new EagerLoad(EagerLoadType.UTM_MGRS));
            Assert.AreEqual("23T ML 45381.61737 34245.53566", c.MGRS.ToCentimeterString());
        }

        /// <summary>
        /// Ensures correct UTM Centimeter string.
        /// </summary>
        [TestMethod]
        public void UTM_Centimeter_Check()
        {
            Coordinate c = new Coordinate(45.4596, -45.6986, new EagerLoad(EagerLoadType.UTM_MGRS));
            Assert.AreEqual("23T 445381.61737mE 5034245.53566mN", c.UTM.ToCentimeterString());          
        }

        /// <summary>
        /// Ensures correct UTM Rounded string.
        /// </summary>
        [TestMethod]
        public void UTM_Rounded_Check()
        {
            Coordinate c = new Coordinate(45.4596, -45.6986, new EagerLoad(EagerLoadType.UTM_MGRS));
            Assert.AreEqual("23T 445382mE 5034246mN", c.UTM.ToRoundedString());
        }

      

        /// <summary>
        /// Test geodetic to UPS conversions
        /// </summary>
        [TestMethod]
        public void Geodetic_To_UPS_Conversions()
        {         
            EagerLoad el = new EagerLoad(EagerLoadType.UTM_MGRS);

            foreach (var currentLine in polar_coordinates)
            {
                string[] parts = currentLine.Split(',');

                double lat = Convert.ToDouble(parts[5]);
                double lng = Convert.ToDouble(parts[6]);
              
                string zone = parts[1];

                int easting = Convert.ToInt32(parts[2]);
                int northing = Convert.ToInt32(parts[3]);

                Coordinate c = new Coordinate(lat, lng, el);

                //skip 80-84 due to Earthpoint using different UTM zone returns. Both methods are accurate and test against EarthPoint, but will cause test to fail.
                if (lat >= 80 && lat <= 84) { continue; }
                if (Math.Abs(lat) >= 89.99999) { continue; }//Dont test as long doesn't exist at pole.

                Assert.AreEqual(string.Format("{0}{1}", c.UTM.LongZone, c.UTM.LatZone), zone, "Zone conversion value unexpected.");
                Assert.AreEqual(0, c.UTM.Easting - easting, 1, "Easting value outside limits");
                Assert.AreEqual(0, c.UTM.Northing - northing, 1, "Northing value outside limits");
            }
            
        }

        /// <summary>
        /// Test geodetic to MGRS Polar conversions
        /// </summary>
        [TestMethod]
        public void Geodetic_To_MGRS_Polar_Conversions()
        {
            EagerLoad el = new EagerLoad(EagerLoadType.UTM_MGRS);


            foreach (var currentLine in polar_coordinates)
            {
                string[] parts = currentLine.Split(',');

                double lat = Convert.ToDouble(parts[5]);
                double lng = Convert.ToDouble(parts[6]);

                string mgrs = parts[4];

                Coordinate c = new Coordinate(lat, lng, el);

                //skip 80-84 due to Earthpoint using different UTM zone returns. Both methods are accurate and test against EarthPoint, but will cause test to fail.
                if (lat >= 80 && lat <= 84) { continue; }
                if (Math.Abs(lat) >= 89.99999) { continue; }//Dont test as long doesn't exist at pole.


                string nMgrs = c.MGRS.ToString().Replace(" ", "");//Formatting test to ensure test runs correctly
                Assert.AreEqual(mgrs, nMgrs, "MGRS Polar value not expected");
            }

        }


        /// <summary>
        /// Test UPS to Geodetic conversions
        /// </summary>
        [TestMethod]
        public void UPS_To_Geodetic_Conversions()
        {
            EagerLoad el = new EagerLoad(EagerLoadType.UTM_MGRS);
            foreach (var currentLine in polar_coordinates)
            {

                string[] parts = currentLine.Split(',');

                double lat = Convert.ToDouble(parts[5]);
                double lng = Convert.ToDouble(parts[6]);
         
                Coordinate c = new Coordinate(lat, lng, el);

                //skip 80-84 due to Earthpoint using different UTM zone returns. Both methods are accurate and test against EarthPoint, but will cause test to fail.
                if (lat >= 80 && lat <= 84) { continue; }
                if (Math.Abs(lat) >= 89.99999) { continue; } //Dont test as long doesn't exist at pole.
           

                //CONVERT BACK TEST
                double precision = .0000001; //1.1 CM Convert Back Precision

                Coordinate bc = UniversalTransverseMercator.ConvertUTMtoLatLong(c.UTM, new EagerLoad(false));
                
                double l = c.Latitude.ToDouble();
                double bL = bc.Latitude.ToDouble();
                //IGNORE 360 values as that equals 0 degrees
                Assert.IsFalse(Math.Abs(bL - l) > precision && Math.Abs(bL - l) != 360, "Latitude value not expected.");
               
               
                l = c.Longitude.ToDouble();
                bL = bc.Longitude.ToDouble();
                //IGNORE 360 values as that equals 0 degrees
                Assert.IsFalse(Math.Abs(bL - l) > precision && Math.Abs(bL - l) != 360, "Longitude value not expected.");
            }
            
        }

        /// <summary>
        /// Test MGRS to Geodetic conversions
        /// </summary>
        [TestMethod]
        public void MGRS_Polar_To_Geodetic_Conversions()
        {
            EagerLoad el = new EagerLoad(EagerLoadType.UTM_MGRS);
            foreach (var currentLine in polar_coordinates)
            {

                string[] parts = currentLine.Split(',');

                double lat = Convert.ToDouble(parts[5]);
                double lng = Convert.ToDouble(parts[6]);
              
                Coordinate c = new Coordinate(lat, lng, el);

                //skip 80-84 due to Earthpoint using different UTM zone returns. Both methods are accurate and test against EarthPoint, but will cause test to fail.
                if (lat >= 80 && lat <= 84) { continue; }
                if (Math.Abs(lat) >= 89.99999) { continue; } //Dont test as long doesn't exist at pole.


                //CONVERT BACK TEST
                double precision = .0000001; //1.1 CM Convert Back Precision

              
                precision = .0003;
                if (Math.Abs(c.Latitude.ToDouble()) > 89) { precision = .002; }
                else if (Math.Abs(c.Latitude.ToDouble()) > 88) { precision = .0006; }

                Coordinate bc = MilitaryGridReferenceSystem.MGRStoLatLong(c.MGRS, new EagerLoad(false));
                double l = c.Latitude.ToDouble();
                double bL = bc.Latitude.ToDouble();
                Assert.IsFalse(Math.Abs(bL - l) > precision && Math.Abs(bL - l) != 360, "Longitude value not expected.");

                l = c.Longitude.ToDouble();
                bL = bc.Longitude.ToDouble();
                Assert.IsFalse(Math.Abs(bL - l) > precision && Math.Abs(bL - l) != 360, "Longitude value not expected.");
            }

        }

        /// <summary>
        /// Test roundtripping to ensure it is off.
        /// </summary>
        [TestMethod]
        public void Coordinate_Roundtrippable_Test()
        {
            CoordinatePart lat = new CoordinatePart(-0, -0, -0, CoordinatesPosition.N);
            Assert.AreEqual("N 0º 0' 0\"", lat.ToString(), "Latitude not expected");
            CoordinatePart lng = new CoordinatePart(-0, -0, -0, CoordinatesPosition.E);
            Assert.AreEqual("E 0º 0' 0\"", lng.ToString(), "Longitude not expected");
            Coordinate c = new Coordinate();
            c.Latitude = lat;
            c.Longitude = lng;
            Assert.AreEqual("N 0º 0' 0\" E 0º 0' 0\"", c.ToString(), "Coordinate not expected");
        }


        /// <summary>
        /// Asserts conversions
        /// </summary>
        /// <param name="lat">Latitude</param>
        /// <param name="lng">Longitude</param>
        /// <param name="expected">Expected Value</param>
        /// <param name="format">Coordinate Format</param>
        private void Assert_Geodetic_Conversion(string[] expecteds, CoordinateFormatOptions format)
        {
            int x = 0;
            List<double[]> coordinates = new List<double[]>() { tc1, tc2, tc3, tc4 };
          

            foreach (string expected in expecteds)
            {
                Coordinate c = new Coordinate(coordinates[x][0], coordinates[x][1], eg);
                c.FormatOptions = format;
                if(format.Format == CoordinateFormatType.Degree_Minutes_Seconds) { Debug.WriteLine(c.ToString()); }
                Assert.AreEqual(expected, c.ToString(), string.Format("Value: {0}      Expected: {1}", c.ToString(), expected));
                x++;
            }
        }
    }
}
