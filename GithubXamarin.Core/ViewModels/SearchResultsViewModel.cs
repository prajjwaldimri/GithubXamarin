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
