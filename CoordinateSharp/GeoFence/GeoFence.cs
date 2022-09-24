/*
CoordinateSharp is a .NET standard library that is intended to ease geographic coordinate 
format conversions and location based celestial calculations.
https://github.com/Tronald/CoordinateSharp

Many celestial formulas in this library are based on Jean Meeus's 
Astronomical Algorithms (2nd Edition). Comments that reference only a chapter
are referring to this work.

License

CoordinateSharp is split licensed and may be licensed under the GNU Affero General Public License version 3 or a commercial use license as stated.

Copyright (C) 2022, Signature Group, LLC
  
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
using System.Collections.Generic;
using System;

namespace CoordinateSharp
{   
    /// <summary>
    /// The GeoFence class is used to help check if points/coordinates are inside or near a specified polygon/polyline, 
    /// </summary>
    [Serializable]
    public partial class GeoFence
    {   
        private readonly List<Point> _points = new List<Point>();

        /// <summary>
        /// Create a GeoFence using a list of points. 
        /// A GeoFence can be either a series of lines or polygons.
        /// </summary>
        /// <param name="points">List of points</param>
        /// <example>
        /// The following example creates a square in the USA using lat/long points.
        /// <code>
        /// List&lt;GeoFence.Point&gt; points = new List&lt;GeoFence.Point&gt;();
        /// 
        /// //Points specified manually to create a square in the USA.
        /// //First and last points should be identical if creating a polygon boundary.
        /// points.Add(new GeoFence.Point(31.65, -106.52));
        /// points.Add(new GeoFence.Point(31.65, -84.02));
        /// points.Add(new GeoFence.Point(42.03, -84.02));
        /// points.Add(new GeoFence.Point(42.03, -106.52));
        /// points.Add(new GeoFence.Point(31.65, -106.52));
        /// </code>
        /// </example>
        public GeoFence(List<Point> points)
        {
            _points = points;
        }

        /// <summary>
        /// Create a GeoFence using a list of coordinates
        /// A GeoFence can be either a series of lines or polygons.
        /// </summary>
        /// <param name="coordinates">List of coordinates</param>
        /// <example>
        /// The following example creates a polyline base on coordinates.
        /// <code>
        /// List&lt;Coordinate&gt; coords = new List&lt;Coordinate&gt;();
        ///  
        /// coords.Add(new Coordinate(25,63));
        /// coords.Add(new Coordinate(26,63));
        /// coords.Add(new Coordinate(27,63));
        ///  
        /// GeoFence gf = new GeoFence(coords); 
        /// </code>
        /// </example>
        public GeoFence(List<Coordinate> coordinates)
        {
            foreach (var c in coordinates)
            {
                _points.Add(new Point { Latitude = c.Latitude.ToDouble(), Longitude = c.Longitude.ToDouble() });
            }
        }
    
        private Coordinate ClosestPointOnSegment(Point a, Point b, Coordinate p, DateTime dt, EagerLoad eg)
        {
            var d = new Point
            {
                Longitude = b.Longitude - a.Longitude,
                Latitude = b.Latitude - a.Latitude,
            };

            double number = (p.Longitude.ToDouble() - a.Longitude) * d.Longitude + (p.Latitude.ToDouble() - a.Latitude) * d.Latitude;

            if (number <= 0.0)
                return new Coordinate(a.Latitude, a.Longitude, dt, eg);

            double denom = d.Longitude * d.Longitude + d.Latitude * d.Latitude;

            if (number >= denom)
                return new Coordinate(b.Latitude, b.Longitude, dt, eg);

            return new Coordinate(a.Latitude + (number / denom) * d.Latitude, a.Longitude + (number / denom) * d.Longitude, dt, eg);
        }

        /// <summary>
        /// Determine if the coordinate is inside the polygon.     
        /// </summary>
        /// <param name="point">Point to test</param>
        /// <remarks>
        /// Points sitting on the edge of a polygon may return true or false.
        /// </remarks>
        /// <returns>bool</returns>
        /// <example>
        /// The following example shows how to determine if a coordinate is inside of a specified polygon.
        /// <code>
        /// List&lt;GeoFence.Point&gt; points = new List&lt;GeoFence.Point&gt;();
        /// 
        /// //Points specified manually to create a square in the USA.
        /// //First and last points should be identical if creating a polygon boundary.
        /// points.Add(new GeoFence.Point(31.65, -106.52));
        /// points.Add(new GeoFence.Point(31.65, -84.02));
        /// points.Add(new GeoFence.Point(42.03, -84.02));
        /// points.Add(new GeoFence.Point(42.03, -106.52));
        /// points.Add(new GeoFence.Point(31.65, -106.52));
        /// 
        /// GeoFence gf = new GeoFence(points);
        /// 
        /// Coordinate c = new Coordinate(36.67, -101.51);
        /// 
        /// //Determine if Coordinate is within polygon
        /// Console.WriteLine(gf.IsPointInPolygon(c)); //True (coordinate is within the polygon)
        /// </code>
        /// </example>
        public bool IsPointInPolygon(Coordinate point)
        {
            if (point == null)
                return false;

            double latitude = point.Latitude.ToDouble();
            double longitude = point.Longitude.ToDouble();
            int sides = _points.Count;
            int j = sides - 1;
            bool pointStatus = false;
            for (int i = 0; i < sides; i++)
            {
                if (_points[i].Latitude < latitude && _points[j].Latitude >= latitude || _points[j].Latitude < latitude && _points[i].Latitude >= latitude)
                {
                    if (_points[i].Longitude + (latitude - _points[i].Latitude) / (_points[j].Latitude - _points[i].Latitude) * (_points[j].Longitude - _points[i].Longitude) < longitude)
                    {
                        pointStatus = !pointStatus;
                    }
                }
                j = i;
            }
            return pointStatus;
        }

        /// <summary>
        /// Determine if a coordinate is next to the given range (in meters) of the polyline.
        /// </summary>
        /// <param name="point">Point to test</param>
        /// <param name="range">Range in meters</param>
        /// <returns>bool</returns>
        /// <example>
        /// The following example shows how to determine if a coordinate is within 1000 meters of
        /// the edge of the specified polygon.
        /// <code>
        /// List&lt;GeoFence.Point&gt; points = new List&lt;GeoFence.Point&gt;();
        /// 
        /// //Points specified manually to create a square in the USA.
        /// //First and last points should be identical if creating a polygon boundary.
        /// points.Add(new GeoFence.Point(31.65, -106.52));
        /// points.Add(new GeoFence.Point(31.65, -84.02));
        /// points.Add(new GeoFence.Point(42.03, -84.02));
        /// points.Add(new GeoFence.Point(42.03, -106.52));
        /// points.Add(new GeoFence.Point(31.65, -106.52));
        /// 
        /// GeoFence gf = new GeoFence(points);
        /// 
        /// Coordinate c = new Coordinate(36.67, -101.51);
        ///  
        /// //Determine if Coordinate is within specific range of shapes line.
        /// Console.WriteLine(gf.IsPointInRangeOfLine(c, 1000)); //False (coordinate is not within 1000 meters of the edge of the polygon)
        /// </code>
        /// </example>
        public bool IsPointInRangeOfLine(Coordinate point, double range)
        {
            if (point == null)
                return false;

            for (int i = 0; i < _points.Count - 1; i++)
            {
                Coordinate c = ClosestPointOnSegment(_points[i], _points[i + 1], point, point.GeoDate, point.EagerLoadSettings);
                if (c.Get_Distance_From_Coordinate(point).Meters <= range)
                    return true;
            }
        
            return false;
        }

        /// <summary>
        /// Determine if the coordinate is next the given range of the polyline.
        /// </summary>
        /// <param name="point">Point to test</param>
        /// <param name="range">Range is a distance object</param>
        /// <returns>bool</returns>
        /// <example>
        /// The following example shows how to determine if a coordinate is within 1 km of
        /// the edge of the specified polygon.
        /// <code>
        /// List&lt;GeoFence.Point&gt; points = new List&lt;GeoFence.Point&gt;();
        /// 
        /// //Points specified manually to create a square in the USA.
        /// //First and last points should be identical if creating a polygon boundary.
        /// points.Add(new GeoFence.Point(31.65, -106.52));
        /// points.Add(new GeoFence.Point(31.65, -84.02));
        /// points.Add(new GeoFence.Point(42.03, -84.02));
        /// points.Add(new GeoFence.Point(42.03, -106.52));
        /// points.Add(new GeoFence.Point(31.65, -106.52));
        /// 
        /// GeoFence gf = new GeoFence(points);
        /// 
        /// Coordinate c = new Coordinate(36.67, -101.51);
        /// 
        /// Distance d = new Distance(1, DistanceType.Kilometers);
        /// Console.WriteLine(gf.IsPointInRangeOfLine(c, d)); //False (coordinate is not within 1 km of the edge of the polygon)
        /// </code>
        /// </example>
        public bool IsPointInRangeOfLine(Coordinate point, Distance range)
        {
            if (point == null || range == null)
                return false;

            return IsPointInRangeOfLine(point, range.Meters);
        }   
        
        /// <summary>
        /// Gets distance from nearest polyline in shape
        /// </summary>
        /// <param name="point">Coordinate</param>
        /// <returns>Distance</returns>
        public Distance DistanceFromNearestPolyLine(Coordinate point)
        {
            if (point == null)
                return null;

            Distance d = null;

            for (int i = 0; i < _points.Count - 1; i++)
            {
                Coordinate c = ClosestPointOnSegment(_points[i], _points[i + 1], point, point.GeoDate, point.EagerLoadSettings);

                if (d == null) { d= new Distance(point, c); }
                else
                {
                    Distance nd = new Distance(point, c);
                    if (nd.Meters < d.Meters) { d = nd; }
                }
              
            }

            return d;
        }

       
    }  
}