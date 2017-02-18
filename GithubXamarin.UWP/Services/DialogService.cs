using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using GithubXamarin.Core.Contracts.Service;

namespace GithubXamarin.UWP.Services
{
    public class DialogService : IDialogService
    {
        public async Task ShowDialogASync(string message, string title)
        {
            var msgDialog = new MessageDialog(message, title);
            await msgDialog.ShowAsync();
        }
    }
}
