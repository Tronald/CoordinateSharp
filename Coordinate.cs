//SUMMARY
//
//(C) 2017, Justin Gielski
// 
// CoordinateSharp is a library designed to make the formatting of Latitudinal/Longitudinal geographic coordinates simple. 
// It provides methods for easy conversion of various coordinate formats. 
//
// This library contains observable classes so that bindings / MVVM patterns may established.
//
// Refer to documentation for usage instructions
//
// EXTRAS:
//
// A feature has been added to calculate sun / moon - rise / set times & moon illumination based on coordinate position and provided geodate.
//
// SunTime calculations were adapted from NOAA and Zacky Pickholz 2008 "C# Class for Calculating Sunrise and Sunset Times" 
// NOAA: https://www.esrl.noaa.gov/gmd/grad/solcalc/main.js
// The Zacky Pickholz project can be found here https://www.codeproject.com/Articles/29306/C-Class-for-Calculating-Sunrise-and-Sunset-Times
//
// MoonTime calculations were adapted from the mourner / suncalc project (c) 2011-2015, Vladimir Agafonkin
// suncalc: https://github.com/mourner/suncalc/blob/master/suncalc.js
// suncalc's moon calculations are based on "Astronomical Algorithms" 2nd edition by Jean Meeus (Willmann-Bell, Richmond) 1998.
//           & http://aa.quae.nl/en/reken/hemelpositie.html formulas

// calculations for illumination parameters of the moon,
// based on http://idlastro.gsfc.nasa.gov/ftp/pro/astro/mphase.pro formulas and
// Chapter 48 of "Astronomical Algorithms" 2nd edition by Jean Meeus (Willmann-Bell, Richmond) 1998.
// 
// DO NOT USE celestial calculations contained in this library if hyper accuracy is required. Based on testing, times and illuminations are "ball park" and may be off by up to a few minutes. 
// Times may become less accurate near the poles of the earth.
//
//////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;

namespace CoordinateSharp
{   
    /// <summary>
    /// Observable class for handling Lat/Long coordinates
    /// </summary>
    public class Coordinate : INotifyPropertyChanged
    {     
        /// <summary>
        /// Creates an empty Coordinates object. Values will need to be provided to Latitude/Longitude manually.
        /// </summary>
        public Coordinate()
        {
            latitude = new CoordinatePart(CoordinateType.Lat, this);
            longitude = new CoordinatePart(CoordinateType.Long, this);
            celestialInfo = new Celestial();
        }
        /// <summary>
        /// Creates a populated Coordinate object.
        /// </summary>
        /// <param name="lat">Decimal format latitude</param>
        /// <param name="longi">Decimal format longitude</param>
        public Coordinate(double lat, double longi)
        {
            latitude = new CoordinatePart(lat, CoordinateType.Lat, this);
            longitude = new CoordinatePart(longi, CoordinateType.Long, this);
            celestialInfo = new Celestial(lat,longi,new DateTime(1900,1,1));
        }
        /// <summary>
        /// Creates a populated Coordinate object. With an assigned GeoDate.
        /// </summary>
        /// <param name="lat">Decimal format latitude</param>
        /// <param name="longi">Decimal format longitude</param>
        /// <param name="date">DateTime you wish to use for celestial calculation</param>
        public Coordinate(double lat, double longi, DateTime date)
        {
            latitude = new CoordinatePart(lat, CoordinateType.Lat, this);
            longitude = new CoordinatePart(longi, CoordinateType.Long, this);
            celestialInfo = new Celestial(lat, longi, date);            
            this.geoDate = date;                        
        }

        private CoordinatePart latitude;
        private CoordinatePart longitude;
      
        private DateTime geoDate;
        private Celestial celestialInfo;
        /// <summary>
        /// Latitudinal Coordinate Part.
        /// </summary>
        public CoordinatePart Latitude
        {
            get { return this.latitude; }
            set
            {
                if (this.latitude != value)
                {
                    if (value.Position == CoordinatesPosition.E || value.Position == CoordinatesPosition.W)
                    { throw new ArgumentException("Invalid Position", "Latitudinal positions cannot be set to East or West."); }
                    this.latitude = value;
                    this.NotifyPropertyChanged("Latitude");
                    celestialInfo.CalculateCelestialTime(this.Latitude.DecimalDegree, this.Longitude.DecimalDegree, this.geoDate);
                    this.NotifyPropertyChanged("CelestialInfo");
                }
            }
        }
        /// <summary>
        /// Longitudinal Coordinate Part.
        /// </summary>
        public CoordinatePart Longitude 
        {
            get { return this.longitude; }
            set
            {
                if (this.longitude != value)
                {
                    if (value.Position == CoordinatesPosition.N || value.Position == CoordinatesPosition.S)
                    { throw new ArgumentException("Invalid Position", "Longitudinal positions cannot be set to North or South."); }
                    this.longitude = value;
                    this.NotifyPropertyChanged("Longitude");
                    celestialInfo.CalculateCelestialTime(this.Latitude.DecimalDegree, this.Longitude.DecimalDegree, this.geoDate);
                    this.NotifyPropertyChanged("CelestialInfo");
                }
            }
        }     
        /// <summary>
        /// Date for with to calculate celestial information. Assumes all times are in UTC.
        /// </summary>
        public DateTime GeoDate
        {
            get { return this.geoDate; }
            set
            {
                if (this.geoDate != value)
                {
                    this.geoDate = value;
                    this.NotifyPropertyChanged("GeoDate");
                    celestialInfo.CalculateCelestialTime(this.Latitude.DecimalDegree, this.Longitude.DecimalDegree, this.geoDate);
                    this.NotifyPropertyChanged("CelestialInfo");                 
                }
            }
        }
        
        /// <summary>
        /// Celestial information based on the objects lat/long and geo date.
        /// </summary>
        public Celestial CelestialInfo
        {
            get { return this.celestialInfo; }          
        }

        /// <summary>
        /// Overridden Coordinate ToString() method
        /// </summary>
        /// <returns>Degree Minute Seconds formated coordinate. Seconds round to the 3rd</returns>
        public override string ToString()
        {
            string latString = latitude.ToString();
            string longSting = longitude.ToString();
            return latString + " " + longSting;
        }
        /// <summary>
        /// Overridden Coordinate ToString() method that accepts formatting. 
        /// Refer to documentation for coordinate format options
        /// </summary>
        /// <param name="format">Format string</param>
        /// <returns>Custom formatted coordinate</returns>
        public string ToString(string format)
        {
            string latString = latitude.ToString(format.ToUpper());
            string longSting = longitude.ToString(format.ToUpper());
            return latString + " " + longSting;         
        }
        /// <summary>
        /// Overridden Coordinate ToString() method that accepts formatting for XAML Binding. 
        /// Refer to documentation for coordinate format options
        /// </summary>
        /// <param name="format">Format string</param>
        /// <param name="formatProvider">Fromat Provider</param>
        /// <returns>Custom formatted coordinate</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            string latString = latitude.ToString(format.ToUpper());
            string longSting = longitude.ToString(format.ToUpper());
            return latString + " " + longSting;
        }
              
