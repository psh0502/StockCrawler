using System.Reflection;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("IRONMAN.UnitTest")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("DOWILL Studio")]
[assembly: AssemblyProduct("IRONMAN.UnitTest")]
[assembly: AssemblyCopyright("Copyright ©  2010")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM componenets.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("2e70d9aa-0f11-4195-b620-8a7b9134b738")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:
#if(DEBUG)
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("2013.0220.0.0")]
[assembly: AssemblyDescription("Debug build")]
#else
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("2013.0220.1.0")]
[assembly: AssemblyDescription("Release build")]
#endif
