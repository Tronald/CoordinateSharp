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
using System.Linq;
using System.Text;

namespace CoordinateSharp
{
    public partial class Celestial
    {
        /// <summary>
        /// Creates a Celestial object based on a Coordinate.
        /// </summary>
        /// <param name="coord">Coordinate</param>
        internal static Celestial LoadCelestial(Coordinate coord)
        {
            DateTime geoDate = coord.GeoDate;

            DateTime d = new DateTime(geoDate.Year, geoDate.Month, geoDate.Day, geoDate.Hour, geoDate.Minute, geoDate.Second, DateTimeKind.Utc);
            Celestial cel = new Celestial(coord.Latitude.ToDouble(), coord.Longitude.ToDouble(), coord.GeoDate);

            return cel;
        }

        /// <summary>
        /// Calculate celestial data based on latitude, longitude and UTC date at the location.
        /// </summary>
        /// <param name="lat">Decimal format latitude</param>
        /// <param name="longi">Decimal format longitude</param>
        /// <param name="date">Geographic DateTime</param>
        /// <returns>Celestial (Fully Populated)</returns>
        /// <example>
        /// The following example demonstrates how to create a fully populated Celestial object 
        /// using static functions.
        /// <code>
        /// //Get Celestial data at N 39, W 72 on 19-Mar-2019 10:10:12 UTC
        /// Celestial cel = Celestial.CalculateCelestialTimes(39, -72, new DateTime(2019, 3, 19, 10, 10, 12));
        /// 
        /// Console.WriteLine(cel.SunRise); //3/19/2019 10:54:50 AM
        /// Console.WriteLine(cel.MoonRise); //3/19/2019 9:27:27 PM
        /// </code>
        /// </example>
        public static Celestial CalculateCelestialTimes(double lat, double longi, DateTime date)
        {
            Celestial c = new Celestial(lat, longi, date);
            return c;
        }
        /// <summary>
        /// Calculate celestial data based on latitude, longitude and UTC date at the location.
        /// </summary>
        /// <param name="lat">Decimal format latitude</param>
        /// <param name="longi">Decimal format longitude</param>
        /// <param name="date">Geographic DateTime</param>
        /// <param name="el">EagerLoad</param>
        /// <returns>Celestial</returns>
        /// <example>
        /// The following example demonstrates how to create a fully populated Celestial object 
        /// using static functions with EagerLoading solar cycle information only.
        /// <code>
        /// //Set EagerLoading parameters to only load solar cycle data for maximum efficiency
        /// EagerLoad el = new EagerLoad(EagerLoadType.Celestial);
        /// el.Extensions = new EagerLoad_Extensions(EagerLoad_ExtensionsType.Solar_Cycle);
        /// 
        /// //Get Celestial data at N 39, W 72 on 19-Mar-2019 10:10:12 UTC
        /// Celestial cel = Celestial.CalculateCelestialTimes(39, -72, new DateTime(2019, 3, 19, 10, 10, 12), el);
        ///
        /// Console.WriteLine(cel.SunRise); //3/19/2019 10:54:50 AM
        /// </code>
        /// </example>
        public static Celestial CalculateCelestialTimes(double lat, double longi, DateTime date, EagerLoad el)
        {
            Celestial c = new Celestial(lat, longi, date, 0, el);
            return c;
        }
        /// <summary>
        /// Calculate celestial data based on latitude, longitude and UTC date with hours offset at the location.
        /// </summary>
        /// <param name="lat">Decimal format latitude</param>
        /// <param name="longi">Decimal format longitude</param>
        /// <param name="date">Geographic DateTime</param>
        /// <param name="el">EagerLoad</param>
        /// <param name="offset">Offset hours</param>
        /// <returns>Celestial</returns>
        /// <example>
        /// The following example demonstrates how to create a fully populated Celestial object in local time
        /// using static functions using solar cycle only eager loading.
        /// <code>
        /// // Set EagerLoading parameters to only load solar cycle data for maximum efficiency
        /// EagerLoad el = new EagerLoad(EagerLoadType.Celestial);
        /// el.Extensions = new EagerLoad_Extensions(EagerLoad_ExtensionsType.Solar_Cycle);
        /// 
        /// //Get Celestial data at N 39, W 72 on 19-Mar-2019 10:10:12 Local using Pacific Standard Time offset in hours (-7)
        /// Celestial cel = Celestial.CalculateCelestialTimes(39, -72, new DateTime(2019, 3, 19, 10, 10, 12), el, -7);
        /// Console.WriteLine(cel.SunRise); //03:54:50 AM
        /// 
        /// </code>
        /// </example>
        public static Celestial CalculateCelestialTimes(double lat, double longi, DateTime date, EagerLoad el, double offset)
        {
            Celestial c = new Celestial(lat, longi, date, offset, el);
            return c;
        }
        /// <summary>
        /// Calculate celestial data based on latitude, longitude and UTC date with hours offset at the location.
        /// </summary>
        /// <param name="lat">Decimal format latitude</param>
        /// <param name="longi">Decimal format longitude</param>
        /// <param name="date">Geographic DateTime</param>
        /// <param name="offset">Offset hours</param>
        /// <returns>Celestial</returns>
        /// <example>
        /// The following example demonstrates how to create a fully populated Celestial object in local time
        /// using static functions.
        /// <code>
        /// //Get Celestial data at N 39, W 72 on 19-Mar-2019 10:10:12 Local using Pacific Standard Time offset in hours (-7)
        /// Celestial cel = Celestial.CalculateCelestialTimes(39, -72, new DateTime(2019, 3, 19, 10, 10, 12), -7);
        /// Console.WriteLine(cel.SunRise); //03:54:50 AM
        /// 
        /// </code>
        /// </example>
        public static Celestial CalculateCelestialTimes(double lat, double longi, DateTime date, double offset)
        {
            Celestial c = new Celestial(lat, longi, date, offset, new EagerLoad());
            return c;
        }

