using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Configuration;

namespace TomTang.Core.CodeDom
{
    /// <summary>
    /// Help class for generating dynamic assembly for C#.
    /// </summary>
    public class DynamicAssemblyGenerator
    {
        /// <summary>
        /// Prevent it from been initiated.
        /// </summary>
        private DynamicAssemblyGenerator() { }
        /// <summary>
        /// Generate a physic dynamic assembly file.
        /// </summary>
        /// <param name="codeSnipplet">C# source code content string</param>
        /// <param name="destAsmFile">Indicate the destination of dynamic assembly output file</param>
        /// <param name="referedDLLs">List of referenced DLL in string array</param>
        public static void GenerateAssemblyFromCodeSnipplet(ref string codeSnipplet, string destAsmFile, ref string[] referedDLLs)
        {
            CopyrightChecker.CheckCopyright(Assembly.GetCallingAssembly());
            generateAssembly(ref codeSnipplet, ref referedDLLs, destAsmFile);
        }
        /// <summary>
        /// Generate a physic dynamic assembly instance.
        /// </summary>
        /// <param name="codeSnipplet"></param>
        /// <param name="referedDLLs"></param>
        /// <returns></returns>
        public static Assembly GenerateAssemblyFromCodeSnipplet(ref string codeSnipplet, ref string[] referedDLLs)
        {
            CopyrightChecker.CheckCopyright(Assembly.GetCallingAssembly());
            Assembly asm = Assembly.Load(generateAssembly(ref codeSnipplet, ref referedDLLs, null));
            return asm;
        }
        /// <summary>
        /// Generate a physic dynamic assembly image in byte array.
        /// </summary>
        /// <param name="codeSnipplet"></param>
        /// <param name="referedDLLs"></param>
        /// <returns></returns>
        public static byte[] GenerateAssemblyBytesFromCodeSnipplet(ref string codeSnipplet, ref string[] referedDLLs)
        {
            CopyrightChecker.CheckCopyright(Assembly.GetCallingAssembly());
            return generateAssembly(ref codeSnipplet, ref referedDLLs, null);
        }

        private static byte[] generateAssembly(ref string codeSnipplet, ref string[] referedDLLs, string destination)
        {
            // http://blog.miniasp.com/post/2011/03/25/How-to-check-Web-environment-in-the-Class-Library.aspx
            bool isWebAppProcess = (HttpRuntime.AppDomainAppId != null);

            // Compiling the source file to an assembly DLL (IL code)
            var cc = CodeDomProvider.CreateProvider("CSharp");
            var cpar = new CompilerParameters();
            cpar.ReferencedAssemblies.Add("System.dll");
            string destFileName = (string.IsNullOrEmpty(destination)) ? string.Format("{0}.DLL", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")) : destination;
            if (isWebAppProcess)
            {
                string genFolder = ConfigurationManager.AppSettings["GEN_FOLDER"];
                destFileName = HttpContext.Current.Server.MapPath(genFolder + "/" + destFileName);
                System.Diagnostics.Debug.WriteLine(string.Format("generateAssembly:{0}", destFileName));
            }
            File.Delete(destFileName);
            cpar.OutputAssembly = destFileName;
            FileInfo asmFile = new FileInfo(destFileName);
            cpar.TempFiles = new TempFileCollection(asmFile.Directory.FullName, false);
            if (null != referedDLLs) foreach (var rdll in referedDLLs) cpar.ReferencedAssemblies.Add(rdll);
            cpar.GenerateExecutable = false;

            cpar.GenerateInMemory = false;

            var compRes = cc.CompileAssemblyFromSource(cpar, codeSnipplet);
            if (0 != compRes.NativeCompilerReturnValue)
            {
                StringBuilder errorBuilder = new StringBuilder();
                foreach (CompilerError err in compRes.Errors)
                {
                    if (!err.IsWarning) errorBuilder.AppendLine(err.ErrorText);
                }
                throw new FileLoadException(string.Format("Compiler failed by returning {0}. ERROR={1}",
                    compRes.NativeCompilerReturnValue,
                    errorBuilder.ToString()));
            }
            byte[] asmBytes = null;
            asmFile.Refresh();
            if (asmFile.Exists)
            {
                using (FileStream fs = asmFile.OpenRead())
                {
                    int len = Convert.ToInt32(fs.Length);
                    asmBytes = new byte[len];
                    fs.Read(asmBytes, 0, len);
                }

                // Delete temp assembly file if there's no destination assignment.
                if (string.IsNullOrEmpty(destination)) asmFile.Delete();
            }
            else
            {
                throw new FileNotFoundException("Can't find the generated assembly file.", asmFile.FullName);
            }
            return asmBytes;
        }
    }
}
