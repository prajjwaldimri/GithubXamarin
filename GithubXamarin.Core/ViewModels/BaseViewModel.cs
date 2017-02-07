using GithubXamarin.Core.Contracts.Service;
using MvvmCross.Core.ViewModels;
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
