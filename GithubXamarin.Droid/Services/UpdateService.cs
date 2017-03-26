using System;
using Android.App;
using Android.Preferences;
using GithubXamarin.Core.Contracts.Service;

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
            prefsEditor.PutString("VersionNumber", current);
            prefsEditor.Apply();
            return currentVersion.CompareTo(storedVersion) > 0;
        }
    }
}