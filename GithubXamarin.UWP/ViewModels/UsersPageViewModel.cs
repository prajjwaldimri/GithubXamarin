using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Octokit;
using Template10.Mvvm;

namespace GithubUWP.ViewModels
{
    public class UsersPageViewModel : ViewModelBase
    {
        private object Parameter;
        public string UsersPageHeader { get; set; }
        public ObservableCollection<User> Users { get; set; }

        private DelegateCommand<ItemClickEventArgs> _onUserClickDelegateCommand;
        private DelegateCommand _pullToRefreshDelegateCommand;
        
        public DelegateCommand<ItemClickEventArgs> OnUserClickDelegateCommand
            =>
                _onUserClickDelegateCommand ??
                (_onUserClickDelegateCommand = new DelegateCommand<ItemClickEventArgs>(GoToUser));
        //PullToRefresh Command
        public DelegateCommand PullToRefreshDelegateCommand
            =>
                _pullToRefreshDelegateCommand ??
                (_pullToRefreshDelegateCommand = new DelegateCommand(RefreshList));

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            Views.Busy.SetBusy(true, "Getting Details about Users");
            Parameter = parameter;
            await GetUsers();
            Views.Busy.SetBusy(false);
        }

        private Task GetUsers()
        {
            if (Parameter == null) return Task.CompletedTask;
            Users = SessionState.Get<ObservableCollection<User>>(Parameter.ToString());
            RaisePropertyChanged(string.Empty);
            return Task.CompletedTask;
        }

        private async void GoToUser(ItemClickEventArgs obj)
        {
            var user = (User)obj.ClickedItem;
            const string key = nameof(user);
            if (SessionState.ContainsKey(key) == true)
            {
                SessionState.Remove(key);
            }
            SessionState.Add(key, user);
            await NavigationService.NavigateAsync(typeof(Views.UserProfilePage), key);
        }

        private async void RefreshList()
        {
            Views.Busy.SetBusy(true,"Refreshing");
            await GetUsers();
            Views.Busy.SetBusy(false);
        }
    }
}
