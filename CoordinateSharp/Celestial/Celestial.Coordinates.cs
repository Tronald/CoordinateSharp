/*
CoordinateSharp is a .NET standard library that is intended to ease geographic coordinate 
format conversions and location based celestial calculations.
https://github.com/Tronald/CoordinateSharp

Many celestial formulas in this library are based on Jean Meeus's 
Astronomical Algorithms (2nd Edition). Comments that reference only a chapter
are referring to this work.

License

CoordinateSharp is split licensed and may be licensed under the GNU Affero General Public License version 3 or a commercial use license as stated.

Copyright (C) 2019, Signature Group, LLC
  
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

Organizations or use cases that fall under the following conditions may receive a free commercial use license upon request.
-Department of Defense
-Department of Homeland Security
-Open source contributors to this library
-Scholarly or scientific uses on a case by case basis.
-Emergency response / management uses on a case by case basis.

For more information, please contact Signature Group, LLC at this address: sales@signatgroup.com
*/

using CoordinateSharp.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoordinateSharp
{
    /// <summary>
    /// Coordinate information of the Sun, expressed in degrees (unless otherwise specified).
    /// </summary>
    [Serializable]
    public class SolarCoordinates
    {
        //Test Against https://www.timeanddate.com/worldclock/sunearth.html     
        internal double trueLongitude;
        internal double trueLatitude;
        internal double julianDayDecimal;
        internal double radiusVector;

        internal double longitude;
        internal double latitude;
        internal double rightAscension;
        internal double declination;
        internal double geometricMeanLongitude;
    

        /// <summary>
        /// Radius Vector (expressed in astronomical units).
        /// </summary>
        public double RadiusVector { get { return radiusVector; } }

        /// <summary>
        /// Apparent Longitude.
        /// </summary>
        public double Longitude { get { return longitude; } }

        /// <summary>
        /// Apparent Latitude.
        /// </summary>
        public double Latitude { get { return latitude; } }

        /// <summary>
        /// Apparent Right Ascension.
        /// </summary>
        public double RightAscension { get { return rightAscension; } }

        /// <summary>
        /// Apparent Declination.
        /// </summary>
        public double Declination { get { return declination; } }

        /// <summary>
        /// Geometric Mean Longitude.
        /// </summary>
        public double GeometricMeanLongitude { get { return geometricMeanLongitude; } }

        /// <summary>
        /// Subsolar Latitude. The point at which the sun is perceived to be directly overhead of the Earth (at the zenith).
        /// </summary>
        public double SubsolarLatitude
        {
            get
            {
                return declination;
            } 
        }

        /// <summary>
        /// Subsolar Longitude. The point at which the sun is perceived to be directly overhead of the Earth (at the zenith).
        /// </summary>
        public double SubsolarLongitude
        {
            get
            {
                double gmsto = geometricMeanLongitude;
                gmsto += 180;

                double circle = (0*180+rightAscension - gmsto-(julianDayDecimal)*360);
                var x = circle - Math.Floor(circle / 360.0) * 360;
                if (x > 180) { x = (360 - x) * -1; }
                return x;
             

            }
        }

    }

    /// <summary>
    /// Coordinate information of the Moon, expressed in degrees (unless otherwise specified).
    /// </summary>
    [Serializable]
    public class LunarCoordinates
    {
        //Test Against https://www.timeanddate.com/worldclock/sunearth.html
        internal double longitude;
        internal double latitude;
        internal double rightAscension;
        internal double julianDayDecimal;
        internal double declination;
        internal double geometricMeanLongitude;
        internal double sunMeanLongitude; //For Over Earth Calculation

        /// <summary>
        /// Apparent Longitude.
        /// </summary>
        public double Longitude { get { return longitude; } }

        /// <summary>
        /// Apparent Latitude.
        /// </summary>
        public double Latitude { get { return latitude; } }

        /// <summary>
        /// Apparent Right Ascension.
        /// </summary>
        public double RightAscension { get { return rightAscension; } }

        /// <summary>
        /// Apparent Declination.
        /// </summary>
        public double Declination { get { return declination; } }

        /// <summary>
        /// Geometric Mean Longitude.
        /// </summary>
        public double GeometricMeanLongitude { get { return geometricMeanLongitude; } }

        /// <summary>
        /// Sublunar Latitude. The point at which the moon is perceived to be directly overhead of the Earth (at the zenith).
        /// </summary>
        public double SublunarLatitude
        {
            get
            {
                return declination;
            }
        }

        /// <summary>
        /// Sublunar Longitude. The point at which the moon is perceived to be directly overhead of the Earth (at the zenith).
        /// </summary>
        public double SublunarLongitude
        {
            get
            {
                double gmsto = sunMeanLongitude;
                gmsto += 180;

                double circle = (0 * 180 + rightAscension - gmsto - (julianDayDecimal) * 360);
                var x = circle - Math.Floor(circle / 360.0) * 360;
                if (x > 180) { x = (360 - x) * -1; }
                return x;            
            }
        }

    }
}
