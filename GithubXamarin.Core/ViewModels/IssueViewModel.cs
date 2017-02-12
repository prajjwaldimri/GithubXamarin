using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;
using GithubXamarin.Core.Messages;
using MvvmCross.Plugins.Messenger;
using Octokit;

namespace GithubXamarin.Core.ViewModels
{
    public class IssueViewModel : BaseViewModel, IIssueViewModel
    {
        #region Properties and Commands

        private readonly IIssueDataService _issueDataService;

        private Issue _issue;
        public Issue Issue
        {
            get { return _issue; }
            set
            {
                _issue = value;
                RaisePropertyChanged(() => Issue);
            }
        }

        #endregion


        public IssueViewModel(IGithubClientService githubClientService, IIssueDataService issueDataService, IMvxMessenger messenger, IDialogService dialogService) : base(githubClientService, messenger, dialogService)
        {
            _issueDataService = issueDataService;
        }

        public async void Init(int issueNumber, long repositoryId, string owner = null, string repoName = null)
        {
            if (!IsInternetAvailable())
            {
                await DialogService.ShowDialogASync("What is better ? To be born good or to overcome your evil nature through great effort ?", "No Internet Connection!");
                return;
            }
            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = true });

            if (string.IsNullOrWhiteSpace(owner) || string.IsNullOrWhiteSpace(repoName))
            {
                Issue = await _issueDataService.GetIssueForRepository(repositoryId, issueNumber,
                    GithubClientService.GetAuthorizedGithubClient());
            }
            else
            {
                Issue = await _issueDataService.GetIssueForRepository(owner, repoName, issueNumber,
                    GithubClientService.GetAuthorizedGithubClient());
            }
            Messenger.Publish(new AppBarHeaderChangeMessage(this) { HeaderTitle = $"{Issue.Title}" });
            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = false });
        }
    }
}
