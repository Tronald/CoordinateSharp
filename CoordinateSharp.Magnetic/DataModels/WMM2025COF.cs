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
using System.Collections.Generic;

namespace CoordinateSharp.Magnetic
{
    internal class WMM2025COF
    {
        public static List<CoefficientModel> Data
        {
            get
            {
                List<CoefficientModel> models = new List<CoefficientModel>();

                //Convert COF to models.
                //NOTES TO QUICKLY CREATE FROM COF FILE
                //REGEX REPLACE (In Notepad++)
                //FIND: ^\s*(-?\d+)\s+(-?\d+)\s+(-?\d+\.?\d*)\s+(-?\d+\.?\d*)\s+(-?\d+\.?\d*)\s+(-?\d+\.?\d*)\s*$
                //REPALCE: models.Add\(new CoefficientModel\(\1, \2, \3, \4, \5, \6\)\);\n
                models.Add(new CoefficientModel(1, 0, -29351.8, 0.0, 12.0, 0.0));
                models.Add(new CoefficientModel(1, 1, -1410.8, 4545.4, 9.7, -21.5));
                models.Add(new CoefficientModel(2, 0, -2556.6, 0.0, -11.6, 0.0));
                models.Add(new CoefficientModel(2, 1, 2951.1, -3133.6, -5.2, -27.7));
                models.Add(new CoefficientModel(2, 2, 1649.3, -815.1, -8.0, -12.1));
                models.Add(new CoefficientModel(3, 0, 1361.0, 0.0, -1.3, 0.0));
                models.Add(new CoefficientModel(3, 1, -2404.1, -56.6, -4.2, 4.0));
                models.Add(new CoefficientModel(3, 2, 1243.8, 237.5, 0.4, -0.3));
                models.Add(new CoefficientModel(3, 3, 453.6, -549.5, -15.6, -4.1));
                models.Add(new CoefficientModel(4, 0, 895.0, 0.0, -1.6, 0.0));
                models.Add(new CoefficientModel(4, 1, 799.5, 278.6, -2.4, -1.1));
                models.Add(new CoefficientModel(4, 2, 55.7, -133.9, -6.0, 4.1));
                models.Add(new CoefficientModel(4, 3, -281.1, 212.0, 5.6, 1.6));
                models.Add(new CoefficientModel(4, 4, 12.1, -375.6, -7.0, -4.4));
                models.Add(new CoefficientModel(5, 0, -233.2, 0.0, 0.6, 0.0));
                models.Add(new CoefficientModel(5, 1, 368.9, 45.4, 1.4, -0.5));
                models.Add(new CoefficientModel(5, 2, 187.2, 220.2, 0.0, 2.2));
                models.Add(new CoefficientModel(5, 3, -138.7, -122.9, 0.6, 0.4));
                models.Add(new CoefficientModel(5, 4, -142.0, 43.0, 2.2, 1.7));
                models.Add(new CoefficientModel(5, 5, 20.9, 106.1, 0.9, 1.9));
                models.Add(new CoefficientModel(6, 0, 64.4, 0.0, -0.2, 0.0));
                models.Add(new CoefficientModel(6, 1, 63.8, -18.4, -0.4, 0.3));
                models.Add(new CoefficientModel(6, 2, 76.9, 16.8, 0.9, -1.6));
                models.Add(new CoefficientModel(6, 3, -115.7, 48.8, 1.2, -0.4));
                models.Add(new CoefficientModel(6, 4, -40.9, -59.8, -0.9, 0.9));
                models.Add(new CoefficientModel(6, 5, 14.9, 10.9, 0.3, 0.7));
                models.Add(new CoefficientModel(6, 6, -60.7, 72.7, 0.9, 0.9));
                models.Add(new CoefficientModel(7, 0, 79.5, 0.0, -0.0, 0.0));
                models.Add(new CoefficientModel(7, 1, -77.0, -48.9, -0.1, 0.6));
                models.Add(new CoefficientModel(7, 2, -8.8, -14.4, -0.1, 0.5));
                models.Add(new CoefficientModel(7, 3, 59.3, -1.0, 0.5, -0.8));
                models.Add(new CoefficientModel(7, 4, 15.8, 23.4, -0.1, 0.0));
                models.Add(new CoefficientModel(7, 5, 2.5, -7.4, -0.8, -1.0));
                models.Add(new CoefficientModel(7, 6, -11.1, -25.1, -0.8, 0.6));
                models.Add(new CoefficientModel(7, 7, 14.2, -2.3, 0.8, -0.2));
                models.Add(new CoefficientModel(8, 0, 23.2, 0.0, -0.1, 0.0));
                models.Add(new CoefficientModel(8, 1, 10.8, 7.1, 0.2, -0.2));
                models.Add(new CoefficientModel(8, 2, -17.5, -12.6, 0.0, 0.5));
                models.Add(new CoefficientModel(8, 3, 2.0, 11.4, 0.5, -0.4));
                models.Add(new CoefficientModel(8, 4, -21.7, -9.7, -0.1, 0.4));
                models.Add(new CoefficientModel(8, 5, 16.9, 12.7, 0.3, -0.5));
                models.Add(new CoefficientModel(8, 6, 15.0, 0.7, 0.2, -0.6));
                models.Add(new CoefficientModel(8, 7, -16.8, -5.2, -0.0, 0.3));
                models.Add(new CoefficientModel(8, 8, 0.9, 3.9, 0.2, 0.2));
                models.Add(new CoefficientModel(9, 0, 4.6, 0.0, -0.0, 0.0));
                models.Add(new CoefficientModel(9, 1, 7.8, -24.8, -0.1, -0.3));
                models.Add(new CoefficientModel(9, 2, 3.0, 12.2, 0.1, 0.3));
                models.Add(new CoefficientModel(9, 3, -0.2, 8.3, 0.3, -0.3));
                models.Add(new CoefficientModel(9, 4, -2.5, -3.3, -0.3, 0.3));
                models.Add(new CoefficientModel(9, 5, -13.1, -5.2, 0.0, 0.2));
                models.Add(new CoefficientModel(9, 6, 2.4, 7.2, 0.3, -0.1));
                models.Add(new CoefficientModel(9, 7, 8.6, -0.6, -0.1, -0.2));
                models.Add(new CoefficientModel(9, 8, -8.7, 0.8, 0.1, 0.4));
                models.Add(new CoefficientModel(9, 9, -12.9, 10.0, -0.1, 0.1));
                models.Add(new CoefficientModel(10, 0, -1.3, 0.0, 0.1, 0.0));
                models.Add(new CoefficientModel(10, 1, -6.4, 3.3, 0.0, 0.0));
                models.Add(new CoefficientModel(10, 2, 0.2, 0.0, 0.1, -0.0));
                models.Add(new CoefficientModel(10, 3, 2.0, 2.4, 0.1, -0.2));
                models.Add(new CoefficientModel(10, 4, -1.0, 5.3, -0.0, 0.1));
                models.Add(new CoefficientModel(10, 5, -0.6, -9.1, -0.3, -0.1));
                models.Add(new CoefficientModel(10, 6, -0.9, 0.4, 0.0, 0.1));
                models.Add(new CoefficientModel(10, 7, 1.5, -4.2, -0.1, 0.0));
                models.Add(new CoefficientModel(10, 8, 0.9, -3.8, -0.1, -0.1));
                models.Add(new CoefficientModel(10, 9, -2.7, 0.9, -0.0, 0.2));
                models.Add(new CoefficientModel(10, 10, -3.9, -9.1, -0.0, -0.0));
                models.Add(new CoefficientModel(11, 0, 2.9, 0.0, 0.0, 0.0));
                models.Add(new CoefficientModel(11, 1, -1.5, 0.0, -0.0, -0.0));
                models.Add(new CoefficientModel(11, 2, -2.5, 2.9, 0.0, 0.1));
                models.Add(new CoefficientModel(11, 3, 2.4, -0.6, 0.0, -0.0));
                models.Add(new CoefficientModel(11, 4, -0.6, 0.2, 0.0, 0.1));
                models.Add(new CoefficientModel(11, 5, -0.1, 0.5, -0.1, -0.0));
                models.Add(new CoefficientModel(11, 6, -0.6, -0.3, 0.0, -0.0));
                models.Add(new CoefficientModel(11, 7, -0.1, -1.2, -0.0, 0.1));
                models.Add(new CoefficientModel(11, 8, 1.1, -1.7, -0.1, -0.0));
                models.Add(new CoefficientModel(11, 9, -1.0, -2.9, -0.1, 0.0));
                models.Add(new CoefficientModel(11, 10, -0.2, -1.8, -0.1, 0.0));
                models.Add(new CoefficientModel(11, 11, 2.6, -2.3, -0.1, 0.0));
                models.Add(new CoefficientModel(12, 0, -2.0, 0.0, 0.0, 0.0));
                models.Add(new CoefficientModel(12, 1, -0.2, -1.3, 0.0, -0.0));
                models.Add(new CoefficientModel(12, 2, 0.3, 0.7, -0.0, 0.0));
                models.Add(new CoefficientModel(12, 3, 1.2, 1.0, -0.0, -0.1));
                models.Add(new CoefficientModel(12, 4, -1.3, -1.4, -0.0, 0.1));
                models.Add(new CoefficientModel(12, 5, 0.6, -0.0, -0.0, -0.0));
                models.Add(new CoefficientModel(12, 6, 0.6, 0.6, 0.1, -0.0));
                models.Add(new CoefficientModel(12, 7, 0.5, -0.1, -0.0, -0.0));
                models.Add(new CoefficientModel(12, 8, -0.1, 0.8, 0.0, 0.0));
                models.Add(new CoefficientModel(12, 9, -0.4, 0.1, 0.0, -0.0));
                models.Add(new CoefficientModel(12, 10, -0.2, -1.0, -0.1, -0.0));
                models.Add(new CoefficientModel(12, 11, -1.3, 0.1, -0.0, 0.0));
                models.Add(new CoefficientModel(12, 12, -0.7, 0.2, -0.1, -0.1));

                models.Add(new CoefficientModel(13, 0, null, null, null, null));
                models.Add(new CoefficientModel(13, 1, null, null, null, null));
                models.Add(new CoefficientModel(13, 2, null, null, null, null));
                models.Add(new CoefficientModel(13, 3, null, null, null, null));
                models.Add(new CoefficientModel(13, 4, null, null, null, null));
                models.Add(new CoefficientModel(13, 5, null, null, null, null));
                models.Add(new CoefficientModel(13, 6, null, null, null, null));
                models.Add(new CoefficientModel(13, 7, null, null, null, null));
                models.Add(new CoefficientModel(13, 8, null, null, null, null));
                models.Add(new CoefficientModel(13, 9, null, null, null, null));
                models.Add(new CoefficientModel(13, 10, null, null, null, null));
                models.Add(new CoefficientModel(13, 11, null, null, null, null));
                models.Add(new CoefficientModel(13, 12, null, null, null, null));

                return models;
            }
        }
    }
}
