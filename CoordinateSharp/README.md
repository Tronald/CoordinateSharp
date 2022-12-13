CoordinateSharp is a simple .NET library that is designed to assist with geographic coordinate conversions, formatting and location based celestial calculations such as sunset, sunrise, moonset, moonrise and more.

### Like CoordinateSharp? Tell us about it!

This library was built to help other developers. Please make the time and effort worth while by [telling us what you are using it for](https://github.com/Tronald/CoordinateSharp/issues/79).

### Prerequisites
.NET 4.0+ Framework, .NET 5.0+, or .NET Standard 1.3+ compatible runtimes.


### Licensing & Purchase

CoordinateSharp is split licensed. You may use CoordinateSharp under the AGPL3.0 or a Commercial Use License that absolves you of the AGPL3.0 requirements. Details as well as pricing may be viewed [on our website](https://coordinatesharp.com/Licensing)

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

### Extension Packages

Extension packages are being built to work in conjunction with CoordinateSharp. Currently available extension packages may be downloaded via Nuget.org. Documentation for extension packages is contained in the main documentation of the library and can be found [on our website](https://coordinatesharp.com/).

* **CoordinateSharp.Magnetic** - Calculate location based magnetic field elements.

### Performance Tips

CoordinateSharp's ease of use comes from its eager loaded architecture. This approach may come at the cost of performance at times, but luckily the eager loading of certain properties can be turned off to improve benchmarks. For example if you only need MGRS conversions, you can turn off celestial calculations and improve `Coordinate` initialization by 6-10ms.

It is highly recommended that developers reference our [Performance Tips Page](https://coordinatesharp.com/Performance) once they have become familiar with the basic usage of 
CoordinateSharp.

### Abilities
 
* **Lat/Long formatting:** Quickly format how a coordinate is output.
* **Coordinate conversions:** Convert Geodetic Latitude / Longitude to UTM, MGRS, Cartesian (Spherical and ECEF), Web Mercator (EPSG:3857), GEOREF or vice versa.
* **Coordinate parsing:** Initialize a `Coordinate` with multiple format types using `TryParse()`.
* **Coordinate moving/shifting:** Shift coordinates using a distance and bearing, or a distance and target coordinate.
* **Location based celestial information:** Quickly determine sun set, moon rise, next solar eclipse or even zodiac signs at the input location.
* **Property change notification:** All properties automatically adjust as the `Coordinate` changes. For example, changing the `GeoDate` will cause all celestial times to recalculate. Adjusting a `Coordinate` latitudinal seconds, will retrigger all coordinate conversions and celestial data so your information is always up to date. 
* **Geo-Fencing:** Define a perimeter and determine if your coordinate is within or near polylines.

### Guides

Check out the [CoordinateSharp Developer Guide](https://www.coordinatesharp.com/DeveloperGuide) for more detailed instructions on the usage and abilities of CoordinateSharp.

You may also view the [Documentation](https://www.coordinatesharp.com/Help/index.html) for a more in depth look at CoordinateSharp's structure.

Change notes can be viewed [here](https://www.coordinatesharp.com/ChangeNotes).

### Questions

Have a technical question, but don't feel like creating an issue? Email us at support@signatgroup.com