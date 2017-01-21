using System;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace GithubXamarin.UWP.UserControls
{
    public sealed partial class ShowDetailsControl : UserControl
    {
        public ShowDetailsControl()
        {
            this.InitializeComponent();
        }

        public event EventHandler ShowDetailsFinished;
        private void FireShowDetailsFinished()
        {
            ShowDetailsFinished?.Invoke(null, null);
        }
    }
}
