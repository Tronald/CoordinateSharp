using System;


namespace CoordinateSharp
{
    /// <summary>
    /// Military Grid Reference System (MGRS). Uses the WGS 84 Datum.
    /// Relies upon values from the UniversalTransverseMercator class
    /// </summary>
    [Serializable]
    public partial class MilitaryGridReferenceSystem 
    {
        private double equatorialRadius;
        private double inverseFlattening;

        private string latZone;
        private int longZone;
        private double easting;
        private double northing;
        private string digraph;

        private bool withinCoordinateSystemBounds = true;
     

        /// <summary>
        /// MGRS Zone Letter
        /// </summary>
        public string LatZone
        {
            get { return latZone; }

        }
        /// <summary>
        /// MGRS Zone Number
        /// </summary>
        public int LongZone
        {
            get { return longZone; }

        }
        /// <summary>
        /// MGRS Easting
        /// </summary>
        public double Easting
        {
            get { return easting; }

        }
        /// <summary>
        /// MGRS Northing
        /// </summary>
        public double Northing
        {
            get { return northing; }

        }
        /// <summary>
        /// MGRS Digraph
        /// </summary>
        public string Digraph
        {
            get { return digraph; }
        }

        /// <summary>
        /// Is MGRS conversion within the coordinate system's accurate boundaries after conversion from Lat/Long.
        /// </summary>
        public bool WithinCoordinateSystemBounds
        {
            get { return withinCoordinateSystemBounds; }
        }
    }
}
