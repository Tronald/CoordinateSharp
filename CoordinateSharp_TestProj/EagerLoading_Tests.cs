using System;
using CoordinateSharp;
using System.Reflection;
namespace CoordinateSharp_TestProj
{
    public class EagerLoading_Tests
    {
        public static void Run_Tests()
        {
            EagerLoad e = new EagerLoad(false);
            Coordinate c = new Coordinate(45, 75, new DateTime(2008, 1, 2), e);

            Null_Properties_Check(c);
            Load_Call_Tests(c);
            Flags_Test();
        }

        private static void Null_Properties_Check(Coordinate c)
        {
            EagerLoad e = new EagerLoad(false);
           
            //Check to make sure items don't initialize
            bool pass = true;
            if (c.CelestialInfo != null) { pass = false; }
            if (c.UTM != null) { pass = false; }
            if (c.MGRS != null) { pass = false; }
            if (c.Cartesian != null) { pass = false; }
            if (c.ECEF != null) { pass = false; }
            Pass.Write("Null Properties (Celestial, UTM, MGRS, Cartesian, ECEF)", pass);
        }
        private static void Load_Call_Tests(Coordinate c)
        {
            //Check Load_Calls
            bool pass = true;
            c.LoadCelestialInfo();
            if (c.CelestialInfo == null) { pass = false; }
            c.LoadUTM_MGRS_Info();
            if (c.UTM == null) { pass = false; }
            if (c.MGRS == null) { pass = false; }
            c.LoadCartesianInfo();
            if (c.Cartesian == null) { pass = false; }
            c.LoadECEFInfo();
            if (c.ECEF == null) { pass = false; }
            Pass.Write("Load Calls (Celestial, UTM, MGRS, Cartesian, ECEF)", pass);
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
                if (!ReflectiveEquals(c.CelestialInfo, cel)) { pass = false; }
                if (!ReflectiveEquals(c.MGRS, mgrs)) { pass = false; }
                if (!ReflectiveEquals(c.UTM, utm)) { pass = false; }
                if (!ReflectiveEquals(c.Cartesian, cart)) { pass = false; }
                if (!ReflectiveEquals(c.ECEF, ecef)) { pass = false; }
                //Properties should remain equal as no load calls were made
                Pass.Write("Property State Hold (Celestial, UTM, MGRS, Cartesian, ECEF)", pass);

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
                Pass.Write("Property State Change (Celestial, UTM, MGRS, Cartesian, ECEF)", pass);

            }
            else
            {
                //Passes auto fail has properties didn't load when called.

                Pass.Write("Property State Hold (Celestial, UTM, MGRS, Cartesian, ECEF)", false);
                Pass.Write("Property State Change (Celestial, UTM, MGRS, Cartesian, ECEF)", false);

            }

        }
        private static void Flags_Test()
        {
            EagerLoad eg = new EagerLoad(EagerLoadType.Cartesian | EagerLoadType.Celestial | EagerLoadType.UTM_MGRS | EagerLoadType.ECEF);
            bool pass = true;
            if (eg.Cartesian == false || eg.Celestial == false || eg.UTM_MGRS == false || eg.ECEF == false) { pass = false; }
            eg = new EagerLoad(EagerLoadType.Celestial);
            if (eg.Cartesian == true || eg.Celestial == false || eg.UTM_MGRS == true || eg.ECEF == true) { pass = false; }
            eg = new EagerLoad(EagerLoadType.Cartesian);
            if (eg.Cartesian == false || eg.Celestial == true || eg.UTM_MGRS == true || eg.ECEF == true) { pass = false; }
            eg = new EagerLoad(EagerLoadType.UTM_MGRS);
            if (eg.Cartesian == true || eg.Celestial == true || eg.UTM_MGRS == false || eg.ECEF == true) { pass = false; }
            eg = new EagerLoad(EagerLoadType.ECEF);
            if (eg.Cartesian == true || eg.Celestial == true || eg.UTM_MGRS == true || eg.ECEF == false) { pass = false; }

            eg = new EagerLoad(EagerLoadType.UTM_MGRS | EagerLoadType.Celestial);
            if (eg.Cartesian == true || eg.Celestial == false || eg.UTM_MGRS == false || eg.ECEF == true) { pass = false; }
            eg = new EagerLoad(EagerLoadType.Cartesian | EagerLoadType.Celestial);
            if (eg.Cartesian == false || eg.Celestial == false || eg.UTM_MGRS == true || eg.ECEF == true) { pass = false; }
            eg = new EagerLoad(EagerLoadType.UTM_MGRS | EagerLoadType.Cartesian);
            if (eg.Cartesian == false || eg.Celestial == true || eg.UTM_MGRS == false || eg.ECEF == true) { pass = false; }
            eg = new EagerLoad(EagerLoadType.ECEF | EagerLoadType.Celestial);
            if (eg.Cartesian == true || eg.Celestial == false || eg.UTM_MGRS == true || eg.ECEF == false) { pass = false; }
            eg = new EagerLoad(EagerLoadType.ECEF | EagerLoadType.Cartesian);
            if (eg.Cartesian == false || eg.Celestial == true || eg.UTM_MGRS == true || eg.ECEF == false) { pass = false; }
            eg = new EagerLoad(EagerLoadType.ECEF | EagerLoadType.Cartesian | EagerLoadType.UTM_MGRS);
            if (eg.Cartesian == false || eg.Celestial == true || eg.UTM_MGRS == false || eg.ECEF == false) { pass = false; }

            Pass.Write("Flags Test", pass);

        }

        private static bool ReflectiveEquals(object first, object second)
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
    }
}
