using System;
using System.Diagnostics;
namespace CoordinateSharp
{
    /// <summary>
    /// Contains distance values between two coordinates.
    /// </summary>
    [Serializable]
    public class Distance
    {
        private double kilometers;
        private double miles;
        private double feet;
        private double meters;
        private double bearing;
        private double nauticalMiles;

        /// <summary>
        /// Initializes a distance object using Haversine (Spherical Earth).
        /// </summary>
        /// <param name="c1">Coordinate 1</param>
        /// <param name="c2">Coordinate 2</param>
        public Distance(Coordinate c1, Coordinate c2)
        {
            Haversine(c1, c2);
        }
        /// <summary>
        /// Initializes a distance object using Haversine (Spherical Earth) or Vincenty (Elliptical Earth).
        /// </summary>
        /// <param name="c1">Coordinate 1</param>
        /// <param name="c2">Coordinate 2</param>
        /// <param name="shape">Shape of earth</param>
        public Distance(Coordinate c1, Coordinate c2, Shape shape)
        {
            if (shape == Shape.Sphere)
            {
                Haversine(c1, c2);
            }
            else
            {
                Vincenty(c1, c2);
            }
        }
        /// <summary>
        /// Initializes distance object based on distance in KM
        /// </summary>
        /// <param name="km">Kilometers</param>
        public Distance(double km)
        {
            kilometers = km;
            meters = km * 1000;
            feet = meters * 3.28084;
            miles = meters * 0.000621371;
            nauticalMiles = meters * 0.0005399565;
            bearing = 0;//None specified
        }
        /// <summary>
        /// Initializaes distance object based on specified distance and measurement type
        /// </summary>
        /// <param name="distance">Distance</param>
        /// <param name="type">Measurement type</param>
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
        private void Vincenty(Coordinate c1, Coordinate c2)
        {
            double lat1, lat2, lon1, lon2;
            double d, crs12, crs21;

            lat1 = c1.Latitude.ToRadians();
            lat2 = c2.Latitude.ToRadians();
            lon1 = c1.Longitude.ToRadians();
            lon2 = c2.Longitude.ToRadians();
            //Ensure datums match between coords
            if ((c1.equatorial_radius != c2.equatorial_radius) || (c1.inverse_flattening != c2.inverse_flattening))
            {
                throw new InvalidOperationException("The datum set does not match between Coordinate objects.");
            }
            double[] ellipse = new double[] { c1.equatorial_radius, c1.inverse_flattening };


            // elliptic code
            double[] cde = Distance_Assistant.Dist_Ell(lat1, -lon1, lat2, -lon2, ellipse);  // ellipse uses East negative
            crs12 = cde[1] * (180 / Math.PI); //Bearing
            crs21 = cde[2] * (180 / Math.PI); //Reverse Bearing
            d = cde[0]; //Distance

            bearing = crs21;
            //reverseBearing = crs12;
            meters = d;
            kilometers = d / 1000;
            feet = d * 3.28084;
            miles = d * 0.000621371;
            nauticalMiles = d * 0.0005399565;

        }

        private void Haversine(Coordinate c1, Coordinate c2)
        {
            ////RADIANS
            double nLat = c1.Latitude.ToDouble() * Math.PI / 180;
            double nLong = c1.Longitude.ToDouble() * Math.PI / 180;
            double cLat = c2.Latitude.ToDouble() * Math.PI / 180;
            double cLong = c2.Longitude.ToDouble() * Math.PI / 180;

            //Calcs
            double R = 6371e3; //meters
            double v1 = nLat;
            double v2 = cLat;
            double latRad = (c2.Latitude.ToDouble() - c1.Latitude.ToDouble()) * Math.PI / 180;
            double longRad = (c2.Longitude.ToDouble() - c1.Longitude.ToDouble()) * Math.PI / 180;

            double a = Math.Sin(latRad / 2.0) * Math.Sin(latRad / 2.0) +
                Math.Cos(nLat) * Math.Cos(cLat) * Math.Sin(longRad / 2.0) * Math.Sin(longRad / 2.0);
            double cl = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double dist = R * cl;

            //Get bearing
            double y = Math.Sin(cLong - nLong) * Math.Cos(cLat);
            double x = Math.Cos(nLat) * Math.Sin(cLat) -
                    Math.Sin(nLat) * Math.Cos(cLat) * Math.Cos(cLong - nLong);
            double brng = Math.Atan2(y, x) * (180 / Math.PI); //Convert bearing back to degrees.
            if (brng < 0) { brng -= 180; brng = Math.Abs(brng); }
            kilometers = dist / 1000;
            meters = dist;
            feet = dist * 3.28084;
            miles = dist * 0.000621371;
            nauticalMiles = dist * 0.0005399565;
            bearing = brng;
        }
        /// <summary>
        /// Distance in Kilometers
        /// </summary>
        public double Kilometers
        {
            get { return kilometers; }
        }
        /// <summary>
        /// Distance in Statute Miles
        /// </summary>
        public double Miles
        {
            get { return miles; }
        }
        /// <summary>
        /// Distance in Nautical Miles
        /// </summary>
        public double NauticalMiles
        {
            get { return nauticalMiles; }
        }
        /// <summary>
        /// Distance in Meters
        /// </summary>
        public double Meters
        {
            get { return meters; }
        }
        /// <summary>
        /// Distance in Feet
        /// </summary>
        public double Feet
        {
            get { return feet; }
        }
        /// <summary>
        /// Initial Bearing from Coordinate 1 to Coordinate 2
        /// </summary>
        public double Bearing
        {
            get { return bearing; }
        }
    }
    /// <summary>
    /// Distance measurement type
    /// </summary>
    public enum DistanceType
    {
        /// <summary>
        /// Distance in Meters
        /// </summary>
        Meters,
        /// <summary>
        /// Distance in Kilometers
        /// </summary>
        Kilometers,
        /// <summary>
        /// Distance in Feet
        /// </summary>
        Feet,
        /// <summary>
        /// Distance in Statute Miles
        /// </summary>
        Miles,
        /// <summary>
        /// Distance in Nautical Miles
        /// </summary>
        NauticalMiles
    }

