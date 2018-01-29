using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoordinateSharp;

namespace CoordinateSharpTesterConsole2
{
    class Program
    {
        //Unit Test Console
        //This is a partial unit test meant to test initialization of objects.
        //Error, limit & MVVM testing should be done in the WPF test app.

        static void Main(string[] args)
        {
            //Test Lat Long KWRI 40.0352° N, 74.5844° W
           
            //Test Coordinate Initialization w & w/o eager load
            d("INITIALIZATION TESTS:");
            Coordinate c = new Coordinate();
            d(c.ToString());
            c = new Coordinate(40.0352, -74.5844);
            d(c.ToString());
            c = new Coordinate(40.0352, -74.5844, new DateTime(2017,8,21,0,0,0));
            d(c.ToString());

            
            EagerLoad eagerLoad = new EagerLoad();
            c = new Coordinate(eagerLoad);
            d(c.ToString());
            c = new Coordinate(40.0352, -74.5844, eagerLoad);
            d(c.ToString());
            c = new Coordinate(40.0352, -74.5844, new DateTime(2017, 8, 21, 0, 0, 0), eagerLoad);
            d(c.ToString());

            //Test Coord String Formats
            CoordinateFormatOptions cfo = new CoordinateFormatOptions();
            cfo.Format = CoordinateFormatType.Degree_Decimal_Minutes;
            cfo.Display_Symbols = false;
            cfo.Display_Trailing_Zeros = true;
            c.FormatOptions = cfo;
            d(c.Latitude.ToString() + " - " + c.Longitude.ToString());

            //Test ToDouble
            d(c.Latitude.ToDouble() + "  -  " + c.Longitude.ToDouble());

            //Test UTM & MGRS
            d(c.MGRS.ToString());
            d(c.MGRS.Digraph + " - " + c.MGRS.Easting + " - " + c.MGRS.LatZone + " - " + c.MGRS.LongZone + " - " + c.MGRS.Northing);
            d(c.UTM.ToString());
            d(c.UTM.Easting + " - " + c.UTM.LatZone + " - " + c.UTM.LongZone + " - " + c.UTM.Northing);

            //TEST CELESTIAL
            //Main Properties
            d("SUN CONDITION: " + c.CelestialInfo.SunCondition.ToString());
            d("SUN RISE: " + c.CelestialInfo.SunRise);
            d("SUN SET: " + c.CelestialInfo.SunSet);
            d("SUN ALTITUDE: " + c.CelestialInfo.SunAltitude);
            d("SUN AZIMUTH: " + c.CelestialInfo.SunAzimuth);      

            d("MOON CONDITION: " + c.CelestialInfo.MoonCondition.ToString());
            d("MOON DISTANCE: " + c.CelestialInfo.MoonDistance);
            d("MOON RISE: " + c.CelestialInfo.MoonRise);
            d("MOON SET: " + c.CelestialInfo.MoonSet);

            //Sub Properties
            d("CIVIL DAWN: " + c.CelestialInfo.AdditionalSolarTimes.CivilDawn.ToString());
            d("CIVIL DUSK: " + c.CelestialInfo.AdditionalSolarTimes.CivilDusk.ToString());
            d("NAUTICAL DAWN: " + c.CelestialInfo.AdditionalSolarTimes.NauticalDawn.ToString());
            d("NAUTICAL DUSK: " + c.CelestialInfo.AdditionalSolarTimes.NauticalDusk.ToString());

            d("MOON NAME: " + c.CelestialInfo.AstrologicalSigns.MoonName);
            d("MOON SIGN: " + c.CelestialInfo.AstrologicalSigns.MoonSign);
            d("ZODIAC SIGN: " + c.CelestialInfo.AstrologicalSigns.ZodiacSign);

            d("MOON ANGLE: " + c.CelestialInfo.MoonIllum.Angle.ToString());
            d("MOON FRACTION: " + c.CelestialInfo.MoonIllum.Fraction.ToString());
            d("MOON PHASE: " + c.CelestialInfo.MoonIllum.Phase.ToString());
            d("MOON PHASENAME: " + c.CelestialInfo.MoonIllum.PhaseName.ToString());

            //VALUE CHANGE NOTIFICATION TEST (PARTIAL. MORE COMPLETE TESTING OF NOTIFICATION CHANGES SHOULD BE DONE IN THE MVVM WPF TESTER)
            //New Lat Long Cape Town Intl 33.9715° S, 18.6021° E
            d("COORDINATE CHANGE NOTIFICATION TESTS");
            c.Latitude.DecimalDegree = 33.9715;
            c.Latitude.Position = CoordinatesPosition.S;
            c.Longitude.DecimalDegree = 18.6021;
            c.Longitude.Position = CoordinatesPosition.E;

            d(c.ToString());
            d(c.MGRS.ToString());
            d(c.UTM.ToString());

            d("SUN CONDITION: " + c.CelestialInfo.SunCondition.ToString());
            d("SUN RISE: " + c.CelestialInfo.SunRise);
            d("SUN SET: " + c.CelestialInfo.SunSet);
            d("SUN ALTITUDE: " + c.CelestialInfo.SunAltitude);
            d("SUN AZIMUTH: " + c.CelestialInfo.SunAzimuth);

            d("MOON CONDITION: " + c.CelestialInfo.MoonCondition.ToString());
            d("MOON DISTANCE: " + c.CelestialInfo.MoonDistance);
            d("MOON RISE: " + c.CelestialInfo.MoonRise);
            d("MOON SET: " + c.CelestialInfo.MoonSet);
             
            //EAGERLOAD TESTS
            eagerLoad.Celestial = false;
            c = new Coordinate(40.0352, -74.5844, DateTime.Now, eagerLoad);
            try { d(c.CelestialInfo.SunSet.Value.ToString()); }
            catch (NullReferenceException ex) { d("TEST EAGERLOAD OFF: " + ex.Message); }
            c.LoadCelestialInfo();
            d(c.CelestialInfo.SunSet.Value.ToString());

            //TEST STATIC CELESTIALS
            Console.WriteLine(Celestial.CalculateCelestialTimes(40.0352, -74.5844, DateTime.Now).SunSet);

            Console.ReadKey();
        }
        //For quicker display code writes
        static void d(string display)
        {
            Console.WriteLine(display);
        }
    }
    
}