        /// <summary>
        /// Returns a List containing solar eclipse data for the century at the specified location.
        /// Century returned is based on the date passed.
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="longi">longitude</param>
        /// <param name="date">DateTime</param>
        /// <returns>List&gt;SolarEclipseDetails&gt;</returns>
        /// <example>
        /// The following example gets a Solar Eclipse table for the 20th century at N 39, W 72
        /// and displays each type of eclipse and occurrence date in that century.
        /// <code>
        /// List&gt;SolarEclipseDetails&gt; seList = Celestial.Get_Solar_Eclipse_Table(39, -72, new DateTime(1950, 1, 1));
        ///
        /// foreach(SolarEclipseDetails sd in seList)
        /// {
        ///	    Console.WriteLine(sd.Type + " " + sd.Date.ToShortDateString());
        /// }
        /// 
        /// //Partial 8/30/1905
        /// //Partial 6/28/1908
        /// //Partial 6/18/1909
        /// //Partial 4/17/1912
        /// //Partial 2/3/1916
        /// //Partial 6/7/1918
        /// //Partial 11/22/1919
        /// ...      
        /// </code>
        /// </example>
        public static List<SolarEclipseDetails> Get_Solar_Eclipse_Table(double lat, double longi, DateTime date)
        {
            //Convert to Radians
            double latR = lat * Math.PI / 180;
            double longR = longi * Math.PI / 180;
            //Get solar data based on date
            double[] events = Eclipse.SolarData.SolarDateData_100Year(date);
            //Return list of solar data.
            return SolarEclipseCalc.CalculateSolarEclipse(date, latR, longR, events);
        }
        /// <summary>
        /// Returns a List containing solar eclipse data for the century at the specified location.
        /// Century return is based on the date passed.
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="longi">longitude</param>
        /// <param name="date">DateTime</param>
        /// <returns>List&gt;LunarEclipseDetails&gt;</returns>
        ///  /// <example>
        /// The following example gets a Lunar Eclipse table for the 20th century at N 39, W 72
        /// and displays each type of eclipse and occurrence date in that century.
        /// <code>
        /// List&gt;LunarEclipseDetails&gt; leList = Celestial.Get_Lunar_Eclipse_Table(39, -72, new DateTime(1950, 1, 1));
        ///
        /// foreach(LunarEclipseDetails ld in leList)
        /// {
        ///	    Console.WriteLine(ld.Type + " " + ld.Date.ToShortDateString());
        /// }
        /// 
        /// //Total 10/17/1902
        /// //Partial 4/11/1903
        /// //Penumbral 3/2/1904
        /// //Partial 8/15/1905
        /// //Total 2/9/1906
        /// //Partial 1/29/1907
        /// //Partial 7/25/1907
        /// ...      
        /// </code>
        /// </example>
        public static List<LunarEclipseDetails> Get_Lunar_Eclipse_Table(double lat, double longi, DateTime date)
        {
            //Convert to Radians
            double latR = lat * Math.PI / 180;
            double longR = longi * Math.PI / 180;
            //Get solar data based on date
            double[] events = Eclipse.LunarData.LunarDateData_100Year(date);
            //Return list of solar data.
            return LunarEclipseCalc.CalculateLunarEclipse(date, latR, longR, events);
        }

        /// <summary>
        /// Set boolean SunIsUp and MoonIsUp values
        /// </summary>
        /// <param name="date">Coordinate GeoDate</param>
        /// <param name="cel">Celestial Object</param>
        private static void Calculate_Celestial_IsUp_Booleans(DateTime date, Celestial cel)
        {
            //SUN
            switch (cel.SunCondition)
            {
                case CelestialStatus.DownAllDay:
                    cel.isSunUp = false;
                    break;
                case CelestialStatus.UpAllDay:
                    cel.isSunUp = true;
                    break;
                case CelestialStatus.NoRise:
                    if (date < cel.SunSet)
                    {
                        cel.isSunUp = true;
                    }
                    else { cel.isSunUp = false; }
                    break;
                case CelestialStatus.NoSet:
                    if (date > cel.SunRise)
                    {
                        cel.isSunUp = true;
                    }
                    else { cel.isSunUp = false; }
                    break;
                case CelestialStatus.RiseAndSet:
                    if (cel.SunRise < cel.SunSet)
                    {
                        if (date > cel.SunRise && date < cel.SunSet)
                        {
                            cel.isSunUp = true;
                        }
                        else
                        {
                            cel.isSunUp = false;
                        }
                    }
                    else
                    {
                        if (date > cel.SunRise || date < cel.SunSet)
                        {
                            cel.isSunUp = true;
                        }
                        else
                        {
                            cel.isSunUp = false;
                        }
                    }
                    break;
                default:
                    //Should never be reached unless intended null. If reached, previous calculations failed somewhere.
                    break;
            }

            //MOON
            switch (cel.MoonCondition)
            {
                case CelestialStatus.DownAllDay:
                    cel.isMoonUp = false;
                    break;
                case CelestialStatus.UpAllDay:
                    cel.isMoonUp = true;
                    break;
                case CelestialStatus.NoRise:
                    if (date < cel.MoonSet)
                    {
                        cel.isMoonUp = true;
                    }
                    else { cel.isMoonUp = false; }
                    break;
                case CelestialStatus.NoSet:
                    if (date > cel.MoonRise)
                    {
                        cel.isMoonUp = true;
                    }
                    else { cel.isMoonUp = false; }
                    break;
                case CelestialStatus.RiseAndSet:
                    if (cel.MoonRise < cel.MoonSet)
                    {
                        if (date > cel.MoonRise && date < cel.MoonSet)
                        {
                            cel.isMoonUp = true;
                        }
                        else
                        {
                            cel.isMoonUp = false;
                        }
                    }
                    else
                    {
                        if (date > cel.MoonRise || date < cel.MoonSet)
                        {
                            cel.isMoonUp = true;
                        }
                        else
                        {
                            cel.isMoonUp = false;
                        }
                    }
                    break;
                default:
                    //Should never be reached unless intended null. If reached, previous calculations failed somewhere.
                    break;
            }
        }

        /// <summary>
        /// Returns Apogee object containing last and next apogees based on the specified UTC date.
        /// </summary>
        /// <param name="d">DateTime</param>
        /// <returns>Apogee</returns>
        /// <example>
        /// The following example gets the last and next lunar apogees from the specified date
        /// and display's their DateTime and Distance.
        /// <code>
        /// Apogee apogee = Celestial.GetApogees(new DateTime(2019,3,1));
        ///
        /// Console.WriteLine(apogee.LastApogee.Date + " " + apogee.LastApogee.Distance.Kilometers); //2/5/2019 9:27:28 AM 406551.526207563
		/// Console.WriteLine(apogee.NextApogee.Date + " " + apogee.NextApogee.Distance.Kilometers); //3/4/2019 11:26:35 AM 406387.933655865
        /// </code>
        /// </example>
        public static Apogee GetApogees(DateTime d)
        {
            return MoonCalc.GetApogeeEvents(d);
        }
        /// <summary>
        /// Returns Perigee object containing last and next perigees based on the specified UTC date.
        /// </summary>
        /// <param name="d">DateTime</param>
        /// <returns>Perigee</returns>
        /// <example>
        /// The following example gets the last and next lunar perigees from the specified date
        /// and display's their DateTime and Distance.
        /// <code>
        /// Perigee perigee = Celestial.GetPerigees(new DateTime(2019, 3, 1));
        ///
        /// Console.WriteLine(perigee.LastPerigee.Date + " " + perigee.LastPerigee.Distance.Kilometers); //2/19/2019 9:06:55 AM 356762.812526435
		/// Console.WriteLine(perigee.NextPerigee.Date + " " + perigee.NextPerigee.Distance.Kilometers); //3/19/2019 7:48:22 PM 359378.005775414
        /// </code>
        /// </example>
        public static Perigee GetPerigees(DateTime d)
        {
            return MoonCalc.GetPerigeeEvents(d);
        }

