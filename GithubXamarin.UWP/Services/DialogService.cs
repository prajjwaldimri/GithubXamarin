using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Controls;
using GithubXamarin.Core.Contracts.Service;
using Microsoft.Toolkit.Uwp.UI.Controls;

namespace GithubXamarin.UWP.Services
{
    public class DialogService : IDialogService
    {
        public Task ShowPopupAsync(string message)
        {
            throw new NotImplementedException();
        }

        public async Task ShowSimpleDialogAsync(string message, string title)
        {
            var contentDialog = new ContentDialog()
            {
                Title = title,
                Content = message,
                PrimaryButtonText = "Alrighty Then!",
                SecondaryButtonText = ""
            };
            await contentDialog.ShowAsync();
        }

        public async Task<bool> ShowBooleanDialogAsync(string message, string title)
        {
            var contentDialog = new ContentDialog()
            {
                Title = title,
                Content = message,
                PrimaryButtonText = "Yup",
                SecondaryButtonText = "Nope"
            };
            var result = await contentDialog.ShowAsync();
            return result == ContentDialogResult.Primary;
        }

        public async Task ShowMarkdownDialogAsync(string markdown, string title)
        {
            var version = Package.Current.Id.Version;
            if (string.IsNullOrWhiteSpace(title))
            {
                title = $"What's new in v.{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            }
            var contentDialog = new ContentDialog
            {
                Title = title,
                Content = new MarkdownTextBlock() {Text = markdown},
                PrimaryButtonText = "Cool"
            };
            await contentDialog.ShowAsync();
        }
    }
}
