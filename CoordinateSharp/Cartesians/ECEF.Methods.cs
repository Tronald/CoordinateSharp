/*
CoordinateSharp is a .NET standard library that is intended to ease geographic coordinate 
format conversions and location based celestial calculations.
https://github.com/Tronald/CoordinateSharp

Many celestial formulas in this library are based on Jean Meeus's 
Astronomical Algorithms (2nd Edition). Comments that reference only a chapter
are referring to this work.

License

CoordinateSharp is split licensed and may be licensed under the GNU Affero General Public License version 3 or a commercial use license as stated.

Copyright (C) 2022, Signature Group, LLC
  
This program is free software; you can redistribute it and/or modify it under the terms of the GNU Affero General Public License version 3 
as published by the Free Software Foundation with the addition of the following permission added to Section 15 as permitted in Section 7(a): 
FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY Signature Group, LLC. Signature Group, LLC DISCLAIMS THE WARRANTY OF 
NON INFRINGEMENT OF THIRD PARTY RIGHTS.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Affero General Public License for more details. You should have received a copy of the GNU 
Affero General Public License along with this program; if not, see http://www.gnu.org/licenses or write to the 
Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA, 02110-1301 USA, or download the license from the following URL:

https://www.gnu.org/licenses/agpl-3.0.html

The interactive user interfaces in modified source and object code versions of this program must display Appropriate Legal Notices, 
as required under Section 5 of the GNU Affero General Public License.

You can be released from the requirements of the license by purchasing a commercial license. Buying such a license is mandatory 
as soon as you develop commercial activities involving the CoordinateSharp software without disclosing the source code of your own applications. 
These activities include: offering paid services to customers as an ASP, on the fly location based calculations in a web application, 
or shipping CoordinateSharp with a closed source product.

Organizations or use cases that fall under the following conditions may receive a free commercial use license upon request on a case by case basis.


	-Open source contributors to this library.
	-Scholarly or scientific research.
	-Emergency response / management uses.

Please visit http://coordinatesharp.com/licensing or contact Signature Group, LLC to purchase a commercial license, or for any questions regarding the AGPL 3.0 license requirements or free use license: sales@signatgroup.com.
*/
using System;

