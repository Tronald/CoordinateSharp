/*
CoordinateSharp is a .NET standard library that is intended to ease geographic coordinate 
format conversions and location based celestial calculations.
https://github.com/Tronald/CoordinateSharp

Many celestial formulas in this library are based on Jean Meeus's 
Astronomical Algorithms (2nd Edition). Comments that reference only a chapter
are referring to this work.

License

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

For more information, please contact Signature Group, LLC at this address: sales@signatgroup.com
*/
using System;


namespace CoordinateSharp
{
    /// <summary>
    /// Universal Transverse Mercator (UTM) coordinate system. Uses the WGS 84 Datum by default.
    /// </summary>
    [Serializable]
    public partial class UniversalTransverseMercator 
    {
        private Coordinate coordinate;

        internal double equatorial_radius;
        internal double inverse_flattening;
        private string latZone;
        private int longZone;

        private double easting;
        private double northing;

        private bool withinCoordinateSystemBounds = true;

        /// <summary>
        /// UTM Zone Letter
        /// </summary>
        public string LatZone
        {
            get { return latZone; }
        }
        /// <summary>
        /// UTM Zone Number
        /// </summary>
        public int LongZone
        {
            get { return longZone; }
        }
        /// <summary>
        /// UTM Easting
        /// </summary>
        public double Easting
        {
            get { return easting; }
        }
        /// <summary>
        /// UTM Northing
        /// </summary>
        public double Northing
        {
            get { return northing; }
        }

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
        /// Determine if the UTM conversion within the coordinate system's accurate boundaries after conversion from Lat/Long.
        /// </summary>
        public bool WithinCoordinateSystemBounds
        {
            get { return withinCoordinateSystemBounds; }
        }     
    }
}
