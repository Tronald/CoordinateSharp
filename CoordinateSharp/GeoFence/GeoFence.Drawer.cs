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
using System.Collections.Generic;
using System.Linq;

namespace CoordinateSharp
{

    public partial class GeoFence
    {
        /// <summary>
        /// GenFence subclass used for continuous shape drawing
        /// </summary>
        [Serializable]
        public class Drawer
        {         
            private Coordinate initialCoordinate;  //This is the intial and always the last drawn point in the shape.
            private Coordinate referenceCoordinate; //This coordinate is referenced for final bearing calculations
         
            private double bearing;
            private double finalBearing;
            private Shape shape;

            private List<Coordinate> points = new List<Coordinate>(); //Stores all drawn points

            /// <summary>
            /// Initializes the GeoFence Drawer with an initial point a bearing/facing direction.
            /// </summary>
            /// <param name="coordinate">Starting Coordinate</param>
            /// <param name="earthShape">Earth Shape for calculations</param>
            /// <param name="initialBearing">Initial bearing or direction facing</param>
            /// <example>
            /// The following example creates a GeoFence Drawer.
            /// <code>
            /// //Create a coordinate with EagerLoading off for efficiency during drawing.
            /// Coordinate c = new Coordinate(31.65, -84.02, new EagerLoad(false));
            /// 
            /// //Create the GeoFence Drawer, specifying an ellipsoidal earth
            /// //shape with a start bearing / direction faced of 0 degrees.
            /// GeoFence.Drawer gd = new GeoFence.Drawer(c, Shape.Ellipsoid, 0);
            /// </code>
            /// </example>
            public Drawer(Coordinate coordinate, Shape earthShape, double initialBearing)
            {
                //Create initial coordinate to move.
                Coordinate nc = new Coordinate(coordinate.Latitude.ToDouble(), coordinate.Longitude.ToDouble(), coordinate.GeoDate, coordinate.EagerLoadSettings);
                nc.equatorial_radius = coordinate.equatorial_radius;
                nc.inverse_flattening = coordinate.inverse_flattening;

                //Create first point to add to points list.
                Coordinate firstPoint= new Coordinate(coordinate.Latitude.ToDouble(), coordinate.Longitude.ToDouble(), coordinate.GeoDate, coordinate.EagerLoadSettings);
                nc.equatorial_radius = coordinate.equatorial_radius;
                nc.inverse_flattening = coordinate.inverse_flattening;

                points.Add(firstPoint);

                shape = earthShape;
                initialCoordinate = nc;
                bearing = (initialBearing) % 360; //normalize
                finalBearing = (initialBearing) % 360; //normailza
            }

            /// <summary>
            /// Draws line from the initial or last drawn point.
            /// </summary>
            /// <param name="d">Distance</param>
            /// <param name="bearingChange">Bearing change in degrees</param>
            /// <example>
            /// The following example draws a 5 km square in the USA.
            /// <code>
            /// //Create a coordinate with EagerLoading off for efficiency during drawing.
            /// Coordinate c = new Coordinate(31.65, -84.02, new EagerLoad(false));
            /// 
            /// //Create the GeoFence Drawer, specifying an ellipsoidal earth
            /// //shape with a start bearing / direction faced of 0 degrees.
            /// GeoFence.Drawer gd = new GeoFence.Drawer(c, Shape.Ellipsoid, 0);
            /// 
            /// //Draw the first line using the initial bearing (0 means no change in heading)
            /// gd.Draw(new Distance(5), 0);
            /// //Draw the next to line by changing 90 degrees at each point.
            /// gd.Draw(new Distance(5), 90);
            /// gd.Draw(new Distance(5), 90);
            /// //Close the shape by drawing from the line end point to the initial coordinate.
            /// gd.Close();
            /// </code>
            /// </example>
            public void Draw(Distance d, double bearingChange)
            {
                //Store reference point for reverse bearing lookup
                referenceCoordinate = new Coordinate(initialCoordinate.Latitude.ToDouble(), initialCoordinate.Longitude.ToDouble(), new EagerLoad(false));

                //Get new bearing
                //This formula will account for bearing changes of distance and offset the next movement
                bearing = (finalBearing - bearing + bearing + bearingChange) % 360;
                initialCoordinate.Move(d, bearing, shape);
                finalBearing = (new Distance(initialCoordinate, referenceCoordinate, shape).Bearing + 180) % 360;

                Coordinate nc = new Coordinate(initialCoordinate.Latitude.ToDouble(), initialCoordinate.Longitude.ToDouble(), initialCoordinate.GeoDate, initialCoordinate.EagerLoadSettings);
                nc.equatorial_radius = initialCoordinate.equatorial_radius;
                nc.inverse_flattening = initialCoordinate.inverse_flattening;

                points.Add(nc);

            }
            /// <summary>
            /// Draws a line to the Drawer start point (closes the shape).
            /// </summary>
            public void Close()
            {
                Coordinate c = points.First();
                Coordinate nc = new Coordinate(c.Latitude.ToDouble(), c.Longitude.ToDouble(), c.GeoDate, c.EagerLoadSettings);
                nc.equatorial_radius = c.equatorial_radius;
                nc.inverse_flattening = c.inverse_flattening;

                points.Add(nc);
            }

            /// <summary>
            /// Get's the last point's ending Coordinate.
            /// </summary>
            public Coordinate Last
            {
                get
                {
                    return points.Last();
                }
            }

            /// <summary>
            /// Gets all drawn points (the shape drawn).
            /// </summary>
            public List<Coordinate> Points
            {
                get { return points; }
            }
        }
    }
}
