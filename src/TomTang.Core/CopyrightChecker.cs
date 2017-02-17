using System;
using System.Reflection;

namespace TomTang.Core
{
    /// <summary>
    /// Checker for protection of DOWILL library copyright.
    /// </summary>
    public class CopyrightChecker
    {
        /// <summary>
        /// Check if the invocation came from unauthorized program.
        /// </summary>
        /// <param name="callingAssembly">Pass the calling assembly which is invoking classes in DOWILL library. Pass null as itself.</param>
        /// <exception cref="TomTang.Core.Core.CopyrightViolationException">This invocation came from unauthorized program!</exception>
        public static void CheckCopyright(Assembly callingAssembly)
        {
            if (null == callingAssembly) callingAssembly = Assembly.GetCallingAssembly();
            DebugInfo.WriteLine(callingAssembly.ToString());
            var signs = callingAssembly.ToString().Split(',');
            if (4 != signs.Length) throw new ArgumentException("The fullname of invoker assembly is incorrect format.");
            var p = signs[3].Split('=');
            var ptk = p[1];
            if (Constants.DOWILL_PUBLICTOKEN_KEY != ptk) throw new CopyrightViolationException("This invocation came from unauthorized program!");
        }
    }
}
