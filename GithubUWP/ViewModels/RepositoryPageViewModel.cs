using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.UI.Xaml.Navigation;
using Octokit;
using Template10.Mvvm;

namespace GithubUWP.ViewModels
{
    public class RepositoryPageViewModel : ViewModelBase
    {
        public string RepositoryName { get; set; }
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var repository = SessionState.Get<Repository>(parameter.ToString());
            RepositoryName = repository.FullName;
            RaisePropertyChanged(String.Empty);
        }
    }
}
