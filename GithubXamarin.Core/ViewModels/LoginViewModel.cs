using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;
using MvvmCross.Plugins.Messenger;

namespace GithubXamarin.Core.ViewModels
{
    public class LoginViewModel : BaseViewModel, ILoginViewModel
    {
        public LoginViewModel(IGithubClientService githubClientService, IMvxMessenger messenger) : base(githubClientService, messenger)
        {
        }

        public void GoToEvents()
        {
            ShowViewModel<EventsViewModel>();
        }
    }
}
