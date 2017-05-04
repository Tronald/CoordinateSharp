using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;

namespace CoordinateSharp
{
    /// <summary>
    /// Universal Transverse Mercator (UTM) coordinate system
    /// </summary>
    public class UniversalTransverseMercator : INotifyPropertyChanged
    {
        private Coordinate coordinate;

        private string latZone;
        private int longZone;
        private double easting;
        private double northing;   

        public string LatZone
        {
            get { return this.latZone; }
            set
            {
                if (this.latZone != value)
                {
                    this.latZone = value;
                    this.NotifyPropertyChanged("LatZone");
                    //double[] d = FromUTM.convertUTMToLatLong(this);
                    double[] d = FromUTM(longZone, latZone, easting, northing, this);

                    coordinate.Latitude.DecimalDegree = d[0];                   
                    coordinate.Longitude.DecimalDegree = d[1];

                    coordinate.Latitude.NotifyPropertyChanged("DecimalDegree");
                    coordinate.Longitude.NotifyPropertyChanged("DecimalDegree");
                }
            }
        }
        public int LongZone
        {
            get { return this.longZone; }
            set
            {
                if (this.longZone != value)
                {
                    this.longZone = value;
                    this.NotifyPropertyChanged("LongZone");
                    double[] d = FromUTM(longZone, latZone, easting, northing, this);
                    //double[] d = FromUTM.convertUTMToLatLong(this);
                    coordinate.Latitude.DecimalDegree = d[0];
                    coordinate.Longitude.DecimalDegree = d[1];

                    coordinate.Latitude.NotifyPropertyChanged("DecimalDegree");
                    coordinate.Longitude.NotifyPropertyChanged("DecimalDegree");
                }
            }
        }
        public double Easting
        {
            get { return this.easting; }
            set
            {
                if (this.easting != value)
                {
                    this.easting = value;
                    this.NotifyPropertyChanged("Easting");
                    double[] d = FromUTM(longZone, latZone, easting, northing, this);
                    //double[] d = FromUTM.convertUTMToLatLong(this);
                    coordinate.Latitude.DecimalDegree = d[0];
                    coordinate.Longitude.DecimalDegree = d[1];

                    coordinate.Latitude.NotifyPropertyChanged("DecimalDegree");
                    coordinate.Longitude.NotifyPropertyChanged("DecimalDegree");
                }
            }
        }
        public double Northing
        {
            get { return this.northing; }
            set
            {
                if (this.northing != value)
                {
                    this.northing = value;
                    this.NotifyPropertyChanged("Northing");
                    double[] d = FromUTM(longZone, latZone, easting, northing, this);
                    //double[] d = FromUTM.convertUTMToLatLong(this);
                    coordinate.Latitude.DecimalDegree = d[0];
                    coordinate.Longitude.DecimalDegree = d[1];

                    coordinate.Latitude.NotifyPropertyChanged("DecimalDegree");
                    coordinate.Longitude.NotifyPropertyChanged("DecimalDegree");
                }
            }
        }

        public UniversalTransverseMercator(double lat, double longi, Coordinate c)
        {
            //validate coords

            //if (lat > 180) { throw new ArgumentOutOfRangeException("Degrees out of range", "Longitudinal coordinate decimal cannot be greater than 180."); }
            //if (lat < -180) { throw new ArgumentOutOfRangeException("Degrees out of range", "Longitudinal coordinate decimal cannot be less than 180."); }

            //if (longi > 90) { throw new ArgumentOutOfRangeException("Degrees out of range", "Latitudinal coordinate decimal cannot be greater than 90."); }
            //if (longi < -90) { throw new ArgumentOutOfRangeException("Degrees out of range", "Latitudinal coordinate decimal cannot be less than 90."); }

            ToUTM(lat,longi, this);
           
            coordinate = c;
        }
        private UniversalTransverseMercator(string latz, int longz, double e, double n, Coordinate c)
        {
            //validate utm
            if (longz < 1 || longz > 60) { throw new ArgumentOutOfRangeException("Longitudinal zone out of range", "UTM longitudinal zones must be between 1-60."); }
            if (!Verify_Lat_Zone(latz)) { throw new ArgumentException("Latitudinal zone invalid", "UTM latitudinal zone was unrecognized."); }
            if (e < 160000 || e > 834000) { throw new WarningException("The Easting value provided is outside the max allowable range. If this is intentional, use with caution."); }
            if (n < 0 || n > 10000000) { throw new ArgumentOutOfRangeException("Northing out of range", "Northing must be between 0-10,000,000."); }

            latZone = latz;
            longZone = longz;

            easting = e;
            northing = n;

            coordinate = c;
           // double[] d = FromUTM.convertUTMToLatLong(this);
            double[] d = FromUTM(longZone, latZone, easting, northing, this);
            coordinate.Latitude.DecimalDegree = d[0];
            coordinate.Longitude.DecimalDegree = d[1];
            coordinate.Latitude.NotifyPropertyChanged("DecimalDegree");
            coordinate.Longitude.NotifyPropertyChanged("DecimalDegree");
           
        }
       