namespace CoordinateSharp
{
    public partial class ECEF 
    {
        /// <summary>
        /// Creates an ECEF coordinate in kilometers.
        /// </summary>
        /// <remarks>
        /// ECEF values will be populated by converting from the passed geodetic Coordinate object.
        /// </remarks>
        /// <param name="coordinate">Coordinate</param>
        /// <example>
        /// The following example demonstrates how to create a populated ECEF coordinate
        /// based on a converted geodetic coordinate.
        /// <code>
        /// //Create a geodetic coordinate at N25, E45
        /// Coordinate c = new Coordinate(25,45);
        /// 
        /// //Create and convert geodetic to ECEF
	    /// ECEF ecef = new ECEF(c);
        /// 
        /// Console.WriteLine(ecef); 4089.916 km, 4089.916 km, 2679.074 km
        /// </code>
        /// </example>
        public ECEF(Coordinate coordinate)
        {
            equatorial_radius = GlobalSettings.Default_EquatorialRadius;
            inverse_flattening = GlobalSettings.Default_InverseFlattening;
            Ellipsoid();
            geodetic_height = new Distance(0);
            double[] ecef = LatLong_To_ECEF(coordinate.Latitude.DecimalDegree, coordinate.Longitude.DecimalDegree, geodetic_height.Kilometers);
            x = ecef[0];
            y = ecef[1];
            z = ecef[2];
        }
        /// <summary>
        /// Creates an ECEF coordinate in kilometers.
        /// </summary>
        /// <remarks>
        /// ECEF values will be populated by converting from the passed geodetic Coordinate object and height above ellipsoid.
        /// </remarks>
        /// <param name="coordinate">Coordinate</param>
        /// <param name="height">Height above Mean Sea Level</param>
        /// <example>
        /// The following example demonstrates how to create a populated ECEF coordinate
        /// based on a converted geodetic coordinate and height above mean sea level.
        /// <code>
        /// //Create a geodetic coordinate at N25, E45
        /// Coordinate c = new Coordinate(25,45);
        /// 
        /// //Create a distance object set at 450 meters.
        /// //This will be used to signal the ECEF coordinate is 450 meters above HAE (Height above Ellipsoid).
        /// Distance height = new Distance(450, DistanceType.Meters);
        /// 
        /// //Create and convert geodetic to ECEF
	    /// ECEF ecef = new ECEF(c, height);
        /// 
        /// Console.WriteLine(ecef); 4090.204 km, 4090.204 km, 2679.265 km
        /// </code>
        /// </example>
        public ECEF(Coordinate coordinate, Distance height)
        {
            equatorial_radius = GlobalSettings.Default_EquatorialRadius;
            inverse_flattening = GlobalSettings.Default_InverseFlattening;
            Ellipsoid();
            geodetic_height = height;
            double[] ecef = LatLong_To_ECEF(coordinate.Latitude.DecimalDegree, coordinate.Longitude.DecimalDegree, geodetic_height.Kilometers);
            x = ecef[0];
            y = ecef[1];
            z = ecef[2];
        }
        /// <summary>
        /// Create an ECEF coordinate in kilometers.
        /// </summary>
        /// <param name="xc">X coordinate in KM</param>
        /// <param name="yc">Y coordinate in KM</param>
        /// <param name="zc">Z coordinate in KM</param>
        /// <remarks>
        /// ECEF values will be populated from the provided X, Y, Z coordinates.
        /// </remarks>        
        /// <example>
        /// The following example demonstrates how to create a populated ECEF coordinate.
        /// <code>
        /// //Create an ECEF coordinate
	    /// ECEF ecef = new ECEF(4089.916, 4089.916, 2679.074);
        /// 
        /// Console.WriteLine(ecef); 4089.916 km, 4089.916 km, 2679.074 km
        /// </code>
        /// </example>
        public ECEF(double xc, double yc, double zc)
        {
            equatorial_radius = GlobalSettings.Default_EquatorialRadius;
            inverse_flattening = GlobalSettings.Default_InverseFlattening;
            Ellipsoid();
            geodetic_height = new Distance(0);
            x = xc;
            y = yc;
            z = zc;
        }
        
        /// <summary>re
        /// Updates ECEF values when eagerloading is used.
        /// </summary>
        /// <param name="coordinate">Geodetic coordinate</param>
        internal void ToECEF(Coordinate coordinate)
        {
            equatorial_radius = GlobalSettings.Default_EquatorialRadius;
            inverse_flattening = GlobalSettings.Default_InverseFlattening;
            Ellipsoid();           
            double[] ecef = LatLong_To_ECEF(coordinate.Latitude.DecimalDegree, coordinate.Longitude.DecimalDegree, geodetic_height.Kilometers);
            x = ecef[0];
            y = ecef[1];
            z = ecef[2];
        }

        /// <summary>
        /// Sets Geodetic height for ECEF conversion.
        /// </summary>
        /// <remarks>
        /// Setting the height will trigger ECEF values to recalculate.
        /// </remarks>
        /// <param name="coordinate">Geodetic coordinate</param>
        /// <param name="distance">Height above Mean Sea Level</param>
        /// <example>
        /// The following example demonstrates how to set the height above ellipsoid (HAE) at the geodetic coordinate. 
        /// The provided height is used in the conversion from geodetic to ECEF.
        /// <code>
        /// //Create a geodetic coordinate at N25, E45 at 0 HAE.
        /// Coordinate c = new Coordinate(25, 45);
        ///
        /// //Display converted ECEF values.        
        /// Console.WriteLine(c.ECEF); //4089.916 km, 4089.916 km, 2679.074 km
		///
		/// //Set geodetic coordinate height to 1500 meters above HAE
		/// c.ECEF.Set_GeoDetic_Height(c, new Distance(1500, DistanceType.Meters));						
		/// 
		/// //Display new ECEF values.        
        /// Console.WriteLine(c.ECEF); //4090.877 km, 4090.877 km, 2679.708 km
        /// </code>
        /// </example>
        public void Set_GeoDetic_Height(Coordinate coordinate, Distance distance)
        {
            geodetic_height = distance;
            double[] values = LatLong_To_ECEF(coordinate.Latitude.DecimalDegree, coordinate.Longitude.DecimalDegree, distance.Kilometers);
            x = values[0];
            y = values[1];
            z = values[2];
        }

