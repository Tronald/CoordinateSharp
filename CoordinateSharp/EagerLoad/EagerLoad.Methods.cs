using System;

namespace CoordinateSharp
{
    public partial class EagerLoad
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
    }
   
}
