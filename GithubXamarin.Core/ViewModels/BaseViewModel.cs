using System;
using GithubXamarin.Core.Contracts.Service;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using Plugin.Connectivity;

namespace GithubXamarin.Core.ViewModels
{
    public class BaseViewModel : MvxViewModel, IDisposable
    {
        protected IGithubClientService GithubClientService;
        protected IMvxMessenger Messenger;

        protected BaseViewModel(IGithubClientService githubClientService, IMvxMessenger messenger)
        {
            GithubClientService = githubClientService;
            Messenger = messenger;
        }

        protected static bool IsInternetAvailable()
        {
            //https://github.com/jamesmontemagno/ConnectivityPlugin
            return CrossConnectivity.Current.IsConnected;
        }

        public void Dispose()
        {
            GithubClientService = null;
            Messenger = null;
        }
    }
}
