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

namespace CoordinateSharp.Magnetic
{
    [Serializable]
    internal class DataPoints
    {
        public DataPoints(DataModel model, Magnetic magnetic)
        {
            Parent = magnetic;
            Points = new List<DataPoint>();

            if (model == DataModel.WMM2025)
            {
                Load_Data(WMM2025COF.Data);
                modelYear = 2025;
            }
            else if (model == DataModel.WMM2020)
            {
                Load_Data(WMM2020COF.Data);
                modelYear = 2020;
            }
            else if (model == DataModel.WMM2015)
            {
                Load_Data(WMM2015COF.Data);
                modelYear = 2015;
            }
            
        }

        private readonly double modelYear;

        public void Load_Values()
        {
            Load_TimeDependents();
            Load_Schmidts();
            Load_Fields();
            Load_Seculars();
        }

        private void Load_Data(List<CoefficientModel> models)
        {
            int index = 0;
            foreach (var model in models)
            {
                Points.Add(new DataPoint(model, Parent, index));
                index++;
            }
        }

        public List<DataPoint> Points { get; }

        public Magnetic Parent { get; }

        //Time Dependents
        private void Load_TimeDependents()
        {
            
            foreach (DataPoint dp in Points)
            {             
                var dec = Parent.decimalDate - modelYear;

                //Gm
                if (dp.Gm == null) { dp.TimeDependent_Gm = null; }
                dp.TimeDependent_Gm = dp.Gm + dec * dp.Gtm;
                //Hm
                if (dp.Hm == null) { dp.TimeDependent_Hm = null; }
                dp.TimeDependent_Hm = dp.Hm + dec * dp.Htm;           
            }
        }

        //Schmidts
        private void Load_Schmidts()
        {
            Load_Schimdts_P();
            Load_Schimdts_Pt();
            Load_Schimdts_dP();


        }
        private void Load_Schimdts_P()
        {
            int x = 0;
            int xx = 0;
           
            foreach (DataPoint dp in Points)
            {
                if (dp.IsFactFactDouble)
                {
                    //Factdouble
                    double fact = DoubleFactorial(2 * dp.Degree - 1.0);
                    double sin = Math.Pow(Math.Sin(Parent.nLatGC), 2.0);
                    double pow = Math.Pow(1 - sin, (dp.Degree / 2.0));
                    dp.Schmidt_Semi_P = fact * pow;
                }
                else if (dp.IndexPoint == 0)
                {
                    dp.Schmidt_Semi_P = Math.Sin(Parent.nLatGC) * (2 * dp.Order + 1) * 1;
                }
                else if (dp.Degree > 2 && dp.Degree - dp.Order >= 2)
                {
                    dp.Schmidt_Semi_P = (Math.Sin(Parent.nLatGC) * (2 * dp.Degree - 1) * Points[x].Schmidt_Semi_P - (dp.Degree + dp.Order - 1) * Points[xx].Schmidt_Semi_P) / (dp.Degree - dp.Order);
                    x++;
                    xx++;

                }
                else if (dp.Order == 0)
                {
                    dp.Schmidt_Semi_P = (Math.Sin(Parent.nLatGC) * (2 * dp.Degree - 1) * Points[x].Schmidt_Semi_P - (dp.Degree + dp.Order - 1)) / (dp.Degree - dp.Order);
                    x++;
                }
                else
                {
                    dp.Schmidt_Semi_P = (Math.Sin(Parent.nLatGC) * (2 * dp.Degree - 1) * Points[x].Schmidt_Semi_P) / (dp.Degree - dp.Order);
                    x++;
                }
               
            }

        }
        private void Load_Schimdts_Pt()
        {

            foreach (DataPoint dp in Points)
            {
                if (dp.Order == 0) { dp.Schmidt_Semi_Pt = dp.Schmidt_Semi_P; }
                else
                {

                    double fact1 = Factorial(dp.Degree - dp.Order);
                    double fact2 = Factorial(dp.Degree + dp.Order);
                    double sqrt = Math.Sqrt(2 * fact1 / fact2);

                    dp.Schmidt_Semi_Pt = sqrt * dp.Schmidt_Semi_P;
                }


            }
        }
        private void Load_Schimdts_dP()
        {

            int forwardIndex = 1;
           
            foreach (DataPoint dp in Points)
            {
                if (dp.Gm == null) { dp.Schmidt_Semi_dP = null; return; }
                while (Points[forwardIndex].IsFactFactDouble)
                {
                    forwardIndex++;
                }
                //Hold as we must iterate forward
                double p1 = ((dp.Degree + 1) * Math.Tan(Parent.nLatGC) * dp.Schmidt_Semi_Pt);
                double sqrt = Math.Sqrt(Math.Pow(dp.Degree + 1, 2) - Math.Pow(dp.Order, 2)) * 1;
                double p2 = Math.Cos(Parent.nLatGC);
                dp.Schmidt_Semi_dP = p1 - (sqrt / p2) * Points[forwardIndex].Schmidt_Semi_Pt;
                forwardIndex++;
            }
        }

