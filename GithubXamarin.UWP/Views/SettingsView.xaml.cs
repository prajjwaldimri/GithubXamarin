using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
        private bool IsFirstTimeOpenedRadioButton = true;
        private bool IsFirstTimeOpenedComboBox = true;

        public SettingsView()
        {
            this.InitializeComponent();
            ThemeChecker();
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                StatusBarStackPanel.Visibility = Visibility.Visible;
                StatusBarVisibilityChecker();
            }
            BackgroundTaskStatusChecker();
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

        private void BackgroundTaskStatusChecker()
        {
            var localSettingsValues = ApplicationData.Current.LocalSettings.Values;
            switch (int.Parse(localSettingsValues["BackgroundTaskTime"].ToString()))
            {
                case 15:
                    BackgroundTaskComboBox.SelectedIndex = 0;
                    break;
                case 30:
                    BackgroundTaskComboBox.SelectedIndex = 1;
                    break;
                case 60:
                    BackgroundTaskComboBox.SelectedIndex = 2;
                    break;
                case 360:
                    BackgroundTaskComboBox.SelectedIndex = 3;
                    break;
            }
        }

        private async void StatusBarToggleSwitch_OnToggled(object sender, RoutedEventArgs e)
        {
            var localSettingsValues = ApplicationData.Current.LocalSettings.Values;
            if (IsFirstTimeOpenedRadioButton && (string)localSettingsValues["StatusBarVisibility"] == "Visible")
            {
                IsFirstTimeOpenedRadioButton = false;
                return;
            }
            IsFirstTimeOpenedRadioButton = false;
            var statusBar = StatusBar.GetForCurrentView();
            if (statusBar == null) return;

            switch (StatusBarToggleSwitch.IsOn)
            {
                //Reverses the value in AppData
                case false:
                    localSettingsValues["StatusBarVisibility"] = "Hidden";
                    await statusBar.HideAsync();
                    break;
                case true:
                    localSettingsValues["StatusBarVisibility"] = "Visible";
                    await statusBar.ShowAsync();
                    break;
            }
        }

        private async void DarkThemeRadioButton_OnChecked(object sender, RoutedEventArgs e)
        {
            var localSettingsValues = ApplicationData.Current.LocalSettings.Values;
            if (IsFirstTimeOpenedRadioButton)
            {
                IsFirstTimeOpenedRadioButton = false;
                return;
            }
            localSettingsValues["RequestedTheme"] = "Dark";
            await AfterThemeChangedMessageDialog();
        }

        private async void LightThemeRadioButton_OnChecked(object sender, RoutedEventArgs e)
        {
            var localSettingsValues = ApplicationData.Current.LocalSettings.Values;
            if (IsFirstTimeOpenedRadioButton)
            {
                IsFirstTimeOpenedRadioButton = false;
                return;
            }
            localSettingsValues["RequestedTheme"] = "Light";
            await AfterThemeChangedMessageDialog();
        }

        private async void SystemThemeRadioButton_OnChecked(object sender, RoutedEventArgs e)
        {
            var localSettingsValues = ApplicationData.Current.LocalSettings.Values;
            if (IsFirstTimeOpenedRadioButton)
            {
                IsFirstTimeOpenedRadioButton = false;
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

        private async void BackgroundTaskComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsFirstTimeOpenedComboBox)
            {
                IsFirstTimeOpenedComboBox = false;
                return;
            }
            var localSettings = ApplicationData.Current.LocalSettings.Values;
            var builder = new BackgroundTaskBuilder();
            
            var access = await BackgroundExecutionManager.RequestAccessAsync();
            switch (access)
            {
                case BackgroundAccessStatus.DeniedByUser:
                case BackgroundAccessStatus.DeniedBySystemPolicy:
                    return;
            }

            //UnRegister the previously register task
            const string taskName = "GithubNotificationsBackgroundTask";
            foreach (var taskRegistration in BackgroundTaskRegistration.AllTasks.Values)
            {
                if (taskRegistration != null && taskRegistration.Name == taskName)
                {
                    taskRegistration.Unregister(true);
                }
            }

            builder.Name = taskName;
            builder.IsNetworkRequested = true;
            builder.TaskEntryPoint =
                typeof(Background.GithubNotificationsBackgroundTask).FullName;

            switch (BackgroundTaskComboBox.SelectedIndex)
            {
                case 0:
                    localSettings["BackgroundTaskTime"] = 15;
                    builder.SetTrigger(new TimeTrigger(15, false));
                    break;
                case 1:
                    localSettings["BackgroundTaskTime"] = 30;
                    builder.SetTrigger(new TimeTrigger(30, false));
                    break;
                case 2:
                    localSettings["BackgroundTaskTime"] = 60;
                    builder.SetTrigger(new TimeTrigger(60, false));
                    break;
                case 3:
                    localSettings["BackgroundTaskTime"] = 360;
                    builder.SetTrigger(new TimeTrigger(360, false));
                    break;
            }
            var task = builder.Register();
        }
    }
}
