using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using GithubXamarin.Core.Contracts.Service;

namespace GithubXamarin.UWP.Services
{
    public class DialogService : IDialogService
    {
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
    }
}
