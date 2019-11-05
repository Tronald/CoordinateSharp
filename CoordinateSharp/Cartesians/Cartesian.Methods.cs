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

namespace CoordinateSharp
{
    public partial class Cartesian
    {
        /// <summary>
        /// Create a spherical Cartesian coordinate.        
        /// </summary>
        /// <remarks>
        /// Cartesian values will be populated by converting from the passed geodetic Coordinate object.
        /// </remarks>
        /// <param name="coordinate">Geodetic coordinate</param>
        /// <example>
        /// The following example demonstrates how to create a populated spherical Cartesian coordinate
        /// based on a converted geodetic coordinate.
        /// <code>
        /// //Create a geodetic coordinate at N25, E45
        /// Coordinate c = new Coordinate(25,45);
        /// 
        /// //Create and convert geodetic to spherical Cartesian
	    /// Cartesian cart = new Cartesian(c);
        /// 
        /// Console.WriteLine(cart); //0.64085638 0.64085638 0.42261826
        /// </code>
        /// </example>
        public Cartesian(Coordinate coordinate)
        {
            //formulas:
            x = Math.Cos(coordinate.Latitude.ToRadians()) * Math.Cos(coordinate.Longitude.ToRadians());
            y = Math.Cos(coordinate.Latitude.ToRadians()) * Math.Sin(coordinate.Longitude.ToRadians());
            z = Math.Sin(coordinate.Latitude.ToRadians());
        }
        /// <summary>
        /// Create a spherical Cartesian coordinate.
        /// </summary>
        /// <param name="xc">X</param>
        /// <param name="yc">Y</param>
        /// <param name="zc">Z</param>
        /// <example>
        /// <code>
        /// Cartesian cart = new Cartesian(0.64085638, 0.64085638, 0.42261826);
        /// </code>
        /// </example>
        public Cartesian(double xc, double yc, double zc)
        {
            //formulas:
            x = xc;
            y = yc;
            z = zc;
        }

        /// <summary>
        /// Updates Cartesian values when eagerloading is used.
        /// </summary>       
        /// <param name="coordinate">Geodetic coordinate</param>
        internal void ToCartesian(Coordinate coordinate)
        {
            x = Math.Cos(coordinate.Latitude.ToRadians()) * Math.Cos(coordinate.Longitude.ToRadians());
            y = Math.Cos(coordinate.Latitude.ToRadians()) * Math.Sin(coordinate.Longitude.ToRadians());
            z = Math.Sin(coordinate.Latitude.ToRadians());
        }

        /// <summary>
        /// Returns a geodetic Coordinate object based on the provided Cartesian coordinate X, Y, Z values.
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="z">Z</param>
        /// <returns>Coordinate</returns>
        /// <example>
        /// The following example creates (converts to) a geodetic Coordinate object based on spherical Cartesian X, Y, Z values.
        /// <code>
        /// Coordinate c = Cartesian.CartesianToLatLong(0.64085638, 0.64085638, 0.42261826);	    
        /// Console.WriteLine(c); //N 24º 59' 60" E 45º 0' 0"
        /// </code>
        /// </example>
        public static Coordinate CartesianToLatLong(double x, double y, double z)
        {
            double lon = Math.Atan2(y, x);
            double hyp = Math.Sqrt(x * x + y * y);
            double lat = Math.Atan2(z, hyp);

            double Lat = lat * (180 / Math.PI);
            double Lon = lon * (180 / Math.PI);
            return new Coordinate(Lat, Lon);
        }
        /// <summary>
        /// Returns a geodetic Coordinate object based on the provided Cartesian coordinate.
        /// </summary>
        /// <param name="cart">Cartesian Coordinate</param>
        /// <returns>Coordinate</returns>
        /// <example>
        /// The following example creates (converts to) a geodetic Coordinate object based on a spherical Cartesian object.
        /// <code>
        /// Cartesian cart = new Cartesian(0.64085638, 0.64085638, 0.42261826);
		/// Coordinate c = Cartesian.CartesianToLatLong(cart);
        /// Console.WriteLine(c); //N 24º 59' 60" E 45º 0' 0"
        /// </code>
        /// </example>
        public static Coordinate CartesianToLatLong(Cartesian cart)
        {
            double x = cart.X;
            double y = cart.Y;
            double z = cart.Z;

            double lon = Math.Atan2(y, x);
            double hyp = Math.Sqrt(x * x + y * y);
            double lat = Math.Atan2(z, hyp);

            double Lat = lat * (180 / Math.PI);
            double Lon = lon * (180 / Math.PI);
            return new Coordinate(Lat, Lon);
        }

        /// <summary>
        /// Default formatted Cartesian string.
        /// </summary>
        /// <remarks>
        /// X, Y, Z values are rounded to the 8th place.
        /// </remarks>
        /// <returns>Cartesian Formatted Coordinate String</returns>
        public override string ToString()
        {
            return Math.Round(x,8).ToString() + " " + Math.Round(y, 8).ToString() + " " + Math.Round(z, 8).ToString();
        }
    }
}
