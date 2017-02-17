using System.Reflection;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("TomTang Framework")]
#if(DEBUG)
[assembly: AssemblyDescription("TomTang data access framework(DEBUG).")]
#else
[assembly: AssemblyDescription("TomTang data access framework(RELEASE).")]
#endif
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("TomTang")]
[assembly: AssemblyProduct("TomTang framework core.")]
[assembly: AssemblyCopyright("Copyright 2017, all rights reserved by TomTang")]
[assembly: AssemblyTrademark("TomTang")]
[assembly: AssemblyCulture("")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using '*'.
#if(DEBUG)
[assembly: AssemblyVersion("0.0.0.0")]
[assembly: AssemblyFileVersion("0.0.0.0")]
#else
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
#endif
[assembly: ComVisible(false)]
