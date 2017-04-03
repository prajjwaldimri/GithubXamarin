using Windows.UI.Xaml.Controls;
using GithubXamarin.Core.ViewModels;
using MvvmCross.WindowsUWP.Views;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GithubXamarin.UWP.Views
{
    [MvxRegion("MainFrame")]
    public sealed partial class RepositoriesView : MvxWindowsPage
    {
        private new RepositoriesViewModel ViewModel
        {
            get { return (RepositoriesViewModel) base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public RepositoriesView()
        {
            this.InitializeComponent();
            DataContext = ViewModel;
        }

        private async void MainPivot_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainPivot.SelectedIndex == 1)
            {
                await ViewModel.RefreshStarred();
            }
        }

        private void YourRepositoriesList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.NavigateToRepositoryView(null);
        }

        private void StarredRepositoriesList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.NavigateToRepositoryViewStarred(null);
        }
    }
}
