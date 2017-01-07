using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Template10.Mvvm;
using Template10.Services.SettingsService;
using Windows.UI;
using Windows.UI.Xaml;

namespace GithubUWP.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        public SettingsPartViewModel SettingsPartViewModel { get; } = new SettingsPartViewModel();
        public AboutPartViewModel AboutPartViewModel { get; } = new AboutPartViewModel();
    }

    public class SettingsPartViewModel : ViewModelBase
    {
        Services.SettingsServices.SettingsService _settings;

        public SettingsPartViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                // designtime
            }
            else
            {
                _settings = Services.SettingsServices.SettingsService.Instance;
            }
        }

        public bool ShowHamburgerButton
        {
            get { return _settings.ShowHamburgerButton; }
            set { _settings.ShowHamburgerButton = value; base.RaisePropertyChanged(); }
        }

        public bool IsFullScreen
        {
            get { return _settings.IsFullScreen; }
            set
            {
                _settings.IsFullScreen = value;
                base.RaisePropertyChanged();
                if (value)
                {
                    ShowHamburgerButton = false;
                }
                else
                {
                    ShowHamburgerButton = true;
                }
            }
        }

        public bool UseShellBackButton
        {
            get { return _settings.UseShellBackButton; }
            set { _settings.UseShellBackButton = value; base.RaisePropertyChanged(); }
        }

        public bool UseLightThemeButton
        {
            get { return _settings.AppTheme.Equals(ApplicationTheme.Light); }
            set
            {
                if (value)
                {
                    _settings.AppTheme = ApplicationTheme.Light;
                    //Can't switch RequestedTheme on RunTime
                    //Application.Current.RequestedTheme = ApplicationTheme.Light;
                    if (!ApplicationData.Current.RoamingSettings.Values.ContainsKey("CurrentThemeApplied"))
                    {
                        ApplicationData.Current.RoamingSettings.Values.Add("CurrentThemeApplied", "Light");
                    }
                    else
                    {
                        ApplicationData.Current.RoamingSettings.Values.Remove("CurrentThemeApplied");
                        ApplicationData.Current.RoamingSettings.Values.Add("CurrentThemeApplied", "Light");
                    }
                }
                else
                {
                    _settings.AppTheme = ApplicationTheme.Dark;
                    if (!ApplicationData.Current.RoamingSettings.Values.ContainsKey("CurrentThemeApplied"))
                    {
                        ApplicationData.Current.RoamingSettings.Values.Add("CurrentThemeApplied", "Dark");
                    }
                    else
                    {
                        ApplicationData.Current.RoamingSettings.Values.Remove("CurrentThemeApplied");
                        ApplicationData.Current.RoamingSettings.Values.Add("CurrentThemeApplied", "Dark");
                    }
                }
                IsStatusBar = false;
                RaisePropertyChanged("UseLightThemeButton");
                RaisePropertyChanged("IsStatusBar");
            }
        }


        private bool _isStatusBar = false;
        public bool IsStatusBar
        {
            get
            {
                return _isStatusBar;
            }
            set
            {
                _isStatusBar = value;
                base.RaisePropertyChanged("IsStatusBar");
            }
        }
    }

    public class AboutPartViewModel : ViewModelBase
    {
        public Uri Logo => Windows.ApplicationModel.Package.Current.Logo;

        public string DisplayName => Windows.ApplicationModel.Package.Current.DisplayName;

        public string Publisher => Windows.ApplicationModel.Package.Current.PublisherDisplayName;

        public string Version
        {
            get
            {
                var v = Windows.ApplicationModel.Package.Current.Id.Version;
                return $"{v.Major}.{v.Minor}.{v.Build}.{v.Revision}";
            }
        }

        public Uri RateMe => new Uri("http://aka.ms/template10");
    }

}

