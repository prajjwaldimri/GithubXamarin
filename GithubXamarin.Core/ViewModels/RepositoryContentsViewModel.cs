using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Messages;
using GithubXamarin.Core.Model;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using Octokit;

namespace GithubXamarin.Core.ViewModels
{
    public class RepositoryContentsViewModel : BaseViewModel
    {
        #region Commands and Properties

        private readonly IRepoDataService _repoDataService;
        private readonly IShareService _shareService;

        private ObservableCollection<RepositoryContent> _content;
        public ObservableCollection<RepositoryContent> Content
        {
            get => _content;
            set
            {
                _content = value;
                RaisePropertyChanged(() => Content);
            }
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                _selectedIndex = value;
                RaisePropertyChanged(() => SelectedIndex);
            }
        }


        private string _path;
        public string Path
        {
            get => _path;
            set
            {
                _path = value;
                RaisePropertyChanged(() => Path);
            }
        }

        private long _repoId;
        public long RepoId
        {
            get => _repoId;
            set
            {
                _repoId = value;
                RaisePropertyChanged(() => RepoId);
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

        private ICommand _contentClickCommand;
        public ICommand ContentClickCommand
        {
            get
            {
                _contentClickCommand = _contentClickCommand ?? new MvxCommand<object>(ContentClick);
                return _contentClickCommand;
            }
        }

        private ICommand _addFileCommand;
        public ICommand AddFileCommand
        {
            get
            {
                _addFileCommand = _addFileCommand ?? new MvxCommand(GoToNewFileView);
                return _addFileCommand;
            }
        }


        #endregion

        public RepositoryContentsViewModel(IGithubClientService githubClientService, IMvxMessenger messenger,
            IDialogService dialogService, IRepoDataService repoDataService, IShareService shareService)
            : base(githubClientService, messenger, dialogService)
        {
            _repoDataService = repoDataService;
            _shareService = shareService;
        }

        public async void Init(long repositoryId, string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                Path = path;
            }
            RepoId = repositoryId;
            await Refresh();
        }

        private void ContentClick(object selectedContent)
        {
            var content = selectedContent as RepositoryContent ?? Content[SelectedIndex];

            switch (content.Type)
            {
                case ContentType.File:
                    ShowViewModel<FileViewModel>(new
                    {
                        fileType = FileTypeEnumeration.Nonencoded,
                        repositoryId = RepoId,
                        filePath = content.Path
                    });
                    break;

                case ContentType.Dir:
                    ShowViewModel<RepositoryContentsViewModel>(new
                    {
                        repositoryId = RepoId,
                        path = content.Path
                    });
                    break;

                case ContentType.Symlink:
                    break;
                case ContentType.Submodule:
                    break;
                default:
                    break;
            }
        }

        private void GoToNewFileView()
        {
            ShowViewModel<NewFileViewModel>(new
            {
                repositoryId = RepoId,
                filePath = Path
            });
        }

        public async Task Refresh()
        {
            if (!await IsInternetAvailable())
            {
                await DialogService.ShowSimpleDialogAsync("Check your internet connection.", "Internet Not Available!");
                return;
            }

            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = true });

            if (string.IsNullOrWhiteSpace(Path))
            {
                Messenger.Publish(new AppBarHeaderChangeMessage(this) { HeaderTitle = $"Content in Root Directory" });
                Content =
                    await _repoDataService.GetContentsOfRepository(RepoId,
                        GithubClientService.GetAuthorizedGithubClient());
            }
            else
            {
                Messenger.Publish(new AppBarHeaderChangeMessage(this) { HeaderTitle = $"Content in {Path}" });
                Content =
                    await _repoDataService.GetContentsOfRepository(RepoId,
                        GithubClientService.GetAuthorizedGithubClient(), Path);
            }

            Messenger.Publish(new LoadingStatusMessage(this) { IsLoadingIndicatorActive = false });
        }
    }
}
