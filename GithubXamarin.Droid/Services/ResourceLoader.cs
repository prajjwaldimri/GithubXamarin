using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace GithubXamarin.Droid.Services
{
    /// <summary>
    /// A class which provides classes with static resources available in the solution.
    /// </summary>
    public class ResourceLoader
    {
        public static Stream GetEmbeddedResourceStream(Assembly assembly, string resourceFileName)
        {
            var resourceNames = assembly.GetManifestResourceNames();
            var resourcePaths = resourceNames
                .Where(x => x.EndsWith(resourceFileName, StringComparison.CurrentCultureIgnoreCase))
                .ToArray();
            if (!resourcePaths.Any())
            {
                throw new Exception(string.Format($"Resource Ending with {resourceFileName} not found"));
            }

            return assembly.GetManifestResourceStream(resourcePaths.Single());
        }

        public static string GetEmbeddedResourceString(Assembly assembly, string resourceFileName)
        {
            var stream = GetEmbeddedResourceStream(assembly, resourceFileName);

            using (var streamReader = new StreamReader(stream))
            {
                return streamReader.ReadToEnd();
            }
        }
    }
}