    [Serializable]
    internal class Distance_Assistant
    {
        /// <summary>
        /// Returns new geodetic coordinate in radians
        /// </summary>
        /// <param name="glat1">Latitude in Radians</param>
        /// <param name="glon1">Longitude in Radians</param>
        /// <param name="faz">Bearing</param>
        /// <param name="s">Distance</param>
        /// <param name="ellipse">Earth Ellipse Values</param>
        /// <returns>double[]</returns>
        public static double[] Direct_Ell(double glat1, double glon1, double faz, double s, double[] ellipse)
        {
            double EPS = 0.00000000005;//Used to determine if starting at pole.
            double r, tu, sf, cf, b, cu, su, sa, c2a, x, c, d, y, sy = 0, cy = 0, cz = 0, e = 0;
            double glat2, glon2, f;

            //Determine if near pole
            if ((Math.Abs(Math.Cos(glat1)) < EPS) && !(Math.Abs(Math.Sin(faz)) < EPS))
            {
                Debug.WriteLine("Warning: Location is at earth's pole. Only N-S courses are meaningful at this location.");
            }


            double a = ellipse[0];//Equitorial Radius
            f = 1 / ellipse[1];//Flattening
            r = 1 - f;
            tu = r * Math.Tan(glat1);
            sf = Math.Sin(faz);
            cf = Math.Cos(faz);
            if (cf == 0)
            {
                b = 0.0;
            }
            else
            {
                b = 2.0 * Math.Atan2(tu, cf);
            }
            cu = 1.0 / Math.Sqrt(1 + tu * tu);
            su = tu * cu;
            sa = cu * sf;
            c2a = 1 - sa * sa;
            x = 1.0 + Math.Sqrt(1.0 + c2a * (1.0 / (r * r) - 1.0));
            x = (x - 2.0) / x;
            c = 1.0 - x;
            c = (x * x / 4.0 + 1.0) / c;
            d = (0.375 * x * x - 1.0) * x;
            tu = s / (r * a * c);
            y = tu;
            c = y + 1;
            while (Math.Abs(y - c) > EPS)
            {
                sy = Math.Sin(y);
                cy = Math.Cos(y);
                cz = Math.Cos(b + y);
                e = 2.0 * cz * cz - 1.0;
                c = y;
                x = e * cy;
                y = e + e - 1.0;
                y = (((sy * sy * 4.0 - 3.0) * y * cz * d / 6.0 + x) *
                        d / 4.0 - cz) * sy * d + tu;
            }

            b = cu * cy * cf - su * sy;
            c = r * Math.Sqrt(sa * sa + b * b);
            d = su * cy + cu * sy * cf;

            glat2 = ModM.ModLat(Math.Atan2(d, c));
            c = cu * cy - su * sy * cf;
            x = Math.Atan2(sy * sf, c);
            c = ((-3.0 * c2a + 4.0) * f + 4.0) * c2a * f / 16.0;
            d = ((e * cy * c + cz) * sy * c + y) * sa;
            glon2 = ModM.ModLon(glon1 + x - (1.0 - c) * d * f);  //Adjust for IDL
            //baz = ModM.ModCrs(Math.Atan2(sa, b) + Math.PI);
            return new double[] { glat2, glon2 };
        }
        /// <summary>
        /// Returns new geodetic coordinate in radians
        /// </summary>
        /// <param name="lat1">Latitude in radians</param>
        /// <param name="lon1">Longitude in radians</param>
        /// <param name="crs12">Bearing</param>
        /// <param name="d12">Distance</param>
        /// <returns>double[]</returns>
        public static double[] Direct(double lat1, double lon1, double crs12, double d12)
        {
            var EPS = 0.00000000005;//Used to determine if near pole.
            double dlon, lat, lon;
            d12 = d12 * 0.0005399565; //convert meter to nm
            d12 = d12 / (180 * 60 / Math.PI);//Convert to Radian
            //Determine if near pole
            if ((Math.Abs(Math.Cos(lat1)) < EPS) && !(Math.Abs(Math.Sin(crs12)) < EPS))
            {
                Debug.WriteLine("Warning: Location is at earth's pole. Only N-S courses are meaningful at this location.");
            }

            lat = Math.Asin(Math.Sin(lat1) * Math.Cos(d12) +
                          Math.Cos(lat1) * Math.Sin(d12) * Math.Cos(crs12));
            if (Math.Abs(Math.Cos(lat)) < EPS)
            {
                lon = 0.0; //endpoint a pole
            }
            else
            {
                dlon = Math.Atan2(Math.Sin(crs12) * Math.Sin(d12) * Math.Cos(lat1),
                              Math.Cos(d12) - Math.Sin(lat1) * Math.Sin(lat));
                lon = ModM.Mod(lon1 - dlon + Math.PI, 2 * Math.PI) - Math.PI;
            }

            return new double[] { lat, lon };
        }
        public static double[] Dist_Ell(double glat1, double glon1, double glat2, double glon2, double[] ellipse)
        {
            double a = ellipse[0]; //Equitorial Radius
            double f = 1 / ellipse[1]; //Flattening

            double r, tu1, tu2, cu1, su1, cu2, s1, b1, f1;
            double x = 0, sx = 0, cx = 0, sy = 0, cy = 0, y = 0, sa = 0, c2a = 0, cz = 0, e = 0, c = 0, d = 0;
            double EPS = 0.00000000005;
            double faz, baz, s;
            double iter = 1;
            double MAXITER = 100;
            if ((glat1 + glat2 == 0.0) && (Math.Abs(glon1 - glon2) == Math.PI))
            {
                Debug.WriteLine("Warning: Course and distance between antipodal points is undefined");
                glat1 = glat1 + 0.00001; // allow algorithm to complete
            }
            if (glat1 == glat2 && (glon1 == glon2 || Math.Abs(Math.Abs(glon1 - glon2) - 2 * Math.PI) < EPS))
            {
                Debug.WriteLine("Warning: Points 1 and 2 are identical- course undefined");
                //D
                //crs12
                //crs21
                return new double[] { 0, 0, Math.PI };
            }
            r = 1 - f;
            tu1 = r * Math.Tan(glat1);
            tu2 = r * Math.Tan(glat2);
            cu1 = 1.0 / Math.Sqrt(1.0 + tu1 * tu1);
            su1 = cu1 * tu1;
            cu2 = 1.0 / Math.Sqrt(1.0 + tu2 * tu2);
            s1 = cu1 * cu2;
            b1 = s1 * tu2;
            f1 = b1 * tu1;
            x = glon2 - glon1;
            d = x + 1; // force one pass
            while ((Math.Abs(d - x) > EPS) && (iter < MAXITER))
            {
                iter = iter + 1;
                sx = Math.Sin(x);
                cx = Math.Cos(x);
                tu1 = cu2 * sx;
                tu2 = b1 - su1 * cu2 * cx;
                sy = Math.Sqrt(tu1 * tu1 + tu2 * tu2);
                cy = s1 * cx + f1;
                y = Math.Atan2(sy, cy);
                sa = s1 * sx / sy;
                c2a = 1 - sa * sa;
                cz = f1 + f1;
                if (c2a > 0.0)
                {
                    cz = cy - cz / c2a;
                }
                e = cz * cz * 2.0 - 1.0;
                c = ((-3.0 * c2a + 4.0) * f + 4.0) * c2a * f / 16.0;
                d = x;
                x = ((e * cy * c + cz) * sy * c + y) * sa;
                x = (1.0 - c) * x * f + glon2 - glon1;
            }
            faz = ModM.ModCrs(Math.Atan2(tu1, tu2));
            baz = ModM.ModCrs(Math.Atan2(cu1 * sx, b1 * cx - su1 * cu2) + Math.PI);
            x = Math.Sqrt((1 / (r * r) - 1) * c2a + 1);
            x += 1;
            x = (x - 2.0) / x;
            c = 1.0 - x;
            c = (x * x / 4.0 + 1.0) / c;
            d = (0.375 * x * x - 1.0) * x;
            x = e * cy;
            s = ((((sy * sy * 4.0 - 3.0) * (1.0 - e - e) * cz * d / 6.0 - x) * d / 4.0 + cz) * sy * d + y) * c * a * r;

            if (Math.Abs(iter - MAXITER) < EPS)
            {
                Debug.WriteLine("Warning: Distance algorithm did not converge");
            }

            return new double[] { s, faz, baz };
        }
    }
}
