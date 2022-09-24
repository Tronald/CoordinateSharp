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
using System.Collections.Generic;
using System.Linq;
using System.Text;

//REFERENCE TEC-SR-7 US ARMY 1996
namespace CoordinateSharp
{
    /// <summary>
    /// Universal Polar Stereographic (FOR UTM POLAR REGION)
    /// </summary>
    internal class UPS
    {
        public void Geodetic_To_UPS(double lat, double longi, UniversalTransverseMercator utm)
        {
            if (longi == -180) { longi = 180; }//RESET FOR CALCS
            //LAT LONG TO UPS
            //TEC-SR-7 US ARMY Corps of Engineers CONVERSIONS TEXT PAGE 99 STEP 0
            double latRad = lat * Math.PI / 180;
            double longRad = longi * Math.PI / 180;

            //STEP 1
            //E = Eccentricity
            //E^2 = (a^2 - b^2) / a2
            //A and B pulled from Table A-1
            //A-1 are datum value, pull in form Coordinate object

            //ELLIPSOID
            double f = 1/utm.Inverse_Flattening; //Flattening (convert from Inverse that CoordinateSharp works in)
            double a = utm.Equatorial_Radius; //Semi-Major Axis
            double b = a*(1-f); //Semi Minor Axis a(1-f)           
            double E2 = (2 * f) - Math.Pow(f, 2); //Eccentricity Squared          
            double E = Math.Sqrt(E2); //Eccentricity 

            //STEP 2
            double K = ((2 * (a * a)) / b) * Math.Pow(((1 - E) / (1 + E)), (E / 2));

            //STEP 3
            double KTan = K * Math.Tan((Math.PI / 4) - (Math.Abs(latRad) / 2));
            double r = KTan * Math.Pow((1 + E * Math.Sin(Math.Abs(latRad))) / (1 - E * Math.Sin(Math.Abs(latRad))), (E / 2));

            //STEP 4
            double x = 2000000 + .994 * r * Math.Sin(longRad);

            double y;
            if (lat > 0) { y = 2000000 - .994 * r * Math.Cos(longRad); }
            else { y = 2000000 + .994 * r * Math.Cos(longRad); }
           

            //GRAB LETTER.
            string zone;
            if (lat >= 0 )
            {
                if (longi >= 0 || longi == -180||lat==90) { zone = "Z"; }
                else { zone = "Y"; }
            }
            else
            {
                if (longi >= 0 || longi==-180 || lat==-90) { zone = "B"; }                
                else { zone = "A"; }
            }

            utm.Easting = x;
            utm.Northing = y;
            utm.LongZone = 0;
            utm.LatZone = zone;

        }
        public static Coordinate UPS_To_Geodetic(UniversalTransverseMercator utm, EagerLoad el)
        {
            //INTERNATIONAL ELLIPSOID
            double f = 1 / utm.Inverse_Flattening; //Flattening (convert from Inverse that CoordinateSharp works in)
            double a = utm.Equatorial_Radius; //Semi-Major Axis
            double b = a * (1 - f); //Semi Minor Axis a(1-f)           
                                   

            double E2 = (2 * f) - Math.Pow(f, 2); //Eccentricity Squared
            double E = Math.Sqrt(E2); //Eccentricity 

            //CONVERT BACK
            double x = utm.Easting;
            double y = utm.Northing;          

            //STEP 1          
            double Xps =(x - 2000000) / .994;

            double Yps = (y - 2000000) / .994; // 001116144;
            
            double tPS = Yps; //Used for true longi calcs.

            if (Yps == 0) { Yps = 1;  }
            
            //STEP 2
            //ATAN = ARCTAN

            bool southernHemi = true;
            if (utm.LatZone.ToUpper() == "Z" || utm.LatZone.ToUpper() == "Y") { southernHemi = false; }

            double longRad;
            double longRadForCalcs; //USED FOR LAT CALCS. LongRad is will LongRad. This is needed to due exact 90 issues.
            if (southernHemi)
            {
                longRad = Math.PI + Math.Atan(Xps / tPS);
                longRadForCalcs = Math.PI + Math.Atan(Xps / Yps);
            }
            else
            {
                longRad = Math.PI - Math.Atan(Xps / tPS);
                longRadForCalcs = Math.PI - Math.Atan(Xps / Yps);
            }          

            //STEP 3
            double K = (2 * Math.Pow(a, 2) / b) * Math.Pow(((1 - E) / (1 + E)), (E / 2));


            //STEP 4
            double absYps = Math.Abs(Yps);
            double kCos = K * Math.Abs(Math.Cos(longRadForCalcs));
            double q = Math.Log(absYps / kCos) / Math.Log(Math.E) * -1;
           
            //STEP 5
            double estLat = 2 * Math.Atan(Math.Pow(Math.E, q)) - (Math.PI / 2);


            double lat = 0;
            while (Math.Abs(estLat - lat) > .0000001)
            {
                if (double.IsInfinity(estLat)) { break; }
                lat = estLat;
                //STEP 6       
                double bracket = (1 + Math.Sin(estLat)) / (1 - Math.Sin(estLat)) * Math.Pow((1 - E * Math.Sin(estLat)) / (1 + E * Math.Sin(estLat)), E);
                double fLat = -q + 1 / 2.0 * Math.Log(bracket);
                double fLat2 = (1 - Math.Pow(E, 2)) / ((1 - Math.Pow(E, 2) * Math.Pow(Math.Sin(estLat), 2)) * Math.Cos(estLat));

                //STEP 7
                estLat = estLat - fLat / fLat2;
               
            }
            if (!double.IsInfinity(estLat))
            {
                lat = estLat;
            }
          
            //NaN signals poles
            double latDeg;
            if (double.IsNaN(lat))
            {
                latDeg = 90;
            }
            else
            {
                latDeg = lat * (180 / Math.PI); //Radians to Degrees                
            }
            if (southernHemi) { latDeg *= -1; }
          

            double longDeg;
            if (double.IsNaN(longRad)) { longDeg = 0; }
            else
            {
                longDeg = (longRad) * (180 / Math.PI);
            }
            if (utm.Easting<2000000)
            {
                longDeg = 180 - longDeg % 180; //Normalize to 180 degrees
                longDeg *= -1; //Set Western Hemi
            }
           
            else if (longDeg > 180)
            {
                longDeg -= 180;
            }
            else if (longDeg < -180)
            {
                longDeg += 180;
            }

            if(utm.Northing >= 2000000 && Xps == 0 && southernHemi ) { longDeg = 0; } // SET TO 0 or it will equate to 180
            if(utm.Northing < 2000000 && Xps == 0 && !southernHemi) { longDeg = 0; } // SET TO 0 or it will equate to 180
            return new Coordinate(latDeg, longDeg, el);
          
        }
    }
}
