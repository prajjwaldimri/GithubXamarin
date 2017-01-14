using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;

namespace GithubXamarin.Core.ViewModels
{
    public class SearchResultsViewModel : BaseViewModel, ISearchResultViewModel
    {
        public SearchResultsViewModel(IGithubClientService githubClientService) : base(githubClientService)
        {
        }
    }
}
