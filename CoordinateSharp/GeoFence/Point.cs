using System;

namespace CoordinateSharp
{
    public partial class GeoFence
    {
        /// <summary>
        /// This class is a help sub class to simplify GeoFence calculus
        /// </summary>
        [Serializable]
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
