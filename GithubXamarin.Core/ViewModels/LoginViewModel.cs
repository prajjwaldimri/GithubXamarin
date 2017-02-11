using System.Threading.Tasks;
using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;
using GithubXamarin.Core.Messages;
using MvvmCross.Plugins.Messenger;
using Plugin.SecureStorage;

namespace GithubXamarin.Core.ViewModels
{
    public class LoginViewModel : BaseViewModel, ILoginViewModel
    {
        public LoginViewModel(IGithubClientService githubClientService, IMvxMessenger messenger, IDialogService dialogService) : base(githubClientService, messenger, dialogService)
        {
            Messenger.Publish(new LoadingStatusMessage(this) {IsLoadingIndicatorActive = false});
            Messenger.Publish(new AppBarHeaderChangeMessage(this) {HeaderTitle = "Login"});
        }

        public void GoToEvents()
        {
            ShowViewModel<EventsViewModel>();
        }

        public override async void Start()
        {
            while (!CrossSecureStorage.Current.HasKey("OAuthToken"))
            {
                await Task.Delay(500);
            }
            ShowViewModel<EventsViewModel>();
        }
    }
}
