# CoordinateSharp v1.0.0.1
A simple library designed to assist with geographic coordinate format conversions in C#. This library is intended to enhance latitudinal/longitudinal displays by converting various input formats to various output formats. Most properties in the library are observable, and may be used with MVVM patterns. The ability to calculate various pieces of celestial information (sunset, moon illum..), also exist in this library as it is being used for a planning application.

# Getting Started
These instructions will get a copy of the library running on your local machine for development and testing purposes.

### Prerequisites
.NET 4.0 or greater

### Installing
CoordinateSharp is now available as a nuget packet from [nuget.org](https://www.nuget.org/packages/CoordinateSharp/)

# Usage Instructions

## Creating a Coordinate object

```C#
//The following method creates a coordinate based on the standard and most widely used Decimal Degree format.
//It will then output to the default Degree Minute Seconds format
Coordinate c = new Coordinate(40.57682, -70.75678);
c.ToString(); //Ouputs N 40º 34' 36.552" W 70º 45' 24.408"
```
### Creating a Coordinate object from a non Decimal Degree formatted Lat/Long

```C#
//Using the known Decimal Minute Seconds formatted coordinate N 40º 34' 36.552" W 70º 45' 24.408"
Coordinate c = new Coordinate();
c.Latitude = new CoordinatePart(40,34, 36.552, CoordinatePosition.N, c);
c.Longitude = new CoordinatePart(70, 45, 24.408, CoordinatePosition.W, c);
c.Latitude.ToDouble(); // Returns 40.57682
c.Longitude.ToDouble(); //Returns 70.75678
```
### Formatting a Coordinate

Coordinates may be formatted by passing 2 character format rules. Multiple rules may be passed by separating each variable by a semi-colon. The following example displays a Degree DecimalMinute formatted Lat/Long with leading zeros rounded to the third decimal.
```C#
//Using Lat/Long 40.57682, -70.75678 
string format = "FM:LT:R3";
c.ToString(format); // N 40º 34.609' W 070º 045.407'
c.Latitude.ToString(format);// N 40º 34.609'
c.Longitude.ToString(format);// W 070º 45.407'
```

The first character of the format is the rule with the second being the value. Each rule expects a specific value type. Reference below for formatting options. The first character in each example is the specific format rule. Everything in parentheses represents a value that may be passed with the rule.

If a format is not passed for a specific rule, the default rule will be executed during format.

### Formatting Rules

```
F(D,M,S,C) = Format. Format values must be either 'D' (Decimal Degree) 'M' (Degree Decimal Minute) 'S' (Degree Minute Second) 'C' (Decimal): ex. 'FS' = N 70º 40' 56.678"
R(0-9) = Rounding. Rounding values may be 0-9. Any decimals will be rounded to the declared digit. ex. 70.635473 with 'R3' = 70.635
L(T,F) = Leading Zeros. Leading zeros may be set 'T' (true) or 'F' (false). ex. W 70.645 with 'LT' = 070.645
T(T,F) = Trailing Zeros. Trailing zeros may be set 'T' (true) or 'F' (false). Will only trail to the specified Rounding rule. ex 70.746 with 'R5' & 'TT' = 70.74600
B(T,F) = Display Symbols. Display symbols may be set 'T' (true) or 'F' (false). Will hide degree, minute, seconds symbols if false
D(T,F) = Display Degree Symbol. May be set 'T' (true) or 'F' (false). May only be set when displaying symbols. Will hide degree symbol if false
M(T,F) = Display Minute Symbol. May be set 'T' (true) or 'F' (false). May only be set when displaying symbols. Will hide minute symbol if false
S(T,F) = Display Second Symbol. May be set 'T' (true) or 'F' (false). May only be set when displaying symbols. Will hide seconds symbol if false
H(T,F) = Display Hyphens. May be set 'T' (true) or 'F' (false). Will display hyphens between degrees, minute, seconds if set true.
P(F,L) = Position Display. May be set to 'F' (first) or 'L' (last). Will display postion letter where disired

Defaults:
Format: (FS) Degrees Minutes Seconds
Rounding: Dependent upon selected format
Leading Zeros: False
Trailing Zeros: False
Display Symbols: True (All Symbols display)
Display Hyphens: False
Position Display: First        
```

### Binding and MVVM

The properties in CoordinateSharp are observable and may be bound. Format strings may also be passed from the front end. Example in WPF/XAML

Output Example:
```XAML
 <TextBlock Text="{Binding Path=DataContext.Latitude, StringFormat='FS:TT', UpdateSourceTrigger=PropertyChanged}"/>
 ```
 Input Example:
 ```XAML
 <ComboBox Name="latPosBox" VerticalAlignment="Center" SelectedItem="{Binding Path=DataContext.Latitude.Position, UpdateSourceTrigger=LostFocus, Mode=TwoWay}"/>
 <TextBox Text="{Binding Path=DataContext.Latitude.Degrees, UpdateSourceTrigger=LostFocus, Mode=TwoWay, ValidatesOnExceptions=True}"/>
 <TextBox Text="{Binding Path=DataContext.Latitude.Minutes, UpdateSourceTrigger=LostFocus, Mode=TwoWay, ValidatesOnExceptions=True}"/>
 <TextBox Text="{Binding Path=DataContext.Latitude.Seconds, StringFormat={}{0:0.####}, UpdateSourceTrigger=LostFocus, Mode=TwoWay, ValidatesOnExceptions=True}"/>
 ```
 
 It is important that input boxes be set with 'ValidatesOnExceptions=True'. This will ensure boxes display input erros when incorrect values are passed.
 
 ## Celestial Information
 
 You may pull the following pieces of celestial information by passing a geodate to a Coordinate object. You may initialize an object with a date or pass it later. All dates are assumed to be in UTC. Only pass UTC DateTimes.

  ```C#
  Coordinate c = new Coordinate(40.57682, -70.75678, new DateTime(2017,3,21));
  Debug.Print(coord.CelestialInfo.SunRise.ToString()); //Outputs 3/21/2017 10:44:00 AM
  ```
  
  The following pieces of celestial information are available
    -Sun Set
    -Sun Rise
    -MoonSet
    -Moon Rise
    -Moon Illumination
    
  Sun/Moon Set and Rise DateTimes are nullable. If a null value is return the Sun or Moon Condition needs to be viewed to see why. In the below example we are using a lat/long near the North Pole with a date in august. The sun does not set that far North during the specified time of year.
  
   ```C#
  Coordinate c = new Coordinate(85.57682, -70.75678, new DateTime(2017,8,21));
  Debug.Print(coord.CelestialInfo.SunRise.ToString() + " " + coord.CelestialInfo.SunCondition); //Outputs UpAllDay
  ```
  
### Acknowledgements

SunTime calculations were adapted from NOAA and Zacky Pickholz 2008 "C# Class for Calculating Sunrise and Sunset Times" 
 [NOAA](https://www.esrl.noaa.gov/gmd/grad/solcalc/main.js)
 [The Zacky Pickholz project](https://www.codeproject.com/Articles/29306/C-Class-for-Calculating-Sunrise-and-Sunset-Times)

MoonTime calculations were adapted from the mourner / suncalc project (c) 2011-2015, Vladimir Agafonkin [suncalc](https://github.com/mourner/suncalc/blob/master/suncalc.js)
suncalc's moon calculations are based on "Astronomical Algorithms" 2nd edition by Jean Meeus (Willmann-Bell, Richmond) 1998.
 & [These Formulas](http://aa.quae.nl/en/reken/hemelpositie.html formulas)

Calculations for illumination parameters of the moon based on [NASA Formulas](http://idlastro.gsfc.nasa.gov/ftp/pro/astro/mphase.pro) and Chapter 48 of "Astronomical Algorithms" 2nd edition by Jean Meeus (Willmann-Bell, Richmond) 1998.
  
  
