# CoordinateSharp v1.1.1.4
A simple library designed to assist with geographic coordinate string formatting in C#. This library is intended to enhance latitudinal/longitudinal displays by converting various input string formats to various output string formats. Most properties in the library are observable, and may be used with MVVM patterns. This library can now convert Lat/Long to UTM/MGRS. The ability to calculate various pieces of celestial information (sunset, moon illum..), also exist in this library as it is being used for a planning application.


# Getting Started
These instructions will get a copy of the library running on your local machine for development and testing purposes.

### Prerequisites
.NET 4.0 or greater.

### Installing
CoordinateSharp is now available as a nuget packet from [nuget.org](https://www.nuget.org/packages/CoordinateSharp/)

# Usage Instructions

## Creating a Coordinate object

The following method creates a coordinate based on the standard Decimal Degree format.
It will then output to the default Degree Minute Seconds format.

```C#
Coordinate c = new Coordinate(40.57682, -70.75678);
c.ToString(); //Ouputs N 40º 34' 36.552" W 70º 45' 24.408"
```

### Creating a Coordinate object from a non Decimal Degree formatted Lat/Long

Using the known Decimal Minute Seconds formatted coordinate N 40º 34' 36.552" W 70º 45' 24.408.

```C#
Coordinate c = new Coordinate();
c.Latitude = new CoordinatePart(40,34, 36.552, CoordinatePosition.N, c);
c.Longitude = new CoordinatePart(70, 45, 24.408, CoordinatePosition.W, c);
c.Latitude.ToDouble(); // Returns 40.57682
c.Longitude.ToDouble(); //Returns -70.75678
```
### Formatting a Coordinate

Coordinate string formats may be changed by passing or editing the ```CoordinateFormatOptions``` property contained in the ```Coordinate``` object.

```C#
Coordinate c = new Coordinate(40.57682, -70.75678);
c.CoordinateFormatOptions.CoordinateFormatType = CoordinateFormatType.Degree_Decimal_Minutes;
c.CoordinateFormatOptions.Display_Leading_Zeros = true;
c.CoordinateFormatOptions.Round = 3;
c.ToString(); // N 40º 34.609' W 070º 045.407'
c.Latitude.ToString();// N 40º 34.609'
c.Longitude.ToString();// W 070º 45.407'
```

### Universal Transverse Mercator (UTM) & Military Grid Reference System (MGRS) Formats

UTM and MGRS formats are available for display. They are converted from the lat/long decimal values based on the WGS 84 datum. You cannot convert these formats back to lat/long at this time. These formats are accessible from the ```Coordinate``` object.

```C#
Coordinate c = new Coordinate(40.57682, -70.75678);
c.UTM.ToString(); // Outputs 19T 351307mE 4493264mN
```

### Binding and MVVM

The properties in CoordinateSharp are observable and may be bound. If you wish to bind to the entire ```CoordinatePart``` bind to the ```Display``` property. This property can be notified of changes, unlike the overridden ```ToString()```

Output Example:
```XAML
 <TextBlock Text="{Binding Path=DataContext.Latitude.Display, UpdateSourceTrigger=PropertyChanged}"/>
 ```
 Input Example:
 ```XAML
 <ComboBox Name="latPosBox" VerticalAlignment="Center" SelectedItem="{Binding Path=DataContext.Latitude.Position, UpdateSourceTrigger=LostFocus, Mode=TwoWay}"/>
 <TextBox Text="{Binding Path=DataContext.Latitude.Degrees, UpdateSourceTrigger=LostFocus, Mode=TwoWay, ValidatesOnExceptions=True}"/>
 <TextBox Text="{Binding Path=DataContext.Latitude.Minutes, UpdateSourceTrigger=LostFocus, Mode=TwoWay, ValidatesOnExceptions=True}"/>
 <TextBox Text="{Binding Path=DataContext.Latitude.Seconds, StringFormat={}{0:0.####}, UpdateSourceTrigger=LostFocus, Mode=TwoWay, ValidatesOnExceptions=True}"/>
 ```
 
 It is important that input boxes be set with 'ValidatesOnExceptions=True'. This will ensure UIElements display input erros when incorrect values are passed.
 
 ## Celestial Information
 
 You may pull the following pieces of celestial information by passing a geodate to a Coordinate object. You may initialize an object with a date or pass it later. All dates are assumed to be in UTC. Only pass UTC DateTimes.

  ```C#
  Coordinate c = new Coordinate(40.57682, -70.75678, new DateTime(2017,3,21));
  Debug.Print(coord.CelestialInfo.SunRise.ToString()); //Outputs 3/21/2017 10:44:00 AM
  ```
  
  The following pieces of celestial information are available:
  
  -Sun Set        
  -Sun Rise         
  -MoonSet          
  -Moon Rise        
  -Moon Illumination
    
  Sun/Moon Set and Rise DateTimes are nullable. If a null value is returned the Sun or Moon Condition needs to be viewed to see why. In the below example we are using a lat/long near the North Pole with a date in August. The sun does not set that far North during the specified time of year.
  
   ```C#
  Coordinate c = new Coordinate(85.57682, -70.75678, new DateTime(2017,8,21));
  Debug.Print(coord.CelestialInfo.SunRise.ToString() + " " + coord.CelestialInfo.SunCondition); //Outputs UpAllDay
  ```
  
   Moon Illimination returns a value from 0.0 to 1.0. The table shown is a basic break down. You may determine Waxing and Waning types between the values shown.
  
|Value |Phase          |
| ---- | ------------- |
| 0.0  | New Moon      |
| 0.25 | First Quarter |
| 0.5  | Full Moon     |
| 0.75 | Third Quarter |

  You may also grab celestial data through static functions if you do not wish to create a Coordinate object.
  
  ```C#
  Celestial cel = Celestial.CalculateCelestialTimes(85.57682, -70.75678, new DateTime(2017,8,21));
  Console.WriteLine(cel.SunRise.Value.ToString());
  ```
   
### Acknowledgements

SunTime calculations were adapted from NOAA and Zacky Pickholz 2008 "C# Class for Calculating Sunrise and Sunset Times" 
 [NOAA](https://www.esrl.noaa.gov/gmd/grad/solcalc/main.js)
 [The Zacky Pickholz project](https://www.codeproject.com/Articles/29306/C-Class-for-Calculating-Sunrise-and-Sunset-Times)

MoonTime calculations were adapted from the mourner / suncalc project (c) 2011-2015, Vladimir Agafonkin [suncalc](https://github.com/mourner/suncalc/blob/master/suncalc.js)
suncalc's moon calculations are based on "Astronomical Algorithms" 2nd edition by Jean Meeus (Willmann-Bell, Richmond) 1998.
 & [These Formulas](http://aa.quae.nl/en/reken/hemelpositie.html formulas)

Calculations for illumination parameters of the moon based on [NASA Formulas](http://idlastro.gsfc.nasa.gov/ftp/pro/astro/mphase.pro) and Chapter 48 of "Astronomical Algorithms" 2nd edition by Jean Meeus (Willmann-Bell, Richmond) 1998.

UTM & MGRS Conversions were referenced from [Sami Salkosuo's j-coordconvert library](https://www.ibm.com/developerworks/library/j-coordconvert/)
  
  
