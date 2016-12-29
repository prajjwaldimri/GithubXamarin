using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Octokit;
using Template10.Mvvm;

namespace GithubUWP.ViewModels
{
    public class UsersPageViewModel : ViewModelBase
    {
        public string UsersPageHeader { get; set; }
        public ObservableCollection<User> Users { get; set; }

        private DelegateCommand<ItemClickEventArgs> _onUserClickDelegateCommand;

        public DelegateCommand<ItemClickEventArgs> OnUserClickDelegateCommand
            =>
                _onUserClickDelegateCommand ??
                (_onUserClickDelegateCommand = new DelegateCommand<ItemClickEventArgs>(GoToUser));

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            Views.Busy.SetBusy(true,"Getting Details about Users");
            Users = SessionState.Get<ObservableCollection<User>>(parameter.ToString());
            SessionState.Remove(parameter.ToString());
            RaisePropertyChanged(string.Empty);
            Views.Busy.SetBusy(false);
            return Task.CompletedTask;
        }

        private async void GoToUser(ItemClickEventArgs obj)
        {
            var user = (User)obj.ClickedItem;
            const string key = nameof(user);
            SessionState.Add(key, user);
            await NavigationService.NavigateAsync(typeof(Views.UserProfilePage), key);
        }
    }
}
