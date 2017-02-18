using GithubXamarin.Core.ViewModels;
using MvvmCross.WindowsUWP.Views;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GithubXamarin.UWP.Views
{
    [MvxRegion("MainFrame")]
    public sealed partial class NotificationsView : MvxWindowsPage
    {
        private new NotificationsViewModel ViewModel
        {
            get { return (NotificationsViewModel) base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public NotificationsView()
        {
            this.InitializeComponent();
        }
    }
}
