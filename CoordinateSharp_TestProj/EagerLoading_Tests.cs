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
            Switch_Test();
            Extensions_Test();
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
        private static void Extensions_Test()
        {
            EagerLoad e = new EagerLoad(false);
            Coordinate c = new Coordinate(45, 75, new DateTime(2008, 1, 2), e);
            Celestial cel = c.CelestialInfo;

            c.Latitude.DecimalDegree = -44; //Trigger Calculations

            bool pass = true;

            if (!ReflectiveEquals(c.CelestialInfo, cel)) { pass = false; }
            c.EagerLoadSettings.Celestial = true;
            c.EagerLoadSettings.Extensions = new EagerLoad_Extensions(false); //All Extensions Off

            cel = c.CelestialInfo;
            c.Latitude.DecimalDegree++;

            //EagerLoading was turned on but exentsions were turned off
            //CelestialInfo should no longer be null but, properties will still be empty
            if (c.CelestialInfo == null || c.CelestialInfo.SunSet != null ) { pass = false; } 

           
            if (c.CelestialInfo == null) { pass = false; }

            var sunset = c.CelestialInfo.SunSet;
            var moonset = c.CelestialInfo.MoonSet;
            var sEclipse = c.CelestialInfo.SolarEclipse.LastEclipse.MaximumEclipse;
            var lEclipse = c.CelestialInfo.LunarEclipse.LastEclipse.PenumbralEclipseBegin;
            var moonZodiac = c.CelestialInfo.AstrologicalSigns.MoonSign;
            var sunZodiac = c.CelestialInfo.AstrologicalSigns.ZodiacSign;
            var moonName = c.CelestialInfo.AstrologicalSigns.MoonName;
          

            //The following test will test each extension individually.
            //The Eager Loaded extension property should change while all others should not.
            c.EagerLoadSettings.Extensions = new EagerLoad_Extensions(EagerLoad_ExtensionsType.Solar_Cycle); 
            c.Latitude.DecimalDegree++;
            if (c.CelestialInfo.SunSet == sunset ) { pass = false; }
            sunset = c.CelestialInfo.SunSet;

            c.EagerLoadSettings.Extensions = new EagerLoad_Extensions(EagerLoad_ExtensionsType.Lunar_Cycle);
            c.Latitude.DecimalDegree++;
            if (c.CelestialInfo.MoonSet == moonset || sunset != c.CelestialInfo.SunSet) { pass = false; }
            moonset = c.CelestialInfo.MoonSet;

            c.EagerLoadSettings.Extensions = new EagerLoad_Extensions(EagerLoad_ExtensionsType.Solar_Eclipse);
            c.Latitude.DecimalDegree*=-1;
            if (c.CelestialInfo.SolarEclipse.LastEclipse.MaximumEclipse== sEclipse || moonset!=c.CelestialInfo.MoonSet) { pass = false; }
            sEclipse = c.CelestialInfo.SolarEclipse.LastEclipse.MaximumEclipse;

            c.EagerLoadSettings.Extensions = new EagerLoad_Extensions(EagerLoad_ExtensionsType.Lunar_Eclipse);
            c.Latitude.DecimalDegree++;
            if (c.CelestialInfo.LunarEclipse.LastEclipse.PenumbralEclipseBegin == lEclipse || sEclipse != c.CelestialInfo.SolarEclipse.LastEclipse.MaximumEclipse) { pass = false; }
            lEclipse = c.CelestialInfo.LunarEclipse.LastEclipse.PenumbralEclipseBegin;

            //Check both moon and sun signs and names!
            c.EagerLoadSettings.Extensions = new EagerLoad_Extensions(EagerLoad_ExtensionsType.Zodiac);
            c.GeoDate = c.GeoDate.AddDays(15);
            if (c.CelestialInfo.AstrologicalSigns.MoonName == moonName || c.CelestialInfo.AstrologicalSigns.MoonSign == moonZodiac || c.CelestialInfo.AstrologicalSigns.ZodiacSign == sunZodiac || lEclipse != c.CelestialInfo.LunarEclipse.LastEclipse.PenumbralEclipseBegin) { pass = false; }
            moonName = c.CelestialInfo.AstrologicalSigns.MoonName;
            sunZodiac = c.CelestialInfo.AstrologicalSigns.ZodiacSign;
            moonZodiac = c.CelestialInfo.AstrologicalSigns.MoonSign;

            c.EagerLoadSettings.UTM_MGRS = true;
            c.EagerLoadSettings.Extensions = new EagerLoad_Extensions(EagerLoad_ExtensionsType.MGRS);             
            c.Latitude.DecimalDegree++;
            if (c.MGRS == null || c.CelestialInfo.AstrologicalSigns.MoonName != moonName || c.CelestialInfo.AstrologicalSigns.MoonSign != moonZodiac || c.CelestialInfo.AstrologicalSigns.ZodiacSign != sunZodiac) { pass = false; }
           

            Pass.Write("Extensions Tests (EagerLoad Extensions)", pass);

        }

        //Tests switching EagerLoading from on to off to see if properties initialize and avoid null exceptions
        private static void Switch_Test()
        {
            Coordinate c = new Coordinate(1, 1, new EagerLoad(false));
            c.EagerLoadSettings = new EagerLoad(true);
            try
            {
                c.Latitude.DecimalDegree++;
                Pass.Write("EagerLoad Switch Test", true);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                Pass.Write("EagerLoad Switch Test", false);
            }
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
