/*
CoordinateSharp is a .NET standard library that is intended to ease geographic coordinate 
format conversions and location based celestial calculations.
https://github.com/Tronald/CoordinateSharp

Many celestial formulas in this library are based on Jean Meeus's 
Astronomical Algorithms (2nd Edition). Comments that reference only a chapter
are refering to this work.

MIT License

(c) 2017, Justin Gielski

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.


THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE. 
*/

using System;
using System.ComponentModel;
using System.Diagnostics;

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
    public class Coordinate : INotifyPropertyChanged
    {
        /// <summary>
        /// Creates an empty Coordinate.
        /// </summary>
        /// <remarks>
        /// Values will need to be provided to latitude/longitude CoordinateParts manually
        /// </remarks>
        public Coordinate()
        {
            FormatOptions = new CoordinateFormatOptions();
            geoDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            latitude = new CoordinatePart(CoordinateType.Lat);            
            longitude = new CoordinatePart(CoordinateType.Long);
            latitude.parent = this;
            longitude.parent = this;
            celestialInfo = new Celestial();
            utm = new UniversalTransverseMercator(latitude.ToDouble(), longitude.ToDouble(), this);
            mgrs = new MilitaryGridReferenceSystem(utm);
            cartesian = new Cartesian(this);
            ecef = new ECEF(this);

            EagerLoadSettings = new EagerLoad();

            equatorial_radius = 6378137.0;
            inverse_flattening = 298.257223563;
        }
        /// <summary>
        /// Creates an empty Coordinate with custom datum.
        /// </summary>
        /// <remarks>
        /// Values will need to be provided to latitude/longitude CoordinateParts manually
        /// </remarks>
        internal Coordinate(double equatorialRadius, double inverseFlattening, bool t)
        {
            FormatOptions = new CoordinateFormatOptions();
            geoDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            latitude = new CoordinatePart(CoordinateType.Lat);
            longitude = new CoordinatePart(CoordinateType.Long);
            latitude.parent = this;
            longitude.parent = this;
            celestialInfo = new Celestial();
            utm = new UniversalTransverseMercator(latitude.ToDouble(), longitude.ToDouble(), this, equatorialRadius, inverseFlattening);
            mgrs = new MilitaryGridReferenceSystem(utm);
            cartesian = new Cartesian(this);
            ecef = new ECEF(this);

            EagerLoadSettings = new EagerLoad();
            Set_Datum(equatorialRadius, inverseFlattening);
        }
        /// <summary>
        /// Creates a populated Coordinate based on decimal (signed degrees) formated latitude and longitude.
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="longi">longitude</param>
        /// <remarks>
        /// Geodate will default to 1/1/1900 GMT until provided
        /// </remarks>
        public Coordinate(double lat, double longi)
        {
            FormatOptions = new CoordinateFormatOptions();
            geoDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            latitude = new CoordinatePart(lat, CoordinateType.Lat);
            longitude = new CoordinatePart(longi, CoordinateType.Long);
            latitude.parent = this;
            longitude.parent = this;
            celestialInfo = new Celestial(lat, longi, geoDate);
            utm = new UniversalTransverseMercator(lat, longi, this);
            mgrs = new MilitaryGridReferenceSystem(utm);
            cartesian = new Cartesian(this);
            ecef = new ECEF(this);
            EagerLoadSettings = new EagerLoad();

            equatorial_radius = 6378137.0;
            inverse_flattening = 298.257223563;
        }
        /// <summary>
        /// Creates a populated Coordinate object with an assigned GeoDate.
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="longi">longitude</param>
        /// <param name="date">DateTime (UTC)</param>
        public Coordinate(double lat, double longi, DateTime date)
        {
            FormatOptions = new CoordinateFormatOptions();
            latitude = new CoordinatePart(lat, CoordinateType.Lat);
            longitude = new CoordinatePart(longi, CoordinateType.Long);
            latitude.parent = this;
            longitude.parent = this;
            celestialInfo = new Celestial(lat, longi, date);
            geoDate = date;
            utm = new UniversalTransverseMercator(lat, longi, this);
            mgrs = new MilitaryGridReferenceSystem(utm);
            cartesian = new Cartesian(this);
            ecef = new ECEF(this);
            EagerLoadSettings = new EagerLoad();

            equatorial_radius = 6378137.0;
            inverse_flattening = 298.257223563;
        }

        /// <summary>
        /// Creates an empty Coordinates object with specificied eager loading options.
        /// </summary>
        /// <remarks>
        /// Values will need to be provided to latitude/longitude manually
        /// </remarks>
        /// <param name="eagerLoad">Eager loading options</param>
        public Coordinate(EagerLoad eagerLoad)
        {
            FormatOptions = new CoordinateFormatOptions();
            geoDate = geoDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            latitude = new CoordinatePart(CoordinateType.Lat);
            longitude = new CoordinatePart(CoordinateType.Long);
            latitude.parent = this;
            longitude.parent = this;

            if (eagerLoad.Cartesian)
            {
                cartesian = new Cartesian(this);
            }
            if (eagerLoad.Celestial)
            {
                celestialInfo = new Celestial();
            }
            if (eagerLoad.UTM_MGRS)
            {
                utm = new UniversalTransverseMercator(latitude.ToDouble(), longitude.ToDouble(), this);
                mgrs = new MilitaryGridReferenceSystem(utm);
            }
            if (eagerLoad.ECEF)
            {
                ecef = new ECEF(this);
            }
            EagerLoadSettings = eagerLoad;

            equatorial_radius = 6378137.0;
            inverse_flattening = 298.257223563;
        }
        /// <summary>
        /// Creates a populated Coordinate object with specified eager loading options.
        /// </summary>
        /// <remarks>
        /// Geodate will default to 1/1/1900 GMT until provided
        /// </remarks>
        /// <param name="lat">latitude</param>
        /// <param name="longi">longitude</param>
        /// <param name="eagerLoad">Eager loading options</param>
        public Coordinate(double lat, double longi, EagerLoad eagerLoad)
        {
            FormatOptions = new CoordinateFormatOptions();
            geoDate = geoDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            latitude = new CoordinatePart(lat, CoordinateType.Lat);
            longitude = new CoordinatePart(longi, CoordinateType.Long);
            latitude.parent = this;
            longitude.parent = this;

            if (eagerLoad.Celestial)
            {
                celestialInfo = new Celestial(lat, longi, geoDate);
            }
            if (eagerLoad.UTM_MGRS)
            {
                utm = new UniversalTransverseMercator(lat, longi, this);
                mgrs = new MilitaryGridReferenceSystem(utm);
            }
            if (eagerLoad.Cartesian)
            {
                cartesian = new Cartesian(this);
            }
            if (eagerLoad.ECEF)
            {
                ecef = new ECEF(this);
            }

            EagerLoadSettings = eagerLoad;

            equatorial_radius = 6378137.0;
            inverse_flattening = 298.257223563;
        }
        /// <summary>
        /// Creates a populated Coordinate object with specified eager load options and an assigned GeoDate.
        /// </summary>
        /// <param name="lat">Decimal format latitude</param>
        /// <param name="longi">Decimal format longitude</param>
        /// <param name="date">DateTime you wish to use for celestial calculation</param>
        /// <param name="eagerLoad">Eager loading options</param>
        public Coordinate(double lat, double longi, DateTime date, EagerLoad eagerLoad)
        {
            FormatOptions = new CoordinateFormatOptions();
            latitude = new CoordinatePart(lat, CoordinateType.Lat);
            longitude = new CoordinatePart(longi, CoordinateType.Long);
            latitude.parent = this;
            longitude.parent = this;
            geoDate = date;
            if (eagerLoad.Celestial)
            {
                celestialInfo = new Celestial(lat, longi, date);
            }

            if (eagerLoad.UTM_MGRS)
            {
                utm = new UniversalTransverseMercator(lat, longi, this);
                mgrs = new MilitaryGridReferenceSystem(utm);
            }
            if (eagerLoad.Cartesian)
            {
                cartesian = new Cartesian(this);
            }
            if (eagerLoad.ECEF)
            {
                ecef = new ECEF(this);
            }
            EagerLoadSettings = eagerLoad;

            equatorial_radius = 6378137.0;
            inverse_flattening = 298.257223563;
        }

        private CoordinatePart latitude;
        private CoordinatePart longitude;
        private UniversalTransverseMercator utm;
        private MilitaryGridReferenceSystem mgrs;
        private Cartesian cartesian;
        private ECEF ecef;
        private DateTime geoDate;
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
                        if(EagerLoadSettings.ECEF)
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

        //PARSER INDICATOR
        private Parse_Format_Type parse_Format = Parse_Format_Type.None;
        /// <summary>
        /// Used to determine what format the coordinate was parsed from.
        /// Will equal "None" if Coordinate was not initialzed via a TryParse() method.
        /// </summary>
        public Parse_Format_Type Parse_Format
        {
            get
            {
                return parse_Format;
            }
            internal set
            {
                if(parse_Format!=value)
                {
                    parse_Format = value;
                    NotifyPropertyChanged("Parse_Format");
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
        /// Initialize celestial information (required if eager loading is turned off).
        /// </summary>
        public void LoadCelestialInfo()
        {
            celestialInfo = Celestial.LoadCelestial(this);
        }
        /// <summary>
        /// Initialize UTM and MGRS information (required if eager loading is turned off).
        /// </summary>
        public void LoadUTM_MGRS_Info()
        {
            utm = new UniversalTransverseMercator(latitude.ToDouble(), longitude.ToDouble(), this);
            mgrs = new MilitaryGridReferenceSystem(utm);
        }
        /// <summary>
        /// Initialize cartesian information (required if eager loading is turned off).
        /// </summary>
        public void LoadCartesianInfo()
        {
            cartesian = new Cartesian(this);
        }
        /// <summary>
        /// Initialize ECEF information (required if eager loading is turned off).
        /// </summary>
        public void LoadECEFInfo()
        {
            ecef = new ECEF(this);
        }

        /// <summary>
        /// Coordinate string formatting options.
        /// </summary>
        public CoordinateFormatOptions FormatOptions { get; set; }
        /// <summary>
        /// Eager loading settings.
        /// </summary>
        public EagerLoad EagerLoadSettings { get; set; }

        /// <summary>
        /// Bindable formatted coordinate string.
        /// </summary>
        /// <remarks>Bind to this property when MVVM patterns used</remarks>
        public string Display
        {
            get
            {
                return Latitude.Display + " " + Longitude.Display;
            }
        }
        /// <summary>
        /// Overridden Coordinate ToString() method.
        /// </summary>
        /// <returns>string (formatted).</returns>
        public override string ToString()
        {
            string latString = latitude.ToString();
            string longSting = longitude.ToString();
            return latString + " " + longSting;
        }     

        /// <summary>
        /// Overridden Coordinate ToString() method that accepts formatting. 
        /// Refer to documentation for coordinate format options.
        /// </summary>
        /// <param name="options">CoordinateFormatOptions</param>
        /// <returns>Custom formatted coordinate</returns>
        public string ToString(CoordinateFormatOptions options)
        {
            string latString = latitude.ToString(options);
            string longSting = longitude.ToString(options);
            return latString + " " + longSting;
        }

        /// <summary>
        /// Set a custom datum for coordinate conversions and distance calculation.
        /// Objects must be loaded prior to setting if EagerLoading is turned off or else the items Datum won't be set.
        /// Use overload if EagerLoading options are used.
        /// </summary>
        /// <param name="radius">Equatorial Radius</param>
        /// <param name="flat">Inverse Flattening</param>
        public void Set_Datum(double radius, double flat)
        {
            //WGS84
            //RADIUS 6378137.0;
            //FLATTENING 298.257223563;
            if(utm != null)
            {
                utm.inverse_flattening = flat;
                utm.ToUTM(Latitude.ToDouble(), Longitude.ToDouble(), utm);
                mgrs = new MilitaryGridReferenceSystem(utm);
                NotifyPropertyChanged("UTM");
                NotifyPropertyChanged("MGRS");              
            }
            if(ecef != null)
            {
                ecef.equatorial_radius = radius;
                ecef.inverse_flattening = flat;
                ecef.ToECEF(this);
                NotifyPropertyChanged("ECEF");              
            }
            equatorial_radius = radius;
            inverse_flattening = flat;
        }

        /// <summary>
        /// Set a custom datum for coordinate conversions and distance calculation for specified coordinate formats only.
        /// Objects must be loaded prior to setting if EagerLoading is turned off.
        /// </summary>
        /// <param name="radius">Equatorial Radius</param>
        /// <param name="flat">Inverse Flattening</param>
        /// <param name="cd">Coordinate_Datum</param>
        public void Set_Datum(double radius, double flat, Coordinate_Datum cd)
        {
            //WGS84
            //RADIUS 6378137.0;
            //FLATTENING 298.257223563;
         
            if (cd.HasFlag(Coordinate_Datum.UTM_MGRS))
            {
                if(utm==null || mgrs == null) { throw new NullReferenceException("UTM/MGRS objects must be loaded prior to changing the datum."); }
                utm.inverse_flattening = flat;
                utm.ToUTM(Latitude.ToDouble(), Longitude.ToDouble(), utm);
                mgrs = new MilitaryGridReferenceSystem(utm);
                NotifyPropertyChanged("UTM");
                NotifyPropertyChanged("MGRS");
            
            }
            if (cd.HasFlag(Coordinate_Datum.ECEF))
            {
                if (ECEF==null) { throw new NullReferenceException("ECEF objects must be loaded prior to changing the datum."); }
                ecef.equatorial_radius = radius;
                ecef.inverse_flattening = flat;
                ecef.ToECEF(this);
                NotifyPropertyChanged("ECEF");
            
            }
            if (cd.HasFlag(Coordinate_Datum.LAT_LONG))
            {
                equatorial_radius = radius;
                inverse_flattening = flat;
            }
        }


        /// <summary>
        /// Returns a Distance object based on the current and specified coordinate (Haversine / Spherical Earth).
        /// </summary>
        /// <param name="c2">Coordinate</param>
        /// <returns>Distance</returns>
        public Distance Get_Distance_From_Coordinate(Coordinate c2)
        {
            return new Distance(this, c2);
        }
        /// <summary>
        /// Returns a Distance object based on the current and specified coordinate and specified earth shape.
        /// </summary>
        /// <param name="c2">Coordinate</param>
        /// <param name="shape">Earth shape</param>
        /// <returns>Distance</returns>
        public Distance Get_Distance_From_Coordinate(Coordinate c2, Shape shape)
        {
            return new Distance(this, c2, shape);
        }
      
        /// <summary>
        /// Move coordinate based on provided bearing and distance (in meters).
        /// </summary>
        /// <param name="distance">distance in meters</param>
        /// <param name="bearing">bearing</param>
        /// <param name="shape">shape of earth</param>
        public void Move(double distance, double bearing, Shape shape)
        {
            //Convert to Radians for formula
            double lat1 = latitude.ToRadians();
            double lon1 = longitude.ToRadians();
            double crs12 = bearing * Math.PI / 180; //Convert bearing to radians

            double[] ellipse = new double[] { equatorial_radius, inverse_flattening };

            if (shape == Shape.Sphere)
            {
                double[] cd = Distance_Assistant.Direct(lat1, lon1, crs12, distance);
                double lat2 = cd[0] * (180 / Math.PI);
                double lon2 = cd[1] * (180 / Math.PI);

                //ADJUST CORD
                Latitude.DecimalDegree = lat2;
                Longitude.DecimalDegree = lon2;
            }
            else
            {
                double[] cde = Distance_Assistant.Direct_Ell(lat1, -lon1, crs12, distance, ellipse);  // ellipse uses East negative
                //Convert back from radians 
                double lat2 = cde[0] * (180 / Math.PI);
                double lon2 = -cde[1] * (180 / Math.PI); // ellipse uses East negative             
                //ADJUST CORD
                Latitude.DecimalDegree = lat2;
                Longitude.DecimalDegree = lon2;
            }        
        }
       
        /// <summary>
        /// Move coordinate based on provided target coordinate and distance (in meters).
        /// </summary>
        /// <param name="c">Target coordinate</param>
        /// <param name="distance">Distance toward target in meters</param>
        /// <param name="shape">Shape of earth</param>
        public void Move(Coordinate c, double distance, Shape shape)
        {
            Distance d = new Distance(this, c, shape);
            //Convert to Radians for formula
            double lat1 = latitude.ToRadians();
            double lon1 = longitude.ToRadians();
            double crs12 = d.Bearing * Math.PI / 180; //Convert bearing to radians

            double[] ellipse = new double[] { equatorial_radius, inverse_flattening };

            if (shape == Shape.Sphere)
            {
                double[] cd = Distance_Assistant.Direct(lat1, lon1, crs12, distance);
                double lat2 = cd[0] * (180 / Math.PI);
                double lon2 = cd[1] * (180 / Math.PI);

                //ADJUST CORD
                Latitude.DecimalDegree = lat2;
                Longitude.DecimalDegree = lon2;
            }
            else
            {
                double[] cde = Distance_Assistant.Direct_Ell(lat1, -lon1, crs12, distance, ellipse);  // ellipse uses East negative
                //Convert back from radians 
                double lat2 = cde[0] * (180 / Math.PI);
                double lon2 = -cde[1] * (180 / Math.PI); // ellipse uses East negative             
                //ADJUST CORD
                Latitude.DecimalDegree = lat2;
                Longitude.DecimalDegree = lon2;
            }
        }

        /// <summary>
        /// Attempts to parse a string into a Coordinate.
        /// </summary>
        /// <param name="s">Coordinate string</param>
        /// <param name="c">Coordinate</param>
        /// <returns>boolean</returns>
        /// <example>
        /// <code>
        /// Coordinate c;
        /// if(Coordinate.TryParse("N 32.891º W 64.872º",out c))
        /// {
        ///     Console.WriteLine(c); //N 32º 53' 28.212" W 64º 52' 20.914"
        /// }
        /// </code>
        /// </example>
        public static bool TryParse(string s, out Coordinate c)
        {
            c = null;
            if (FormatFinder.TryParse(s, CartesianType.Cartesian, out c))
            {
                Parse_Format_Type pft = c.Parse_Format;
                c = new Coordinate(c.Latitude.ToDouble(), c.Longitude.ToDouble()); //Reset with EagerLoad back on.
                c.parse_Format = pft;
               
                return true;
            }
            return false;
        }
        /// <summary>
        /// Attempts to parse a string into a Coordinate with specified DateTime
        /// </summary>
        /// <param name="s">Coordinate string</param>
        /// <param name="geoDate">GeoDate</param>
        /// <param name="c">Coordinate</param>
        /// <returns>boolean</returns>
        /// <example>
        /// <code>
        /// Coordinate c;
        /// if(Coordinate.TryParse("N 32.891º W 64.872º", new DateTime(2018,7,7), out c))
        /// {
        ///     Console.WriteLine(c); //N 32º 53' 28.212" W 64º 52' 20.914"
        /// }
        /// </code>
        /// </example>
        public static bool TryParse(string s, DateTime geoDate, out Coordinate c)
        {
            c = null;
            if (FormatFinder.TryParse(s, CartesianType.Cartesian, out c))
            {
                Parse_Format_Type pft = c.Parse_Format;
                c = new Coordinate(c.Latitude.ToDouble(), c.Longitude.ToDouble(), geoDate); //Reset with EagerLoad back on.
                c.parse_Format = pft;
              
                return true;
            }
            return false;
        }
        /// <summary>
        /// Attempts to parse a string into a Coordinate.
        /// </summary>
        /// <param name="s">Coordinate string</param>
        /// <param name="c">Coordinate</param>
        /// <param name="ct">Cartesian Type</param>
        /// <returns>boolean</returns>
        /// <example>
        /// <code>
        /// Coordinate c;
        /// if(Coordinate.TryParse("N 32.891º W 64.872º", CartesianType.Cartesian, out c))
        /// {
        ///     Console.WriteLine(c); //N 32º 53' 28.212" W 64º 52' 20.914"
        /// }
        /// </code>
        /// </example>
        public static bool TryParse(string s, CartesianType ct, out Coordinate c)
        {
            c = null;
            if (FormatFinder.TryParse(s, ct, out c))
            {
                Parse_Format_Type pft = c.Parse_Format;
                if (ct == CartesianType.ECEF)
                {
                    Distance h = c.ecef.GeoDetic_Height;
                    c = new Coordinate(c.Latitude.ToDouble(), c.Longitude.ToDouble()); //Reset with EagerLoad back on.
                    c.ecef.Set_GeoDetic_Height(c, h);
                }
                else
                {
                    c = new Coordinate(c.Latitude.ToDouble(), c.Longitude.ToDouble()); //Reset with EagerLoad back on.
                }
                c.parse_Format = pft;
               
                return true;
            }
            return false;
        }
        /// <summary>
        /// Attempts to parse a string into a Coordinate with specified DateTime
        /// </summary>
        /// <param name="s">Coordinate string</param>
        /// <param name="geoDate">GeoDate</param>
        /// <param name="c">Coordinate</param>
        /// <param name="ct">Cartesian Type</param>
        /// <returns>boolean</returns>
        /// <example>
        /// <code>
        /// Coordinate c;
        /// if(Coordinate.TryParse("N 32.891º W 64.872º", new DateTime(2018,7,7), CartesianType.Cartesian, out c))
        /// {
        ///     Console.WriteLine(c); //N 32º 53' 28.212" W 64º 52' 20.914"
        /// }
        /// </code>
        /// </example>
        public static bool TryParse(string s, DateTime geoDate, CartesianType ct, out Coordinate c)
        {
            c = null;
            if (FormatFinder.TryParse(s, ct, out c))
            {
                Parse_Format_Type pft = c.Parse_Format;
                if (ct == CartesianType.ECEF)
                {
                    Distance h = c.ecef.GeoDetic_Height;
                    c = new Coordinate(c.Latitude.ToDouble(), c.Longitude.ToDouble(), geoDate); //Reset with EagerLoad back on.
                    c.ecef.Set_GeoDetic_Height(c, h);
                }            
                else
                {
                    c = new Coordinate(c.Latitude.ToDouble(), c.Longitude.ToDouble(), geoDate); //Reset with EagerLoad back on.
                }
                c.parse_Format = pft;
             
                return true;
            }
            return false;
        }

        /// <summary>
        /// Property changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Notify property changed
        /// </summary>
        /// <param name="propName">Property name</param>
        public void NotifyPropertyChanged(string propName)
        {
            switch (propName)
            {
                case "CelestialInfo":
                    if(!EagerLoadSettings.Celestial || celestialInfo == null) { return; } //Prevent Null Exceptions and calls while eagerloading is off
                    celestialInfo.CalculateCelestialTime(latitude.DecimalDegree, longitude.DecimalDegree, geoDate);
                    break;
                case "UTM":
                    if (!EagerLoadSettings.UTM_MGRS || UTM == null) { return; } 
                    utm.ToUTM(latitude.ToDouble(), longitude.ToDouble(), utm);
                    break;
                case "utm":
                    //Adjust case and notify of change. 
                    //Use to notify without calling ToUTM()
                    propName = "UTM";
                    break;
                case "MGRS":
                    if (!EagerLoadSettings.UTM_MGRS || MGRS == null) { return; }
                    MGRS.ToMGRS(utm);
                    break;
                case "Cartesian":
                    if (!EagerLoadSettings.Cartesian || Cartesian == null) { return; }
                    Cartesian.ToCartesian(this);
                    break;
                case "ECEF":
                    if (!EagerLoadSettings.ECEF) { return; }
                    ECEF.ToECEF(this);
                    break;            
                default:
                    break;
            }
            if (PropertyChanged != null)
            {                         
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
    /// <summary>
    /// Observable class for handling latitudinal and longitudinal coordinate parts.
    /// </summary>
    /// <remarks>
    /// Objects can be passed to Coordinate object Latitude and Longitude properties.
    /// </remarks>
    [Serializable]
    public class CoordinatePart : INotifyPropertyChanged
    {       
        //Defaults:
        //Format: Degrees Minutes Seconds
        //Rounding: Dependent upon selected format
        //Leading Zeros: False
        //Trailing Zeros: False
        //Display Symbols: True (All Symbols display)
        //Display Hyphens: False
        //Position Display: First                               

        private double decimalDegree;
        private double decimalMinute;
        private int degrees;
        private int minutes;
        private double seconds;
        private CoordinatesPosition position;
        private CoordinateType type;

        internal Coordinate parent;
        /// <summary>
        /// Used to determine and notify the CoordinatePart parent Coordinate object.
        /// </summary>
        public Coordinate Parent { get { return parent; } }

        /// <summary>
        /// Observable decimal format coordinate.
        /// </summary>
        public double DecimalDegree
        {
            get { return decimalDegree; }
            set
            {
                //If changing, notify the needed property changes
                if (decimalDegree != value)
                {
                    //Validate the value
                    if (type == CoordinateType.Lat)
                    {
                        if (value > 90)
                        {
                            throw new ArgumentOutOfRangeException("Degrees out of range", "Latitude degrees cannot be greater than 90");
                        }
                        if (value < -90)
                        {
                            throw new ArgumentOutOfRangeException("Degrees out of range", "Latitude degrees cannot be less than -90");
                        }

                    }
                    if (type == CoordinateType.Long)
                    {
                        if (value > 180)
                        {
                            throw new ArgumentOutOfRangeException("Degrees out of range", "Longitude degrees cannot be greater than 180");
                        }
                        if (value < -180)
                        {
                            throw new ArgumentOutOfRangeException("Degrees out of range", "Longitude degrees cannot be less than -180");
                        }

                    }
                    decimalDegree = value;
                   
                    //Update Position
                    if ((position == CoordinatesPosition.N || position == CoordinatesPosition.E) && decimalDegree < 0)
                    {
                        if (type == CoordinateType.Lat) { position = CoordinatesPosition.S; }
                        else { position = CoordinatesPosition.W; }
                       
                    }
                    if ((position == CoordinatesPosition.W || position == CoordinatesPosition.S) && decimalDegree >= 0)
                    {
                        if (type == CoordinateType.Lat) { position = CoordinatesPosition.N; }
                        else { position = CoordinatesPosition.E; }
                      
                    }
                    //Update the Degree & Decimal Minute
                    double degABS = Math.Abs(decimalDegree); //Make decimalDegree positive for calculations
                    double degFloor = Math.Truncate(degABS); //Truncate the number leftto extract the degree
                    decimal f = Convert.ToDecimal(degFloor); //Convert to degree to decimal to keep precision during calculations
                    decimal ddm = Convert.ToDecimal(degABS) - f; //Extract decimalMinute value from decimalDegree
                    ddm *= 60; //Multiply by 60 to get readable decimalMinute

                    double dm = Convert.ToDouble(ddm); //Convert decimalMinutes back to double for storage
                    int df = Convert.ToInt32(degFloor); //Convert degrees to int for storage

                    if (degrees != df)
                    {
                        degrees = df;
                      
                    }
                    if (decimalMinute != dm)
                    {
                        decimalMinute = dm;
                   
                    }
                    //Update Minutes Seconds              
                    double dmFloor = Math.Floor(dm); //Get number left of decimal to grab minute value
                    int mF = Convert.ToInt32(dmFloor); //Convert minute to int for storage
                    f = Convert.ToDecimal(dmFloor); //Create a second minute value and store as decimal for precise calculation

                    decimal s = ddm - f; //Get seconds from minutes
                    s *= 60; //Multiply by 60 to get readable seconds
                    double secs = Convert.ToDouble(s); //Convert back to double for storage

                    if (minutes != mF)
                    {
                        minutes = mF;
                      
                    }
                    if (seconds != secs)
                    {
                        seconds = secs;                    
                    }
                    NotifyProperties(PropertyTypes.DecimalDegree);
                }
            }
        }
        /// <summary>
        /// Observable decimal format minute.
        /// </summary>
        public double DecimalMinute
        {
            get { return decimalMinute; }
            set
            {
                if (decimalMinute != value)
                {
                    if (value < 0) { value *= -1; }//Adjust accidental negative input
                    //Validate values     
                   
                    decimal dm = Math.Abs(Convert.ToDecimal(value)) / 60;
                    double decMin = Convert.ToDouble(dm);
                    if (type == CoordinateType.Lat)
                    {

                        if (degrees + decMin > 90) { throw new ArgumentOutOfRangeException("Degrees out of range", "Latitudinal degrees cannot be greater than 90"); }
                    }
                    else
                    {
                        if (degrees + decMin > 180) { throw new ArgumentOutOfRangeException("Degrees out of range", "Longitudinal degrees cannot be greater than 180"); }
                    }
                    if (value >= 60) { throw new ArgumentOutOfRangeException("Minutes out of range", "Coordinate Minutes cannot be greater than or equal to 60"); }
                    if (value < 0) { throw new ArgumentOutOfRangeException("Minutes out of range", "Coordinate Minutes cannot be less than 0"); }


                    decimalMinute = value;
                   

                    decimal decValue = Convert.ToDecimal(value); //Convert value to decimal for precision during calculation
                    decimal dmFloor = Math.Floor(decValue); //Extract minutes
                    decimal secs = decValue - dmFloor; //Extract seconds
                    secs *= 60; //Convert seconds to human readable format

                    decimal newDM = decValue / 60; //divide decimalMinute by 60 to get storage value
                    decimal newDD = degrees + newDM;//Add new decimal value to the floor degree value to get new decimalDegree;
                    if (decimalDegree < 0) { newDD = newDD * -1; } //Restore negative if needed

                    decimalDegree = Convert.ToDouble(newDD);  //Convert back to double for storage                      
                   

                    minutes = Convert.ToInt32(dmFloor); //Convert minutes to int for storage
                   
                    seconds = Convert.ToDouble(secs); //Convert seconds to double for storage 
                    NotifyProperties(PropertyTypes.DecimalMinute);              
                }
            }

        }
        /// <summary>
        /// Observable coordinate degree.
        /// </summary>
        public int Degrees
        {
            get { return degrees; }
            set
            {              
                //Validate Value
                if (degrees != value)
                {
                   
                    if (value < 0) { value *= -1; }//Adjust accidental negative input
                    
                    if (type == CoordinateType.Lat)
                    {
                        if (value + decimalMinute /100.0 > 90)
                        {
                            throw new ArgumentOutOfRangeException("Degrees", "Latitude degrees cannot be greater than 90");
                        }
                    }
                    if (type == CoordinateType.Long)
                    {                    
                        if (value + decimalMinute /100.0 > 180)
                        {
                            throw new ArgumentOutOfRangeException("Degrees", "Longitude degrees cannot be greater than 180");
                        }

                    }

                    decimal f = Convert.ToDecimal(degrees);

                    degrees = value;

                    double degABS = Math.Abs(decimalDegree); //Make decimalDegree positive for calculations
                    decimal dDec = Convert.ToDecimal(degABS); //Convert to Decimal for precision during calculations              
                                                              //Convert degrees to decimal to keep precision        
                    decimal dm = dDec - f; //Extract minutes                                      
                    decimal newDD = degrees + dm; //Add minutes to new degree for decimalDegree
                 
                    if (decimalDegree < 0) { newDD *= -1; } //Set negative as required
                   
                    decimalDegree = Convert.ToDouble(newDD); // Convert decimalDegree to double for storage
                    NotifyProperties(PropertyTypes.Degree);
                }
            }
        }
        /// <summary>
        /// Observable coordinate minute.
        /// </summary>
        public int Minutes
        {
            get { return minutes; }
            set
            {
                if (minutes != value)
                {
                    if (value < 0) { value *= -1; }//Adjust accidental negative input
                    //Validate the minutes
                    decimal vMin = Convert.ToDecimal(value);
                    if (type == CoordinateType.Lat)
                    {
                        if (degrees + (vMin / 60) > 90) { throw new ArgumentOutOfRangeException("Degrees out of range", "Latitudinal degrees cannot be greater than 90"); }
                    }
                    else
                    {
                        if (degrees + (vMin / 60) > 180) { throw new ArgumentOutOfRangeException("Degrees out of range", "Longitudinal degrees cannot be greater than 180"); }
                    }
                    if (value >= 60)
                    {
                        throw new ArgumentOutOfRangeException("Minutes out of range", "Minutes cannot be greater than or equal to 60");
                    }
                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException("Minutes out of range", "Minutes cannot be less than 0");
                    }
                    decimal minFloor = Convert.ToDecimal(minutes);//Convert decimal to minutes for calculation
                    decimal f = Convert.ToDecimal(degrees); //Convert to degree to keep precision during calculation 

                    minutes = value;
                   

                    double degABS = Math.Abs(decimalDegree); //Make decimalDegree positive
                    decimal dDec = Convert.ToDecimal(degABS); //Convert to decimalDegree for precision during calucation                        

                    decimal dm = dDec - f; //Extract minutes
                    dm *= 60; //Make minutes human readable

                    decimal secs = dm - minFloor;//Extract Seconds

                    decimal newDM = minutes + secs;//Add seconds to minutes for decimalMinute
                    double decMin = Convert.ToDouble(newDM); //Convert decimalMinute to double for storage
                    decimalMinute = decMin; //Round to correct precision
                   

                    newDM /= 60; //Convert decimalMinute to storage format
                    decimal newDeg = f + newDM; //Add value to degree for decimalDegree
                    if (decimalDegree < 0) { newDeg *= -1; }// Set to negative as required.
                    decimalDegree = Convert.ToDouble(newDeg);//Convert to double and roun to correct precision for storage
                    NotifyProperties(PropertyTypes.Minute);
                }
            }
        }
        /// <summary>
        /// Observable coordinate second.
        /// </summary>
        public double Seconds
        {
            get { return seconds; }
            set
            {
                if (value < 0) { value *= -1; }//Adjust accidental negative input
                if (seconds != value)
                {
                    //Validate Seconds
                    decimal vSec = Convert.ToDecimal(value);
                    vSec /= 60;

                    decimal vMin = Convert.ToDecimal(minutes);
                    vMin += vSec;
                    vMin /= 60;

                    if (type == CoordinateType.Lat)
                    {
                        if (degrees + vMin > 90) { throw new ArgumentOutOfRangeException("Degrees out of range", "Latitudinal degrees cannot be greater than 90"); }
                    }
                    else
                    {
                        if (degrees + vMin > 180) { throw new ArgumentOutOfRangeException("Degrees out of range", "Longitudinal degrees cannot be greater than 180"); }
                    }
                    if (value >= 60)
                    {
                        throw new ArgumentOutOfRangeException("Seconds out of range", "Seconds cannot be greater than or equal to 60");
                    }
                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException("Seconds out of range", "Seconds cannot be less than 0");
                    }
                    seconds = value;
                 

                    double degABS = Math.Abs(decimalDegree); //Make decimalDegree positive
                    double degFloor = Math.Truncate(degABS); //Truncate the number left of the decimal
                    decimal f = Convert.ToDecimal(degFloor); //Convert to decimal to keep precision

                    decimal secs = Convert.ToDecimal(seconds); //Convert seconds to decimal for calculations
                    secs /= 60; //Convert to storage format
                    decimal dm = minutes + secs;//Add seconds to minutes for decimalMinute
                    double minFD = Convert.ToDouble(dm); //Convert decimalMinute for storage
                    decimalMinute = minFD;//Round to proper precision
                  
                    decimal nm = Convert.ToDecimal(decimalMinute) / 60;//Convert decimalMinute to decimal and divide by 60 to get storage format decimalMinute
                    double newDeg = degrees + Convert.ToDouble(nm);//Convert to double and add to degree for storage decimalDegree
                    if (decimalDegree < 0) { newDeg *= -1; }//Make negative as needed
                    decimalDegree = newDeg;//Update decimalDegree and round to proper precision    
                    NotifyProperties(PropertyTypes.Second);
                }
            }
        }       
        /// <summary>
        /// Formate coordinate part string.
        /// </summary>
        public string Display
        {
            get 
            {
                if (parent != null)
                {
                    return ToString(parent.FormatOptions);
                }
                else
                {
                    return ToString();
                }
            }
        }
        /// <summary>
        /// Observable coordinate position.
        /// </summary>
        public CoordinatesPosition Position
        {
            get { return position; }
            set
            {
                if (position != value)
                {
                    if (type == CoordinateType.Long && (value == CoordinatesPosition.N || value == CoordinatesPosition.S))
                    {
                        throw new InvalidOperationException("You cannot change a Longitudinal type coordinate into a Latitudinal");
                    }
                    if (type == CoordinateType.Lat && (value == CoordinatesPosition.E || value == CoordinatesPosition.W))
                    {
                        throw new InvalidOperationException("You cannot change a Latitudinal type coordinate into a Longitudinal");
                    }
                    decimalDegree *= -1; // Change the position
                    position = value;
                    NotifyProperties(PropertyTypes.Position);
                }
            }
        }

        /// <summary>
        /// Creates an empty CoordinatePart.
        /// </summary>
        /// <param name="t">CoordinateType</param>
        /// <param name="c">Parent Coordinate object</param>
        [Obsolete("Method is deprecated. You no longer need to pass a Coordinate object through the constructor.")]
        public CoordinatePart(CoordinateType t, Coordinate c)
        {     
            parent = c;
            type = t;
            decimalDegree = 0;
            degrees = 0;
            minutes = 0;
            seconds = 0;
            if (type == CoordinateType.Lat) { position = CoordinatesPosition.N; }
            else { position = CoordinatesPosition.E; }
        }
        /// <summary>
        /// Creates a populated CoordinatePart from a decimal format part.
        /// </summary>
        /// <param name="value">Coordinate decimal value</param>
        /// <param name="t">Coordinate type</param>
        /// <param name="c">Parent Coordinate object</param>
        [Obsolete("Method is deprecated. You no longer need to pass a Coordinate object through the constructor.")]
        public CoordinatePart(double value, CoordinateType t, Coordinate c)
        {
            parent = c;
            type = t;

            if (type == CoordinateType.Long)
            {
                if (value > 180) { throw new ArgumentOutOfRangeException("Degrees out of range", "Longitudinal coordinate decimal cannot be greater than 180."); }
                if (value < -180) { throw new ArgumentOutOfRangeException("Degrees out of range", "Longitudinal coordinate decimal cannot be less than 180."); }
                if (value < 0) { position = CoordinatesPosition.W; }
                else { position = CoordinatesPosition.E; }
            }
            else
            {
                if (value > 90) { throw new ArgumentOutOfRangeException("Degrees out of range", "Latitudinal coordinate decimal cannot be greater than 90."); }
                if (value < -90) { throw new ArgumentOutOfRangeException("Degrees out of range", "Latitudinal coordinate decimal cannot be less than 90."); }
                if (value < 0) { position = CoordinatesPosition.S; }
                else { position = CoordinatesPosition.N; }
            }
            decimal dd = Convert.ToDecimal(value);
            dd = Math.Abs(dd);
            decimal ddFloor = Math.Floor(dd);//DEGREE
            decimal dm = dd - ddFloor;
            dm *= 60; //DECIMAL MINUTE
            decimal dmFloor = Math.Floor(dm); //MINUTES
            decimal sec = dm - dmFloor;
            sec *= 60;//SECONDS


            decimalDegree = value;
            degrees = Convert.ToInt32(ddFloor);
            minutes = Convert.ToInt32(dmFloor);
            decimalMinute = Convert.ToDouble(dm);
            seconds = Convert.ToDouble(sec);
        }
        /// <summary>
        /// Creates a populated CoordinatePart object from a Degrees Minutes Seconds part.
        /// </summary>
        /// <param name="deg">Degrees</param>
        /// <param name="min">Minutes</param>
        /// <param name="sec">Seconds</param>
        /// <param name="pos">Coordinate Part Position</param>
        /// <param name="c">Parent Coordinate</param>
        [Obsolete("Method is deprecated. You no longer need to pass a Coordinate object through the constructor.")]
        public CoordinatePart(int deg, int min, double sec, CoordinatesPosition pos, Coordinate c)
        {
            parent = c;
            if (pos == CoordinatesPosition.N || pos == CoordinatesPosition.S) { type = CoordinateType.Lat; }
            else { type = CoordinateType.Long; }

            if (deg < 0) { throw new ArgumentOutOfRangeException("Degrees out of range", "Degrees cannot be less than 0."); }
            if (min < 0) { throw new ArgumentOutOfRangeException("Minutes out of range", "Minutes cannot be less than 0."); }
            if (sec < 0) { throw new ArgumentOutOfRangeException("Seconds out of range", "Seconds cannot be less than 0."); }
            if (min >= 60) { throw new ArgumentOutOfRangeException("Minutes out of range", "Minutes cannot be greater than or equal to 60."); }
            if (sec >= 60) { throw new ArgumentOutOfRangeException("Seconds out of range", "Seconds cannot be greater than or equal to 60."); }
            degrees = deg;
            minutes = min;
            seconds = sec;
            position = pos;

            decimal secD = Convert.ToDecimal(sec);
            secD /= 60; //Decimal Seconds
            decimal minD = Convert.ToDecimal(min);
            minD += secD; //Decimal Minutes

            if (type == CoordinateType.Long)
            {           
                if (deg + (minD / 60) > 180) { throw new ArgumentOutOfRangeException("Degrees out of range", "Longitudinal Degrees cannot be greater than 180."); }
            }
            else
            {
                if (deg + (minD / 60) > 90) { throw new ArgumentOutOfRangeException("Degrees out of range", "Latitudinal Degrees cannot be greater than 90."); }
            }
            decimalMinute = Convert.ToDouble(minD);
            decimal dd = Convert.ToDecimal(deg) + (minD / 60);


            if (pos == CoordinatesPosition.S || pos == CoordinatesPosition.W)
            {
                dd *= -1;
            }
            decimalDegree = Convert.ToDouble(dd);
        }
        /// <summary>
        /// Creates a populated CoordinatePart from a Degrees Minutes Seconds part.
        /// </summary>
        /// <param name="deg">Degrees</param>
        /// <param name="minSec">Decimal Minutes</param> 
        /// <param name="pos">Coordinate Part Position</param>
        /// <param name="c">Parent Coordinate object</param>
        [Obsolete("Method is deprecated. You no longer need to pass a Coordinate object through the constructor.")]
        public CoordinatePart(int deg, double minSec, CoordinatesPosition pos, Coordinate c)
        {
            parent = c;
         
            if (pos == CoordinatesPosition.N || pos == CoordinatesPosition.S) { type = CoordinateType.Lat; }
            else { type = CoordinateType.Long; }

            if (deg < 0) { throw new ArgumentOutOfRangeException("Degree out of range", "Degree cannot be less than 0."); }
            if (minSec < 0) { throw new ArgumentOutOfRangeException("Minutes out of range", "Minutes cannot be less than 0."); }

            if (minSec >= 60) { throw new ArgumentOutOfRangeException("Minutes out of range", "Minutes cannot be greater than or equal to 60."); }

            if (type == CoordinateType.Lat)
            {
                if (deg + (minSec / 60) > 90) { throw new ArgumentOutOfRangeException("Degree out of range", "Latitudinal degrees cannot be greater than 90."); }
            }
            else
            {
                if (deg + (minSec / 60) > 180) { throw new ArgumentOutOfRangeException("Degree out of range", "Longitudinal degrees cannot be greater than 180."); }
            }
            degrees = deg;
            decimalMinute = minSec;
            position = pos;

            decimal minD = Convert.ToDecimal(minSec);
            decimal minFloor = Math.Floor(minD);
            minutes = Convert.ToInt32(minFloor);
            decimal sec = minD - minFloor;
            sec *= 60;
            decimal secD = Convert.ToDecimal(sec);
            seconds = Convert.ToDouble(secD);
            decimal dd = deg + (minD / 60);

            if (pos == CoordinatesPosition.S || pos == CoordinatesPosition.W)
            {
                dd *= -1;
            }
            decimalDegree = Convert.ToDouble(dd);
        }

        /// <summary>
        /// Creates an empty CoordinatePart.
        /// </summary>
        /// <param name="t">CoordinateType</param>
        public CoordinatePart(CoordinateType t)
        {           
            type = t;
            decimalDegree = 0;
            degrees = 0;
            minutes = 0;
            seconds = 0;
            if (type == CoordinateType.Lat) { position = CoordinatesPosition.N; }
            else { position = CoordinatesPosition.E; }
        }
        /// <summary>
        /// Creates a populated CoordinatePart from a decimal format part.
        /// </summary>
        /// <param name="value">Coordinate decimal value</param>
        /// <param name="t">Coordinate type</param>
        public CoordinatePart(double value, CoordinateType t)
        {
            type = t;

            if (type == CoordinateType.Long)
            {
                if (value > 180) { throw new ArgumentOutOfRangeException("Degrees out of range", "Longitudinal coordinate decimal cannot be greater than 180."); }
                if (value < -180) { throw new ArgumentOutOfRangeException("Degrees out of range", "Longitudinal coordinate decimal cannot be less than 180."); }
                if (value < 0) { position = CoordinatesPosition.W; }
                else { position = CoordinatesPosition.E; }
            }
            else
            {
                if (value > 90) { throw new ArgumentOutOfRangeException("Degrees out of range", "Latitudinal coordinate decimal cannot be greater than 90."); }
                if (value < -90) { throw new ArgumentOutOfRangeException("Degrees out of range", "Latitudinal coordinate decimal cannot be less than 90."); }
                if (value < 0) { position = CoordinatesPosition.S; }
                else { position = CoordinatesPosition.N; }
            }
            decimal dd = Convert.ToDecimal(value);
            dd = Math.Abs(dd);
            decimal ddFloor = Math.Floor(dd);//DEGREE
            decimal dm = dd - ddFloor;
            dm *= 60; //DECIMAL MINUTE
            decimal dmFloor = Math.Floor(dm); //MINUTES
            decimal sec = dm - dmFloor;
            sec *= 60;//SECONDS


            decimalDegree = value;
            degrees = Convert.ToInt32(ddFloor);
            minutes = Convert.ToInt32(dmFloor);
            decimalMinute = Convert.ToDouble(dm);
            seconds = Convert.ToDouble(sec);
        }
        /// <summary>
        /// Creates a populated CoordinatePart object from a Degrees Minutes Seconds part.
        /// </summary>
        /// <param name="deg">Degrees</param>
        /// <param name="min">Minutes</param>
        /// <param name="sec">Seconds</param>
        /// <param name="pos">Coordinate Part Position</param>
        public CoordinatePart(int deg, int min, double sec, CoordinatesPosition pos)
        {
            if (pos == CoordinatesPosition.N || pos == CoordinatesPosition.S) { type = CoordinateType.Lat; }
            else { type = CoordinateType.Long; }

            if (deg < 0) { throw new ArgumentOutOfRangeException("Degrees out of range", "Degrees cannot be less than 0."); }
            if (min < 0) { throw new ArgumentOutOfRangeException("Minutes out of range", "Minutes cannot be less than 0."); }
            if (sec < 0) { throw new ArgumentOutOfRangeException("Seconds out of range", "Seconds cannot be less than 0."); }
            if (min >= 60) { throw new ArgumentOutOfRangeException("Minutes out of range", "Minutes cannot be greater than or equal to 60."); }
            if (sec >= 60) { throw new ArgumentOutOfRangeException("Seconds out of range", "Seconds cannot be greater than or equal to 60."); }
            degrees = deg;
            minutes = min;
            seconds = sec;
            position = pos;

            decimal secD = Convert.ToDecimal(sec);
            secD /= 60; //Decimal Seconds
            decimal minD = Convert.ToDecimal(min);
            minD += secD; //Decimal Minutes

            if (type == CoordinateType.Long)
            {
                if (deg + (minD / 60) > 180) { throw new ArgumentOutOfRangeException("Degrees out of range", "Longitudinal Degrees cannot be greater than 180."); }
            }
            else
            {
                if (deg + (minD / 60) > 90) { throw new ArgumentOutOfRangeException("Degrees out of range", "Latitudinal Degrees cannot be greater than 90."); }
            }
            decimalMinute = Convert.ToDouble(minD);
            decimal dd = Convert.ToDecimal(deg) + (minD / 60);


            if (pos == CoordinatesPosition.S || pos == CoordinatesPosition.W)
            {
                dd *= -1;
            }
            decimalDegree = Convert.ToDouble(dd);
        }
        /// <summary>
        /// Creates a populated CoordinatePart from a Degrees Minutes Seconds part.
        /// </summary>
        /// <param name="deg">Degrees</param>
        /// <param name="minSec">Decimal Minutes</param> 
        /// <param name="pos">Coordinate Part Position</param>
        public CoordinatePart(int deg, double minSec, CoordinatesPosition pos)
        {            
            if (pos == CoordinatesPosition.N || pos == CoordinatesPosition.S) { type = CoordinateType.Lat; }
            else { type = CoordinateType.Long; }

            if (deg < 0) { throw new ArgumentOutOfRangeException("Degree out of range", "Degree cannot be less than 0."); }
            if (minSec < 0) { throw new ArgumentOutOfRangeException("Minutes out of range", "Minutes cannot be less than 0."); }

            if (minSec >= 60) { throw new ArgumentOutOfRangeException("Minutes out of range", "Minutes cannot be greater than or equal to 60."); }

            if (type == CoordinateType.Lat)
            {
                if (deg + (minSec / 60) > 90) { throw new ArgumentOutOfRangeException("Degree out of range", "Latitudinal degrees cannot be greater than 90."); }
            }
            else
            {
                if (deg + (minSec / 60) > 180) { throw new ArgumentOutOfRangeException("Degree out of range", "Longitudinal degrees cannot be greater than 180."); }
            }
            degrees = deg;
            decimalMinute = minSec;
            position = pos;

            decimal minD = Convert.ToDecimal(minSec);
            decimal minFloor = Math.Floor(minD);
            minutes = Convert.ToInt32(minFloor);
            decimal sec = minD - minFloor;
            sec *= 60;
            decimal secD = Convert.ToDecimal(sec);
            seconds = Convert.ToDouble(secD);
            decimal dd = deg + (minD / 60);

            if (pos == CoordinatesPosition.S || pos == CoordinatesPosition.W)
            {
                dd *= -1;
            }
            decimalDegree = Convert.ToDouble(dd);
        }

        /// <summary>
        /// Signed degrees (decimal) format coordinate.
        /// </summary>
        /// <returns>double</returns>
        public double ToDouble()
        {
            return decimalDegree;
        }

        /// <summary>
        /// Overridden Coordinate ToString() method
        /// </summary>
        /// <returns>Dstring</returns>
        public override string ToString()
        {
            if(parent==null)
            {
                return FormatString(new CoordinateFormatOptions());
            }
            return FormatString(Parent.FormatOptions);
        }

        /// <summary>
        /// Formatted CoordinatePart string.
        /// </summary>
        /// <param name="options">CoordinateFormatOptions</param>
        /// <returns>string (formatted)</returns>
        public string ToString(CoordinateFormatOptions options)
        {
            return FormatString(options);
        }
       /// <summary>
        /// String formatting logic
        /// </summary>
        /// <param name="options">CoordinateFormatOptions</param>
        /// <returns>Formatted coordinate part string</returns>
        private string FormatString(CoordinateFormatOptions options)
        {
            ToStringType type = ToStringType.Degree_Minute_Second;
            int? rounding = null;
            bool lead = false;
            bool trail = false;
            bool hyphen = false;
            bool symbols = true;
            bool degreeSymbol = true;
            bool minuteSymbol = true;
            bool secondsSymbol = true;
            bool positionFirst = true;           

            #region Assign Formatting Rules
            switch (options.Format)
            {
                case CoordinateFormatType.Degree_Minutes_Seconds:
                    type = ToStringType.Degree_Minute_Second;
                    break;
                case CoordinateFormatType.Degree_Decimal_Minutes:
                    type = ToStringType.Degree_Decimal_Minute;
                    break;
                case CoordinateFormatType.Decimal_Degree:
                    type = ToStringType.Decimal_Degree;
                    break;
                case CoordinateFormatType.Decimal:
                    type = ToStringType.Decimal;
                    break;
                default:
                    type = ToStringType.Degree_Minute_Second;
                    break;
            }
            rounding = options.Round;
            lead = options.Display_Leading_Zeros;
            trail = options.Display_Trailing_Zeros;
            symbols = options.Display_Symbols;
            degreeSymbol = options.Display_Degree_Symbol;
            minuteSymbol = options.Display_Minute_Symbol;
            secondsSymbol = options.Display_Seconds_Symbol;
            hyphen = options.Display_Hyphens;
            positionFirst = options.Position_First;                     
            #endregion

            switch (type)
            {
                case ToStringType.Decimal_Degree:
                    if (rounding == null) { rounding = 6; }
                    return ToDecimalDegreeString(rounding.Value, lead, trail, symbols, degreeSymbol, positionFirst, hyphen);
                case ToStringType.Degree_Decimal_Minute:
                    if (rounding == null) { rounding = 3; }
                    return ToDegreeDecimalMinuteString(rounding.Value, lead, trail, symbols, degreeSymbol, minuteSymbol, hyphen, positionFirst);
                case ToStringType.Degree_Minute_Second:
                    if (rounding == null) { rounding = 3; }
                    return ToDegreeMinuteSecondString(rounding.Value, lead, trail, symbols, degreeSymbol, minuteSymbol, secondsSymbol, hyphen, positionFirst);
                case ToStringType.Decimal:
                    if (rounding == null) { rounding = 9; }
                    double dub = ToDouble();
                    dub = Math.Round(dub, rounding.Value);
                    string lt = Leading_Trailing_Format(lead, trail, rounding.Value, Position);
                    return string.Format(lt, dub);
            }

            return string.Empty;
        }
        //DMS Coordinate Format
        private string ToDegreeMinuteSecondString(int rounding, bool lead, bool trail, bool symbols, bool degreeSymbol, bool minuteSymbol, bool secondSymbol, bool hyphen, bool positionFirst)
        {

            string leadString = Leading_Trailing_Format(lead, false, rounding, Position);
            string d = string.Format(leadString, Degrees); // Degree String
            string minute;
            if (lead) { minute = string.Format("{0:00}", Minutes); }
            else { minute = Minutes.ToString(); }
            string leadTrail = Leading_Trailing_Format(lead, trail, rounding);

            double sc = Math.Round(Seconds, rounding);
            string second = string.Format(leadTrail, sc);
            string hs = " ";
            string ds = "";
            string ms = "";
            string ss = "";
            if (symbols)
            {
                if (degreeSymbol) { ds = "º"; }
                if (minuteSymbol) { ms = "'"; }
                if (secondSymbol) { ss = "\""; }
            }
            if (hyphen) { hs = "-"; }

            if (positionFirst) { return Position.ToString() + hs + d + ds + hs + minute + ms + hs + second + ss; }
            else { return d + ds + hs + minute + ms + hs + second + ss + hs + Position.ToString(); }
        }
        //DDM Coordinate Format
        private string ToDegreeDecimalMinuteString(int rounding, bool lead, bool trail, bool symbols, bool degreeSymbol, bool minuteSymbol, bool hyphen, bool positionFirst)
        {
            string leadString = "{0:0";
            if (lead)
            {
                if (Position == CoordinatesPosition.E || Position == CoordinatesPosition.W)
                {
                    leadString += "00";
                }
                else
                {
                    leadString += "0";
                }
            }
            leadString += "}";
            string d = string.Format(leadString, Degrees); // Degree String

            string leadTrail = "{0:0";
            if (lead)
            {
                leadTrail += "0";
            }
            leadTrail += ".";
            if (trail)
            {
                for (int i = 0; i < rounding; i++)
                {
                    leadTrail += "0";
                }
            }
            else
            {
                leadTrail += "#########";
            }
            leadTrail += "}";

            double ns = Seconds / 60;
            double c = Math.Round(Minutes + ns, rounding);
            if(c == 60 && Degrees+1 <91) { c = 0;d = string.Format(leadString, Degrees + 1); }//Adjust for rounded maxed out Seconds. will Convert 42 60.0 to 43
            string ms = string.Format(leadTrail, c);
            string hs = " ";
            string ds = "";
            string ss = "";
            if (symbols)
            {
                if (degreeSymbol) { ds = "º"; }
                if (minuteSymbol) { ss = "'"; }
            }
            if (hyphen) { hs = "-"; }

            if (positionFirst) { return Position.ToString() + hs + d + ds + hs + ms + ss; }
            else { return d + ds + hs + ms + ss + hs + Position.ToString(); }

        }
        ////DD Coordinate Format
        private string ToDecimalDegreeString(int rounding, bool lead, bool trail, bool symbols, bool degreeSymbol, bool positionFirst, bool hyphen)
        {
            string degreeS = "";
            string hyph = " ";
            if (degreeSymbol) { degreeS = "º"; }
            if (!symbols) { degreeS = ""; }
            if (hyphen) { hyph = "-"; }

            string leadTrail = "{0:0";
            if (lead)
            {
                if (Position == CoordinatesPosition.E || Position == CoordinatesPosition.W)
                {
                    leadTrail += "00";
                }
                else
                {
                    leadTrail += "0";
                }
            }
            leadTrail += ".";
            if (trail)
            {
                for (int i = 0; i < rounding; i++)
                {
                    leadTrail += "0";
                }
            }
            else
            {
                leadTrail += "#########";
            }
            leadTrail += "}";

            double result = (Degrees) + (Convert.ToDouble(Minutes)) / 60 + (Convert.ToDouble(Seconds)) / 3600;
            result = Math.Round(result, rounding);
            string d = string.Format(leadTrail, Math.Abs(result));
            if (positionFirst) { return Position.ToString() + hyph + d + degreeS; }
            else { return d + degreeS + hyph + Position.ToString(); }

        }

        private string Leading_Trailing_Format(bool isLead, bool isTrail, int rounding, CoordinatesPosition? p = null)
        {
            string leadString = "{0:0";
            if (isLead)
            {
                if (p != null)
                {
                    if (p.Value == CoordinatesPosition.W || p.Value == CoordinatesPosition.E)
                    {
                        leadString += "00";
                    }
                }
                else
                {
                    leadString += "0";
                }
            }

            leadString += ".";
            if (isTrail)
            {
                for (int i = 0; i < rounding; i++)
                {
                    leadString += "0";
                }
            }
            else
            {
                leadString += "#########";
            }

            leadString += "}";
            return leadString;

        }

        private string FormatError(string argument, string rule)
        {
            return "'" + argument + "' is not a valid argument for string format rule: " + rule + ".";
        }

        private enum ToStringType
        {
            Decimal_Degree, Degree_Decimal_Minute, Degree_Minute_Second, Decimal
        }
        /// <summary>
        /// Notify the correct properties and parent properties.
        /// </summary>
        /// <param name="p">Property Type</param>
        private void NotifyProperties(PropertyTypes p)
        {
            switch (p)
            {
                case PropertyTypes.DecimalDegree:
                    NotifyPropertyChanged("DecimalDegree");
                    NotifyPropertyChanged("DecimalMinute");
                    NotifyPropertyChanged("Degrees");
                    NotifyPropertyChanged("Minutes");
                    NotifyPropertyChanged("Seconds");
                    NotifyPropertyChanged("Position");
                    break;
                case PropertyTypes.DecimalMinute:
                    NotifyPropertyChanged("DecimalDegree");
                    NotifyPropertyChanged("DecimalMinute");
                    NotifyPropertyChanged("Minutes");
                    NotifyPropertyChanged("Seconds");
                    break;
                case PropertyTypes.Degree:
                    NotifyPropertyChanged("DecimalDegree");
                    NotifyPropertyChanged("Degree");
                    break;
                case PropertyTypes.Minute:
                    NotifyPropertyChanged("DecimalDegree");
                    NotifyPropertyChanged("DecimalMinute");
                    NotifyPropertyChanged("Minutes");
                    break;
                case PropertyTypes.Position:
                    NotifyPropertyChanged("DecimalDegree");
                    NotifyPropertyChanged("Position");
                    break;
                case PropertyTypes.Second:
                    NotifyPropertyChanged("DecimalDegree");
                    NotifyPropertyChanged("DecimalMinute");
                    NotifyPropertyChanged("Seconds");
                    break;
                default:
                    NotifyPropertyChanged("DecimalDegree");
                    NotifyPropertyChanged("DecimalMinute");
                    NotifyPropertyChanged("Degrees");
                    NotifyPropertyChanged("Minutes");
                    NotifyPropertyChanged("Seconds");
                    NotifyPropertyChanged("Position");
                    break;
            }
            NotifyPropertyChanged("Display");

            if (Parent != null)
            {
                Parent.NotifyPropertyChanged("Display");
                Parent.NotifyPropertyChanged("CelestialInfo");
                Parent.NotifyPropertyChanged("UTM");
                Parent.NotifyPropertyChanged("MGRS");
                Parent.NotifyPropertyChanged("Cartesian");
                Parent.NotifyPropertyChanged("ECEF");
           }

        }

        /// <summary>
        /// Property changed event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Notify property changed
        /// </summary>
        /// <param name="propName">Property name</param>
        public void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        /// <summary>
        /// Used for notifying the correct properties.
        /// </summary>
        private enum PropertyTypes
        {
            DecimalDegree, DecimalMinute, Position, Degree, Minute, Second, FormatChange
        }

        /// <summary>
        /// Returns CoordinatePart in radians
        /// </summary>
        /// <returns></returns>
        public double ToRadians()
        {
            return decimalDegree * Math.PI / 180;
        }
        /// <summary>
        /// Attempts to parse a string into a CoordinatePart.
        /// </summary>
        /// <param name="s">CoordinatePart string</param>
        /// <param name="cp">CoordinatePart</param>
        /// <returns>boolean</returns>
        /// <example>
        /// <code>
        /// CoordinatePart cp;
        /// if(CoordinatePart.TryParse("N 32.891º", out cp))
        /// {
        ///     Console.WriteLine(cp); //N 32º 53' 28.212"
        /// }
        /// </code>
        /// </example>
        public static bool TryParse(string s, out CoordinatePart cp)
        {
            cp = null;
            
            if (FormatFinder_CoordPart.TryParse(s, out cp))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Attempts to parse a string into a CoordinatePart. 
        /// </summary>
        /// <param name="s">CoordinatePart string</param>
        /// <param name="t">CoordinateType</param>
        /// <param name="cp">CoordinatePart</param>
        /// <returns>boolean</returns>
        /// <example>
        /// <code>
        /// CoordinatePart cp;
        /// if(CoordinatePart.TryParse("-32.891º", CoordinateType.Long, out cp))
        /// {
        ///     Console.WriteLine(cp); //W 32º 53' 27.6"
        /// }
        /// </code>
        /// </example>
        public static bool TryParse(string s, CoordinateType t, out CoordinatePart cp)
        {
            cp = null;
            //Comma at beginning parses to long
            //Asterik forces lat
            if(t== CoordinateType.Long) { s = "," + s; }
            else { s = "*" + s; }
            if (FormatFinder_CoordPart.TryParse(s, out cp))
            {
                return true;
            }
            return false;
        }

    }
}
