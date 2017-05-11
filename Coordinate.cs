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
            this.FormatOptions = new CoordinateFormatOptions();
            latitude = new CoordinatePart(CoordinateType.Lat, this);
            longitude = new CoordinatePart(CoordinateType.Long, this);
            celestialInfo = new Celestial();
            utm = new UniversalTransverseMercator(latitude.ToDouble(), longitude.ToDouble(), this);
            mgrs = new MilitaryGridReferenceSystem(this.utm);          
        }
        /// <summary>
        /// Creates a populated Coordinate object.
        /// </summary>
        /// <param name="lat">Decimal format latitude</param>
        /// <param name="longi">Decimal format longitude</param>
        public Coordinate(double lat, double longi)
        {
            this.FormatOptions = new CoordinateFormatOptions();
            latitude = new CoordinatePart(lat, CoordinateType.Lat, this);
            longitude = new CoordinatePart(longi, CoordinateType.Long, this);
            celestialInfo = new Celestial(lat,longi,new DateTime(1900,1,1));
            utm = new UniversalTransverseMercator(lat, longi, this);
            mgrs = new MilitaryGridReferenceSystem(this.utm);
        }
        /// <summary>
        /// Creates a populated Coordinate object. With an assigned GeoDate.
        /// </summary>
        /// <param name="lat">Decimal format latitude</param>
        /// <param name="longi">Decimal format longitude</param>
        /// <param name="date">DateTime you wish to use for celestial calculation</param>
        public Coordinate(double lat, double longi, DateTime date)
        {
            this.FormatOptions = new CoordinateFormatOptions();
            latitude = new CoordinatePart(lat, CoordinateType.Lat, this);
            longitude = new CoordinatePart(longi, CoordinateType.Long, this);
            celestialInfo = new Celestial(lat, longi, date);            
            this.geoDate = date;
            utm = new UniversalTransverseMercator(lat, longi, this);
            mgrs = new MilitaryGridReferenceSystem(this.utm);
        }
        /// <summary>
        /// Creates a populated Coordinate object.
        /// Not yet implemented.
        /// </summary>
        /// <param name="utm">Universal Transverse Mercator Coordinate</param>
        private Coordinate(string latz, int longz, double easting, double northing)
        {   
            //latitude = new CoordinatePart(CoordinateType.Lat, this);
            //longitude = new CoordinatePart(CoordinateType.Long, this);
            //celestialInfo = new Celestial(this.latitude.ToDouble(), this.longitude.ToDouble(), new DateTime(1900, 1, 1));
            //utm = new UniversalTransverseMercator(latz, longz, easting, northing, this);
            //mgrs = new MilitaryGridReferenceSystem(this.utm);
        }
        /// <summary>
        /// Creates a populated Coordinate object.
        /// Not yet implemented
        /// </summary>
        /// <param name="utm">Universal Transverse Mercator Coordinate</param>
        /// <param name="date">DateTime you wish to use for celestial calculation</param>
        private Coordinate(string latz, int longz, double easting, double northing, DateTime date)
        {
            //latitude = new CoordinatePart(CoordinateType.Lat, this);
            //longitude = new CoordinatePart(CoordinateType.Long, this);
            //celestialInfo = new Celestial(this.latitude.ToDouble(), this.longitude.ToDouble(), date);
            //utm = new UniversalTransverseMercator(latz, longz, easting, northing, this);
            //mgrs = new MilitaryGridReferenceSystem(this.utm);    
        }
        
        private CoordinatePart latitude;
        private CoordinatePart longitude;
        private UniversalTransverseMercator utm;
        private MilitaryGridReferenceSystem mgrs;        
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
                   
                    celestialInfo.CalculateCelestialTime(this.Latitude.DecimalDegree, this.Longitude.DecimalDegree, this.geoDate);
                 
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
                    celestialInfo.CalculateCelestialTime(this.Latitude.DecimalDegree, this.Longitude.DecimalDegree, this.geoDate);                  
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
                  
                    celestialInfo.CalculateCelestialTime(this.Latitude.DecimalDegree, this.Longitude.DecimalDegree, this.geoDate);
                    this.NotifyPropertyChanged("GeoDate");
                    this.NotifyPropertyChanged("CelestialInfo");
                                    
                }
            }
        }
        /// <summary>
        /// Universal Transverse Mercator Values
        /// </summary>
        public UniversalTransverseMercator UTM
        {
            get
            {
                return this.utm;
            }
            //set
            //{
            //    if (this.utm != value)
            //    {
            //        this.utm = value;
            //        this.NotifyPropertyChanged("UTM");
            //        celestialInfo.CalculateCelestialTime(this.Latitude.DecimalDegree, this.Longitude.DecimalDegree, this.geoDate);
            //        this.NotifyPropertyChanged("CelestialInfo");
            //    }
            //}
        }
        /// <summary>
        /// Military Grid Reference System Values
        /// </summary>
        public MilitaryGridReferenceSystem MGRS
        {
            get
            {
                return this.mgrs;
            }
            //set
            //{
            //    if (this.utm != value)
            //    {
            //        this.utm = value;
            //        this.NotifyPropertyChanged("UTM");
            //        celestialInfo.CalculateCelestialTime(this.Latitude.DecimalDegree, this.Longitude.DecimalDegree, this.geoDate);
            //        this.NotifyPropertyChanged("CelestialInfo");
            //    }
            //}
        }
        /// <summary>
        /// Celestial information based on the objects lat/long and geo date.
        /// </summary>
        public Celestial CelestialInfo
        {
            get { return this.celestialInfo; }          
        }
        /// <summary>
        /// Formatting Options
        /// </summary>
        public CoordinateFormatOptions FormatOptions { get; set; }
        /// <summary>
        /// Formatted coordinate String
        /// </summary>
        public string Display
        {
            get
            {
                return this.Latitude.Display + " " + this.Longitude.Display;
            }
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
        [System.Obsolete("The 2 character format string method is deprecated. The CoordinateFormatOptions method should be used when passing formats with ToString().")]      
        public string ToString(string format)
        {
            string latString = latitude.ToString(format.ToUpper());
            string longSting = longitude.ToString(format.ToUpper());
            return latString + " " + longSting;         
        }
        /// <summary>
        /// Overridden Coordinate ToString() method that accepts formatting. 
        /// Refer to documentation for coordinate format options
        /// </summary>
        /// <param name="options">CoordinateFormatOptions</param>
        /// <returns>Custom formatted coordinate</returns>
        public string ToString(CoordinateFormatOptions options)
        {
            string latString = latitude.ToString(options);
            string longSting = longitude.ToString(options);
            return latString + " " + longSting;
        }     

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                switch (propName)
                {
                    case "CelestialInfo":
                        this.celestialInfo.CalculateCelestialTime(this.latitude.DecimalDegree, this.longitude.DecimalDegree, this.geoDate);
                        break;
                    case "UTM":
                        this.utm.ToUTM(this.latitude.ToDouble(), this.longitude.ToDouble(), this.utm);
                        break;
                    case "MGRS":
                        this.MGRS.ToMGRS(this.utm);
                        break;
                }
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
    /// <summary>
    /// Observable class for handling a single coordinate (Lat or Long)
    /// </summary>
    public class CoordinatePart : INotifyPropertyChanged
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
                   
                    //Update Position
                    if ((this.position == CoordinatesPosition.N || this.position == CoordinatesPosition.E) && this.decimalDegree < 0)
                    {
                        if (this.type == CoordinateType.Lat) { this.position = CoordinatesPosition.S; }
                        else { this.position = CoordinatesPosition.W; }
                       
                    }
                    if ((this.position == CoordinatesPosition.W || this.position == CoordinatesPosition.S) && this.decimalDegree >= 0)
                    {
                        if (this.type == CoordinateType.Lat) { this.position = CoordinatesPosition.N; }
                        else { this.position = CoordinatesPosition.E; }
                      
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
                      
                    }
                    if (this.decimalMinute != dm)
                    {
                        this.decimalMinute = dm;
                   
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
                      
                    }
                    if (this.seconds != secs)
                    {
                        this.seconds = secs;                    
                    }
                    NotifyProperties(PropertyTypes.DecimalDegree);
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
                   

                    decimal decValue = Convert.ToDecimal(value); //Convert value to decimal for precision during calculation
                    decimal dmFloor = Math.Floor(decValue); //Extract minutes
                    decimal secs = decValue - dmFloor; //Extract seconds
                    secs *= 60; //Convert seconds to human readable format

                    decimal newDM = decValue / 60; //divide decimalMinute by 60 to get storage value
                    decimal newDD = this.degrees + newDM;//Add new decimal value to the floor degree value to get new decimalDegree;
                    if (this.decimalDegree < 0) { newDD = newDD * -1; } //Restore negative if needed

                    this.decimalDegree = Convert.ToDouble(newDD);  //Convert back to double for storage                      
                   

                    this.minutes = Convert.ToInt32(dmFloor); //Convert minutes to int for storage
                   
                    this.seconds = Convert.ToDouble(secs); //Convert seconds to double for storage 
                    NotifyProperties(PropertyTypes.DecimalMinute);              
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

                    double degABS = Math.Abs(this.decimalDegree); //Make decimalDegree positive for calculations
                    decimal dDec = Convert.ToDecimal(degABS); //Convert to Decimal for precision during calculations              
                    //Convert degrees to decimal to keep precision        
                    decimal dm = dDec - f; //Extract minutes                                      
                    decimal newDD = this.degrees + dm; //Add minutes to new degree for decimalDegree

                    if (this.decimalDegree < 0) { newDD *= -1; } //Set negative as required

                    this.decimalDegree = Convert.ToDouble(newDD); // Convert decimalDegree to double for storage
                    NotifyProperties(PropertyTypes.Degree);
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
                   

                    double degABS = Math.Abs(this.decimalDegree); //Make decimalDegree positive
                    decimal dDec = Convert.ToDecimal(degABS); //Convert to decimalDegree for precision during calucation                        

                    decimal dm = dDec - f; //Extract minutes
                    dm *= 60; //Make minutes human readable

                    decimal secs = dm - minFloor;//Extract Seconds

                    decimal newDM = this.minutes + secs;//Add seconds to minutes for decimalMinute
                    double decMin = Convert.ToDouble(newDM); //Convert decimalMinute to double for storage
                    this.decimalMinute = decMin; //Round to correct precision
                   

                    newDM /= 60; //Convert decimalMinute to storage format
                    decimal newDeg = f + newDM; //Add value to degree for decimalDegree
                    if (this.decimalDegree < 0) { newDeg *= -1; }// Set to negative as required.
                    this.decimalDegree = Convert.ToDouble(newDeg);//Convert to double and roun to correct precision for storage
                    NotifyProperties(PropertyTypes.Minute);
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
                 

                    double degABS = Math.Abs(this.decimalDegree); //Make decimalDegree positive
                    double degFloor = Math.Truncate(degABS); //Truncate the number left of the decimal
                    decimal f = Convert.ToDecimal(degFloor); //Convert to decimal to keep precision

                    decimal secs = Convert.ToDecimal(this.seconds); //Convert seconds to decimal for calculations
                    secs /= 60; //Convert to storage format
                    decimal dm = this.minutes + secs;//Add seconds to minutes for decimalMinute
                    double minFD = Convert.ToDouble(dm); //Convert decimalMinute for storage
                    this.decimalMinute = minFD;//Round to proper precision
                  
                    decimal nm = Convert.ToDecimal(this.decimalMinute) / 60;//Convert decimalMinute to decimal and divide by 60 to get storage format decimalMinute
                    double newDeg = this.degrees + Convert.ToDouble(nm);//Convert to double and add to degree for storage decimalDegree
                    if (this.decimalDegree < 0) { newDeg *= -1; }//Make negative as needed
                    this.decimalDegree = newDeg;//Update decimalDegree and round to proper precision    
                    NotifyProperties(PropertyTypes.Second);
                }
            }
        }       
        /// <summary>
        /// Formate coordinate part string
        /// </summary>
        public string Display
        {
            get 
            {
                if (this.Parent != null)
                {
                    return ToString(Parent.FormatOptions);
                }
                else
                {
                    return ToString();
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
                    NotifyProperties(PropertyTypes.Position);
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
            return FormatString(this.Parent.FormatOptions);
        }
        /// <summary>
        /// Overridden Coordinate ToString() method that accepts formatting.
        /// Does not require the CoordinatePart format property to be set.
        /// Refer to documentation for coordinate format options.
        /// </summary>
        /// <param name="format">Format string</param>
        /// <returns>Custom formatted coordinate part</returns>
        [System.Obsolete("The 2 character format string method is deprecated. The CoordinateFormatOptions method should be used when passing formats with ToString().")]
        public string ToString(string format)
        {
            if (format == null)
            {
                return FormatString("");
            }
            return FormatString(format.ToUpper());
        }
        /// <summary>
        /// Formatted CoordinatePart string.
        /// </summary>
        /// <param name="options">CoordinateFormatOptions</param>
        /// <returns>string</returns>
        public string ToString(CoordinateFormatOptions options)
        {
            return FormatString(options);
        }
        /// <summary>
        /// String formatting logic
        /// </summary>
        /// <param name="format">Formated Coordinate</param>
        /// <returns>string</returns>
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
        /// <summary>
        /// String formatting logic
        /// </summary>
        /// <param name="options">CoordinateFormatOptions</param>
        /// <returns>string</returns>
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

        private enum ToStringType
        {
            Decimal_Degree, Degree_Decimal_Minute, Degree_Minute_Second, Decimal
        }
        /// <summary>
        /// Notify the correct properties and parent properties
        /// </summary>
        /// <param name="p">Property Type</param>
        private void NotifyProperties(PropertyTypes p)
        {
            switch (p)
            {
                case PropertyTypes.DecimalDegree:
                    this.NotifyPropertyChanged("DecimalDegree");
                    this.NotifyPropertyChanged("DecimalMinute");
                    this.NotifyPropertyChanged("Degrees");
                    this.NotifyPropertyChanged("Minutes");
                    this.NotifyPropertyChanged("Seconds");
                    this.NotifyPropertyChanged("Position");
                    break;
                case PropertyTypes.DecimalMinute:
                    this.NotifyPropertyChanged("DecimalDegree");
                    this.NotifyPropertyChanged("DecimalMinute");
                    this.NotifyPropertyChanged("Minutes");
                    this.NotifyPropertyChanged("Seconds");
                    break;
                case PropertyTypes.Degree:
                    this.NotifyPropertyChanged("DecimalDegree");
                    this.NotifyPropertyChanged("Degree");                   
                    break;
                case PropertyTypes.Minute:
                    this.NotifyPropertyChanged("DecimalDegree");
                    this.NotifyPropertyChanged("DecimalMinute");
                    this.NotifyPropertyChanged("Minutes");
                    break;
                case PropertyTypes.Position:
                    this.NotifyPropertyChanged("DecimalDegree");                    
                    this.NotifyPropertyChanged("Position");
                    break;
                case PropertyTypes.Second:
                    this.NotifyPropertyChanged("DecimalDegree");
                    this.NotifyPropertyChanged("DecimalMinute");                
                    this.NotifyPropertyChanged("Seconds");                 
                    break;               
                default:
                    this.NotifyPropertyChanged("DecimalDegree");
                    this.NotifyPropertyChanged("DecimalMinute");
                    this.NotifyPropertyChanged("Degrees");
                    this.NotifyPropertyChanged("Minutes");
                    this.NotifyPropertyChanged("Seconds");
                    this.NotifyPropertyChanged("Position");
                    break;
            }
            this.NotifyPropertyChanged("Display");
            this.Parent.NotifyPropertyChanged("Display");
            this.Parent.NotifyPropertyChanged("CelestialInfo");
            this.Parent.NotifyPropertyChanged("UTM");
            this.Parent.NotifyPropertyChanged("MGRS");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        /// <summary>
        /// Used for notifying the correct properties
        /// </summary>
        private enum PropertyTypes
        {
            DecimalDegree, DecimalMinute, Position, Degree, Minute, Second, FormatChange
        }
    }
}
