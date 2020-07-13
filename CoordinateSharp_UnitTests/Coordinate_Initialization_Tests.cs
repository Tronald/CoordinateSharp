using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CoordinateSharp;

namespace CoordinateSharp_UnitTests
{
    [TestClass]
    public class Coordinate_Initialization_Tests
    {
        /// <summary>
        /// Test base coordinate initialization to ensure no exceptions are thrown.
        /// </summary>
        [TestMethod]
        public void Coordinate_Initializes_Without_Exceptions()
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

        /// <summary>
        /// Test base coordinate part initialization to ensure no exceptions are thrown.
        /// </summary>
        [TestMethod]
        public void CoordinatePart_Initializes_Without_Exceptions()
        {

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

        /// <summary>
        /// Test UTM initialization to ensure no exceptions are thrown.
        /// </summary>
        [TestMethod]
        public void UTM_Initializes_Without_Exceptions()
        {

            UniversalTransverseMercator utm = new UniversalTransverseMercator("Q", 14, 581943.5, 2111989.8);
            utm = new UniversalTransverseMercator("Q", 14, 581943.5, 2111989.8, 6378160.000, 298.25);
            utm = new UniversalTransverseMercator("14Q", 581943.5, 2111989.8);
            utm = new UniversalTransverseMercator("14Q", 581943.5, 2111989.8, 6378160.000, 298.25);
            //ADD UPS

        }

        /// <summary>
        /// Test MGRS initialization to ensure no exceptions are thrown.
        /// </summary>
        [TestMethod]
        public void MGRS_Initializes_Without_Exceptions()
        {
            MilitaryGridReferenceSystem mgrs = new MilitaryGridReferenceSystem("T", 19, "CE", 51307, 93264);
            mgrs = new MilitaryGridReferenceSystem("T", 19, "CE", 51307, 93264, 6378160.000, 298.25);
            mgrs = new MilitaryGridReferenceSystem("19T", "CE", 51307, 93264);
            mgrs = new MilitaryGridReferenceSystem("19T", "CE", 51307, 93264, 6378160.000, 298.25);
            //ADD MGRS POLAR

        }
        /// <summary>
        /// Test Cartesian initialization to ensure no exceptions are thrown.
        /// </summary>
        [TestMethod]
        public void Cartesian_Initializes_Without_Exceptions()
        {
            Coordinate c = new Coordinate();
            Cartesian cart = new Cartesian(c);
            cart = new Cartesian(345, -123, 2839);
        }
        /// <summary>
        /// Test ECEF initialization to ensure no exceptions are thrown.
        /// </summary>
        [TestMethod]
        public void ECEF_Initializes_Without_Exceptions()
        {
            Coordinate c = new Coordinate();
            ECEF ecef = new ECEF(c);
            ecef = new ECEF(3194.469, 3194.469, 4487.419);
        }
        /// <summary>
        /// Tests to ensure coordinate cannot initialize outside of allowed system ranges.
        /// </summary>
        [TestMethod]
        public void Coordinate_Initialize_Within_Allowed_Ranges()
        {
            EagerLoad eg = new EagerLoad();
            Coordinate c = new Coordinate(90, 180);
            c = new Coordinate(-90, -180);
            c = new Coordinate(90, 180, new DateTime());
            c = new Coordinate(-90, -180, new DateTime());
            c = new Coordinate(90, 180, eg);
            c = new Coordinate(-90, -180, eg);
            c = new Coordinate(90, 180, new DateTime(), eg);
            c = new Coordinate(-90, -180, new DateTime(), eg);
        }
        /// <summary>
        /// Tests to ensure coordinate throws exceptions when limits are exceeded.
        /// </summary>
        [TestMethod]
        public void Coordinate_Throws_ArguementOutOfRangeException_When_Ranges_Exceed()
        {

            Coordinate c;
            EagerLoad eg = new EagerLoad();
            string failMsg = "Coordinate initialized with exceeded limitations.";

            //Should fail as arguments are out of range.
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => c = new Coordinate(91, 180), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => c = new Coordinate(91, 180), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => c = new Coordinate(90, 181), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => c = new Coordinate(-91, -180), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => c = new Coordinate(-90, -181), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => c = new Coordinate(91, 180, new DateTime()), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => c = new Coordinate(90, 181, new DateTime()), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => c = new Coordinate(-91, -180, new DateTime()), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => c = new Coordinate(-90, -181, new DateTime()), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => c = new Coordinate(91, 180, new DateTime(), eg), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => c = new Coordinate(90, 181, new DateTime(), eg), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => c = new Coordinate(-91, -180, new DateTime(), eg), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => c = new Coordinate(-90, -181, new DateTime(), eg), failMsg);

        }
        /// <summary>
        /// Tests to ensure coordinate part cannot initialize outside of allowed system ranges.
        /// </summary>
        [TestMethod]
        public void CoordinatePart_Initialize_Within_Allowed_Ranges()
        {
            Coordinate c = new Coordinate();
            CoordinatePart cp = new CoordinatePart(90, CoordinateType.Lat);
            cp = new CoordinatePart(-90, CoordinateType.Lat);
            cp = new CoordinatePart(89, 59, CoordinatesPosition.N);
            cp = new CoordinatePart(89, 59, CoordinatesPosition.S);
            cp = new CoordinatePart(89, 59, 59, CoordinatesPosition.N);
            cp = new CoordinatePart(89, 59, 59, CoordinatesPosition.S);
            cp = new CoordinatePart(180, CoordinateType.Long);
            cp = new CoordinatePart(-180, CoordinateType.Long);
            cp = new CoordinatePart(179, 59, CoordinatesPosition.E);
            cp = new CoordinatePart(179, 59, CoordinatesPosition.W);
            cp = new CoordinatePart(179, 59, 59, CoordinatesPosition.E);
            cp = new CoordinatePart(179, 59, 59, CoordinatesPosition.W);
        }
        /// <summary>
        /// Tests to ensure coordinate part throws exceptions when limits are exceeded.
        /// </summary>
        [TestMethod]
        public void CoordinatePart_Throws_ArguementOutOfRangeException_When_Ranges_Exceed()
        {
            CoordinatePart cp;
            string failMsg = "CoordinatePart initialized with exceeded limitations.";

            //Should fail as arguments are out of range.
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(91, CoordinateType.Lat), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(-91, CoordinateType.Lat), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(181, CoordinateType.Long), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(-181, CoordinateType.Long), failMsg);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(91, 0, CoordinatesPosition.N), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(90, 1, CoordinatesPosition.N), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(89, 60, CoordinatesPosition.N), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(91, 0, CoordinatesPosition.N), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(90, 1, CoordinatesPosition.N), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(89, 60, CoordinatesPosition.N), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(-90, 1, CoordinatesPosition.N), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(89, -1, CoordinatesPosition.N), failMsg);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(91, 0, CoordinatesPosition.S), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(90, 1, CoordinatesPosition.S), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(89, 60, CoordinatesPosition.S), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(91, 0, CoordinatesPosition.S), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(90, 1, CoordinatesPosition.S), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(89, 60, CoordinatesPosition.S), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(-90, 1, CoordinatesPosition.S), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(89, -1, CoordinatesPosition.S), failMsg);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(91, 0, 0, CoordinatesPosition.N), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(90, 0, 1, CoordinatesPosition.N), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(89, 59, 60, CoordinatesPosition.N), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(90, 0, 1, CoordinatesPosition.N), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(89, 59, 60, CoordinatesPosition.N), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(-90, 0, 0, CoordinatesPosition.N), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(89, -1, 0, CoordinatesPosition.N), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(89, 1, -1, CoordinatesPosition.N), failMsg);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(91, 0, 0, CoordinatesPosition.S), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(90, 0, 1, CoordinatesPosition.S), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(89, 59, 60, CoordinatesPosition.S), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(90, 0, 1, CoordinatesPosition.S), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(89, 59, 60, CoordinatesPosition.S), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(-90, 0, 0, CoordinatesPosition.S), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(89, -1, 0, CoordinatesPosition.S), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(89, 1, -1, CoordinatesPosition.S), failMsg);


            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(181, 0, CoordinatesPosition.E), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(180, 1, CoordinatesPosition.E), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(179, 60, CoordinatesPosition.E), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(181, 0, CoordinatesPosition.E), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(180, 1, CoordinatesPosition.E), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(179, 60, CoordinatesPosition.E), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(-180, 1, CoordinatesPosition.E), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(179, -1, CoordinatesPosition.E), failMsg);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(181, 0, CoordinatesPosition.W), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(180, 1, CoordinatesPosition.W), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(179, 60, CoordinatesPosition.W), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(181, 0, CoordinatesPosition.W), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(180, 1, CoordinatesPosition.W), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(179, 60, CoordinatesPosition.W), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(-180, 1, CoordinatesPosition.W), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(179, -1, CoordinatesPosition.W), failMsg);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(181, 0, 0, CoordinatesPosition.E), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(180, 0, 1, CoordinatesPosition.E), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(179, 59, 60, CoordinatesPosition.E), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(180, 0, 1, CoordinatesPosition.E), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(179, 59, 60, CoordinatesPosition.E), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(-180, 0, 0, CoordinatesPosition.E), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(179, -1, 0, CoordinatesPosition.E), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(179, 1, -1, CoordinatesPosition.E), failMsg);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(181, 0, 0, CoordinatesPosition.W), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(180, 0, 1, CoordinatesPosition.W), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(179, 59, 60, CoordinatesPosition.W), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(180, 0, 1, CoordinatesPosition.W), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(179, 59, 60, CoordinatesPosition.W), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(-180, 0, 0, CoordinatesPosition.W), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(179, -1, 0, CoordinatesPosition.W), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(179, 1, -1, CoordinatesPosition.W), failMsg);

        }
        
        /// <summary>
        /// Tests to ensures properties change when a new CoordinatePart is initialized and assigned to a Coordinate object.
        /// </summary>
        [TestMethod]
        public void CoordinatePart_Constructor_Property_Checks()
        {
            Coordinate coord = new Coordinate();

            double lat = coord.Latitude.DecimalDegree;
            double lng = coord.Longitude.DecimalDegree;
            string MGRS = coord.MGRS.ToString();
            string UTM = coord.UTM.ToString();
            string ECEF = coord.ECEF.ToString();
            string Cartesian = coord.Cartesian.ToString();
            DateTime? cel = coord.CelestialInfo.MoonSet;

            CoordinatePart cpLat = new CoordinatePart(25, CoordinateType.Lat);
            CoordinatePart cpLng = new CoordinatePart(25, CoordinateType.Long);

            //PROP CHANGE ERROR CHECK

            cpLat.DecimalDegree = 27;
            cpLng.Seconds = 24;

            coord.Latitude = cpLat;
            Assert.AreNotEqual(lat, coord.Latitude.ToDouble());
           
            coord.Longitude = cpLng;
            Assert.AreNotEqual(lng, coord.Longitude.ToDouble());

            Assert.AreNotEqual(MGRS, coord.MGRS.ToString()); 
            Assert.AreNotEqual(UTM, coord.UTM.ToString());
            Assert.AreNotEqual(ECEF, coord.ECEF.ToString());
            Assert.AreNotEqual(Cartesian, coord.Cartesian.ToString());
            Assert.AreNotEqual(cel, coord.CelestialInfo.MoonSet);
        }

       
        /// <summary>
        /// Tests to Coordinate object initializes as specified in the Global Settings.
        /// </summary>
        [TestMethod]
        public void Global_Settings_Initialization_Check()
        {
            GlobalSettings.Default_CoordinateFormatOptions = new CoordinateFormatOptions() { Format = CoordinateFormatType.Decimal };
            GlobalSettings.Default_EagerLoad = new EagerLoad(false);

            Coordinate c = new Coordinate(25, 25);

            //Ensure setting took
            Assert.AreEqual("25 25", c.ToString());
            Assert.AreEqual(null, c.CelestialInfo);

            //Reset to continue tests
            GlobalSettings.Default_CoordinateFormatOptions = new CoordinateFormatOptions() { Format = CoordinateFormatType.Degree_Minutes_Seconds };
            GlobalSettings.Default_EagerLoad = new EagerLoad(true);
        }

        /// <summary>
        /// Test base coordinate initialization with new date to ensure no exceptions are thrown.
        /// </summary>
        [TestMethod]
        public void Coordinate_Initializes_With_New_Date_Without_Exceptions()
        {

            Coordinate c = new Coordinate();
            c = new Coordinate(25, 25, new DateTime());

        }

        /// <summary>
        /// Test base coordinate initialization with high date to ensure no exceptions are thrown.
        /// </summary>
        [TestMethod]
        public void Coordinate_Initializes_With_High_Date_Without_Exceptions()
        {
            Coordinate c = new Coordinate();
            c = new Coordinate(25, 25, new DateTime(3999, 8, 5, 10, 10, 0));        
        }


    }
}
