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
using System.Collections.Generic;
using System.ComponentModel;

namespace CoordinateSharp
{
    /// <summary>
    /// Class for handling all location based information.
    /// This is the main class of CoordinateSharp. It will contain all coordinate conversions and celestial information once populated. 
    /// Most everything you need in the library will be contained in the Coordinate class.
    /// </summary>
    [Serializable]
    public partial class Coordinate : INotifyPropertyChanged
    {      
        private CoordinatePart latitude;
        private CoordinatePart longitude;
        private DateTime geoDate;
        private double offset=0;
        private int? utm_mgrs_LongitudeZone_Override = null;

        private UniversalTransverseMercator utm;
        private MilitaryGridReferenceSystem mgrs;
        private Cartesian cartesian;
        private ECEF ecef;
             
        private Celestial celestialInfo;

        internal double equatorial_radius;
        internal double inverse_flattening;       

        /// <summary>
        /// Latitudinal Coordinate Part.
        /// </summary>
        public CoordinatePart Latitude
        {
            get { return latitude; }
            set
            {
                if (latitude != value)
                {
                    if (value.Position == CoordinatesPosition.E || value.Position == CoordinatesPosition.W)
                    { throw new ArgumentException("Invalid Position", "Latitudinal positions cannot be set to East or West."); }
                    latitude = value;
                    latitude.parent = this;
                    if (EagerLoadSettings.Celestial)
                    {
                        celestialInfo.CalculateCelestialTime(Latitude.DecimalDegree, Longitude.DecimalDegree, geoDate, EagerLoadSettings, offset);
                    }
                    if (longitude != null)
                    {

                        if (EagerLoadSettings.UTM_MGRS)
                        {
                            utm = new UniversalTransverseMercator(latitude.ToDouble(), longitude.ToDouble(), this, utm.equatorial_radius, utm.inverse_flattening);
                            mgrs = new MilitaryGridReferenceSystem(utm);
                        }
                        if (EagerLoadSettings.Cartesian)
                        {
                            cartesian = new Cartesian(this);
                        }
                        if (EagerLoadSettings.ECEF)
                        {
                            ecef = new ECEF(this);
                        }
                    }
                    CoordinateChanged?.Invoke(this, new EventArgs());
                }
            }
        }
        /// <summary>
        /// Longitudinal Coordinate Part.
        /// </summary>
        public CoordinatePart Longitude
        {
            get { return longitude; }
            set
            {
                if (longitude != value)
                {
                    if (value.Position == CoordinatesPosition.N || value.Position == CoordinatesPosition.S)
                    { throw new ArgumentException("Invalid Position", "Longitudinal positions cannot be set to North or South."); }
                    longitude = value;
                    longitude.parent = this;
                    if (EagerLoadSettings.Celestial)
                    {
                        celestialInfo.CalculateCelestialTime(Latitude.DecimalDegree, Longitude.DecimalDegree, geoDate, EagerLoadSettings, offset);
                    }
                    if (latitude != null)
                    {
                        if (EagerLoadSettings.UTM_MGRS)
                        {
                            utm = new UniversalTransverseMercator(latitude.ToDouble(), longitude.ToDouble(), this, utm.equatorial_radius, utm.inverse_flattening);
                            mgrs = new MilitaryGridReferenceSystem(utm);
                        }
                        if (EagerLoadSettings.Cartesian)
                        {
                            cartesian = new Cartesian(this);
                        }
                        if (EagerLoadSettings.ECEF)
                        {
                            ecef = new ECEF(this);
                        }
                    }
                    CoordinateChanged?.Invoke(this, new EventArgs());
                }
            }
        }
        /// <summary>
        /// Date at coordinate used to calculate celestial information.
        /// </summary>
        /// <remarks>
        /// Assumes all times are in UTC, regardless of DateTimeKind value.
        /// </remarks>
        public DateTime GeoDate
        {
            get { return geoDate; }
            set
            {
                if (geoDate != value)
                {
                    geoDate = value;                 
                    NotifyPropertyChanged("CelestialInfo");
                    NotifyPropertyChanged("GeoDate");
                    CoordinateChanged?.Invoke(this, new EventArgs());
                }
            }
        }
        /// <summary>
        /// GeoDate UTC Offset. This must be set if working / eager loading in local time.
        /// </summary>
        public double Offset
        {
            get { return offset; }
            set
            {              
                if (offset != value)
                {
                    if (value < -12 || value > 12)
                    { throw new ArgumentOutOfRangeException("Time offsets cannot be greater than 12 or less than -12."); }

                    offset = value;
                   
                    NotifyPropertyChanged("CelestialInfo");
                    NotifyPropertyChanged("Offset");
                    CoordinateChanged?.Invoke(this, new EventArgs());
                }
            }
        }       

        /// <summary>
        /// Universal Transverse Mercator values.
        /// </summary>
        public UniversalTransverseMercator UTM
        {
            get
            {
                return utm;
            }
        }
        
        /// <summary>
        /// Military Grid Reference System (NATO UTM) values.
        /// </summary>
        public MilitaryGridReferenceSystem MGRS
        {
            get
            {
                return mgrs;
            }
        }
        /// <summary>
        /// Cartesian (based on spherical earth).
        /// </summary>
        public Cartesian Cartesian
        {
            get
            {
                return cartesian;
            }
        }
        /// <summary>
        /// Earth Centered Earth Fixed Coordinate. 
        /// Uses Ellipsoidal height with no geoid model included.
        /// <remarks>
        /// GeoHeight at 0 = Mean Sea Level based on the provided Datum.
        /// </remarks>
        /// </summary>
        public ECEF ECEF
        {
            get
            {
                return ecef;
            }

            //Required due to GeoDetic Height
            internal set
            {
                if (ecef != value)
                {
                    ecef = value;
                    NotifyPropertyChanged("ECEF");
                }
            }
        }
        /// <summary>
        /// Celestial information based on the objects location and geographic UTC date.
        /// </summary>
        public Celestial CelestialInfo
        {
            get { return celestialInfo; }
        }

        /// <summary>
        /// Coordinate string formatting options.
        /// </summary>
        public CoordinateFormatOptions FormatOptions { get; set; } = GlobalSettings.Default_CoordinateFormatOptions;
        /// <summary>
        /// Eagerloading settings.
        /// </summary>
        public EagerLoad EagerLoadSettings { get; set; }

        /// <summary>
        /// Equatorial Radius of Earth (Default WGS84)
        /// </summary>
        public double Equatorial_Radius { get { return equatorial_radius; } }

        /// <summary>
        /// Inverse Flattening of Earth (Default WGS84)
        /// </summary>
        public double Inverse_Flattening { get { return inverse_flattening; } }

        //PARSER INDICATOR
        private Parse_Format_Type parse_Format = Parse_Format_Type.None;
        /// <summary>
        /// Used to determine what format the coordinate was parsed from.
        /// Will equal "None" if Coordinate was not initialized via a TryParse() method.
        /// </summary>
        public Parse_Format_Type Parse_Format
        {
            get
            {
                return parse_Format;
            }
            internal set
            {
                if (parse_Format != value)
                {
                    parse_Format = value;
                    NotifyPropertyChanged("Parse_Format");
                }
            }
        }
    }   
}
