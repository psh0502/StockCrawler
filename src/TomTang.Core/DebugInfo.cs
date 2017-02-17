using System;
using System.Reflection;
using System.Diagnostics;

namespace TomTang.Core
{
	/// <summary>
	/// In Debugging mode, show some formatted info for developer
	/// </summary>
	public class DebugInfo
	{
		/// <summary>
		/// In DEBUG mode only, use outputDebugString to show some information, as "[Timestamp] File Name : Method Name : Message"
		/// </summary>
        /// <param name="message">Debug Message</param>
		public static void WriteLine(string message)
		{
#if (DEBUG)
			StackTrace oStackTrace = new StackTrace();
			StackFrame oStack = oStackTrace.GetFrame(1);
            Debug.WriteLine(string.Format("[{0}] {1} : {2} : {3}", 
                DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), 
                oStack.GetFileName(), 
                oStack.GetMethod().Name,
                message));
#endif
		}
	}
}
