using Windows.UI.Xaml.Controls;
using GithubXamarin.Core.ViewModels;
using MvvmCross.WindowsUWP.Views;

namespace GithubXamarin.UWP.Views
{
    [MvxRegion("MainFrame")]
    public sealed partial class IssuesView : MvxWindowsPage
    {
        private new IssuesViewModel ViewModel
        {
            get { return (IssuesViewModel) base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public IssuesView()
        {
            this.InitializeComponent();
            DataContext = ViewModel;
        }

        private async void MainPivot_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainPivot.SelectedIndex == 1)
            {
                await ViewModel.RefreshClosed();
            }
        }

        private void OpenIssuesList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.NavigateToIssueView(null);
        }

        private void ClosedIssuesList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.NavigateToIssueViewClosed(null);
        }
    }
}
