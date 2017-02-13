using System;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using MvvmCross.WindowsUWP.Views;
using Octokit;
using Plugin.SecureStorage;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GithubXamarin.UWP.Views
{
    [MvxRegion("MainFrame")]
    public sealed partial class SettingsView : MvxWindowsPage
    {
        /// <summary>
        /// Checks if the page is opened for the first time.
        /// Used because the toggled event fires automatically on startup and shows a message.
        /// </summary>
        private bool IsFirstTimeOpened = true;

        public SettingsView()
        {
            this.InitializeComponent();
            ThemeChecker();
        }

        private void ThemeChecker()
        {
            var localSettingsValues = ApplicationData.Current.LocalSettings.Values;
            switch (localSettingsValues["RequestedTheme"].ToString())
            {
                case "Dark":
                    ThemeToggleSwitch.IsOn = true;
                    break;
                case "Light":
                    ThemeToggleSwitch.IsOn = false;
                    break;
            }
        }
        
        private async void ThemeToggleSwitch_OnToggled(object sender, RoutedEventArgs e)
        {
            var localSettingsValues = ApplicationData.Current.LocalSettings.Values;
            if (IsFirstTimeOpened && (string) localSettingsValues["RequestedTheme"] == "Dark")
            {
                IsFirstTimeOpened = false;
                return;
            }
            IsFirstTimeOpened = false;
            switch (ThemeToggleSwitch.IsOn)
            {
                //Reverses the value in AppData
                case false:
                    localSettingsValues["RequestedTheme"] = "Light";
                    break;
                case true:
                    localSettingsValues["RequestedTheme"] = "Dark";
                    break;
            }
            var msgDialog = new MessageDialog("Please restart the app to change the current theme!");
            await msgDialog.ShowAsync();
        }
    }
}
