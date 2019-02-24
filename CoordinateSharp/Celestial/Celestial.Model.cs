using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoordinateSharp
{
    /// <summary>
    /// The main class for handling location based celestial information.
    /// </summary>
    /// <remarks>
    /// This class can calculate various pieces of solar and lunar data, based on location and date
    /// </remarks>
    [Serializable]
    public partial class Celestial
    { 
        internal DateTime? sunSet;
        internal DateTime? sunRise;
        internal DateTime? moonSet;
        internal DateTime? moonRise;

        internal double sunAltitude;
        internal double sunAzimuth;
        internal double moonAltitude;
        internal double moonAzimuth;

        internal Distance moonDistance;

        internal CelestialStatus sunCondition;
        internal CelestialStatus moonCondition;

        internal bool isSunUp;
        internal bool isMoonUp;

        internal MoonIllum moonIllum;
        internal Perigee perigee;
        internal Apogee apogee;
        internal AdditionalSolarTimes additionalSolarTimes;
        internal AstrologicalSigns astrologicalSigns;
        internal SolarEclipse solarEclipse;
        internal LunarEclipse lunarEclipse;

        /// <summary>
        /// Sunset time.
        /// </summary>
        public DateTime? SunSet { get { return sunSet; } }
        /// <summary>
        /// Sunrise time.
        /// </summary>
        public DateTime? SunRise { get { return sunRise; } }
        /// <summary>
        /// Moonset time.
        /// </summary>
        public DateTime? MoonSet { get { return moonSet; } }
        /// <summary>
        /// Moonrise time.
        /// </summary>
        public DateTime? MoonRise { get { return moonRise; } }

        /// <summary>
        /// Sun altitude in degrees (E of N).
        /// </summary>
        public double SunAltitude { get { return sunAltitude; } }
        /// <summary>
        /// Sun azimuth in degrees (E of N).
        /// </summary>
        public double SunAzimuth { get { return sunAzimuth; } }
        /// <summary>
        /// Moon altitude in degrees (corrected for parallax and refraction).
        /// </summary>
        public double MoonAltitude { get { return moonAltitude; } }
        /// <summary>
        /// Moon azimuth in degrees (E of N).
        /// </summary>
        public double MoonAzimuth { get { return moonAzimuth; } }

        /// <summary>
        /// Estimated moon distance from the earth.
        /// </summary>
        public Distance MoonDistance { get { return moonDistance; } }
        /// <summary>
        /// Sun's Condition based on the provided date.
        /// </summary>
        public CelestialStatus SunCondition { get { return sunCondition; } }
        /// <summary>
        /// Moon's condition based on the provided date.
        /// </summary>
        public CelestialStatus MoonCondition { get { return moonCondition; } }

        /// <summary>
        /// Determine if the sun is currently up, based on sunset and sunrise time at the provided location and date.
        /// </summary>
        public bool IsSunUp { get { return isSunUp; } }

        /// <summary>
        /// Determine if the moon is currently up, based on moonset and moonrise time at the provided location and date.
        /// </summary>
        public bool IsMoonUp { get { return isMoonUp; } }

        /// <summary>
        /// Moon ilumination details based on the provided date.
        /// </summary>
        /// <remarks>
        /// Contains phase, phase name, fraction and angle
        /// </remarks>
        public MoonIllum MoonIllum { get { return moonIllum; } }
        /// <summary>
        /// Moons perigee details based on the provided date.
        /// </summary>
        public Perigee Perigee { get { return perigee; } }
        /// <summary>
        /// Moons apogee details based on the provided date.
        /// </summary>
        public Apogee Apogee { get { return apogee; } }

        /// <summary>
        /// Additional solar event times based on the provided date and location.
        /// </summary>
        /// <remarks>Contains civil and nautical dawn and dusk times.</remarks>
        public AdditionalSolarTimes AdditionalSolarTimes { get { return additionalSolarTimes; } }
        /// <summary>
        /// Astrological signs based on the provided date.
        /// </summary>
        /// <remarks>
        /// Contains zodiac, moon sign and moon name during full moon events
        /// </remarks>
        public AstrologicalSigns AstrologicalSigns { get { return astrologicalSigns; } }

        /// <summary>
        /// Returns a SolarEclipse.
        /// </summary>
        public SolarEclipse SolarEclipse { get { return solarEclipse; } }
        /// <summary>
        /// Returns a LunarEclipse.
        /// </summary>
        public LunarEclipse LunarEclipse { get { return lunarEclipse; } }
    }
}
