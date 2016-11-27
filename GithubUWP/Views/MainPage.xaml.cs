using System;
using GithubUWP.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;
using Windows.UI;
using GithubUWP.UserControls;
using System.Threading.Tasks;
using Octokit;

namespace GithubUWP.Views
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
