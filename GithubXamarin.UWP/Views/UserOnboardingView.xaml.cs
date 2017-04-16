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
using MvvmCross.WindowsUWP.Views;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace GithubXamarin.UWP.Views
{
    public sealed partial class UserOnboardingView : MvxWindowsPage
    {
        private bool _firstTime = true;

        public UserOnboardingView()
        {
            this.InitializeComponent();
        }

        private void MainFlipView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_firstTime)
            {
                _firstTime = false;
                return;
            }
            switch (MainFlipView.SelectedIndex)
            {
                case 0:
                    FirstRadioButton.IsChecked = true;
                    break;
                case 1:
                    SecondRadioButton.IsChecked = true;
                    break;
                case 2:
                    ThirdRadioButton.IsChecked = true;
                    break;
                case 3:
                    ForthRadioButton.IsChecked = true;
                    break;
                case 4:
                    FifthRadioButton.IsChecked = true;
                    break;
                case 5:
                    SixthRadioButton.IsChecked = true;
                    break;
                default:
                    FirstRadioButton.IsChecked = true;
                    break;
            }
        }
    }
}
