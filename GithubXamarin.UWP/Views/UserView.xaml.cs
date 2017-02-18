using GithubXamarin.Core.ViewModels;
using MvvmCross.WindowsUWP.Views;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GithubXamarin.UWP.Views
{
    [MvxRegion("MainFrame")]
    public sealed partial class UserView : MvxWindowsPage
    {
        private new UserViewModel ViewModel
        {
            get { return (UserViewModel) base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public UserView()
        {
            this.InitializeComponent();
        }
    }
}