        /// <summary>
        /// Gets the next sunset from the provided point in time at the passed location. 
        /// </summary>
        /// <param name="coordinate">Coordinate</param>
        /// <returns>DateTime</returns>
        /// <example>
        /// The following example gets the next sunset from the point in time at the provided location.
        /// <code>
        /// DateTime d = new DateTime(2019, 2, 6);
        /// Coordinate c = new Coordinate(40.0352, -74.5844, d);
        /// 
        /// //JBMDL
        /// DateTime sun = Celestial.Get_Next_SunSet(c); //2/6/2019 10:23:54 PM (UTC)
        /// </code>
        /// </example>
        public static DateTime Get_Next_SunSet(Coordinate coordinate)
        {
            return Get_Next_SunSet(coordinate.Latitude.ToDouble(), coordinate.Longitude.ToDouble(), coordinate.GeoDate, coordinate.Offset);
        }
        /// <summary>
        /// Gets the next sunset from the provided point in time at the passed location. 
        /// </summary>
        /// <param name="lat">Latitude</param>
        /// <param name="longi">Longitude</param>
        /// <param name="geoDate">DateTime at Location</param>   
        /// <returns>DateTime</returns>
        /// <example>
        /// The following example gets the next sunset from the point in time at the provided location.
        /// <code>
        /// DateTime d = new DateTime(2019, 2, 6);
        /// 
        /// //JBMDL
        /// DateTime sun = Celestial.Get_Next_SunSet(40.0352, -74.5844, d); //2/6/2019 10:23:54 PM (UTC)
        /// </code>
        /// </example>
        public static DateTime Get_Next_SunSet(double lat, double longi, DateTime geoDate)
        {
            return Get_Next_SunSet(lat, longi, geoDate, 0);
        }
        /// <summary>
        /// Gets the next sunset from the provided point in time at the passed location. 
        /// </summary>
        /// <param name="lat">Latitude</param>
        /// <param name="longi">Longitude</param>
        /// <param name="geoDate">DateTime at Location</param>
        /// <param name="offset">UTC Offset</param>
        /// <returns>DateTime</returns>
        /// <example>
        /// The following example gets the next sunset from the point in time at the provided location using a UTC offset to convert the time to local.
        /// <code>
        /// DateTime d = new DateTime(2019, 2, 6);
        /// 
        /// //JBMDL (EST TIME)
        /// DateTime sun = Celestial.Get_Next_SunSet(40.0352, -74.5844, d, -4); //2/6/2019 6:23:54 PM (EST)
        /// </code>
        /// </example>
        public static DateTime Get_Next_SunSet(double lat, double longi, DateTime geoDate, double offset)
        {
            //Only run solar cycles for max efficiency
            var el = new EagerLoad(EagerLoadType.Celestial);
            el.Extensions = new EagerLoad_Extensions(EagerLoad_ExtensionsType.Solar_Cycle);

            DateTime? d = null;

            //Iterate until set has been found
            int x = 0;
            while (!d.HasValue)
            {
                var cel = new Celestial(lat, longi, geoDate.AddDays(x), offset, el);
                if (cel.SunSet >= geoDate)//If sunset occurs before the date, continue to next iteration
                {
                    d = cel.SunSet;
                }
                x++;
            }

            return d.Value;
        }

        /// <summary>
        /// Gets the last sunset from the provided point in time at the passed location. 
        /// </summary>
        /// <param name="coordinate">Coordinate</param>
        /// <returns>DateTime</returns>
        /// <example>
        /// The following example gets the last sunset from the point in time at the provided location.
        /// <code>
        /// DateTime d = new DateTime(2019, 2, 6);
        /// Coordinate c = new Coordinate(40.0352, -74.5844, d);
        /// 
        /// //JBMDL
        /// DateTime sun = Celestial.Get_Last_SunSet(c); //2/5/2019 10:22:41 PM (UTC)
        /// </code>
        /// </example>
        public static DateTime Get_Last_SunSet(Coordinate coordinate)
        {
            return Get_Last_SunSet(coordinate.Latitude.ToDouble(), coordinate.Longitude.ToDouble(), coordinate.GeoDate, coordinate.Offset);
        }
        /// <summary>
        /// Gets the last sunset from the provided point in time at the passed location. 
        /// </summary>
        /// <param name="lat">Latitude</param>
        /// <param name="longi">Longitude</param>
        /// <param name="geoDate">DateTime at Location</param>   
        /// <returns>DateTime</returns>
        /// <example>
        /// The following example gets the last sunset from the point in time at the provided location.
        /// <code>
        /// DateTime d = new DateTime(2019, 2, 6);
        /// 
        /// //JBMDL
        /// DateTime sun = Celestial.Get_Last_SunSet(40.0352, -74.5844, d); //2/5/2019 10:22:41 PM (UTC)
        /// </code>
        /// </example>
        public static DateTime Get_Last_SunSet(double lat, double longi, DateTime geoDate)
        {
            return Get_Last_SunSet(lat, longi, geoDate, 0);
        }
        /// <summary>
        /// Gets the last sunset from the provided point in time at the passed location. 
        /// </summary>
        /// <param name="lat">Latitude</param>
        /// <param name="longi">Longitude</param>
        /// <param name="geoDate">DateTime at Location</param>
        /// <param name="offset">UTC Offset</param>
        /// <returns>DateTime</returns>
        /// <example>
        /// The following example gets the last sunset from the point in time at the provided location using a UTC offset to convert the time to local.
        /// <code>
        /// DateTime d = new DateTime(2019, 2, 6);
        /// 
        /// //JBMDL (EST TIME)
        /// DateTime sun = Celestial.Get_Last_SunSet(40.0352, -74.5844, d, -4); //2/5/2019 6:22:41 PM (EST)
        /// </code>
        /// </example>
        public static DateTime Get_Last_SunSet(double lat, double longi, DateTime geoDate, double offset)
        {
            //Only run solar cycles for max efficiency
            var el = new EagerLoad(EagerLoadType.Celestial);
            el.Extensions = new EagerLoad_Extensions(EagerLoad_ExtensionsType.Solar_Cycle);

            DateTime? d = null;

            //Iterate until set has been found
            int x = 0;
            while (!d.HasValue)
            {
                var cel = new Celestial(lat, longi, geoDate.AddDays(x), offset, el);
                if (cel.SunSet <= geoDate)//If sunset occurs after the date, continue to next iteration
                {
                    d = cel.SunSet;
                }
                x--;
            }

            return d.Value;
        }

