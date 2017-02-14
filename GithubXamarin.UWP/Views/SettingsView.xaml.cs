using System;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
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
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                StatusBarStackPanel.Visibility = Visibility.Visible;
                StatusBarVisibilityChecker();
            }
        }

        private void StatusBarVisibilityChecker()
        {
            var localSettingsValues = ApplicationData.Current.LocalSettings.Values;
            switch (localSettingsValues["StatusBarVisibility"].ToString())
            {
                case "Visible":
                    StatusBarToggleSwitch.IsOn = true;
                    break;
                case "Hidden":
                    StatusBarToggleSwitch.IsOn = false;
                    break;
            }
        }

        private void ThemeChecker()
        {
            var localSettingsValues = ApplicationData.Current.LocalSettings.Values;
            switch (localSettingsValues["RequestedTheme"].ToString())
            {
                case "Dark":
                    DarkThemeRadioButton.IsChecked = true;
                    break;
                case "Light":
                    LightThemeRadioButton.IsChecked = true;
                    break;
                case "System":
                    SystemThemeRadioButton.IsChecked = true;
                    break;
            }
        }

        private void StatusBarToggleSwitch_OnToggled(object sender, RoutedEventArgs e)
        {
            var localSettingsValues = ApplicationData.Current.LocalSettings.Values;
            if (IsFirstTimeOpened && (string)localSettingsValues["StatusBarVisibility"] == "Visible")
            {
                IsFirstTimeOpened = false;
                return;
            }
            IsFirstTimeOpened = false;
            var statusBar = StatusBar.GetForCurrentView();
            if (statusBar == null) return;

            switch (StatusBarToggleSwitch.IsOn)
            {
                //Reverses the value in AppData
                case false:
                    localSettingsValues["StatusBarVisibility"] = "Hidden";
                    statusBar.HideAsync();
                    break;
                case true:
                    localSettingsValues["StatusBarVisibility"] = "Visible";
                    statusBar.ShowAsync();
                    break;
            }
        }

        private async void DarkThemeRadioButton_OnChecked(object sender, RoutedEventArgs e)
        {
            var localSettingsValues = ApplicationData.Current.LocalSettings.Values;
            if (IsFirstTimeOpened)
            {
                IsFirstTimeOpened = false;
                return;
            }
            localSettingsValues["RequestedTheme"] = "Dark";
            await AfterThemeChangedMessageDialog();
        }

        private async void LightThemeRadioButton_OnChecked(object sender, RoutedEventArgs e)
        {
            var localSettingsValues = ApplicationData.Current.LocalSettings.Values;
            if (IsFirstTimeOpened)
            {
                IsFirstTimeOpened = false;
                return;
            }
            localSettingsValues["RequestedTheme"] = "Light";
            await AfterThemeChangedMessageDialog();
        }

        private async void SystemThemeRadioButton_OnChecked(object sender, RoutedEventArgs e)
        {
            var localSettingsValues = ApplicationData.Current.LocalSettings.Values;
            if (IsFirstTimeOpened)
            {
                IsFirstTimeOpened = false;
                return;
            }
            localSettingsValues["RequestedTheme"] = "System";
            await AfterThemeChangedMessageDialog();
        }

        private async Task AfterThemeChangedMessageDialog()
        {
            var msgDialog = new MessageDialog("The new theme can only be applied after an app restart. Do you want to restart the app?","Theme Changed!");
            msgDialog.Commands.Add(new UICommand("Yes", command => App.Current.Exit()));
            msgDialog.Commands.Add(new UICommand("No"));
            msgDialog.CancelCommandIndex = 1;
            await msgDialog.ShowAsync();
        }
    }
}
