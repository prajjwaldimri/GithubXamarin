using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GithubXamarin.Core.Contracts.Service;
using MvvmCross.Plugins.Messenger;

namespace GithubXamarin.Core.ViewModels
{
    public class GistsViewModel : BaseViewModel
    {
        private readonly IGistDataService _gistDataService;

        public GistsViewModel(IGistDataService gistDataService, IGithubClientService githubClientService, IMvxMessenger messenger, IDialogService dialogService) : base(githubClientService, messenger, dialogService)
        {
            _gistDataService = gistDataService;
        }
    }
}