        /// <summary>
        /// Gets the next sunrise from the provided point in time at the passed location. 
        /// </summary>
        /// <param name="coordinate">Coordinate</param>
        /// <returns>DateTime</returns>
        /// <example>
        /// The following example gets the next sunrise from the point in time at the provided location.
        /// <code>
        /// DateTime d = new DateTime(2019, 2, 6);
        /// Coordinate c = new Coordinate(40.0352, -74.5844, d);
        /// 
        /// //JBMDL
        /// DateTime sun = Celestial.Get_Next_SunRise(c); //2/6/2019 12:03:33 PM (UTC)
        /// </code>
        /// </example>
        public static DateTime Get_Next_SunRise(Coordinate coordinate)
        {
            return Get_Next_SunRise(coordinate.Latitude.ToDouble(), coordinate.Longitude.ToDouble(), coordinate.GeoDate, coordinate.Offset);
        }
        /// <summary>
        /// Gets the next sunrise from the provided point in time at the passed location. 
        /// </summary>
        /// <param name="lat">Latitude</param>
        /// <param name="longi">Longitude</param>
        /// <param name="geoDate">DateTime at Location</param>
        /// <returns>DateTime</returns>
        /// <example>
        /// The following example gets the next sunrise from the point in time at the provided location using a UTC offset to convert the time to local.
        /// <code>
        /// DateTime d = new DateTime(2019, 2, 6);
        /// 
        /// //JBMDL
        /// DateTime sun = Celestial.Get_Next_SunRise(40.0352, -74.5844, d); //2/6/2019 12:03:33 PM (UTC)
        /// </code>
        /// </example>
        public static DateTime Get_Next_SunRise(double lat, double longi, DateTime geoDate)
        {
            return Get_Next_SunRise(lat, longi, geoDate, 0);
        }
        /// <summary>
        /// Gets the next sunrise from the provided point in time at the passed location. 
        /// </summary>
        /// <param name="lat">Latitude</param>
        /// <param name="longi">Longitude</param>
        /// <param name="geoDate">DateTime at Location</param>
        /// <param name="offset">UTC Offset</param>
        /// <returns>DateTime</returns>
        /// <example>
        /// The following example gets the next sunrise from the point in time at the provided location using a UTC offset to convert the time to local.
        /// <code>
        /// DateTime d = new DateTime(2019, 2, 6);
        /// 
        /// //JBMDL (EST TIME)
        /// DateTime sun = Celestial.Get_Next_SunRise(40.0352, -74.5844, d, -4); //2/6/2019 8:03:33 AM (EST)
        /// </code>
        /// </example>
        public static DateTime Get_Next_SunRise(double lat, double longi, DateTime geoDate, double offset)
        {
            //Only run solar cycles for max efficiency
            var el = new EagerLoad(EagerLoadType.Celestial);
            el.Extensions = new EagerLoad_Extensions(EagerLoad_ExtensionsType.Solar_Cycle);

            DateTime? d = null;

            //Iterate until set has been found
            int x = 0;
            while (!d.HasValue)
            {
                var cel = new Celestial(lat, longi, geoDate.AddDays(x), offset, el);
                if (cel.SunRise >= geoDate)//If sun rise occurs before the date, continue to next iteration
                {
                    d = cel.SunRise;
                }
                x++;
            }

            return d.Value;
        }

        /// <summary>
        /// Gets the last sunrise from the provided point in time at the passed location. 
        /// </summary>
        /// <param name="coordinate">Coordinate</param>
        /// <returns>DateTime</returns>
        /// <example>
        /// The following example gets the last sunrise from the point in time at the provided location.
        /// <code>
        /// DateTime d = new DateTime(2019, 2, 6);
        /// Coordinate c = new Coordinate(40.0352, -74.5844, d);
        /// 
        /// //JBMDL
        /// DateTime sun = Celestial.Get_Last_SunRise(c); //2/5/2019 12:04:35 PM (UTC)
        /// </code>
        /// </example>
        public static DateTime Get_Last_SunRise(Coordinate coordinate)
        {
            return Get_Last_SunRise(coordinate.Latitude.ToDouble(), coordinate.Longitude.ToDouble(), coordinate.GeoDate, coordinate.Offset);
        }
        /// <summary>
        /// Gets the last sunrise from the provided point in time at the passed location. 
        /// </summary>
        /// <param name="lat">Latitude</param>
        /// <param name="longi">Longitude</param>
        /// <param name="geoDate">DateTime at Location</param>
        /// <returns>DateTime</returns>
        /// <example>
        /// The following example gets the last sunrise from the point in time at the provided location using a UTC offset to convert the time to local.
        /// <code>
        /// DateTime d = new DateTime(2019, 2, 6);
        /// 
        /// //JBMDL
        /// DateTime sun = Celestial.Get_Last_SunRise(40.0352, -74.5844, d); //2/5/2019 12:04:35 PM (UTC)
        /// </code>
        /// </example>
        public static DateTime Get_Last_SunRise(double lat, double longi, DateTime geoDate)
        {
            return Get_Last_SunRise(lat, longi, geoDate, 0);
        }
        /// <summary>
        /// Gets the last sunrise from the provided point in time at the passed location. 
        /// </summary>
        /// <param name="lat">Latitude</param>
        /// <param name="longi">Longitude</param>
        /// <param name="geoDate">DateTime at Location</param>
        /// <param name="offset">UTC Offset</param>
        /// <returns>DateTime</returns>
        /// <example>
        /// The following example gets the last sunrise from the point in time at the provided location using a UTC offset to convert the time to local.
        /// <code>
        /// DateTime d = new DateTime(2019, 2, 6);
        /// 
        /// //JBMDL (EST TIME)
        /// DateTime sun = Celestial.Get_Last_SunRise(40.0352, -74.5844, d, -4); //2/5/2019 8:04:35 AM (EST)
        /// </code>
        /// </example>
        public static DateTime Get_Last_SunRise(double lat, double longi, DateTime geoDate, double offset)
        {
            //Only run solar cycles for max efficiency
            var el = new EagerLoad(EagerLoadType.Celestial);
            el.Extensions = new EagerLoad_Extensions(EagerLoad_ExtensionsType.Solar_Cycle);

            DateTime? d = null;

            //Iterate until set has been found
            int x = 0;
            while (!d.HasValue)
            {
                var cel = new Celestial(lat, longi, geoDate.AddDays(x), offset, el);
                if (cel.SunRise <= geoDate)//If sun rise occurs after the date, continue to next iteration
                {
                    d = cel.SunRise;
                }
                x--;
            }

            return d.Value;
        }

        /// <summary>
        /// Gets the next moon set from the provided point in time at the passed location. 
        /// </summary>
        /// <param name="coordinate">Coordinate</param>
        /// <returns>DateTime</returns>
        /// <example>
        /// The following example gets the next moon set from the point in time at the provided location.
        /// <code>
        /// DateTime d = new DateTime(2019, 2, 6);
        /// Coordinate c = new Coordinate(40.0352, -74.5844, d);
        /// 
        /// //JBMDL
        /// DateTime moon = Celestial.Get_Next_MoonSet(c); //2/7/2019 12:08:33 AM (UTC)
        /// </code>
        /// </example>
        public static DateTime Get_Next_MoonSet(Coordinate coordinate)
        {
            return Get_Next_MoonSet(coordinate.Latitude.ToDouble(), coordinate.Longitude.ToDouble(), coordinate.GeoDate, coordinate.Offset);
        }
        /// <summary>
        /// Gets the next moon set from the provided point in time at the passed location. 
        /// </summary>
        /// <param name="lat">Latitude</param>
        /// <param name="longi">Longitude</param>
        /// <param name="geoDate">DateTime at Location</param>   
        /// <returns>DateTime</returns>
        /// <example>
        /// The following example gets the next moon set from the point in time at the provided location.
        /// <code>
        /// DateTime d = new DateTime(2019, 2, 6);
        /// 
        /// //JBMDL
        /// DateTime moon = Celestial.Get_Next_MoonSet(40.0352, -74.5844, d); //2/7/2019 12:08:33 AM (UTC)
        /// </code>
        /// </example>
        public static DateTime Get_Next_MoonSet(double lat, double longi, DateTime geoDate)
        {
            return Get_Next_MoonSet(lat, longi, geoDate, 0);
        }
        /// <summary>
        /// Gets the next moon set from the provided point in time at the passed location. 
        /// </summary>
        /// <param name="lat">Latitude</param>
        /// <param name="longi">Longitude</param>
        /// <param name="geoDate">DateTime at Location</param>
        /// <param name="offset">UTC Offset</param>
        /// <returns>DateTime</returns>
        /// <example>
        /// The following example gets the next moon set from the point in time at the provided location using a UTC offset to convert the time to local.
        /// <code>
        /// DateTime d = new DateTime(2019, 2, 6);
        /// 
        /// //JBMDL (EST TIME)
        /// DateTime moon = Celestial.Get_Next_MoonSet(40.0352, -74.5844, d, -4); //2/6/2019 8:08:33 PM (EST)
        /// </code>
        /// </example>
        public static DateTime Get_Next_MoonSet(double lat, double longi, DateTime geoDate, double offset)
        {
            //Only run solar cycles for max efficiency
            var el = new EagerLoad(EagerLoadType.Celestial);
            el.Extensions = new EagerLoad_Extensions(EagerLoad_ExtensionsType.Lunar_Cycle);

            DateTime? d = null;

            //Iterate until set has been found
            int x = 0;
            while (!d.HasValue)
            {
                var cel = new Celestial(lat, longi, geoDate.AddDays(x), offset, el);
                if (cel.MoonSet >= geoDate)//If Moon set occurs before the date, continue to next iteration
                {
                    d = cel.MoonSet;
                }
                x++;
            }

            return d.Value;
        }