        /// <summary>
        /// Returns a Geodetic Coordinate object based on the provided ECEF coordinate X, Y, Z values.
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="z">Z</param>
        /// <returns>Coordinate</returns>
        /// <example>
        /// The following example creates (converts to) a geodetic Coordinate object based on ECEF X, Y, Z values.
        /// <code>
        /// Coordinate c = ECEF.ECEFToLatLong(4090.877, 4090.877, 2679.708);   
        /// 
        /// Console.WriteLine(c); //N 24º 59' 59.986" E 45º 0' 0"
        /// Console.WriteLine(c.ECEF.GeoDetic_Height.Meters); //1499.97912820436
        /// </code>
        /// </example>
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
        /// Returns a Geodetic Coordinate object based on the provided ECEF coordinate.
        /// </summary>
        /// <param name="ecef">ECEF Coordinate</param>
        /// <returns>Coordinate</returns>
        /// <example>
        /// The following example creates (converts to) a geodetic Coordinate object based on an ECEF object.
        /// <code>
        /// ECEF ecef = new ECEF(4090.877, 4090.877, 2679.708);
		/// Coordinate c = ECEF.ECEFToLatLong(ecef);
        /// 
        /// Console.WriteLine(c); //N 24º 59' 59.986" E 45º 0' 0"
        /// Console.WriteLine(c.ECEF.GeoDetic_Height.Meters); //1499.97912820436
        /// </code>
        /// </example>
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
        /// Default formatted ECEF string.
        /// </summary>
        /// <remarks>
        /// X, Y, Z values are rounded to the 3rd place.
        /// </remarks>
        /// <returns>ECEF Formatted Coordinate String</returns>
        public override string ToString()
        {
            double xD = Math.Round(x, 3);
            double yD = Math.Round(y, 3);
            double zD = Math.Round(z, 3);

            if (xD == -0) { xD = 0; }
            if (yD == -0) { yD = 0; }
            if (zD == -0) { zD = 0; }

            return xD.ToString() + " km, " + yD.ToString() + " km, " + zD.ToString() + " km";
        }
  
        /*CONVERSION LOGIC*/
        
        /// <summary>
        /// Initialize EARTH global variables based on the Datum.
        /// </summary>
        private void Ellipsoid()
        {
            double a = equatorial_radius / 1000;
            double f = 1.0 / inverse_flattening;
            double b = a * (1.0 - f);

            EarthCon(a, b);
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
        /// Compute the radii at the geodetic latitude (degrees).
        /// </summary>
        /// <param name="lat">Latitude in degrees</param>
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
        /// Physical radius of the Earth.
        /// </summary>
        /// <param name="lat">Latitude in degrees</param>
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
        /// Converts geodetic latitude to geocentric latitude.
        /// </summary>
        /// <param name="flatgd">Geodetic latitude to geocentric latitude</param>
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
        /// Converts lat / long to east, north, up vectors.
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
        /// Gets ECEF vector in KM.
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
        /// Converts ECEF X, Y, Z to GeoDetic Lat / Long and Height in KM.
        /// </summary>
        /// <param name="x">X coordinate part</param>
        /// <param name="y">Y coordinate part</param>
        /// <param name="z">Z coordinate part</param>
        /// <returns>double[]</returns>
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
