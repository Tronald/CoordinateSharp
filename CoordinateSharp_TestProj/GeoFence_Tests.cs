using System;
using System.Collections.Generic;
using CoordinateSharp;

namespace CoordinateSharp_TestProj
{
    public class GeoFence_Tests
    {
        public static void Run_Test()
        {
            // CHANGE LATS e LONS
            Coordinate c1 = new Coordinate(31.66, -106.52);
            Coordinate c2 = new Coordinate(31.64, -106.53);
            Coordinate c3 = new Coordinate(42.02, -106.52);
            Coordinate c4 = new Coordinate(42.03, -106.53);

            List<GeoFence.Point> points = new List<GeoFence.Point>();

            points.Add(new GeoFence.Point(31.65, -106.52));
            points.Add(new GeoFence.Point(31.65, -84.02));
            points.Add(new GeoFence.Point(42.03, -84.02));
            points.Add(new GeoFence.Point(42.03, -106.52));
            points.Add(new GeoFence.Point(31.65, -106.52));

            GeoFence gf = new GeoFence(points);

            bool pass = true;
            if (gf.IsPointInPolygon(c1)) { pass = false; }
            if (gf.IsPointInPolygon(c2)) { pass = false; }
            if (gf.IsPointInPolygon(c3)) { pass = false; }
            if (gf.IsPointInPolygon(c4)) { pass = false; }

            Pass.Write("Outside Polygon: ", pass);

            c1 = new Coordinate(31.67, -106.51);
            c2 = new Coordinate(31.67, -84.03);
            c3 = new Coordinate(42.01, -106.51);
            c4 = new Coordinate(42.01, -84.03);

            pass = true;
            if (!gf.IsPointInPolygon(c1)) { pass = false; }
            if (!gf.IsPointInPolygon(c2)) { pass = false; }
            if (!gf.IsPointInPolygon(c3)) { pass = false; }
            if (!gf.IsPointInPolygon(c4)) { pass = false; }

            Pass.Write("Inside Polygon: ", pass);

            pass = true;
            Distance d = new Distance(1000, DistanceType.Meters);
            if (!gf.IsPointInRangeOfLine(c1, 1000)) { pass = false; }
            if (!gf.IsPointInRangeOfLine(c1, d)) { pass = false; }
            Pass.Write("In Range of Polyline: ", pass);

            pass = true;
            d = new Distance(900, DistanceType.Meters);
            if (gf.IsPointInRangeOfLine(c1, 900)) { pass = false; }
            if (gf.IsPointInRangeOfLine(c1, d)) { pass = false; }

            Pass.Write("Out of Range of Polyline: ", pass);

            //Draw rectangle and ensure prescision falls withing met guideline;
            pass = true;

            Coordinate c = new Coordinate(25, 45);

            //Check Vincenty
            GeoFence.Drawer gd = new GeoFence.Drawer(c, Shape.Ellipsoid, 0);

            gd.Draw(new Distance(20), 0);
            gd.Draw(new Distance(20), 90);
            gd.Draw(new Distance(20), 90);
            gd.Draw(new Distance(20), 90);

            double latPrecision = Math.Abs(c.Latitude.ToDouble() - gd.Last.Latitude.ToDouble());
            double longPrecision = Math.Abs(c.Longitude.ToDouble() - gd.Last.Longitude.ToDouble());

            if(latPrecision >.000001 || longPrecision > .000001) { pass = false; } //111.32 mm precision boundary

            //Check Haversine
            gd = new GeoFence.Drawer(c, Shape.Sphere, 0);

            gd.Draw(new Distance(20), 0);
            gd.Draw(new Distance(20), 90);
            gd.Draw(new Distance(20), 90);
            gd.Draw(new Distance(20), 90);

            latPrecision = Math.Abs(c.Latitude.ToDouble() - gd.Last.Latitude.ToDouble());
            longPrecision = Math.Abs(c.Longitude.ToDouble() - gd.Last.Longitude.ToDouble());

            //Check shape closer
            if (latPrecision > .000001 || longPrecision > .000001) { pass = false; } //111.32 mm precision boundary

            gd = new GeoFence.Drawer(c, Shape.Ellipsoid, 0);

            gd.Draw(new Distance(20), 0);
            gd.Draw(new Distance(20), 90);
            gd.Draw(new Distance(20), 90);
            gd.Close();

            latPrecision = Math.Abs(c.Latitude.ToDouble() - gd.Last.Latitude.ToDouble());
            longPrecision = Math.Abs(c.Longitude.ToDouble() - gd.Last.Longitude.ToDouble());

            if (latPrecision != 0 || longPrecision != 0 || gd.Points.Count!=5) { pass = false; } //111.32 mm precision boundary

            Pass.Write("GeoFence Drawer Precision Checks: ", pass);

        }
    }
}
