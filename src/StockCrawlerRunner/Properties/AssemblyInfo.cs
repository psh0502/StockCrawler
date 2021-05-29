using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("StockCrawlerRunner")]
#if(DEBUG)
[assembly: AssemblyConfiguration("Debug version")]
#else
[assembly: AssemblyConfiguration("Release version")]
#endif
[assembly: AssemblyCompany("DOWAY Studio")]
[assembly: AssemblyProduct("StockCrawlerRunner")]
[assembly: AssemblyCopyright("Copyright ©  2020")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("50f38f51-d1f5-4ef6-8b34-07b3d233c294")]

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
[assembly: AssemblyVersion("1.2.0.0")]
[assembly: AssemblyFileVersion("1.2.2.1")]
#if(DEBUG)
[assembly: AssemblyDescription("Debug build")]
[assembly: InternalsVisibleTo("StockCrawler.UnitTest")]
#else
[assembly: AssemblyDescription("Release build")]
[assembly: AssemblyKeyFile("DOWAY.snk")]
#endif
