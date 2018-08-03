## Contributing to CoordinateSharp
Any contribution to this library is greatly appreciated! Developers are encouraged to create issues, ask questions and fork this library. 
If you wish to contribute to the codebase, please follow the below guidlines. 

### Organization

The codebase is currently organized in various .cs files. If modifying the codebase, ensure you are familiar with how it is organized so that
items go into the proper file. 

* Coordinate.cs - Handles the `Coordinate` and `CoordinatePart` model, properties and change notifications.
* Coordinate.Assistant.cs - Handles additional logic and additional custom property types for all coordinate type models.
* Coordinate.UTM.cs - Handles UTM model and property types. Also contains conversion logic.
* Coordinate.MGRS.cs - Handles MGRS (NATO UTM) model and property types. Also contains conversion logic.
* Coordinate.Cartesian.cs - Handles Cartesian model and property types. Also contains conversion logic.
* Celestial.cs - Handles the `Celestial` model and properties.
* Celestial.Assistant.cs - Handles logic and custom property types for the `Celestial` model.
* Celestial.SunCalculations.cs - Handles all solar calculations (except eclipse).
* Celestial.MoonCalculation.cs - Handles all lunar calculations (except eclipse).
* Celestial.SolarEclipseCalc.cs - Handles solar eclipse calculations.
* Celestial.LunarEclipseCalc.cs - Handles lunar eclipse calculations.

### Readable Code

Please make code readable and easy to understand. Comment on any obscure object names or code.

### Testing

Developers are encouraged to write and share tests for changes made. With that said all forks will be manually tested prior to merging.

### Pull Requests

It is asked that you are clear in specifying **WHAT** you changed and **WHY**. With that said there is no formal template for pull requests.
You may however be asked to adjust code if organization doesn't flow or code isn't clear. If you plan to make changes to celestial calculations,
an explanation of the changes will be sought.

## Summary

That's it! Be creative and be innovative. All help is welcome and encouraged!
