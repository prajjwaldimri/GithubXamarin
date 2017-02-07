using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;
using MvvmCross.Plugins.Network.Reachability;
using MvvmCross.Plugins.Network.Rest;

namespace GithubXamarin.Core.ViewModels
{
    public class SearchResultsViewModel : BaseViewModel, ISearchResultViewModel
    {
        public SearchResultsViewModel(IGithubClientService githubClientService) : base(githubClientService)
        {
        }
    }
}
