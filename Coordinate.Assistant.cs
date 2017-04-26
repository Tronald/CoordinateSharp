using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoordinateSharp
{
    /// <summary>
    /// Used for UTM/MGRS Conversions
    /// </summary>
    internal class LatZones
    {
        public static List<string> longZongLetters = new List<string>(new string[]{"C", "D", "E", "F", "G", "H", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T",
        "U", "V", "W", "X"});
    }
    /// <summary>
    /// Used for setting whether a coordinate part is latitudinal or longitudinal
    /// </summary>
    public enum CoordinateType
    {
        /// <summary>
        /// Latitude
        /// </summary>
        Lat,
        /// <summary>
        /// Longitude
        /// </summary>
        Long
    }
    /// <summary>
    /// Used to set a coordinate parts position
    /// </summary>
    public enum CoordinatesPosition
    {
        /// <summary>
        /// North
        /// </summary>
        N,
        /// <summary>
        /// East
        /// </summary>
        E,
        /// <summary>
        /// South
        /// </summary>
        S,
        /// <summary>
        /// West
        /// </summary>
        W
    }   
    
}
