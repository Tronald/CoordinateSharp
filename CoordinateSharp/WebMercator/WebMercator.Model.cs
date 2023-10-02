/*
CoordinateSharp is a .NET standard library that is intended to ease geographic coordinate 
format conversions and location based celestial calculations.
https://github.com/Tronald/CoordinateSharp

Many celestial formulas in this library are based on Jean Meeus's 
Astronomical Algorithms (2nd Edition). Comments that reference only a chapter
are referring to this work.

License

CoordinateSharp is split licensed and may be licensed under the GNU Affero General Public License version 3 or a commercial use license as stated.

Copyright (C) 2023, Signature Group, LLC
  
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
    /// <summary>
    /// Web Mercator EPSG:3857 coordinate system. Must use WGS84 datum.
    /// Coordinates above +/- 85.06 Latitude should not be considered accurate due to Web Mercator specified limitations.
    /// </summary>
    [Serializable]
    public partial class WebMercator
    {
        private Coordinate coordinate;

        private double easting;
        private double northing;

        private double false_easting=0;
        private double false_northing=0;

        /// <summary>
        /// Web Mercator Easting
        /// </summary>
        public double Easting
        {
            get { return easting; }
            internal set { easting = value; }
        }

        /// <summary>
        /// Web Mercator Northing
        /// </summary>
        public double Northing
        {
            get 
            { 
                return northing; 
            }
            internal set { northing = value; }
        }

        /// <summary>
        /// Web Mercator False Easting (Set internal, add feature for user input if requested).
        /// </summary>
        internal double False_Easting
        {
            get { return false_easting; }
            //internal set { false_easting = value; }
        }

        /// <summary>
        /// Web Mercator  False Northing (Set internal, add feature for user input if requested).
        /// </summary>
        internal double False_Northing
        {
            get { return false_northing; }
            //internal set { false_northing = value; }
        }
          
    }
}
