using System.Collections.Generic;
using System.Linq;

namespace CoordinateSharp
{
    /// <summary>
    /// Geo Fence class. It helps to check if points/coordinates are inside a polygon, 
    /// Next to a polyline, and counting...
    /// </summary>
    public class GeoFence
    {
        #region Fields
        private List<Point> _points = new List<Point>();
        #endregion

        /// <summary>
        /// Prepare GeoFence with a list of points
        /// </summary>
        /// <param name="points">List of points</param>
        public GeoFence(List<Point> points)
        {
            _points = points;
        }

        /// <summary>
        /// Prepare Geofence with a list of coordinates
        /// </summary>
        /// <param name="coordinates">List of coordinates</param>
        public GeoFence(List<Coordinate> coordinates)
        {
            foreach (var c in coordinates)
            {
                _points.Add(new Point { Latitude = c.Latitude.ToDouble(), Longitude = c.Longitude.ToDouble() });
            }
        }

        #region Utils
        private Coordinate ClosestPointOnSegment(Point a, Point b, Coordinate p)
        {
            var d = new Point
            {
                Longitude = b.Longitude - a.Longitude,
                Latitude = b.Latitude - a.Latitude,
            };

            double number = (p.Longitude.ToDouble() - a.Longitude) * d.Longitude + (p.Latitude.ToDouble() - a.Latitude) * d.Latitude;

            if (number <= 0.0)
                return new Coordinate(a.Latitude, a.Longitude);

            double denom = d.Longitude * d.Longitude + d.Latitude * d.Latitude;

            if (number >= denom)
                return new Coordinate(b.Latitude, b.Longitude);

            return new Coordinate(a.Latitude + (number / denom) * d.Latitude, a.Longitude + (number / denom) * d.Longitude);
        }
        #endregion

        /// <summary>
        /// The function will return true if the point x,y is inside the polygon, or
        /// false if it is not.  If the point is exactly on the edge of the polygon,
        /// then the function may return true or false.
        /// </summary>
        /// <param name="point">The point to test</param>
        /// <returns>bool</returns>
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
        /// The function will return true if the point x,y is next the given range of 
        /// the polyline, or false if it is not.
        /// </summary>
        /// <param name="point">The point to test</param>
        /// <param name="range">The range in meters</param>
        /// <returns>bool</returns>
        public bool IsPointInRangeOfLine(Coordinate point, double range)
        {
            if (point == null)
                return false;

            for (int i = 0; i < _points.Count - 1; i++)
            {
                Coordinate c = ClosestPointOnSegment(_points[i], _points[i + 1], point);
                if (c.Get_Distance_From_Coordinate(point).Meters <= range)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// The function will return true if the point x,y is next the given range of 
        /// the polyline, or false if it is not.
        /// </summary>
        /// <param name="point">The point to test</param>
        /// <param name="range">The range is a distance object</param>
        /// <returns>bool</returns>
        public bool IsPointInRangeOfLine(Coordinate point, Distance range)
        {
            if (point == null || range == null)
                return false;

            return IsPointInRangeOfLine(point, range.Meters);
        }

        /// <summary>
        /// This class is a help class to simplify GeoFence calculus
        /// </summary>
        public class Point
        {

            /// <summary>
            /// Initialize empty point
            /// </summary>
            public Point()
            {
               
            }
            /// <summary>
            /// Initialize point with defined Latitude and Longitude
            /// </summary>
            /// <param name="lat">Latitude (signed)</param>
            /// <param name="lng">Longitude (signed)</param>
            public Point(double lat, double lng)
            {
                Latitude = lat;
                Longitude = lng;
            }
            /// <summary>
            /// The longitude in degrees
            /// </summary>
            public double Longitude;
            /// <summary>
            /// The latitude in degrees
            /// </summary>
            public double Latitude;
        }
    }
}