using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Platform;
using File = Java.IO.File;
using FileNotFoundException = Java.IO.FileNotFoundException;

namespace GithubXamarin.Droid.Services
{
    public class ApiKeysManager
    {
        public static string GithubClientId;
        public static string GithubClientSecret;


        public static Task KeyRetriever()
        {
            try
            {
                GithubClientId = ResourceLoader.GetEmbeddedResourceString(Assembly.GetAssembly(typeof(ResourceLoader)),
                    "ClientId.txt");
                GithubClientSecret = ResourceLoader.GetEmbeddedResourceString(Assembly.GetAssembly(typeof(ResourceLoader)),
                    "ClientSecret.txt");

            }
            catch (Exception)
            {
                
            }
            return Task.CompletedTask;
        }
    }
}