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
    /// World Geographic Reference System (GEOREF)
    /// </summary>
    [Serializable]
    public partial class GEOREF
    {
        static string digits = "0123456789";
        static string lngTile = "ABCDEFGHJKLMNPQRSTUVWXYZZ"; // Repeat the last Z for 180 degree check efficiency
        static string latTile = "ABCDEFGHJKLMM"; // Repeat the last M for 90 degree check efficiency
        static string degrees = "ABCDEFGHJKLMNPQ";
        static int tile = 15;
        static double lngorig = -180;
        static double latorig = -90;
        static double based = 10;
        static int maxprec = 11;     
        private string quad_15;
        private string quad_1;
        private string easting; //stored as string due to leading zero with dynamic precision requirements.
        private string northing; //stored as string due to leading zero with dynamic precision requirements. 

        /// <summary>
        /// 15 Degree Quadrangle
        /// </summary>
        public string Quad_15
        {
            get { return quad_15; }
            internal set { quad_15 = value; }
        }
        /// <summary>
        /// 1 Degree Quadrangle
        /// </summary>
        public string Quad_1
        {
            get { return quad_1; }
            internal set { quad_1 = value; }
        }

        /// <summary>
        /// Easting
        /// </summary>
        public string Easting
        {
            get { return easting; }
            internal set { easting = value; }
        }

        /// <summary>
        /// Northing
        /// </summary>
        public string Northing
        {
            get { return northing; }
            internal set{northing = value;}
        }     
    }
}
