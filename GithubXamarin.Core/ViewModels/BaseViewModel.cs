using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Cheesebaron.MvxPlugins.Connectivity;
using GithubXamarin.Core.Contracts.Service;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Network.Reachability;
using MvvmCross.Plugins.Network.Rest;
using Plugin.Connectivity;

namespace GithubXamarin.Core.ViewModels
{
    public class BaseViewModel : MvxViewModel
    {
        protected readonly IGithubClientService GithubClientService;

        protected BaseViewModel(IGithubClientService githubClientService)
        {
            GithubClientService = githubClientService;
        }

        protected static bool IsInternetAvailable()
        {
            return CrossConnectivity.Current.IsConnected;
        }
    }
}
