using System;
using System.Linq;
using System.Reflection;

namespace MyLib.Web.Helpers
{
    public static class ApplicationHelper
    {
        /// <summary>
        /// Return version of the current web app
        /// </summary>
        /// <returns></returns>
        public static string GetVersion()
        {
            Assembly assembly = Assembly.GetEntryAssembly();

            String version = assembly.GetName().Version.ToString();

            String productName =
                assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false)
                    .Cast<AssemblyProductAttribute>()
                    .Select(c => c.Product)
                    .FirstOrDefault();

            return productName + " - " + version;
        }
    }
}
