using System;
using System.Threading.Tasks;
using System.Windows.Input;
using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Messages;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using Octokit;

namespace GithubXamarin.Core.ViewModels
{
    public class GistViewModel : BaseViewModel
    {
        #region Properties and Commands

        private readonly IGistDataService _gistDataService;
        private readonly IShareService _shareService;

        private Gist _gist;
        public Gist Gist
        {
            get => _gist;
            set { _gist = value; RaisePropertyChanged(() => Gist); }
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

        private ICommand _shareCommand;
        public ICommand ShareCommand
        {
            get
            {
                _shareCommand = _shareCommand ?? new MvxAsyncCommand(ShareGist);
                return _shareCommand;
            }
        }

        private string _gistId;

        #endregion

        public GistViewModel(IGistDataService gistDataService, IGithubClientService githubClientService, IMvxMessenger messenger, IDialogService dialogService, IShareService shareService) : base(githubClientService, messenger, dialogService)
        {
            _gistDataService = gistDataService;
            _shareService = shareService;
        }

        public async void Init(string gistId)
        {
            _gistId = gistId;
            await Refresh();
        }

        private async Task Refresh()
        {
            if (!(await IsInternetAvailable()) || string.IsNullOrWhiteSpace(_gistId))
            {
                return;
            }
            Gist = await _gistDataService.GetGist(_gistId, GithubClientService.GetAuthorizedGithubClient());
            Messenger.Publish(new AppBarHeaderChangeMessage(this) { HeaderTitle = $"Files in {Gist.Description}" });
        }

        private async Task ShareGist()
        {
            if (Gist == null) return;
            _shareService.ShareLinkAsync(new Uri(Gist.HtmlUrl), Gist.Description);
        }
    }
}
