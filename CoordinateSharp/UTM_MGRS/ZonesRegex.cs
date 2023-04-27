using System.Text.RegularExpressions;

namespace CoordinateSharp
{
    internal static class ZonesRegex
    {
        /// <summary>
        /// [aAbByYzZ] / Also works for [AaBbYyZz] / [ABYZ]
        /// </summary>
        internal static Regex UpsZoneRegex = new Regex("[aAbByYzZ]");

        /// <summary>
        /// [CcDdEeFfGgHhJjKkLlMm]
        /// </summary>
        internal static Regex SouthEmisphereZoneRegex = new Regex("[CcDdEeFfGgHhJjKkLlMm]");
    }
}
