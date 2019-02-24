using System;
using System.ComponentModel;

namespace CoordinateSharp
{
    
    /// <summary>
    /// Observable class for handling all location based information.
    /// This is the main class for CoordinateSharp.
    /// </summary>
    /// <remarks>
    /// All information should be pulled from this class to include celestial information
    /// </remarks>
    [Serializable]
    public partial class Coordinate : INotifyPropertyChanged
    {      
        private CoordinatePart latitude;
        private CoordinatePart longitude;
        private DateTime geoDate;

        private UniversalTransverseMercator utm;
        private MilitaryGridReferenceSystem mgrs;
        private Cartesian cartesian;
        private ECEF ecef;

        private Celestial celestialInfo;

        internal double equatorial_radius;
        internal double inverse_flattening;

        /// <summary>
        /// Latitudinal Coordinate Part
        /// </summary>
        public CoordinatePart Latitude
        {
            get { return latitude; }
            set
            {
                if (latitude != value)
                {
                    if (value.Position == CoordinatesPosition.E || value.Position == CoordinatesPosition.W)
                    { throw new ArgumentException("Invalid Position", "Latitudinal positions cannot be set to East or West."); }
                    latitude = value;
                    latitude.parent = this;
                    if (EagerLoadSettings.Celestial)
                    {
                        celestialInfo.CalculateCelestialTime(Latitude.DecimalDegree, Longitude.DecimalDegree, geoDate);
                    }
                    if (longitude != null)
                    {

                        if (EagerLoadSettings.UTM_MGRS)
                        {
                            utm = new UniversalTransverseMercator(latitude.ToDouble(), longitude.ToDouble(), this, utm.equatorial_radius, utm.inverse_flattening);
                            mgrs = new MilitaryGridReferenceSystem(utm);
                        }
                        if (EagerLoadSettings.Cartesian)
                        {
                            cartesian = new Cartesian(this);
                        }
                        if (EagerLoadSettings.ECEF)
                        {
                            ecef = new ECEF(this);
                        }
                    }

                }
            }
        }
        /// <summary>
        /// Longitudinal Coordinate Part
        /// </summary>
        public CoordinatePart Longitude
        {
            get { return longitude; }
            set
            {
                if (longitude != value)
                {
                    if (value.Position == CoordinatesPosition.N || value.Position == CoordinatesPosition.S)
                    { throw new ArgumentException("Invalid Position", "Longitudinal positions cannot be set to North or South."); }
                    longitude = value;
                    longitude.parent = this;
                    if (EagerLoadSettings.Celestial)
                    {
                        celestialInfo.CalculateCelestialTime(Latitude.DecimalDegree, Longitude.DecimalDegree, geoDate);
                    }
                    if (latitude != null)
                    {
                        if (EagerLoadSettings.UTM_MGRS)
                        {
                            utm = new UniversalTransverseMercator(latitude.ToDouble(), longitude.ToDouble(), this, utm.equatorial_radius, utm.inverse_flattening);
                            mgrs = new MilitaryGridReferenceSystem(utm);
                        }
                        if (EagerLoadSettings.Cartesian)
                        {
                            cartesian = new Cartesian(this);
                        }
                        if (EagerLoadSettings.ECEF)
                        {
                            ecef = new ECEF(this);
                        }
                    }

                }
            }
        }
        /// <summary>
        /// Date used to calculate celestial information
        /// </summary>
        /// <remarks>
        /// Assumes all times are in UTC
        /// </remarks>
        public DateTime GeoDate
        {
            get { return geoDate; }
            set
            {
                if (geoDate != value)
                {
                    geoDate = value;
                    if (EagerLoadSettings.Celestial)
                    {
                        celestialInfo.CalculateCelestialTime(Latitude.DecimalDegree, Longitude.DecimalDegree, geoDate);
                        NotifyPropertyChanged("CelestialInfo");
                    }

                    NotifyPropertyChanged("GeoDate");
                }
            }
        }

        /// <summary>
        /// Universal Transverse Mercator Values
        /// </summary>
        public UniversalTransverseMercator UTM
        {
            get
            {
                return utm;
            }
        }
        
        /// <summary>
        /// Military Grid Reference System (NATO UTM)
        /// </summary>
        public MilitaryGridReferenceSystem MGRS
        {
            get
            {
                return mgrs;
            }
        }
        /// <summary>
        /// Cartesian (Based on Spherical Earth)
        /// </summary>
        public Cartesian Cartesian
        {
            get
            {
                return cartesian;
            }
        }
        /// <summary>
        /// Earth Centered Earth Fixed Coordinate. 
        /// Uses Ellipsoidal height with no geoid model included.
        /// 0 = Mean Sea Level based on the provided Datum.
        /// </summary>
        public ECEF ECEF
        {
            get
            {
                return ecef;
            }

            //Required due to GeoDetic Height
            internal set
            {
                if (ecef != value)
                {
                    ecef = value;
                    NotifyPropertyChanged("ECEF");
                }
            }
        }
        /// <summary>
        /// Celestial information based on the objects location and geographic UTC date.
        /// </summary>
        public Celestial CelestialInfo
        {
            get { return celestialInfo; }
        }

        /// <summary>
        /// Coordinate string formatting options.
        /// </summary>
        public CoordinateFormatOptions FormatOptions { get; set; }
        /// <summary>
        /// Eager loading settings.
        /// </summary>
        public EagerLoad EagerLoadSettings { get; set; }

        //PARSER INDICATOR
        private Parse_Format_Type parse_Format = Parse_Format_Type.None;
        /// <summary>
        /// Used to determine what format the coordinate was parsed from.
        /// Will equal "None" if Coordinate was not initialized via a TryParse() method.
        /// </summary>
        public Parse_Format_Type Parse_Format
        {
            get
            {
                return parse_Format;
            }
            internal set
            {
                if (parse_Format != value)
                {
                    parse_Format = value;
                    NotifyPropertyChanged("Parse_Format");
                }
            }
        }
    }   
}
