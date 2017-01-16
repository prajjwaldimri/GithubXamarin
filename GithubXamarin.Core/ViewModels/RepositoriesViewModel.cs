using System.Collections.ObjectModel;
using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;
using Octokit;

namespace GithubXamarin.Core.ViewModels
{
    public class RepositoriesViewModel : BaseViewModel, IRepositoriesViewModel
    {
        #region Commands and Properties

        private readonly IRepoDataService _repoDataService;

        private ObservableCollection<Repository> _repositories;
        public ObservableCollection<Repository> Repositories
        {
            get { return _repositories; }
            set
            {
                _repositories = value;
                RaisePropertyChanged(() => Repositories);
            }
        }

        #endregion


        public RepositoriesViewModel(IGithubClientService githubClientService, IRepoDataService repoDataService) : base(githubClientService)
        {
            _repoDataService = repoDataService;
        }

        public override void Start()
        {
            base.Start();
        }

        public async void Init(string userLogin)
        {
            if (string.IsNullOrWhiteSpace(userLogin))
            {
                Repositories =
                    await _repoDataService.GetAllRepositoriesForCurrentUser(
                        _githubClientService.GetAuthorizedGithubClient());
            }
            else
            {
                Repositories = await _repoDataService.GetAllRepositoriesForUser(userLogin,
                    _githubClientService.GetAuthorizedGithubClient());
            }
        }
    }
}
