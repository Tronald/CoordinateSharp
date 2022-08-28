using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace CoordinateSharp
{
    public partial class WebMercator
    {
        /// <summary>
        /// Parses a string into a Web Mercator coordinate.
        /// </summary>
        /// <param name="value">string</param>
        /// <returns>WebMercator</returns>
        /// <example>
        /// The following example parses a Web Mercator coordinate.
        /// <code>
        /// WebMecator wmc = WebMercator.Parse("8284118.2 6339892.6");    
        /// </code>
        /// </example>
        public static WebMercator Parse(string value)
        {
            WebMercator wmc;         
            if (TryParse(value, out wmc)) { return wmc; }
            throw new FormatException(string.Format("Input Coordinate \"{0}\" was not in a correct format.", value));
        }
     
     

        /// <summary>
        /// Attempts to parse a string into an Web Mercator coordinate.
        /// </summary>
        /// <param name="value">string</param>
        /// <param name="wmc">WebMercator</param>
        /// <returns>WebMercator</returns>
        /// <example>
        /// The following example attempts to parse a Web Mercator coordinate.
        /// <code>
        /// WebMercator wmc;
        /// if(!WebMercator.TryParse("8284118.2 6339892.6", out wmc))
        /// {
        ///     Console.WriteLine(wmc);//8284118.2mE 6339892.6mN
        /// }
        /// </code>
        /// </example>
        public static bool TryParse(string value, out WebMercator wmc)
        {
            string[] vals = null;
            if (FormatFinder.TryWebMercator(value, out vals))
            {
                try
                {

                    double easting = Convert.ToDouble(vals[0]);
                    double northing = Convert.ToDouble(vals[1]);
                    wmc = new WebMercator(easting, northing);

                    return true;
                }
                catch
                {
                    //silent fail, return false.
                }
            }
            wmc = null;
            return false;
        }    
    }
}
