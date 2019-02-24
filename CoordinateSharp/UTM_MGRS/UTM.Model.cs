using System;


namespace CoordinateSharp
{
    /// <summary>
    /// Universal Transverse Mercator (UTM) coordinate system. Uses the WGS 84 Datum.
    /// </summary>
    [Serializable]
    public partial class UniversalTransverseMercator 
    {
        private Coordinate coordinate;

        internal double equatorial_radius;
        internal double inverse_flattening;
        private string latZone;
        private int longZone;

        private double easting;
        private double northing;

        private bool withinCoordinateSystemBounds = true;

        /// <summary>
        /// UTM Zone Letter
        /// </summary>
        public string LatZone
        {
            get { return latZone; }
        }
        /// <summary>
        /// UTM Zone Number
        /// </summary>
        public int LongZone
        {
            get { return longZone; }
        }
        /// <summary>
        /// UTM Easting
        /// </summary>
        public double Easting
        {
            get { return easting; }
        }
        /// <summary>
        /// UTM Northing
        /// </summary>
        public double Northing
        {
            get { return northing; }
        }

        /// <summary>
        /// Datum Equatorial Radius / Semi Major Axis
        /// </summary>
        public double Equatorial_Radius
        {
            get { return equatorial_radius; }
        }
        /// <summary>
        /// Datum Flattening
        /// </summary>
        public double Inverse_Flattening
        {
            get { return inverse_flattening; }
        }

        /// <summary>
        /// Is the UTM conversion within the coordinate system's accurate boundaries after conversion from Lat/Long.
        /// </summary>
        public bool WithinCoordinateSystemBounds
        {
            get { return withinCoordinateSystemBounds; }
        }     
    }
}
