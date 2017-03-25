using System;
using Windows.ApplicationModel;
using Windows.Storage;
using GithubXamarin.Core.Contracts.Service;

namespace GithubXamarin.UWP.Services
{
    public class UpdateService : IUpdateService
    {
        public bool IsAppUpdated()
        {
            var current = Package.Current.Id.Version;
            var localSettingValues = ApplicationData.Current.LocalSettings.Values;
            if (!(localSettingValues.ContainsKey("VersionNumber")))
            {
                localSettingValues["VersionNumber"] = $"{current.Major}.{current.Minor}.{current.Build}.{current.Revision}";
                return true;
            }
            var currentVersion = new Version($"{current.Major}.{current.Minor}.{current.Build}.{current.Revision}");
            var storedVersion = new Version(localSettingValues["VersionNumber"].ToString());
            localSettingValues["VersionNumber"] = $"{current.Major}.{current.Minor}.{current.Build}.{current.Revision}";
            return currentVersion.CompareTo(storedVersion) > 0;
        }
    }
}
