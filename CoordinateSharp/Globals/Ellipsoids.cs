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
    /// Handles earth ellipsoid values
    /// </summary>
    public struct Earth_Ellipsoid
    {      
        /// <summary>
        /// Earth equatorial radius (semi-major axis)
        /// </summary>
        public double Equatorial_Radius { get; private set; }

        /// <summary>
        /// Earth inverse of flattening
        /// </summary>
        public double Inverse_Flattening { get; private set; }

        /// <summary>
        /// Earth ellipsoid specification
        /// </summary>
        public Earth_Ellipsoid_Spec Spec { get; private set; }

        /// <summary>
        /// Returns a populated Earth_Ellipsoid based on the specified ellipsoidal spec.
        /// </summary>
        /// <param name="spec">Earth ellipsoid specification</param>
        /// <returns>Earth_Ellipsoid</returns>
        public static Earth_Ellipsoid Get_Ellipsoid(Earth_Ellipsoid_Spec spec)
        {
            Earth_Ellipsoid ee = new Earth_Ellipsoid();
            ee.Spec = spec;

            if (spec == Earth_Ellipsoid_Spec.WGS84_1984) { ee.Equatorial_Radius = 6378137; ee.Inverse_Flattening = 298.257223563; }
            else if (spec == Earth_Ellipsoid_Spec.Maupertuis_1738) { ee.Equatorial_Radius = 6397300; ee.Inverse_Flattening = 191; }
            else if (spec == Earth_Ellipsoid_Spec.Plessis_1817) { ee.Equatorial_Radius = 6376523; ee.Inverse_Flattening = 308.64; }
            else if (spec == Earth_Ellipsoid_Spec.Everest_1830) { ee.Equatorial_Radius = 6377299.365; ee.Inverse_Flattening = 300.80172554; }
            else if (spec == Earth_Ellipsoid_Spec.Everest_1830_Modified_1967) { ee.Equatorial_Radius = 6377304.063; ee.Inverse_Flattening = 300.8017; }
            else if (spec == Earth_Ellipsoid_Spec.Everest_1830_1967_Definition) { ee.Equatorial_Radius = 6377298.556; ee.Inverse_Flattening = 300.8017; }
            else if (spec == Earth_Ellipsoid_Spec.Airy_1830) { ee.Equatorial_Radius = 6377563.396; ee.Inverse_Flattening = 299.3249646; }
            else if (spec == Earth_Ellipsoid_Spec.Bessel_1841) { ee.Equatorial_Radius = 6377397.155; ee.Inverse_Flattening = 299.1528128; }
            else if (spec == Earth_Ellipsoid_Spec.Clarke_1866) { ee.Equatorial_Radius = 6378206.4; ee.Inverse_Flattening = 294.9786982; }
            else if (spec == Earth_Ellipsoid_Spec.Clarke_1878) { ee.Equatorial_Radius = 6378190; ee.Inverse_Flattening = 293.465998; }
            else if (spec == Earth_Ellipsoid_Spec.Clarke_1880) { ee.Equatorial_Radius = 6378249.145; ee.Inverse_Flattening = 293.465; }
            else if (spec == Earth_Ellipsoid_Spec.Helmert_1906) { ee.Equatorial_Radius = 6378200; ee.Inverse_Flattening = 298.3; }
            else if (spec == Earth_Ellipsoid_Spec.Hayford_1910) { ee.Equatorial_Radius = 6378388; ee.Inverse_Flattening = 297; }
            else if (spec == Earth_Ellipsoid_Spec.International_1924) { ee.Equatorial_Radius = 6378388; ee.Inverse_Flattening = 297; }
            else if (spec == Earth_Ellipsoid_Spec.Krassovsky_1940) { ee.Equatorial_Radius = 6378245; ee.Inverse_Flattening = 298.3; }
            else if (spec == Earth_Ellipsoid_Spec.WGS66_1966) { ee.Equatorial_Radius = 6378145; ee.Inverse_Flattening = 298.25; }
            else if (spec == Earth_Ellipsoid_Spec.Australian_National_1966) { ee.Equatorial_Radius = 6378160; ee.Inverse_Flattening = 298.25; }
            else if (spec == Earth_Ellipsoid_Spec.New_International_1967) { ee.Equatorial_Radius = 6378157.5; ee.Inverse_Flattening = 298.24961539; }
            else if (spec == Earth_Ellipsoid_Spec.GRS67_1967) { ee.Equatorial_Radius = 6378160; ee.Inverse_Flattening = 298.247167427; }
            else if (spec == Earth_Ellipsoid_Spec.South_American_1969) { ee.Equatorial_Radius = 6378160; ee.Inverse_Flattening = 298.25; }
            else if (spec == Earth_Ellipsoid_Spec.WGS72_1972) { ee.Equatorial_Radius = 6378135; ee.Inverse_Flattening = 298.26; }
            else if (spec == Earth_Ellipsoid_Spec.GRS80_1979) { ee.Equatorial_Radius = 6378137; ee.Inverse_Flattening = 298.257222101; }           
            else if (spec == Earth_Ellipsoid_Spec.IERS_1989) { ee.Equatorial_Radius = 6378136; ee.Inverse_Flattening = 298.257; }
            else if (spec == Earth_Ellipsoid_Spec.IERS_2003) { ee.Equatorial_Radius = 6378136.6; ee.Inverse_Flattening = 298.25642; }


            return ee;
        }
    }

    /// <summary>
    /// Earth ellipsoid definitions
    /// </summary>
    public enum Earth_Ellipsoid_Spec
    {
        ///<summary>
        ///Maupertuis_1738
        ///</summary>
        Maupertuis_1738,

        ///<summary>
        ///Plessis 1817
        ///</summary>
        Plessis_1817,

        ///<summary>
        ///Everest 1830 
        ///</summary>
        Everest_1830,

        ///<summary>
        ///Everest 1830 Modified1967
        ///</summary>
        Everest_1830_Modified_1967,

        ///<summary>
        ///Everest 1830 1967 Definition 
        ///</summary>
        Everest_1830_1967_Definition,
        ///<summary>
        ///Airy 1830 
        ///</summary>
        Airy_1830,
        ///<summary>
        ///Bessel 1841 
        ///</summary>
        Bessel_1841,

        ///<summary>
        ///Clarke 1866 
        ///</summary>
        Clarke_1866,

        ///<summary>
        ///Clarke 1878 
        ///</summary>
        Clarke_1878,

        ///<summary>
        ///Clarke 1880 
        ///</summary>
        Clarke_1880,

        ///<summary>
        ///Helmert 1906 
        ///</summary>
        Helmert_1906,

        ///<summary>
        ///Hayford 1910 
        ///</summary>
        Hayford_1910,

        ///<summary>
        ///International 1924 
        ///</summary>
        International_1924,

        ///<summary>
        ///Krassovsky 194 
        ///</summary>
        Krassovsky_1940,

        ///<summary>
        ///WGS-66 1966
        ///</summary>
        WGS66_1966,

        ///<summary>
        ///Australian National 1966 
        ///</summary>
        Australian_National_1966,

        ///<summary>
        ///New International 1967,        
        ///</summary>
        New_International_1967,

        ///<summary>
        ///GRS-67 1967        
        ///</summary>
        GRS67_1967,

        ///<summary>
        ///South American 1969 
        ///</summary>
        South_American_1969,

        ///<summary>
        ///WGS-72 1972 
        ///</summary>
        WGS72_1972,

        ///<summary>
        ///GRS-80 1979
        ///</summary>
        GRS80_1979,

        ///<summary>
        ///WGS-84 1984 
        ///</summary>
        WGS84_1984,

        ///<summary>
        ///IERS 1989
        ///</summary>
        IERS_1989,

        ///<summary>
        ///IERS2003 
        ///</summary>
        IERS_2003

    }

}
