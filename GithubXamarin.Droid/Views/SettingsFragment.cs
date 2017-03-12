using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Shared.Attributes;
using GithubXamarin.Core.ViewModels;
using GithubXamarin.Droid.Activities;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V4;

namespace GithubXamarin.Droid.Views
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame, true)]
    [Register("githubxamarin.droid.views.SettingsFragment")]
    public class SettingsFragment : MvxFragment<SettingsViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            return this.BindingInflate(Resource.Layout.SettingsView, null);
        }
    }
}