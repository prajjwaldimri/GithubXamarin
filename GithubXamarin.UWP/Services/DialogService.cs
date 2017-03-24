using System;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using GithubXamarin.Core.Contracts.Service;
using RavinduL.LocalNotifications.Presenters;

namespace GithubXamarin.UWP.Services
{
    public class DialogService : IDialogService
    {
        public async Task ShowPopupAsync(string message)
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
    }
}
