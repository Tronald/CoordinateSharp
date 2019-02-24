using System;

namespace CoordinateSharp
{
    /// <summary>
    /// Cartesian (X, Y, Z) Coordinate
    /// </summary>
    [Serializable]
    public partial class Cartesian
    {
        private double x;
        private double y;
        private double z;

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
        /// Returns a Lat Long Coordinate object based on the provided Cartesian Coordinate
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="z">Z</param>
        /// <returns></returns>
            
    }
}