        /// <summary>
        /// Gets the last moon set from the provided point in time at the passed location. 
        /// </summary>
        /// <param name="coordinate">Coordinate</param>
        /// <returns>DateTime</returns>
        /// <example>
        /// The following example gets the last moon set from the point in time at the provided location.
        /// <code>
        /// DateTime d = new DateTime(2019, 2, 6);
        /// Coordinate c = new Coordinate(40.0352, -74.5844, d);
        /// 
        /// //JBMDL
        /// DateTime moon = Celestial.Get_Last_MoonSet(c); //2/5/2019 11:11:09 PM (UTC)
        /// </code>
        /// </example>
        public static DateTime Get_Last_MoonSet(Coordinate coordinate)
        {
            return Get_Last_MoonSet(coordinate.Latitude.ToDouble(), coordinate.Longitude.ToDouble(), coordinate.GeoDate, coordinate.Offset);
        }
        /// <summary>
        /// Gets the last moon set from the provided point in time at the passed location. 
        /// </summary>
        /// <param name="lat">Latitude</param>
        /// <param name="longi">Longitude</param>
        /// <param name="geoDate">DateTime at Location</param>   
        /// <returns>DateTime</returns>
        /// <example>
        /// The following example gets the last moon set from the point in time at the provided location.
        /// <code>
        /// DateTime d = new DateTime(2019, 2, 6);
        /// 
        /// //JBMDL
        /// DateTime moon = Celestial.Get_Last_MoonSet(40.0352, -74.5844, d); //2/5/2019 11:11:09 PM (UTC)
        /// </code>
        /// </example>
        public static DateTime Get_Last_MoonSet(double lat, double longi, DateTime geoDate)
        {
            return Get_Last_MoonSet(lat, longi, geoDate, 0);
        }
        /// <summary>
        /// Gets the last moon set from the provided point in time at the passed location. 
        /// </summary>
        /// <param name="lat">Latitude</param>
        /// <param name="longi">Longitude</param>
        /// <param name="geoDate">DateTime at Location</param>
        /// <param name="offset">UTC Offset</param>
        /// <returns>DateTime</returns>
        /// <example>
        /// The following example gets the last moon set from the point in time at the provided location using a UTC offset to convert the time to local.
        /// <code>
        /// DateTime d = new DateTime(2019, 2, 6);
        /// 
        /// //JBMDL (EST TIME)
        /// DateTime moon = Celestial.Get_Last_MoonSet(40.0352, -74.5844, d, -4); //2/5/2019 7:11:09 PM (EST)
        /// </code>
        /// </example>
        public static DateTime Get_Last_MoonSet(double lat, double longi, DateTime geoDate, double offset)
        {
            //Only run solar cycles for max efficiency
            var el = new EagerLoad(EagerLoadType.Celestial);
            el.Extensions = new EagerLoad_Extensions(EagerLoad_ExtensionsType.Lunar_Cycle);

            DateTime? d = null;

            //Iterate until set has been found
            int x = 0;
            while (!d.HasValue)
            {
                var cel = new Celestial(lat, longi, geoDate.AddDays(x), offset, el);
                if (cel.MoonSet <= geoDate)//If Moon set occurs after the date, continue to next iteration
                {
                    d = cel.MoonSet;
                }
                x--;
            }

            return d.Value;
        }

        /// <summary>
        /// Gets the next moon rise from the provided point in time at the passed location. 
        /// </summary>
        /// <param name="coordinate">Coordinate</param>
        /// <returns>DateTime</returns>
        /// <example>
        /// The following example gets the next moon rise from the point in time at the provided location.
        /// <code>
        /// DateTime d = new DateTime(2019, 2, 6);
        /// Coordinate c = new Coordinate(40.0352, -74.5844, d);
        /// 
        /// //JBMDL
        /// DateTime moon = Celestial.Get_Next_MoonRise(c); //2/6/2019 1:10:32 PM (UTC)
        /// </code>
        /// </example>
        public static DateTime Get_Next_MoonRise(Coordinate coordinate)
        {
            return Get_Next_MoonRise(coordinate.Latitude.ToDouble(), coordinate.Longitude.ToDouble(), coordinate.GeoDate, coordinate.Offset);
        }
        /// <summary>
        /// Gets the next moon rise from the provided point in time at the passed location. 
        /// </summary>
        /// <param name="lat">Latitude</param>
        /// <param name="longi">Longitude</param>
        /// <param name="geoDate">DateTime at Location</param>   
        /// <returns>DateTime</returns>
        /// <example>
        /// The following example gets the next moon rise from the point in time at the provided location.
        /// <code>
        /// DateTime d = new DateTime(2019, 2, 6);
        /// 
        /// //JBMDL
        /// DateTime moon = Celestial.Get_Next_MoonRise(40.0352, -74.5844, d); //2/6/2019 1:10:32 PM (UTC)
        /// </code>
        /// </example>
        public static DateTime Get_Next_MoonRise(double lat, double longi, DateTime geoDate)
        {
            return Get_Next_MoonRise(lat, longi, geoDate, 0);
        }
        /// <summary>
        /// Gets the next moon rise from the provided point in time at the passed location. 
        /// </summary>
        /// <param name="lat">Latitude</param>
        /// <param name="longi">Longitude</param>
        /// <param name="geoDate">DateTime at Location</param>
        /// <param name="offset">UTC Offset</param>
        /// <returns>DateTime</returns>
        /// <example>
        /// The following example gets the next moon rise from the point in time at the provided location using a UTC offset to convert the time to local.
        /// <code>
        /// DateTime d = new DateTime(2019, 2, 6);
        /// 
        /// //JBMDL (EST TIME)
        /// DateTime moon = Celestial.Get_Next_MoonRise(40.0352, -74.5844, d, -4); //2/6/2019 9:10:32 AM (EST)
        /// </code>
        /// </example>
        public static DateTime Get_Next_MoonRise(double lat, double longi, DateTime geoDate, double offset)
        {
            //Only run solar cycles for max efficiency
            var el = new EagerLoad(EagerLoadType.Celestial);
            el.Extensions = new EagerLoad_Extensions(EagerLoad_ExtensionsType.Lunar_Cycle);

            DateTime? d = null;

            //Iterate until set has been found
            int x = 0;
            while (!d.HasValue)
            {
                var cel = new Celestial(lat, longi, geoDate.AddDays(x), offset, el);
                if (cel.MoonRise >= geoDate)//If Moon rise occurs before the date, continue to next iteration
                {
                    d = cel.MoonRise;
                }
                x++;
            }

            return d.Value;
        }

