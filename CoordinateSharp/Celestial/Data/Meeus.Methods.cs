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
using CoordinateSharp.Formatters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CoordinateSharp
{
    internal partial class MeeusTables
    {
        /// <summary>
        /// Returns Moon Periodic Value Er
        /// </summary>
        /// <param name="D">Moon's mean elongation</param>
        /// <param name="M">Sun's mean anomaly</param>
        /// <param name="N">Moon's mean anomaly</param>
        /// <param name="F">Moon's argument of latitude</param>
        /// <param name="T">Dynamic time</param>
        /// <returns>Er</returns>
        public static double Moon_Periodic_Er(double D, double M, double N, double F, double T)
        {
            //Table 47A contains 60 lines to sum
            double[] values = new double[] { D, M, N, F };
            double sum = 0;
            for (int x = 0; x < 60; x++)
            {
                sum += Get_Table47A_Values(values, x, T, false);
            }

            return sum;
        }
        /// <summary>
        /// Returns Moon Periodic Value El
        /// </summary>
        /// <param name="L">Moon's mean longitude</param>
        /// <param name="D">Moon's mean elongation</param>
        /// <param name="M">Sun's mean anomaly</param>
        /// <param name="N">Moon's mean anomaly</param>
        /// <param name="F">Moon's argument of latitude</param>
        /// <param name="T">Dynamic time</param>
        /// <returns>El</returns>
        public static double Moon_Periodic_El(double L, double D, double M, double N, double F, double T)
        {
            //Table 47A contains 60 lines to sum
            double[] values = new double[] { D, M, N, F };
            double sum = 0;
            for (int x = 0; x < 60; x++)
            {
                sum += Get_Table47A_Values(values, x, T, true);
            }

            //Planetary adjustments
            double A1 = 119.75 + 131.849 * T;
            double A2 = 53.09 + 479264.290 * T;

            sum += 3958 * Math.Sin(A1.ToRadians());
            sum += 1962 * Math.Sin(L.ToRadians() - F);
            sum += 318 * Math.Sin(A2.ToRadians());

            return sum;
        }
        /// <summary>
        /// Returns Moon Periodic Value Eb
        /// </summary>
        /// <param name="L">Moon's mean longitude</param>
        /// <param name="D">Moon's mean elongation</param>
        /// <param name="M">Sun's mean anomaly</param>
        /// <param name="N">Moon's mean anomaly</param>
        /// <param name="F">Moon's argument of latitude</param>
        /// <param name="T">Dynamic time</param>
        /// <returns>Eb</returns>
        public static double Moon_Periodic_Eb(double L, double D, double M, double N, double F, double T)
        {
            //Not returning exact Meeus Values
            //LDMNFT Accurate
            //A1 & A3 Accurate
            //Table checked match (book example may be contain incorrect sum or table value)?

            //Table 47B contains 60 lines to sum
            double[] values = new double[] { D, M, N, F };
          
            double sum = 0;
            for (int x = 0; x < 60; x++)
            {
                sum += Get_Table47B_Values(values, x, T);
            }

            //Planetary adjustments     
            double A1 = 119.75 + 131.849 * T;
            double A3 = 313.45 + 481266.484 * T;

            sum += -2235 * Math.Sin(L.ToRadians())
              + 382 * Math.Sin(A3.ToRadians())
            + 175 * Math.Sin(A1.ToRadians() - F)
            + 175 * Math.Sin(A1.ToRadians() + F)
            + 127 * Math.Sin(L.ToRadians() - M)
            - 115 * Math.Sin(L.ToRadians() + M);

            return sum;
        }
        //Ch 50
        /// <summary>
        /// Sum of Apogee Terms from Jean Meeus Astronomical Algorithms Table 50.A
        /// </summary>
        /// <param name="D">Moom's mean elongation at time JDE</param>
        /// <param name="M">Sun's mean anomaly</param>
        /// <param name="F">Moon's arguement f latitude</param>
        /// <param name="T">Time in Julian centuries since epoch 2000</param>
        /// <returns>double</returns>
        public static double ApogeeTermsA(double D, double M, double F, double T)
        {
            double sum;

            sum = Math.Sin(2 * D) * 0.4392;
            sum += Math.Sin(4 * D) * 0.0684;
            sum += Math.Sin(M) * .0456 - 0.00011 * T;
            sum += Math.Sin(2 * D - M) * .0426 - 0.00011 * T;
            sum += Math.Sin(2 * F) * .0212;
            sum += Math.Sin(D) * -0.0189;
            sum += Math.Sin(6 * D) * .0144;
            sum += Math.Sin(4 * D - M) * .0113;
            sum += Math.Sin(2 * D + 2 * F) * .0047;
            sum += Math.Sin(D + M) * .0036;
            sum += Math.Sin(8 * D) * .0035;
            sum += Math.Sin(6 * D - M) * .0034;
            sum += Math.Sin(2 * D - 2 * F) * -0.0034;
            sum += Math.Sin(2 * D - 2 * M) * .0022;
            sum += Math.Sin(3 * D) * -.0017;
            sum += Math.Sin(4 * D + 2 * F) * 0.0013;

            sum += Math.Sin(8 * D - M) * .0011;
            sum += Math.Sin(4 * D - 2 * M) * .0010;
            sum += Math.Sin(10 * D) * .0009;
            sum += Math.Sin(3 * D + M) * .0007;
            sum += Math.Sin(2 * M) * .0006;
            sum += Math.Sin(2 * D + M) * .0005;
            sum += Math.Sin(2 * D + 2 * M) * .0005;
            sum += Math.Sin(6 * D + 2 * F) * .0004;
            sum += Math.Sin(6 * D - 2 * M) * .0004;
            sum += Math.Sin(10 * D - M) * .0004;
            sum += Math.Sin(5 * D) * -0.0004;
            sum += Math.Sin(4 * D - 2 * F) * -0.0004;
            sum += Math.Sin(2 * F + M) * .0003;
            sum += Math.Sin(12 * D) * .0003;
            sum += Math.Sin(2 * D + 2 * F - M) * 0.0003;
            sum += Math.Sin(D - M) * -0.0003;
            return sum;
        }
        /// <summary>
        /// Sum of Perigee Terms from Jean Meeus Astronomical Algorithms Table 50.A
        /// </summary>
        /// <param name="D">Moom's mean elongation at time JDE</param>
        /// <param name="M">Sun's mean anomaly</param>
        /// <param name="F">Moon's arguement f latitude</param>
        /// <param name="T">Time in Julian centuries since epoch 2000</param>
        /// <returns>double</returns>
        public static double PerigeeTermsA(double D, double M, double F, double T)
        {
            double sum;

            sum = Math.Sin(2 * D) * -1.6769;
            sum += Math.Sin(4 * D) * .4589;
            sum += Math.Sin(6 * D) * -.1856;
            sum += Math.Sin(8 * D) * .0883;
            sum += Math.Sin(2 * D - M) * -.0773 + .00019 * T;
            sum += Math.Sin(M) * .0502 - .00013 * T;
            sum += Math.Sin(10 * D) * -.0460;
            sum += Math.Sin(4 * D - M) * .0422 - .00011 * T;
            sum += Math.Sin(6 * D - M) * -.0256;
            sum += Math.Sin(12 * D) * .0253;
            sum += Math.Sin(D) * .0237;
            sum += Math.Sin(8 * D - M) * .0162;
            sum += Math.Sin(14 * D) * -.0145;
            sum += Math.Sin(2 * F) * .0129;
            sum += Math.Sin(3 * D) * -.0112;
            sum += Math.Sin(10 * D - M) * -.0104;
            sum += Math.Sin(16 * D) * .0086;
            sum += Math.Sin(12 * D - M) * .0069;
            sum += Math.Sin(5 * D) * .0066;
            sum += Math.Sin(2 * D + 2 * F) * -.0053;
            sum += Math.Sin(18 * D) * -.0052;
            sum += Math.Sin(14 * D - M) * -.0046;
            sum += Math.Sin(7 * D) * -.0041;
            sum += Math.Sin(2 * D + M) * .0040;
            sum += Math.Sin(20 * D) * .0032;
            sum += Math.Sin(D + M) * -.0032;
            sum += Math.Sin(16 * D - M) * .0031;
            sum += Math.Sin(4 * D + M) * -.0029;
            sum += Math.Sin(9 * D) * .0027;
            sum += Math.Sin(4 * D + 2 * F) * .0027;

            sum += Math.Sin(2 * D - 2 * M) * -.0027;
            sum += Math.Sin(4 * D - 2 * M) * .0024;
            sum += Math.Sin(6 * D - 2 * M) * -.0021;
            sum += Math.Sin(22 * D) * -.0021;
            sum += Math.Sin(18 * D - M) * -.0021;
            sum += Math.Sin(6 * D + M) * .0019;
            sum += Math.Sin(11 * D) * -.0018;
            sum += Math.Sin(8 * D + M) * -.0014;
            sum += Math.Sin(4 * D - 2 * F) * -.0014;
            sum += Math.Sin(6 * D + 2 * F) * -.0014;
            sum += Math.Sin(3 * D + M) * .0014;
            sum += Math.Sin(5 * D + M) * -.0014;
            sum += Math.Sin(13 * D) * .0013;
            sum += Math.Sin(20 * D - M) * .0013;
            sum += Math.Sin(3 * D + 2 * M) * .0011;
            sum += Math.Sin(4 * D + 2 * F - 2 * M) * -.0011;
            sum += Math.Sin(D + 2 * M) * -.0010;
            sum += Math.Sin(22 * D - M) * -.0009;
            sum += Math.Sin(4 * F) * -.0008;
            sum += Math.Sin(6 * D - 2 * F) * .0008;
            sum += Math.Sin(2 * D - 2 * F + M) * .0008;
            sum += Math.Sin(2 * M) * .0007;
            sum += Math.Sin(2 * F - M) * .0007;
            sum += Math.Sin(2 * D + 4 * F) * .0007;
            sum += Math.Sin(2 * F - 2 * M) * -.0006;
            sum += Math.Sin(2 * D - 2 * F + 2 * M) * -.0006;
            sum += Math.Sin(24 * D) * .0006;
            sum += Math.Sin(4 * D - 4 * F) * .0005;
            sum += Math.Sin(2 * D + 2 * M) * .0005;
            sum += Math.Sin(D - M) * -.0004;

            return sum;
        }
        /// <summary>
        /// Sum of Apogee Terms from Jean Meeus Astronomical Algorithms Table 50.B
        /// </summary>
        /// <param name="D">Moom's mean elongation at time JDE</param>
        /// <param name="M">Sun's mean anomaly</param>
        /// <param name="F">Moon's arguement f latitude</param>
        /// <param name="T">Time in Julian centuries since epoch 2000</param>
        /// <returns>double</returns>
        public static double ApogeeTermsB(double D, double M, double F, double T)
        {
            double sum = 3245.251;

            sum += Math.Cos(2 * D) * -9.147;
            sum += Math.Cos(D) * -.841;
            sum += Math.Cos(2 * F) * .697;
            sum += Math.Cos(M) * -0.656 + .0016 * T;
            sum += Math.Cos(4 * D) * .355;
            sum += Math.Cos(2 * D - M) * .159;
            sum += Math.Cos(D + M) * .127;
            sum += Math.Cos(4 * D - M) * .065;

            sum += Math.Cos(6 * D) * .052;
            sum += Math.Cos(2 * D + M) * .043;
            sum += Math.Cos(2 * D + 2 * F) * .031;
            sum += Math.Cos(2 * D - 2 * F) * -.023;
            sum += Math.Cos(2 * D - 2 * M) * .022;
            sum += Math.Cos(2 * D + 2 * M) * .019;
            sum += Math.Cos(2 * M) * -.016;
            sum += Math.Cos(6 * D - M) * .014;
            sum += Math.Cos(8 * D) * .010;

            return sum;
        }
        /// <summary>
        /// Sum of Perigee Terms from Jean Meeus Astronomical Algorithms Table 50.B
        /// </summary>
        /// <param name="D">Moom's mean elongation at time JDE</param>
        /// <param name="M">Sun's mean anomaly</param>
        /// <param name="F">Moon's arguement f latitude</param>
        /// <param name="T">Time in Julian centuries since epoch 2000</param>
        /// <returns>double</returns>
        public static double PerigeeTermsB(double D, double M, double F, double T)
        {
            //Sum of Perigee Terms from Jean Meeus Astronomical Algorithms Table 50.B          
            double sum = 3629.215;

            sum += Math.Cos(2 * D) * 63.224;
            sum += Math.Cos(4 * D) * -6.990;
            sum += Math.Cos(2 * D - M) * 2.834 - .0071 * T;
            sum += Math.Cos(6 * D) * 1.927;
            sum += Math.Cos(D) * -1.263;
            sum += Math.Cos(8 * D) * -.702;
            sum += Math.Cos(M) * .696 - .0017 * T;
            sum += Math.Cos(2 * F) * -.690;
            sum += Math.Cos(4 * D - M) * -.629 + .0016 * T;
            sum += Math.Cos(2 * D - 2 * F) * -.392;
            sum += Math.Cos(10 * D) * .297;
            sum += Math.Cos(6 * D - M) * .260;
            sum += Math.Cos(3 * D) * .201;
            sum += Math.Cos(2 * D + M) * -.161;
            sum += Math.Cos(D + M) * .157;
            sum += Math.Cos(12 * D) * -.138;
            sum += Math.Cos(8 * D - M) * -.127;
            sum += Math.Cos(2 * D + 2 * F) * .104;
            sum += Math.Cos(2 * D - 2 * M) * .104;
            sum += Math.Cos(5 * D) * -.079;
            sum += Math.Cos(14 * D) * .068;

            sum += Math.Cos(10 * D - M) * .067;
            sum += Math.Cos(4 * D + M) * .054;
            sum += Math.Cos(12 * D - M) * -.038;
            sum += Math.Cos(4 * D - 2 * M) * -.038;
            sum += Math.Cos(7 * D) * .037;
            sum += Math.Cos(4 * D + 2 * F) * -.037;
            sum += Math.Cos(16 * D) * -.035;
            sum += Math.Cos(3 * D + M) * -.030;
            sum += Math.Cos(D - M) * .029;
            sum += Math.Cos(6 * D + M) * -.025;
            sum += Math.Cos(2 * M) * .023;
            sum += Math.Cos(14 * D - M) * .023;
            sum += Math.Cos(2 * D + 2 * M) * -.023;
            sum += Math.Cos(6 * D - 2 * M) * .022;
            sum += Math.Cos(2 * D - 2 * F - M) * -.021;
            sum += Math.Cos(9 * D) * -.020;
            sum += Math.Cos(18 * D) * .019;
            sum += Math.Cos(6 * D + 2 * F) * .017;
            sum += Math.Cos(2 * F - M) * .014;
            sum += Math.Cos(16 * D - M) * -.014;
            sum += Math.Cos(4 * D - 2 * F) * .013;
            sum += Math.Cos(8 * D + M) * .012;
            sum += Math.Cos(11 * D) * .011;
            sum += Math.Cos(5 * D + M) * .010;
            sum += Math.Cos(20 * D) * -.010;

            return sum;
        }

        /// <summary>
        /// Sum of S from Jean Meeus Astronomical Algorithms Table 27.C
        /// </summary>
        /// <param name="T">T</param>
        /// <returns>double</returns>
        public static double Equinox_Solstice_Sum_of_S(double T)
        {
            double sum = 0;

            for (int x = 0; x < Table27C_Arguements.Count(); x += 3)
            {
                double a = Table27C_Arguements[x];
                double b = Table27C_Arguements[x + 1];
                double c = Table27C_Arguements[x + 2];
                sum += a * Math.Cos(b + c * T);
            }

            return sum;
        }

    }
    internal class MeeusFormulas
    {
        public static double Get_Sidereal_Time(double JD)
        {
            //Ch. 12
            //T = Dynamic Time
            //Oo = mean sidereal time at Greenwich at 0h UT
            double T = (JD - 2451545) / 36525;
            double Oo = 280.46061837 + 360.98564736629 * (JD - 2451545) +
                .000387933 * Math.Pow(T, 2) - Math.Pow(T, 3) / 38710000;
            return Oo;
        }
    }
}
