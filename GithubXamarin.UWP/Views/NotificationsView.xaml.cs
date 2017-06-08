using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using GithubXamarin.Core.ViewModels;
using MvvmCross.WindowsUWP.Views;
using Octokit;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GithubXamarin.UWP.Views
{
    [MvxRegion("MainFrame")]
    public sealed partial class NotificationsView : MvxWindowsPage
    {
        private new NotificationsViewModel ViewModel
        {
            get { return (NotificationsViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public NotificationsView()
        {
            this.InitializeComponent();
            DataContext = ViewModel;
        }


        private void Select_OnClick(object sender, RoutedEventArgs e)
        {
            MarkButton.Visibility = Visibility.Visible;
            CancelButton.Visibility = Visibility.Visible;
            SelectButton.Visibility = Visibility.Collapsed;
            RefreshButton.Visibility = Visibility.Collapsed;
            SelectAllButton.Visibility = Visibility.Collapsed;
            NotificationsListView.SelectionMode = ListViewSelectionMode.Multiple;
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            MarkButton.Visibility = Visibility.Collapsed;
            CancelButton.Visibility = Visibility.Collapsed;
            SelectButton.Visibility = Visibility.Visible;
            RefreshButton.Visibility = Visibility.Visible;
            SelectAllButton.Visibility = Visibility.Visible;
            NotificationsListView.SelectionMode = ListViewSelectionMode.None;
        }

        private async void MarkButton_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (var notification in NotificationsListView.SelectedItems)
            {
                if (notification is Notification)
                {
                    await ViewModel.MarkNotificationAsRead(notification as Notification);
                    await ViewModel.Refresh();
                }
            }
            CancelButton_OnClick(null, null);
        }

        private void NotificationsListView_OnHolding(object sender, HoldingRoutedEventArgs e)
        {
            MarkButton.Visibility = Visibility.Visible;
            CancelButton.Visibility = Visibility.Visible;
            SelectButton.Visibility = Visibility.Collapsed;
            RefreshButton.Visibility = Visibility.Collapsed;
            SelectAllButton.Visibility = Visibility.Collapsed;
            NotificationsListView.SelectionMode = ListViewSelectionMode.Multiple;
        }
    }
}
