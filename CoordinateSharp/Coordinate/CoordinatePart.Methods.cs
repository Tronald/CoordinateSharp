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
using System.ComponentModel;

namespace CoordinateSharp
{
    public partial class CoordinatePart : INotifyPropertyChanged
    {
        /// <summary>
        /// Creates a default CoordinatePart.
        /// </summary>
        /// <param name="cType">Coordinate part type</param>
        /// <remarks>
        /// Defaults coordinate part value to 0 degrees.
        /// </remarks>
        /// <example>
        /// The following example creates a default latitudinal coordinate part and assigns it to a Coordinate object
        /// <code>
        /// Coordinate c = new Coordinate();
        /// CoordinatePart cp = new CoordinatePart(CoordinateType.Lat);
        /// c.Latitude = cp;
        /// </code>
        /// </example>
        public CoordinatePart(CoordinateType cType)
        {
            type = cType;
            decimalDegree = 0;
            degrees = 0;
            minutes = 0;
            seconds = 0;
            if (type == CoordinateType.Lat) { position = CoordinatesPosition.N; }
            else { position = CoordinatesPosition.E; }
        }
        /// <summary>
        /// Creates a populated CoordinatePart from a signed degree value.
        /// </summary>
        /// <param name="value">Coordinate part signed value</param>
        /// <param name="cType">Coordinate part type</param>       
        /// <example>
        /// The following example creates a populated latitudinal coordinate part from a signed value and assigns it to a Coordinate object.
        /// <code>
        /// //Create a new default coordinate
        /// Coordinate c = new Coordinate();
        /// 
        /// //Create a coordinate part using a the signed latitude 25.6°.
        /// CoordinatePart cp = new CoordinatePart(25.6, CoordinateType.Lat);
        /// 
        /// //Assign the latitudinal value to the coordinate.
        /// c.Latitude = cp;
        /// </code>
        /// </example>
        public CoordinatePart(double value, CoordinateType cType)
        {
            type = cType;

            if (type == CoordinateType.Long)
            {
                if (value > 180) { throw new ArgumentOutOfRangeException("Degrees out of range", "Longitudinal coordinate decimal cannot be greater than 180."); }
                if (value < -180) { throw new ArgumentOutOfRangeException("Degrees out of range", "Longitudinal coordinate decimal cannot be less than 180."); }
                if (value < 0) { position = CoordinatesPosition.W; }
                else { position = CoordinatesPosition.E; }
            }
            else
            {
                if (value > 90) { throw new ArgumentOutOfRangeException("Degrees out of range", "Latitudinal coordinate decimal cannot be greater than 90."); }
                if (value < -90) { throw new ArgumentOutOfRangeException("Degrees out of range", "Latitudinal coordinate decimal cannot be less than 90."); }
                if (value < 0) { position = CoordinatesPosition.S; }
                else { position = CoordinatesPosition.N; }
            }
            decimal dd = Convert.ToDecimal(value);
            dd = Math.Abs(dd);
            decimal ddFloor = Math.Floor(dd);//DEGREE
            decimal dm = dd - ddFloor;
            dm *= 60; //DECIMAL MINUTE
            decimal dmFloor = Math.Floor(dm); //MINUTES
            decimal sec = dm - dmFloor;
            sec *= 60;//SECONDS


            decimalDegree = value;
            degrees = Convert.ToInt32(ddFloor);
            minutes = Convert.ToInt32(dmFloor);
            decimalMinute = Convert.ToDouble(dm);
            seconds = Convert.ToDouble(sec);
        }
        /// <summary>
        /// Creates a populated CoordinatePart from a Degrees Minutes Seconds value.
        /// </summary>
        /// <param name="deg">Degrees</param>
        /// <param name="min">Minutes</param>
        /// <param name="sec">Seconds</param>
        /// <param name="pos">Coordinate part position</param>
        /// <example>
        /// The following example creates a populated latitudinal coordinate part from Degrees Minutes Seconds values and assigns it to a Coordinate object.
        /// <code>
        /// //Create a new default coordinate
        /// Coordinate c = new Coordinate();
        /// 
        /// //Create a coordinate part using the Degrees Minutes Seconds latitude N25 36 24.657°.
        /// CoordinatePart cp = new CoordinatePart(25, 36, 24.657, CoordinatesPosition.N);
        /// 
        /// //Assign the latitudinal value to the coordinate.
        /// c.Latitude = cp;
        /// </code>
        /// </example>
        public CoordinatePart(int deg, int min, double sec, CoordinatesPosition pos)
        {
            if (pos == CoordinatesPosition.N || pos == CoordinatesPosition.S) { type = CoordinateType.Lat; }
            else { type = CoordinateType.Long; }

            if (deg < 0) { throw new ArgumentOutOfRangeException("Degrees out of range", "Degrees cannot be less than 0."); }
            if (min < 0) { throw new ArgumentOutOfRangeException("Minutes out of range", "Minutes cannot be less than 0."); }
            if (sec < 0) { throw new ArgumentOutOfRangeException("Seconds out of range", "Seconds cannot be less than 0."); }
            if (min >= 60) { throw new ArgumentOutOfRangeException("Minutes out of range", "Minutes cannot be greater than or equal to 60."); }
            if (sec >= 60) { throw new ArgumentOutOfRangeException("Seconds out of range", "Seconds cannot be greater than or equal to 60."); }
            degrees = deg;
            minutes = min;
            seconds = sec;
            position = pos;

            decimal secD = Convert.ToDecimal(sec);
            secD /= 60; //Decimal Seconds
            decimal minD = Convert.ToDecimal(min);
            minD += secD; //Decimal Minutes

            if (type == CoordinateType.Long)
            {
                if (deg + (minD / 60) > 180) { throw new ArgumentOutOfRangeException("Degrees out of range", "Longitudinal Degrees cannot be greater than 180."); }
            }
            else
            {
                if (deg + (minD / 60) > 90) { throw new ArgumentOutOfRangeException("Degrees out of range", "Latitudinal Degrees cannot be greater than 90."); }
            }
            decimalMinute = Convert.ToDouble(minD);
            decimal dd = Convert.ToDecimal(deg) + (minD / 60);


            if (pos == CoordinatesPosition.S || pos == CoordinatesPosition.W)
            {
                dd *= -1;
            }
            decimalDegree = Convert.ToDouble(dd);
        }
        /// <summary>
        /// Creates a populated CoordinatePart from a Degrees Minutes Seconds part.
        /// </summary>
        /// <param name="deg">Degrees</param>
        /// <param name="minSec">Decimal Minutes</param> 
        /// <param name="pos">Coordinate part position</param>
        /// <example>
        /// The following example creates a populated latitudinal coordinate part from Decimal Degree Minute values and assigns it to a Coordinate object.
        /// <code>
        /// //Create a new default coordinate
        /// Coordinate c = new Coordinate();
        /// 
        /// //Create a coordinate part using the Decimal Degree latitude N25 36.24854°.
        /// CoordinatePart cp = new CoordinatePart(25, 36.24854, CoordinatesPosition.N);
        /// 
        /// //Assign the latitudinal value to the coordinate.
        /// c.Latitude = cp;
        /// </code>
        /// </example>
        public CoordinatePart(int deg, double minSec, CoordinatesPosition pos)
        {
            if (pos == CoordinatesPosition.N || pos == CoordinatesPosition.S) { type = CoordinateType.Lat; }
            else { type = CoordinateType.Long; }

            if (deg < 0) { throw new ArgumentOutOfRangeException("Degree out of range", "Degree cannot be less than 0."); }
            if (minSec < 0) { throw new ArgumentOutOfRangeException("Minutes out of range", "Minutes cannot be less than 0."); }

            if (minSec >= 60) { throw new ArgumentOutOfRangeException("Minutes out of range", "Minutes cannot be greater than or equal to 60."); }

            if (type == CoordinateType.Lat)
            {
                if (deg + (minSec / 60) > 90) { throw new ArgumentOutOfRangeException("Degree out of range", "Latitudinal degrees cannot be greater than 90."); }
            }
            else
            {
                if (deg + (minSec / 60) > 180) { throw new ArgumentOutOfRangeException("Degree out of range", "Longitudinal degrees cannot be greater than 180."); }
            }
            degrees = deg;
            decimalMinute = minSec;
            position = pos;

            decimal minD = Convert.ToDecimal(minSec);
            decimal minFloor = Math.Floor(minD);
            minutes = Convert.ToInt32(minFloor);
            decimal sec = minD - minFloor;
            sec *= 60;
            decimal secD = Convert.ToDecimal(sec);
            seconds = Convert.ToDouble(secD);
            decimal dd = deg + (minD / 60);

            if (pos == CoordinatesPosition.S || pos == CoordinatesPosition.W)
            {
                dd *= -1;
            }
            decimalDegree = Convert.ToDouble(dd);
        }

