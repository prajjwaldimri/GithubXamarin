using GithubXamarin.Core.ViewModels;
using MvvmCross.WindowsUWP.Views;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GithubXamarin.UWP.Views
{
    [MvxRegion("MainFrame")]
    public sealed partial class RepositoryView : MvxWindowsPage
    {
        private new RepositoryViewModel ViewModel
        {
            get { return (RepositoryViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public RepositoryView()
        {
            this.InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
