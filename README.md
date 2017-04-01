# CoordinateSharp
A simple library designed to assist with geographic coordinate format conversions in C#. This library is intended to enhance latitudinal/longitudinal displays by converting various input formats to various output formats. Most properties in the library are observable, and may be used with MVVM patterns. The ability to calculate various pieces of celestial information (sunset, moon illum..), also exist in this library as it is being used for a planning application.

# Getting Started
These instructions will get a copy of the library running on your local machine for development and testing purposes.

### Prerequisites
.NET 4.0

### Installing
A Nuget package does not yet exist for CoordinateSharp, but will in the near future. Until then, users will need to download CoordinateSharp.cs build the library themselves.

*The following example is for Visual Studio 2012, but should work for most VS verisons*

  1. Create a new Class Library project called CoordinateSharp
  2. Replace the default .cs file with the downloaded version of CoordinateSharp.cs
  3. Goto Project->CoordinateSharp Properties
  4. Select the build tab
  5. Under the 'Output' section Check "XML-Documentation file:"
  6. Open the 'Advanced' build settings
  7. Set Debug Info to "None"
  8. Build the project
 
Once the above steps have been completed you may add the CoordinateSharp reference to your required project by browsing to the CoordinateSharp projects Debug or Release folder.

# Usage Instructions

### Creating a Coordinate

```C#
//The following method creates a coordinate based on the standard and most widely used Decimal Degree format.
//It will then output to the default Degree Minute Seconds format
Coordinate c = new Coordinate(40.57682, -70.75678);
c.ToString(); //Ouputs N 40º 34' 36.552" W 70º 45' 24.408"
```

STILL WORKING CONTINUE FROM HERE...
