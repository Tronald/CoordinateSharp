/*
CoordinateSharp is a .NET standard library that is intended to ease geographic coordinate 
format conversions and location based celestial calculations.
https://github.com/Tronald/CoordinateSharp

Many celestial formulas in this library are based on Jean Meeus's 
Astronomical Algorithms (2nd Edition). Comments that reference only a chapter
are referring to this work.

License

CoordinateSharp is split licensed and may be licensed under the GNU Affero General Public License version 3 or a commercial use license as stated.

Copyright (C) 2019, Signature Group, LLC
  
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

Organizations or use cases that fall under the following conditions may receive a free commercial use license upon request.
-Department of Defense
-Department of Homeland Security
-Open source contributors to this library
-Scholarly or scientific uses on a case by case basis.
-Emergency response / management uses on a case by case basis.

For more information, please contact Signature Group, LLC at this address: sales@signatgroup.com
*/
using System;
using System.Collections.Generic;
namespace CoordinateSharp
{ 
    public partial class Celestial
    {
        //When a rise or a set does not occur, the DateTime will return null
        /// <summary>
        /// Creates an empty Celestial.
        /// </summary>
        public Celestial()
        {
            Create_Properties();
            CalculateCelestialTime(0, 0, new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc), new EagerLoad());                     
        }
        /// <summary>
        /// Creates a Celestial object based on a location and specified date.
        /// </summary>
        /// <param name="lat">Latitude</param>
        /// <param name="longi">Longitude</param>
        /// <param name="geoDate">DateTime (UTC)</param>
        /// <remarks>
        /// Celestial information is normally populated within the Coordinate classes CelestialInfo property. 
        /// However, you may choose to work directly within the Celestial class.
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to get the sunset time at Seattle on 19-Mar-2019
        /// directly from a Celestial object.
        /// <code>
        /// //Create a Celestial object the calculates from Seattle's signed lat/long on
        /// //19-Mar-2019 (UTC) Date
        /// Celestial cel = new Celestial(47.60357, -122.32945, new DateTime(2019, 3, 19));
		///
		/// //Check if a sunset will occur on the specified day.
		/// if(cel.SunSet.HasValue)
		/// {
		///     Console.WriteLine(cel.SunSet.Value); //3/19/2019 2:19:31 AM
		/// }
        /// </code>
        /// </example>
        public Celestial(double lat, double longi, DateTime geoDate)
        {
            DateTime d = new DateTime(geoDate.Year, geoDate.Month, geoDate.Day, geoDate.Hour, geoDate.Minute, geoDate.Second, DateTimeKind.Utc);
            Create_Properties();
            CalculateCelestialTime(lat, longi, d, new EagerLoad());
        }
        /// <summary>
        /// Creates a Celestial object based on a location and specified date.
        /// </summary>
        /// <param name="lat">Latitude</param>
        /// <param name="longi">Longitude</param>
        /// <param name="geoDate">DateTime (UTC)</param>
        /// <param name="offset">UTC offset in hours</param>
        /// <remarks>
        /// Celestial information is normally populated within the Coordinate classes CelestialInfo property. 
        /// However, you may choose to work directly within the Celestial class.
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to get the sunset time at Seattle on 19-Mar-2019
        /// directly from a Celestial object in local time.
        /// <code>
        /// //Create a Celestial object the calculates from Seattle's signed lat/long on
        /// //19-Mar-2019 (UTC) Date. Seattle is -7 UTC on this date.
        /// Celestial cel = new Celestial(47.60357, -122.32945, new DateTime(2019, 3, 19), -7);
        ///
        /// //Check if a sunset will occur on the specified day.
        /// if(cel.SunSet.HasValue)
        /// {
        ///     Console.WriteLine(cel.SunSet.Value); //3/19/2019 7:20:56 PM
        /// }
        /// </code>
        /// </example>
        public Celestial(double lat, double longi, DateTime geoDate, double offset)
        {
            DateTime d = new DateTime(geoDate.Year, geoDate.Month, geoDate.Day, geoDate.Hour, geoDate.Minute, geoDate.Second, DateTimeKind.Utc);
            Create_Properties();
            CalculateCelestialTime(lat, longi, d, new EagerLoad(), offset);
        }
        /// <summary>
        /// Creates a Celestial object based on a location and specified date.
        /// </summary>
        /// <param name="lat">Latitude</param>
        /// <param name="longi">Longitude</param>
        /// <param name="geoDate">DateTime (UTC)</param>
        /// <param name="offset">UTC offset in hours</param>
        /// <param name="el">EagerLoad</param>
        /// <remarks>
        /// Celestial information is normally populated within the Coordinate classes CelestialInfo property. 
        /// However, you may choose to work directly within the Celestial class.
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to get the sunset time at Seattle on 19-Mar-2019
        /// directly from a Celestial object in local time populated only with solar cycle information.
        /// <code>
        /// //Create EagerLoading object to load only solar cycle data for maximum efficiency.
        /// EagerLoad el = new EagerLoad(EagerLoadType.Celestial);
        /// el.Extensions = new EagerLoad_Extensions(EagerLoad_ExtensionsType.Solar_Cycle);
        /// 
        /// //Create a Celestial object the calculates from Seattle's signed lat/long on
        /// //19-Mar-2019 (UTC) Date. Seattle is -7 UTC on this date.
        /// Celestial cel = new Celestial(47.60357, -122.32945, new DateTime(2019, 3, 19), -7, el);
        ///
        /// //Check if a sunset will occur on the specified day.
        /// if(cel.SunSet.HasValue)
        /// {
        ///     Console.WriteLine(cel.SunSet.Value); //3/19/2019 7:20:56 PM
        /// }
        /// </code>
        /// </example>
        public Celestial(double lat, double longi, DateTime geoDate, double offset, EagerLoad el)
        {
            DateTime d = new DateTime(geoDate.Year, geoDate.Month, geoDate.Day, geoDate.Hour, geoDate.Minute, geoDate.Second, DateTimeKind.Utc);
            Create_Properties();
            CalculateCelestialTime(lat, longi, d, el, offset);
        }

