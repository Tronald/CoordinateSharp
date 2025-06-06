/*
CoordinateSharp is a .NET standard library that is intended to ease geographic coordinate 
format conversions and location based celestial calculations.
https://github.com/Tronald/CoordinateSharp

Many celestial formulas in this library are based on Jean Meeus's 
Astronomical Algorithms (2nd Edition). Comments that reference only a chapter
are referring to this work.

License

CoordinateSharp is split licensed and may be licensed under the GNU Affero General Public License version 3 or a commercial use license as stated.

Copyright (C) 2023, Signature Group, LLC
  
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
using CoordinateSharp.Formatters;
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

        /// <summary>
        /// Get solstice events in UTC based on provided DateTime.
        /// </summary>
        /// <param name="d">DateTime</param>
        /// <returns>Solstices</returns>
        public static Solstices Get_Solstices(DateTime d)
        {
            return Get_Solstices(d, 0);   
        }

        /// <summary>
        /// Get solstice events in local based on provided DateTime and UTC offset.
        /// </summary>
        /// <param name="d">DateTime</param>
        /// <param name="offset">UTC Offset</param>
        /// <returns>Solstices</returns>
        public static Solstices Get_Solstices(DateTime d, double offset)
        {
            Celestial c = new Celestial();
            SunCalc.Calculate_Solstices_Equinoxes(d, c, offset);
            return c.Solstices;
        }


        /// <summary>
        /// Get equinox events in UTC based on provided DateTime.
        /// </summary>
        /// <param name="d">DateTime</param>
        /// <returns>Equinoxes</returns>
        public static Equinoxes Get_Equinoxes(DateTime d)
        {
            return Get_Equinoxes(d, 0);
        }

        /// <summary>
        /// Get equinoxe events in local based on provided DateTime and UTC offset.
        /// </summary>
        /// <param name="d">DateTime</param>
        /// <param name="offset">UTC Offset</param>
        /// <returns>Solstices</returns>
        public static Equinoxes Get_Equinoxes(DateTime d, double offset)
        {
            Celestial c = new Celestial();
            SunCalc.Calculate_Solstices_Equinoxes(d, c, offset);
            return c.Equinoxes;
        }

        /// <summary>
        /// Get times based on the specified Coordinate object's data and altitude.
        /// Condition (AltitudeEvents) should be checked if either Rising or Setting values return null.
        /// <b>Caution:</b> These methods are only reliable during daylight hours when the sun is visible. 
        /// Time calculations will not be accurate after sunset when the altitude is negative.
        /// </summary>
        /// <param name="c">Coordinate</param>
        /// <param name="alt">Altitude in degrees</param>
        /// <returns>AltitudeEvents</returns>
        /// <example>
        /// The following example returns local event times based on the provided Coordinate object
        /// data and solar altitude.
        /// <code>
        /// Coordinate coordinate = new Coordinate(40.0,-74.6, new DateTime(2018,3,1));
        /// coordinate.Offset = -5; //Offset UTC Time by 8 hours to operate in local time
        ///
        /// var sa = Celestial.Get_Time_at_Solar_Altitude(coordinate, 23.4);
        /// Console.WriteLine(sa.Rising); //3/1/2018 8:49:35 AM
        /// Console.WriteLine(sa.Setting); //3/1/2018 3:34:51 PM
        /// Console.WriteLine(sa.Condition); //RiseAndSet;
        /// </code>
        /// </example>
        public static AltitudeEvents Get_Time_at_Solar_Altitude(Coordinate c, double alt)
        {
            return Get_Time_at_Solar_Altitude(c.Latitude.ToDouble(), c.Longitude.ToDouble(), c.GeoDate, alt, c.Offset);
        }
        /// <summary>
        /// Get times based on the specified Coordinate object's data and altitude.
        /// Condition (AltitudeEvents) should be checked if either Rising or Setting values return null.
        /// <b>Caution:</b> These methods are only reliable during daylight hours when the sun is visible. 
        /// Time calculations will not be accurate after sunset when the altitude is negative.
        /// </summary>
        /// <param name="lat">Signed latitude</param>
        /// <param name="longi">Signed longitude</param>
        /// <param name="date">Date</param>
        /// <param name="alt">Altitude in degrees</param>
        /// <returns>AltitudeEvents</returns>
        /// <example>
        /// The following example returns UTC event times based on the provided lat/long, date
        /// and solar altitude.
        /// <code>
        /// var sa = Celestial.Get_Time_at_Solar_Altitude(40.0, -74.6, new DateTime(2018,3,1), 23.4);
        /// Console.WriteLine(sa.Rising); //3/1/2018 13:49:35 UTC
        /// Console.WriteLine(sa.Setting); //3/1/2018 20:34:51 UTC
        /// Console.WriteLine(sa.Condition); //RiseAndSet;
        /// </code>
        /// </example>
        public static AltitudeEvents Get_Time_at_Solar_Altitude(double lat, double longi, DateTime date, double alt)
        {
            return Get_Time_at_Solar_Altitude(lat, longi, date, alt, 0);
        }
        /// <summary>
        /// Get times based on the specified Coordinate object's data and altitude.
        /// Condition (AltitudeEvents) should be checked if either Rising or Setting values return null.
        /// <b>Caution:</b> These methods are only reliable during daylight hours when the sun is visible. 
        /// Time calculations will not be accurate after sunset when the altitude is negative.
        /// </summary>
        /// <param name="lat">Signed latitude</param>
        /// <param name="longi">Signed longitude</param>
        /// <param name="date">Date</param>
        /// <param name="alt">Altitude in degrees</param>
        /// <param name="offset">UTC offset</param>
        /// <returns>AltitudeEvents</returns>
        /// <example>
        /// The following example returns local event times based on the provided lat/long, date, 
        /// solar altitude and UTC offset.
        /// <code>
        /// var sa = Celestial.Get_Time_at_Solar_Altitude(40.0, -74.6, new DateTime(2018,3,1), 23.4, -5);
        /// Console.WriteLine(sa.Rising); //3/1/2018 8:49:35 AM
        /// Console.WriteLine(sa.Setting); //3/1/2018 3:34:51 PM
        /// Console.WriteLine(sa.Condition); //RiseAndSet;
        /// </code>
        /// </example>
        public static AltitudeEvents Get_Time_at_Solar_Altitude(double lat, double longi, DateTime date, double alt, double offset)
        {
            //IAW 15.1
            double rad = Math.PI / 180;
            double lw = rad * -longi;
            double phi = rad * lat;

            var events = SunCalc.Get_Event_Time(lat, longi, lw, phi, alt, date, offset, false);
         
            var altE = new AltitudeEvents() { Rising = events[0], Setting = events[1] };

            //Determine Condition is event horizon is passed.
            if (altE.Rising == null && altE.Setting == null)
            {
                var celC = SunCalc.Get_Solar_Coordinates(date, -offset);
                var ang = SunCalc.CalculateSunAngle(date.AddHours(-offset), lat, longi, celC); //subtract offset to run calc at Z time.
                if(ang[1] < alt) { altE.Condition = CelestialStatus.DownAllDay; }
                else { altE.Condition = CelestialStatus.UpAllDay; }
            }
            else if (altE.Rising == null)
            {
                altE.Condition = CelestialStatus.NoRise;
            }
            else if (altE.Setting == null)
            {
                altE.Condition = CelestialStatus.NoSet;
            }
            else
            {
                altE.Condition = CelestialStatus.RiseAndSet;
            }

            return altE;
        }

        /// <summary>
        /// Gets solar coordinates based on UTC DateTime. Values are in degrees unless otherwise specified.
        /// </summary>
        /// <param name="date">UTC DateTime</param>
        /// <returns>SolarCoordinates</returns>
        /// <example>
        /// The following example returns the coordinates of the sun based on the provided UTC DateTime.
        /// <code>
        /// var celC = Celestial.Get_Solar_Coordinates(new DateTime(1992,4,12));
        /// 
        /// //PROPERTY VALUES
        /// //RadiusVector: 1.00249723716304
        /// //Longitude: 22.3395779176985
        /// //Latitude: 0
        /// //RightAscension: 20.6576437884708
        /// //Declination: 8.69650660229221
        /// //GeometricMeanLongitude: 20.4480824646839
        /// //SubsolarLatitude: 8.69650660229221
        /// //SubsolarLongitude: -179.790438676213
        /// </code>
        /// </example>
        public static SolarCoordinates Get_Solar_Coordinate(DateTime date)
        {
            return SunCalc.Get_Solar_Coordinates(date, 0);
        }

        /// <summary>
        /// Gets solar coordinates based on UTC DateTime. Values are in degrees unless otherwise specified.
        /// </summary>
        /// <param name="date">UTC DateTime</param>
        /// <param name="offset">UTC Offset in Hours</param>
        /// <returns>SolarCoordinates</returns>
        /// <example>
        /// The following example returns the coordinates of the sun based on the provided Local DateTime and UTC Offset.
        /// <code>
        /// var celC = Celestial.Get_Solar_Coordinates(new DateTime(1992,4,12,7,0,0), 7);
        /// 
        /// //PROPERTY VALUES
        /// //RadiusVector: 1.00249723716304
        /// //Longitude: 22.3395779176985
        /// //Latitude: 0
        /// //RightAscension: 20.6576437884708
        /// //Declination: 8.69650660229221
        /// //GeometricMeanLongitude: 20.4480824646839
        /// //SubsolarLatitude: 8.69650660229221
        /// //SubsolarLongitude: -179.790438676213
        /// </code>
        /// </example>
        public static SolarCoordinates Get_Solar_Coordinate(DateTime date, double offset)
        {
            offset *= -1; //REVERSE OFFSET FOR POSITIONING
            return SunCalc.Get_Solar_Coordinates(date, offset);
        }

        /// <summary>
        /// Gets lunar coordinates based on UTC DateTime. Values are in degrees unless otherwise specified.
        /// </summary>
        /// <param name="date">UTC DateTime</param>
        /// <returns>LunarCoordinates</returns>
        /// <example>
        /// The following example returns the coordinates of the moon based on the provided UTC DateTime.
        /// <code>
        /// var celC = Celestial.Get_Lunar_Coordinates(new DateTime(1992,4,12));
        /// 
        /// //PROPERTY VALUES
        /// //Longitude: 133.167197165444
        /// //Latitude: -3.22898378016249
        /// //RightAscension: 134.683729152193
        /// //Declination: 13.7688377109201
        /// //GeometricMeanLongitude: 134.29018141258
        /// //SublunarLatitude: 13.7688377109201
        /// //SublunarLongitude: -65.7643937981642
        /// </code>
        /// </example>
        public static LunarCoordinates Get_Lunar_Coordinate(DateTime date)
        {
            return MoonCalc.GetMoonCoords(date, 0);
        }

        /// <summary>
        /// Gets lunar coordinates based on UTC DateTime. Values are in degrees unless otherwise specified.
        /// </summary>
        /// <param name="date">UTC DateTime</param>
        /// <param name="offset">UTC Offset in Hours</param>
        /// <returns>LunarCoordinates</returns>
        /// <example>
        /// The following example returns the coordinates of the moon based on the provided Local DateTime and UTC Offset.
        /// <code>
        /// var celC = Celestial.Get_Lunar_Coordinates(new DateTime(1992,4,12,7,0,0), 7);
        /// 
        /// //PROPERTY VALUES
        /// //Longitude: 133.167197165444
        /// //Latitude: -3.22898378016249
        /// //RightAscension: 134.683729152193
        /// //Declination: 13.7688377109201
        /// //GeometricMeanLongitude: 134.29018141258
        /// //SublunarLatitude: 13.7688377109201
        /// //SublunarLongitude: -65.7643937981642
        /// </code>
        /// </example>
        public static LunarCoordinates Get_Lunar_Coordinate(DateTime date, double offset)
        {
            offset *= -1; //REVERSE OFFSET FOR POSITIONING
            return MoonCalc.GetMoonCoords(date, offset);
        }

        /// <summary>
        /// Returns the estimated time of day at the provided coordinate based on the azimuth of the sun at the current date, with an azimuth delta of 1.
        /// </summary>
        /// <param name="azimuth">Sun azimuth in degrees</param>
        /// <param name="c">Coordinate</param>
        /// <returns>DateTime?</returns>
        /// <remarks>Returns null if azimuth does not exist for day or sun is down. May not be reliable in circumpolar regions</remarks>
        /// <example>
        /// <code>
        /// DateTime d = new DateTime(2023, 9, 29);
        /// double azimuth = 190.1
        /// 
        /// Coordinate c = new Coordinate(47.3, -122, d);
        /// 
        /// DateTime timeAtAzimuth = Get_Time_At_Solar_Azimuth(azimuth, c);
        ///  
        /// Console.WriteLine($"TIME AT AZ: {timeAtAzimuth}"); //9/29/2023 1:31:03 PM
        /// </code>
        /// </example>
        public static DateTime? Get_Time_At_Solar_Azimuth(double azimuth, Coordinate c)
        {
           return Get_Time_At_Solar_Azimuth(azimuth, c, 1);
        }

        /// <summary>
        /// Returns the estimated time of day at the provided coordinate based on the azimuth of the sun at the current date with a specified azimuth delta.
        /// </summary>
        /// <param name="azimuth">Sun azimuth in degrees</param>
        /// <param name="c">Coordinate</param>
        /// <param name="delta">Error Delta</param>
        /// <returns>DateTime?</returns>
        /// <remarks>Returns null if azimuth does not exist for day or sun is down. May not be reliable in circumpolar regions</remarks>
        /// <example>
        /// <code>
        /// DateTime d = new DateTime(2023, 9, 29);
        /// double azimuth = 190.1
        /// 
        /// Coordinate c = new Coordinate(47.3, -122, d);
        /// 
        /// DateTime timeAtAzimuth = Get_Time_At_Solar_Azimuth(azimuth, c);
        ///  
        /// Console.WriteLine($"TIME AT AZ: {timeAtAzimuth}"); //9/29/2023 1:31:03 PM
        /// </code>
        /// </example>
        public static DateTime? Get_Time_At_Solar_Azimuth(double azimuth, Coordinate c, double delta)
        {
            DateTime d = c.GeoDate.Date; //Set to date
            DateTime hour = Get_Hour(d, azimuth, c);
            DateTime minutes = Get_Minute(hour, azimuth, c);
            DateTime seconds = Get_Seconds(minutes, azimuth, c);

            Coordinate nc = new Coordinate(c.Latitude.ToDouble(), c.Longitude.ToDouble(), seconds, new EagerLoad(EagerLoadType.Celestial));
            nc.Offset = c.Offset;
            if (!nc.CelestialInfo.isSunUp || Math.Abs(azimuth - nc.CelestialInfo.sunAzimuth) > delta)
            {
                return null;
            }

            return seconds;
        }      
        private static DateTime Get_Hour(DateTime d, double azimuth, Coordinate c)
        {
            EagerLoad el = new EagerLoad(EagerLoadType.Celestial);
            el.Extensions = new EagerLoad_Extensions(EagerLoad_ExtensionsType.Solar_Cycle);
            int closeHour = 0;
            double az = 999;
            DateTime nd = new DateTime(d.Year, d.Month, d.Day);
            for (int x = 0; x < 24; x++)
            {
                Coordinate nc = new Coordinate(c.Latitude.ToDouble(), c.Longitude.ToDouble(), nd.AddHours(x), el);
                nc.Offset = c.Offset;
                if (Math.Abs(nc.CelestialInfo.SunAzimuth - azimuth) < az)
                {
                    az = Math.Abs(nc.CelestialInfo.SunAzimuth - azimuth);
                    closeHour = x;
                }
            }


            return d.AddHours(closeHour);
        }

        private static DateTime Get_Minute(DateTime d, double azimuth, Coordinate c)
        {
            EagerLoad el = new EagerLoad(EagerLoadType.Celestial);
            el.Extensions = new EagerLoad_Extensions(EagerLoad_ExtensionsType.Solar_Cycle);
            int closeMinutes = 0;
            double az = 999;
            DateTime nd = new DateTime(d.Year, d.Month, d.Day, d.Hour, 0, 0);
            for (int x = 0; x < 60; x++)
            {
                Coordinate nc = new Coordinate(c.Latitude.ToDouble(), c.Longitude.ToDouble(), nd.AddMinutes(x), el);
                nc.Offset = c.Offset;
                if (Math.Abs(nc.CelestialInfo.SunAzimuth - azimuth) < az)
                {
                    az = Math.Abs(nc.CelestialInfo.SunAzimuth - azimuth);
                    closeMinutes = x;
                }
            }

            for (int x = 0; x < 60; x++)
            {
                Coordinate nc = new Coordinate(c.Latitude.ToDouble(), c.Longitude.ToDouble(), nd.AddMinutes(-x), el);
                nc.Offset = c.Offset;
                if (Math.Abs(nc.CelestialInfo.SunAzimuth - azimuth) < az)
                {
                    az = Math.Abs(nc.CelestialInfo.SunAzimuth - azimuth);
                    closeMinutes = -x;
                }
            }

            return d.AddMinutes(closeMinutes);
        }

        private static DateTime Get_Seconds(DateTime d, double azimuth, Coordinate c)
        {
            EagerLoad el = new EagerLoad(EagerLoadType.Celestial);
            el.Extensions = new EagerLoad_Extensions(EagerLoad_ExtensionsType.Solar_Cycle);
            int closeSeconds = 0;
            double az = 999;
            DateTime nd = new DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, 0);
            for (int x = 0; x < 60; x++)
            {
                Coordinate nc = new Coordinate(c.Latitude.ToDouble(), c.Longitude.ToDouble(), nd.AddSeconds(x), el);
                nc.Offset = c.Offset;
                if (Math.Abs(nc.CelestialInfo.SunAzimuth - azimuth) < az)
                {
                    az = Math.Abs(nc.CelestialInfo.SunAzimuth - azimuth);
                    closeSeconds = x;
                }
            }

            for (int x = 0; x < 60; x++)
            {
                Coordinate nc = new Coordinate(c.Latitude.ToDouble(), c.Longitude.ToDouble(), nd.AddSeconds(-x), el);
                nc.Offset = c.Offset;
                if (Math.Abs(nc.CelestialInfo.SunAzimuth - azimuth) < az)
                {
                    az = Math.Abs(nc.CelestialInfo.SunAzimuth - azimuth);
                    closeSeconds = -x;
                }
            }

            return d.AddSeconds(closeSeconds);
        }

       
      
    }
}
