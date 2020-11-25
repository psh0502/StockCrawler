using System.Reflection;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("StockCrawler.UnitTest")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Tom Tang")]
[assembly: AssemblyProduct("StockCrawler.UnitTest")]
[assembly: AssemblyCopyright("Copyright ©  2017")]
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
[assembly: AssemblyVersion("1.1.0.1")]
[assembly: AssemblyFileVersion("1.1.1.1")]
#if(DEBUG)
[assembly: AssemblyDescription("Debug build")]
#else
[assembly: AssemblyDescription("Release build")]
#endif
