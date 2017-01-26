using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;

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