        private bool Verify_Lat_Zone(string l)
        {
            if (LatZones.longZongLetters.Where(x => x == l.ToUpper()).Count() != 1)
            {
                return false;
            }
            return true;
        }     

        /// <summary>
        /// Property changed event handler.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Notifies Coordinate property of changing.
        /// </summary>
        /// <param name="propName"></param>
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
                if (propName != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
                }
            }
        }

        public void ToUTM(double lat, double longi, UniversalTransverseMercator utm)
        {      
            string letter = "";
            double easting = 0;
            double northing = 0;
            int zone = (int)Math.Floor(longi / 6 + 31);
            if (lat < -72)
                letter = "C";
            else if (lat < -64)
                letter = "D";
            else if (lat < -56)
                letter = "E";
            else if (lat < -48)
                letter = "F";
            else if (lat < -40)
                letter = "G";
            else if (lat < -32)
                letter = "H";
            else if (lat < -24)
                letter = "J";
            else if (lat < -16)
                letter = "K";
            else if (lat < -8)
                letter = "L";
            else if (lat < 0)
                letter = "M";
            else if (lat < 8)
                letter = "N";
            else if (lat < 16)
                letter = "P";
            else if (lat < 24)
                letter = "Q";
            else if (lat < 32)
                letter = "R";
            else if (lat < 40)
                letter = "S";
            else if (lat < 48)
                letter = "T";
            else if (lat < 56)
                letter = "U";
            else if (lat < 64)
                letter = "V";
            else if (lat < 72)
                letter = "W";
            else
                letter = "X";
            easting = 0.5 * Math.Log((1 + Math.Cos(lat * Math.PI / 180) * Math.Sin(longi * Math.PI / 180 - (6 * zone - 183) * Math.PI / 180)) / (1 - Math.Cos(lat *
                Math.PI / 180) * Math.Sin(longi * Math.PI / 180 - (6 * zone - 183) * Math.PI / 180))) * 0.9996 * 6399593.62 / Math.Pow((1 + Math.Pow(0.0820944379,
                2) * Math.Pow(Math.Cos(lat * Math.PI / 180), 2)), 0.5) * (1 + Math.Pow(0.0820944379, 2) / 2 * Math.Pow((0.5 * Math.Log((1 + Math.Cos(lat * Math.PI /
                180) * Math.Sin(longi * Math.PI / 180 - (6 * zone - 183) * Math.PI / 180)) / (1 - Math.Cos(lat * Math.PI / 180) * Math.Sin(longi * Math.PI / 180 -
                (6 * zone - 183) * Math.PI / 180)))), 2) * Math.Pow(Math.Cos(lat * Math.PI / 180), 2) / 3) + 500000;
            easting = Math.Round(easting * 100) * 0.01;
            northing = (Math.Atan(Math.Tan(lat * Math.PI / 180) / Math.Cos((longi * Math.PI / 180 - (6 * zone - 183) * Math.PI / 180))) - lat * Math.PI / 180) *
                0.9996 * 6399593.625 / Math.Sqrt(1 + 0.006739496742 * Math.Pow(Math.Cos(lat * Math.PI / 180), 2)) * (1 + 0.006739496742 / 2 * Math.Pow(0.5 *
                Math.Log((1 + Math.Cos(lat * Math.PI / 180) * Math.Sin((longi * Math.PI / 180 - (6 * zone - 183) * Math.PI / 180))) / (1 - Math.Cos(lat * Math.PI /
                180) * Math.Sin((longi * Math.PI / 180 - (6 * zone - 183) * Math.PI / 180)))), 2) * Math.Pow(Math.Cos(lat * Math.PI / 180), 2)) + 0.9996 *
                6399593.625 * (lat * Math.PI / 180 - 0.005054622556 * (lat * Math.PI / 180 + Math.Sin(2 * lat * Math.PI / 180) / 2) + 4.258201531e-05 * (3 * (lat *
                Math.PI / 180 + Math.Sin(2 * lat * Math.PI / 180) / 2) + Math.Sin(2 * lat * Math.PI / 180) * Math.Pow(Math.Cos(lat * Math.PI / 180), 2)) / 4 -
                1.674057895e-07 * (5 * (3 * (lat * Math.PI / 180 + Math.Sin(2 * lat * Math.PI / 180) / 2) + Math.Sin(2 * lat * Math.PI / 180) *
                Math.Pow(Math.Cos(lat * Math.PI / 180), 2)) / 4 + Math.Sin(2 * lat * Math.PI / 180) * Math.Pow(Math.Cos(lat * Math.PI / 180), 2) *
                Math.Pow(Math.Cos(lat * Math.PI / 180), 2)) / 3);
            if ((new[] { "C", "D", "E", "F", "G", "H", "J", "K", "L", "M" }).Contains(letter))
            { northing = northing + 10000000; }
            northing = Math.Round(northing * 100) * 0.01;

            utm.latZone = letter;
            utm.longZone = zone;
            utm.easting = easting;
            utm.northing = northing;
        }
        public double[] FromUTM(int zone, string letter, double easting, double northing, UniversalTransverseMercator utm)
        {
            double[] d = { 0, 0 };
            double north;
            if ((new[] { "C", "D", "E", "F", "G", "H", "J", "K", "L", "M" }).Contains(letter))
            {
                north = northing - 10000000;
            }
            else
            {
                north = northing;
            }

            double latitude = (north / 6366197.724 / 0.9996 + (1 + 0.006739496742 * Math.Pow(Math.Cos(north / 6366197.724 / 0.9996), 2) - 0.006739496742 *
                Math.Sin(north / 6366197.724 / 0.9996) * Math.Cos(north / 6366197.724 / 0.9996) * (Math.Atan(Math.Cos(Math.Atan((Math.Exp((easting - 500000) /
                (0.9996 * 6399593.625 / Math.Sqrt((1 + 0.006739496742 * Math.Pow(Math.Cos(north / 6366197.724 / 0.9996), 2)))) * (1 - 0.006739496742 *
                Math.Pow((easting - 500000) / (0.9996 * 6399593.625 / Math.Sqrt((1 + 0.006739496742 * Math.Pow(Math.Cos(north / 6366197.724 / 0.9996), 2)))), 2) /
                2 * Math.Pow(Math.Cos(north / 6366197.724 / 0.9996), 2) / 3)) - Math.Exp(-(easting - 500000) / (0.9996 * 6399593.625 /
                Math.Sqrt((1 + 0.006739496742 * Math.Pow(Math.Cos(north / 6366197.724 / 0.9996), 2)))) * (1 - 0.006739496742 * Math.Pow((easting - 500000) /
                (0.9996 * 6399593.625 / Math.Sqrt((1 + 0.006739496742 * Math.Pow(Math.Cos(north / 6366197.724 / 0.9996), 2)))), 2) / 2 * Math.Pow(Math.Cos(north /
                6366197.724 / 0.9996), 2) / 3))) / 2 / Math.Cos((north - 0.9996 * 6399593.625 * (north / 6366197.724 / 0.9996 - 0.006739496742 * 3 / 4 *
                (north / 6366197.724 / 0.9996 + Math.Sin(2 * north / 6366197.724 / 0.9996) / 2) + Math.Pow(0.006739496742 * 3 / 4, 2) * 5 / 3 * (3 * (north /
                6366197.724 / 0.9996 + Math.Sin(2 * north / 6366197.724 / 0.9996) / 2) + Math.Sin(2 * north / 6366197.724 / 0.9996) * Math.Pow(Math.Cos(north /
                6366197.724 / 0.9996), 2)) / 4 - Math.Pow(0.006739496742 * 3 / 4, 3) * 35 / 27 * (5 * (3 * (north / 6366197.724 / 0.9996 + Math.Sin(2 * north /
                6366197.724 / 0.9996) / 2) + Math.Sin(2 * north / 6366197.724 / 0.9996) * Math.Pow(Math.Cos(north / 6366197.724 / 0.9996), 2)) / 4 + Math.Sin(2 *
                north / 6366197.724 / 0.9996) * Math.Pow(Math.Cos(north / 6366197.724 / 0.9996), 2) * Math.Pow(Math.Cos(north / 6366197.724 / 0.9996), 2)) / 3)) /
                (0.9996 * 6399593.625 / Math.Sqrt((1 + 0.006739496742 * Math.Pow(Math.Cos(north / 6366197.724 / 0.9996), 2)))) * (1 - 0.006739496742 *
                Math.Pow((easting - 500000) / (0.9996 * 6399593.625 / Math.Sqrt((1 + 0.006739496742 * Math.Pow(Math.Cos(north / 6366197.724 / 0.9996), 2)))), 2) /
                2 * Math.Pow(Math.Cos(north / 6366197.724 / 0.9996), 2)) + north / 6366197.724 / 0.9996))) * Math.Tan((north - 0.9996 * 6399593.625 * (north /
                6366197.724 / 0.9996 - 0.006739496742 * 3 / 4 * (north / 6366197.724 / 0.9996 + Math.Sin(2 * north / 6366197.724 / 0.9996) / 2) +
                Math.Pow(0.006739496742 * 3 / 4, 2) * 5 / 3 * (3 * (north / 6366197.724 / 0.9996 + Math.Sin(2 * north / 6366197.724 / 0.9996) / 2) +
                Math.Sin(2 * north / 6366197.724 / 0.9996) * Math.Pow(Math.Cos(north / 6366197.724 / 0.9996), 2)) / 4 - Math.Pow(0.006739496742 * 3 / 4, 3) * 35 /
                27 * (5 * (3 * (north / 6366197.724 / 0.9996 + Math.Sin(2 * north / 6366197.724 / 0.9996) / 2) + Math.Sin(2 * north / 6366197.724 / 0.9996) *
                Math.Pow(Math.Cos(north / 6366197.724 / 0.9996), 2)) / 4 + Math.Sin(2 * north / 6366197.724 / 0.9996) * Math.Pow(Math.Cos(north / 6366197.724 /
                0.9996), 2) * Math.Pow(Math.Cos(north / 6366197.724 / 0.9996), 2)) / 3)) / (0.9996 * 6399593.625 / Math.Sqrt((1 + 0.006739496742 *
                Math.Pow(Math.Cos(north / 6366197.724 / 0.9996), 2)))) * (1 - 0.006739496742 * Math.Pow((easting - 500000) / (0.9996 * 6399593.625 /
                Math.Sqrt((1 + 0.006739496742 * Math.Pow(Math.Cos(north / 6366197.724 / 0.9996), 2)))), 2) / 2 * Math.Pow(Math.Cos(north / 6366197.724 /
                0.9996), 2)) + north / 6366197.724 / 0.9996)) - north / 6366197.724 / 0.9996) * 3 / 2) * (Math.Atan(Math.Cos(Math.Atan((Math.Exp((easting -
                500000) / (0.9996 * 6399593.625 / Math.Sqrt((1 + 0.006739496742 * Math.Pow(Math.Cos(north / 6366197.724 / 0.9996), 2)))) *
                (1 - 0.006739496742 * Math.Pow((easting - 500000) / (0.9996 * 6399593.625 / Math.Sqrt((1 + 0.006739496742 * Math.Pow(Math.Cos(north /
                6366197.724 / 0.9996), 2)))), 2) / 2 * Math.Pow(Math.Cos(north / 6366197.724 / 0.9996), 2) / 3)) - Math.Exp(-(easting - 500000) /
                (0.9996 * 6399593.625 / Math.Sqrt((1 + 0.006739496742 * Math.Pow(Math.Cos(north / 6366197.724 / 0.9996), 2)))) * (1 - 0.006739496742 *
                Math.Pow((easting - 500000) / (0.9996 * 6399593.625 / Math.Sqrt((1 + 0.006739496742 * Math.Pow(Math.Cos(north / 6366197.724 / 0.9996), 2)))), 2) /
                2 * Math.Pow(Math.Cos(north / 6366197.724 / 0.9996), 2) / 3))) / 2 / Math.Cos((north - 0.9996 * 6399593.625 * (north / 6366197.724 / 0.9996 -
                0.006739496742 * 3 / 4 * (north / 6366197.724 / 0.9996 + Math.Sin(2 * north / 6366197.724 / 0.9996) / 2) + Math.Pow(0.006739496742 * 3 / 4, 2) *
                5 / 3 * (3 * (north / 6366197.724 / 0.9996 + Math.Sin(2 * north / 6366197.724 / 0.9996) / 2) + Math.Sin(2 * north / 6366197.724 / 0.9996) *
                Math.Pow(Math.Cos(north / 6366197.724 / 0.9996), 2)) / 4 - Math.Pow(0.006739496742 * 3 / 4, 3) * 35 / 27 * (5 * (3 * (north / 6366197.724 /
                0.9996 + Math.Sin(2 * north / 6366197.724 / 0.9996) / 2) + Math.Sin(2 * north / 6366197.724 / 0.9996) * Math.Pow(Math.Cos(north / 6366197.724 /
                0.9996), 2)) / 4 + Math.Sin(2 * north / 6366197.724 / 0.9996) * Math.Pow(Math.Cos(north / 6366197.724 / 0.9996), 2) * Math.Pow(Math.Cos(north /
                6366197.724 / 0.9996), 2)) / 3)) / (0.9996 * 6399593.625 / Math.Sqrt((1 + 0.006739496742 * Math.Pow(Math.Cos(north / 6366197.724 / 0.9996), 2))))
                * (1 - 0.006739496742 * Math.Pow((easting - 500000) / (0.9996 * 6399593.625 / Math.Sqrt((1 + 0.006739496742 * Math.Pow(Math.Cos(north /
                6366197.724 / 0.9996), 2)))), 2) / 2 * Math.Pow(Math.Cos(north / 6366197.724 / 0.9996), 2)) + north / 6366197.724 / 0.9996))) * Math.Tan((north -
                0.9996 * 6399593.625 * (north / 6366197.724 / 0.9996 - 0.006739496742 * 3 / 4 * (north / 6366197.724 / 0.9996 + Math.Sin(2 * north / 6366197.724 /
                0.9996) / 2) + Math.Pow(0.006739496742 * 3 / 4, 2) * 5 / 3 * (3 * (north / 6366197.724 / 0.9996 + Math.Sin(2 * north / 6366197.724 / 0.9996) / 2) +
                Math.Sin(2 * north / 6366197.724 / 0.9996) * Math.Pow(Math.Cos(north / 6366197.724 / 0.9996), 2)) / 4 - Math.Pow(0.006739496742 * 3 / 4, 3) * 35 /
                27 * (5 * (3 * (north / 6366197.724 / 0.9996 + Math.Sin(2 * north / 6366197.724 / 0.9996) / 2) + Math.Sin(2 * north / 6366197.724 / 0.9996) *
                Math.Pow(Math.Cos(north / 6366197.724 / 0.9996), 2)) / 4 + Math.Sin(2 * north / 6366197.724 / 0.9996) * Math.Pow(Math.Cos(north / 6366197.724 /
                0.9996), 2) * Math.Pow(Math.Cos(north / 6366197.724 / 0.9996), 2)) / 3)) / (0.9996 * 6399593.625 / Math.Sqrt((1 + 0.006739496742 *
                Math.Pow(Math.Cos(north / 6366197.724 / 0.9996), 2)))) * (1 - 0.006739496742 * Math.Pow((easting - 500000) / (0.9996 * 6399593.625 / Math.Sqrt((1 +
                0.006739496742 * Math.Pow(Math.Cos(north / 6366197.724 / 0.9996), 2)))), 2) / 2 * Math.Pow(Math.Cos(north / 6366197.724 / 0.9996), 2)) + north /
                6366197.724 / 0.9996)) - north / 6366197.724 / 0.9996)) * 180 / Math.PI;
            latitude = Math.Round(latitude * 10000000);
            latitude = latitude / 10000000;

            double longitude = Math.Atan((Math.Exp((easting - 500000) / (0.9996 * 6399593.625 / Math.Sqrt((1 + 0.006739496742 * Math.Pow(Math.Cos(north /
                6366197.724 / 0.9996), 2)))) * (1 - 0.006739496742 * Math.Pow((easting - 500000) / (0.9996 * 6399593.625 / Math.Sqrt((1 + 0.006739496742 *
                Math.Pow(Math.Cos(north / 6366197.724 / 0.9996), 2)))), 2) / 2 * Math.Pow(Math.Cos(north / 6366197.724 / 0.9996), 2) / 3)) - Math.Exp(-(easting -
                500000) / (0.9996 * 6399593.625 / Math.Sqrt((1 + 0.006739496742 * Math.Pow(Math.Cos(north / 6366197.724 / 0.9996), 2)))) * (1 - 0.006739496742 *
                Math.Pow((easting - 500000) / (0.9996 * 6399593.625 / Math.Sqrt((1 + 0.006739496742 * Math.Pow(Math.Cos(north / 6366197.724 / 0.9996), 2)))), 2) /
                2 * Math.Pow(Math.Cos(north / 6366197.724 / 0.9996), 2) / 3))) / 2 / Math.Cos((north - 0.9996 * 6399593.625 * (north / 6366197.724 / 0.9996 -
                0.006739496742 * 3 / 4 * (north / 6366197.724 / 0.9996 + Math.Sin(2 * north / 6366197.724 / 0.9996) / 2) + Math.Pow(0.006739496742 * 3 / 4, 2) *
                5 / 3 * (3 * (north / 6366197.724 / 0.9996 + Math.Sin(2 * north / 6366197.724 / 0.9996) / 2) + Math.Sin(2 * north / 6366197.724 / 0.9996) *
                Math.Pow(Math.Cos(north / 6366197.724 / 0.9996), 2)) / 4 - Math.Pow(0.006739496742 * 3 / 4, 3) * 35 / 27 * (5 * (3 * (north / 6366197.724 /
                0.9996 + Math.Sin(2 * north / 6366197.724 / 0.9996) / 2) + Math.Sin(2 * north / 6366197.724 / 0.9996) * Math.Pow(Math.Cos(north / 6366197.724 /
                0.9996), 2)) / 4 + Math.Sin(2 * north / 6366197.724 / 0.9996) * Math.Pow(Math.Cos(north / 6366197.724 / 0.9996), 2) * Math.Pow(Math.Cos(north /
                6366197.724 / 0.9996), 2)) / 3)) / (0.9996 * 6399593.625 / Math.Sqrt((1 + 0.006739496742 * Math.Pow(Math.Cos(north / 6366197.724 / 0.9996), 2)))) *
                (1 - 0.006739496742 * Math.Pow((easting - 500000) / (0.9996 * 6399593.625 / Math.Sqrt((1 + 0.006739496742 * Math.Pow(Math.Cos(north / 6366197.724 /
                0.9996), 2)))), 2) / 2 * Math.Pow(Math.Cos(north / 6366197.724 / 0.9996), 2)) + north / 6366197.724 / 0.9996)) * 180 / Math.PI + zone * 6 - 183;
            longitude = Math.Round(longitude * 10000000);
            longitude = longitude / 10000000;
            d[0] = latitude;
            d[1] = longitude;
            return d;

        }
      
        public override string ToString()
        {
            return this.longZone.ToString() + this.LatZone + " " + (int)this.easting + "mE " + (int)this.northing + "mN";
        }            
    }
   
}
