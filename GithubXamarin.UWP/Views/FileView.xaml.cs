using GithubXamarin.Core.ViewModels;
using MvvmCross.WindowsUWP.Views;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GithubXamarin.UWP.Views
{
    [MvxRegion("MainFrame")]
    public sealed partial class FileView : MvxWindowsPage
    {
        private new FileViewModel ViewModel
        {
            get { return (FileViewModel) base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public FileView()
        {
            this.InitializeComponent();
            DataContext = ViewModel;
        }
        
    }
}
