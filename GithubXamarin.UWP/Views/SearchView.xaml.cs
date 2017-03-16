using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GithubXamarin.Core.ViewModels;
using MvvmCross.WindowsUWP.Views;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GithubXamarin.UWP.Views
{
    [MvxRegion("MainFrame")]
    public sealed partial class SearchView : MvxWindowsPage
    {
        private new SearchViewModel ViewModel
        {
            get { return (SearchViewModel) base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public SearchView()
        {
            this.InitializeComponent();
            DataContext = ViewModel;
            FilterComboBox.SelectedIndex = 0;
        }

        private void FilterComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch ((FilterComboBox.SelectedItem as ComboBoxItem).Content.ToString())
            {
                case "Issues":
                    IssuesListView.Visibility = Visibility.Visible;
                    IssuesListView.SelectionMode = ListViewSelectionMode.None;
                    IssuesListView.SelectionMode = ListViewSelectionMode.Single;
                    RepositoriesListView.Visibility = Visibility.Collapsed;
                    UsersListView.Visibility = Visibility.Collapsed;
                    break;
                case "Repos":
                    RepositoriesListView.Visibility = Visibility.Visible;
                    RepositoriesListView.SelectionMode = ListViewSelectionMode.None;
                    RepositoriesListView.SelectionMode = ListViewSelectionMode.Single;
                    IssuesListView.Visibility = Visibility.Collapsed;
                    UsersListView.Visibility = Visibility.Collapsed;
                    break;
                case "Users":
                    UsersListView.Visibility = Visibility.Visible;
                    UsersListView.SelectionMode = ListViewSelectionMode.None;
                    UsersListView.SelectionMode = ListViewSelectionMode.Single;
                    IssuesListView.Visibility = Visibility.Collapsed;
                    RepositoriesListView.Visibility = Visibility.Collapsed;
                    break;
            }
        }
    }
}