        /// <summary>
        /// Signed degrees coordinate part.
        /// </summary>
        /// <returns>double</returns>
        public double ToDouble()
        {
            return decimalDegree;
        }

        /// <summary>
        /// Formatted coordinate part string.
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            if (parent == null)
            {
                return FormatString(new CoordinateFormatOptions());
            }
            return FormatString(Parent.FormatOptions);
        }

        /// <summary>
        /// Formatted coordinate part string.
        /// </summary>
        /// <param name="options">CoordinateFormatOptions</param>
        /// <returns>string</returns>
        public string ToString(CoordinateFormatOptions options)
        {
            return FormatString(options);
        }
        /// <summary>
        /// String format settings.
        /// </summary>
        /// <param name="options">CoordinateFormatOptions</param>
        /// <returns>Formatted coordinate part string</returns>
        private string FormatString(CoordinateFormatOptions options)
        {
            ToStringType type = ToStringType.Degree_Minute_Second;
            int? rounding = null;
            bool lead = false;
            bool trail = false;
            bool hyphen = false;
            bool symbols = true;
            bool degreeSymbol = true;
            bool minuteSymbol = true;
            bool secondsSymbol = true;
            bool positionFirst = true;

            #region Assign Formatting Rules
            switch (options.Format)
            {
                case CoordinateFormatType.Degree_Minutes_Seconds:
                    type = ToStringType.Degree_Minute_Second;
                    break;
                case CoordinateFormatType.Degree_Decimal_Minutes:
                    type = ToStringType.Degree_Decimal_Minute;
                    break;
                case CoordinateFormatType.Decimal_Degree:
                    type = ToStringType.Decimal_Degree;
                    break;
                case CoordinateFormatType.Decimal:
                    type = ToStringType.Decimal;
                    break;
                default:
                    type = ToStringType.Degree_Minute_Second;
                    break;
            }
            rounding = options.Round;
            lead = options.Display_Leading_Zeros;
            trail = options.Display_Trailing_Zeros;
            symbols = options.Display_Symbols;
            degreeSymbol = options.Display_Degree_Symbol;
            minuteSymbol = options.Display_Minute_Symbol;
            secondsSymbol = options.Display_Seconds_Symbol;
            hyphen = options.Display_Hyphens;
            positionFirst = options.Position_First;
            #endregion

            switch (type)
            {
                case ToStringType.Decimal_Degree:
                    if (rounding == null) { rounding = 6; }
                    return ToDecimalDegreeString(rounding.Value, lead, trail, symbols, degreeSymbol, positionFirst, hyphen);
                case ToStringType.Degree_Decimal_Minute:
                    if (rounding == null) { rounding = 3; }
                    return ToDegreeDecimalMinuteString(rounding.Value, lead, trail, symbols, degreeSymbol, minuteSymbol, hyphen, positionFirst);
                case ToStringType.Degree_Minute_Second:
                    if (rounding == null) { rounding = 3; }
                    return ToDegreeMinuteSecondString(rounding.Value, lead, trail, symbols, degreeSymbol, minuteSymbol, secondsSymbol, hyphen, positionFirst);
                case ToStringType.Decimal:
                    if (rounding == null) { rounding = 9; }
                    double dub = ToDouble();
                    dub = Math.Round(dub, rounding.Value);
                    string lt = Leading_Trailing_Format(lead, trail, rounding.Value, Position);
                    return string.Format(lt, dub);
            }

            return string.Empty;
        }
        //DMS Coordinate Format
        private string ToDegreeMinuteSecondString(int rounding, bool lead, bool trail, bool symbols, bool degreeSymbol, bool minuteSymbol, bool secondSymbol, bool hyphen, bool positionFirst)
        {

            string leadString = Leading_Trailing_Format(lead, false, rounding, Position);
            string d = string.Format(leadString, Degrees); // Degree String
            string minute;
            if (lead) { minute = string.Format("{0:00}", Minutes); }
            else { minute = Minutes.ToString(); }
            string leadTrail = Leading_Trailing_Format(lead, trail, rounding);

            double sc = Math.Round(Seconds, rounding);
            string second = string.Format(leadTrail, sc);
            string hs = " ";
            string ds = "";
            string ms = "";
            string ss = "";
            if (symbols)
            {
                if (degreeSymbol) { ds = "º"; }
                if (minuteSymbol) { ms = "'"; }
                if (secondSymbol) { ss = "\""; }
            }
            if (hyphen) { hs = "-"; }

            if (positionFirst) { return Position.ToString() + hs + d + ds + hs + minute + ms + hs + second + ss; }
            else { return d + ds + hs + minute + ms + hs + second + ss + hs + Position.ToString(); }
        }
        //DDM Coordinate Format
        private string ToDegreeDecimalMinuteString(int rounding, bool lead, bool trail, bool symbols, bool degreeSymbol, bool minuteSymbol, bool hyphen, bool positionFirst)
        {
            string leadString = "{0:0";
            if (lead)
            {
                if (Position == CoordinatesPosition.E || Position == CoordinatesPosition.W)
                {
                    leadString += "00";
                }
                else
                {
                    leadString += "0";
                }
            }
            leadString += "}";
            string d = string.Format(leadString, Degrees); // Degree String

            string leadTrail = "{0:0";
            if (lead)
            {
                leadTrail += "0";
            }
            leadTrail += ".";
            if (trail)
            {
                for (int i = 0; i < rounding; i++)
                {
                    leadTrail += "0";
                }
            }
            else
            {
                leadTrail += "#########";
            }
            leadTrail += "}";

            double ns = Seconds / 60;
            double c = Math.Round(Minutes + ns, rounding);
            if (c == 60 && Degrees + 1 < 91) { c = 0; d = string.Format(leadString, Degrees + 1); }//Adjust for rounded maxed out Seconds. will Convert 42 60.0 to 43
            string ms = string.Format(leadTrail, c);
            string hs = " ";
            string ds = "";
            string ss = "";
            if (symbols)
            {
                if (degreeSymbol) { ds = "º"; }
                if (minuteSymbol) { ss = "'"; }
            }
            if (hyphen) { hs = "-"; }

            if (positionFirst) { return Position.ToString() + hs + d + ds + hs + ms + ss; }
            else { return d + ds + hs + ms + ss + hs + Position.ToString(); }

        }
        ////DD Coordinate Format
        private string ToDecimalDegreeString(int rounding, bool lead, bool trail, bool symbols, bool degreeSymbol, bool positionFirst, bool hyphen)
        {
            string degreeS = "";
            string hyph = " ";
            if (degreeSymbol) { degreeS = "º"; }
            if (!symbols) { degreeS = ""; }
            if (hyphen) { hyph = "-"; }

            string leadTrail = "{0:0";
            if (lead)
            {
                if (Position == CoordinatesPosition.E || Position == CoordinatesPosition.W)
                {
                    leadTrail += "00";
                }
                else
                {
                    leadTrail += "0";
                }
            }
            leadTrail += ".";
            if (trail)
            {
                for (int i = 0; i < rounding; i++)
                {
                    leadTrail += "0";
                }
            }
            else
            {
                leadTrail += "#########";
            }
            leadTrail += "}";

            double result = (Degrees) + (Convert.ToDouble(Minutes)) / 60 + (Convert.ToDouble(Seconds)) / 3600;
            result = Math.Round(result, rounding);
            string d = string.Format(leadTrail, Math.Abs(result));
            if (positionFirst) { return Position.ToString() + hyph + d + degreeS; }
            else { return d + degreeS + hyph + Position.ToString(); }

        }

