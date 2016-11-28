using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace GithubUWP.UserControls
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
