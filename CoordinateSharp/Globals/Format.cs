using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoordinateSharp.Formatters
{
    /// <summary>
    /// Static formatters designed to assist with format conversions of coordinates and celestial data points.
    /// </summary>
    public static class Format
    {
        /// <summary>
        /// Converts degrees to radians.
        /// </summary>
        /// <param name="degrees">Degrees</param>
        /// <returns>double</returns>
        public static double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }

        /// <summary>
        /// Converts radians to degrees.
        /// </summary>
        /// <param name="radians">Radians</param>
        /// <returns>double</returns>
        public static double ToDegrees(double radians)
        {
            return radians * 180 / Math.PI;
        }
        /// <summary>
        /// Converts DDM format to signed degrees format.
        /// </summary>
        /// <param name="degrees">Degrees</param>
        /// <param name="decimalMinutes">Decimal Minutes</param>
        /// <returns>double</returns>
        public static double ToDegrees(int degrees, double decimalMinutes)
        {
            return degrees + (Math.Sign(degrees) * (decimalMinutes / 60));
        }

        /// <summary>
        /// Converts DMS format to signed degrees format.
        /// </summary>
        /// <param name="degrees">Degrees</param>
        /// <param name="minutes">Minutes</param>
        /// <param name="seconds">Seconds</param>
        /// <returns>double</returns>
        public static double ToDegrees(int degrees, int minutes, double seconds)
        {
            double decimalMinutes = minutes + (seconds / 60);
            return degrees + (Math.Sign(degrees) * (decimalMinutes / 60));
        }

        /// <summary>
        /// Converts signed degrees to DDM format.
        /// </summary>
        /// <param name="signedDegrees">Signed Degrees</param>
        /// <returns>double[Degrees, Decimal Minutes]</returns>
        public static double[] ToDegreeDecimalMinutes(double signedDegrees)
        {
            //Update the Degree & Decimal Minute
            double degFloor = Math.Truncate(signedDegrees); //Truncate the number left to extract the degree          
            double ddm = signedDegrees - degFloor; //Extract decimalMinute value from decimalDegree
            ddm *= 60; //Multiply by 60 to get readable decimalMinute
            double dm = Convert.ToDouble(ddm); //Convert decimalMinutes back to double for storage
            int df = Convert.ToInt32(degFloor); //Convert degrees to int for storage

            return new double[] {df,dm};
        }

        /// <summary>
        /// Converts signed degrees to DMS format.
        /// </summary>
        /// <param name="signedDegrees">Signed Degrees</param>
        /// <returns>double[Degrees, Minutes, Seconds]</returns>
        public static double[] ToDegreesMinutesSeconds(double signedDegrees)
        {
            var ddm = ToDegreeDecimalMinutes(signedDegrees);

            double dmFloor = Math.Truncate(ddm[1]); //Get number left of decimal to grab minute value
            int mF = Convert.ToInt32(dmFloor); //Convert minute to int for storage
     
            double s = ddm[1] - dmFloor; //Get seconds from minutes
            s *= 60; //Multiply by 60 to get readable seconds
            double secs = Convert.ToDouble(s); //Convert back to double for storage

            return new double[] { ddm[0], dmFloor, secs };
        }

        /// <summary>
        /// Normalizes degrees to a value between 0 and 360.
        /// </summary>
        /// <param name="degree">Signed Degrees</param>
        /// <returns>double</returns>
        public static double NormalizeDegrees360(double degree)
        {
            double normalized = degree % 360;
            if (normalized < 0)
            {
                normalized = 360 + normalized;
            }

            return normalized;
        }

        /// <summary>
        /// Converts signed degrees to HMS format.
        /// </summary>
        /// <param name="degrees">Signed Degrees</param>
        /// <returns>double[Hours, Minutes, Seconds]</returns>
        public static double[] ToHoursMinutesSeconds(double degrees)
        {
            double hoursDecimal = degrees / 15;
            double hours = Math.Truncate(hoursDecimal);
            double minutesDecimal = (hoursDecimal - hours)*60;
            double minutes = Math.Truncate(minutesDecimal);
            double seconds = (minutesDecimal - minutes) * 60;
           
            return new double[] { hours, minutes ,seconds };
        }
    }
}
