using System;

namespace CoordinateSharp
{
    /// <summary>
    /// Turn on/off eager loading of certain properties.
    /// </summary>
    [Serializable]
    public partial class EagerLoad
    {       
        /// <summary>
        /// Eager load celestial information.
        /// </summary>
        public bool Celestial { get; set; }
        /// <summary>
        /// Eager load UTM and MGRS information
        /// </summary>
        public bool UTM_MGRS { get; set; }
        /// <summary>
        /// Eager load Cartesian information
        /// </summary>
        public bool Cartesian { get; set; }
        /// <summary>
        /// Eager load ECEF information
        /// </summary>
        public bool ECEF { get; set; }
    }
}
