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
using System;
using CoordinateSharp;
using CoordinateSharp.Formatters;

namespace CoordinateSharp.Magnetic
{
    public partial class Magnetic
    {
        internal double semiMajorAxis;
        internal double semiMinorAxis;
        internal double inverseFlattening;
        internal double flattening;
        internal double eccentricitySquared;

        internal double nLatGD;
        internal double nLngGD;
        internal double nLatGC;
        internal double nLngGC;


        internal double geomagneticRadius = 6371200;
        internal double radiusOfCurvature;
        internal double northPolarAxis;
        internal double pointOfInterest;
        internal double radius;

        internal double decimalDate;

        internal DataPoints points;

        private MagneticFieldElements magneticFieldElements;
        private SecularVariations secularVariations;
        private Uncertainty uncertainty;
        public MagneticFieldElements MagneticFieldElements { get { return magneticFieldElements; } }
        public SecularVariations SecularVariations { get { return secularVariations; } }
        public Uncertainty Uncertainty { get { return uncertainty; } }
        public DataModel Model { get; }

    }

    /// <summary>
    /// Field elements used in various magnetic data method types.
    /// </summary>
    public class FieldElements
    {
        public double NorthComponent { get; internal set; }
        public double EastComponent { get; internal set; }
        public double DownComponent { get; internal set; }
        public double HorizontalIntensity { get; internal set; }
        public double TotalIntensity { get; internal set; }
        public double Inclination { get; internal set; }
        public double Declination { get; internal set; }
    }

    /// <summary>
    /// Magnetic field elements.
    /// </summary>
    public class MagneticFieldElements : FieldElements
    {
        public MagneticFieldElements(Magnetic m)
        {
            double gc = m.nLatGC - m.nLatGD.ToRadians();
            NorthComponent = m.points.Sum_Fields_X.Value  * Math.Cos(gc) - m.points.Sum_Fields_Z.Value  * Math.Sin(gc);
            EastComponent = m.points.Sum_Fields_Y.Value;
            DownComponent = m.points.Sum_Fields_X.Value * Math.Sin(gc) + m.points.Sum_Fields_Z.Value * Math.Cos(gc);
            HorizontalIntensity = Math.Sqrt(Math.Pow(NorthComponent, 2) + Math.Pow(EastComponent, 2));
            TotalIntensity = Math.Sqrt(Math.Pow(HorizontalIntensity, 2) + Math.Pow(DownComponent, 2));
            Inclination = Math.Atan2(DownComponent, HorizontalIntensity).ToDegrees(); //Y,X
            Declination = Math.Atan2(EastComponent, NorthComponent).ToDegrees(); //Y,X
            if (m.nLatGD > 55)
            {
                GridVariation = Declination - m.nLngGD;
            }
            else if (m.nLatGD < -55)
            {
                GridVariation = Declination + m.nLngGD;
            }
            else
            {
                GridVariation = Declination;
            }

        }

        public double GridVariation { get; }

    }

    /// <summary>
    /// Magnetic secular variation elements.
    /// </summary>
    public class SecularVariations : FieldElements
    {
        public SecularVariations(Magnetic m)
        {
            MagneticFieldElements mf = m.MagneticFieldElements;
            double gc = m.nLatGC - m.nLatGD.ToRadians();
            NorthComponent = m.points.Sum_Seculars_X.Value * Math.Cos(gc) - m.points.Sum_Seculars_Z.Value * Math.Sin(gc);
            EastComponent = m.points.Sum_Seculars_Y.Value;
            DownComponent = m.points.Sum_Seculars_X.Value * Math.Sin(gc) + m.points.Sum_Seculars_Z.Value * Math.Cos(gc);
            HorizontalIntensity = (mf.NorthComponent * NorthComponent + mf.EastComponent * EastComponent) / mf.HorizontalIntensity;
            TotalIntensity = (mf.NorthComponent * NorthComponent + mf.EastComponent * EastComponent + mf.DownComponent*DownComponent) / mf.TotalIntensity;
            Inclination = ((mf.HorizontalIntensity*DownComponent-mf.DownComponent*HorizontalIntensity)/Math.Pow(mf.TotalIntensity,2)).ToDegrees(); //Y,X
            Declination = ((mf.NorthComponent*EastComponent-mf.EastComponent*NorthComponent) / Math.Pow(mf.HorizontalIntensity, 2)).ToDegrees();          
        }

    }

    /// <summary>
    /// Magenetic uncertainty elements.
    /// </summary>
    public class Uncertainty : FieldElements
    {
        //Uncertaints taken from https://www.ngdc.noaa.gov/geomag/WMM/limit.shtml
        public Uncertainty(Magnetic m)
        {
            if(m.Model== DataModel.WMM2015) { Load_2015(m); }
            else if(m.Model == DataModel.WMM2020) { Load_2020(m); }

        }

        //Load WMM 2015 COF
        private void Load_2015(Magnetic m)
        {
            NorthComponent = 138;
            EastComponent = 89;
            DownComponent = 165;
            HorizontalIntensity = 133;
            TotalIntensity = 152;
            Inclination = .22;
            Declination = Math.Sqrt(Math.Pow(.23, 2) + Math.Pow(5430 / m.MagneticFieldElements.HorizontalIntensity, 2));
        }

        //Load WMM 2020 COF
        private void Load_2020(Magnetic m)
        {
            NorthComponent = 131;
            EastComponent = 94;
            DownComponent = 157;
            HorizontalIntensity = 128;
            TotalIntensity = 148;
            Inclination = .21;
            Declination = Math.Sqrt(Math.Pow(.26, 2) + Math.Pow(5625 / m.MagneticFieldElements.HorizontalIntensity, 2));
        }
    }



}
