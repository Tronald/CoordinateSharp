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
using System.ComponentModel;

namespace CoordinateSharp
{
    /// <summary>
    /// Observable class for handling geodetic (latitudinal and longitudinal) coordinate parts.
    /// </summary>
    /// <remarks>
    /// Values can be passed to Coordinate object Latitude and Longitude properties.
    /// </remarks>
    [Serializable]
    public partial class CoordinatePart : INotifyPropertyChanged
    {
        //String Format Defaults:
        //Format: Degrees Minutes Seconds
        //Rounding: Dependent upon selected format
        //Leading Zeros: False
        //Trailing Zeros: False
        //Display Symbols: True (All Symbols display)
        //Display Hyphens: False
        //Position Display: First                               

        private double decimalDegree;
        private double decimalMinute;
        private int degrees;
        private int minutes;
        private double seconds;
        private CoordinatesPosition position;
        private CoordinateType type;

        internal Coordinate parent;
        /// <summary>
        /// CoordinatePart's parent Coordinate object.
        /// </summary>
        public Coordinate Parent { get { return parent; } }

        /// <summary>
        /// Decimal Degree format coordinate.
        /// </summary>
        public double DecimalDegree
        {
            get { return decimalDegree; }
            set
            {
                //If changing, notify the needed property changes
                if (decimalDegree != value)
                {
                    //Validate the value
                    if (type == CoordinateType.Lat)
                    {
                        if (value > 90)
                        {
                            throw new ArgumentOutOfRangeException("Degrees out of range", "Latitude degrees cannot be greater than 90");
                        }
                        if (value < -90)
                        {
                            throw new ArgumentOutOfRangeException("Degrees out of range", "Latitude degrees cannot be less than -90");
                        }

                    }
                    if (type == CoordinateType.Long)
                    {
                        if (value > 180)
                        {
                            throw new ArgumentOutOfRangeException("Degrees out of range", "Longitude degrees cannot be greater than 180");
                        }
                        if (value < -180)
                        {
                            throw new ArgumentOutOfRangeException("Degrees out of range", "Longitude degrees cannot be less than -180");
                        }

                    }
                    decimalDegree = value;

                    //Update Position
                    if ((position == CoordinatesPosition.N || position == CoordinatesPosition.E) && decimalDegree < 0)
                    {
                        if (type == CoordinateType.Lat) { position = CoordinatesPosition.S; }
                        else { position = CoordinatesPosition.W; }

                    }
                    if ((position == CoordinatesPosition.W || position == CoordinatesPosition.S) && decimalDegree >= 0)
                    {
                        if (type == CoordinateType.Lat) { position = CoordinatesPosition.N; }
                        else { position = CoordinatesPosition.E; }

                    }
                    //Update the Degree & Decimal Minute
                    double degABS = Math.Abs(decimalDegree); //Make decimalDegree positive for calculations
                    double degFloor = Math.Truncate(degABS); //Truncate the number left to extract the degree
                    decimal f = Convert.ToDecimal(degFloor); //Convert to degree to decimal to keep precision during calculations
                    decimal ddm = Convert.ToDecimal(degABS) - f; //Extract decimalMinute value from decimalDegree
                    ddm *= 60; //Multiply by 60 to get readable decimalMinute

                    double dm = Convert.ToDouble(ddm); //Convert decimalMinutes back to double for storage
                    int df = Convert.ToInt32(degFloor); //Convert degrees to int for storage

                    if (degrees != df)
                    {
                        degrees = df;

                    }
                    if (decimalMinute != dm)
                    {
                        decimalMinute = dm;

                    }
                    //Update Minutes Seconds              
                    double dmFloor = Math.Floor(dm); //Get number left of decimal to grab minute value
                    int mF = Convert.ToInt32(dmFloor); //Convert minute to int for storage
                    f = Convert.ToDecimal(dmFloor); //Create a second minute value and store as decimal for precise calculation

                    decimal s = ddm - f; //Get seconds from minutes
                    s *= 60; //Multiply by 60 to get readable seconds
                    double secs = Convert.ToDouble(s); //Convert back to double for storage

                    if (minutes != mF)
                    {
                        minutes = mF;

                    }
                    if (seconds != secs)
                    {
                        seconds = secs;
                    }

                    Roundtripping_Correction();

                    NotifyProperties(PropertyTypes.DecimalDegree);
                }
            }
        }
        /// <summary>
        /// Decimal formatted minute.
        /// </summary>
        public double DecimalMinute
        {
            get { return decimalMinute; }
            set
            {
                if (decimalMinute != value)
                {
                    if (value < 0) { value *= -1; }//Adjust accidental negative input
                                                   //Validate values     

                    decimal dm = Math.Abs(Convert.ToDecimal(value)) / 60;
                    double decMin = Convert.ToDouble(dm);
                    if (type == CoordinateType.Lat)
                    {

                        if (degrees + decMin > 90) { throw new ArgumentOutOfRangeException("Degrees out of range", "Latitudinal degrees cannot be greater than 90"); }
                    }
                    else
                    {
                        if (degrees + decMin > 180) { throw new ArgumentOutOfRangeException("Degrees out of range", "Longitudinal degrees cannot be greater than 180"); }
                    }
                    if (value >= 60) { throw new ArgumentOutOfRangeException("Minutes out of range", "Coordinate Minutes cannot be greater than or equal to 60"); }
                    if (value < 0) { throw new ArgumentOutOfRangeException("Minutes out of range", "Coordinate Minutes cannot be less than 0"); }


                    decimalMinute = value;


                    decimal decValue = Convert.ToDecimal(value); //Convert value to decimal for precision during calculation
                    decimal dmFloor = Math.Floor(decValue); //Extract minutes
                    decimal secs = decValue - dmFloor; //Extract seconds
                    secs *= 60; //Convert seconds to human readable format

                    decimal newDM = decValue / 60; //divide decimalMinute by 60 to get storage value
                    decimal newDD = degrees + newDM;//Add new decimal value to the floor degree value to get new decimalDegree;
                    if (decimalDegree < 0) { newDD = newDD * -1; } //Restore negative if needed

                    decimalDegree = Convert.ToDouble(newDD);  //Convert back to double for storage                      

                    minutes = Convert.ToInt32(dmFloor); //Convert minutes to int for storage

                    seconds = Convert.ToDouble(secs); //Convert seconds to double for storage 

                    Roundtripping_Correction();

                    NotifyProperties(PropertyTypes.DecimalMinute);
                }
            }

        }
        /// <summary>
        /// Coordinate part degrees.
        /// </summary>
        public int Degrees
        {
            get { return degrees; }
            set
            {
                //Validate Value
                if (degrees != value)
                {

                    if (value < 0) { value *= -1; }//Adjust accidental negative input

                    if (type == CoordinateType.Lat)
                    {
                        if (value + decimalMinute / 100.0 > 90)
                        {
                            throw new ArgumentOutOfRangeException("Degrees", "Latitude degrees cannot be greater than 90");
                        }
                    }
                    if (type == CoordinateType.Long)
                    {
                        if (value + decimalMinute / 100.0 > 180)
                        {
                            throw new ArgumentOutOfRangeException("Degrees", "Longitude degrees cannot be greater than 180");
                        }

                    }

                    decimal f = Convert.ToDecimal(degrees);

                    degrees = value;

                    double degABS = Math.Abs(decimalDegree); //Make decimalDegree positive for calculations
                    decimal dDec = Convert.ToDecimal(degABS); //Convert to Decimal for precision during calculations              
                                                              //Convert degrees to decimal to keep precision        
                    decimal dm = dDec - f; //Extract minutes                                      
                    decimal newDD = degrees + dm; //Add minutes to new degree for decimalDegree

                    if (decimalDegree < 0) { newDD *= -1; } //Set negative as required

                    decimalDegree = Convert.ToDouble(newDD); // Convert decimalDegree to double for storage
                   
                    Roundtripping_Correction();

                    NotifyProperties(PropertyTypes.Degree);
                }
            }
        }
        /// <summary>
        /// Coordinate part minutes.
        /// </summary>
        public int Minutes
        {
            get { return minutes; }
            set
            {
                if (minutes != value)
                {
                    if (value < 0) { value *= -1; }//Adjust accidental negative input
                    //Validate the minutes
                    decimal vMin = Convert.ToDecimal(value);
                    if (type == CoordinateType.Lat)
                    {
                        if (degrees + (vMin / 60) > 90) { throw new ArgumentOutOfRangeException("Degrees out of range", "Latitudinal degrees cannot be greater than 90"); }
                    }
                    else
                    {
                        if (degrees + (vMin / 60) > 180) { throw new ArgumentOutOfRangeException("Degrees out of range", "Longitudinal degrees cannot be greater than 180"); }
                    }
                    if (value >= 60)
                    {
                        throw new ArgumentOutOfRangeException("Minutes out of range", "Minutes cannot be greater than or equal to 60");
                    }
                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException("Minutes out of range", "Minutes cannot be less than 0");
                    }
                    decimal minFloor = Convert.ToDecimal(minutes);//Convert decimal to minutes for calculation
                    decimal f = Convert.ToDecimal(degrees); //Convert to degree to keep precision during calculation 

                    minutes = value;


                    double degABS = Math.Abs(decimalDegree); //Make decimalDegree positive
                    decimal dDec = Convert.ToDecimal(degABS); //Convert to decimalDegree for precision during calculation                        

                    decimal dm = dDec - f; //Extract minutes
                    dm *= 60; //Make minutes human readable

                    decimal secs = dm - minFloor;//Extract Seconds

                    decimal newDM = minutes + secs;//Add seconds to minutes for decimalMinute
                    double decMin = Convert.ToDouble(newDM); //Convert decimalMinute to double for storage
                    decimalMinute = decMin; //Round to correct precision


                    newDM /= 60; //Convert decimalMinute to storage format
                    decimal newDeg = f + newDM; //Add value to degree for decimalDegree
                    if (decimalDegree < 0) { newDeg *= -1; }// Set to negative as required.
                    decimalDegree = Convert.ToDouble(newDeg);//Convert to double and round to correct precision for storage
                 
                    Roundtripping_Correction();

                    NotifyProperties(PropertyTypes.Minute);
                }
            }
        }
        /// <summary>
        /// Coordinate part seconds.
        /// </summary>
        public double Seconds
        {
            get { return seconds; }
            set
            {
                if (value < 0) { value *= -1; }//Adjust accidental negative input
                if (seconds != value)
                {
                    //Validate Seconds
                    decimal vSec = Convert.ToDecimal(value);
                    vSec /= 60;

                    decimal vMin = Convert.ToDecimal(minutes);
                    vMin += vSec;
                    vMin /= 60;

                    if (type == CoordinateType.Lat)
                    {
                        if (degrees + vMin > 90) { throw new ArgumentOutOfRangeException("Degrees out of range", "Latitudinal degrees cannot be greater than 90"); }
                    }
                    else
                    {
                        if (degrees + vMin > 180) { throw new ArgumentOutOfRangeException("Degrees out of range", "Longitudinal degrees cannot be greater than 180"); }
                    }
                    if (value >= 60)
                    {
                        throw new ArgumentOutOfRangeException("Seconds out of range", "Seconds cannot be greater than or equal to 60");
                    }
                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException("Seconds out of range", "Seconds cannot be less than 0");
                    }
                    seconds = value;


                    double degABS = Math.Abs(decimalDegree); //Make decimalDegree positive
                    double degFloor = Math.Truncate(degABS); //Truncate the number left of the decimal
                    decimal f = Convert.ToDecimal(degFloor); //Convert to decimal to keep precision

                    decimal secs = Convert.ToDecimal(seconds); //Convert seconds to decimal for calculations
                    secs /= 60; //Convert to storage format
                    decimal dm = minutes + secs;//Add seconds to minutes for decimalMinute
                    double minFD = Convert.ToDouble(dm); //Convert decimalMinute for storage
                    decimalMinute = minFD;//Round to proper precision

                    decimal nm = Convert.ToDecimal(decimalMinute) / 60;//Convert decimalMinute to decimal and divide by 60 to get storage format decimalMinute
                    double newDeg = degrees + Convert.ToDouble(nm);//Convert to double and add to degree for storage decimalDegree
                    if (decimalDegree < 0) { newDeg *= -1; }//Make negative as needed
                    decimalDegree = newDeg;//Update decimalDegree and round to proper precision   

                    Roundtripping_Correction();

                    NotifyProperties(PropertyTypes.Second);
                }
            }
        }
        /// <summary>
        /// Formatted coordinate part string.
        /// </summary>
        public string Display
        {
            get
            {
                if (parent != null)
                {
                    return ToString(parent.FormatOptions);
                }
                else
                {
                    return ToString();
                }
            }
        }
        /// <summary>
        /// Coordinate part position.
        /// </summary>
        /// <remarks>
        /// Used to determine what hemisphere the coordinate part lies in.
        /// </remarks>
        public CoordinatesPosition Position
        {
            get { return position; }
            set
            {
                if (position != value)
                {
                    if (type == CoordinateType.Long && (value == CoordinatesPosition.N || value == CoordinatesPosition.S))
                    {
                        throw new InvalidOperationException("You cannot change a Longitudinal type coordinate into a Latitudinal");
                    }
                    if (type == CoordinateType.Lat && (value == CoordinatesPosition.E || value == CoordinatesPosition.W))
                    {
                        throw new InvalidOperationException("You cannot change a Latitudinal type coordinate into a Longitudinal");
                    }
                    decimalDegree *= -1; // Change the position
                    position = value;
                    NotifyProperties(PropertyTypes.Position);
                }
            }
        }

        /// <summary>
        /// Adjust for roundtripping added in .NET Core 3.0
        /// Effect floating points only
        /// </summary>
        private void Roundtripping_Correction()
        {
            if (decimalDegree == -0) { decimalDegree = 0; }
            if (decimalMinute == -0) { decimalMinute = 0; }          
            if (seconds == -0) { seconds = 0; } 
        }

        /// <summary>
        /// Used for notify the correct properties.
        /// </summary>
        private enum PropertyTypes
        {
            DecimalDegree, DecimalMinute, Position, Degree, Minute, Second, FormatChange
        }
        /// <summary>
        /// Used to determine the set coordinate part string format.
        /// </summary>
        private enum ToStringType
        {
            Decimal_Degree, Degree_Decimal_Minute, Degree_Minute_Second, Decimal
        }      
    }
}
