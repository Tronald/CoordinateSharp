using System;

namespace CoordinateSharp
{ 
    public partial class Distance
    {        
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
    }
   
}
