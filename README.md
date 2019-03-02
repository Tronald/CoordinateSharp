# THE COORDINATESHARP WEBSITE IS CURRENTLY DOWN. 
### THE HOSTING PROVIDER IS EXPERIENCING ISSUES AND IS ACTIVELY WORKING TO RESTORE SERVICE (2-MAR-2019 11:57 PST). 
<p align="center"><img src="https://s8.postimg.cc/y7wuenuzp/LOGO_COORDINATE_SHARP.jpg"></p>

<h2 align="center">v1.1.5.1</h2>

CoordinateSharp is a simple .NET library that is designed to assist with geographic coordinate conversions, formatting and location based celestial calculations. This library has the ability to convert various lat long formats, UTM, MGRS(NATO UTM) and Cartesian (Spherical and ECEF X, Y, Z). 

<b>CAUTION:</b> v1.1.5.1 begins the depracation of certain methods in preparation of CoordinateSharp v2.1.1.1. Many user mutable items, have also been made user immutable in v1.1.5.1. This only effects properties that do not need to be set by users (i.e. Celestial properties). You should see no negative effects from this conversion if the library has been used in accordance with the [CoordinateSharp Developer Guide](https://coordinatesharp.com/DeveloperGuide).

<b>ANNOUNCEMENT:</b> v2.1.1.1 development has begun. This version is going to focus on simplifying constructors, better management of circular references and property change notification. It is also going to focus on proper assignment of user mutable vs user immutable properties. Tests will be expanded. Lastly, code examples are going to be provided in the library documentation in order to give developers a more standard documentation experience outside of the Developer Guide. This version should give the user an even simpler experience when using CoordinateSharp. 

Change notes can be viewed [here](https://www.coordinatesharp.com/ChangeNotes)

### Like CoordinateSharp? Tell us about it or please consider donating.

This library was built to help other developers. Please make the time and effort worth while by [telling us what you are using it for](https://github.com/Tronald/CoordinateSharp/issues/79). If you are unable to share your experience and use case, please consider donating.
</br></br>
[![](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=S78DZZX5KMVUS)
</br></br>
### Prerequisites
.NET 4.0 or .NET Standard 2.0, 1.4, 1.3 compatible runtimes.

### Installing
CoordinateSharp is available as a nuget package from [nuget.org](https://www.nuget.org/packages/CoordinateSharp/)

Alternatively, you may download the library directly [on our website](https://www.coordinatesharp.com/Download)

### Usage Example


CoordinateSharp is simple to use. In the below example we create a `Coordinate` using one of the methods below.

```csharp
//Seattle coordinates on 5 Jun 2018 @ 10:10 AM (UTC)
//Signed-Decimal Degree    47.6062, -122.3321
//Degrees Minutes Seconds  N 47º 36' 22.32" W 122º 19' 55.56"

/***********************************************************/

//Initialize with signed degree (standard method)
Coordinate c = new Coordinate(47.6062, -122.3321, new DateTime(2018,6,5,10,10,0));

/***IF OTHER FORMAT IS USED SUCH AS DEGREE MINUTES SECONDS***/

//Initialize with TryParse() Method
Coordinate.TryParse("N 47º 36' 22.32\" W 122º 19' 55.56\"", new DateTime(2018,6,5,10,10,0), out c);

/****OR****/

//Initialize with Secondary Method
Coordinate c = new Coordinate();
c.Latitude = new CoordinatePart(47,36, 22.32, CoordinatePosition.N, c);
c.Longitude = new CoordinatePart(122, 19, 55.56, CoordinatePosition.W, c);
c.GeoDate = new DateTime(2018,6,5,10,10,0);
```

Once the `Coordinate` is created we have access to various formats and celestial data. Here are just a few examples.
 
 ```C#
Console.WriteLine(c);                              // N 47º 36' 22.32" W 122º 19' 55.56"
Console.WriteLine(c.Latitude.Seconds);             // 22.32
Console.WriteLine(c.UTM);                          // 10T 550200mE 5272748mN

Console.WriteLine(c.CelestialInfo.SunSet);         // 5-Jun-2018 4:02:00 AM
Console.WriteLine(c.CelestialInfo.MoonAltitude);   // 14.4169966277874
```



### Abilities
 
* **Lat/Long formatting:** Quickly format how a coordinate is output.
* **Coordinate conversions:** Convert Lat/Long to UTM, MGRS, Cartesian (Spherical and ECEF) or vice versa.
* **Coordinate parsing:** Initialize a `Coordinate` with multiple format types using `TryParse()`.
* **Coordinate moving/shifting:** Shift coordinates using a distance and bearing, or a distance and target coordinate.
* **Location based celestial information:** Quickly determine sun set, moon rise, next solar eclipse or even zodiac signs at the input location.
* **Property change notification:** All properties automatically adjust as the `Coordinate` changes. For example, changing the `GeoDate` will cause all celestial times to recalculate. Adjusting a `Coordinate` latitudinal seconds, will retrigger all coordinate conversions and celestial data so your information is always up to date. 
* **Geo-Fencing:** Define a perimeter and determine if your coordinate is within or near polylines.

### Guides

Check out the [CoordinateSharp Developer Guide](https://www.coordinatesharp.com/DeveloperGuide) for more detailed instructions on the usage and abilities of CoordinateSharp.

You may also view the [Documentation](https://www.coordinatesharp.com/Help/index.html) for a more in depth look at CoordinateSharp's structure.
   
<p align="center"><img src="https://s8.postimg.cc/wvf5cfpqt/LOGO_COORDINATE_SHARP_1.jpg"></p>

