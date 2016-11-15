using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.UI.Xaml.Navigation;
using Octokit;
using Template10.Mvvm;

namespace GithubUWP.ViewModels
{
    public class IssuePageViewModel : ViewModelBase
    {
        public string IssueName { get; set; }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode,
            IDictionary<string, object> state)
        {
            var issue = SessionState.Get<Issue>(parameter.ToString());
            IssueName = issue.Title;
            RaisePropertyChanged(string.Empty);
        }
    }
}
