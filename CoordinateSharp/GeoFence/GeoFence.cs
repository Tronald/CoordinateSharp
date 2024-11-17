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

// Ignore Spelling: Densify

using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CoordinateSharp
{
    /// <summary>
    /// The GeoFence class is used to help check if points/coordinates are inside or near a specified polygon/polyline, 
    /// </summary>
    [Serializable]
    public partial class GeoFence
    {
        private List<Point> _points = new List<Point>();
      
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
        /// <summary>
        /// Provides a List of Points contained within the drawn GeoFence.
        /// </summary>
        public List<Point> Points
        {
            get { return _points; }
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
        /// This method utilizes 2D ray casting techniques and does not inherently account for the curvature of the Earth. To mitigate the impact of Earth shape distortion on polygons or polylines that span long distances, users should employ densification.
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
        /// <remarks>
        /// This method utilizes 2D ray casting techniques and does not inherently account for the curvature of the Earth. To mitigate the impact of Earth shape distortion on polygons or polylines that span long distances, users should employ densification.
        /// </remarks>
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
        /// <remarks>
        /// This method utilizes 2D ray casting techniques and does not inherently account for the curvature of the Earth. To mitigate the impact of Earth shape distortion on polygons or polylines that span long distances, users should employ densification.
        /// </remarks>
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
        /// <remarks>This method utilizes 2D ray casting techniques and does not inherently account for the curvature of the Earth. To mitigate the impact of Earth shape distortion on polygons or polylines that span long distances, users should employ densification.</remarks>
        /// <returns>Distance</returns>
        public Distance DistanceFromNearestPolyLine(Coordinate point)
        {
            if (point == null)
                return null;

            Distance d = null;

            for (int i = 0; i < _points.Count - 1; i++)
            {
                Coordinate c = ClosestPointOnSegment(_points[i], _points[i + 1], point, point.GeoDate, point.EagerLoadSettings);

                if (d == null) { d = new Distance(point, c); }
                else
                {
                    Distance nd = new Distance(point, c);
                    if (nd.Meters < d.Meters) { d = nd; }
                }

            }

            return d;
        }

        /// <summary>
        /// Densifies the polygon by adding additional points along each edge at specified intervals using ellipsoidal (Vincenty) logic.
        /// </summary>
        /// <param name="distance">The distance between points along the polygon's edges. This distance determines how frequently new points are inserted into the polygon</param>
        /// <remarks>
        /// This method is particularly useful for large polygons where the curvature of the Earth can cause 
        /// significant distortion in geographic calculations. By adding more points at regular intervals, 
        /// the polygon better conforms to the curved surface of the Earth, reducing errors in area calculations, 
        /// perimeter calculations, and point-in-polygon tests.
        ///
        /// The function automatically calculates intermediate points based on the great-circle distance between 
        /// existing vertices, ensuring that the new points adhere to the true geographic shape of the polygon.
        /// This is essential for maintaining geographic integrity when performing spatial operations or visualizations.
        ///
        /// Note: The densification process increases the number of vertices in the polygon, which may impact performance 
        /// and memory usage in spatial computations and data storage. Optimal use of this function depends on the required 
        /// precision and the geographic extent of the application.
        /// </remarks>
        /// <example>
        /// Here is how you might use this function to densify a polygon representing a large geographic area:
        /// <code>
        /// //Create a four point GeoFence around Utah
        /// List&lt;GeoFence.Point&gt; points = new List&lt;GeoFence.Point&gt;();
        /// points.Add(new GeoFence.Point(41.003444, -109.045223));
        /// points.Add(new GeoFence.Point(41.003444, -102.041524));
        /// points.Add(new GeoFence.Point(36.993076, -102.041524));
        /// points.Add(new GeoFence.Point(36.993076, -109.045223));
        /// points.Add(new GeoFence.Point(41.003444, -109.045223));
        ///
        /// GeoFence gf = new GeoFence(points);
        ///
        /// gf.Densify(new Distance(10, DistanceType.Kilometers));
        /// 
        /// //The gf.Points list now contains additional points at intervals of approximately 10 kilometers.
        /// </code>
        /// </example>
        public void Densify(Distance distance)
        {
            Densify(distance, Shape.Ellipsoid, Earth_Ellipsoid.Get_Ellipsoid(Earth_Ellipsoid_Spec.WGS84_1984));
        }

        /// <summary>
        /// Densifies the polygon by adding additional points along each edge at specified intervals.
        /// </summary>
        /// <param name="distance">The distance between points along the polygon's edges. This distance determines how frequently new points are inserted into the polygon</param>
        /// <param name="shape">Specify earth shape. Sphere is more efficient, but less precise than ellipsoid.</param>
        /// <remarks>
        /// This method is particularly useful for large polygons where the curvature of the Earth can cause 
        /// significant distortion in geographic calculations. By adding more points at regular intervals, 
        /// the polygon better conforms to the curved surface of the Earth, reducing errors in area calculations, 
        /// perimeter calculations, and point-in-polygon tests.
        ///
        /// The function automatically calculates intermediate points based on the great-circle distance between 
        /// existing vertices, ensuring that the new points adhere to the true geographic shape of the polygon.
        /// This is essential for maintaining geographic integrity when performing spatial operations or visualizations.
        ///
        /// Note: The densification process increases the number of vertices in the polygon, which may impact performance 
        /// and memory usage in spatial computations and data storage. Optimal use of this function depends on the required 
        /// precision and the geographic extent of the application.
        /// </remarks>
        /// <example>
        /// Here is how you might use this function to densify a polygon representing a large geographic area:
        /// <code>     
        /// //Create a four point GeoFence around Utah
        /// List&lt;GeoFence.Point&gt; points = new List&lt;GeoFence.Point&gt;();
        /// points.Add(new GeoFence.Point(41.003444, -109.045223));
        /// points.Add(new GeoFence.Point(41.003444, -102.041524));
        /// points.Add(new GeoFence.Point(36.993076, -102.041524));
        /// points.Add(new GeoFence.Point(36.993076, -109.045223));
        /// points.Add(new GeoFence.Point(41.003444, -109.045223));
        ///
        /// GeoFence gf = new GeoFence(points);
        ///
        /// gf.Densify(new Distance(10, DistanceType.Kilometers), Shape.Sphere);
        /// 
        /// //The gf.Points list now contains additional points at intervals of approximately 10 kilometers.
        /// </code>
        /// </example>
        public void Densify(Distance distance, Shape shape)
        {
            Densify(distance, shape, Earth_Ellipsoid.Get_Ellipsoid(Earth_Ellipsoid_Spec.WGS84_1984));
        }
        /// <summary>
        /// Densifies the polygon by adding additional points along each edge at specified intervals using ellipsoidal (Vincenty) logic with a user specified earth shape.
        /// </summary>
        /// <param name="distance">The distance between points along the polygon's edges. This distance determines how frequently new points are inserted into the polygon</param>    
        /// <param name="ellipsoid">Specify earth ellipsoid shape.</param>
        /// <remarks>
        /// This method is particularly useful for large polygons where the curvature of the Earth can cause 
        /// significant distortion in geographic calculations. By adding more points at regular intervals, 
        /// the polygon better conforms to the curved surface of the Earth, reducing errors in area calculations, 
        /// perimeter calculations, and point-in-polygon tests.
        ///
        /// The function automatically calculates intermediate points based on the great-circle distance between 
        /// existing vertices, ensuring that the new points adhere to the true geographic shape of the polygon.
        /// This is essential for maintaining geographic integrity when performing spatial operations or visualizations.
        ///
        /// Note: The densification process increases the number of vertices in the polygon, which may impact performance 
        /// and memory usage in spatial computations and data storage. Optimal use of this function depends on the required 
        /// precision and the geographic extent of the application.
        /// </remarks>
        /// <example>
        /// Here is how you might use this function to densify a polygon representing a large geographic area:
        /// <code>     
        /// //Create a four point GeoFence around Utah
        /// List&lt;GeoFence.Point&gt; points = new List&lt;GeoFence.Point&gt;();
        /// points.Add(new GeoFence.Point(41.003444, -109.045223));
        /// points.Add(new GeoFence.Point(41.003444, -102.041524));
        /// points.Add(new GeoFence.Point(36.993076, -102.041524));
        /// points.Add(new GeoFence.Point(36.993076, -109.045223));
        /// points.Add(new GeoFence.Point(41.003444, -109.045223));
        ///
        /// GeoFence gf = new GeoFence(points);
        ///
        /// gf.Densify(new Distance(10, DistanceType.Kilometers), Earth_Ellipsoid.Get_Ellipsoid(Earth_Ellipsoid_Spec.GRS67_1967));
        /// 
        /// //The gf.Points list now contains additional points at intervals of approximately 10 kilometers.
        /// </code>
        /// </example>
        public void Densify(Distance distance, Earth_Ellipsoid ellipsoid)
        {
            Densify(distance, Shape.Ellipsoid, ellipsoid);
        }

        private void Densify(Distance distance, Shape shape, Earth_Ellipsoid ellipsoid)
        {
            if (_points.Count < 2)
            {
                throw new InvalidOperationException("You cannot perform densification a Geofence that has less than 2 points.");
            }
            //Store the original points for reference as _points will be modified
            List<Point> ogpoints = new List<Point>(_points);

            //Create a collection of point collections to insert into the polygon
            List<List<Point>> inserts = new List<List<Point>>();

            //Iterate the polygon to create densification points
            for (int x = 0; x < ogpoints.Count - 1; x++)
            {
                var p1 = ogpoints[x];
                var p2 = ogpoints[x + 1];

                List<Point> ipoints = new List<Point>();

                //Coordinate to move
                Coordinate mc = new Coordinate(p1.Latitude, p1.Longitude, new EagerLoad(false), ellipsoid.Equatorial_Radius, ellipsoid.Inverse_Flattening);

                //Destination
                Coordinate dc = new Coordinate(p2.Latitude, p2.Longitude, new EagerLoad(false), ellipsoid.Equatorial_Radius, ellipsoid.Inverse_Flattening);

                while (new Distance(mc, dc).Meters > distance.Meters)
                {
                    mc.Move(dc, distance, shape);
                    ipoints.Add(new Point(mc.Latitude.ToDouble(), mc.Longitude.ToDouble()));
                }

                inserts.Add(ipoints);
            }

            //Clear existing collection
            _points.Clear();

            //Create new points collection.
            for (int x = 0; x < ogpoints.Count - 1; x++)
            {
                var p = ogpoints[x];
                _points.Add(p);
                foreach (var dp in inserts[x])
                {
                    _points.Add(dp);
                }
            }

            //Add last point to close shape.
            _points.Add(ogpoints.Last());
        }

        /// <summary>
        /// Orders the points of the GeoFence to be right-handed (counterclockwise).
        /// This method is typically used for the outer boundary of a polygon,
        /// following the GeoJSON specification, where the outer boundary is represented with right-handed (counterclockwise) orientation.
        /// </summary>
        /// <remarks>
        /// A polygon is considered right-handed if its points are ordered in a counterclockwise direction.
        /// This method rearranges the points in the GeoFence to ensure a counterclockwise orientation.
        /// </remarks>
        /// <returns>Returns the GeoFence instance with points ordered in a right-handed (counterclockwise) direction.</returns>
        /// <example>
        /// Example usage:
        /// <code>
        /// // Create a GeoFence with points in arbitrary order
        /// List&lt;Coordinate&gt; points = new List&lt;Coordinate&gt;()
        /// {
        ///     new Coordinate(47.6062, -122.3321),  // Seattle
        ///     new Coordinate(48.7519, -122.4787),  // Bellingham
        ///     new Coordinate(47.2529, -122.4443),  // Tacoma
        ///     new Coordinate(48.0419, -122.9025),  // Port Townsend
        /// };
        /// GeoFence outerFence = new GeoFence(points);
        ///
        /// // Order the points to be right-handed (counterclockwise)
        /// outerFence.OrderPoints_RightHanded();
        ///
        /// // Verify the points are ordered correctly and close the polygon
        /// outerFence.ClosePolygon();
        ///
        /// // The GeoFence is now ready to be used as an outer boundary in a GeoJSON polygon
        /// </code>
        /// </example>

        public void OrderPoints_RightHanded()
        {
            List<Point> points = new List<Point>();

            // Calculate the centroid of the points
            var centroid = GetCentroid();

            // Sort points based on their angle relative to the centroid
            _points = _points
                .OrderBy(p => Math.Atan2(p.Latitude - centroid.Latitude, p.Longitude - centroid.Longitude))
                .ToList();

        }

        /// <summary>
        /// Orders the points of the GeoFence to be left-handed (clockwise).
        /// This method is typically used for inner boundaries (holes) in a polygon,
        /// following the GeoJSON specification, where holes are represented with left-handed (clockwise) orientation.
        /// </summary>
        /// <remarks>
        /// A polygon is considered left-handed if its points are ordered in a clockwise direction.
        /// This method rearranges the points in the GeoFence to ensure a clockwise orientation.
        /// </remarks>
        /// <returns>Returns the GeoFence instance with points ordered in a left-handed (clockwise) direction.</returns>
        /// <example>
        /// Example usage:
        /// <code>
        /// // Create a GeoFence with points in arbitrary order
        /// List&lt;Coordinate&gt; points = new List&lt;Coordinate&gt;()
        /// {
        ///       new Coordinate(46.8523, -121.7603),  // Mount Rainier (center point)
        ///       new Coordinate(46.8625, -121.7401),  // Slightly north-east
        ///       new Coordinate(46.8421, -121.7805),  // Slightly south-west
        ///       new Coordinate(46.8650, -121.7850),  // North-west
        ///       new Coordinate(46.8400, -121.7500)   // South-east
        /// };
        /// GeoFence innerFence = new GeoFence(points);
        ///
        /// // Order the points to be left-handed (clockwise)
        /// innerFence.OrderPoints_LeftHanded();
        ///
        /// // Verify the points are ordered correctly and close the polygon
        /// innerFence.ClosePolygon();
        ///
        /// // The GeoFence is now ready to be used as an inner boundary (hole) in a GeoJSON polygon
        /// </code>
        /// </example>
        public void OrderPoints_LeftHanded()
        {
            // Calculate the centroid of the points
            var centroid = GetCentroid();

            // Sort points based on their angle relative to the centroid in reverse order
            _points = _points
                .OrderByDescending(p => Math.Atan2(p.Latitude - centroid.Latitude, p.Longitude - centroid.Longitude))
                .ToList();
        }

        /// <summary>
        /// Returns the central point of the polygon.
        /// </summary>
        /// <returns>Point</returns>
        public Point GetCentroid()
        {
            double centroidLat = 0;
            double centroidLong = 0;

            foreach (var point in _points)
            {
                centroidLat += point.Latitude;
                centroidLong += point.Longitude;
            }

            int totalPoints = Points.Count;

            return new Point(centroidLat / totalPoints, centroidLong / totalPoints);
        }

        /// <summary>
        /// Closes and Completes the polygon shape ensure the first and last points are identical.
        /// </summary>
        public void ClosePolygon()
        {
            if(_points.Count > 0)
            {
                _points.Add(new Point(_points.First().Latitude, _points.First().Longitude));
            }
        }

        /// <summary>
        /// Builds a GeoJSON representation of a polygon with a single outer boundary (fence).
        /// This method is a simplified version that creates GeoJSON without any inner fences (holes).
        /// </summary>
        /// <param name="geoFence">The outer boundary of the polygon, which should be right-handed (counterclockwise).</param>
        /// <returns>Returns a string representing the GeoJSON format of the polygon with the specified outer boundary.</returns>
        /// <example>
        /// The example method ensures that the outer fence is right-handed (counterclockwise), 
        /// following GeoJSON conventions for outer boundaries.
        /// <code>
        /// // Create the outer fence (right-handed, closed polygon)
        /// List&lt;Coordinate&gt; outerPoints = new List&lt;Coordinate&gt;()
        /// {
        ///     new Coordinate(47.6062, -122.3321),  // Seattle
        ///     new Coordinate(48.7519, -122.4787),  // Bellingham
        ///     new Coordinate(47.2529, -122.4443),  // Tacoma
        ///     new Coordinate(48.0419, -122.9025),  // Port Townsend
        ///     new Coordinate(47.6062, -122.3321)   // Closing the loop
        /// };
        /// GeoFence outerFence = new GeoFence(outerPoints);
        /// 
        /// //Points should be right hand ordered and polygon closed if not previously done so.
        /// outerFence.OrderPoints_RightHanded();
        /// outerFence.ClosePolygon();
        ///
        /// // Build the GeoJSON string for the outer fence only
        /// string geoJson = GeoFence.GeoJsonPolygonBuilder(outerFence);
        /// Console.WriteLine(geoJson);
        /// </code>
        /// </example>

        public static string GeoJsonPolygonBuilder(GeoFence geoFence)
        {
            return GeoFence.GeoJsonPolygonBuilder(geoFence, new List<GeoFence>());
        }

        /// <summary>
        /// Builds a GeoJSON representation of a polygon with an outer boundary (fence) and optional inner boundaries (fences).
        /// The method ensures that the outer fence is right-handed (counterclockwise) and inner fences are left-handed (clockwise), 
        /// following GeoJSON conventions for polygons and holes.
        /// </summary>
        /// <param name="outerFence">The outer boundary of the polygon, which should be right-handed (counterclockwise).</param>
        /// <param name="innerFences">A list of inner boundaries (holes), each of which should be left-handed (clockwise).</param>
        /// <returns>Returns a string representing the GeoJSON format of the polygon with the specified boundaries.</returns>
        /// <example>
        /// Example usage:
        /// <code>
        /// // Create the outer fence (right-handed, closed polygon)
        /// List&lt;Coordinate&gt; outerPoints = new List&lt;Coordinate&gt;()
        /// {
        ///     new Coordinate(47.6062, -122.3321),  // Seattle
        ///     new Coordinate(48.7519, -122.4787),  // Bellingham
        ///     new Coordinate(47.2529, -122.4443),  // Tacoma
        ///     new Coordinate(48.0419, -122.9025),  // Port Townsend
        /// };
        /// GeoFence outerFence = new GeoFence(outerPoints);
        /// outerFence.OrderPoints_RightHanded();
        /// outerFence.ClosePolygon();
        ///
        /// // Create inner fences (left-handed, closed polygons)
        /// List&lt;Coordinate&gt; innerPoints1 = new List&lt;Coordinate&gt;()
        /// {
        ///     new Coordinate(46.8523, -121.7603),  // Mount Rainier
        ///     new Coordinate(46.8625, -121.7401),
        ///     new Coordinate(46.8421, -121.7805),
        /// };
        /// GeoFence innerFence1 = new GeoFence(innerPoints1);
        /// innerFence1.OrderPoints_LeftHanded();
        /// innerFence1.ClosePolygon();
        ///
        /// List&lt;Coordinate&gt; innerPoints2 = new List&lt;Coordinate&gt;()
        /// {
        ///     new Coordinate(47.2331, -119.8529),  // Quincy
        ///     new Coordinate(47.2400, -119.8700),
        ///     new Coordinate(47.2280, -119.8350),
        /// };
        /// GeoFence innerFence2 = new GeoFence(innerPoints2);
        /// innerFence2.OrderPoints_LeftHanded();
        /// innerFence2.ClosePolygon();
        ///
        /// // Build the GeoJSON string
        /// string geoJson = GeoFence.GeoJsonPolygonBuilder(outerFence, new List&lt;GeoFence&gt; { innerFence1, innerFence2 });
        /// Console.WriteLine(geoJson);
        /// </code>
        /// </example>
        public static string GeoJsonPolygonBuilder(GeoFence outerFence, List<GeoFence> innerFences)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine("\"type\": \"FeatureCollection\",");
            sb.AppendLine("\"features\": [");
            sb.AppendLine("{");
            sb.AppendLine("\"type\": \"Feature\",");
            sb.AppendLine("\"properties\": {},");
            sb.AppendLine("\"geometry\": {");
            sb.AppendLine("\"type\": \"Polygon\",");
            sb.AppendLine("\"coordinates\": [");
         
            List<string> polygons = new List<string>();
            polygons.Add(GeoJsonPointBuilder(outerFence.Points));

            foreach (var innerFence in innerFences)
            {
                polygons.Add(GeoJsonPointBuilder(innerFence.Points));
            }

            sb.AppendLine(string.Join(",\n", polygons));

            sb.AppendLine("]");
            sb.AppendLine("}");
            sb.AppendLine("}");
            sb.AppendLine("]");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private static string GeoJsonPointBuilder(List<Point> points)
        {
            List<string> results = new List<string>();
         
            foreach (var p in points)
            {
                results.Add($"[{p.Longitude}, {p.Latitude}]");             
            }

            return $"[\n{string.Join(",\n", results)}\n]";
        }
    }
}