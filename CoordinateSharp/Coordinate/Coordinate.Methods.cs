/*
CoordinateSharp is a .NET standard library that is intended to ease geographic coordinate 
format conversions and location based celestial calculations.
https://github.com/Tronald/CoordinateSharp

Many celestial formulas in this library are based on Jean Meeus's 
Astronomical Algorithms (2nd Edition). Comments that reference only a chapter
are referring to this work.

MIT License

(c) 2017, Justin Gielski

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sub-license, and/or sell
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

namespace CoordinateSharp
{  
    public partial class Coordinate : INotifyPropertyChanged
    {
        /*CONSTRUCTORS*/

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
        /// GeoDate will default to 1/1/1900 GMT until provided
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
        /// Creates an empty Coordinates object with specified eager loading options.
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

        /*DATA LOADERS*/

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
        /// Initialize Cartesian information (required if eager loading is turned off).
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

        /*OUTPUT METHODS*/

        /// <summary>
        /// Bind-able formatted coordinate string.
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

        /*DATUM-ELLIPSOID METHODS*/

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

        /*DISTANCE & MOVING METHODS*/

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

        /*PARSER METHODS*/

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

        /*PROPERTY CHANGE HANDLER*/

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
                    if (!EagerLoadSettings.Celestial || celestialInfo == null) { return; } //Prevent Null Exceptions and calls while eagerloading is off
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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

    }
}
