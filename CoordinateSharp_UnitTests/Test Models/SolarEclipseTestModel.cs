using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoordinateSharp.UnitTests
{
    public class SolarEclipseTestModel
    {
        public DateTime Date { get; set; }
        public SolarEclipseType Type { get; set; }
        public DateTime PartialEclipseBegin { get; set; }
        public DateTime AorTEclipseBegin { get; set; }
        public DateTime MaximumEclipse { get; set; }
        public DateTime AorTEclipseEnd { get; set; }
        public DateTime PartialEclipseEnd { get; set; }
        public TimeSpan AorTDuration { get; set; }
        public double Magnitude { get; set; }
        public double Coverage { get; set; }
    }
}
