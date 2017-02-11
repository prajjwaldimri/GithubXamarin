using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Messages;
using MvvmCross.Plugins.Messenger;
using Octokit;

namespace GithubXamarin.Core.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        #region Properties and Commands

        private readonly IUserDataService _userDataService;
        private readonly IFileDataService _fileDataService;

        //TODO: Get Correct Repo ID
        private const long GithubXamarinRepositoryId = 73414278;

        private ObservableCollection<RepositoryContributor> _contributors;
        public ObservableCollection<RepositoryContributor> Contributors
        {
            get { return _contributors; }
            set { _contributors = value; RaisePropertyChanged(() => Contributors); }
        }

        private string _license;
        public string License
        {
            get { return _license;}
            set { _license = value; RaisePropertyChanged(() => License); }
        }

        #endregion


        public SettingsViewModel(IUserDataService userDataService, IFileDataService fileDataService,
            IGithubClientService githubClientService, IMvxMessenger messenger, 
            IDialogService dialogService) : base(githubClientService, messenger, dialogService)
        {
            _userDataService = userDataService;
            _fileDataService = fileDataService;
            Messenger.Publish(new AppBarHeaderChangeMessage(this) { HeaderTitle = "Settings" });
        }

        public async void Init()
        {
            if (!IsInternetAvailable())
                return;

            Messenger.Publish(new LoadingStatusMessage(this) {IsLoadingIndicatorActive = true});

            Contributors = await _userDataService.GetContributorsForRepository(GithubXamarinRepositoryId,
                GithubClientService.GetAuthorizedGithubClient());

            License = (await _fileDataService.GetFile(GithubXamarinRepositoryId, "LICENSE",
                GithubClientService.GetAuthorizedGithubClient())).Content;

            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = false });
        }
    }
}
