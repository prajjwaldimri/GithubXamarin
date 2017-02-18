using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using GithubXamarin.Core.ViewModels;
using GithubXamarin.UWP.Services;
using GithubXamarin.UWP.UserControls;
using MvvmCross.WindowsUWP.Views;
using Plugin.SecureStorage;

namespace GithubXamarin.UWP
{
    public sealed partial class MainPage : MvxWindowsPage
    {
        public new MainViewModel ViewModel
        {
            get { return (MainViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public Frame AppFrame => (Frame)WrappedFrame.UnderlyingControl;

        public MainPage()
        {
            this.InitializeComponent();
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
            MainFrame.Navigated += (sender, args) =>
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    ((Frame) sender).CanGoBack
                        ? AppViewBackButtonVisibility.Visible
                        : AppViewBackButtonVisibility.Collapsed;
                SyncMenu();
            };
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            NavPaneDivider.Visibility = Visibility.Collapsed;
            //ViewModel handles the navigation to the 0 index on startup so have to manually set this here.
            
            if (!CrossSecureStorage.Current.HasKey("OAuthToken"))
            {
                await ApiKeysManager.KeyRetriever();
            }
        }

        private void TogglePaneButton_Toggle(object sender, RoutedEventArgs e)
        {
            if (HamburgerMenu.IsPaneOpen)
            {
                HamburgerMenu.IsPaneOpen = false;
                return;
            }
            NavPaneDivider.Visibility = Visibility.Visible;
            HamburgerMenu.IsPaneOpen = true;
            FeedbackNavPaneButton.IsTabStop = true;
            SettingsNavPaneButton.IsTabStop = true;
        }

        /// <summary>
        /// Enable accessibility on each nav menu item by setting the AutomationProperties.Name on each container
        /// using the associated Label of each item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void NavMenuList_OnContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            if (!args.InRecycleQueue && args.Item != null && args.Item is NavMenuItem)
            {
                args.ItemContainer.SetValue(AutomationProperties.NameProperty, ((NavMenuItem)args.Item).Label);
            }
            else
            {
                args.ItemContainer.ClearValue(AutomationProperties.NameProperty);
            }
        }

        private void HamburgerMenu_OnPaneClosed(SplitView sender, object args)
        {
            NavPaneDivider.Visibility = Visibility.Collapsed;
            FeedbackNavPaneButton.IsTabStop = false;
            SettingsNavPaneButton.IsTabStop = false;
        }

        private void SettingsNavPaneButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (HamburgerMenu.IsPaneOpen)
            {
                HamburgerMenu.IsPaneOpen = false;
            }
        }

        private void SearchIconButton_OnClick(object sender, RoutedEventArgs e)
        {
            SearchIconButton.Visibility = Visibility.Collapsed;
            SearchBox.Visibility = Visibility.Visible;
            HeaderTextBlock.Visibility = Visibility.Collapsed;
            SearchBox.Focus(FocusState.Programmatic);
        }

        private void SearchBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            SearchBox.Visibility = Visibility.Collapsed;
            SearchIconButton.Visibility = Visibility.Visible;
            HeaderTextBlock.Visibility = Visibility.Visible;
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (!e.Handled && MainFrame.CanGoBack)
            {
                e.Handled = true;
                MainFrame.GoBack();
            }
        }

        private void SyncMenu()
        {
            var content = MainFrame.Content as MvxWindowsPage;
            var index = -1;
            switch (content.BaseUri.Segments[2])
            {
                case "EventsView.xaml":
                    index = 0;
                    break;
                case "NotificationsView.xaml":
                    index = 1;
                    break;
                case "RepositoriesView.xaml":
                    index = 2;
                    break;
                case "IssuesView.xaml":
                    index = 3;
                    break;
                case "GistsView.xaml":
                    index = 4;
                    break;
            }
            NavMenuList.SelectedIndex = -1;
            NavMenuList.SetSelectedItem();
            if (index > -1)
            {
                NavMenuList.SelectedIndex = index;
                NavMenuList.SetSelectedItem(NavMenuList.Items[index] as NavMenuItem);
            }
        }
    }
}
