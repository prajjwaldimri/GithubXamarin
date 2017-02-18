using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Navigation;
using GithubXamarin.UWP.Services;
using GithubXamarin.UWP.UserControls;
using MvvmCross.WindowsUWP.Views;
using Plugin.SecureStorage;

namespace GithubXamarin.UWP
{
    public sealed partial class MainPage : MvxWindowsPage
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            NavPaneDivider.Visibility = Visibility.Collapsed;
            //ViewModel handles the navigation to the 0 index on startup so have to manually set this here.
            var item = NavMenuList.Items[0] as NavMenuItem;
            NavMenuList.SelectedIndex = 0;
            NavMenuList.SetSelectedItem(item);

            if (!CrossSecureStorage.Current.HasKey("OAuthToken"))
            {
                await ApiKeysManager.KeyRetriever();
            }
        }

        public Frame AppFrame { get { return (Frame) this.WrappedFrame.UnderlyingControl; } }

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
            NavMenuList.SelectedIndex = -1;
            NavMenuList.SetSelectedItem();
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
    }
}
