using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;
using MvvmCross.Plugins.Messenger;

namespace GithubXamarin.Core.ViewModels
{
    public class SearchResultsViewModel : BaseViewModel, ISearchResultViewModel
    {
        public SearchResultsViewModel(IGithubClientService githubClientService, IMvxMessenger messenger) : base(githubClientService, messenger)
        {
        }
    }
}
