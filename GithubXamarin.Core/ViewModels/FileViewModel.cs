using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;
using GithubXamarin.Core.Messages;
using GithubXamarin.Core.Model;
using MvvmCross.Plugins.Messenger;

namespace GithubXamarin.Core.ViewModels
{
    public class FileViewModel : BaseViewModel, IFileViewModel
    {
        #region Properties and Commands

        private readonly IFileDataService _fileDataService;

        private string _fileContent;
        public string FileContent
        {
            get { return _fileContent;}
            set { _fileContent = value; RaisePropertyChanged(() => FileContent); }
        }

        private FileTypeEnumeration _fileType;
        private string _filePath;
        private long _repositoryId;
        #endregion

        public FileViewModel(IFileDataService fileDataService, IGithubClientService githubClientService, IMvxMessenger messenger, IDialogService dialogService) : base(githubClientService, messenger, dialogService)
        {
            _fileDataService = fileDataService;
        }

        public async void Init(FileTypeEnumeration fileType, long repositoryId, string filePath = null)
        {
            _fileType = fileType;
            _filePath = filePath;
            _repositoryId = repositoryId;
            await Refresh();
        }

        private async Task Refresh()
        {
            if (!(await IsInternetAvailable()))
            {
                return;
            }

            switch (_fileType)
            {
                case FileTypeEnumeration.Readme:

                    var readme = await _fileDataService.GetReadme(_repositoryId,
                        GithubClientService.GetAuthorizedGithubClient());
                    FileContent = readme.Content;
                    Messenger.Publish(new AppBarHeaderChangeMessage(this) {HeaderTitle = $"{readme.Name}"});

                    break;

                case FileTypeEnumeration.Encoded:
                    
                    break;

                case FileTypeEnumeration.Nonencoded:
                    Messenger.Publish(new AppBarHeaderChangeMessage(this) {HeaderTitle = $"{_filePath}"});
                    FileContent = (await _fileDataService.GetFile(_repositoryId, _filePath,
                        GithubClientService.GetAuthorizedGithubClient())).Content;

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
