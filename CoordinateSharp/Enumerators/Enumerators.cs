using System;

namespace CoordinateSharp
{

    /// <summary>
    /// Used for setting whether a coordinate part is latitudinal or longitudinal.
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
    /// Used to set a coordinate part position.
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

    /// <summary>
    /// Type of format a Coordinate parsed from. 
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
}
