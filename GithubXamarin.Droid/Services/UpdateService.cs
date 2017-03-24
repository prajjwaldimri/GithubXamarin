using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Preferences;
using GithubXamarin.Core.Contracts.Service;
using Java.Lang;

namespace GithubXamarin.Droid.Services
{
    public class UpdateService : IUpdateService
    {
        public bool IsAppUpdated()
        {
            var current =
                Application.Context.PackageManager.GetPackageInfo(Application.Context.PackageName, 0).VersionName;
            var prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
            var prefsEditor = prefs.Edit();
            if (!(prefs.Contains("VersionNumber")))
            {
                prefsEditor.PutString("VersionNumber", current);
                prefsEditor.Apply();
                return true;
            }
            var currentVersion = new Version(current);
            var storedVersion = new Version(prefs.GetString("VersionNumber","999999"));
            return currentVersion.CompareTo(storedVersion) > 0;
        }
    }
}