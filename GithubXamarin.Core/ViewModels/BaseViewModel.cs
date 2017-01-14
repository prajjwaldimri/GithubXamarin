using GithubXamarin.Core.Contracts.Service;
using MvvmCross.Core.ViewModels;

namespace GithubXamarin.Core.ViewModels
{
    public class BaseViewModel : MvxViewModel
    {
        protected readonly IGithubClientService _githubClientService;

        public BaseViewModel(IGithubClientService githubClientService)
        {
            _githubClientService = githubClientService;
        }
    }
}
