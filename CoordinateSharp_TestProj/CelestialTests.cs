using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoordinateSharp_TestProj
{
    public class CelestialTests
    {
        public List<DateTime?> SunRises { get; set; }
        public List<DateTime?> MoonRises { get; set; }
        public List<DateTime?> SunSets { get; set; }
        public List<DateTime?> MoonSets { get; set; }

        public List<DateTime?> CivilDawn { get; set; }
        public List<DateTime?> CivilDusk { get; set; }
        public List<DateTime?> NauticalDawn { get; set; }
        public List<DateTime?> NauticalDusk { get; set; }
        public List<DateTime?> AstroDawn { get; set; }
        public List<DateTime?> AstroDusk { get; set; }
        public List<DateTime?> BottomSolarDiscRise { get; set; }
        public List<DateTime?> BottomSolarDiscSet { get; set; }

        public List<double> SunAlts { get; set; }
        public List<double> SunAzs { get; set; }
        public List<double> MoonAlts { get; set; }
        public List<double> MoonAzs { get; set; }
        public List<double> MoonDistances { get; set;}

        public List<double> MoonFraction{ get; set; }
        public List<DateTime> SolarEclispe { get; set; }
        public List<DateTime> LunarEclispe { get; set; }
        public List<DateTime> Perigee { get; set; }
    }
}
