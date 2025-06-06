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
using System.Collections.Generic;
using System.Linq;

namespace CoordinateSharp.Eclipse
{
    //Data taken from https://eclipse.gsfc.nasa.gov/
    internal partial class SolarData
    {
        //Data compiled from https://eclipse.gsfc.nasa.gov/JSEX/JSEX-AS.html

        private static readonly int startCentury = 0;
        private static readonly int endCentury = 29;
        public static double[] SolarDateData(DateTime d)
        {            
            double cent = Math.Floor(d.Year * .01) * 100; //Gets turn of century year.
            int index = GetIndex(cent); //Gets index for calling data list.

            if (index == -1) { return new double[] { };} //RETURN EMPTY ARRAY IF OUTSIDE DB RANGE

            //Determine data to send if year is near beginning or end of database
            int halfCent = d.Year - (int)cent;

            double[] century = SolarDateData_100Year(d);
           
            if (index == startCentury || index == endCentury)
            {
                if(index == 0)
                {
                    if (halfCent <= 50) 
                    {
                        return century; 
                    }
                    else 
                    {
                        return century.Concat(SolarDateData_100Year(d.AddYears(100))).ToArray();
                    }
                }
                else
                {
                    if (halfCent <= 50) 
                    { 
                        return century.Concat(SolarDateData_100Year(d.AddYears(-100))).ToArray();
                    }
                    else 
                    {
                        return century;
                    }
                }
            }
            else
            {
                if (halfCent <= 50) 
                {
                    //DateTime Integrity
                    if (index == 1) { return century.Concat(SolarDateData_100Year(d.AddYears(-99))).ToArray(); }
                    else
                    {
                        return century.Concat(SolarDateData_100Year(d.AddYears(-100))).ToArray();
                    }
                }
                else 
                { 
                    return century.Concat(SolarDateData_100Year(d.AddYears(100))).ToArray();
                }
            }
      
        }
        public static double[] SolarDateData_100Year(DateTime d)
        {           
            double cent = Math.Floor(d.Year * .01) * 100; //Gets turn of century year.
            int index = GetIndex(cent); //Gets index for calling data list.

            //Return proper 100 year table.
            switch (index)
            {
                case 0: return SE0001;
                case 1: return SE0101;
                case 2: return SE0201;
                case 3: return SE0301;
                case 4: return SE0401;
                case 5: return SE0501;
                case 6: return SE0601;
                case 7: return SE0701;
                case 8: return SE0801;
                case 9: return SE0901;
                case 10: return SE1001;
                case 11: return SE1101;
                case 12: return SE1201;
                case 13: return SE1301;
                case 14: return SE1401;
                case 15: return SE1501;
                case 16: return SE1601;
                case 17: return SE1701;
                case 18: return SE1801;
                case 19: return SE1901;
                case 20: return SE2001;
                case 21: return SE2101;
                case 22: return SE2201;
                case 23: return SE2301;
                case 24: return SE2401;
                case 25: return SE2501;
                case 26: return SE2601;
                case 27: return SE2701;
                case 28: return SE2801;
                case 29: return SE2901;
            }

            return new double[] { };

        }
       
        private static int GetIndex(double cent)
        {
            int dex = 0;
            int c = Convert.ToInt32(cent * .01);
            //START CENTURY 16
            //END CENTRURY 26
            //AJDUST AS DATABASE GROWS. i = first available century, end greater than last
            for(int i = startCentury; i<=endCentury;i++)
            {
                if(i == c) { return dex; }
                dex++;
            }
            return -1;
        }
    }
}
