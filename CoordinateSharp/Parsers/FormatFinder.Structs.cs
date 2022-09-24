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
    internal struct DirectionFinder
    {
        public DirectionFinder(string coordinatePartString)
        {
            rad = 1;
            partString = string.Empty;
            coordinateType = null;
            position = null;
            success = false;
            
            if (coordinatePartString.ToLower().Contains("n"))
            {
                partString = coordinatePartString;
                position = CoordinatesPosition.N;
                coordinateType = CoordinateSharp.CoordinateType.Lat;
                success = true;
            }
            else if (coordinatePartString.ToLower().Contains("s"))
            {
                partString = coordinatePartString;
                position = CoordinatesPosition.S;
                coordinateType = CoordinateSharp.CoordinateType.Lat;
                rad = -1;
                success = true;
            }
            else if (coordinatePartString.ToLower().Contains("e"))
            {
                partString = coordinatePartString;
                position = CoordinatesPosition.E;
                coordinateType = CoordinateSharp.CoordinateType.Long;
                success = true;
            }
            else if (coordinatePartString.ToLower().Contains("w"))
            {
                partString = coordinatePartString;
                position = CoordinatesPosition.W;
                coordinateType = CoordinateSharp.CoordinateType.Long;
                rad = -1;
                success = true;
            }
        }

        private Nullable<CoordinateType> coordinateType;
        private Nullable<CoordinatesPosition> position;
        private string partString;
        private int rad;
        private bool success;

        public Nullable<CoordinateType> CoordinateType { get { return coordinateType; } }
        public Nullable<CoordinatesPosition> Position { get { return position; } }
        public string PartString { get { return partString; } }
        public int Rad { get { return rad; } }
        public int RadZero
        {
            get
            {
                if (rad == 1) { return 0; }
                else { return 1; }
            }
        }
        public bool Success { get { return success; } }
    }
   
}
