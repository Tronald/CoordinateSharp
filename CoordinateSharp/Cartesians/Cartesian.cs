using System;
using System.ComponentModel;
namespace CoordinateSharp
{
    /// <summary>
    /// Cartesian (X, Y, Z) Coordinate
    /// </summary>
    [Serializable]
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
        /// Create a Cartesian Object
        /// </summary>
        /// <param name="xc">X</param>
        /// <param name="yc">Y</param>
        /// <param name="zc">Z</param>
        public Cartesian(double xc, double yc, double zc)
        {
            //formulas:
            x = xc;
            y = yc;
            z = zc;
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
        public double X
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
        public double Y
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
        public double Z
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
        /// Returns a Lat Long Coordinate object based on the provided Cartesian Coordinate
        /// </summary>
        /// <param name="cart">Cartesian Coordinate</param>
        /// <returns></returns>
        public static Coordinate CartesianToLatLong(Cartesian cart)
        {
            double x = cart.X;
            double y = cart.Y;
            double z = cart.Z;

            double lon = Math.Atan2(y, x);
            double hyp = Math.Sqrt(x * x + y * y);
            double lat = Math.Atan2(z, hyp);

            double Lat = lat * (180 / Math.PI);
            double Lon = lon * (180 / Math.PI);
            return new Coordinate(Lat, Lon);
        }
        /// <summary>
        /// Cartesian Default String Format
        /// </summary>
        /// <returns>Cartesian Formatted Coordinate String</returns>
        /// <returns>Values rounded to the 8th place</returns>
        public override string ToString()
        {
            return Math.Round(x,8).ToString() + " " + Math.Round(y, 8).ToString() + " " + Math.Round(z, 8).ToString();
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
