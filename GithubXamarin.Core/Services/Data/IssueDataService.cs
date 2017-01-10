using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Services.General;
using Octokit;

namespace GithubXamarin.Core.Services.Data
{
    public class IssueDataService : IIssueDataService
    {
        private IGithubClientService _authorizedGithubClient;

        public IssueDataService(IGithubClientService githubClientService)
        {
            _authorizedGithubClient = githubClientService;
        }
    }
}
