using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;
using Octokit;

namespace GithubXamarin.Core.ViewModels
{
    public class RepositoryViewModel : BaseViewModel, IRepositoryViewModel
    {
        #region Commands and Properties

        private readonly IRepoDataService _repoDataService;

        private Repository _repository;
        public Repository Repository
        {
            get { return _repository;}
            set
            {
                _repository = value;
                RaisePropertyChanged(() => Repository);
            }
        }

        #endregion


        public RepositoryViewModel(IGithubClientService githubClientService, IRepoDataService repoDataService) : base(githubClientService)
        {
            _repoDataService = repoDataService;
        }

        public override void Start()
        {
            base.Start();
        }

        public async void Init(long repositoryId)
        {
            Repository = await _repoDataService.GetRepository(repositoryId,
                _githubClientService.GetAuthorizedGithubClient());
        }
    }
}
