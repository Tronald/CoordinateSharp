using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoordinateSharp
{
    internal class DataValues
    {
        //WGS84
        internal static readonly double WebMercatorEastingLimit = 20037508.342789; //20026376.39;
        //WGS84
        internal static readonly double WebMercatorNorthingLimit = 20037508.342789; //20048966.1;

        internal static readonly double DefaultSemiMajorAxis = 6378137.0; //WGS84 (Do Not Change without changing WebMercator to static value)

        internal static readonly double DefaultInverseFlattening = 298.257223563; //WGS84 (Do Not Change without changing WebMercator to static value)
    }
}
