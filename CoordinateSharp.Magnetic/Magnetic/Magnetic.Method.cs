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
using System;
using CoordinateSharp.Formatters;

namespace CoordinateSharp.Magnetic
{
    /// <summary>
    /// Coordinate based magnetic data.
    /// </summary>
    public partial class Magnetic 
    {
        /// <summary>
        /// Initializes a magnetic object based on a Coordinate and a provided data Model. Assumes height is at MSL.
        /// </summary>
        /// <param name="coordinate">Coordinate</param>
        /// <param name="dataModel">DataModel</param>
        /// <example>
        /// Creating a Magnetic object from a coordinate.
        /// <code>
        /// Coordinate c = Coordinate(25,25, new DateTime(2020,1,1));
        /// Magnetic m = new Magnetic(c, DataModel.WMM2020);
        /// </code>
        /// </example>
        public Magnetic(Coordinate coordinate, DataModel dataModel)
        {
            DateTime d = coordinate.GeoDate.AddHours(-coordinate.Offset);
            Model = dataModel;
            Load(coordinate.Latitude.ToDouble(), coordinate.Longitude.ToDouble(), 0, coordinate.Equatorial_Radius, coordinate.Inverse_Flattening, d);
        }
        /// <summary>
        /// Initializes a magnetic object based on a Coordinate, height and a provided data model.
        /// </summary>
        /// <param name="coordinate">Coordinate</param>
        /// <param name="height">Height in Meters</param>
        /// <param name="dataModel">Data Model</param>
        /// <example>
        /// Creating a Magnetic object from a coordinate with a specified height (in meters).
        /// <code>
        /// Coordinate c = Coordinate(25,25, new DateTime(2020,1,1));
        /// Magnetic m = new Magnetic(c, 1000, DataModel.WMM2020);
        /// </code>
        /// </example>
        public Magnetic(Coordinate coordinate, double height, DataModel dataModel)
        {
            DateTime d = coordinate.GeoDate.AddHours(-coordinate.Offset);
            Model = dataModel;
            Load(coordinate.Latitude.ToDouble(), coordinate.Longitude.ToDouble(), height, coordinate.Equatorial_Radius, coordinate.Inverse_Flattening, d);
        }
        /// <summary>
        /// Initializes a magnetic object based on a Coordinate, height and a provided data model.
        /// </summary>
        /// <param name="coordinate">Coordinate</param>
        /// <param name="height">Height</param>
        /// <param name="dataModel">Data Model</param>
        /// <example>
        /// Creating a Magnetic object from a coordinate with a specified height (in meters).
        /// <code>
        /// Coordinate c = Coordinate(25,25, new DateTime(2020,1,1));
        /// Distance d = new Distance(10, DistanceType.Miles); //Height is 10 miles above MSL
        /// Magnetic m = new Magnetic(c, d, DataModel.WMM2020);
        /// </code>
        /// </example>
        public Magnetic(Coordinate coordinate, Distance height, DataModel dataModel)
        {
            DateTime d = coordinate.GeoDate.AddHours(-coordinate.Offset);
            Model = dataModel;
            Load(coordinate.Latitude.ToDouble(), coordinate.Longitude.ToDouble(), height.Meters, coordinate.Equatorial_Radius, coordinate.Inverse_Flattening, d);
        }

        /// <summary>
        /// Initializes a magnetic object based on a signed lat/long, date, time offset, height in meters and a provided data model.
        /// Assume WGS84 datum earth shape for calculations.
        /// </summary>
        /// <param name="lat">Latitude</param>
        /// <param name="lng">Longitude</param>
        /// <param name="date">DateTime</param>
        /// <param name="offset">UTC Offset in Hours</param>
        /// <param name="height">Height in Meters</param>
        /// <param name="dataModel">Data Model</param>
        public Magnetic(double lat, double lng, DateTime date, double offset, double height, DataModel dataModel)
        {
            DateTime d = date.AddHours(-offset);
            Model = dataModel;
            Load(lat, lng, height, 6378137.0, 298.257223563, d);
        }
      


        /// <summary>
        /// Load Magnetic Values.
        /// </summary>
        /// <param name="lat">Signed Latitude (Geodetic)</param>
        /// <param name="lng">Signed Longitude (Geodetic)</param>
        /// <param name="height">Height in Meters</param>
        /// <param name="semiMajorAxis">Semi-Major Axis</param>
        /// <param name="inverseFlattening">Inverse Flattening</param>
        /// <param name="date">Date Time</param>
        private void Load(double lat, double lng, double height, double semiMajorAxis, double inverseFlattening, DateTime date)
        {
            if (lat > 90 || lat<-90) { throw new ArgumentOutOfRangeException("Latitude exceeds maximum of 90 degrees."); }
            if (lng > 180 || lng < -180) { throw new ArgumentOutOfRangeException("Longitude exceeds maximum of 180 degrees."); }
            latSigned = lat;
            lngSigned = lng;
            this.semiMajorAxis = semiMajorAxis;
            this.inverseFlattening = inverseFlattening;
            flattening = 1 / inverseFlattening;//Flattening
            eccentricitySquared = flattening * (2 - flattening);//Eccentricity  Squared
            semiMinorAxis = semiMajorAxis * (1 - flattening);//Semi-minor axis

            nLatGD = lat.NormalizeDegrees360();
            nLngGD = lng.NormalizeDegrees360();
         
            radiusOfCurvature = semiMajorAxis / Math.Sqrt(1 - (eccentricitySquared * Math.Pow(Math.Sin(nLatGD.ToRadians()), 2)));
            northPolarAxis = (radiusOfCurvature * (1 - eccentricitySquared) + height) * Math.Sin(nLatGD.ToRadians());
            double latRad = nLatGD.ToRadians();
            pointOfInterest = (radiusOfCurvature + height) * Math.Cos(latRad);
            radius = Math.Sqrt(Math.Pow(pointOfInterest, 2) + Math.Pow(northPolarAxis, 2));

            nLatGC = Math.Asin(northPolarAxis / radius);
            nLngGC = nLngGD.ToRadians();

            TimeSpan ts = date - new DateTime(date.Year, 1, 1);
            int ly = 365;
            if (DateTime.IsLeapYear(date.Year)) { ly = 366; }
            decimalDate = date.Year + (ts.TotalDays / ly);
  

            points = new DataPoints(Model, this);
            points.Load_Values(); //Load Calculated Values

            magneticFieldElements = new MagneticFieldElements(this);
            secularVariations = new SecularVariations(this);
            uncertainty = new Uncertainty(this);
        }
    }
}
