/*
CoordinateSharp is a .NET standard library that is intended to ease geographic coordinate 
format conversions and location based celestial calculations.
https://github.com/Tronald/CoordinateSharp

Many celestial formulas in this library are based on Jean Meeus's 
Astronomical Algorithms (2nd Edition). Comments that reference only a chapter
are referring to this work.

License

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

For more information, please contact Signature Group, LLC at this address: sales@signatgroup.com
*/
using System;

namespace CoordinateSharp
{

    /// <summary>
    /// Used to specify whether a CoordinatePart object is latitudinal or longitudinal.
    /// </summary>
    [Serializable]
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
    /// Used to set a CoordinatePart object's position.
    /// </summary>
    [Serializable]
    public enum CoordinatesPosition : int
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

    /// <summary>
    /// Coordinate type datum specification
    /// </summary>
    [Serializable]
    [Flags]
    public enum Coordinate_Datum
    {
        /// <summary>
        /// Lat Long GeoDetic
        /// </summary>
        LAT_LONG = 1,
        /// <summary>
        /// UTM and MGRS
        /// </summary>
        UTM_MGRS = 2,
        /// <summary>
        /// ECEF
        /// </summary>
        ECEF = 4,
    }

    /// <summary>
    /// Cartesian Coordinate Type
    /// </summary>
    [Serializable]
    public enum CartesianType
    {
        /// <summary>
        /// Spherical Cartesian
        /// </summary>
        Cartesian,
        /// <summary>
        /// Earth Centered Earth Fixed
        /// </summary>
        ECEF,
    }

    /// <summary>
    /// Earth Shape for Calculations.
    /// </summary>
    [Serializable]
    public enum Shape
    {
        /// <summary>
        /// Calculate as sphere (less accurate, more efficient).
        /// </summary>
        Sphere,
        /// <summary>
        /// Calculate as ellipsoid (more accurate, less efficient).
        /// </summary>
        Ellipsoid
    }

    /// <summary>
    /// Distance measurement type
    /// </summary>
    [Serializable]
    public enum DistanceType
    {
        /// <summary>
        /// Distance in Meters
        /// </summary>
        Meters,
        /// <summary>
        /// Distance in Kilometers
        /// </summary>
        Kilometers,
        /// <summary>
        /// Distance in Feet
        /// </summary>
        Feet,
        /// <summary>
        /// Distance in Statute Miles
        /// </summary>
        Miles,
        /// <summary>
        /// Distance in Nautical Miles
        /// </summary>
        NauticalMiles
    }

    /// <summary>
    /// EagerLoad property type enumerator
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

    /// <summary>
    /// Coordinate format types.
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

    /// <summary>
    /// Type of format a Coordinate is parsed from. 
    /// </summary>
    [Serializable]
    public enum Parse_Format_Type
    {
        /// <summary>
        /// Coordinate was not initialized from a parser method.
        /// </summary>
        None,
        /// <summary>
        /// Signed Degree
        /// DD.dddd
        /// </summary>
        Signed_Degree,
        /// <summary>
        /// Decimal Degree
        /// P DD.dddd
        /// </summary>
        Decimal_Degree,
        /// <summary>
        /// Degree Decimal Minute
        /// P DD MM.sss
        /// </summary>
        Degree_Decimal_Minute,
        /// <summary>
        /// Degree Minute Second
        /// P DD MM SS.sss
        /// </summary>
        Degree_Minute_Second,
        /// <summary>
        /// Universal Transverse Mercator
        /// </summary>
        UTM,
        /// <summary>
        /// Military Grid Reference System
        /// </summary>
        MGRS,
        /// <summary>
        /// Spherical Cartesian
        /// </summary>
        Cartesian_Spherical,
        /// <summary>
        /// Earth Centered Earth Fixed
        /// </summary>
        Cartesian_ECEF
    }
    /// <summary>
    /// Used to display a celestial condition for a specified date.
    /// </summary>
    [Serializable]
    public enum CelestialStatus
    {
        /// <summary>
        /// Celestial body rises and sets on the set day.
        /// </summary>
        RiseAndSet,
        /// <summary>
        /// Celestial body is down all day
        /// </summary>
        DownAllDay,
        /// <summary>
        /// Celestial body is up all day
        /// </summary>
        UpAllDay,
        /// <summary>
        /// Celestial body rises, but does not set on the set day
        /// </summary>
        NoRise,
        /// <summary>
        /// Celestial body sets, but does not rise on the set day
        /// </summary>
        NoSet
    }
    /// <summary>
    /// Moon perigee or apogee indicator
    /// </summary>
    internal enum MoonDistanceType
    {
        /// <summary>
        /// Moon's perigee
        /// </summary>
        Perigee,
        /// <summary>
        /// Moon's apogee
        /// </summary>
        Apogee
    }
    /// <summary>
    /// Solar eclipse type
    /// </summary>
    [Serializable]
    public enum SolarEclipseType
    {
        /// <summary>
        /// Partial Eclipse
        /// </summary>
        Partial,
        /// <summary>
        /// Annular Eclipse
        /// </summary>
        Annular,
        /// <summary>
        /// Total Eclipse...of the heart...
        /// </summary>
        Total
    }
    /// <summary>
    /// Lunar eclipse type
    /// </summary>
    [Serializable]
    public enum LunarEclipseType
    {
        /// <summary>
        /// Penumbral Eclipse
        /// </summary>
        Penumbral,
        /// <summary>
        /// Partial Eclipse
        /// </summary>
        Partial,
        /// <summary>
        /// Total Eclipse...of the heart...
        /// </summary>
        Total
    }
    internal enum Celestial_EagerLoad
    {
        All, Lunar, Solar
    }
}
