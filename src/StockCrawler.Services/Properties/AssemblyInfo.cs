﻿using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("IRONMAN Services")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("DOWILL Studio")]
[assembly: AssemblyProduct("IRONMAN")]
[assembly: AssemblyCopyright("Copyright ©  2010")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("ce8f676e-3fb4-4ed8-81f9-5715b27aa03c")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
#if(DEBUG)
[assembly: AssemblyVersion("1.1.0.0")]
[assembly: AssemblyFileVersion("1.1.0.0")]
[assembly: AssemblyDescription("Debug build")]
[assembly: InternalsVisibleTo("StockCrawler.UnitTest")]
#else
[assembly: AssemblyVersion("1.1.0.0")]
[assembly: AssemblyFileVersion("1.1.0.0")]
[assembly: AssemblyDescription("Release build")]
#endif