        private string Leading_Trailing_Format(bool isLead, bool isTrail, int rounding, CoordinatesPosition? pos = null)
        {
            string leadString = "{0:0";
            if (isLead)
            {
                if (pos != null)
                {
                    if (pos.Value == CoordinatesPosition.W || pos.Value == CoordinatesPosition.E)
                    {
                        leadString += "00";
                    }
                }
                else
                {
                    leadString += "0";
                }
            }

            leadString += ".";
            if (isTrail)
            {
                for (int i = 0; i < rounding; i++)
                {
                    leadString += "0";
                }
            }
            else
            {
                leadString += "#########";
            }

            leadString += "}";
            return leadString;

        }

        private string FormatError(string argument, string rule)
        {
            return "'" + argument + "' is not a valid argument for string format rule: " + rule + ".";
        }

        /// <summary>
        /// Returns Coordinate part in radians.
        /// </summary>
        /// <example>
        /// The following example demonstrates how to get the radian values of the geodetic coordinate parts.
        /// <code>
        /// //Create a new coordinate at N25, E45
        /// Coordinate c = new Coordinate(25,45);
        /// 
        /// //Get the radian values of each part
        /// Console.WriteLine(c.Latitude.ToRadians()); //0.436332312998582
        /// Console.WriteLine(c.Longitude.ToRadians()); //0.785398163397448
        /// </code>
        /// </example>
        /// <returns>double</returns>
        public double ToRadians()
        {
            return decimalDegree * Math.PI / 180;
        }
        /// <summary>
        /// Attempts to parse a string into a CoordinatePart.
        /// </summary>
        /// <param name="value">CoordinatePart string</param>
        /// <param name="coordinatePart">CoordinatePart object to populate</param>
        /// <returns>boolean</returns>
        /// <example>
        /// The following example demonstrates how to parse a latitude from a string.
        /// <code>
        /// CoordinatePart cp;
        /// if(CoordinatePart.TryParse("N 32.891º", out cp))
        /// {
        ///     Console.WriteLine(cp); //N 32º 53' 28.212"
        /// }
        /// </code>
        /// </example>
        public static bool TryParse(string value, out CoordinatePart coordinatePart)
        {
            coordinatePart = null;

            if (FormatFinder_CoordPart.TryParse(value, out coordinatePart))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Attempts to parse a string into a CoordinatePart. 
        /// </summary>
        /// <param name="value">CoordinatePart string</param>
        /// <param name="cType">CoordinateType</param>
        /// <param name="coordinatePart">CoordinatePart object to populate</param>
        /// <returns>boolean</returns>
        /// <example>
        /// The following example demonstrates how to parse a latitude from a string.
        /// <code>
        /// CoordinatePart cp;
        /// if(CoordinatePart.TryParse("-32.891º", CoordinateType.Long, out cp))
        /// {
        ///     Console.WriteLine(cp); //W 32º 53' 27.6"
        /// }
        /// </code>
        /// </example>
        public static bool TryParse(string value, CoordinateType cType, out CoordinatePart coordinatePart)
        {
            coordinatePart = null;
            //Comma at beginning parses to long
            //Asterisk forces lat
            if (cType == CoordinateType.Long) { value = "," + value; }
            else { value = "*" + value; }
            if (FormatFinder_CoordPart.TryParse(value, out coordinatePart))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Property changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Notifies the correct properties and parent properties.
        /// </summary>
        /// <param name="props">Property Type</param>
        private void NotifyProperties(PropertyTypes props)
        {
            switch (props)
            {
                case PropertyTypes.DecimalDegree:
                    NotifyPropertyChanged("DecimalDegree");
                    NotifyPropertyChanged("DecimalMinute");
                    NotifyPropertyChanged("Degrees");
                    NotifyPropertyChanged("Minutes");
                    NotifyPropertyChanged("Seconds");
                    NotifyPropertyChanged("Position");
                    break;
                case PropertyTypes.DecimalMinute:
                    NotifyPropertyChanged("DecimalDegree");
                    NotifyPropertyChanged("DecimalMinute");
                    NotifyPropertyChanged("Minutes");
                    NotifyPropertyChanged("Seconds");
                    break;
                case PropertyTypes.Degree:
                    NotifyPropertyChanged("DecimalDegree");
                    NotifyPropertyChanged("Degree");
                    break;
                case PropertyTypes.Minute:
                    NotifyPropertyChanged("DecimalDegree");
                    NotifyPropertyChanged("DecimalMinute");
                    NotifyPropertyChanged("Minutes");
                    break;
                case PropertyTypes.Position:
                    NotifyPropertyChanged("DecimalDegree");
                    NotifyPropertyChanged("Position");
                    break;
                case PropertyTypes.Second:
                    NotifyPropertyChanged("DecimalDegree");
                    NotifyPropertyChanged("DecimalMinute");
                    NotifyPropertyChanged("Seconds");
                    break;
                default:
                    NotifyPropertyChanged("DecimalDegree");
                    NotifyPropertyChanged("DecimalMinute");
                    NotifyPropertyChanged("Degrees");
                    NotifyPropertyChanged("Minutes");
                    NotifyPropertyChanged("Seconds");
                    NotifyPropertyChanged("Position");
                    break;
            }
            NotifyPropertyChanged("Display");

            if (Parent != null)
            {
                Parent.NotifyPropertyChanged("Display");
                Parent.NotifyPropertyChanged("CelestialInfo");
                Parent.NotifyPropertyChanged("UTM");
                Parent.NotifyPropertyChanged("MGRS");
                Parent.NotifyPropertyChanged("Cartesian");
                Parent.NotifyPropertyChanged("ECEF");
            }

        }

        /// <summary>
        /// Notify property changes
        /// </summary>        
        /// <param name="propName">Property name</param>
        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

    }
}
