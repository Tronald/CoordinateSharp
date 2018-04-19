using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
namespace CoordinateSharp
{
    /// <summary>
    /// Cartesian (X, Y, Z) Coordinate
    /// </summary>
    public class Cartesian : INotifyPropertyChanged
    {
        /// <summary>
        /// Create a Cartesian Object
        /// </summary>
        /// <param name="c"></param>
        public Cartesian(Coordinate c)
        {
            //formulas:
            x = Math.Cos(c.Latitude.ToRadians()) * Math.Cos(c.Longitude.ToRadians());
            y = Math.Cos(c.Latitude.ToRadians()) * Math.Sin(c.Longitude.ToRadians());
            z = Math.Sin(c.Latitude.ToRadians());
        }
        /// <summary>
        /// Updates Cartesian Values
        /// </summary>
        /// <param name="c"></param>
        public void ToCartesian(Coordinate c)
        {
            x = Math.Cos(c.Latitude.ToRadians()) * Math.Cos(c.Longitude.ToRadians());
            y = Math.Cos(c.Latitude.ToRadians()) * Math.Sin(c.Longitude.ToRadians());
            z = Math.Sin(c.Latitude.ToRadians());
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
                if(x != value)
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
        /// Returns a Lat Long Coordinate object based on the provided Cartesian Coordinate
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="z">Z</param>
        /// <returns></returns>
        public static Coordinate CartesianToLatLong(double x, double y, double z)
        {
            double lon = Math.Atan2(y, x);
            double hyp = Math.Sqrt(x * x + y * y);
            double lat = Math.Atan2(z, hyp);

            double Lat = lat * (180 / Math.PI);
            double Lon = lon * (180 / Math.PI);
            return new Coordinate(Lat, Lon);
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
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
