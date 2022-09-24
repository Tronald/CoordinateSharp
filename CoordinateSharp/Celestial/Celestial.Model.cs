/*
CoordinateSharp is a .NET standard library that is intended to ease geographic coordinate 
format conversions and location based celestial calculations.
https://github.com/Tronald/CoordinateSharp

Many celestial formulas in this library are based on Jean Meeus's 
Astronomical Algorithms (2nd Edition). Comments that reference only a chapter
are referring to this work.

License

CoordinateSharp is split licensed and may be licensed under the GNU Affero General Public License version 3 or a commercial use license as stated.

Copyright (C) 2022, Signature Group, LLC
  
This program is free software; you can redistribute it and/or modify it under the terms of the GNU Affero General Public License version 3 
as published by the Free Software Foundation with the addition of the following permission added to Section 15 as permitted in Section 7(a): 
FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY Signature Group, LLC. Signature Group, LLC DISCLAIMS THE WARRANTY OF 
NON INFRINGEMENT OF THIRD PARTY RIGHTS.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Affero General Public License for more details. You should have received a copy of the GNU 
Affero General Public License along with this program; if not, see http://www.gnu.org/licenses or write to the 
Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA, 02110-1301 USA, or download the license from the following URL:

https://www.gnu.org/licenses/agpl-3.0.html

The interactive user interfaces in modified source and object code versions of this program must display Appropriate Legal Notices, 
as required under Section 5 of the GNU Affero General Public License.

You can be released from the requirements of the license by purchasing a commercial license. Buying such a license is mandatory 
as soon as you develop commercial activities involving the CoordinateSharp software without disclosing the source code of your own applications. 
These activities include: offering paid services to customers as an ASP, on the fly location based calculations in a web application, 
or shipping CoordinateSharp with a closed source product.

Organizations or use cases that fall under the following conditions may receive a free commercial use license upon request on a case by case basis.


	-Open source contributors to this library.
	-Scholarly or scientific research.
	-Emergency response / management uses.

Please visit http://coordinatesharp.com/licensing or contact Signature Group, LLC to purchase a commercial license, or for any questions regarding the AGPL 3.0 license requirements or free use license: sales@signatgroup.com.
*/
using System;


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

        internal DateTime? solarNoon;

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
        internal Solstices solstices;
        internal Equinoxes equinoxes;

        internal LunarCoordinates lunarCoordinates;
        internal SolarCoordinates solarCoordinates;

        /// <summary>
        /// Sunset time.
        /// </summary>
        /// <remarks>If DateTime is null, check the SunCondition property.</remarks>
        /// <example>
        /// The following example gets the sunset time in UTC at N 39, W 72 on 1-March-2018
        /// <code>
        /// Coordinate coordinate = new Coordinate(39,-72, new DateTime(2018,3,1));
		/// Console.WriteLine(coordinate.CelestialInfo.SunSet); //3/1/2018 10:41:34 PM (UTC)
        /// </code>
        /// 
        /// The following example demonstrates a returned null SunSet value as the sun does not set at N 39, W 72 on 1-March-2018. 
        /// You can see why there is no value returned by checking the SunCondition property to see that the sun is down all day
        /// at the specified location.
        /// <code>
        /// Coordinate coordinate = new Coordinate(85,-72, new DateTime(2018,3,1));
		/// Console.WriteLine(coordinate.CelestialInfo.SunSet); //Returns Null
        /// Console.WriteLine(coordinate.CelestialInfo.SunCondition); //DownAllDay
        /// </code>
        /// 
        /// </example>
        public DateTime? SunSet { get { return sunSet; } }

        /// <summary>
        /// Sunrise time.
        /// </summary>
        /// <remarks>If DateTime is null, check the SunCondition property.</remarks>
        /// <example>
        /// The following example gets the sunrise time in UTC at N 39, W 72 on 1-March-2018
        /// <code>
        /// Coordinate coordinate = new Coordinate(39,-72, new DateTime(2018,3,1));
		/// Console.WriteLine(coordinate.CelestialInfo.SunRise); //3/1/2018 11:22:04 AM (UTC)
        /// </code>
        /// The following example demonstrates a returned null SunRise value as the sun does not rise at N 39, W 72 on 1-March-2018. 
        /// You can see why there is no value returned by checking the SunCondition property to see that the sun is down all day
        /// at the specified location.
        /// <code>
        /// Coordinate coordinate = new Coordinate(85,-72, new DateTime(2018,3,1));
		/// Console.WriteLine(coordinate.CelestialInfo.SunSet); //Returns Null
        /// Console.WriteLine(coordinate.CelestialInfo.SunCondition); //DownAllDay
        /// </code>
        /// </example>
        public DateTime? SunRise { get { return sunRise; } }

        /// <summary>
        /// Solar noon time.
        /// </summary>
        /// <example>
        /// The following example gets the solar noon time in UTC at N 39, W 72 on 1-March-2018
        /// <code>
        /// Coordinate coordinate = new Coordinate(39,-72, new DateTime(2018,3,1));
        /// Console.WriteLine(coordinate.CelestialInfo.SolarNoon); //3/1/2018 5:01:49 PM(UTC)
        /// </code> 
        /// </example>
        public DateTime? SolarNoon{ get { return solarNoon; } }

        /// <summary>
        /// Moon set time.
        /// </summary>
        /// <remarks>If DateTime is null, check the MoonCondition property.</remarks>
        /// <example>
        /// The following example gets the moon set time in UTC at N 39, W 72 on 1-March-2018
        /// <code>
        /// Coordinate coordinate = new Coordinate(39,-72, new DateTime(2018,3,1));
		/// Console.WriteLine(coordinate.CelestialInfo.MoonSet); //3/1/2018 11:12:08 AM (UTC)
        /// </code>
        /// The following example demonstrates a returned null MoonSet value as the moon does not set at N 39, W 72 on 1-March-2018. 
        /// You can see why there is no value returned by checking the MoonCondition property to see that the moon is up all day
        /// at the specified location.
        /// <code>
        /// Coordinate coordinate = new Coordinate(85,-72, new DateTime(2018,3,1));
		/// Console.WriteLine(coordinate.CelestialInfo.MoonSet); //Returns Null
        /// Console.WriteLine(coordinate.CelestialInfo.MoonCondition); //UpAllDay
        /// </code>
        /// </example>
        public DateTime? MoonSet { get { return moonSet; } }
        /// <summary>
        /// Moon rise time.
        /// </summary>
        /// <remarks>If DateTime is null, check the MoonCondition property.</remarks>
        /// <example>
        /// The following example gets the moon rise time in UTC at N 39, W 72 on 1-March-2018
        /// <code>
        /// Coordinate coordinate = new Coordinate(39,-72, new DateTime(2018,3,1));
	    /// Console.WriteLine(coordinate.CelestialInfo.MoonRise); //3/1/2018 10:27:07 PM (UTC)
        /// </code>
        /// The following example demonstrates a returned null MoonRise value as the moon does not rise at N 39, W 72 on 1-March-2018. 
        /// You can see why there is no value returned by checking the MoonCondition property to see that the moon is up all day
        /// at the specified location.
        /// <code>
        /// Coordinate coordinate = new Coordinate(85,-72, new DateTime(2018,3,1));
		/// Console.WriteLine(coordinate.CelestialInfo.MoonRise); //Returns Null
        /// Console.WriteLine(coordinate.CelestialInfo.MoonCondition); //UpAllDay
        /// </code>
        /// </example>
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
        /// Moon altitude in degrees (E of N) (corrected for parallax and refraction).
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
        /// Determine if the moon is currently up, based on moon set and moon rise time at the provided location and date.
        /// </summary>
        public bool IsMoonUp { get { return isMoonUp; } }

        /// <summary>
        /// Moon illumination details based on the provided date.
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
        /// <remarks>Contains dawn, dusk and twilight times.</remarks>
        public AdditionalSolarTimes AdditionalSolarTimes { get { return additionalSolarTimes; } }
        /// <summary>
        /// Astrological signs based on the provided date.
        /// </summary>
        /// <remarks>
        /// Contains zodiac, moon sign and moon name during full moon events.
        /// </remarks>
        public AstrologicalSigns AstrologicalSigns { get { return astrologicalSigns; } }

        /// <summary>
        /// Solar eclipse details.
        /// </summary>
        public SolarEclipse SolarEclipse { get { return solarEclipse; } }
        /// <summary>
        /// Lunar eclipse details.
        /// </summary>
        public LunarEclipse LunarEclipse { get { return lunarEclipse; } }

        /// <summary>
        /// Get solstice values based on provided year.
        /// </summary>
        public Solstices Solstices { get { return solstices; } }
       
        /// <summary>
        /// Get equinox values based on provided year.
        /// </summary>
        public Equinoxes Equinoxes { get { return equinoxes; } }

        /// <summary>
        /// Lunar Coordinates.
        /// </summary>
        public LunarCoordinates LunarCoordinates { get { return lunarCoordinates; } }

        /// <summary>
        /// Solar Coordinates.
        /// </summary>
        public SolarCoordinates SolarCoordinates { get { return solarCoordinates; } }
    }
}
