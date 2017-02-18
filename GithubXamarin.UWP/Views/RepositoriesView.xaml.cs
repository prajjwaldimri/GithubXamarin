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
        }
    }
}