        /// <summary>
        /// Gets the last moon rise from the provided point in time at the passed location. 
        /// </summary>
        /// <param name="coordinate">Coordinate</param>
        /// <returns>DateTime</returns>
        /// <example>
        /// The following example gets the last moon rise from the point in time at the provided location.
        /// <code>
        /// DateTime d = new DateTime(2019, 2, 6);
        /// Coordinate c = new Coordinate(40.0352, -74.5844, d);
        /// 
        /// //JBMDL
        /// DateTime moon = Celestial.Get_Last_MoonRise(c); //2/5/2019 12:39:02 PM (UTC)
        /// </code>
        /// </example>
        public static DateTime Get_Last_MoonRise(Coordinate coordinate)
        {
            return Get_Last_MoonRise(coordinate.Latitude.ToDouble(), coordinate.Longitude.ToDouble(), coordinate.GeoDate, coordinate.Offset);
        }
        /// <summary>
        /// Gets the last moon rise from the provided point in time at the passed location. 
        /// </summary>
        /// <param name="lat">Latitude</param>
        /// <param name="longi">Longitude</param>
        /// <param name="geoDate">DateTime at Location</param>   
        /// <returns>DateTime</returns>
        /// <example>
        /// The following example gets the last moon rise from the point in time at the provided location.
        /// <code>
        /// DateTime d = new DateTime(2019, 2, 6);
        /// 
        /// //JBMDL
        /// DateTime moon = Celestial.Get_Last_MoonRise(40.0352, -74.5844, d); //2/5/2019 12:39:02 PM (UTC)
        /// </code>
        /// </example>
        public static DateTime Get_Last_MoonRise(double lat, double longi, DateTime geoDate)
        {
            return Get_Last_MoonRise(lat, longi, geoDate, 0);
        }
        /// <summary>
        /// Gets the last moon rise from the provided point in time at the passed location. 
        /// </summary>
        /// <param name="lat">Latitude</param>
        /// <param name="longi">Longitude</param>
        /// <param name="geoDate">DateTime at Location</param>
        /// <param name="offset">UTC Offset</param>
        /// <returns>DateTime</returns>
        /// <example>
        /// The following example gets the last moon rise from the point in time at the provided location using a UTC offset to convert the time to local.
        /// <code>
        /// DateTime d = new DateTime(2019, 2, 6);
        /// 
        /// //JBMDL (EST TIME)
        /// DateTime moon = Celestial.Get_Last_MoonRise(40.0352, -74.5844, d, -4); //2/5/2019 8:39:02 AM (EST)
        /// </code>
        /// </example>
        public static DateTime Get_Last_MoonRise(double lat, double longi, DateTime geoDate, double offset)
        {
            //Only run solar cycles for max efficiency
            var el = new EagerLoad(EagerLoadType.Celestial);
            el.Extensions = new EagerLoad_Extensions(EagerLoad_ExtensionsType.Lunar_Cycle);

            DateTime? d = null;

            //Iterate until set has been found
            int x = 0;
            while (!d.HasValue)
            {
                var cel = new Celestial(lat, longi, geoDate.AddDays(x), offset, el);
                if (cel.MoonRise <= geoDate)//If Moon rise occurs after the date, continue to next iteration
                {
                    d = cel.MoonRise;
                }
                x--;
            }

            return d.Value;
        }


        //Time Slips
        private static DateTime? Get_Correct_Slipped_Date(DateTime? actual, DateTime? pre, DateTime? post, int i)
        {
            switch (i)
            {
                case 0:
                    return pre;
                case 1:
                    return actual;
                case 2:
                    return post;
                default:
                    return null;
            }
        }
        private static int Determine_Slipped_Event_Index(DateTime? actual, DateTime? pre, DateTime? post, DateTime d)
        {

            if (actual.HasValue)
            {
                if (actual.Value.Day != d.Day)
                {
                    if (pre.HasValue)
                    {
                        if (pre.Value.Day == d.Day) { return 0; }
                    }
                    if (post.HasValue)
                    {
                        if (post.Value.Day == d.Day) { return 2; }
                    }
                    return 3;
                }
            }
            else
            {
                if (pre.HasValue)
                {
                    if (pre.Value.Day == d.Day) { return 0; }
                }
                if (post.HasValue)
                {
                    if (post.Value.Day == d.Day) { return 2; }
                }
            }
            return 1;
        }

