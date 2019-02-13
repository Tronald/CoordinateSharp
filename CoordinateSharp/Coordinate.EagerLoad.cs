using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoordinateSharp
{
    /// <summary>
    /// Turn on/off eager loading of certain properties.
    /// </summary>
    [Serializable]
    public class EagerLoad
    {
        /// <summary>
        /// Create an EagerLoad object
        /// </summary>
        public EagerLoad()
        {
            Celestial = true;
            UTM_MGRS = true;
            Cartesian = true;
            ECEF = true;
        }

        /// <summary>
        /// Create an EagerLoad object with all options on or off
        /// </summary>
        /// <param name="isOn">Turns EagerLoad on or off</param>
        public EagerLoad(bool isOn)
        {
            Celestial = isOn;
            UTM_MGRS = isOn;
            Cartesian = isOn;
            ECEF = isOn;
        }

        /// <summary>
        /// Create an EagerLoad object with only the specified flag options turned on.
        /// </summary>
        /// <param name="et">EagerLoadType</param>
        public EagerLoad(EagerLoadType et)
        {
            Cartesian = false;
            Celestial = false;
            UTM_MGRS = false;
            ECEF = false;

            if (et.HasFlag(EagerLoadType.Cartesian))
            {
                Cartesian = true;
            }
            if (et.HasFlag(EagerLoadType.Celestial))
            {
                Celestial = true;
            }
            if (et.HasFlag(EagerLoadType.UTM_MGRS))
            {
                UTM_MGRS = true;
            }
            if (et.HasFlag(EagerLoadType.ECEF))
            {
                ECEF = true;
            }
        }

        /// <summary>
        /// Creates an EagerLoad object. Only the specified flags will be set to EagerLoad.
        /// </summary>
        /// <param name="et">EagerLoadType</param>
        /// <returns>EagerLoad</returns>
        public static EagerLoad Create(EagerLoadType et)
        {
            EagerLoad el = new EagerLoad(et);
            return el;
        }

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
    /// <summary>
    /// EagerLoad Enumerator
    /// </summary>
    [Serializable]
    [Flags]
    public enum EagerLoadType
    {
        /// <summary>
        /// UTM and MGRS
        /// </summary>
        UTM_MGRS = 1,
        /// <summary>
        /// Celestial
        /// </summary>
        Celestial = 2,
        /// <summary>
        /// Cartesian
        /// </summary>
        Cartesian = 4,
        /// <summary>
        /// ECEF
        /// </summary>
        ECEF = 8

    }
}
