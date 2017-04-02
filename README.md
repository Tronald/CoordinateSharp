# CoordinateSharp
A simple library designed to assist with geographic coordinate format conversions in C#. This library is intended to enhance latitudinal/longitudinal displays by converting various input formats to various output formats. Most properties in the library are observable, and may be used with MVVM patterns. The ability to calculate various pieces of celestial information (sunset, moon illum..), also exist in this library as it is being used for a planning application.

# Getting Started
These instructions will get a copy of the library running on your local machine for development and testing purposes.

### Prerequisites
.NET 4.0 or greater

### Installing
CoordinateSharp is now available as a nuget packet from [nuget.org](https://www.nuget.org/packages/CoordinateSharp/)

# Usage Instructions

### Creating a Coordinate object

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
c.Longitude.TouDouble(); //Returns 70.75678
```
//STILL WORKING