        #region Obsoletes
        /// <summary>
        /// Converts all Celestial values to local time.
        /// </summary>
        /// <param name="coord">Coordinate</param>
        /// <param name="offset">UTC offset</param>
        /// <returns>Celestial</returns>
        /// <example>
        /// The following example demonstrates how to get Celestial values in Local time.
        /// DateTime offsets are done manually for readability purposes of this example. 
        /// <code>
        /// //Local time 
        ///  DateTime d = new DateTime(2018, 3, 19, 6, 56, 0, 0, DateTimeKind.Local);
        /// 
        /// //EST Time is -4 hours from UTC
        /// double offset = -4;
        /// 
        /// //Convert the date to UTC time
        /// d = d.AddHours(offset*-1);
        /// 
        /// //Create a Coordinate with the UTC time		
        /// Coordinate c = new Coordinate(39.0000, -72.0000, d);
        /// //Create a new Celestial object by converting the existing one to Local
        /// Celestial celestial = Celestial.Celestial_LocalTime(c, offset);
        /// 
        /// Console.WriteLine(celestial.IsSunUp); //True
        /// Console.WriteLine(celestial.SunRise); //3/19/2018 6:54:25 AM
        /// </code>
        /// </example>        
        [Obsolete("Use instance method 'Coordinate.Celestial_LocalTime(double hourOffset)'.")]
        public static Celestial Celestial_LocalTime(Coordinate coord, double offset)
        {
            if (offset < -12 || offset > 12) { throw new ArgumentOutOfRangeException("Time offsets cannot be greater 12 or less than -12."); }
            //Probably need to offset initial UTC date so user can op in local
            //Determine best way to do this.
            DateTime d = coord.GeoDate.AddHours(offset);

            //Get 3 objects for comparison
            Celestial cel = new Celestial(coord.Latitude.ToDouble(), coord.Longitude.ToDouble(), coord.GeoDate);
            Celestial celPre = new Celestial(coord.Latitude.ToDouble(), coord.Longitude.ToDouble(), coord.GeoDate.AddDays(-1));
            Celestial celPost = new Celestial(coord.Latitude.ToDouble(), coord.Longitude.ToDouble(), coord.GeoDate.AddDays(1));

            //Slip objects for comparison. Compare with slipped date. 
            celPre.Local_Convert(coord, offset, Celestial_EagerLoad.All);
            cel.Local_Convert(coord, offset, Celestial_EagerLoad.All);
            celPost.Local_Convert(coord, offset, Celestial_EagerLoad.All);

            //Get SunSet
            int i = Determine_Slipped_Event_Index(cel.SunSet, celPre.SunSet, celPost.SunSet, d);
            cel.sunSet = Get_Correct_Slipped_Date(cel.SunSet, celPre.SunSet, celPost.SunSet, i);
            cel.AdditionalSolarTimes.civilDusk = Get_Correct_Slipped_Date(cel.AdditionalSolarTimes.CivilDusk,
                celPre.AdditionalSolarTimes.CivilDusk, celPost.AdditionalSolarTimes.CivilDusk, i);
            cel.AdditionalSolarTimes.nauticalDusk = Get_Correct_Slipped_Date(cel.AdditionalSolarTimes.NauticalDusk,
               celPre.AdditionalSolarTimes.NauticalDusk, celPost.AdditionalSolarTimes.NauticalDusk, i);
            //Get SunRise
            i = Determine_Slipped_Event_Index(cel.SunRise, celPre.SunRise, celPost.SunRise, d);
            cel.sunRise = Get_Correct_Slipped_Date(cel.SunRise, celPre.SunRise, celPost.SunRise, i);
            cel.AdditionalSolarTimes.civilDawn = Get_Correct_Slipped_Date(cel.AdditionalSolarTimes.CivilDawn,
                celPre.AdditionalSolarTimes.CivilDawn, celPost.AdditionalSolarTimes.CivilDawn, i);
            cel.AdditionalSolarTimes.nauticalDawn = Get_Correct_Slipped_Date(cel.AdditionalSolarTimes.NauticalDawn,
               celPre.AdditionalSolarTimes.NauticalDawn, celPost.AdditionalSolarTimes.NauticalDawn, i);

            //MoonRise
            i = Determine_Slipped_Event_Index(cel.MoonRise, celPre.MoonRise, celPost.MoonRise, d);
            cel.moonRise = Get_Correct_Slipped_Date(cel.MoonRise, celPre.MoonRise, celPost.MoonRise, i);

            //MoonSet
            i = Determine_Slipped_Event_Index(cel.MoonSet, celPre.MoonSet, celPost.MoonSet, d);
            cel.moonSet = Get_Correct_Slipped_Date(cel.MoonSet, celPre.MoonSet, celPost.MoonSet, i);

            //Local Conditions
            CelestialStatus[] cels = new CelestialStatus[]
            {
                celPre.MoonCondition,cel.MoonCondition,celPost.MoonCondition
            };
            cel.moonCondition = GetStatus(cel.MoonRise, cel.MoonSet, cels);
            cels = new CelestialStatus[]
            {
                celPre.SunCondition, cel.SunCondition, celPost.SunCondition
            };
            cel.sunCondition = GetStatus(cel.SunRise, cel.SunSet, cels);

            //Load IsUp values based on local time with populated Celestial
            //If EagerLoading Extensions used, this function will handle null values
            Calculate_Celestial_IsUp_Booleans(d, cel);

            return cel;
        }
        /// <summary>
        /// Converts solar time values to local time. 
        /// </summary>
        /// <param name="coord">Coordinate</param>
        /// <param name="offset">UTC offset</param>
        /// <returns>Celestial</returns>
        /// <example>
        /// The following example demonstrates how to get Celestial, solar time only values in Local time.
        /// DateTime offsets are done manually for readability purposes of this example. 
        /// <code>
        /// //Local time 
        ///  DateTime d = new DateTime(2018, 3, 19, 6, 56, 0, 0, DateTimeKind.Local);
        /// 
        /// //EST Time is -4 hours from UTC
        /// double offset = -4;
        /// 
        /// //Convert the date to UTC time
        /// d = d.AddHours(offset*-1);
        /// 
        /// //Create a Coordinate with the UTC time		
        /// Coordinate c = new Coordinate(39.0000, -72.0000, d);
        /// //Create a new Celestial object by converting the existing one to Local
        /// Celestial celestial = Celestial.Solar_LocalTime(c, offset);
        /// 
        /// Console.WriteLine(celestial.IsSunUp); //True
        /// Console.WriteLine(celestial.SunRise); //3/19/2018 6:54:25 AM
        /// </code>
        /// </example>  
        [Obsolete("Use instance method 'Coordinate.Celestial_LocalTime(double hourOffset)'.")]
        public static Celestial Solar_LocalTime(Coordinate coord, double offset)
        {
            if (offset < -12 || offset > 12) { throw new ArgumentOutOfRangeException("Time offsets cannot be greater 12 or less than -12."); }
            //Probably need to offset initial UTC date so user can op in local
            //Determine best way to do this.
            DateTime d = coord.GeoDate.AddHours(offset);

            //Get 3 objects for comparison
            Celestial cel = CalculateSunData(coord.Latitude.ToDouble(), coord.Longitude.ToDouble(), coord.GeoDate);
            Celestial celPre = CalculateSunData(coord.Latitude.ToDouble(), coord.Longitude.ToDouble(), coord.GeoDate.AddDays(-1));
            Celestial celPost = CalculateSunData(coord.Latitude.ToDouble(), coord.Longitude.ToDouble(), coord.GeoDate.AddDays(1));

            //Slip objects for comparison. Compare with slipped date. 
            celPre.Local_Convert(coord, offset, Celestial_EagerLoad.Solar);
            cel.Local_Convert(coord, offset, Celestial_EagerLoad.Solar);
            celPost.Local_Convert(coord, offset, Celestial_EagerLoad.Solar);

            //Get SunSet
            int i = Determine_Slipped_Event_Index(cel.SunSet, celPre.SunSet, celPost.SunSet, d);
            cel.sunSet = Get_Correct_Slipped_Date(cel.SunSet, celPre.SunSet, celPost.SunSet, i);
            cel.AdditionalSolarTimes.civilDusk = Get_Correct_Slipped_Date(cel.AdditionalSolarTimes.CivilDusk,
                celPre.AdditionalSolarTimes.CivilDusk, celPost.AdditionalSolarTimes.CivilDusk, i);
            cel.AdditionalSolarTimes.nauticalDusk = Get_Correct_Slipped_Date(cel.AdditionalSolarTimes.NauticalDusk,
               celPre.AdditionalSolarTimes.NauticalDusk, celPost.AdditionalSolarTimes.NauticalDusk, i);
            //Get SunRise
            i = Determine_Slipped_Event_Index(cel.SunRise, celPre.SunRise, celPost.SunRise, d);
            cel.sunRise = Get_Correct_Slipped_Date(cel.SunRise, celPre.SunRise, celPost.SunRise, i);
            cel.AdditionalSolarTimes.civilDawn = Get_Correct_Slipped_Date(cel.AdditionalSolarTimes.CivilDawn,
                celPre.AdditionalSolarTimes.CivilDawn, celPost.AdditionalSolarTimes.CivilDawn, i);
            cel.AdditionalSolarTimes.nauticalDawn = Get_Correct_Slipped_Date(cel.AdditionalSolarTimes.NauticalDawn,
               celPre.AdditionalSolarTimes.NauticalDawn, celPost.AdditionalSolarTimes.NauticalDawn, i);



            //Local Conditions          
            CelestialStatus[] cels = new CelestialStatus[]
            {
                celPre.SunCondition, cel.SunCondition, celPost.SunCondition
            };
            cel.sunCondition = Celestial.GetStatus(cel.SunRise, cel.SunSet, cels);

            //Load IsUp values based on local time with populated Celestial
            Calculate_Celestial_IsUp_Booleans(d, cel);

            return cel;
        }
        /// <summary>
        /// Converts lunar time values to local time. 
        /// </summary>
        /// <param name="coord">Coordinate</param>
        /// <param name="offset">UTC offset</param>
        /// <returns>Celestial</returns>
        /// <example>
        /// The following example demonstrates how to get Celestial, lunar time only values in Local time.
        /// DateTime offsets are done manually for readability purposes of this example. 
        /// <code>
        /// //Local time 
        ///  DateTime d = new DateTime(2018, 3, 19, 6, 56, 0, 0, DateTimeKind.Local);
        /// 
        /// //EST Time is -4 hours from UTC
        /// double offset = -4;
        /// 
        /// //Convert the date to UTC time
        /// d = d.AddHours(offset*-1);
        /// 
        /// //Create a Coordinate with the UTC time		
        /// Coordinate c = new Coordinate(39.0000, -72.0000, d);
        /// //Create a new Celestial object by converting the existing one to Local
        /// Celestial celestial = Celestial.Lunar_LocalTime(c, offset);
        /// 
        /// Console.WriteLine(celestial.IsMoonUp); //False
        /// Console.WriteLine(celestial.SunRise); //3/19/2018 08:17 AM
        /// </code>
        /// </example>     
        [Obsolete("Use instance method 'Coordinate.Celestial_LocalTime(double hourOffset)'.")]
        public static Celestial Lunar_LocalTime(Coordinate coord, double offset)
        {
            if (offset < -12 || offset > 12) { throw new ArgumentOutOfRangeException("Time offsets cannot be greater 12 or less than -12."); }
            //Probably need to offset initial UTC date so user can op in local
            //Determine best way to do this.
            DateTime d = coord.GeoDate.AddHours(offset);

            //Get 3 objects for comparison
            Celestial cel = CalculateMoonData(coord.Latitude.ToDouble(), coord.Longitude.ToDouble(), coord.GeoDate);
            Celestial celPre = CalculateMoonData(coord.Latitude.ToDouble(), coord.Longitude.ToDouble(), coord.GeoDate.AddDays(-1));
            Celestial celPost = CalculateMoonData(coord.Latitude.ToDouble(), coord.Longitude.ToDouble(), coord.GeoDate.AddDays(1));

            //Slip objects for comparison. Compare with slipped date. 
            celPre.Local_Convert(coord, offset, Celestial_EagerLoad.Lunar);
            cel.Local_Convert(coord, offset, Celestial_EagerLoad.Lunar);
            celPost.Local_Convert(coord, offset, Celestial_EagerLoad.Lunar);


            //MoonRise
            int i = Determine_Slipped_Event_Index(cel.MoonRise, celPre.MoonRise, celPost.MoonRise, d);
            cel.moonRise = Get_Correct_Slipped_Date(cel.MoonRise, celPre.MoonRise, celPost.MoonRise, i);

            //MoonSet
            i = Determine_Slipped_Event_Index(cel.MoonSet, celPre.MoonSet, celPost.MoonSet, d);
            cel.moonSet = Get_Correct_Slipped_Date(cel.MoonSet, celPre.MoonSet, celPost.MoonSet, i);

            //Local Conditions
            CelestialStatus[] cels = new CelestialStatus[]
            {
                celPre.MoonCondition,cel.MoonCondition,celPost.MoonCondition
            };
            cel.moonCondition = Celestial.GetStatus(cel.MoonRise, cel.MoonSet, cels);

            //Load IsUp values based on local time with populated Celestial
            Celestial.Calculate_Celestial_IsUp_Booleans(d, cel);

            return cel;
        }
        /// <summary>
        /// Calculate sun data based on latitude, longitude and UTC date at the location.
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="longi">longitude</param>
        /// <param name="date">DateTime</param>
        /// <returns>Celestial (Partially Populated)</returns>
        /// <example>
        /// The following example demonstrates how to create a partially populated Celestial object 
        /// that only calculates solar data using static functions. 
        /// <code>
        /// //Get Celestial data at N 39, W 72 on 19-Mar-2019 10:10:12 UTC
        /// Celestial cel = Celestial.CalculateSunData(39, -72, new DateTime(2019, 3, 19, 10, 10, 12));
        /// 
        /// Console.WriteLine(cel.SunRise); //3/19/2019 10:54:50 AM
        /// </code>
        /// </example>
        [Obsolete("Use static method 'Celestial.CalculateCelestialTimes(double lat, double longi, DateTime date, EagerLoad el)'.")]
        public static Celestial CalculateSunData(double lat, double longi, DateTime date)
        {
            date = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, DateTimeKind.Utc);

