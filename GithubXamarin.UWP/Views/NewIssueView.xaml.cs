using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using MvvmCross.WindowsUWP.Views;
using Octokit;

namespace GithubXamarin.UWP.Views
{
    [MvxRegion("MainFrame")]
    public sealed partial class NewIssueView : MvxWindowsPage
    {
        public NewIssueView()
        {
            this.InitializeComponent();
        }

        private void SubmitButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var focusObj = FocusManager.GetFocusedElement();
            if (focusObj is TextBox)
            {
                var binding = (focusObj as TextBox).GetBindingExpression(TextBox.TextProperty);
                binding?.UpdateSource();
            }
        }

        private void MilestoneClearButton_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            MilestonesComboBox.SelectedIndex = -1;
        }

        private void LabelsAutoSuggestBox_OnSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            var selectedLabel = args.SelectedItem as Label;
            var text = LabelsAutoSuggestBox.Text;
            if (!string.IsNullOrWhiteSpace(text) && text[text.Length - 1] != ',')
            {
                LabelsAutoSuggestBox.Text += $",{selectedLabel.Name}";
            }
            else
            {
                LabelsAutoSuggestBox.Text += $"{selectedLabel.Name}";
            }
        }

        private void AssigneesAutoSuggestBox_OnSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            var selectedUser = args.SelectedItem as User;
            var text = AssigneesAutoSuggestBox.Text;

            if (!string.IsNullOrWhiteSpace(text) && text[text.Length - 1] != ',')
            {
                AssigneesAutoSuggestBox.Text += $",{selectedUser.Login}";
            }
            else
            {
                AssigneesAutoSuggestBox.Text += $"{selectedUser.Login}";
            }
        }
    }
}
