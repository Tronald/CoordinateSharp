# CoordinateSharp v1.1.2.5

A simple library designed to assist with geographic coordinate string formatting in C#. This library is intended to enhance latitudinal/longitudinal displays by converting various input string formats to various output string formats. Most properties in the library implement ```INotifyPropertyChanged``` and may be used with MVVM patterns. This library can convert Lat/Long to UTM/MGRS(NATO UTM) and Cartesian (X, Y, Z). The ability to calculate various pieces of celestial information (sunset, moon illum..) also exist.

[Change Notes](###-1.1.2.2-change-notes)

### 1.1.2.5 Change Notes
* -Added ability to convert to/from cartesian
* -Added ability to calculate distance between two points (Haversine Formula)
* -Added ability to get radians from CoordinatePart
* -Added Solar/Lunar eclipse information
* -Expanded eager loading options

### 1.1.2.4 Change Notes
* -Added ability to pass custom datum for UTM and MGRS conversions
* -Adjusted UTM conversion formula to be more readable

### 1.1.2.3 Change Notes
* -Makes AdditionalSolarTimes nullable (fixes issue #24)

### 1.1.2.2 Change Notes
* -Fixes issue #21

### 1.1.2.1 Change Notes
* -Added UTM to Lat/Long conversion.
* -Added MGRS(NATO UTM) to Lat/Long conversion.
* -Added sun altitude and azimuth properties.
* -Added additional solar times.
* -Added additional moon illumination properties.
* -Added astrological sign properties.
* -XML documentation added to assembly.
* -Added UTC time integrity to calculations.
* -Minor bug fixes.

# Getting Started
These instructions will get a copy of the library running on your local machine for development and testing purposes.

### Prerequisites
.NET 4.0 or greater.

### Installing
CoordinateSharp is now available as a nuget packet from [nuget.org](https://www.nuget.org/packages/CoordinateSharp/)

# Usage Instructions

### Creating a Coordinate object

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

Coordinate string formats may be changed by passing or editing the ```FormatOptions``` property contained in the ```Coordinate``` object. 

```C#
Coordinate c = new Coordinate(40.57682, -70.75678);

c.FormatOptions.CoordinateFormatType = CoordinateFormatType.Degree_Decimal_Minutes;
c.FormatOptions.Display_Leading_Zeros = true;
c.FormatOptions.Round = 3;

c.ToString(); // N 40º 34.609' W 070º 045.407'
c.Latitude.ToString();// N 40º 34.609'
c.Longitude.ToString();// W 070º 45.407'
```

### Universal Transverse Mercator (UTM) & Military Grid Reference System/NATO UTM (MGRS/ NATO UTM) Formats

UTM and MGRS formats are available for display. They are converted from the lat/long decimal values. The default datum is WGS84 but a custom datum may be passed. These formats are accessible from the ```Coordinate``` object.

```C#
Coordinate c = new Coordinate(40.57682, -70.75678);
c.UTM.ToString(); // Outputs 19T 351307mE 4493264mN
c.MGRS.ToString(); // Outputs 19T CE 51307 93264
```

To convert UTM or MGRS coordinates into Lat/Long.

```C#
UniversalTransverseMercator utm = new UniversalTransverseMercator("T", 32, 233434, 234234);
Coordinate c = UniversalTransverseMercator.ConvertUTMtoLatLong(utm);
```

You may change or pass a custom datum by using the Equatorial Radius (Semi-Major Axis) and Inverse of Flattening of the datum. This will cause UTM/MGRS conversions to be based on the new datum.

To change the current datum.

```C#
c.Set_Datum(6378160.000, 298.25);
```

To create an object with the custom datum.

```C#
UniversalTransverseMercator utm = new UniversalTransverseMercator("Q", 14, 581943.5, 2111989.8, 6378160.000, 298.25);
c = UniversalTransverseMercator.ConvertUTMtoLatLong(utm);
```

Some UTM formats may contain a "Southern Hemisphere" boolean value instead of a Lat Zone character. If this is the case for a UTM you are converting use the letter "C" for southern hemisphere UTMs and "N" for northern hemisphere UTMs.

```C#
//MY UTM COORD ZONE: 32 EASTING: 233434 NORTHING: 234234 (NORTHERN HEMISPHERE)
UniversalTransverseMercator utm = new UniversalTransverseMercator("N", 32, 233434, 234234);
Coordinate c = UniversalTransverseMercator.ConvertUTMtoLatLong(utm);
```

NOTE: UTM conversions below and above the 85th parallels become highly inaccurate. MGRS conversion may suffer accuracy loss even sooner. Lastly, due to grid overlap the MGRS coordinates input, may not be the same ones output in the created Coordinate class. If accuracy is in question you may test the conversion by following the steps below. 

```C#
MilitaryGridReferenceSystem mgrs = new MilitaryGridReferenceSystem("N", 21, "SA", 66037, 61982);
Coordinate c = MilitaryGridReferenceSystem.MGRStoLatLong(mgrs);
Coordinate nc = MilitaryGridReferenceSystem.MGRStoLatLong(c.MGRS); //c.MGRS is now 20N RF 33962 61982
Debug.Print(c.ToString() + "  " + nc.ToString()); // N 0º 33' 35.988" W 60º 0' 0.01"   N 0º 33' 35.988" W 60º 0' 0.022"
```
In the above example, the MGRS values are different once converted, but the Lat/Long is almost the same once converted back.

### Cartesian Format

Cartesian (X, Y, Z) is available for display. They are converted from the lat/long radian values. These formats are accessible from the ```Coordinate``` object. You may also convert a Cartesian coordinate into a lat/long coordinate.

To Cartesian:
```C#
Coordinate c = new Coordinate(40.7143538, -74.0059731);
c.Cartesian.ToString(); //Outputs 0.20884915 -0.72863022 0.65228831
```

To Lat/Long:
```C#
Cartesian cart = new Cartesian(0.20884915, -0.72863022, 0.65228831);
Coordinate c = Cartesian.CartesianToLatLong(cart);
//OR
Coordinate c = Cartesian.CartesianToLatLong(0.20884915, -0.72863022, 0.65228831);
```

### Calculating Distance

Distance can be calculated between two Coordinates. Various distance values are stored in the Distance object. 

```C#
Distance d = new Distance(coord1, coord2);
d.Kilometers;
```

You may also grab a distance by passing a second Coordinate to an existing Coordinate.

```C#
coord1.Get_Distance_From_Coordinate(coord2).Miles;
```

### Binding and MVVM

The properties in CoordinateSharp implement INotifyPropertyChanged and may be bound. If you wish to bind to the entire ```CoordinatePart``` bind to the ```Display``` property. This property can be notified of changes, unlike the overridden ```ToString()```. The ```Display``` will reflect the formats previously specified for the ```Coordinate``` object in the code-behind.

Output Example:
```XAML
 <TextBlock Text="{Binding Latitude.Display, UpdateSourceTrigger=PropertyChanged}"/>
 ```
 Input Example:
 ```XAML
 <ComboBox Name="latPosBox" VerticalAlignment="Center" SelectedItem="{Binding Path=DataContext.Latitude.Position, UpdateSourceTrigger=LostFocus, Mode=TwoWay}"/>
 <TextBox Text="{Binding Latitude.Degrees, UpdateSourceTrigger=LostFocus, Mode=TwoWay, ValidatesOnExceptions=True}"/>
 <TextBox Text="{Binding Latitude.Minutes, UpdateSourceTrigger=LostFocus, Mode=TwoWay, ValidatesOnExceptions=True}"/>
 <TextBox Text="{Binding Latitude.Seconds, StringFormat={}{0:0.####}, UpdateSourceTrigger=LostFocus, Mode=TwoWay, ValidatesOnExceptions=True}"/>
 ```
 
NOTE: It is important that input boxes be set with 'ValidatesOnExceptions=True'. This will ensure UIElements display input errors when incorrect values are passed.
 
 ### Celestial Information
 
 You may pull the following pieces of celestial information by passing a UTC geographic date to a Coordinate object. You may initialize an object with a date or pass it later. All dates are assumed to be in UTC. Only pass UTC DateTimes. You must convert back to local time on your own.

  ```C#
  Coordinate c = new Coordinate(40.57682, -70.75678, new DateTime(2017,3,21));
  c.CelestialInfo.SunRise.ToString(); //Outputs 3/21/2017 10:44:00 AM
  ```
  
  The following pieces of celestial information are available:
  
  * -Sun Set        
  * -Sun Rise    
  * -Sun Altitude
  * -Sun Azimuth
  * -MoonSet          
  * -Moon Rise        
  * -Moon Distance
  * -Moon Illumination (Phase, Phase Name, etc)
  * -Additional Solar Times (Civil/Nautical Dawn/Dusk)
  * -Astrological Information (Moon Sign, Zodiac Sign, Moon Name If Full Moon")
  * -(BETA) Solar/Lunar Eclipse information (see below).
    
  Sun/Moon Set and Rise DateTimes are nullable. If a null value is returned the Sun or Moon Condition needs to be viewed to see why. In the below example we are using a lat/long near the North Pole with a date in August. The sun does not set that far North during the specified time of year.
  
   ```C#
  Coordinate c = new Coordinate(85.57682, -70.75678, new DateTime(2017,8,21));
  coord.CelestialInfo.SunCondition.ToString(); //Outputs UpAllDay
  ```
  
   Moon Illumination returns a value from 0.0 to 1.0. The table shown is a basic break down. You may determine Waxing and Waning types between the values shown or you may get the phase name from the Celestial.MoonIllum.PhaseName property.
  
|Value |Phase          |
| ---- | ------------- |
| 0.0  | New Moon      |
| 0.25 | First Quarter |
| 0.5  | Full Moon     |
| 0.75 | Third Quarter |

  ```C#
  c.Celestial.MoonIllum.PhaseName
  ```

  You may also grab celestial data through static functions if you do not wish to create a Coordinate object.
  
  ```C#
  Celestial cel = Celestial.CalculateCelestialTimes(85.57682, -70.75678, new DateTime(2017,8,21));
  cel.SunRise.Value.ToString();
  ```
  
  NOTE REGARDING MOON DISTANCE: The formula used to calculate moon distance in this library has a been discovered to have standard distance deviation of 3,388 km with Perigee and Apogee approximate time deviations of 36 hours. Results may be innacurate at times and should be used for estimations only. This formula will be worked for accuracy in future releases.
  
  (BETA) The Solar and Lunar Eclipse.
  
  ```C#
  Coordinate seattle = new Coordinate(47.6062, -122.3321, DateTime.Now);
  //Solar
  SolarEclipse se = seattle.CelestialInfo.SolarEclipse;
  se.LastEclipse.Date;
  se.LastEclipse.Type;
  //Lunar
  LunarEclipse le = seattle.CelestialInfo.LunarEclipse;
  se.NextEclipse.Date;
  se.NextEclipse.Type;
  ```
  
  You may also grab a list of eclipse data based on the century for the location's date.
  
  ```C#
  List<SolarEclipseDetails> events = Celestial.Get_Solar_Eclipse_Table(seattle.Latitude.ToDouble(), seattle.Longitude.ToDouble(),  DateTime.Now);
 ```
  NOTE REGARDING ECLIPSE DATA: Eclipse data can only be obtained from the years 1701-2400. Thas range will be expanded with future updates.
 
  NOTE REGARDING SOLAR/LUNAR ECLIPSE PROPERTIES: The `Date` property for both the Lunar and Solar eclipse classes will only return the date of the event. Other properties such as `PartialEclipseBegin` will give more exact timing for event parts.
  
  Properties will return `0001/1/1 12:00:00` if the referenced event didn't occur. For example if a solar eclipse is not a Total or Annular eclipse, the `AorTEclipseBegin` property won't return a populated DateTime. 

  NOTE REGARDING CALCULATIONS: The formulas used take into account the locations altitude. Currently all calculations for eclipse timing are set with an altitude of 100 meters. Slight deviations in actual eclipse timing may occur based on the locations actual altitude. Deviations are very minimal and should suffice for most applications.
  
### Eager Loading

CoordinateSharp values are all eager loaded upon initialization of the Coordinate object. Anytime a Coordinate object property changes, everything is recalculated. The calculations are generally small, but you may wish to turn off eager loading if you are trying to maximize performance. This will allow you to specify when certain calculations take place. 

```C#
EagerLoad eagerLoad = New EagerLoad();
eagerLoad.Celestial = false;
Coordinate c = new Coordinate(40.0352, -74.5844, DateTime.Now, eagerLoad);
//To load Celestial data when ready
c.LoadCelestialInfo();           
 ```
The above example initializes a Coordinate with eager loading in place. You may however turn it on or off after initialization.

```C#
c.EagerLoadSettings.Celestial = false;    
 ```
   
# Acknowledgements

SunTime calculations were adapted from NOAA and Zacky Pickholz 2008 "C# Class for Calculating Sunrise and Sunset Times" 
 [NOAA](https://www.esrl.noaa.gov/gmd/grad/solcalc/main.js)
 [The Zacky Pickholz project](https://www.codeproject.com/Articles/29306/C-Class-for-Calculating-Sunrise-and-Sunset-Times)

MoonTime calculations were adapted from the mourner / suncalc project (c) 2011-2015, Vladimir Agafonkin [suncalc](https://github.com/mourner/suncalc/blob/master/suncalc.js)
suncalc's moon calculations are based on "Astronomical Algorithms" 2nd edition by Jean Meeus (Willmann-Bell, Richmond) 1998.
 & [These Formulas by Dr. Louis Strous](http://aa.quae.nl/en/reken/hemelpositie.html)

Calculations for illumination parameters of the moon based on [NASA Formulas](http://idlastro.gsfc.nasa.gov/ftp/pro/astro/mphase.pro) and Chapter 48 of "Astronomical Algorithms" 2nd edition by Jean Meeus (Willmann-Bell, Richmond) 1998.

UTM & MGRS Conversions were referenced from [Sami Salkosuo's j-coordconvert library](https://www.ibm.com/developerworks/library/j-coordconvert/) & [Steven Dutch, Natural and Applied Sciences,University of Wisconsin - Green Bay](https://www.uwgb.edu/dutchs/UsefulData/ConvertUTMNoOZ.HTM)

Solar and Lunar Eclipse calculations were adapted from NASA's [Eclipse Calculator](https://eclipse.gsfc.nasa.gov/) created by Chris O'Byrne and Fred Espenak.
