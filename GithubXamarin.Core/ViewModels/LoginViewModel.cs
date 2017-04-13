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

        private IFileDataService _fileDataService;

        public LoginViewModel(IGithubClientService githubClientService, IMvxMessenger messenger, IDialogService dialogService, IFileDataService fileDataService) : base(githubClientService, messenger, dialogService)
        {
            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = false });
            Messenger.Publish(new AppBarHeaderChangeMessage(this) { HeaderTitle = "Login" });

            _fileDataService = fileDataService;
        }

        public override async void Start()
        {
            await Task.Delay(500);

            var privacyPolicy = await _fileDataService.GetFile(73414278, "PRIVACY_POLICY.md", GithubClientService.GetUnAuthorizedGithubClient());
            await DialogService.ShowMarkdownDialogAsync(privacyPolicy.Content, "Privacy Policy");

            while (!CrossSecureStorage.Current.HasKey("OAuthToken"))
            {
                await Task.Delay(5000);
            }
            ShowViewModel<EventsViewModel>();
        }
    }
}
