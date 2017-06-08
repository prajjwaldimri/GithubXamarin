using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using MvvmCross.WindowsUWP.Views;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace GithubXamarin.UWP.Views
{
    [MvxRegion("MainFrame")]
    public sealed partial class NewFileView : MvxWindowsPage
    {
        public NewFileView()
        {
            this.InitializeComponent();
        }

        private void SubmitButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var focusObj = FocusManager.GetFocusedElement();
            if (focusObj is TextBox)
            {
                var binding = (focusObj as TextBox).GetBindingExpression(TextBox.TextProperty);
                binding.UpdateSource();
            }
        }
    }
}
