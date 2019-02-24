using System;

namespace CoordinateSharp
{
    /// <summary>
    /// Contains distance values between two coordinates.
    /// </summary>
    [Serializable]
    public partial class Distance
    {
        private double kilometers;
        private double miles;
        private double feet;
        private double meters;
        private double bearing;
        private double nauticalMiles;       
      
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
}
