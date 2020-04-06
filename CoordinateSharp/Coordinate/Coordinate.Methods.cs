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
using System.ComponentModel;

namespace CoordinateSharp
{     
    public partial class Coordinate : INotifyPropertyChanged
    {
        /*CONSTRUCTORS*/ 

        /// <summary>
        /// Creates a Coordinate object with default values.
        /// </summary>
        /// <remarks>       
        /// Coordinate will initialize with a latitude and longitude of 0 degrees and 
        /// a GeoDate of 1900-1-1. All properties will be set to EagerLoaded.
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to create a default Coordinate.
        /// <code>
        /// Coordinate c = new Coordinate();
        /// </code>
        /// </example>
        public Coordinate()
        {
            Coordinate_Builder(0, 0, new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc), GlobalSettings.Default_EagerLoad);
        }      
        /// <summary>
        /// Creates a populated Coordinate based on signed degrees formated latitude and longitude.
        /// </summary>
        /// <param name="lat">signed latitude</param>
        /// <param name="longi">signed longitude</param>
        /// <remarks>
        /// GeoDate will default to 1900-01-01.
        /// All properties will be set to EagerLoaded.
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to create a defined Coordinate.
        /// <code>
        /// Coordinate c = new Coordinate(25, 25);
        /// </code>
        /// </example>
        public Coordinate(double lat, double longi)
        {
            Coordinate_Builder(lat, longi, new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc), GlobalSettings.Default_EagerLoad);
        }
        /// <summary>
        /// Creates a populated Coordinate object with an assigned GeoDate.
        /// </summary>
        /// <param name="lat">signed latitude</param>
        /// <param name="longi">signed longitude</param>
        /// <param name="date">DateTime (UTC)</param>
        /// <remarks>
        /// All properties will be set to EagerLoaded.
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to create a defined Coordinate object with a defined GeoDate.
        /// <code>
        /// Coordinate c = new Coordinate(25, 25, new DateTime(2018, 2, 5, 10, 38, 22));
        /// </code>
        /// </example>
        public Coordinate(double lat, double longi, DateTime date)
        {
            Coordinate_Builder(lat, longi, date, GlobalSettings.Default_EagerLoad);
        }
        /// <summary>
        /// Creates an empty Coordinates object with specified eager loading options.
        /// </summary>
        /// <remarks>
        /// Coordinate will initialize with a latitude and longitude of 0 degrees and
        /// a GeoDate of 1900-1-1.
        /// </remarks>
        /// <param name="eagerLoad">Eager loading options</param>
        /// <example>
        /// The following example demonstrates how to create a default Coordinate object with defined
        /// eager loading options
        /// <code>
        /// //Create a new EagerLoading object set to only
        /// //eager load celestial calculations.
        /// EagerLoading el = new EagerLoading(EagerLoadType.Celestial);
        /// 
        /// Coordinate c = new Coordinate(el);
        /// </code>
        /// </example>
        public Coordinate(EagerLoad eagerLoad)
        {
            Coordinate_Builder(0, 0, new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc), eagerLoad);
        }
        /// <summary>
        /// Creates a populated Coordinate object with specified eager loading options.
        /// </summary>
        /// <remarks>
        /// Geodate will default to 1900-01-01.
        /// </remarks>
        /// <param name="lat">signed latitude</param>
        /// <param name="longi">signed longitude</param>
        /// <param name="eagerLoad">Eager loading options</param>
        /// <example>
        /// The following example demonstrates how to create a defined Coordinate object with defined 
        /// eager loading options.
        /// <code>
        /// //Create a new EagerLoading object set to only
        /// //eager load celestial calculations.
        /// EagerLoading el = new EagerLoading(EagerLoadType.Celestial);
        /// 
        /// Coordinate c = new Coordinate(25, 25, el);
        /// </code>
        /// </example>
        public Coordinate(double lat, double longi, EagerLoad eagerLoad)
        {
            Coordinate_Builder(lat, longi, new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc), eagerLoad);
        }
        /// <summary>
        /// Creates a populated Coordinate object with specified eager load options and an assigned GeoDate.
        /// </summary>
        /// <param name="lat">signed latitude</param>
        /// <param name="longi">signed longitude</param>
        /// <param name="date">DateTime you wish to use for celestial calculation</param>
        /// <param name="eagerLoad">Eager loading options</param>
        /// <example>
        /// The following example demonstrates how to create a defined Coordinate object with defined 
        /// eager loading options and a GeoDate.
        /// <code>
        /// //Create a new EagerLoading object set to only
        /// //eager load celestial calculations.
        /// EagerLoading el = new EagerLoading(EagerLoadType.Celestial);
        /// DateTime geoDate = new DateTime(2018, 2, 5, 10, 38, 22);
        /// 
        /// Coordinate c = new Coordinate(25, 25, geoDate, el);
        /// </code>
        /// </example>
        public Coordinate(double lat, double longi, DateTime date, EagerLoad eagerLoad)
        {
            Coordinate_Builder(lat, longi, date, eagerLoad);
        }
       
        /// <summary>
        /// Creates a populated Coordinate object with specified eager load options, assigned GeoDate and Earth Shape. Should only be called when Coordinate 
        /// is create from another system.
        /// </summary>
        /// <param name="lat">signed latitude</param>
        /// <param name="longi">signed longitude</param>      
        /// <param name="eagerLoad">Eager loading options</param>
        /// <param name="equatorialRadius">Semi Major Axis or Equatorial Radius of the Earth</param>
        /// <param name="inverseFlattening">Inverse of Flattening of the Earth</param>              
        internal Coordinate(double lat, double longi,  EagerLoad eagerLoad, double equatorialRadius, double inverseFlattening)
        {
            Coordinate_Builder(lat, longi, new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc), eagerLoad);
            equatorial_radius = equatorialRadius;
            inverse_flattening = inverseFlattening;

        }

        /// <summary>
        /// Coordinate build logic goes here.
        /// </summary>
        /// <param name="lat">Signed latitude</param>
        /// <param name="longi">Signed longitude</param>
        /// <param name="date">Date at location</param>
        /// <param name="eagerLoad">Eagerloading settings</param>
        private void Coordinate_Builder(double lat, double longi, DateTime date, EagerLoad eagerLoad)
        {
            //SET EagerLoading Setting
            EagerLoadSettings = eagerLoad;
          
            //Use default constructor if signed degree is 0 for performance.
            if (lat == 0) { latitude = new CoordinatePart(CoordinateType.Lat); }
            else { latitude = new CoordinatePart(lat, CoordinateType.Lat); }

            if (longi == 0) { longitude = new CoordinatePart(CoordinateType.Long); }
            else { longitude = new CoordinatePart(longi, CoordinateType.Long); }
           
            //Set CoordinatePart parents
            latitude.parent = this;
            longitude.parent = this;

            //Set UTC date at location
            geoDate = date;


            //LOAD NEW COORDINATE SYSTEMS HERE

            //Load Celestial
            if (eagerLoad.Celestial)
            {
                celestialInfo = new Celestial(lat, longi, date,0, eagerLoad);
            }
            //Load UTM MGRS
            if (eagerLoad.UTM_MGRS)
            {
                utm = new UniversalTransverseMercator(lat, longi, this);
                mgrs = new MilitaryGridReferenceSystem(utm);
            }
            //Load CARTESIAN
            if (eagerLoad.Cartesian)
            {
                cartesian = new Cartesian(this);
            }
            //Load ECEF
            if (eagerLoad.ECEF)
            {
                ecef = new ECEF(this);
            }

          

            //Set Ellipsoid
            equatorial_radius = 6378137.0;
            inverse_flattening = 298.257223563;
        }     

        /*DATA LOADERS*/
        /*LOAD NEW COORDINATE SYSTEMS HERE*/

        /// <summary>
        /// Load celestial information (required if eager loading is turned off).
        /// </summary>
        /// <example>
        /// The following example shows how to Load Celestial information when eager loading is turned off.
        /// <code>
        /// EagerLoad eagerLoad = new EagerLoad();
        /// eagerLoad.Celestial = false;
        /// Coordinate c = new Coordinate(40.0352, -74.5844, DateTime.Now, eagerLoad);
        ///
        /// //To load Celestial information when ready
        /// c.LoadCelestialInfo;           
        /// </code>
        /// </example>
        public void LoadCelestialInfo()
        {
            celestialInfo = Celestial.LoadCelestial(this);
        }
        /// <summary>
        /// Load UTM and MGRS information (required if eager loading is turned off).
        /// </summary>
        /// <example>
        /// The following example shows how to Load UTM and MGRS information when eager loading is turned off.
        /// <code>
        /// EagerLoad eagerLoad = new EagerLoad();
        /// eagerLoad.UTM_MGRS = false;
        /// Coordinate c = new Coordinate(40.0352, -74.5844, DateTime.Now, eagerLoad);
        ///
        /// //To load UTM_MGRS information when ready
        /// c.LoadUTM_MGRSInfo;           
        /// </code>
        /// </example>
        public void LoadUTM_MGRS_Info()
        {
            utm = new UniversalTransverseMercator(latitude.ToDouble(), longitude.ToDouble(), this);
            mgrs = new MilitaryGridReferenceSystem(utm);
        }
        /// <summary>
        /// Load Cartesian information (required if eager loading is turned off).
        /// </summary>
        /// <example>
        /// The following example shows how to Load Cartesian information when eager loading is turned off.
        /// <code>
        /// EagerLoad eagerLoad = new EagerLoad();
        /// eagerLoad.Cartesian = false;
        /// Coordinate c = new Coordinate(40.0352, -74.5844, DateTime.Now, eagerLoad);
        ///
        /// //To load Cartesian information when ready
        /// c.LoadCartesianInfo;           
        /// </code>
        /// </example>
        public void LoadCartesianInfo()
        {
            cartesian = new Cartesian(this);
        }
        /// <summary>
        /// Load ECEF information (required if eager loading is turned off).
        /// </summary>
        /// <example>
        /// The following example shows how to Load ECEF information when eager loading is turned off.
        /// <code>
        /// EagerLoad eagerLoad = new EagerLoad();
        /// eagerLoad.ECEF = false;
        /// Coordinate c = new Coordinate(40.0352, -74.5844, DateTime.Now, eagerLoad);
        ///
        /// //To load ECEF information when ready
        /// c.LoadECEFInfo;           
        /// </code>
        /// </example>
        public void LoadECEFInfo()
        {
            ecef = new ECEF(this);
        }

        /*OUTPUT METHODS*/

        /// <summary>
        /// Bindable formatted coordinate string.
        /// </summary>
        /// <remarks>Bind to this property when MVVM patterns are being used</remarks>
        /// <example>
        /// The following example shows how to bind to a formatted Coordinate in XAML
        /// <code language="XAML">
        /// <TextBlock Text="{Binding Latitude.Display, UpdateSourceTrigger=PropertyChanged}"/>
        /// </code>
        /// </example>
        public string Display
        {
            get
            {
                return Latitude.Display + " " + Longitude.Display;
            }
        }
        /// <summary>
        /// A string formatted and represented coordinate.
        /// </summary>
        /// <returns>Formatted Coordinate string</returns>
        /// <example>
        /// <code>
        /// Coordinate c = new Coordinate(25, 25);
		/// Console.WriteLine(c.ToString()); //N 25º 0' 0" E 25º 0' 0"
        /// </code>
        /// </example>
        public override string ToString()
        {
            string latString = latitude.ToString();
            string longSting = longitude.ToString();
            return latString + " " + longSting;
        }
        /// <summary>
        /// A string formatted and represented coordinate.
        /// </summary>
        /// <param name="options">CoordinateFormatOptions</param>
        /// <returns>Formatted Coordinate string</returns>
        /// <example>
        /// The following example will demonstrate how to output a custom formatted 
        /// string representation of a Coordinate.
        /// <code>
        /// Coordinate c = new Coordinate(25, 25);
        ///
        /// //Create the formatting object to pass to the ToString() overload.
        /// CoordinateFormatOptions cfo = new CoordinateFormatOptions();
        ///
        /// cfo.Format = CoordinateFormatType.Degree_Decimal_Minutes; //Set string format to DDM
        /// cfo.Display_Leading_Zeros = true; //Display leading zeros
        /// cfo.Round = 2; //Round to the 2nd decimal place
        ///
        /// Console.WriteLine(c.ToString(cfo)); //N 25º 00' E 025º 00'
        /// </code>
        /// </example>
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
        /// <param name="flattening">Inverse Flattening</param>
        /// <example>   
        /// The following example demonstrates how to set the earths ellipsoid values for UTM/MGRS and ECEF conversion as well as Distance calculations
        /// that use ellipsoidal earth values.
        /// <code>
        /// //Initialize a coordinate with the default WGS84 Ellipsoid.
        /// Coordinate c = new Coordinate(25,25);
        /// 
        /// //Change Ellipsoid to GRS80 Datum
        /// c.Set_Datum(6378160.000, 298.25);      
        /// </code>
        /// </example>
        public void Set_Datum(double radius, double flattening)
        {
            //WGS84
            //RADIUS 6378137.0;
            //FLATTENING 298.257223563;
            if(utm != null)
            {
                utm.inverse_flattening = flattening;
                utm.ToUTM(Latitude.ToDouble(), Longitude.ToDouble(), utm);
                mgrs = new MilitaryGridReferenceSystem(utm);
                NotifyPropertyChanged("UTM");
                NotifyPropertyChanged("MGRS");              
            }
            if(ecef != null)
            {
                ecef.equatorial_radius = radius;
                ecef.inverse_flattening = flattening;
                ecef.ToECEF(this);
                NotifyPropertyChanged("ECEF");              
            }
            equatorial_radius = radius;
            inverse_flattening = flattening;
        }
        /// <summary>
        /// Set a custom datum for coordinate conversions and distance calculation for specified coordinate formats only.
        /// Objects must be loaded prior to setting if EagerLoading is turned off.
        /// </summary>
        /// <param name="radius">Equatorial Radius</param>
        /// <param name="flattening">Inverse Flattening</param>
        /// <param name="datum">Coordinate_Datum</param>
        /// <example>
        /// The following example demonstrates how to set the earths ellipsoid values for UTM/MGRS conversions only.
        /// <code>
        /// //Initialize a coordinate with the default WGS84 Ellipsoid that eagerloads UTM/MGRS only.
        /// EagerLoadType et = EagerLoadType.UTM_MGRS;
        /// EagerLoad eagerLoad = new EagerLoad(et);
        /// Coordinate c = new Coordinate(25, 25, et);
        /// 
        /// //Change Ellipsoid to GRS80 Datum for UTM_MGRS calculations only.
        /// c.Set_Datum(6378160.000, 298.25, Coordinate_Datum.UTM_MGRS);      
        /// </code>
        /// </example>
        public void Set_Datum(double radius, double flattening, Coordinate_Datum datum)
        {
            //WGS84
            //RADIUS 6378137.0;
            //FLATTENING 298.257223563;
         
            if (datum.HasFlag(Coordinate_Datum.UTM_MGRS))
            {
                if(utm==null || mgrs == null) { throw new NullReferenceException("UTM/MGRS objects must be loaded prior to changing the datum."); }
                utm.inverse_flattening = flattening;
                utm.ToUTM(Latitude.ToDouble(), Longitude.ToDouble(), utm);
                mgrs = new MilitaryGridReferenceSystem(utm);
                NotifyPropertyChanged("UTM");
                NotifyPropertyChanged("MGRS");
            
            }
            if (datum.HasFlag(Coordinate_Datum.ECEF))
            {
                if (ECEF==null) { throw new NullReferenceException("ECEF objects must be loaded prior to changing the datum."); }
                ecef.equatorial_radius = radius;
                ecef.inverse_flattening = flattening;
                ecef.ToECEF(this);
                NotifyPropertyChanged("ECEF");
            
            }
            if (datum.HasFlag(Coordinate_Datum.LAT_LONG))
            {
                equatorial_radius = radius;
                inverse_flattening = flattening;
            }
        }

        /*DISTANCE & MOVING METHODS*/

        /// <summary>
        /// Returns the distance from a target coordinate using spherical earth calculations.
        /// Use overload if ellipsoidal calculations are desired.
        /// </summary>
        /// <param name="target">Coordinate</param>
        /// <returns>Distance</returns>
        /// <example>
        /// The following example demonstrates how to obtain the distance from a target coordinate
        /// using default spherical earth calculations.
        /// <code>
        /// Coordinate coord = new Coordinate(25, 25);
        /// Coordinate target = new Coordinate(28, 30);
        /// 
        /// //Get distance from target using default spherical calculations
        /// Distance d = coord.Get_Distance_From_Coordinate(target);
        /// 
        /// Console.Writeline(d.Kilometers); //598.928622714691
        /// </code>
        /// </example>
        public Distance Get_Distance_From_Coordinate(Coordinate target)
        {
            return new Distance(this, target);
        }
        /// <summary>
        /// Returns the distance from a target coordinate.
        /// </summary>
        /// <param name="target">Target coordinate</param>
        /// <param name="shape">Earth shape</param>
        /// <returns>Distance</returns>
        /// <example>     
        /// The following example demonstrates how to obtain the distance from a target coordinate
        /// using ellipsoidal earth calculations.
        /// <code>
        /// Coordinate coord = new Coordinate(25,25);
        /// Coordinate target = new Coordinate(28, 30);
        /// 
        /// //Get distance from target using ellipsoidal calculations
        /// Distance d = coord.Get_Distance_From_Coordinate(target, Shape.Ellipsoid);
        /// 
        /// Console.Writeline(d.Kilometers); //599.002436777727
        /// </code>
        /// </example>
        public Distance Get_Distance_From_Coordinate(Coordinate target, Shape shape)
        {
            return new Distance(this, target, shape);
        }

        /// <summary>
        /// Move coordinate based on provided bearing and distance (in meters).
        /// </summary>
        /// <param name="distance">Distance in meters</param>
        /// <param name="bearing">Bearing</param>
        /// <param name="shape">Shape of earth</param>
        /// <example>
        /// The following example moves a coordinate 10km in the direction of 
        /// the specified bearing using ellipsoidal earth calculations.
        /// <code>
        /// //N 25º 0' 0" E 25º 0' 0"
        /// Coordinate c = Coordinate(25,25);
        /// 
        /// double meters = 10000;
        /// double bearing = 25;
        /// 
        /// //Move coordinate the specified meters
        /// //and direction using ellipsoidal calculations
        /// c.Move(meters, bearing, Shape.Ellipsoid);
        /// 
        /// //New Coordinate - N 25º 4' 54.517" E 24º 57' 29.189"
        /// </code>
        /// </example>
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
                Longitude.DecimalDegree = -lon2;//v2.1.1.1
            }
            else
            {
                double[] cde = Distance_Assistant.Direct_Ell(lat1, -lon1, crs12, distance, ellipse);  // ellipse uses East negative
                //Convert back from radians 
                double lat2 = cde[0] * (180 / Math.PI);
                double lon2 = cde[1] * (180 / Math.PI); //v2.1.1.1          
                //ADJUST CORD
                Latitude.DecimalDegree = lat2;
                Longitude.DecimalDegree = lon2;
            }        
        }
        /// <summary>
        /// Move a coordinate a specified distance (in meters) towards a target coordinate.
        /// </summary>
        /// <param name="target">Target coordinate</param>
        /// <param name="distance">Distance toward target in meters</param>
        /// <param name="shape">Shape of earth</param>
        /// <example>
        /// The following example moves a coordinate 10km towards a target coordinate using
        /// ellipsoidal earth calculations.
        /// <code>
        /// //N 25º 0' 0" E 25º 0' 0"
        /// Coordinate coord = Coordinate(25,25);
        /// 
        /// //Target Coordinate
        /// Coordinate target = new Coordinate(26.5, 23.2);
        /// 
        /// double meters = 10000;
        /// 
        /// //Move coordinate the specified meters
        /// //towards target using ellipsoidal calculations
        /// coord.Move(target, meters, Shape.Ellipsoid);
        /// 
        /// //New Coordinate - N 24º 56' 21.526" E 25º 4' 23.944"
        /// </code>
        /// </example>
        public void Move(Coordinate target, double distance, Shape shape)
        {
            Distance d = new Distance(this, target, shape);
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
                Longitude.DecimalDegree = -lon2; //v2.1.1.1 update
            }
            else
            {
                double[] cde = Distance_Assistant.Direct_Ell(lat1, -lon1, crs12, distance, ellipse);  // ellipse uses East negative
                //Convert back from radians 
                double lat2 = cde[0] * (180 / Math.PI);
                double lon2 = cde[1] * (180 / Math.PI); // v2.1.1.1           
                //ADJUST CORD
                Latitude.DecimalDegree = lat2;
                Longitude.DecimalDegree = lon2;
            }
        }
        /// <summary>
        /// Move coordinate based on provided bearing and distance (in meters).
        /// </summary>
        /// <param name="distance">Distance</param>
        /// <param name="bearing">Bearing</param>
        /// <param name="shape">Shape of earth</param>
        /// <example>
        /// The following example moves a coordinate 10km in the direction of 
        /// the specified bearing using ellipsoidal earth calculations.
        /// <code>
        /// //N 25º 0' 0" E 25º 0' 0"
        /// Coordinate c = Coordinate(25,25);
        /// 
        /// Distance distance = new Distance(10, DistanceType.Kilometers);
        /// double bearing = 25;
        /// 
        /// //Move coordinate the specified distance
        /// //and direction using ellipsoidal calculations
        /// c.Move(distance, bearing, Shape.Ellipsoid);
        /// 
        /// //New Coordinate - N 25º 4' 54.517" E 24º 57' 29.189"
        /// </code>
        /// </example>
        public void Move(Distance distance, double bearing, Shape shape)
        {
            //Convert to Radians for formula
            double lat1 = latitude.ToRadians();
            double lon1 = longitude.ToRadians();
            double crs12 = bearing * Math.PI / 180; //Convert bearing to radians

            double[] ellipse = new double[] { equatorial_radius, inverse_flattening };

            if (shape == Shape.Sphere)
            {
                double[] cd = Distance_Assistant.Direct(lat1, lon1, crs12, distance.Meters);
                double lat2 = cd[0] * (180 / Math.PI);
                double lon2 = cd[1] * (180 / Math.PI);

                //ADJUST CORD
                Latitude.DecimalDegree = lat2;
                Longitude.DecimalDegree = -lon2; //v2.1.1.1
            }
            else
            {
                double[] cde = Distance_Assistant.Direct_Ell(lat1, -lon1, crs12, distance.Meters, ellipse);  // ellipse uses East negative
                //Convert back from radians 
                double lat2 = cde[0] * (180 / Math.PI);
                double lon2 = cde[1] * (180 / Math.PI); //v2.1.1.1         
                //ADJUST CORD
                Latitude.DecimalDegree = lat2;
                Longitude.DecimalDegree = lon2;
            }
        }
        /// <summary>
        /// Move a coordinate a specified distance towards a target coordinate.
        /// </summary>
        /// <param name="target">Target coordinate</param>
        /// <param name="distance">Distance toward target</param>
        /// <param name="shape">Shape of earth</param>
        /// <example>
        /// The following example moves a coordinate 10km towards a target coordinate using
        /// ellipsoidal earth calculations.
        /// <code>
        /// //N 25º 0' 0" E 25º 0' 0"
        /// Coordinate coord = Coordinate(25,25);
        /// 
        /// //Target Coordinate
        /// Coordinate target = new Coordinate(26.5, 23.2);
        /// 
        /// Distance distance = new Distance(10, DistanceType.Kilometers);
        /// 
        /// //Move coordinate the specified distance
        /// //towards target using ellipsoidal calculations
        /// coord.Move(target, distance, Shape.Ellipsoid);
        /// 
        /// //New Coordinate - N 24º 56' 21.526" E 25º 4' 23.944"
        /// </code>
        /// </example>
        public void Move(Coordinate target, Distance distance, Shape shape)
        {
            Distance d = new Distance(this, target, shape);
            //Convert to Radians for formula
            double lat1 = latitude.ToRadians();
            double lon1 = longitude.ToRadians();
            double crs12 = d.Bearing * Math.PI / 180; //Convert bearing to radians

            double[] ellipse = new double[] { equatorial_radius, inverse_flattening };

            if (shape == Shape.Sphere)
            {
                double[] cd = Distance_Assistant.Direct(lat1, lon1, crs12, distance.Meters);
                double lat2 = cd[0] * (180 / Math.PI);
                double lon2 = cd[1] * (180 / Math.PI);

                //ADJUST CORD
                Latitude.DecimalDegree = lat2;
                Longitude.DecimalDegree = -lon2; //v2.1.1.1 update
            }
            else
            {
                double[] cde = Distance_Assistant.Direct_Ell(lat1, -lon1, crs12, distance.Meters, ellipse);  
                //Convert back from radians 
                double lat2 = cde[0] * (180 / Math.PI);
                double lon2 = cde[1] * (180 / Math.PI); //v2.1.1.1         
                //ADJUST CORD
                Latitude.DecimalDegree = lat2;
                Longitude.DecimalDegree = lon2;
            }
        }
     

        /*UTM MGRS LAT ZONE LOCK*/

        /// <summary>
        /// Locks converted UTM and MGRS longitudinal grid zones to the specified zone number. This will allow over-projection and allow users to remain in a single desired zone.
        /// This should be used with caution as precision loss will occur. 
        /// </summary>
        /// <example>
        /// The following example locks a Coordinate that would normal project into grid zone 31, into grid zone 30 for over-projection.
        /// <code> 
        /// Coordinate coord = new Coordinate(51.5074,1);   
        /// 
        /// //Normal projection at this Coordinate below
        /// //UTM:  31U 361203mE 5708148mN
        /// //MGRS: 31U CT 61203 08148
        /// 
        /// //Lock UTM and MGRS zones to 30 for over-projection.
        /// coord.Lock_UTM_MGRS_Zone(30);
        /// 
        /// //Over-projected coordinates
        /// Console.WriteLine(coord.UTM);  //30U 777555mE 5713840mN
        /// Console.WriteLine(coord.MGRS); //30U YC 77555 13840
        /// </code>
        /// </example>
        /// <param name="zone">UTM longitudinal grid zone</param>
        public void Lock_UTM_MGRS_Zone(int zone)
        {
            //Ensure zone limits are not exceeded
            if(zone<1 || zone>60) {throw new ArgumentOutOfRangeException("UTM and MGRS latitudinal zone is out of range."); }
            utm_mgrs_LongitudeZone_Override = zone;
            NotifyPropertyChanged("UTM");
            NotifyPropertyChanged("MGRS");
        }

        /// <summary>
        /// Unlocks converted UTM and MRGS longitudinal grid zones from the specified zone number. 
        /// This will return UTM and MGRS conversions to normal projection.
        /// </summary>
        public void Unlock_UTM_MGRS_Zone()
        {
            utm_mgrs_LongitudeZone_Override = null;
            NotifyPropertyChanged("UTM");
            NotifyPropertyChanged("MGRS");
        }

        //LOCAL TIME METHOD
        /// <summary>
        /// Returns a new Celestial object in local time, based on the Coordinate objects values.
        /// </summary>
        /// <param name="hourOffset">Offset hours</param>
        /// <example>
        /// In the following example we convert a UTC populated Coordinate.Celestial object and convert it to Local time.
        /// <code>
        /// Coordinate coord = new Coordinate(32,72, DateTime.Now);
        /// //Convert to Pacific Standard Time (PST) -7 UTC and return new
        /// //Celestial object that contains values in PST.
        /// Celestial cel = coord.Celestial_LocalTime(-7);
        /// </code>
        /// </example>
        /// <returns>Celestial</returns>
        public Celestial Celestial_LocalTime(double hourOffset)
        {
            Celestial cel = new Celestial(latitude.ToDouble(), longitude.ToDouble(), geoDate, hourOffset, EagerLoadSettings);
            return cel;
        }

        /*PROPERTY CHANGE HANDLER*/
        /*NOTIFY NEW COORDINATE SYSTEMS HERE*/
        /// <summary>
        /// Property changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Notify property changed
        /// </summary>
        /// <param name="propName">Property Name</param>
        public void NotifyPropertyChanged(string propName)
        {
            switch (propName)
            {
                case "CelestialInfo":
                    if (!EagerLoadSettings.Celestial) { return; } //Prevent calls while eagerloading is off
                    if(EagerLoadSettings.Celestial && celestialInfo == null) { celestialInfo = new Celestial(false); } //Create object if EagerLoading is on and object is null (EagerLoading turned on later).
                    celestialInfo.CalculateCelestialTime(latitude.DecimalDegree, longitude.DecimalDegree, geoDate, EagerLoadSettings, offset);
                    break;
                case "UTM":
                    if (!EagerLoadSettings.UTM_MGRS) { return; }
                    else if (EagerLoadSettings.UTM_MGRS && utm == null) { utm = new UniversalTransverseMercator(latitude.ToDouble(), longitude.ToDouble(), this, equatorial_radius, inverse_flattening); }
                    else { utm.ToUTM(latitude.ToDouble(), longitude.ToDouble(), utm, utm_mgrs_LongitudeZone_Override); }
                    break;
                case "utm":
                    //Adjust case and notify of change. 
                    //Use to notify without calling ToUTM()
                    propName = "UTM";
                    break;
                case "MGRS":
                    if (!EagerLoadSettings.UTM_MGRS || !EagerLoadSettings.Extensions.MGRS || utm == null) { return; }
                    else if (EagerLoadSettings.UTM_MGRS && EagerLoadSettings.Extensions.MGRS && mgrs == null) { mgrs = new MilitaryGridReferenceSystem(utm); }
                    else { MGRS.ToMGRS(utm); }
                    break;
                case "Cartesian":
                    if (!EagerLoadSettings.Cartesian) { return; }
                    else if (EagerLoadSettings.Cartesian && cartesian == null) { cartesian = new Cartesian(this); }
                    else { Cartesian.ToCartesian(this); }
                    break;
                case "ECEF":
                    if (!EagerLoadSettings.ECEF) { return; }
                    else if(EagerLoadSettings.ECEF && ecef == null) { ecef = new ECEF(this); }
                    else ECEF.ToECEF(this);
                    break;
                default:
                    break;
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

    }
}