        /// <summary>
        /// Property changed event handler.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Notifies Coordinate property of changing.
        /// </summary>
        /// <param name="propName"></param>
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
                if (propName != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
                }
            }
        }   
        /// <summary>
        /// Property changed event to be fired from the Coordinate subclass.
        /// </summary>
        /// <param name="type">Coordinate type</param>
        public void NotifyPropertyChanged(CoordinateType type)
        {
            if (this.PropertyChanged != null)
            {
                string propName = string.Empty;
                if (type == CoordinateType.Lat) { propName = "Latitude"; }
                if (type == CoordinateType.Long) { propName = "Longitude"; }
                if (propName == string.Empty) { return; }
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
                if (propName != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
                    this.celestialInfo.CalculateCelestialTime(this.latitude.DecimalDegree, this.longitude.DecimalDegree, this.geoDate);
                    this.NotifyPropertyChanged("CelestialInfo");
                }
            }
        }
    }
    /// <summary>
    /// Observable class for handling a single coordinate (Lat or Long)
    /// </summary>
    public class CoordinatePart : IFormattable, INotifyPropertyChanged
    {
        //Format Example String "FS:R4:LT:TT:MF:HT" = N-075º-40'-35.3645"

        //Format rules are passed in string format with each rule being seperated by a colon
        //Format rules can be passed in any order
        //Format rules are 2 characters long. The first character declares the rule type, and the second declares a value.
        //All rules contain default values. Consider only including a rule in your format string when a default is being overridden

        //Rules:
        //F(D,M,S,C) = Format. Format values must be either 'D' (Decimal Degree) 'M' (Degree Decimal Minute) 'S' (Degree Minute Second) 'C' (Decimal): ex. 'FS' = N 70º 40' 56.678"
        //R(0-9) = Rounding. Rounding values may be 0-9. Any decimals will be rounded to the declared digit. ex. 70.635473 with 'R3' = 70.635
        //L(T,F) = Leading Zeros. Leading zeros may be set 'T' (true) or 'F' (false). ex. W 70.645 with 'LT' = 070.645
        //T(T,F) = Trailing Zeros. Trailing zeros may be set 'T' (true) or 'F' (false). Will only trail to the specified Rounding rule. ex 70.746 with 'R5' & 'TT' = 70.74600
        //B(T,F) = Display Symbols. Display symbols may be set 'T' (true) or 'F' (false). Will hide degree, minute, seconds symbols if false
        //D(T,F) = Degree Symbol. May be set 'T' (true) or 'F' (false). May only be set when displaying symbols. Will hide degree symbol if false
        //M(T,F) = Minute Symbol. May be set 'T' (true) or 'F' (false). May only be set when displaying symbols. Will hide minute symbol if false
        //S(T,F) = Second Symbol. May be set 'T' (true) or 'F' (false). May only be set when displaying symbols. Will hide seconds symbol if false
        //H(T,F) = Display Hyphens. May be set 'T' (true) or 'F' (false). Will display hyphens between degrees, minute, seconds if set true.
        //P(F,L) = Position Display. May be set to 'F' (first) or 'L' (last). Will display postion letter where disired

        //Defaults:
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

        /// <summary>
        /// Used to determine and notify the Coordinate Parts parent.
        /// </summary>
        public Coordinate Parent { get; set; }

        /// <summary>
        /// Observable decimal format coordinate
        /// </summary>
        public double DecimalDegree
        {
            get { return this.decimalDegree; }
            set
            {
                //If changing, notify the needed property changes
                if (this.decimalDegree != value)
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
                    this.decimalDegree = value;
                    this.NotifyPropertyChanged("DecimalDegree");
                    //Update Position
                    if ((this.position == CoordinatesPosition.N || this.position == CoordinatesPosition.E) && this.decimalDegree < 0)
                    {
                        if (this.type == CoordinateType.Lat) { this.position = CoordinatesPosition.S; }
                        else { this.position = CoordinatesPosition.W; }
                        this.NotifyPropertyChanged("Position");
                    }
                    if ((this.position == CoordinatesPosition.W || this.position == CoordinatesPosition.S) && this.decimalDegree >= 0)
                    {
                        if (this.type == CoordinateType.Lat) { this.position = CoordinatesPosition.N; }
                        else { this.position = CoordinatesPosition.E; }
                        this.NotifyPropertyChanged("Position");
                    }
                    //Update the Degree & Decimal Minute
                    double degABS = Math.Abs(this.decimalDegree); //Make decimalDegree positive for calculations
                    double degFloor = Math.Truncate(degABS); //Truncate the number leftto extract the degree
                    decimal f = Convert.ToDecimal(degFloor); //Convert to degree to decimal to keep precision during calculations
                    decimal ddm = Convert.ToDecimal(degABS) - f; //Extract decimalMinute value from decimalDegree
                    ddm *= 60; //Multiply by 60 to get readable decimalMinute

                    double dm = Convert.ToDouble(ddm); //Convert decimalMinutes back to double for storage
                    int df = Convert.ToInt32(degFloor); //Convert degrees to int for storage

                    if (this.degrees != df)
                    {
                        this.degrees = df;
                        this.NotifyPropertyChanged("Degrees");
                    }
                    if (this.decimalMinute != dm)
                    {
                        this.decimalMinute = dm;
                        this.NotifyPropertyChanged("DecimalMinutes");
                    }
                    //Update Minutes Seconds              
                    double dmFloor = Math.Floor(dm); //Get number left of decimal to grab minute value
                    int mF = Convert.ToInt32(dmFloor); //Convert minute to int for storage
                    f = Convert.ToDecimal(dmFloor); //Create a second minute value and store as decimal for precise calculation

                    decimal s = ddm - f; //Get seconds from minutes
                    s *= 60; //Multiply by 60 to get readable seconds
                    double secs = Convert.ToDouble(s); //Convert back to double for storage

                    if (this.minutes != mF)
                    {
                        this.minutes = mF;
                        this.NotifyPropertyChanged("Minutes");
                    }
                    if (this.seconds != secs)
                    {
                        this.seconds = secs;
                        this.NotifyPropertyChanged("Seconds");
                    }
                    //Notify Parent class of change
                    if (type == CoordinateType.Lat)
                    {
                        this.Parent.NotifyPropertyChanged(CoordinateType.Lat);
                    }
                    if (type == CoordinateType.Long)
                    {
                        this.Parent.NotifyPropertyChanged(CoordinateType.Long);
                    }
                }
            }
        }
        /// <summary>
        /// Observable decimal format minute
        /// </summary>
        public double DecimalMinute
        {
            get { return this.decimalMinute; }
            set
            {
                if (this.decimalMinute != value)
                {
                    //Validate values        
                    decimal dm = Math.Abs(Convert.ToDecimal(value)) / 60;
                    double decMin = Convert.ToDouble(dm);
                    if (this.type == CoordinateType.Lat)
                    {

                        if (this.degrees + decMin > 90) { throw new ArgumentOutOfRangeException("Degrees out of range", "Latitudinal degrees cannot be greater than 90"); }
                    }
                    else
                    {
                        if (this.degrees + decMin > 180) { throw new ArgumentOutOfRangeException("Degrees out of range", "Longitudinal degrees cannot be greater than 180"); }
                    }
                    if (value >= 60) { throw new ArgumentOutOfRangeException("Minutes out of range", "Coordinate Minutes cannot be greater than or equal to 60"); }
                    if (value < 0) { throw new ArgumentOutOfRangeException("Minutes out of range", "Coordinate Minutes cannot be less than 0"); }


                    this.decimalMinute = value;
                    this.NotifyPropertyChanged("DecimalMinute");

                    decimal decValue = Convert.ToDecimal(value); //Convert value to decimal for precision during calculation
                    decimal dmFloor = Math.Floor(decValue); //Extract minutes
                    decimal secs = decValue - dmFloor; //Extract seconds
                    secs *= 60; //Convert seconds to human readable format

                    decimal newDM = decValue / 60; //divide decimalMinute by 60 to get storage value
                    decimal newDD = this.degrees + newDM;//Add new decimal value to the floor degree value to get new decimalDegree;
                    if (this.decimalDegree < 0) { newDD = newDD * -1; } //Restore negative if needed

                    this.decimalDegree = Convert.ToDouble(newDD);  //Convert back to double for storage                      
                    this.NotifyPropertyChanged("DecimalDegree");

                    this.minutes = Convert.ToInt32(dmFloor); //Convert minutes to int for storage
                    this.NotifyPropertyChanged("Minutes");
                    this.seconds = Convert.ToDouble(secs); //Convert seconds to double for storage
                    this.NotifyPropertyChanged("Seconds");
                    //Notify parent of change
                    if (type == CoordinateType.Lat)
                    {
                        this.Parent.NotifyPropertyChanged(CoordinateType.Lat);
                    }
                    if (type == CoordinateType.Long)
                    {
                        this.Parent.NotifyPropertyChanged(CoordinateType.Long);
                    }
                }
            }

        }
        /// <summary>
        /// Observable coordinate degree
        /// </summary>
        public int Degrees
        {
            get { return this.degrees; }
            set
            {
                //Validate Value
                if (this.degrees != value)
                {
                    if (type == CoordinateType.Lat)
                    {
                        if (value + this.decimalMinute > 90)
                        {
                            throw new ArgumentOutOfRangeException("Degrees", "Latitude degrees cannot be greater than 90");
                        }
                    }
                    if (type == CoordinateType.Long)
                    {
                        if (value + this.decimalMinute > 180)
                        {
                            throw new ArgumentOutOfRangeException("Degrees", "Longitude degrees cannot be greater than 180");
                        }

                    }
                    decimal f = Convert.ToDecimal(this.degrees);

                    this.degrees = value;
                    this.NotifyPropertyChanged("Degrees");

                    double degABS = Math.Abs(this.decimalDegree); //Make decimalDegree positive for calculations
                    decimal dDec = Convert.ToDecimal(degABS); //Convert to Decimal for precision during calculations              
                    //Convert degrees to decimal to keep precision        
                    decimal dm = dDec - f; //Extract minutes                                      
                    decimal newDD = this.degrees + dm; //Add minutes to new degree for decimalDegree

                    if (this.decimalDegree < 0) { newDD *= -1; } //Set negative as required

                    this.decimalDegree = Convert.ToDouble(newDD); // Convert decimalDegree to double for storage

                    this.NotifyPropertyChanged("DecimalDegree");

                    //Notify Parent Property
                    if (type == CoordinateType.Lat)
                    {
                        this.Parent.NotifyPropertyChanged(CoordinateType.Lat);
                    }
                    if (type == CoordinateType.Long)
                    {
                        this.Parent.NotifyPropertyChanged(CoordinateType.Long);
                    }
                }
            }
        }
        /// <summary>
        /// Observable coordinate minute
        /// </summary>
        public int Minutes
        {
            get { return this.minutes; }
            set
            {
                if (this.minutes != value)
                {
                    //Validate the minutes
                    decimal vMin = Convert.ToDecimal(value);
                    if (type == CoordinateType.Lat)
                    {
                        if (this.degrees + (vMin / 60) > 90) { throw new ArgumentOutOfRangeException("Degrees out of range", "Latitudinal degrees cannot be greater than 90"); }
                    }
                    else
                    {
                        if (this.degrees + (vMin / 60) > 180) { throw new ArgumentOutOfRangeException("Degrees out of range", "Longitudinal degrees cannot be greater than 180"); }
                    }
                    if (value >= 60)
                    {
                        throw new ArgumentOutOfRangeException("Minutes out of range", "Minutes cannot be greater than or equal to 60");
                    }
                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException("Minutes out of range", "Minutes cannot be less than 0");
                    }
                    decimal minFloor = Convert.ToDecimal(this.minutes);//Convert decimal to minutes for calculation
                    decimal f = Convert.ToDecimal(this.degrees); //Convert to degree to keep precision during calculation 

                    this.minutes = value;
                    this.NotifyPropertyChanged("Minutes");

                    double degABS = Math.Abs(this.decimalDegree); //Make decimalDegree positive
                    decimal dDec = Convert.ToDecimal(degABS); //Convert to decimalDegree for precision during calucation                        

                    decimal dm = dDec - f; //Extract minutes
                    dm *= 60; //Make minutes human readable

                    decimal secs = dm - minFloor;//Extract Seconds

                    decimal newDM = this.minutes + secs;//Add seconds to minutes for decimalMinute
                    double decMin = Convert.ToDouble(newDM); //Convert decimalMinute to double for storage
                    this.decimalMinute = decMin; //Round to correct precision
                    this.NotifyPropertyChanged("DecimalMinute");

                    newDM /= 60; //Convert decimalMinute to storage format
                    decimal newDeg = f + newDM; //Add value to degree for decimalDegree
                    if (this.decimalDegree < 0) { newDeg *= -1; }// Set to negative as required.
                    this.decimalDegree = Convert.ToDouble(newDeg);//Convert to double and roun to correct precision for storage
                    this.NotifyPropertyChanged("DecimalDegree");

                    //Notify Parent Property
                    if (type == CoordinateType.Lat)
                    {
                        this.Parent.NotifyPropertyChanged(CoordinateType.Lat);
                    }
                    if (type == CoordinateType.Long)
                    {
                        this.Parent.NotifyPropertyChanged(CoordinateType.Long);
                    }
                }
            }
        }
        /// <summary>
        /// Observable coordinate second
        /// </summary>
        public double Seconds
        {
            get { return this.seconds; }
            set
            {
                if (this.seconds != value)
                {
                    //Validate Seconds
                    decimal vSec = Convert.ToDecimal(value);
                    vSec /= 60;

                    decimal vMin = Convert.ToDecimal(this.minutes);
                    vMin += vSec;
                    vMin /= 60;

                    if (type == CoordinateType.Lat)
                    {
                        if (this.degrees + vMin > 90) { throw new ArgumentOutOfRangeException("Degrees out of range", "Latitudinal degrees cannot be greater than 90"); }
                    }
                    else
                    {
                        if (this.degrees + vMin > 180) { throw new ArgumentOutOfRangeException("Degrees out of range", "Longitudinal degrees cannot be greater than 180"); }
                    }
                    if (value >= 60)
                    {
                        throw new ArgumentOutOfRangeException("Seconds out of range", "Seconds cannot be greater than or equal to 60");
                    }
                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException("Seconds out of range", "Seconds cannot be less than 0");
                    }
                    this.seconds = value;
                    this.NotifyPropertyChanged("Seconds");

                    double degABS = Math.Abs(this.decimalDegree); //Make decimalDegree positive
                    double degFloor = Math.Truncate(degABS); //Truncate the number left of the decimal
                    decimal f = Convert.ToDecimal(degFloor); //Convert to decimal to keep precision

                    decimal secs = Convert.ToDecimal(this.seconds); //Convert seconds to decimal for calculations
                    secs /= 60; //Convert to storage format
                    decimal dm = this.minutes + secs;//Add seconds to minutes for decimalMinute
                    double minFD = Convert.ToDouble(dm); //Convert decimalMinute for storage
                    this.decimalMinute = minFD;//Round to proper precision
                    this.NotifyPropertyChanged("DecimalMinute");

                    decimal nm = Convert.ToDecimal(this.decimalMinute) / 60;//Convert decimalMinute to decimal and divide by 60 to get storage format decimalMinute
                    double newDeg = this.degrees + Convert.ToDouble(nm);//Convert to double and add to degree for storage decimalDegree
                    if (this.decimalDegree < 0) { newDeg *= -1; }//Make negative as needed
                    this.decimalDegree = newDeg;//Update decimalDegree and round to proper precision
                    this.NotifyPropertyChanged("DecimalDegree");

                    //Notify Parent Property
                    if (type == CoordinateType.Lat)
                    {
                        this.Parent.NotifyPropertyChanged(CoordinateType.Lat);
                    }
                    if (type == CoordinateType.Long)
                    {
                        this.Parent.NotifyPropertyChanged(CoordinateType.Long);
                    }
                }
            }
        }

        /// <summary>
        /// Observable coordinate position
        /// </summary>
        public CoordinatesPosition Position
        {
            get { return this.position; }
            set
            {
                if (this.position != value)
                {
                    if (type == CoordinateType.Long && (value == CoordinatesPosition.N || value == CoordinatesPosition.S))
                    {
                        throw new InvalidOperationException("You cannot change a Longitudinal type coordinate into a Latitudinal");
                    }
                    if (type == CoordinateType.Lat && (value == CoordinatesPosition.E || value == CoordinatesPosition.W))
                    {
                        throw new InvalidOperationException("You cannot change a Latitudinal type coordinate into a Longitudinal");
                    }
                    this.decimalDegree *= -1; // Change the position
                    this.position = value;
                    this.NotifyPropertyChanged("DecimalDegree");
                    this.NotifyPropertyChanged("Position");

                }
            }
        }

        /// <summary>
        /// Creates an empty Coordinate object
        /// </summary>
        /// <param name="t">Parent Coordinates object</param>
        /// <param name="c">Parent Coordinates object</param>
        public CoordinatePart(CoordinateType t, Coordinate c)
        {
            this.Parent = c;
            this.type = t;
            if (this.type == CoordinateType.Lat) { this.position = CoordinatesPosition.N; }
            else { this.position = CoordinatesPosition.E; }
        }
        /// <summary>
        /// Creates a populated Coordinate object from a decimal format coordinate
        /// </summary>
        /// <param name="value">Coordinate decimal value</param>
        /// <param name="t">Coordinate type</param>
        /// <param name="c">Parent Coordinates object</param>
        public CoordinatePart(double value, CoordinateType t, Coordinate c)
        {
            this.Parent = c;
            this.type = t;

            if (type == CoordinateType.Long)
            {
                if (value > 180) { throw new ArgumentOutOfRangeException("Degrees out of range", "Longitudinal coordinate decimal cannot be greater than 180."); }
                if (value < -180) { throw new ArgumentOutOfRangeException("Degrees out of range", "Longitudinal coordinate decimal cannot be less than 180."); }
                if (value < 0) { this.position = CoordinatesPosition.W; }
                else { this.position = CoordinatesPosition.E; }
            }
            else
            {
                if (value > 90) { throw new ArgumentOutOfRangeException("Degrees out of range", "Latitudinal coordinate decimal cannot be greater than 90."); }
                if (value < -90) { throw new ArgumentOutOfRangeException("Degrees out of range", "Latitudinal coordinate decimal cannot be less than 90."); }
                if (value < 0) { this.position = CoordinatesPosition.S; }
                else { this.position = CoordinatesPosition.N; }
            }
            decimal dd = Convert.ToDecimal(value);
            dd = Math.Abs(dd);
            decimal ddFloor = Math.Floor(dd);//DEGREE
            decimal dm = dd - ddFloor;
            dm *= 60; //DECIMAL MINUTE
            decimal dmFloor = Math.Floor(dm); //MINUTES
            decimal sec = dm - dmFloor;
            sec *= 60;//SECONDS


            this.decimalDegree = value;
            this.degrees = Convert.ToInt32(ddFloor);
            this.minutes = Convert.ToInt32(dmFloor);
            this.decimalMinute = Convert.ToDouble(dm);
            this.seconds = Convert.ToDouble(sec);
        }
        /// <summary>
        /// Creates a populated Coordinate object from a Degrees Minutes Seconds coordinate
        /// </summary>
        /// <param name="deg">Degrees</param>
        /// <param name="min">Minutes</param>
        /// <param name="sec">Seconds</param>
        /// <param name="pos">Coordinate Part Position</param>
        /// <param name="c">Parent Coordinate</param>
        public CoordinatePart(int deg, int min, double sec, CoordinatesPosition pos, Coordinate c)
        {
            this.Parent = c;
            if (pos == CoordinatesPosition.N || pos == CoordinatesPosition.S) { this.type = CoordinateType.Lat; }
            else { this.type = CoordinateType.Long; }

            if (deg < 0) { throw new ArgumentOutOfRangeException("Degrees out of range", "Degrees cannot be less than 0."); }
            if (min < 0) { throw new ArgumentOutOfRangeException("Minutes out of range", "Minutes cannot be less than 0."); }
            if (sec < 0) { throw new ArgumentOutOfRangeException("Seconds out of range", "Seconds cannot be less than 0."); }
            if (min >= 60) { throw new ArgumentOutOfRangeException("Minutes out of range", "Minutes cannot be greater than or equal to 60."); }
            if (sec >= 60) { throw new ArgumentOutOfRangeException("Seconds out of range", "Seconds cannot be greater than or equal to 60."); }
            this.degrees = deg;
            this.minutes = min;
            this.seconds = sec;
            this.position = pos;

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
            this.decimalMinute = Convert.ToDouble(minD);
            decimal dd = Convert.ToDecimal(deg) + (minD / 60);


            if (pos == CoordinatesPosition.S || pos == CoordinatesPosition.W)
            {
                dd *= -1;
            }
            this.decimalDegree = Convert.ToDouble(dd);
        }
        /// <summary>
        /// Creates a populated Coordinate object from a Degrees Minutes Seconds coordinate
        /// </summary>
        /// <param name="deg">Degrees</param>
        /// <param name="minSec">Decimal Minutes</param> 
        /// <param name="pos">Coordinate Part Position</param>
        /// <param name="c">Parent Coordinate object</param>
        public CoordinatePart(int deg, double minSec, CoordinatesPosition pos, Coordinate c)
        {
            this.Parent = c;
            if (pos == CoordinatesPosition.N || pos == CoordinatesPosition.S) { this.type = CoordinateType.Lat; }
            else { this.type = CoordinateType.Long; }

            if (deg < 0) { throw new ArgumentOutOfRangeException("Degree out of range", "Degree cannot be less than 0."); }
            if (minSec < 0) { throw new ArgumentOutOfRangeException("Minutes out of range", "Minutes cannot be less than 0."); }

            if (minSec >= 60) { throw new ArgumentOutOfRangeException("Minutes out of range", "Minutes cannot be greater than or equal to 60."); }

            if (this.type == CoordinateType.Lat)
            {
                if (deg + (minSec / 60) > 90) { throw new ArgumentOutOfRangeException("Degree out of range", "Latitudinal degrees cannot be greater than 90."); }
            }
            else
            {
                if (deg + (minSec / 60) > 90) { throw new ArgumentOutOfRangeException("Degree out of range", "Longitudinal degrees cannot be greater than 180."); }
            }
            this.degrees = deg;
            this.decimalMinute = minSec;
            this.position = pos;

            decimal minD = Convert.ToDecimal(minSec);
            decimal minFloor = Math.Floor(minD);
            this.minutes = Convert.ToInt32(minFloor);
            decimal sec = minD - minFloor;
            sec *= 60;
            decimal secD = Convert.ToDecimal(sec);
            this.seconds = Convert.ToDouble(secD);
            decimal dd = deg + (minD / 60);

            if (pos == CoordinatesPosition.S || pos == CoordinatesPosition.W)
            {
                dd *= -1;
            }
            this.decimalDegree = Convert.ToDouble(dd);
        }

        /// <summary>
        /// Signed degrees (decimal) format coordinate
        /// </summary>
        /// <returns>Signed degrees (decimal) format coordinate</returns>
        public double ToDouble()
        {
            return this.decimalDegree;
        }

        /// <summary>
        /// Overridden Coordinate ToString() method
        /// </summary>
        /// <returns>Degree Minute Seconds formated coordinate part. Seconds round to the 3rd</returns>
        public override string ToString()
        {
            return FormatString("");
        }
        /// <summary>
        /// Overridden Coordinate ToString() method that accepts formatting. 
        /// Refer to documentation for coordinate format options
        /// </summary>
        /// <param name="format">Format string</param>
        /// <returns>Custom formatted coordinate part</returns>
        public string ToString(string format)
        {
            return FormatString(format.ToUpper());
        }
        /// <summary>
        /// Overridden Coordinate ToString() method that accepts formatting for XAML Binding. 
        /// Refer to documentation for coordinate format options
        /// </summary>
        /// <param name="format">Format string</param>
        /// <param name="formatProvider">Fromat Provider</param>
        /// <returns>Custom formatted coordinate part</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return FormatString(format.ToUpper());
        }
        /// <summary>
        /// String formatting logic
        /// </summary>
        /// <param name="format">Formated Coordinate</param>
        /// <returns></returns>
        private string FormatString(string format)
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

            if (format == string.Empty || format == null)
            {
                return ToDegreeMinuteSecondString(3, lead, trail, symbols, degreeSymbol, minuteSymbol, secondsSymbol, hyphen, positionFirst);
            }

            List<string> fl = format.Split(':').ToList();

            #region Assign Formatting Rules

            foreach (string s in fl)
            {
                if (s == string.Empty) { continue; }
                if (s.ToCharArray().Count() != 2) { throw new FormatException("'" + s + "' is not a valid string format argument."); }
                switch (s[0])
                {
                    case 'F':
                        switch (s[1])
                        {
                            case 'D':
                                type = ToStringType.Decimal_Degree;
                                break;
                            case 'M':
                                type = ToStringType.Degree_Decimal_Minute;
                                break;
                            case 'S':
                                type = ToStringType.Degree_Minute_Second;
                                break;
                            case 'C':
                                type = ToStringType.Decimal;
                                break;
                            default:
                                throw new FormatException(FormatError(s[1].ToString(), "Format (F)"));

                        }
                        break;
                    case 'R':
                        try
                        {
                            rounding = Convert.ToInt32(s[1].ToString());
                        }
                        catch (FormatException)
                        {
                            throw new FormatException(FormatError(s[1].ToString(), "Rounding (R)"));
                        }
                        break;
                    case 'L':
                        switch (s[1])
                        {
                            case 'T':
                                lead = true;
                                break;
                            case 'F':
                                lead = false;
                                break;
                            default:
                                throw new FormatException(FormatError(s[1].ToString(), "Leading Zeros (L)"));
                        }
                        break;
                    case 'T':
                        switch (s[1])
                        {
                            case 'T':
                                trail = true;
                                break;
                            case 'F':
                                trail = false;
                                break;
                            default:
                                throw new FormatException(FormatError(s[1].ToString(), "Trailing Zeros (T)"));
                        }
                        break;
                    case 'B':
                        switch (s[1])
                        {
                            case 'T':
                                symbols = true;
                                break;
                            case 'F':
                                symbols = false;
                                break;
                            default:
                                throw new FormatException(FormatError(s[1].ToString(), "Display Symbols (B)"));
                        }
                        break;
                    case 'D':
                        switch (s[1])
                        {
                            case 'T':
                                degreeSymbol = true;
                                break;
                            case 'F':
                                degreeSymbol = false;
                                break;
                            default:
                                throw new FormatException(FormatError(s[1].ToString(), "Display Degree Symbol (D)"));
                        }
                        break;
                    case 'M':
                        switch (s[1])
                        {
                            case 'T':
                                minuteSymbol = true;
                                break;
                            case 'F':
                                minuteSymbol = false;
                                break;
                            default:
                                throw new FormatException(FormatError(s[1].ToString(), "Display Minutes Symbol (M)"));
                        }
                        break;
                    case 'S':
                        switch (s[1])
                        {
                            case 'T':
                                secondsSymbol = true;
                                break;
                            case 'F':
                                secondsSymbol = false;
                                break;
                            default:
                                throw new FormatException(FormatError(s[1].ToString(), "Display Seconds Symbol (S)"));
                        }
                        break;
                    case 'H':
                        switch (s[1])
                        {
                            case 'T':
                                hyphen = true;
                                break;
                            case 'F':
                                hyphen = false;
                                break;
                            default:
                                throw new FormatException(FormatError(s[1].ToString(), "Display Hyphens (H)"));
                        }
                        break;
                    case 'P':
                        switch (s[1])
                        {
                            case 'F':
                                positionFirst = true;
                                break;
                            case 'L':
                                positionFirst = false;
                                break;
                            default:
                                throw new FormatException(FormatError(s[1].ToString(), "Display Position (P). If using position argument must be either 'F' (first) or 'L' (last)"));
                        }
                        break;
                    default:
                        throw new FormatException("Invalid string format rule passed. '" + s + "' is not a valid argument.");

                }
            }
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
                    double dub = this.ToDouble();
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

        private string Leading_Trailing_Format(bool isLead, bool isTrail, int rounding, CoordinatesPosition? p = null)
        {
            string leadString = "{0:0";
            if (isLead)
            {
                if (p != null)
                {
                    if (p.Value == CoordinatesPosition.W || p.Value == CoordinatesPosition.E)
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
        /// Property changd event handler
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Notifies the Coordinate Part of a property change
        /// </summary>
        /// <param name="propName"></param>
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
                if (propName != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
                    if (Position == CoordinatesPosition.N || Position == CoordinatesPosition.S)
                    {
                        Parent.NotifyPropertyChanged(CoordinateType.Lat);
                    }
                    if (Position == CoordinatesPosition.E || Position == CoordinatesPosition.W)
                    {
                        Parent.NotifyPropertyChanged(CoordinateType.Long);
                    }
                }
            }
        }

        private enum ToStringType
        {
            Decimal_Degree, Degree_Decimal_Minute, Degree_Minute_Second, Decimal
        }

    }
    /// <summary>
    /// Used for setting whether a coordinate part is latitudinal or longitudinal
    /// </summary>
    public enum CoordinateType
    {
        /// <summary>
        /// Latitude
        /// </summary>
        Lat,
        /// <summary>
        /// Longitude
        /// </summary>
        Long 
    }
    /// <summary>
    /// Used to set a coordinate parts position
    /// </summary>
    public enum CoordinatesPosition
    {
        /// <summary>
        /// North
        /// </summary>
        N, 
        /// <summary>
        /// East
        /// </summary>
        E, 
        /// <summary>
        /// South
        /// </summary>
        S, 
        /// <summary>
        /// West
        /// </summary>
        W
    }
    /// <summary>
    /// Used to display a celestial condition for a set day
    /// </summary>
    public enum CelestialStatus
    {
        /// <summary>
        /// Celestial body rises and sets on the set day.
        /// </summary>
        RiseAndSet, 
        /// <summary>
        /// Celestial body is down all day
        /// </summary>
        DownAllDay, 
        /// <summary>
        /// Celestial body is up all day
        /// </summary>
        UpAllDay, 
        /// <summary>
        /// Celestial body rises, but does not set on the set day
        /// </summary>
        NoRise, 
        /// <summary>
        /// Celestial body sets, but does not rise on the set day
        /// </summary>
        NoSet
    }
    /// <summary>
    /// Sun and Moon information
    /// </summary>
    public class Celestial
    {
        //When as rise or a set does not occur, the DateTime will return null

        /// <summary>
        /// Initializes an empty Celestial object
        /// </summary>
        public Celestial()
        {
            CalculateCelestialTime(0, 0, new DateTime(1900, 1, 1));
        }
        private Celestial(bool hasCalcs)
        {
            if (hasCalcs) { CalculateCelestialTime(0, 0, new DateTime(1900, 1, 1)); }
        }
        /// <summary>
        /// Initializes a Celestial object.
        /// </summary>
        /// <param name="lat">Decimal Format Latitude</param>
        /// <param name="longi">Decimal Format Longitude</param>
        /// <param name="geoDate">Geographic DateTime</param>
        public Celestial(double lat, double longi, DateTime geoDate)
        {
            CalculateCelestialTime(lat, longi, geoDate);
        }
        /// <summary>
        /// Sunset time.
        /// </summary>
        public DateTime? SunSet { get; set; }
        /// <summary>
        /// Sunrise time.
        /// </summary>
        public DateTime? SunRise { get; set; }
        /// <summary>
        /// Moonset time.
        /// </summary>
        public DateTime? MoonSet { get; set; }
        /// <summary>
        /// Moonrise time.
        /// </summary>
        public DateTime? MoonRise { get; set; }     
        /// <summary>
        /// Suns Condition for the  set geodate.
        /// </summary>
        public CelestialStatus SunCondition { get; set; }
        /// <summary>
        /// Moons condition for the set geodate.
        /// </summary>
        public CelestialStatus MoonCondition { get; set; }
        /// <summary>
        /// Moon illumination phase.
        /// </summary>
        public double MoonPhase { get; set; }
        /// <summary>
        /// Calculates all celestial data. Coordinates will notify as changes occur
        /// </summary>
        /// <param name="lat">Decimal format latitude</param>
        /// <param name="longi">Decimal format longitude</param>
        /// <param name="date">Geographic DateTime</param>
        public void CalculateCelestialTime(double lat, double longi, DateTime date)
        {
            SunCalc.CalculateSunTime(lat, longi, date, this);
            MoonCalc.GetMoonTimes(date, lat, longi, this);
            MoonCalc.GetMoonIllumination(date, this);
        }
        /// <summary>
        /// Calculate celestial data based on lat/long and date
        /// </summary>
        /// <param name="lat">Decimal format latitude</param>
        /// <param name="longi">Decimal format longitude</param>
        /// <param name="date">Geographic DateTime</param>
        /// <returns>Fully populated Celestial object</returns>
        public static Celestial CalculateCelestialTimes(double lat, double longi, DateTime date)
        {
            Celestial c = new Celestial(false);
            SunCalc.CalculateSunTime(lat, longi, date, c);
            MoonCalc.GetMoonTimes(date, lat, longi, c);
            MoonCalc.GetMoonIllumination(date, c);
            return c;
        }
        /// <summary>
        /// Calculate sun data based on lat/long and date
        /// </summary>
        /// <param name="lat">Decimal format latitude</param>
        /// <param name="longi">Decimal format longitude</param>
        /// <param name="date">Geographic DateTime</param>
        /// <returns>Partially populated Celestial Object</returns>
        public static Celestial CalculateSunData(double lat, double longi, DateTime date)
        {
            Celestial c = new Celestial(false);
            SunCalc.CalculateSunTime(lat, longi, date, c);
            
            return c;
        }
        /// <summary>
        /// Calculate moon data based on lat/long and date
        /// </summary>
        /// <param name="lat">Decimal format latitude</param>
        /// <param name="longi">Decimal format longitude</param>
        /// <param name="date">Geographic DateTime</param>
        /// <returns>Partially populated Celestial Object</returns>
        public static Celestial CalculateMoonData(double lat, double longi, DateTime date)
        {
            Celestial c = new Celestial(false);
           
            MoonCalc.GetMoonTimes(date, lat, longi, c);
            MoonCalc.GetMoonIllumination(date, c);
            return c;
        }
        #region Celestial Calculations
        private class SunCalc
        {
            public static void CalculateSunTime(double lat, double longi, DateTime date, Celestial c)
            {
                date = new DateTime(date.Year, date.Month, date.Day, 0,0,0, DateTimeKind.Utc);
                double zone = -(int)Math.Round(TimeZone.CurrentTimeZone.GetUtcOffset(date).TotalSeconds / 3600);
                double jd = GetJulianDay(date) - 2451545;  // Julian day relative to Jan 1.5, 2000

                double lon = longi / 360;
                double tz = zone / 24;
                double ct = jd / 36525 + 1;                                       // centuries since 1900.0
                double t0 = LocalSiderealTimeForTimeZone(lon, jd, tz);      // local sidereal time

                // get sun position at start of day
                jd += tz;
                CalculateSunPosition(jd, ct);
                double ra0 = mSunPositionInSkyArr[0];
                double dec0 = mSunPositionInSkyArr[1];

                // get sun position at end of day
                jd += 1;
                CalculateSunPosition(jd, ct);
                double ra1 = mSunPositionInSkyArr[0];
                double dec1 = mSunPositionInSkyArr[1];

                // make continuous 
                if (ra1 < ra0)
                    ra1 += 2 * Math.PI;

                mIsSunrise = false;
                mIsSunset = false;

                mRightAscentionArr[0] = ra0;
                mDecensionArr[0] = dec0;

                // check each hour of this day
                for (int k = 0; k < 24; k++)
                {
                    mRightAscentionArr[2] = ra0 + (k + 1) * (ra1 - ra0) / 24;
                    mDecensionArr[2] = dec0 + (k + 1) * (dec1 - dec0) / 24;
                    mVHzArr[2] = TestHour(k, zone, t0, lat);

                    // advance to next hour
                    mRightAscentionArr[0] = mRightAscentionArr[2];
                    mDecensionArr[0] = mDecensionArr[2];
                    mVHzArr[0] = mVHzArr[2];
                }

                c.SunRise = new DateTime(date.Year, date.Month, date.Day, mRiseTimeArr[0], mRiseTimeArr[1], 0);
                c.SunSet = new DateTime(date.Year, date.Month, date.Day, mSetTimeArr[0], mSetTimeArr[1], 0);
                c.SunCondition = CelestialStatus.RiseAndSet;
                // neither sunrise nor sunset
                if ((!mIsSunrise) && (!mIsSunset))
                {
                    if (mVHzArr[2] < 0)
                    {
                        c.SunCondition = CelestialStatus.DownAllDay;
                        c.SunRise = null;
                        c.SunSet = null;
                        // Sun down all day
                    }
                    else
                    {
                        c.SunCondition = CelestialStatus.UpAllDay;
                        c.SunRise = null;
                        c.SunSet = null;
                        // Sun up all day
                    }
                }
                // sunrise or sunset
                else
                {
                    if (!mIsSunrise)
                    {
                        // No sunrise this date
                        c.SunCondition = CelestialStatus.NoRise;
                        c.SunRise = null;

                    }
                    else if (!mIsSunset)
                    {
                        // No sunset this date
                        c.SunCondition = CelestialStatus.NoSet;
                        c.SunSet = null;

                    }
                }
            }
            #region Private Suntime Members

            private const double mDR = Math.PI / 180;
            private const double mK1 = 15 * mDR * 1.0027379;

            private static int[] mRiseTimeArr = new int[2] { 0, 0 };
            private static int[] mSetTimeArr = new int[2] { 0, 0 };
            private static double mRizeAzimuth = 0.0;
            private static double mSetAzimuth = 0.0;

            private static double[] mSunPositionInSkyArr = new double[2] { 0.0, 0.0 };
            private static double[] mRightAscentionArr = new double[3] { 0.0, 0.0, 0.0 };
            private static double[] mDecensionArr = new double[3] { 0.0, 0.0, 0.0 };
            private static double[] mVHzArr = new double[3] { 0.0, 0.0, 0.0 };

            private static bool mIsSunrise = false;
            private static bool mIsSunset = false;

            #endregion
            #region Private Suntime Functions
            private static double GetJulianDay(DateTime date)
            {
                int month = date.Month;
                int day = date.Day;
                int year = date.Year;

                bool gregorian = (year < 1583) ? false : true;

                if ((month == 1) || (month == 2))
                {
                    year = year - 1;
                    month = month + 12;
                }

                double a = Math.Truncate((double)year / 100);
                double b = 0;

                if (gregorian)
                    b = 2 - a + Math.Truncate(a / 4);
                else
                    b = 0.0;

                double jd = Math.Truncate(365.25 * (year + 4716))
                           + Math.Truncate(30.6001 * (month + 1))
                           + day + b - 1524.5;

                return jd;
            }
            private static double LocalSiderealTimeForTimeZone(double lon, double jd, double z)
            {
                double s = 24110.5 + 8640184.812999999 * jd / 36525 + 86636.6 * z + 86400 * lon;
                s = s / 86400;
                s = s - Math.Truncate(s);
                return s * 360 * mDR;
            }
            private static void CalculateSunPosition(double jd, double ct)
            {
                double g, lo, s, u, v, w;

                lo = 0.779072 + 0.00273790931 * jd;
                lo = lo - Math.Truncate(lo);
                lo = lo * 2 * Math.PI;

                g = 0.993126 + 0.0027377785 * jd;
                g = g - Math.Truncate(g);
                g = g * 2 * Math.PI;

                v = 0.39785 * Math.Sin(lo);
                v = v - 0.01 * Math.Sin(lo - g);
                v = v + 0.00333 * Math.Sin(lo + g);
                v = v - 0.00021 * ct * Math.Sin(lo);

                u = 1 - 0.03349 * Math.Cos(g);
                u = u - 0.00014 * Math.Cos(2 * lo);
                u = u + 0.00008 * Math.Cos(lo);

                w = -0.0001 - 0.04129 * Math.Sin(2 * lo);
                w = w + 0.03211 * Math.Sin(g);
                w = w + 0.00104 * Math.Sin(2 * lo - g);
                w = w - 0.00035 * Math.Sin(2 * lo + g);
                w = w - 0.00008 * ct * Math.Sin(g);

                // compute sun's right ascension
                s = w / Math.Sqrt(u - v * v);
                mSunPositionInSkyArr[0] = lo + Math.Atan(s / Math.Sqrt(1 - s * s));

                // ...and declination 
                s = v / Math.Sqrt(u);
                mSunPositionInSkyArr[1] = Math.Atan(s / Math.Sqrt(1 - s * s));
            }
            private static double TestHour(int k, double zone, double t0, double lat)
            {
                double[] ha = new double[3];
                double a, b, c, d, e, s, z;
                double time;
                int hr, min;
                double az, dz, hz, nz;

                ha[0] = t0 - mRightAscentionArr[0] + k * mK1;
                ha[2] = t0 - mRightAscentionArr[2] + k * mK1 + mK1;

                ha[1] = (ha[2] + ha[0]) / 2;    // hour angle at half hour
                mDecensionArr[1] = (mDecensionArr[2] + mDecensionArr[0]) / 2;  // declination at half hour

                s = Math.Sin(lat * mDR);
                c = Math.Cos(lat * mDR);
                z = Math.Cos(90.833 * mDR);    // refraction + sun semidiameter at horizon

                if (k <= 0)
                    mVHzArr[0] = s * Math.Sin(mDecensionArr[0]) + c * Math.Cos(mDecensionArr[0]) * Math.Cos(ha[0]) - z;

                mVHzArr[2] = s * Math.Sin(mDecensionArr[2]) + c * Math.Cos(mDecensionArr[2]) * Math.Cos(ha[2]) - z;

                if (Sign(mVHzArr[0]) == Sign(mVHzArr[2]))
                    return mVHzArr[2];  // no event this hour

                mVHzArr[1] = s * Math.Sin(mDecensionArr[1]) + c * Math.Cos(mDecensionArr[1]) * Math.Cos(ha[1]) - z;

                a = 2 * mVHzArr[0] - 4 * mVHzArr[1] + 2 * mVHzArr[2];
                b = -3 * mVHzArr[0] + 4 * mVHzArr[1] - mVHzArr[2];
                d = b * b - 4 * a * mVHzArr[0];

                if (d < 0)
                    return mVHzArr[2];  // no event this hour

                d = Math.Sqrt(d);
                e = (-b + d) / (2 * a);

                if ((e > 1) || (e < 0))
                    e = (-b - d) / (2 * a);

                time = (double)k + e + (double)1 / (double)120; // time of an event

                hr = (int)Math.Floor(time);
                min = (int)Math.Floor((time - hr) * 60);

                hz = ha[0] + e * (ha[2] - ha[0]);                 // azimuth of the sun at the event
                nz = -Math.Cos(mDecensionArr[1]) * Math.Sin(hz);
                dz = c * Math.Sin(mDecensionArr[1]) - s * Math.Cos(mDecensionArr[1]) * Math.Cos(hz);
                az = Math.Atan2(nz, dz) / mDR;
                if (az < 0) az = az + 360;

                if ((mVHzArr[0] < 0) && (mVHzArr[2] > 0))
                {
                    mRiseTimeArr[0] = hr;
                    mRiseTimeArr[1] = min;
                    mRizeAzimuth = az;
                    mIsSunrise = true;
                }

                if ((mVHzArr[0] > 0) && (mVHzArr[2] < 0))
                {
                    mSetTimeArr[0] = hr;
                    mSetTimeArr[1] = min;
                    mSetAzimuth = az;
                    mIsSunset = true;
                }

                return mVHzArr[2];
            }
            private static int Sign(double value)
            {
                int rv = 0;

                if (value > 0.0) rv = 1;
                else if (value < 0.0) rv = -1;
                else rv = 0;

                return rv;
            }
            #endregion
        }
        private class MoonCalc
        {
            static double rad = Math.PI / 180;
            static double dayMs = 1000 * 60 * 60 * 24, J1970 = 2440588, J2000 = 2451545;
            static double e = rad * 23.4397;

            public static void GetMoonTimes(DateTime date, double lat, double lng, Celestial c)
            {
                c.MoonRise = null;
                c.MoonSet = null;
                DateTime t = new DateTime(date.Year,date.Month, date.Day,0,0,0,DateTimeKind.Utc);
                c.MoonSet = null;
                c.MoonSet = null;
                double hc = 0.133 * rad,
                h0 = GetMoonPosition(t, lat, lng).Altitude - hc,
                h1, h2, a, b, xe, ye, d, roots, dx;
                double? x1 = null, x2 = null, rise = null, set = null;
                
                bool isRise = false;
                bool isSet = false;
                bool isNeg;
                if (h0 < 0)
                {
                    isNeg = true;
                }
                else
                {
                    isNeg = false;
                }
                // go in 2-hour chunks, each time seeing if a 3-point quadratic curve crosses zero (which means rise or set)
                for (var i = 1; i <= 24; i += 2)
                {
                    
                    h1 = GetMoonPosition(hoursLater(t, i), lat, lng).Altitude - hc;
                    h2 = GetMoonPosition(hoursLater(t, i + 1), lat, lng).Altitude - hc;
                    if (isNeg && h1 >= 0 || isNeg && h2 >= 0) { isNeg = false; isRise = true; }
                    if (!isNeg && h1 < 0 || !isNeg && h2 < 0) { isNeg = true; isSet = true; }

                    a = (h0 + h2) / 2 - h1;
                    b = (h2 - h0) / 2;
                    xe = -b / (2 * a);
                    ye = (a * xe + b) * xe + h1;
                    d = b * b - 4 * a * h1;
                    roots = 0;

                    if (d >= 0)
                    {
                        dx = Math.Sqrt(d) / (Math.Abs(a) * 2);
                        x1 = xe - dx;
                        x2 = xe + dx;
                        if (Math.Abs(x1.Value) <= 1) roots++;
                        if (Math.Abs(x2.Value) <= 1) roots++;
                        if (x1 < -1) x1 = x2;
                    }

                    if (roots == 1)
                    {
                        if (h0 < 0) rise = i + x1;
                        else set = i + x1;

                    }
                    else if (roots == 2)
                    {
                        rise = i + (ye < 0 ? x2 : x1);
                        set = i + (ye < 0 ? x1 : x2);
                    }

                    if (rise != null && set != null) break;

                    h0 = h2;
                }

                
                if (rise != null) { c.MoonRise = hoursLater(t, rise.Value); }
                if (set != null) { c.MoonSet = hoursLater(t, set.Value); }
                
                if (isRise && isSet) { c.MoonCondition = CelestialStatus.RiseAndSet; }
                else
                {
                    if (!isRise && !isSet)
                    {
                        if (h0 >= 0) { c.MoonCondition = CelestialStatus.UpAllDay; }
                        else { c.MoonCondition = CelestialStatus.DownAllDay; }
                    }
                    if (!isRise && isSet) { c.MoonCondition = CelestialStatus.NoRise; }
                    if (isRise && !isSet) { c.MoonCondition = CelestialStatus.NoSet; }
                }
                
            }
            static MoonPosition GetMoonPosition(DateTime date, double lat, double lng)
            {
                double d = toDays(date);

                CelCoords c = GetMoonCoords(d);
                double lw = rad * -lng;
                double phi = rad * lat;
                double H = siderealTime(d, lw) - c.ra;
                double h = altitude(H, phi, c.dec);

                // formula 14.1 of "Astronomical Algorithms" 2nd edition by Jean Meeus (Willmann-Bell, Richmond) 1998.
                double pa = Math.Atan2(Math.Sin(H), Math.Tan(phi) * Math.Cos(c.dec) - Math.Sin(c.dec) * Math.Cos(H));

                h = h + astroRefraction(h); // altitude correction for refraction

                MoonPosition mp = new MoonPosition();
                mp.Azimuth = azimuth(H, phi, c.dec);
                mp.Altitude = h;
                mp.Distance = c.dist;
                mp.ParallacticAngle = pa;
                return mp;
            }
            static CelCoords GetMoonCoords(double d)
            { // geocentric ecliptic coordinates of the moon

                double L = rad * (218.316 + 13.176396 * d), // ecliptic longitude
                    M = rad * (134.963 + 13.064993 * d), // mean anomaly
                    F = rad * (93.272 + 13.229350 * d),  // mean distance

                    l = L + rad * 6.289 * Math.Sin(M), // longitude
                    b = rad * 5.128 * Math.Sin(F),     // latitude
                    dt = 385001 - 20905 * Math.Cos(M);  // distance to the moon in km
                CelCoords mc = new CelCoords();
                mc.ra = rightAscension(l, b);
                mc.dec = declination(l, b);
                mc.dist = dt;
                return mc;
            }

            public static void GetMoonIllumination(DateTime date,Celestial c)
            {
                date = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);
                double d = toDays(date);
                CelCoords s = GetSunCoords(d);
                CelCoords m = GetMoonCoords(d);

                double sdist = 149598000,
                phi = Math.Acos(Math.Sin(s.dec) * Math.Sin(m.dec) + Math.Cos(s.dec) * Math.Cos(m.dec) * Math.Cos(s.ra - m.ra)),
                inc = Math.Atan2(sdist * Math.Sin(phi), m.dist - sdist * Math.Cos(phi)),
                angle = Math.Atan2(Math.Cos(s.dec) * Math.Sin(s.ra - m.ra), Math.Sin(s.dec) * Math.Cos(m.dec) -
                        Math.Cos(s.dec) * Math.Sin(m.dec) * Math.Cos(s.ra - m.ra));
                MoonIllum mi = new MoonIllum();
                mi.Fraction = (1 + Math.Cos(inc)) / 2;
                mi.Phase = 0.5 + 0.5 * inc * (angle < 0 ? -1 : 1) / Math.PI;
                mi.Angle = angle;
                c.MoonPhase = mi.Phase;

            }

            //Moon Time Functions
            private static CelCoords GetSunCoords(double d)
            {
                double M = solarMeanAnomaly(d),
                    L = eclipticLongitude(M);
                CelCoords c = new CelCoords();
                c.dec = declination(L, 0);
                c.ra = rightAscension(L, 0);
                return c;
            }
            private static double solarMeanAnomaly(double d) { return rad * (357.5291 + 0.98560028 * d); }

            private static double eclipticLongitude(double M)
            {
                double C = rad * (1.9148 * Math.Sin(M) + 0.02 * Math.Sin(2 * M) + 0.0003 * Math.Sin(3 * M)), // equation of center
                    P = rad * 102.9372; // perihelion of the Earth

                return M + C + P + Math.PI;
            }
            private static DateTime hoursLater(DateTime date, double h)
            {
                return date.AddHours(h);
            }
            static double toJulian(DateTime date)
            {
                DateTime d = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                DateTime t = date;
                double l = (double)(t - d).TotalMilliseconds;
             
                return l / dayMs - 0.5 + J1970;
            }
            static double toDays(DateTime date)
            {
                return toJulian(date) - J2000;
            }
            static double rightAscension(double l, double b) { return Math.Atan2(Math.Sin(l) * Math.Cos(e) - Math.Tan(b) * Math.Sin(e), Math.Cos(l)); }
            static double declination(double l, double b) { return Math.Asin(Math.Sin(b) * Math.Cos(e) + Math.Cos(b) * Math.Sin(e) * Math.Sin(l)); }
            static double azimuth(double H, double phi, double dec) { return Math.Atan2(Math.Sin(H), Math.Cos(H) * Math.Sin(phi) - Math.Tan(dec) * Math.Cos(phi)); }
            static double altitude(double H, double phi, double dec) { return Math.Asin(Math.Sin(phi) * Math.Sin(dec) + Math.Cos(phi) * Math.Cos(dec) * Math.Cos(H)); }
            static double siderealTime(double d, double lw) { return rad * (280.16 + 360.9856235 * d) - lw; }
            static double astroRefraction(double h)
            {
                // the following formula works for positive altitudes only.
                if (h < 0)
                {
                    h = 0; // if h = -0.08901179 a div/0 would occur.
                }

                // formula 16.4 of "Astronomical Algorithms" 2nd edition by Jean Meeus (Willmann-Bell, Richmond) 1998.
                // 1.02 / tan(h + 10.26 / (h + 5.10)) h in degrees, result in arc minutes -> converted to rad:
                return 0.0002967 / Math.Tan(h + 0.00312536 / (h + 0.08901179));
            }

            public class MoonTimes
            {
                public DateTime set { get; set; }
                public DateTime rise { get; set; }
                public CelestialStatus status{get;set;}
            }
            public class MoonPosition
            {
                public double Azimuth { get; set; }
                public double Altitude { get; set; }
                public double Distance { get; set; }
                public double ParallacticAngle { get; set; }
            }
            public class CelCoords
            {
                public double ra { get; set; }
                public double dec { get; set; }
                public double dist { get; set; }
            }
            public class MoonIllum
            {
                public double Fraction { get; set; }
                public double Angle { get; set; }
                public double Phase { get; set; }
            }
        }
        #endregion
    }
      
}
