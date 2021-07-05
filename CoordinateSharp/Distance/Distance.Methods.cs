/*
CoordinateSharp is a .NET standard library that is intended to ease geographic coordinate 
format conversions and location based celestial calculations.
https://github.com/Tronald/CoordinateSharp

Many celestial formulas in this library are based on Jean Meeus's 
Astronomical Algorithms (2nd Edition). Comments that reference only a chapter
are referring to this work.

License

CoordinateSharp is split licensed and may be licensed under the GNU Affero General Public License version 3 or a commercial use license as stated.

Copyright (C) 2021, Signature Group, LLC
  
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
	-United States Department of Defense.
	-United States Department of Homeland Security.
	-Open source contributors to this library.
	-Scholarly or scientific research.
	-Emergency response / management uses.

Please visit http://coordinatesharp.com/licensing or contact Signature Group, LLC to purchase a commercial license, or for any questions regarding the AGPL 3.0 license requirements or free use license: sales@signatgroup.com.
*/
using System;

namespace CoordinateSharp
{ 
    public partial class Distance
    {
        /// <summary>
        /// Initializes a Distance object based on the distance between 2 coordinates using the default distance formula.
        /// </summary>
        /// <remarks>
        /// Default distance formula uses Haversine (Spherical Earth) calculations.
        /// </remarks>
        /// <param name="coord1">Coordinate 1</param>
        /// <param name="coord2">Coordinate 2</param>
        /// <example>
        /// The following example grabs the distance in KM and bearing between 2 coordinates
        /// using the default Haversine calculations.
        /// <code>
        /// Coordinate coordinate1 = new Coordinate(25, 65);
		/// Coordinate coordinate2 = new Coordinate(27.6, 63);
        /// 
        /// Distance distance = new Distance(coordinate1, coordinate2);
        /// 
        /// Console.WriteLine(distance.Kilometers); //351.167091506772
		/// Console.WriteLine(distance.Bearing); //214.152133893015
        /// </code>
        /// </example>
        public Distance(Coordinate coord1, Coordinate coord2)
        {
            Haversine(coord1, coord2);
        }
        /// <summary>
        /// Initializes a Distance object based on the distance between 2 coordinates.
        /// </summary>
        /// <remarks>
        /// Distance formula may either be Haversine (Spherical Earth) or Vincenty (Ellipsoidal Earth) calculations.
        /// </remarks>
        /// <param name="coord1">Coordinate 1</param>
        /// <param name="coord2">Coordinate 2</param>
        /// <param name="shape">Shape of earth</param>
        /// /// <example>
        /// The following example grabs the distance in KM and bearing between 2 coordinates
        /// using Vincenty (ellipsoidal earth) calculations.
        /// <code>
        /// Coordinate coordinate1 = new Coordinate(25, 65);
		/// Coordinate coordinate2 = new Coordinate(27.6, 63);
        /// 
        /// Distance distance = new Distance(coordinate1, coordinate2, Shape.Ellipsoid);
        /// 
        /// Console.WriteLine(distance.Kilometers); //350.50857212259
		/// Console.WriteLine(distance.Bearing); //215.183316089463
        /// </code>
        /// </example>
        public Distance(Coordinate coord1, Coordinate coord2, Shape shape)
        {
            if (shape == Shape.Sphere)
            {
                Haversine(coord1, coord2);
            }
            else
            {
                Vincenty(coord1, coord2);
            }
        }
        /// <summary>
        /// Initializes Distance object based on distance in KM
        /// </summary>
        /// <param name="km">Kilometers</param>
        /// <example>
        /// The following example converts kilometers into miles.
        /// <code>
        /// Distance distance = new Distance(10.36);
		/// Console.WriteLine(distance.Miles); //6.43740356
        /// </code>
        /// </example>
        public Distance(double km)
        {
            kilometers = km;
            meters = km * 1000;
            feet = meters * 3.28084;
            miles = meters * 0.000621371;
            nauticalMiles = meters * 0.0005399565;
        }

