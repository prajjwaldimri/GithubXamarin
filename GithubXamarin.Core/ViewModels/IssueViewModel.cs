using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;
using GithubXamarin.Core.Messages;
using GithubXamarin.Core.Utility;
using MvvmCross.Core.ViewModels;
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

        private ICommand _refreshCommand;
        public ICommand RefreshCommand
        {
            get
            {
                _refreshCommand = _refreshCommand ?? new MvxAsyncCommand(async () => await Refresh());
                return _refreshCommand;
            }
        }

        private ICommand _editCommand;
        public ICommand EditCommand
        {
            get
            {
                _editCommand = _editCommand ?? new MvxAsyncCommand(GoToNewIssue);
                return _editCommand;
            }
        }

        private int _issueNumber;
        private long _repositoryId;
        private string _owner;
        private string _repoName;

        #endregion


        public IssueViewModel(IGithubClientService githubClientService, IIssueDataService issueDataService, IMvxMessenger messenger, IDialogService dialogService) : base(githubClientService, messenger, dialogService)
        {
            _issueDataService = issueDataService;
        }

        public async void Init(int issueNumber, long repositoryId, string owner = null, string repoName = null)
        {
            _issueNumber = issueNumber;
            _repositoryId = repositoryId;
            _owner = owner;
            _repoName = repoName;
            await Refresh();
        }

        private async Task Refresh()
        {
            if (!IsInternetAvailable())
            {
                await DialogService.ShowDialogASync("What is better ? To be born good or to overcome your evil nature through great effort ?", "No Internet Connection!");
                return;
            }
            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = true });

            try
            {
                if (string.IsNullOrWhiteSpace(_owner) || string.IsNullOrWhiteSpace(_repoName))
                {
                    Issue = await _issueDataService.GetIssueForRepository(_repositoryId, _issueNumber,
                        GithubClientService.GetAuthorizedGithubClient());
                }
                else
                {
                    Issue = await _issueDataService.GetIssueForRepository(_owner, _repoName, _issueNumber,
                        GithubClientService.GetAuthorizedGithubClient());
                }
                Messenger.Publish(new AppBarHeaderChangeMessage(this) { HeaderTitle = $"{Issue.Title}" });
            }
            catch (HttpRequestException)
            {
                await DialogService.ShowDialogASync("The internet seems to be working but the code threw an HttpRequestException. Try again.", "Hmm, this is weird!");
            }
            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = false });
        }

        private async Task GoToNewIssue()
        {
            if (!IsInternetAvailable())
            {
                await DialogService.ShowDialogASync("There is nothing here.", "Edit What?");
                return;
            }
            ShowViewModel<NewIssueViewModel>(new
            {
                repositoryId = _repositoryId,
                issueNumber = Issue.Number,
                issueTitle = Issue.Title,
                issueBody = Issue.Body,
                labels = ListToCommasSeperatedStringConverter.Convert(Issue.Labels)
            });
        }
    }
}