        /// Used to create empty Celestial objects.
        internal Celestial(bool hasCalcs)
        {
            Create_Properties();
            if (hasCalcs) { CalculateCelestialTime(0, 0, new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc), new EagerLoad()); }
        }

        //Creates empty properties
        private void Create_Properties()
        {
            astrologicalSigns = new AstrologicalSigns();
            lunarEclipse = new LunarEclipse();
            solarEclipse = new SolarEclipse();
            solstices = new Solstices();
            equinoxes = new Equinoxes();
        }
        
        /// <summary>
        /// Calculates all celestial data. Coordinates will notify as changes occur
        /// </summary>
        /// <param name="lat">Decimal format latitude</param>
        /// <param name="longi">Decimal format longitude</param>
        /// <param name="date">Geographic DateTime</param>
        /// <param name="el">EagerLoading Info for Auto-Calculations</param>
        internal void CalculateCelestialTime(double lat, double longi, DateTime date, EagerLoad el)
        {
            CalculateCelestialTime(lat, longi, date, el, 0);
        }
        /// <summary>
        /// Calculates all celestial data. Coordinates will notify as changes occur
        /// </summary>
        /// <param name="lat">Decimal format latitude</param>
        /// <param name="longi">Decimal format longitude</param>
        /// <param name="date">Geographic DateTime</param>
        /// <param name="el">EagerLoading Info for Auto-Calculations</param>
        /// <param name="offset">UTC offset in hours</param>
        internal void CalculateCelestialTime(double lat, double longi, DateTime date, EagerLoad el, double offset)
        {
            if (offset < -12 || offset > 12) { throw new ArgumentOutOfRangeException("Time offsets cannot be greater than 12 or less than -12."); }

            date = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, DateTimeKind.Utc);

            if (el.Extensions.Solar_Cycle || el.Extensions.Solar_Eclipse || el.Extensions.Solstice_Equinox)
            {
                SunCalc.CalculateSunTime(lat, longi, date, this, el, offset);
            }
            if (el.Extensions.Lunar_Cycle)
            {
                MoonCalc.GetMoonTimes(date, lat, longi, this, offset);
                MoonCalc.GetMoonDistance(date, this, offset);
                

                perigee = MoonCalc.GetPerigeeEvents(date);
                apogee = MoonCalc.GetApogeeEvents(date);

                //Shift perigee / apogee is working outside UTC
                if (offset != 0)
                {
                    perigee.ConvertTo_Local_Time(offset);
                    apogee.ConvertTo_Local_Time(offset);
                }
            }

            if (el.Extensions.Lunar_Cycle || el.Extensions.Zodiac || el.Extensions.Lunar_Eclipse)
            {
                MoonCalc.GetMoonIllumination(date, this, lat, longi, el, offset);
            }

            if (el.Extensions.Zodiac)
            {
                SunCalc.CalculateZodiacSign(date, this);
                MoonCalc.GetMoonSign(date, this);
            }

            if (el.Extensions.Lunar_Cycle || el.Extensions.Solar_Cycle)
            {
                Calculate_Celestial_IsUp_Booleans(date, this);
            }

            //Shift eclipses if eagerloaded and offset is not 0
            if (el.Extensions.Lunar_Eclipse && offset!=0)
            {            
                lunarEclipse.ConvertTo_LocalTime(offset);
            }
            if (el.Extensions.Solar_Eclipse && offset!=0)
            {
                solarEclipse.ConvertTo_LocalTime(offset);
            }
        }
  

        /// <summary>
        /// In place time slip
        /// </summary>
        /// <param name="c">Coordinate</param>
        /// <param name="offset">hour offset</param>
        /// <param name="el">Celestial EagerLoad Option</param>
        private void Local_Convert(Coordinate c, double offset, Celestial_EagerLoad el)
        {
            //Find new lunar set rise times
            if (el == Celestial_EagerLoad.All || el == Celestial_EagerLoad.Lunar)
            {
                if (MoonSet.HasValue) { moonSet = moonSet.Value.AddHours(offset); }
                if (MoonRise.HasValue) { moonRise = moonRise.Value.AddHours(offset); }

                Perigee.ConvertTo_Local_Time(offset);
                Apogee.ConvertTo_Local_Time(offset);
                LunarEclipse.ConvertTo_LocalTime(offset);
                MoonCalc.GetMoonSign(c.GeoDate.AddHours(offset), this);
            }

            ////Solar
            if (el == Celestial_EagerLoad.All || el == Celestial_EagerLoad.Solar)
            {
                if (sunSet.HasValue) { sunSet = sunSet.Value.AddHours(offset); }
                if (SunRise.HasValue) { sunRise = SunRise.Value.AddHours(offset); }
                AdditionalSolarTimes.Convert_To_Local_Time(offset);

                //Eclipse
                SolarEclipse.ConvertTo_LocalTime(offset);
                SunCalc.CalculateZodiacSign(c.GeoDate.AddHours(offset), this);
            }
        }
    }

}
