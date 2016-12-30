using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using ColorCode;
using ColorCode.Compilation.Languages;
using Template10.Mvvm;
using Octokit;

namespace GithubUWP.ViewModels
{
    public class FilePageViewModel : ViewModelBase
    {
        public string FileContent { get; set; }

        
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            Views.Busy.SetBusy(true, "Getting Content of File");
            
            GitHubClient client;
            if (SessionState.Get<GitHubClient>("GitHubClient") != null)
            {
                client = SessionState.Get<GitHubClient>("GitHubClient");
            }
            else
            {
                client = new GitHubClient(new ProductHeaderValue("githubuwp"));
                SessionState.Add("GitHubClient", client);
            }
            var fid = SessionState.Get<FileIdentificationData>(parameter.ToString());
            var repoContentClient = new RepositoryContentsClient(new ApiConnection(client.Connection));
            var content = await repoContentClient.GetAllContentsByRef(fid.RepositoryId,fid.Path,fid.Reference);
            //TODO: Cannot display html content except for a webview and the below method returns html content
            FileContent = new CodeColorizer().Colorize(content[0].Content, new CSharp());
            RaisePropertyChanged(String.Empty);
            Views.Busy.SetBusy(false);
        }
    }

    /// <summary>
    /// A class which contains details necessary to identify, display or alter any file.
    /// </summary>
    public class FileIdentificationData
    {
        public long RepositoryId;
        public string Path;
        public string Reference;
    }
}
