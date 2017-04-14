using System.Collections.ObjectModel;
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
        private readonly IShareService _shareService;

        private Issue _issue;
        public Issue Issue
        {
            get => _issue;
            set
            {
                _issue = value;
                RaisePropertyChanged(() => Issue);
            }
        }

        private ObservableCollection<IssueComment> _comments;
        public ObservableCollection<IssueComment> Comments
        {
            get => _comments;
            set
            {
                _comments = value;
                RaisePropertyChanged(() => Comments);
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
                _editCommand = _editCommand ?? new MvxAsyncCommand(GoToNewIssueView);
                return _editCommand;
            }
        }

        private ICommand _shareCommand;
        public ICommand ShareCommand
        {
            get
            {
                _shareCommand = _shareCommand ?? new MvxAsyncCommand(ShareIssue);
                return _shareCommand;
            }
        }

        private int _issueNumber;
        private long _repositoryId;
        private string _owner;
        private string _repoName;

        #endregion


        public IssueViewModel(IGithubClientService githubClientService, IIssueDataService issueDataService, IMvxMessenger messenger, IDialogService dialogService, IShareService shareService) : base(githubClientService, messenger, dialogService)
        {
            _issueDataService = issueDataService;
            _shareService = shareService;
        }

        public async void Init(int issueNumber, long repositoryId, string owner = null, string repoName = null)
        {
            _issueNumber = issueNumber;
            _repositoryId = repositoryId;
            _owner = owner;
            _repoName = repoName;
            await Refresh();
        }

        public async Task Refresh()
        {
            if (!(await IsInternetAvailable()))
            {
                await DialogService.ShowSimpleDialogAsync("What is better ? To be born good or to overcome your evil nature through great effort ?", "No Internet Connection!");
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
                await DialogService.ShowSimpleDialogAsync("The internet seems to be working but the code threw an HttpRequestException. Try again.", "Hmm, this is weird!");
            }

            await GetCommentsForIssues();
            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = false });
        }

        private async Task GetCommentsForIssues()
        {
            Comments = await _issueDataService.GetCommentsForIssue(_repositoryId, _issueNumber,
                GithubClientService.GetAuthorizedGithubClient());
        }

        public async Task GoToNewIssueView()
        {
            if (!(await IsInternetAvailable()))
            {
                await DialogService.ShowSimpleDialogAsync("There is nothing here.", "Edit What?");
                return;
            }

            string milestoneTitle = null;

            if (Issue.Milestone != null)
            {
                milestoneTitle = Issue.Milestone.Title;
            }

            ShowViewModel<NewIssueViewModel>(new
            {
                repositoryId = _repositoryId,
                issueNumber = Issue.Number,
                issueTitle = Issue.Title,
                issueBody = Issue.Body,
                labels = ListToCommasSeperatedStringConverter.Convert(Issue.Labels),
                assignees = ListToCommasSeperatedStringConverter.Convert(Issue.Assignees),
                milestone = milestoneTitle
            });
        }

        private async Task ShareIssue()
        {
            if (Issue == null) return;
            await _shareService.ShareLinkAsync(Issue.HtmlUrl, Issue.Title);
        }
    }
}
