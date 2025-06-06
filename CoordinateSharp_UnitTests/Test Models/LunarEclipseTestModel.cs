using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoordinateSharp.UnitTests
{
    public class LunarEclipseTestModel
    {
        public DateTime Date { get; set; }
        public LunarEclipseType Type { get; set; }
        public DateTime PenumbralEclipseBegin { get; set; }
        public DateTime PartialEclipseBegin { get; set; }
        public DateTime TotalEclipseBegin { get; set; }
        public DateTime MaximumEclipse { get; set; }
        public DateTime TotalEclipseEnd { get; set; }
        public DateTime PartialEclipseEnd { get; set; }
        public DateTime PenumbralEclipseEnd { get; set; }
        public TimeSpan AorTDuration { get; set; }
        public double PenumbralMagnitude { get; set; }
        public double UmbralMagnitude { get; set; }
    }
}

