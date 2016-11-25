using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
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
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusBar = StatusBar.GetForCurrentView();
                if (statusBar != null)
                {
                    if (Application.Current.RequestedTheme == ApplicationTheme.Light)
                    {
                        statusBar.BackgroundColor = Windows.UI.Colors.White;
                        statusBar.ForegroundColor = Windows.UI.Colors.Black;
                    }
                    if (Application.Current.RequestedTheme == ApplicationTheme.Dark)
                    {
                        statusBar.BackgroundColor = Windows.UI.Colors.Black;
                        statusBar.ForegroundColor = Windows.UI.Colors.White;
                    }
                }
                await statusBar.ShowAsync();
            }
        }

        private async void HideAsync()
        {
            var statusBar = StatusBar.GetForCurrentView();
            await statusBar.HideAsync();
        }
    }
}
