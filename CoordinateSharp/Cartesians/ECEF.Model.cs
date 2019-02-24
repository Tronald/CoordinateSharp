using System;
namespace CoordinateSharp
{
    /// <summary>
    /// Earth Centered - Earth Fixed (X,Y,Z) Coordinate 
    /// </summary>
    [Serializable]
    public partial class ECEF
    {      
        //Globals for calculations
        private double EARTH_A;
        private double EARTH_B;
        private double EARTH_F;
        private double EARTH_Ecc;
        private double EARTH_Esq;

        //ECEF Values
        private double x;
        private double y;
        private double z;
        private Distance geodetic_height;

        //Datum
        internal double equatorial_radius;
        internal double inverse_flattening;

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
        /// X Coordinate
        /// </summary>
        public double X
        {
            get { return x; }
        }
        /// <summary>
        /// y Coordinate
        /// </summary>
        public double Y
        {
            get { return y; }
        }
        /// <summary>
        /// Z Coordinate
        /// </summary>
        public double Z
        {
            get { return z; }
        }

        /// <summary>
        /// GeoDetic Height from Mean Sea Level.
        /// Used for converting Lat Long / ECEF.
        /// Default value is 0. Adjust as needed.
        /// </summary>
        public Distance GeoDetic_Height
        {
            get { return geodetic_height; }           
        }
    }
}
