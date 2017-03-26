using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GithubXamarin.Core.Contracts.Service;
using MvvmCross.Binding.ExtensionMethods;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using Plugin.Connectivity;

namespace GithubXamarin.Core.ViewModels
{
    public class BaseViewModel : MvxViewModel, IDisposable
    {
        protected IGithubClientService GithubClientService;
        protected IMvxMessenger Messenger;
        protected readonly IDialogService DialogService;

        protected BaseViewModel(IGithubClientService githubClientService, IMvxMessenger messenger, IDialogService dialogService)
        {
            GithubClientService = githubClientService;
            Messenger = messenger;
            DialogService = dialogService;
        }

        protected static async Task<bool> IsInternetAvailable()
        {
            //https://github.com/jamesmontemagno/ConnectivityPlugin
            return CrossConnectivity.Current.IsConnected && await CrossConnectivity.Current.IsRemoteReachable("www.google.com");
        }

        public void Dispose()
        {
            GithubClientService = null;
            Messenger = null;
        }
    }
}