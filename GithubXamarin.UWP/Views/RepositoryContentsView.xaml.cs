using MvvmCross.WindowsUWP.Views;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace GithubXamarin.UWP.Views
{
    [MvxRegion("MainFrame")]
    public sealed partial class RepositoryContentsView : MvxWindowsPage
    {
        public RepositoryContentsView()
        {
            this.InitializeComponent();
        }
    }
}
