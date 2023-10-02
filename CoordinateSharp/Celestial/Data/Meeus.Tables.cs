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
    internal partial class MeeusTables
    {
        //Ch 27
        private static double[] Table27C_Arguements = new double[]
        {

                485,
                5.671621937280772,
                33.75704138135305,
                203,
                5.885773836500478,
                575.3384853150176,
                199,
                5.970422305222201,
                0.3523121628075754,
                182,
                0.4860741966804208,
                7771.377155246354,
                156,
                1.2765338149086527,
                786.0419455453388,
                136,
                2.9935887330206743,
                393.0209727726694,
                77,
                3.884055717388181,
                1150.6769706300352,
                74,
                5.178740956517575,
                52.969102188531025,
                70,
                4.251272992007788,
                157.7343580417903,
                58,
                2.0910789768144062,
                588.4926828214484,
                52,
                5.18659493815155,
                2.6298272103200158,
                50,
                0.366868208769208,
                39.81490468210017,
                45,
                4.320388030386763,
                522.369400579779,
                44,
                5.674938062859562,
                550.7553308144597,
                29,
                1.063429113240145,
                77.55225668908888,
                18,
                2.707354735693604,
                1179.0629008647159,
                17,
                5.040336346834424,
                79.62980936420034,
                16,
                3.45645005064957,
                1097.7078858947966,
                14,
                3.4864697137838725,
                548.6777781393482,
                12,
                1.6648695734773908,
                254.43144545527034,
                12,
                5.0110148154009195,
                557.3142781434544,
                12,
                5.599190773323008,
                606.9776743688307,
                9,
                3.9746383055666867,
                21.32991313471798,
                8,
                0.2696533694331239,
                294.2463501373705
        };
        //Ch 47
        private static double[] Table47A_Arguments = new double[]
        {
            0,0,1,0,
            2,0,-1,0,
            2,0,0,0,
            0,0,2,0,
            0,1,0,0,
            0,0,0,2,
            2,0,-2,0,
            2,-1,-1,0,
            2,0,1,0,
            2,-1,0,0,
            0,1,-1,0,
            1,0,0,0,
            0,1,1,0,
            2,0,0,-2,
            0,0,1,2,
            0,0,1,-2,
            4,0,-1,0,
            0,0,3,0,
            4,0,-2,0,
            2,1,-1,0,
            2,1,0,0,
            1,0,-1,0,
            1,1,0,0,
            2,-1,1,0,
            2,0,2,0,
            4,0,0,0,
            2,0,-3,0,
            0,1,-2,0,
            2,0,-1,2,
            2,-1,-2,0,
            1,0,1,0,
            2,-2,0,0,

            0,1,2,0,
            0,2,0,0,
            2,-2,-1,0,
            2,0,1,-2,
            2,0,0,2,
            4,-1,-1,0,
            0,0,2,2,
            3,0,-1,0,
            2,1,1,0,
            4,-1,-2,0,
            0,2,-1,0,
            2,2,-1,0,
            2,1,-2,0,
            2,-1,0,-2,
            4,0,1,0,
            0,0,4,0,
            4,-1,0,0,
            1,0,-2,0,
            2,1,0,-2,
            0,0,2,-2,
            1,1,1,0,
            3,0,-2,0,
            4,0,-3,0,
            2,-1,2,0,
            0,2,1,0,
            1,1,-1,0,
            2,0,3,0,
            2,0,-1,-2
        };
        private static double[] Table47B_Arguments = new double[]
        {
            0,0,0,1,//
            0,0,1,1, //
            0,0,1,-1,//
            2,0,0,-1,//
            2,0,-1,1,//
            2,0,-1,-1,//
            2,0,0,1,//
            0,0,2,1,//
            2,0,1,-1,//
            0,0,2,-1,//
            2,-1,0,-1,//
            2,0,-2,-1,//
            2,0,1,1,//
            2,1,0,-1,//
            2,-1,-1,1,//
            2,-1,0,1,//
            2,-1,-1,-1,//
            0,1,-1,-1,//
            4,0,-1,-1,//
            0,1,0,1,//
            0,0,0,3,//
            0,1,-1,1,//
            1,0,0,1,//
            0,1,1,1,//
            0,1,1,-1,//
            0,1,0,-1,//
            1,0,0,-1,//
            0,0,3,1,//
            4,0,0,-1,//
            4,0,-1,1,//

            0,0,1,-3,//
            4,0,-2,1,//
            2,0,0,-3,//
            2,0,2,-1,//
            2,-1,1,-1,//
            2,0,-2,1,//
            0,0,3,-1,//
            2,0,2,1,//
            2,0,-3,-1,//
            2,1,-1,1,//
            2,1,0,1,//
            4,0,0,1,//
            2,-1,1,1,//
            2,-2,0,-1,//
            0,0,1,3,//
            2,1,1,-1,//
            1,1,0,-1,//
            1,1,0,1,//
            0,1,-2,-1,//
            2,1,-1,-1,//
            1,0,1,1,//
            2,-1,-2,-1,//
            0,1,2,1,//
            4,0,-2,-1,//
            4,-1,-1,-1,//
            1,0,1,-1,//
            4,0,1,-1,//
            1,0,-1,-1,//
            4,-1,0,-1,//
            2,-2,0,1,//
        };
        private static double[] Table47A_El_Er = new double[]
        {
            //El
            6288774, 1274027,658314,213618,-185116,-114332,58793,57066,53322,45758,
            -40923,-34720,-30383,15327,-12528,10980,10675,10034,8548,-7888,-6766,-5163,
            4987,4036,3994,3861,3665,-2689,-2602,2390,-2348,2236,-2120,-2069,2048,-1773,
            -1595,1215,-1110,-892,-810,759,-713,-700,691,596,549,537,520,-487,-399,-381,
            351,-340,330,327,-323,299,294,0,
            //Er
            -20905355,-3699111,-2955968,-569925,48888,-3149,246158,-152138,-170733,-204586,
            -129620,108743,104755,10321,0,79661,-34782,-23210,-21636,24208,30824,-8379,-16675,
            -12831,-10445,-11650,14403,-7003,0,10056,6322,-9884,5751,0,-4950,4130,0,-3958,0,3258,
            2616,-1897,-2117,2354,0,0,-1423,-1117,-1571,-1739,0,-4421,0,0,0,0,1165,0,0,8752

        };
        private static double[] Table47B_Eb = new double[]
        {
            5128122,280602,277693,173237,55413,46271,32573,17198,9266,8822,
            8216,4324,4200,-3359,2463,2211,2065,-1870,1828,-1794,-1749,-1565,-1491,
            -1475,-1410,-1344,-1335,1107,1021,833,

            777,671,607,596,491,-451,439,422,421,-366,-351,331,315,302,-283,-229,
            223,223,-220,-220,-185,181,-177,176,166,-164,132,-119,115,107
        };

        private static double Get_Table47A_Values(double[] values, int l, double t, bool sine)
        {
            //sine true returns El
            //sine false return Er
            //Er values start at 60 in the Table47A_El_Er array.

            int nl = l * 4;

            if (sine)
            {
                double e = 1;

                if (Table47A_Arguments[nl + 1] != 0)
                {
                    e = 1 - .002516 * t - .0000074 * Math.Pow(t, 2);

                    if (Math.Abs(Table47A_Arguments[nl + 1]) == 2)
                    {
                        e *= e;
                    }
                }
                return Table47A_El_Er[l] * e * Math.Sin(Table47A_Arguments[nl] * values[0] + Table47A_Arguments[nl + 1] * values[1] +
                   Table47A_Arguments[nl + 2] * values[2] + Table47A_Arguments[nl + 3] * values[3]);
            }
            else
            {
                double e = 1;
                if (Table47A_Arguments[nl + 1] != 0)
                {
                    e = 1 - .002516 * t - .0000074 * Math.Pow(t, 2);

                    if (Math.Abs(Table47A_Arguments[nl + 1]) == 2)
                    {
                        e *= e;
                    }
                }
                return Table47A_El_Er[l + 60] * e * Math.Cos(Table47A_Arguments[nl] * values[0] + Table47A_Arguments[nl + 1] * values[1] +
                   Table47A_Arguments[nl + 2] * values[2] + Table47A_Arguments[nl + 3] * values[3]);
            }
        }
        private static double Get_Table47B_Values(double[] values, int l, double t)
        {
            int nl = l * 4;
            double e = 1;

            if (Table47B_Arguments[nl + 1] != 0)
            {
                e = 1 - .002516 * t - .0000074 * Math.Pow(t, 2);
             
                if (Math.Abs(Table47B_Arguments[nl + 1]) == 2)
                {
                    e *= e;
                }
            }
            return Table47B_Eb[l] * e * Math.Sin(Table47B_Arguments[nl] * values[0] + Table47B_Arguments[nl + 1] * values[1] +
               Table47B_Arguments[nl + 2] * values[2] + Table47B_Arguments[nl + 3] * values[3]);
        }
    }
}
