using Windows.ApplicationModel.Background;
using Windows.UI.Xaml.Navigation;
using System;

namespace GithubXamarin.UWP.Views
{
    public sealed partial class MainPage : Windows.UI.Xaml.Controls.Page
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }
    }
}
