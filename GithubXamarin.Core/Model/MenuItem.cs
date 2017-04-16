using System;
using MvvmCross.Core.ViewModels;

namespace GithubXamarin.Core.Model
{
    public class MenuItem : MvxViewModel
    {

        private bool _isSelected;

        public string Title { get; set; }
        public Type ViewModelType { get; set; }
        public MenuOption Option { get; set; }
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                RaisePropertyChanged(() => IsSelected);
            }
        }
    }

    public enum MenuOption
    {
        Events = 0,
        Notifications,
        Settings
    }
}
