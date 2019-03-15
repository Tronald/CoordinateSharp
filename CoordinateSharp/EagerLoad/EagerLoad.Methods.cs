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

namespace CoordinateSharp
{
    public partial class EagerLoad
    {
        /// <summary>
        /// Create an EagerLoad object
        /// </summary>
        public EagerLoad()
        {
            Celestial = true;
            UTM_MGRS = true;
            Cartesian = true;
            ECEF = true;
        }

        /// <summary>
        /// Create an EagerLoad object with all options on or off
        /// </summary>
        /// <param name="isOn">Turns EagerLoad on or off</param>
        public EagerLoad(bool isOn)
        {
            Celestial = isOn;
            UTM_MGRS = isOn;
            Cartesian = isOn;
            ECEF = isOn;
        }

        /// <summary>
        /// Create an EagerLoad object with only the specified flag options turned on.
        /// </summary>
        /// <param name="et">EagerLoadType</param>
        public EagerLoad(EagerLoadType et)
        {
            Cartesian = false;
            Celestial = false;
            UTM_MGRS = false;
            ECEF = false;

            if (et.HasFlag(EagerLoadType.Cartesian))
            {
                Cartesian = true;
            }
            if (et.HasFlag(EagerLoadType.Celestial))
            {
                Celestial = true;
            }
            if (et.HasFlag(EagerLoadType.UTM_MGRS))
            {
                UTM_MGRS = true;
            }
            if (et.HasFlag(EagerLoadType.ECEF))
            {
                ECEF = true;
            }
        }

        /// <summary>
        /// Creates an EagerLoad object. Only the specified flags will be set to EagerLoad.
        /// </summary>
        /// <param name="et">EagerLoadType</param>
        /// <returns>EagerLoad</returns>
        public static EagerLoad Create(EagerLoadType et)
        {
            EagerLoad el = new EagerLoad(et);
            return el;
        }      
    }
   
}
