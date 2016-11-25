using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace GithubUWP.Services.Converters
{
    public class StatusBarToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool)
            {
                if ((bool)value == false)
                {
                    HideAsync();
                    return value;
                }

                else if ((bool)value == true)
                {
                    ShowAsync();
                    return value;
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is bool)
            {
                if ((bool)value == false)
                {
                    HideAsync();
                    return value;
                }

                else if ((bool)value == true)
                {
                    ShowAsync();
                    return value;
                }
            }
            return value;
        }

        private async void ShowAsync()
        {
            if (!ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar")) return;
            var statusBar = StatusBar.GetForCurrentView();
            if (statusBar != null)
            {
                switch (Application.Current.RequestedTheme)
                {
                    case ApplicationTheme.Light:
                        statusBar.BackgroundColor = Windows.UI.Colors.White;
                        statusBar.ForegroundColor = Windows.UI.Colors.Black;
                        break;
                    case ApplicationTheme.Dark:
                        statusBar.BackgroundColor = Windows.UI.Colors.Black;
                        statusBar.ForegroundColor = Windows.UI.Colors.White;
                        break;
                }
                switch ((string) ApplicationData.Current.RoamingSettings.Values["CurrentThemeApplied"])
                {
                    case "Light":
                        statusBar.BackgroundColor = Windows.UI.Colors.White;
                        statusBar.ForegroundColor = Windows.UI.Colors.Black;
                        break;
                    case "Dark":
                        statusBar.BackgroundColor = Windows.UI.Colors.Black;
                        statusBar.ForegroundColor = Windows.UI.Colors.White;
                        break;
                }
            }
            await statusBar.ShowAsync();
        }

        private async void HideAsync()
        {
            var statusBar = StatusBar.GetForCurrentView();
            await statusBar.HideAsync();
        }
    }
}
