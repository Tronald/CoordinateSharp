using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoordinateSharp
{
    /// <summary>
    /// Coordinate formatting options for a Coordinate object.
    /// </summary>
    [Serializable]
    public class CoordinateFormatOptions
    {
        /// <summary>
        /// Set default values with the constructor.
        /// </summary>
        public CoordinateFormatOptions()
        {
            Format = CoordinateFormatType.Degree_Minutes_Seconds;
            Round = 3;
            Display_Leading_Zeros = false;
            Display_Trailing_Zeros = false;
            Display_Symbols = true;
            Display_Degree_Symbol = true;
            Display_Minute_Symbol = true;
            Display_Seconds_Symbol = true;
            Display_Hyphens = false;
            Position_First = true;
        }
        /// <summary>
        /// Coordinate format type.
        /// </summary>
        public CoordinateFormatType Format { get; set; }
        /// <summary>
        /// Rounds Coordinates to the set value.
        /// </summary>
        public int Round { get; set; }
        /// <summary>
        /// Displays leading zeros.
        /// </summary>
        public bool Display_Leading_Zeros { get; set; }
        /// <summary>
        /// Display trailing zeros.
        /// </summary>
        public bool Display_Trailing_Zeros { get; set; }
        /// <summary>
        /// Allow symbols to display.
        /// </summary>
        public bool Display_Symbols { get; set; }
        /// <summary>
        /// Display degree symbols.
        /// </summary>
        public bool Display_Degree_Symbol { get; set; }
        /// <summary>
        /// Display minute symbols.
        /// </summary>
        public bool Display_Minute_Symbol { get; set; }
        /// <summary>
        /// Display secons symbol.
        /// </summary>
        public bool Display_Seconds_Symbol { get; set; }
        /// <summary>
        /// Display hyphens between values.
        /// </summary>
        public bool Display_Hyphens { get; set; }
        /// <summary>
        /// Show coordinate position first.
        /// Will show last if set 'false'.
        /// </summary>
        public bool Position_First { get; set; }
    }
    /// <summary>
    /// Coordinate Format Types.
    /// </summary>
    [Serializable]
    public enum CoordinateFormatType
    {
        /// <summary>
        /// Decimal Degree Format
        /// </summary>
        /// <remarks>
        /// Example: N 40.456 W 75.456
        /// </remarks>
        Decimal_Degree,
        /// <summary>
        /// Decimal Degree Minutes Format
        /// </summary>
        /// <remarks>
        /// Example: N 40º 34.552' W 70º 45.408'
        /// </remarks>
        Degree_Decimal_Minutes,
        /// <summary>
        /// Decimal Degree Minutes Format
        /// </summary>
        /// <remarks>
        /// Example: N 40º 34" 36.552' W 70º 45" 24.408'
        /// </remarks>
        Degree_Minutes_Seconds,
        /// <summary>
        /// Decimal Format
        /// </summary>
        /// <remarks>
        /// Example: 40.57674 -70.46574
        /// </remarks>
        Decimal
    }
}
