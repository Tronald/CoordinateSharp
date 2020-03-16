using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                Coordinate nc = new Coordinate(coordinate.Latitude.ToDouble(), coordinate.Longitude.ToDouble(), coordinate.GeoDate, coordinate.EagerLoadSettings);
                nc.equatorial_radius = coordinate.equatorial_radius;
                nc.inverse_flattening = coordinate.inverse_flattening;

                points.Add(nc);
                shape = earthShape;
                initialCoordinate = coordinate;
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
