using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;
using MvvmCross.Plugins.Network.Reachability;
using MvvmCross.Plugins.Network.Rest;

namespace GithubXamarin.Core.ViewModels
{
    public class LoginViewModel : BaseViewModel, ILoginViewModel
    {
        public LoginViewModel(IGithubClientService githubClientService) : base(githubClientService)
        {
        }

        public void GoToEvents()
        {
            ShowViewModel<EventsViewModel>();
        }
    }
}
