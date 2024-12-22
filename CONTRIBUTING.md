## Contributing to CoordinateSharp
Any contribution to this library is greatly appreciated! Developers are encouraged to create issues, ask questions and fork this library. If you wish to contribute to the codebase, please follow the below guidelines. 

### Agree to the Terms

A Contributor Agreement is required to be on file before any pull requests will be considered. The agreement can be signed and emailed to support@signatgroup.com. You may download the agreement [here](https://github.com/Tronald/CoordinateSharp/raw/develop/Signature%20Group%20Contributor%20Agreement.pdf).

### Setup

### Setup

After creating a local fork, additional setup steps may be required depending on your IDE. You might need to download legacy .NET Frameworks. Unfortunately, Microsoft no longer provides a straightforward way to download .NET 4.0 and .NET 4.5 Frameworks. Follow these steps to configure your IDE:

1. **Close all instances of Visual Studio** (or your IDE if using a different one).
2. **Download** [Microsoft.NETFramework.ReferenceAssemblies.net45](https://www.nuget.org/packages/Microsoft.NETFramework.ReferenceAssemblies.net45) from NuGet.org.
3. **Open the package as a ZIP file**:
   - Either directly open it as a ZIP, or 
   - Rename the file extension from `.nupkg` to `.zip` and then open it.
4. **Copy the framework files**:
   - From `build\.NETFramework\v4.0\`, copy the files to:  
     `C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0`  
     *(You’ll need admin permissions. Overwrite any existing files.)*
   - From `build\.NETFramework\v4.5\`, copy the files to:  
     `C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5`  
     *(Again, admin permissions are required. Overwrite any existing files.)*
5. **Reopen Visual Studio** (or your IDE).

Your IDE should now recognize the legacy frameworks.

### Organization

The codebase is organized into various .cs files. If modifying the codebase, ensure you are familiar with how it is organized so that
items go into the proper area. If you are unsure about something, ask and we will be happy to give guidance.

### Readable Code

Please make code readable and easy to understand. Comment on any obscure object names or code.

### Testing

Developers are encouraged to write and share tests for changes made. Please use the automated testing project provided with the solution to verify changes. Forks will be tested prior to merging.

### Pull Requests

It is asked that you are clear in specifying **WHAT** you changed and **WHY**. With that said there is no formal template for pull requests.
You may however be asked to adjust code if organization doesn't flow or code isn't clear. If you plan to make changes to celestial calculations, an explanation of mathematical changes will be sought.

Please submit Pull Requests to the [Develop Branch](https://github.com/Tronald/CoordinateSharp/tree/develop). The Master branch will only contain stable versions.

## Summary

That's it! Be creative and be innovative. All help is welcome and encouraged!
