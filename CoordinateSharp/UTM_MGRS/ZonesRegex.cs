using System.Text.RegularExpressions;

namespace CoordinateSharp
{
    internal static class ZonesRegex
    {
        /// <summary>
        /// [aAbByYzZ] / Also works for [AaBbYyZz] / [ABYZ]
        /// </summary>
        internal static Regex UpsZoneRegex = new Regex("[aAbByYzZ]", RegexOptions.Compiled);

        /// <summary>
        /// [CcDdEeFfGgHhJjKkLlMm]
        /// </summary>
        internal static Regex SouthEmisphereZone = new Regex("[CcDdEeFfGgHhJjKkLlMm]", RegexOptions.Compiled);
    }
}
