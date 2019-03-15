/*
CoordinateSharp is a .NET standard library that is intended to ease geographic coordinate 
format conversions and location based celestial calculations.
https://github.com/Tronald/CoordinateSharp

Many celestial formulas in this library are based on Jean Meeus's 
Astronomical Algorithms (2nd Edition). Comments that reference only a chapter
are referring to this work.

License

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

For more information, please contact Signature Group, LLC at this address: sales@signatgroup.com
*/
using System.Collections.Generic;
using System;

namespace CoordinateSharp
{
    /// <summary>
    /// Geo Fence class. It helps to check if points/coordinates are inside a polygon, 
    /// Next to a polyline, and counting...
    /// </summary>
    [Serializable]
    public partial class GeoFence
    {   
        private List<Point> _points = new List<Point>();
       
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
    }  
}