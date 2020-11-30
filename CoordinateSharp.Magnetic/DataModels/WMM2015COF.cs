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
using System.Collections.Generic;

namespace CoordinateSharp.Magnetic
{
    internal class WMM2015COF
    {
        public static List<CoefficientModel> Data
        {
            get
            {
                List<CoefficientModel> models = new List<CoefficientModel>();

                //Convert COF to models.
                models.Add(new CoefficientModel(1, 0, -29438.5, 0, 10.7, 0));
                models.Add(new CoefficientModel(1, 1, -1501.1, 4796.2, 17.9, -26.8));
                models.Add(new CoefficientModel(2, 0, -2445.3, 0, -8.6, 0));
                models.Add(new CoefficientModel(2, 1, 3012.5, -2845.6, -3.3, -27.1));
                models.Add(new CoefficientModel(2, 2, 1676.6, -642, 2.4, -13.3));
                models.Add(new CoefficientModel(3, 0, 1351.1, 0, 3.1, 0));
                models.Add(new CoefficientModel(3, 1, -2352.3, -115.3, -6.2, 8.4));
                models.Add(new CoefficientModel(3, 2, 1225.6, 245, -0.4, -0.4));
                models.Add(new CoefficientModel(3, 3, 581.9, -538.3, -10.4, 2.3));
                models.Add(new CoefficientModel(4, 0, 907.2, 0, -0.4, 0));
                models.Add(new CoefficientModel(4, 1, 813.7, 283.4, 0.8, -0.6));
                models.Add(new CoefficientModel(4, 2, 120.3, -188.6, -9.2, 5.3));
                models.Add(new CoefficientModel(4, 3, -335, 180.9, 4, 3));
                models.Add(new CoefficientModel(4, 4, 70.3, -329.5, -4.2, -5.3));
                models.Add(new CoefficientModel(5, 0, -232.6, 0, -0.2, 0));
                models.Add(new CoefficientModel(5, 1, 360.1, 47.4, 0.1, 0.4));
                models.Add(new CoefficientModel(5, 2, 192.4, 196.9, -1.4, 1.6));
                models.Add(new CoefficientModel(5, 3, -141, -119.4, 0, -1.1));
                models.Add(new CoefficientModel(5, 4, -157.4, 16.1, 1.3, 3.3));
                models.Add(new CoefficientModel(5, 5, 4.3, 100.1, 3.8, 0.1));
                models.Add(new CoefficientModel(6, 0, 69.5, 0, -0.5, 0));
                models.Add(new CoefficientModel(6, 1, 67.4, -20.7, -0.2, 0));
                models.Add(new CoefficientModel(6, 2, 72.8, 33.2, -0.6, -2.2));
                models.Add(new CoefficientModel(6, 3, -129.8, 58.8, 2.4, -0.7));
                models.Add(new CoefficientModel(6, 4, -29, -66.5, -1.1, 0.1));
                models.Add(new CoefficientModel(6, 5, 13.2, 7.3, 0.3, 1));
                models.Add(new CoefficientModel(6, 6, -70.9, 62.5, 1.5, 1.3));
                models.Add(new CoefficientModel(7, 0, 81.6, 0, 0.2, 0));
                models.Add(new CoefficientModel(7, 1, -76.1, -54.1, -0.2, 0.7));
                models.Add(new CoefficientModel(7, 2, -6.8, -19.4, -0.4, 0.5));
                models.Add(new CoefficientModel(7, 3, 51.9, 5.6, 1.3, -0.2));
                models.Add(new CoefficientModel(7, 4, 15, 24.4, 0.2, -0.1));
                models.Add(new CoefficientModel(7, 5, 9.3, 3.3, -0.4, -0.7));
                models.Add(new CoefficientModel(7, 6, -2.8, -27.5, -0.9, 0.1));
                models.Add(new CoefficientModel(7, 7, 6.7, -2.3, 0.3, 0.1));
                models.Add(new CoefficientModel(8, 0, 24, 0, 0, 0));
                models.Add(new CoefficientModel(8, 1, 8.6, 10.2, 0.1, -0.3));
                models.Add(new CoefficientModel(8, 2, -16.9, -18.1, -0.5, 0.3));
                models.Add(new CoefficientModel(8, 3, -3.2, 13.2, 0.5, 0.3));
                models.Add(new CoefficientModel(8, 4, -20.6, -14.6, -0.2, 0.6));
                models.Add(new CoefficientModel(8, 5, 13.3, 16.2, 0.4, -0.1));
                models.Add(new CoefficientModel(8, 6, 11.7, 5.7, 0.2, -0.2));
                models.Add(new CoefficientModel(8, 7, -16, -9.1, -0.4, 0.3));
                models.Add(new CoefficientModel(8, 8, -2, 2.2, 0.3, 0));
                models.Add(new CoefficientModel(9, 0, 5.4, 0, 0, 0));
                models.Add(new CoefficientModel(9, 1, 8.8, -21.6, -0.1, -0.2));
                models.Add(new CoefficientModel(9, 2, 3.1, 10.8, -0.1, -0.1));
                models.Add(new CoefficientModel(9, 3, -3.1, 11.7, 0.4, -0.2));
                models.Add(new CoefficientModel(9, 4, 0.6, -6.8, -0.5, 0.1));
                models.Add(new CoefficientModel(9, 5, -13.3, -6.9, -0.2, 0.1));
                models.Add(new CoefficientModel(9, 6, -0.1, 7.8, 0.1, 0));
                models.Add(new CoefficientModel(9, 7, 8.7, 1, 0, -0.2));
                models.Add(new CoefficientModel(9, 8, -9.1, -3.9, -0.2, 0.4));
                models.Add(new CoefficientModel(9, 9, -10.5, 8.5, -0.1, 0.3));
                models.Add(new CoefficientModel(10, 0, -1.9, 0, 0, 0));
                models.Add(new CoefficientModel(10, 1, -6.5, 3.3, 0, 0.1));
                models.Add(new CoefficientModel(10, 2, 0.2, -0.3, -0.1, -0.1));
                models.Add(new CoefficientModel(10, 3, 0.6, 4.6, 0.3, 0));
                models.Add(new CoefficientModel(10, 4, -0.6, 4.4, -0.1, 0));
                models.Add(new CoefficientModel(10, 5, 1.7, -7.9, -0.1, -0.2));
                models.Add(new CoefficientModel(10, 6, -0.7, -0.6, -0.1, 0.1));
                models.Add(new CoefficientModel(10, 7, 2.1, -4.1, 0, -0.1));
                models.Add(new CoefficientModel(10, 8, 2.3, -2.8, -0.2, -0.2));
                models.Add(new CoefficientModel(10, 9, -1.8, -1.1, -0.1, 0.1));
                models.Add(new CoefficientModel(10, 10, -3.6, -8.7, -0.2, -0.1));
                models.Add(new CoefficientModel(11, 0, 3.1, 0, 0, 0));
                models.Add(new CoefficientModel(11, 1, -1.5, -0.1, 0, 0));
                models.Add(new CoefficientModel(11, 2, -2.3, 2.1, -0.1, 0.1));
                models.Add(new CoefficientModel(11, 3, 2.1, -0.7, 0.1, 0));
                models.Add(new CoefficientModel(11, 4, -0.9, -1.1, 0, 0.1));
                models.Add(new CoefficientModel(11, 5, 0.6, 0.7, 0, 0));
                models.Add(new CoefficientModel(11, 6, -0.7, -0.2, 0, 0));
                models.Add(new CoefficientModel(11, 7, 0.2, -2.1, 0, 0.1));
                models.Add(new CoefficientModel(11, 8, 1.7, -1.5, 0, 0));
                models.Add(new CoefficientModel(11, 9, -0.2, -2.5, 0, -0.1));
                models.Add(new CoefficientModel(11, 10, 0.4, -2, -0.1, 0));
                models.Add(new CoefficientModel(11, 11, 3.5, -2.3, -0.1, -0.1));
                models.Add(new CoefficientModel(12, 0, -2, 0, 0.1, 0));
                models.Add(new CoefficientModel(12, 1, -0.3, -1, 0, 0));
                models.Add(new CoefficientModel(12, 2, 0.4, 0.5, 0, 0));
                models.Add(new CoefficientModel(12, 3, 1.3, 1.8, 0.1, -0.1));
                models.Add(new CoefficientModel(12, 4, -0.9, -2.2, -0.1, 0));
                models.Add(new CoefficientModel(12, 5, 0.9, 0.3, 0, 0));
                models.Add(new CoefficientModel(12, 6, 0.1, 0.7, 0.1, 0));
                models.Add(new CoefficientModel(12, 7, 0.5, -0.1, 0, 0));
                models.Add(new CoefficientModel(12, 8, -0.4, 0.3, 0, 0));
                models.Add(new CoefficientModel(12, 9, -0.4, 0.2, 0, 0));
                models.Add(new CoefficientModel(12, 10, 0.2, -0.9, 0, 0));
                models.Add(new CoefficientModel(12, 11, -0.9, -0.2, 0, 0));
                models.Add(new CoefficientModel(12, 12, 0, 0.7, 0, 0));
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
