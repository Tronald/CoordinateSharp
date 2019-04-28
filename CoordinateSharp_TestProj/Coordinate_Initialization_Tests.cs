using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoordinateSharp;
namespace CoordinateSharp_TestProj
{
    public class Coordinate_Initialization
    {
        public static void Run_Test()
        {
            //Check for exceptions on coordinate type constructors
            Coordinate_Init_Error_Checks(); 
            CoordinatePart_Init_Error_Checks();
            UTM_Init_Error_Checks();
            MGRS_Init_Error_Checks();
            Cartesian_Init_Error_Checks();
            ECEF_Init_Error_Checks();
            Console.WriteLine();

            //Check to ensure coordinate/coordinate part ranges throw exceptions if outside bounds
            Coordinate_Init_Range_Checks();
            CoordinatePart_Init_Range_Checks();

            //Ensure coordinate part property change notification is working
            CoordinatePart_Constructor_PropChange_Checks();
        }
        private static void Coordinate_Init_Error_Checks()
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
            Pass.Write("Coordinate Initialization Error Checks", pass);
        }
        private static void CoordinatePart_Init_Error_Checks()
        {
            bool pass = true;
            try
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
            catch { pass = false; }
            Pass.Write("CoordinatePart Initialization Error Checks", pass);
        }
        private static void UTM_Init_Error_Checks()
        {
            bool pass = true;
            try
            {
                UniversalTransverseMercator utm = new UniversalTransverseMercator("Q", 14, 581943.5, 2111989.8);
                utm = new UniversalTransverseMercator("Q", 14, 581943.5, 2111989.8, 6378160.000, 298.25);
            }
            catch { pass = false; }
            Pass.Write("UniversalTransverseMercator Initialization Error Checks", pass);
        }
        private static void MGRS_Init_Error_Checks()
        {
            bool pass = true;
            try
            {

                //Outputs 19T CE 51307 93264
                MilitaryGridReferenceSystem mgrs = new MilitaryGridReferenceSystem("T", 19, "CE", 51307, 93264);
                mgrs = new MilitaryGridReferenceSystem("T", 19, "CE", 51307, 93264, 6378160.000, 298.25);
            }
            catch { pass = false; }
            Pass.Write("MilitaryGridReferenceSytem Initialization Error Checks", pass);
        }
        private static void Cartesian_Init_Error_Checks()
        {
            bool pass = true;
            try
            {

                Coordinate c = new Coordinate();
                Cartesian cart = new Cartesian(c);
                cart = new Cartesian(345, -123, 2839);
            }
            catch { pass = false; }
            Pass.Write("Cartesian Initialization Error Checks", pass);
        }
        private static void ECEF_Init_Error_Checks()
        {
            bool pass = true;

            try
            {

                Coordinate c = new Coordinate();
                ECEF ecef = new ECEF(c);
                ecef = new ECEF(3194.469, 3194.469, 4487.419);
            }
            catch { pass = false; }
            Pass.Write("ECEF Initialization Error Checks", pass);
        }

        private static void Coordinate_Init_Range_Checks()
        {
            bool pass = true;
            try
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
                catch { }

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
            Pass.Write("Coordinate Initialization Range Checks", pass);
        }
        private static void CoordinatePart_Init_Range_Checks()
        {
            bool pass = true;
            try
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
            Pass.Write("CoordinatePart Initialization Range Checks", pass);
        }

        private static void CoordinatePart_Constructor_PropChange_Checks()
        {
            bool pass = true;
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
            catch { pass = false; }

            Pass.Write("CoordinatePart Constructor Property Change Checks: ", pass);
        }
    }
}
