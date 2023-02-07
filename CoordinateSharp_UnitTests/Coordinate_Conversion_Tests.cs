using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CoordinateSharp;
using System.Collections;
using System.Collections.Generic;

using System.Diagnostics;
using System.Globalization;
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
        /// Test Web Mercator formatted coordinate conversions
        /// </summary>
        [TestMethod]
        public void WebMercator_Conversion()
        {
            string s1 = "8069093.478mE 4804633.247mN";
            string s2 = "11186039.22mE -1755765.217mN";
            string s3 = "-1690486.655mE 9674532.836mN";
            string s4 = "-16975109.151mE -15975590.566mN";

            string[] expecteds = new string[] { s1, s2, s3, s4 };

            int x = 0;
            List<double[]> coordinates = new List<double[]>() { tc1, tc2, tc3, tc4 };

            foreach (string expected in expecteds)
            {
                EagerLoad el = new EagerLoad(false);

                Coordinate c = new Coordinate(coordinates[x][0], coordinates[x][1], eg);
                Assert.AreEqual(expected, c.WebMercator.ToString());

                Coordinate c2 = WebMercator.ConvertWebMercatortoLatLong(c.WebMercator);
                Assert.AreEqual(c2.ToString(), c.ToString());

                Coordinate c3 = WebMercator.ConvertWebMercatortoLatLong(c.WebMercator, el);
                Assert.AreEqual(c3.ToString(), c.ToString());
                Assert.ThrowsException<NullReferenceException>(() => c3.WebMercator.ToString()); //Ensure eagerload settings

                double easting = c.WebMercator.Easting;
                double northing = c.WebMercator.Northing;

                Coordinate c4 = WebMercator.ConvertWebMercatortoLatLong(easting,northing);
                Assert.AreEqual(c4.ToString(), c.ToString());

                Coordinate c5 = WebMercator.ConvertWebMercatortoLatLong(easting, northing, el);
                Assert.AreEqual(c5.ToString(), c.ToString());
                Assert.ThrowsException<NullReferenceException>(() => c5.WebMercator.ToString()); //Ensure eagerload settings
                x++;
            }
        }

        /// <summary>
        /// Test Web Mercator disables when datum is changed from WGS84
        /// </summary>
        [TestMethod]
        public void WebMercator_Not_WGS84_Block()
        {
            Coordinate c = new Coordinate(45, 80);
            string s = c.WebMercator.ToString();
            c.Set_Datum(Earth_Ellipsoid_Spec.IERS_2003);
            Assert.ThrowsException<FormatException>(() => s = c.WebMercator.ToString());
            c.Set_Datum(Earth_Ellipsoid_Spec.WGS84_1984);
            s = c.WebMercator.ToString();
        }

        /// <summary>
        /// Test GEOREF formatted coordinate conversions
        /// </summary>
        [TestMethod]
        public void GEOREF_Conversion()
        {
            string s1 = "SJNK291540346080";
            string s2 = "UELQ291540253920";
            string s3 = "LLQF488460150000";
            string s4 = "BANK306000204600";

            string[] expecteds = new string[] { s1, s2, s3, s4 };

            int x = 0;
            List<double[]> coordinates = new List<double[]>() { tc1, tc2, tc3, tc4 };

            foreach (string expected in expecteds)
            {
                EagerLoad el = new EagerLoad(false);

                Coordinate c = new Coordinate(coordinates[x][0], coordinates[x][1], eg);
                Assert.AreEqual(expected, c.GEOREF.ToString());

                Coordinate c2 = GEOREF.ConvertGEOREFtoLatLong(c.GEOREF);
                Assert.AreEqual(c2.ToString(), c.ToString());

                Coordinate c3 = GEOREF.ConvertGEOREFtoLatLong(c.GEOREF, el);
                Assert.AreEqual(c3.ToString(), c.ToString());
                Assert.ThrowsException<NullReferenceException>(() => c3.GEOREF.ToString()); //Ensure eagerload settings

                string quad_15 = c.GEOREF.Quad_15;
                string quad_1 = c.GEOREF.Quad_1;
                string easting = c.GEOREF.Easting;
                string northing = c.GEOREF.Northing;

                Coordinate c4 = GEOREF.ConvertGEOREFtoLatLong(quad_15, quad_1, easting, northing);
                Assert.AreEqual(c4.ToString(), c.ToString());

                Coordinate c5 = GEOREF.ConvertGEOREFtoLatLong(quad_15, quad_1, easting, northing, el);
                Assert.AreEqual(c5.ToString(), c.ToString());
                Assert.ThrowsException<NullReferenceException>(() => c5.GEOREF.ToString()); //Ensure eagerload settings
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
        /// Ensures UTM Rounded string with given precision validates precision.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UTM_Rounded_With_Precision_Input_Validation_Check()
        {
            Coordinate c = new Coordinate(45.4596, -45.6986, new EagerLoad(EagerLoadType.UTM_MGRS));
            var result = c.UTM.ToRoundedString(-2);
        }

        /// <summary>
        /// Ensures correct UTM Rounded string with given precision.
        /// </summary>
        [TestMethod]
        public void UTM_Rounded_With_Precision_Check()
        {
            Coordinate c = new Coordinate(45.4596, -45.6986, new EagerLoad(EagerLoadType.UTM_MGRS));
            Assert.AreEqual("23T 445381.62mE 5034245.54mN", c.UTM.ToRoundedString(2));
        }

        /// <summary>
        /// Ensures correct UTM Rounded string with given precision and culture.
        /// </summary>
        [TestMethod]
        public void UTM_Rounded_With_Precision_With_Culture_Check()
        {
            Coordinate c = new Coordinate(45.4596, -45.6986, new EagerLoad(EagerLoadType.UTM_MGRS));
            Assert.AreEqual("23T 445381,62mE 5034245,54mN", c.UTM.ToRoundedString(2, new CultureInfo("hu-HU")));
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
        /// Ensure decimal minutes and seconds properly round string output if field is equal to 60.
        /// </summary>
        [TestMethod]
        public void Check_60_Degree_Handling()
        {
            Coordinate c = new Coordinate(new EagerLoad(false));
            var options = new CoordinateFormatOptions();
            options.Format = CoordinateFormatType.Degree_Decimal_Minutes;
            var lat = new CoordinatePart(CoordinateType.Lat);
            lat.Degrees = 89;
            lat.Minutes = 59;
            lat.Seconds = 59.99999;
            var lng = new CoordinatePart(CoordinateType.Long);
            lng.Degrees = 179;
            lng.Minutes = 59;
            lng.Seconds = 59.99999;

            c.Latitude = lat;
            c.Longitude = lng;

            Assert.AreEqual("N 90º 0' 0\" E 180º 0' 0\"", c.ToString());
            Assert.AreEqual("N 90º 0' E 180º 0'", c.ToString(options));

            lat.Position = CoordinatesPosition.S;
            lng.Position = CoordinatesPosition.W;


            Assert.AreEqual("S 90º 0' 0\" W 180º 0' 0\"", c.ToString());
            Assert.AreEqual("S 90º 0' W 180º 0'", c.ToString(options));

        }

        [TestMethod]
        public void UTM_Ellipsoid_Conversions()
        {
            UniversalTransverseMercator utm = UniversalTransverseMercator.Parse("16N 500872 5505009", Earth_Ellipsoid_Spec.Clarke_1866);
            Coordinate c = UniversalTransverseMercator.ConvertUTMtoLatLong(utm, new EagerLoad(EagerLoadType.UTM_MGRS));

            //Check ellipsoid values carry
            Earth_Ellipsoid ee = Earth_Ellipsoid.Get_Ellipsoid(Earth_Ellipsoid_Spec.Clarke_1866);
            Assert.AreEqual(ee.Equatorial_Radius, c.Equatorial_Radius, "Equatorial Radius do not match");
            Assert.AreEqual(ee.Inverse_Flattening, c.Inverse_Flattening, "Inverse Flattening values do not match");
            Assert.AreEqual(utm.LongZone, c.UTM.LongZone, "UTM Long Zones do not match");
            Assert.AreEqual(utm.Easting, c.UTM.Easting, 1, "UTM Easting does not match");
            Assert.AreEqual(utm.Northing, c.UTM.Northing, 1, "UTM Northing does not match");

            c.Set_Datum(Earth_Ellipsoid_Spec.WGS84_1984);

            Assert.AreEqual(c.UTM.Easting, 500872, 1, "UTM Easting does not match WGS84 expected");
            Assert.AreEqual(c.UTM.Northing, 5505228, 1, "UTM Northing does not match WGS84 expected");
        }

        [TestMethod]
        public void MGRS_Ellipsoid_Conversions()
        {
            MilitaryGridReferenceSystem mgrs = MilitaryGridReferenceSystem.Parse("16U EA 00872 05009", Earth_Ellipsoid_Spec.Clarke_1866);
            Coordinate c = MilitaryGridReferenceSystem.MGRStoLatLong(mgrs, new EagerLoad(EagerLoadType.UTM_MGRS));

            //Check ellipsoid values carry
            Earth_Ellipsoid ee = Earth_Ellipsoid.Get_Ellipsoid(Earth_Ellipsoid_Spec.Clarke_1866);
            Assert.AreEqual(ee.Equatorial_Radius, c.Equatorial_Radius, "Equatorial Radius do not match");
            Assert.AreEqual(ee.Inverse_Flattening, c.Inverse_Flattening, "Inverse Flattening values do not match");

            Assert.AreEqual(mgrs.LongZone, c.MGRS.LongZone, "MGRS Long Zones do not match");
            Assert.AreEqual(mgrs.Digraph, c.MGRS.Digraph, "MGRS Designators do not match");
            Assert.AreEqual(mgrs.Easting, c.MGRS.Easting, 1, "MGRS Easting does not match");
            Assert.AreEqual(mgrs.Northing, c.MGRS.Northing, 1, "MGRS Northing does not match");

            c.Set_Datum(Earth_Ellipsoid_Spec.WGS84_1984);

            Assert.AreEqual(c.MGRS.Easting, 00872, 1, "MGRS Easting does not match WGS84 expected");
            Assert.AreEqual(c.MGRS.Northing, 05228, 1, "MGRS Northing does not match WGS84 expected");
        }


        /// <summary>
        /// Asserts conversions
        /// </summary>
        /// <param name="expecteds">Expected Value</param>
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
