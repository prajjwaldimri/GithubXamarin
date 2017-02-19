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
    }
}
