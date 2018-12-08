using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
namespace CoordinateSharp
{
    /// <summary>
    /// Earth Centered - Earth Fixed (X,Y,Z) Coordinate 
    /// </summary>
    [Serializable]
    public class ECEF : INotifyPropertyChanged
    {
        /// <summary>
        /// Create an ECEF Object
        /// </summary>
        /// <param name="c">Coordinate</param>
        public ECEF(Coordinate c)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Create an ECEF Object
        /// </summary>
        /// <param name="xc">X</param>
        /// <param name="yc">Y</param>
        /// <param name="zc">Z</param>
        public ECEF(double xc, double yc, double zc)
        {          
            x = xc;
            y = yc;
            z = zc;
        }
        /// <summary>
        /// Updates ECEF Values
        /// </summary>
        /// <param name="c"></param>
        public void ToECEF(Coordinate c)
        {
            throw new NotImplementedException();
        }
        private double x;
        private double y;
        private double z;

        /// <summary>
        /// X Coordinate
        /// </summary>
        public Double X
        {
            get { return x; }
            set
            {
                if (x != value)
                {
                    x = value;
                    NotifyPropertyChanged("X");
                }
            }
        }
        /// <summary>
        /// y Coordinate
        /// </summary>
        public Double Y
        {
            get { return y; }
            set
            {
                if (y != value)
                {
                    y = value;
                    NotifyPropertyChanged("Y");
                }
            }
        }
        /// <summary>
        /// Z Coordinate
        /// </summary>
        public Double Z
        {
            get { return z; }
            set
            {
                if (z != value)
                {
                    z = value;
                    NotifyPropertyChanged("Z");
                }
            }
        }


        /// <summary>
        /// Returns a Geodetic Coordinate object based on the provided ECEF Coordinate
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="z">Z</param>
        /// <returns>Coordinate</returns>
        public static Coordinate ECEFToLatLong(double x, double y, double z)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Returns a Geodetic Coordinate object based on the provided ECEF Coordinate
        /// </summary>
        /// <param name="ecef">ECEF Coordinate</param>
        /// <returns>Coordinate</returns>
        public static Coordinate ECEFToLatLong(ECEF ecef)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// ECEF Default String Format
        /// </summary>
        /// <returns>ECEF Formatted Coordinate String</returns>
        /// <returns>Values rounded to the 3rd place</returns>
        public override string ToString()
        {
            return Math.Round(x, 3).ToString() + " " + Math.Round(y, 3).ToString() + " " + Math.Round(z, 3).ToString();
        }

        /// <summary>
        /// Property changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Notify property changed
        /// </summary>
        /// <param name="propName">Property name</param>
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
