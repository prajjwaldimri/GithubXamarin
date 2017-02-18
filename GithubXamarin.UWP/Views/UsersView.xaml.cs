using GithubXamarin.Core.ViewModels;
using MvvmCross.WindowsUWP.Views;

namespace GithubXamarin.UWP.Views
{
    [MvxRegion("MainFrame")]
    public sealed partial class UsersView : MvxWindowsPage
    {
        public new UsersViewModel ViewModel
        {
            get { return (UsersViewModel) base.ViewModel; }
            set { base.ViewModel = value; }
        }
        public UsersView()
        {
            this.InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
