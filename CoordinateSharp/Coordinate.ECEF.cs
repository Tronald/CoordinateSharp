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
            equatorial_radius = 6378137.0;
            inverse_flattening = 298.257223563;
            WGS84();
            geodetic_height = new Distance(0);
            double[] ecef = LatLong_To_ECEF(c.Latitude.DecimalDegree, c.Longitude.DecimalDegree, geodetic_height.Kilometers);
            x = ecef[0];
            y = ecef[1];
            z = ecef[2];
        }
        /// <summary>
        /// Create an ECEF Object
        /// </summary>
        /// <param name="c">Coordinate</param>
        /// <param name="height">Coordinate</param>
        public ECEF(Coordinate c, Distance height)
        {
            equatorial_radius = 6378137.0;
            inverse_flattening = 298.257223563;
            WGS84();
            geodetic_height = height;
            double[] ecef = LatLong_To_ECEF(c.Latitude.DecimalDegree, c.Longitude.DecimalDegree, geodetic_height.Kilometers);
            x = ecef[0];
            y = ecef[1];
            z = ecef[2];
        }
        /// <summary>
        /// Create an ECEF Object
        /// </summary>
        /// <param name="xc">X</param>
        /// <param name="yc">Y</param>
        /// <param name="zc">Z</param>
        public ECEF(double xc, double yc, double zc)
        {
            equatorial_radius = 6378137.0;
            inverse_flattening = 298.257223563;
            WGS84();
            geodetic_height = new Distance(0);
            x = xc;
            y = yc;
            z = zc;
        }
        /// <summary>
        /// Updates ECEF Values
        /// </summary>
        /// <param name="c">Coordinate</param>
        public void ToECEF(Coordinate c)
        {
            equatorial_radius = 6378137.0;
            inverse_flattening = 298.257223563;
            WGS84();           
            double[] ecef = LatLong_To_ECEF(c.Latitude.DecimalDegree, c.Longitude.DecimalDegree, geodetic_height.Kilometers);
            x = ecef[0];
            y = ecef[1];
            z = ecef[2];
        }

        //Globals for calucations
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
        /// GeoDetic Height from Mean Sea Level.
        /// Used for converting Lat Long / ECEF.
        /// Default value is 0. Adjust as needed.
        /// </summary>
        public Distance GeoDetic_Height
        {
            get { return geodetic_height; }
            internal set
            {
                if (geodetic_height != value)
                {
                    geodetic_height = value;
                    NotifyPropertyChanged("Height");
                    
                }
            }
        }

        /// <summary>
        /// Sets GeoDetic height for ECEF conversion.
        /// Recalculate ECEF Coordinate
        /// </summary>
        /// <param name="c">Coordinate</param>
        /// <param name="dist">Height</param>
        public void Set_GeoDetic_Height(Coordinate c, Distance dist)
        {
            geodetic_height = dist;
            double[] values = LatLong_To_ECEF(c.Latitude.DecimalDegree, c.Longitude.DecimalDegree, dist.Kilometers);
            x = values[0];
            y = values[1];
            z = values[2];

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
            ECEF ecef = new ECEF(x, y, z);
            double[] values = ecef.ECEF_To_LatLong(x, y, z);
            ecef.geodetic_height =new Distance(values[2]);
         
            Coordinate c = new Coordinate(values[0], values[1]);
            c.ECEF = ecef;
            return c;
        }
        /// <summary>
        /// Returns a Geodetic Coordinate object based on the provided ECEF Coordinate
        /// </summary>
        /// <param name="ecef">ECEF Coordinate</param>
        /// <returns>Coordinate</returns>
        public static Coordinate ECEFToLatLong(ECEF ecef)
        {           
            double[] values = ecef.ECEF_To_LatLong(ecef.X, ecef.Y, ecef.Z);
           
            Coordinate c = new Coordinate(values[0], values[1]);
            Distance height = new Distance(values[2]);
         
            ecef.geodetic_height = new Distance(values[2]);
            c.ECEF = ecef;
          
            return c;
        }
        /// <summary>
        /// ECEF Default String Format
        /// </summary>
        /// <returns>ECEF Formatted Coordinate String</returns>
        /// <returns>Values rounded to the 3rd place</returns>
        public override string ToString()
        {
            return Math.Round(x, 3).ToString() + " km, " + Math.Round(y, 3).ToString() + " km, " + Math.Round(z, 3).ToString() + " km";
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
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        //CONVERSION LOGIC      
        /// <summary>
        /// Initialize EARTH global variables based on the Datum
        /// </summary>
        private void WGS84()
        {
            double wgs84a = equatorial_radius / 1000;
            double wgs84f = 1.0 / inverse_flattening;
            double wgs84b = wgs84a * (1.0 - wgs84f);

            EarthCon(wgs84a, wgs84b);
        }

        /// <summary>
        /// Sets Earth Constants as Globals
        /// </summary>
        /// <param name="a">a</param>
        /// <param name="b">b</param>
        private void EarthCon(double a, double b)
        {
            double f = 1 - b / a;
            double eccsq = 1 - b * b / (a * a);
            double ecc = Math.Sqrt(eccsq);

            EARTH_A = a;
            EARTH_B = b;
            EARTH_F = f;
            EARTH_Ecc = ecc;
            EARTH_Esq = eccsq;
        }

        /// <summary>
        /// Compute the radii at the geodetic latitude (degrees)
        /// </summary>
        /// <param name="lat">Latitude in degres</param>
        /// <returns>double[]</returns>
        private double[] radcur(double lat)
        {
            double[] rrnrm = new double[3];

            double dtr = Math.PI / 180.0;

            double a = EARTH_A;
            double b = EARTH_B;

            double asq = a * a;
            double bsq = b * b;
            double eccsq = 1 - bsq / asq;
            double ecc = Math.Sqrt(eccsq);

            double clat = Math.Cos(dtr * lat);
            double slat = Math.Sin(dtr * lat);

            double dsq = 1.0 - eccsq * slat * slat;
            double d = Math.Sqrt(dsq);

            double rn = a / d;
            double rm = rn * (1.0 - eccsq) / dsq;

            double rho = rn * clat;
            double z = (1.0 - eccsq) * rn * slat;
            double rsq = rho * rho + z * z;
            double r = Math.Sqrt(rsq);

            rrnrm[0] = r;
            rrnrm[1] = rn;
            rrnrm[2] = rm;

            return (rrnrm);

        }

        /// <summary>
        /// Physical radius of the Earth
        /// </summary>
        /// <param name="lat">Latidude in degrees</param>
        /// <returns>double</returns>
        private double rearth(double lat)
        {
            double[] rrnrm;
            rrnrm = radcur(lat);
            double r = rrnrm[0];

            return r;
        }

        /// <summary>
        /// Converts geocentric latitude to geodetic latitude
        /// </summary>
        /// <param name="flatgc">Geocentric latitude</param>
        /// <param name="altkm">Altitude in KM</param>
        /// <returns>double</returns>
        private double gc2gd(double flatgc, double altkm)
        {
            var dtr = Math.PI / 180.0;
            var rtd = 1 / dtr;

            double ecc = EARTH_Ecc;
            double esq = ecc * ecc;

            //approximation by stages
            //1st use gc-lat as if is gd, then correct alt dependence

            double altnow = altkm;

            double[] rrnrm = radcur(flatgc);
            double rn = rrnrm[1];

            double ratio = 1 - esq * rn / (rn + altnow);

            double tlat = Math.Tan(dtr * flatgc) / ratio;
            double flatgd = rtd * Math.Atan(tlat);

            //now use this approximation for gd-lat to get rn etc.

            rrnrm = radcur(flatgd);
            rn = rrnrm[1];

            ratio = 1 - esq * rn / (rn + altnow);
            tlat = Math.Tan(dtr * flatgc) / ratio;
            flatgd = rtd * Math.Atan(tlat);

            return flatgd;
        }

        /// <summary>
        /// Converts geodetic latitude to geocentric latitude
        /// </summary>
        /// <param name="flatgd">Geodetic latitude tp geocentric latitide</param>
        /// <param name="altkm">Altitude in KM</param>
        /// <returns>double</returns>
        private double gd2gc(double flatgd, double altkm)
        {
            double dtr = Math.PI / 180.0;
            double rtd = 1 / dtr;

            double ecc = EARTH_Ecc;
            double esq = ecc * ecc;

            double altnow = altkm;

            double[] rrnrm = radcur(flatgd);
            double rn = rrnrm[1];

            double ratio = 1 - esq * rn / (rn + altnow);

            double tlat = Math.Tan(dtr * flatgd) * ratio;
            double flatgc = rtd * Math.Atan(tlat);

            return flatgc;
        }

        /// <summary>
        /// Converts lat / long to east, north, up vectors
        /// </summary>
        /// <param name="flat">Latitude</param>
        /// <param name="flon">Longitude</param>
        /// <returns>Array[] of double[]</returns>
        private Array[] llenu(double flat, double flon)
        {
            double clat, slat, clon, slon;
            double[] ee = new double[3];
            double[] en = new double[3];
            double[] eu = new double[3];

            Array[] enu = new Array[3];

            double dtr = Math.PI / 180.0;

            clat = Math.Cos(dtr * flat);
            slat = Math.Sin(dtr * flat);
            clon = Math.Cos(dtr * flon);
            slon = Math.Sin(dtr * flon);

            ee[0] = -slon;
            ee[1] = clon;
            ee[2] = 0.0;

            en[0] = -clon * slat;
            en[1] = -slon * slat;
            en[2] = clat;

            eu[0] = clon * clat;
            eu[1] = slon * clat;
            eu[2] = slat;

            enu[0] = ee;
            enu[1] = en;
            enu[2] = eu;

            return enu;
        }

        /// <summary>
        /// Gets ECEF vector in KM
        /// </summary>
        /// <param name="lat">Latitude</param>
        /// <param name="longi">Longitude</param>
        /// <param name="altkm">Altitude in KM</param>
        /// <returns>double[]</returns>
        private double[] LatLong_To_ECEF(double lat, double longi, double altkm)
        {
            double dtr = Math.PI / 180.0;

            double clat = Math.Cos(dtr * lat);
            double slat = Math.Sin(dtr * lat);
            double clon = Math.Cos(dtr * longi);
            double slon = Math.Sin(dtr * longi);

            double[] rrnrm = radcur(lat);
            double rn = rrnrm[1];
            double re = rrnrm[0];

            double ecc = EARTH_Ecc;
            double esq = ecc * ecc;

            double x = (rn + altkm) * clat * clon;
            double y = (rn + altkm) * clat * slon;
            double z = ((1 - esq) * rn + altkm) * slat;

            double[] xvec = new double[3];

            xvec[0] = x;
            xvec[1] = y;
            xvec[2] = z;

            return xvec;
        }
     
        /// <summary>
        /// Converts ECEF X, Y, Z to GeoDetic Lat / Long and Height in KM
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        private double[] ECEF_To_LatLong(double x, double y, double z)
        {
            var dtr = Math.PI / 180.0;

            double[] rrnrm = new double[3];
            double[] llhvec = new double[3];
            double slat, tangd, flatn, dlat, clat;
            double flat;
            double altkm;

            double esq = EARTH_Esq;

            double rp = Math.Sqrt(x * x + y * y + z * z);

            double flatgc = Math.Asin(z / rp) / dtr;
            double flon;
            double testval = Math.Abs(x) + Math.Abs(y);
            if (testval < 1.0e-10)
            { flon = 0.0; }
            else
            { flon = Math.Atan2(y, x) / dtr; }
            if (flon < 0.0) { flon = flon + 360.0; }

            double p = Math.Sqrt(x * x + y * y);

            //Pole special case

            if (p < 1.0e-10)
            {
                flat = 90.0;
                if (z < 0.0) { flat = -90.0; }

                altkm = rp - rearth(flat);
                llhvec[0] = flat;
                llhvec[1] = flon;
                llhvec[2] = altkm;

                return llhvec;
            }

            //first iteration, use flatgc to get altitude 
            //and alt needed to convert gc to gd lat.

            double rnow = rearth(flatgc);
            altkm = rp - rnow;
            flat = gc2gd(flatgc, altkm);

            rrnrm = radcur(flat);
            double rn = rrnrm[1];

            for (int kount = 0; kount < 5; kount++)
            {
                slat = Math.Sin(dtr * flat);
                tangd = (z + rn * esq * slat) / p;
                flatn = Math.Atan(tangd) / dtr;

                dlat = flatn - flat;
                flat = flatn;
                clat = Math.Cos(dtr * flat);

                rrnrm = radcur(flat);
                rn = rrnrm[1];

                altkm = (p / clat) - rn;

                if (Math.Abs(dlat) < 1.0e-12) { break; }

            }
            //CONVERTER WORKS IN E LAT ONLY, IF E LAT > 180 LAT IS WEST SO IT MUCST BE CONVERTED TO Decimal

            if (flon > 180) { flon = flon - 360; }
            llhvec[0] = flat;
            llhvec[1] = flon;
            llhvec[2] = altkm;

            return llhvec;
        }

       
    }
}