            Celestial c = new Celestial(false);
            SunCalc.CalculateSunTime(lat, longi, date, c, new EagerLoad(), 0);
            SunCalc.CalculateZodiacSign(date, c);

            return c;
        }
        /// <summary>
        /// Calculate moon data based on latitude, longitude and UTC date at the location.
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="longi">longitude</param>
        /// <param name="date">DateTime</param>
        /// <returns>Celestial (Partially Populated)</returns>
        /// <example>
        /// The following example demonstrates how to create a partially populated Celestial object 
        /// that only calculates lunar data using static functions. 
        /// <code>
        /// //Get Celestial data at N 39, W 72 on 19-Mar-2019 10:10:12 UTC
        /// Celestial cel = Celestial.CalculateMoonData(39, -72, new DateTime(2019, 3, 19, 10, 10, 12));
        /// 
        /// Console.WriteLine(cel.MoonRise); //3/19/2019 9:27:27 PM
        /// </code>
        /// </example>
        [Obsolete("Use static method 'Celestial.CalculateCelestialTimes(double lat, double longi, DateTime date, EagerLoad el)'.")]
        public static Celestial CalculateMoonData(double lat, double longi, DateTime date)
        {
            date = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, DateTimeKind.Utc);

            Celestial c = new Celestial(false);

            MoonCalc.GetMoonTimes(date, lat, longi, c, 0);
            MoonCalc.GetMoonDistance(date, c);
            MoonCalc.GetMoonSign(date, c);
            MoonCalc.GetMoonIllumination(date, c, lat, longi, new EagerLoad(), 0);

            c.perigee = MoonCalc.GetPerigeeEvents(date);
            c.apogee = MoonCalc.GetApogeeEvents(date);

            return c;
        }

        //BELOW METHOD NEEDED FOR DEPRECATED FUNCTIONS.
        //REMOVE AS SCHEDULED WITH ABOVE OBSOLETES.
        private static CelestialStatus GetStatus(DateTime? rise, DateTime? set, CelestialStatus[] cels)
        {
            if (set.HasValue && rise.HasValue) { return CelestialStatus.RiseAndSet; }
            if (set.HasValue && !rise.HasValue) { return CelestialStatus.NoRise; }
            if (!set.HasValue && rise.HasValue) { return CelestialStatus.NoSet; }
            for (int x = 0; x < 3; x++)
            {
                if (cels[x] == CelestialStatus.DownAllDay || cels[x] == CelestialStatus.UpAllDay)
                {
                    return cels[x];
                }
            }
            return cels[1];
        }
        #endregion
    }
}
