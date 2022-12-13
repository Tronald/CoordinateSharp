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
using System.Collections.Generic;

namespace CoordinateSharp.Magnetic
{
    internal class WMM2020COF
    {
        public static List<CoefficientModel> Data
        {
            get
            {
                List<CoefficientModel> models = new List<CoefficientModel>();

                //Convert COF to models.
                models.Add(new CoefficientModel(1, 0, -29404.5, 0.0, 6.7, 0.0));
                models.Add(new CoefficientModel(1, 1, -1450.7, 4652.9, 7.7, -25.1));
                models.Add(new CoefficientModel(2, 0, -2500.0, 0.0, -11.5, 0.0));
                models.Add(new CoefficientModel(2, 1, 2982.0, -2991.6, -7.1, -30.2));
                models.Add(new CoefficientModel(2, 2, 1676.8, -734.8, -2.2, -23.9));
                models.Add(new CoefficientModel(3, 0, 1363.9, 0.0, 2.8, 0.0));
                models.Add(new CoefficientModel(3, 1, -2381.0, -82.2, -6.2, 5.7));
                models.Add(new CoefficientModel(3, 2, 1236.2, 241.8, 3.4, -1.0));
                models.Add(new CoefficientModel(3, 3, 525.7, -542.9, -12.2, 1.1));
                models.Add(new CoefficientModel(4, 0, 903.1, 0.0, -1.1, 0.0));
                models.Add(new CoefficientModel(4, 1, 809.4, 282.0, -1.6, 0.2));
                models.Add(new CoefficientModel(4, 2, 86.2, -158.4, -6.0, 6.9));
                models.Add(new CoefficientModel(4, 3, -309.4, 199.8, 5.4, 3.7));
                models.Add(new CoefficientModel(4, 4, 47.9, -350.1, -5.5, -5.6));
                models.Add(new CoefficientModel(5, 0, -234.4, 0.0, -0.3, 0.0));
                models.Add(new CoefficientModel(5, 1, 363.1, 47.7, 0.6, 0.1));
                models.Add(new CoefficientModel(5, 2, 187.8, 208.4, -0.7, 2.5));
                models.Add(new CoefficientModel(5, 3, -140.7, -121.3, 0.1, -0.9));
                models.Add(new CoefficientModel(5, 4, -151.2, 32.2, 1.2, 3.0));
                models.Add(new CoefficientModel(5, 5, 13.7, 99.1, 1.0, 0.5));
                models.Add(new CoefficientModel(6, 0, 65.9, 0.0, -0.6, 0.0));
                models.Add(new CoefficientModel(6, 1, 65.6, -19.1, -0.4, 0.1));
                models.Add(new CoefficientModel(6, 2, 73.0, 25.0, 0.5, -1.8));
                models.Add(new CoefficientModel(6, 3, -121.5, 52.7, 1.4, -1.4));
                models.Add(new CoefficientModel(6, 4, -36.2, -64.4, -1.4, 0.9));
                models.Add(new CoefficientModel(6, 5, 13.5, 9.0, -0.0, 0.1));
                models.Add(new CoefficientModel(6, 6, -64.7, 68.1, 0.8, 1.0));
                models.Add(new CoefficientModel(7, 0, 80.6, 0.0, -0.1, 0.0));
                models.Add(new CoefficientModel(7, 1, -76.8, -51.4, -0.3, 0.5));
                models.Add(new CoefficientModel(7, 2, -8.3, -16.8, -0.1, 0.6));
                models.Add(new CoefficientModel(7, 3, 56.5, 2.3, 0.7, -0.7));
                models.Add(new CoefficientModel(7, 4, 15.8, 23.5, 0.2, -0.2));
                models.Add(new CoefficientModel(7, 5, 6.4, -2.2, -0.5, -1.2));
                models.Add(new CoefficientModel(7, 6, -7.2, -27.2, -0.8, 0.2));
                models.Add(new CoefficientModel(7, 7, 9.8, -1.9, 1.0, 0.3));
                models.Add(new CoefficientModel(8, 0, 23.6, 0.0, -0.1, 0.0));
                models.Add(new CoefficientModel(8, 1, 9.8, 8.4, 0.1, -0.3));
                models.Add(new CoefficientModel(8, 2, -17.5, -15.3, -0.1, 0.7));
                models.Add(new CoefficientModel(8, 3, -0.4, 12.8, 0.5, -0.2));
                models.Add(new CoefficientModel(8, 4, -21.1, -11.8, -0.1, 0.5));
                models.Add(new CoefficientModel(8, 5, 15.3, 14.9, 0.4, -0.3));
                models.Add(new CoefficientModel(8, 6, 13.7, 3.6, 0.5, -0.5));
                models.Add(new CoefficientModel(8, 7, -16.5, -6.9, 0.0, 0.4));
                models.Add(new CoefficientModel(8, 8, -0.3, 2.8, 0.4, 0.1));
                models.Add(new CoefficientModel(9, 0, 5.0, 0.0, -0.1, 0.0));
                models.Add(new CoefficientModel(9, 1, 8.2, -23.3, -0.2, -0.3));
                models.Add(new CoefficientModel(9, 2, 2.9, 11.1, -0.0, 0.2));
                models.Add(new CoefficientModel(9, 3, -1.4, 9.8, 0.4, -0.4));
                models.Add(new CoefficientModel(9, 4, -1.1, -5.1, -0.3, 0.4));
                models.Add(new CoefficientModel(9, 5, -13.3, -6.2, -0.0, 0.1));
                models.Add(new CoefficientModel(9, 6, 1.1, 7.8, 0.3, -0.0));
                models.Add(new CoefficientModel(9, 7, 8.9, 0.4, -0.0, -0.2));
                models.Add(new CoefficientModel(9, 8, -9.3, -1.5, -0.0, 0.5));
                models.Add(new CoefficientModel(9, 9, -11.9, 9.7, -0.4, 0.2));
                models.Add(new CoefficientModel(10, 0, -1.9, 0.0, 0.0, 0.0));
                models.Add(new CoefficientModel(10, 1, -6.2, 3.4, -0.0, -0.0));
                models.Add(new CoefficientModel(10, 2, -0.1, -0.2, -0.0, 0.1));
                models.Add(new CoefficientModel(10, 3, 1.7, 3.5, 0.2, -0.3));
                models.Add(new CoefficientModel(10, 4, -0.9, 4.8, -0.1, 0.1));
                models.Add(new CoefficientModel(10, 5, 0.6, -8.6, -0.2, -0.2));
                models.Add(new CoefficientModel(10, 6, -0.9, -0.1, -0.0, 0.1));
                models.Add(new CoefficientModel(10, 7, 1.9, -4.2, -0.1, -0.0));
                models.Add(new CoefficientModel(10, 8, 1.4, -3.4, -0.2, -0.1));
                models.Add(new CoefficientModel(10, 9, -2.4, -0.1, -0.1, 0.2));
                models.Add(new CoefficientModel(10, 10, -3.9, -8.8, -0.0, -0.0));
                models.Add(new CoefficientModel(11, 0, 3.0, 0.0, -0.0, 0.0));
                models.Add(new CoefficientModel(11, 1, -1.4, -0.0, -0.1, -0.0));
                models.Add(new CoefficientModel(11, 2, -2.5, 2.6, -0.0, 0.1));
                models.Add(new CoefficientModel(11, 3, 2.4, -0.5, 0.0, 0.0));
                models.Add(new CoefficientModel(11, 4, -0.9, -0.4, -0.0, 0.2));
                models.Add(new CoefficientModel(11, 5, 0.3, 0.6, -0.1, -0.0));
                models.Add(new CoefficientModel(11, 6, -0.7, -0.2, 0.0, 0.0));
                models.Add(new CoefficientModel(11, 7, -0.1, -1.7, -0.0, 0.1));
                models.Add(new CoefficientModel(11, 8, 1.4, -1.6, -0.1, -0.0));
                models.Add(new CoefficientModel(11, 9, -0.6, -3.0, -0.1, -0.1));
                models.Add(new CoefficientModel(11, 10, 0.2, -2.0, -0.1, 0.0));
                models.Add(new CoefficientModel(11, 11, 3.1, -2.6, -0.1, -0.0));
                models.Add(new CoefficientModel(12, 0, -2.0, 0.0, 0.0, 0.0));
                models.Add(new CoefficientModel(12, 1, -0.1, -1.2, -0.0, -0.0));
                models.Add(new CoefficientModel(12, 2, 0.5, 0.5, -0.0, 0.0));
                models.Add(new CoefficientModel(12, 3, 1.3, 1.3, 0.0, -0.1));
                models.Add(new CoefficientModel(12, 4, -1.2, -1.8, -0.0, 0.1));
                models.Add(new CoefficientModel(12, 5, 0.7, 0.1, -0.0, -0.0));
                models.Add(new CoefficientModel(12, 6, 0.3, 0.7, 0.0, 0.0));
                models.Add(new CoefficientModel(12, 7, 0.5, -0.1, -0.0, -0.0));
                models.Add(new CoefficientModel(12, 8, -0.2, 0.6, 0.0, 0.1));
                models.Add(new CoefficientModel(12, 9, -0.5, 0.2, -0.0, -0.0));
                models.Add(new CoefficientModel(12, 10, 0.1, -0.9, -0.0, -0.0));
                models.Add(new CoefficientModel(12, 11, -1.1, -0.0, -0.0, 0.0));
                models.Add(new CoefficientModel(12, 12, -0.3, 0.5, -0.1, -0.1));

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