        //Fields
        private void Load_Fields()
        {
            Load_Field_X();
            Load_Field_Y();
            Load_Field_Z();
        }
        private void Load_Field_X()
        {
            foreach (var dp in Points)
            {
                double rad = Parent.nLngGC;
                double pow = Math.Pow(Parent.geomagneticRadius / Parent.radius, dp.Degree + 2);
                dp.Field_X = -1 * pow *
                    (dp.TimeDependent_Gm * Math.Cos(dp.Order * rad) + dp.TimeDependent_Hm * Math.Sin(dp.Order * rad)) *
                    dp.Schmidt_Semi_dP;
            }
        }
        private void Load_Field_Y()
        {
            foreach (var dp in Points)
            {
                double lngRad = Parent.nLngGC;
                double latRad = Parent.nLatGC;
                double pow = Math.Pow(Parent.geomagneticRadius / Parent.radius, dp.Degree + 2);
                dp.Field_Y = pow * dp.Order *
                    (dp.TimeDependent_Gm * Math.Sin(dp.Order * lngRad) - dp.TimeDependent_Hm * Math.Cos(dp.Order * lngRad)) *
                    dp.Schmidt_Semi_Pt / Math.Cos(latRad);
            }
        }
        private void Load_Field_Z()
        {
            foreach (var dp in Points)
            {
                double lngRad = Parent.nLngGC;
            
                double pow = Math.Pow(Parent.geomagneticRadius / Parent.radius, dp.Degree + 2);
                dp.Field_Z = -1 * (dp.Degree + 1) * pow *
                    (dp.TimeDependent_Gm * Math.Cos(dp.Order * lngRad) + dp.TimeDependent_Hm * Math.Sin(dp.Order * lngRad)) *
                    dp.Schmidt_Semi_Pt;
            }
        }

        //Seculars
        private void Load_Seculars()
        {
            Load_Secular_X();
            Load_Secular_Y();
            Load_Secular_Z();
        }
        private void Load_Secular_X()
        {
            foreach (var dp in Points)
            {
                double rad = Parent.nLngGC;
                double pow = Math.Pow(Parent.geomagneticRadius / Parent.radius, dp.Degree + 2);
                dp.Secular_X = -1 * pow *
                    (dp.Gtm * Math.Cos(dp.Order * rad) + dp.Htm * Math.Sin(dp.Order * rad)) *
                    dp.Schmidt_Semi_dP;
            }
        }
        private void Load_Secular_Y()
        {
            foreach (var dp in Points)
            {
                double lngRad = Parent.nLngGC;
                double latRad = Parent.nLatGC;
                double pow = Math.Pow(Parent.geomagneticRadius / Parent.radius, dp.Degree + 2);
                dp.Secular_Y = pow * (dp.Order *
                    (dp.Gtm * Math.Sin(dp.Order * lngRad) - dp.Htm * Math.Cos(dp.Order * lngRad)) *
                    dp.Schmidt_Semi_Pt / Math.Cos(latRad));
            }
        }
        private void Load_Secular_Z()
        {
            foreach (var dp in Points)
            {
                double lngRad = Parent.nLngGC;
               
                double pow = Math.Pow(Parent.geomagneticRadius / Parent.radius, dp.Degree + 2);
                dp.Secular_Z = -1 * (dp.Degree + 1) * pow *
                    (dp.Gtm * Math.Cos(dp.Order * lngRad) + dp.Htm * Math.Sin(dp.Order * lngRad)) *
                    dp.Schmidt_Semi_Pt;
            }
        }

        //Math Functions

        private double DoubleFactorial(double n)
        {
            if (n == 0 || n == 1)
                return 1;

            return n * DoubleFactorial(n - 2);
        }

        private double Factorial(double n)
        {
            for (double i = n - 1; i > 0; i--)
            {
                n *= i;
            }
            if (n == 0) { n = 1; }
            return n;
        }

        public double? Sum_Fields_X
        {
            get
            {
                return Points.Sum(x => x.Field_X);
            }
        }
        public double? Sum_Fields_Y
        {
            get
            {
                return Points.Sum(x => x.Field_Y);
            }
        }
        public double? Sum_Fields_Z
        {
            get
            {
                return Points.Sum(x => x.Field_Z);
            }
        }

        public double? Sum_Seculars_X
        {
            get
            {
                return Points.Sum(x => x.Secular_X);
            }
        }
        public double? Sum_Seculars_Y
        {
            get
            {
                return Points.Sum(x => x.Secular_Y);
            }
        }
        public double? Sum_Seculars_Z
        {
            get
            {
                return Points.Sum(x => x.Secular_Z);
            }
        }


    }
}
