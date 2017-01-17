using System.Collections.Generic;
using GithubXamarin.Core.Contracts.Service;
using GithubXamarin.Core.Contracts.ViewModel;

namespace GithubXamarin.Core.ViewModels
{
    public class MainViewModel : BaseViewModel, IMainViewModel
    {
        public IEnumerable<string> MenuItems { get; private set; } = new[] {"Option1", "Option2"};

    public MainViewModel(IGithubClientService githubClientService) : base(githubClientService)
        {
        }

        public void ShowEvents()
        {
            ShowViewModel<EventsViewModel>();
        }

        public void ShowLogin()
        {
            ShowViewModel<LoginViewModel>();
        }

        public void ShowViewModelByNavigationDrawerMenuItem(int itemId)
        {
            switch (itemId)
            {
                case 0:
                    ShowViewModel<MainViewModel>();
                    break;
                case 1:
                    ShowViewModel<NotificationsViewModel>();
                    break;
                case 2:
                    ShowViewModel<RepositoriesViewModel>();
                    break;
                case 3:
                    ShowViewModel<IssuesViewModel>();
                    break;
                case 4:
                    //TODO: Gists ViewModel
                    break;
            }
        }
    }
}
