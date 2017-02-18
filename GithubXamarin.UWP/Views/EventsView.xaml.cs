using GithubXamarin.Core.ViewModels;
using MvvmCross.WindowsUWP.Views;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GithubXamarin.UWP.Views
{
    [MvxRegion("MainFrame")]
    public sealed partial class EventsView : MvxWindowsPage
    {
        private new EventsViewModel ViewModel
        {
            get { return (EventsViewModel) base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public EventsView()
        {
            this.InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
