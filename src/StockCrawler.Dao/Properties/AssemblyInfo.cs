using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("StockCrawler.Dao")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("DOWAY Studio")]
[assembly: AssemblyProduct("StockCrawler Dao")]
[assembly: AssemblyCopyright("Copyright ©  2017")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("fbf87336-a11f-4554-8ef0-65980ac79567")]

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
[assembly: AssemblyVersion("1.1.0.4")]
[assembly: AssemblyFileVersion("1.1.1.5")]
#if(DEBUG)
[assembly: AssemblyDescription("Debug build")]
[assembly: InternalsVisibleTo("StockCrawler.UnitTest")]
#else
[assembly: AssemblyDescription("Release build")]
[assembly: AssemblyKeyFile("DOWAY.snk")]
#endif