        /// <summary>
        /// Initializes a Distance object based on a specified distance and measurement type.
        /// </summary>
        /// <param name="distance">Distance</param>
        /// <param name="type">Measurement type</param>
        /// <example>
        /// The following example converts meters into miles.
        /// <code>
        /// Distance distance = new Distance(1000.36, DistanceType.Meters);
		/// Console.WriteLine(distance.Miles); //0.62159469356
        /// </code>
        /// </example>
        public Distance(double distance, DistanceType type)
        {
            bearing = 0;
            switch (type)
            {
                case DistanceType.Feet:
                    feet = distance;
                    meters = feet * 0.3048;
                    kilometers = meters / 1000;
                    miles = meters * 0.000621371;
                    nauticalMiles = meters * 0.0005399565;
                    break;
                case DistanceType.Kilometers:
                    kilometers = distance;
                    meters = kilometers * 1000;
                    feet = meters * 3.28084;
                    miles = meters * 0.000621371;
                    nauticalMiles = meters * 0.0005399565;
                    break;
                case DistanceType.Meters:
                    meters = distance;
                    kilometers = meters / 1000;
                    feet = meters * 3.28084;
                    miles = meters * 0.000621371;
                    nauticalMiles = meters * 0.0005399565;
                    break;
                case DistanceType.Miles:
                    miles = distance;
                    meters = miles * 1609.344;
                    feet = meters * 3.28084;
                    kilometers = meters / 1000;
                    nauticalMiles = meters * 0.0005399565;
                    break;
                case DistanceType.NauticalMiles:
                    nauticalMiles = distance;
                    meters = nauticalMiles * 1852.001;
                    feet = meters * 3.28084;
                    kilometers = meters / 1000;
                    miles = meters * 0.000621371;
                    break;
                default:
                    kilometers = distance;
                    meters = distance * 1000;
                    feet = meters * 3.28084;
                    miles = meters * 0.000621371;
                    nauticalMiles = meters * 0.0005399565;
                    break;
            }
        }

        private void Vincenty(Coordinate coord1, Coordinate coord2)
        {
            double lat1, lat2, lon1, lon2;
            double d, crs12, crs21;

            
            lat1 = coord1.Latitude.ToRadians();
            lat2 = coord2.Latitude.ToRadians();
            lon1 = coord1.Longitude.ToRadians() *-1; //REVERSE FOR CALC 2.1.1.1
            lon2 = coord2.Longitude.ToRadians() *-1; //REVERSE FOR CALC 2.1.1.1

            //Ensure datums match between coords
            if ((coord1.equatorial_radius != coord2.equatorial_radius) || (coord1.inverse_flattening != coord2.inverse_flattening))
            {
                throw new InvalidOperationException("The datum set does not match between Coordinate objects.");
            }
            double[] ellipse = new double[] { coord1.equatorial_radius, coord1.inverse_flattening };


            // elliptic code
            double[] cde = Distance_Assistant.Dist_Ell(lat1, -lon1, lat2, -lon2, ellipse);  // ellipse uses East negative
            crs12 = cde[1] * (180 / Math.PI); //Bearing
            crs21 = cde[2] * (180 / Math.PI); //Reverse Bearing
            d = cde[0]; //Distance

            bearing = crs12;
            //reverseBearing = crs21;
            meters = d;
            kilometers = d / 1000;
            feet = d * 3.28084;
            miles = d * 0.000621371;
            nauticalMiles = d * 0.0005399565;

        }

        private void Haversine(Coordinate coord1, Coordinate coord2)
        {        
            ////RADIANS
            double lat1 = coord1.Latitude.ToRadians();         
            double long1 = coord1.Longitude.ToRadians();
            double lat2 = coord2.Latitude.ToRadians();
            double long2 = coord2.Longitude.ToRadians();

            //Distance Calcs
            double R = 6371000; //6378137.0;
               
            double latRad = coord2.Latitude.ToRadians() - coord1.Latitude.ToRadians(); 
            double longRad = coord2.Longitude.ToRadians() - coord1.Longitude.ToRadians(); 

            double a = Math.Sin(latRad / 2.0) * Math.Sin(latRad / 2.0) +
                Math.Cos(lat1) * Math.Cos(lat2) * Math.Sin(longRad / 2.0) * Math.Sin(longRad / 2.0);
            double cl = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double dist = R * cl;

            //Get bearing         
            double dLong = long2 - long1;
            double y = Math.Sin(dLong) * Math.Cos(lat2);
            double x = Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dLong);
            double brng = Math.Atan2(y, x) * (180 / Math.PI); //Convert bearing back to degrees.
           
            //if (brng < 0) { brng -= 180; brng = Math.Abs(brng); }
            brng = (brng + 360) % 360; //v2.1.1.1 NORMALIZE HEADING
            
            kilometers = dist / 1000;
            meters = dist;
            feet = dist * 3.28084;
            miles = dist * 0.000621371;
            nauticalMiles = dist * 0.0005399565;
            bearing = brng;
        }     
    }
   
}
