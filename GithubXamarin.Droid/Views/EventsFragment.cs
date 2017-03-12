using Android.OS;
using Android.Runtime;
using Android.Views;
using MvvmCross.Droid.Shared.Attributes;
using GithubXamarin.Core.ViewModels;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V4;

namespace GithubXamarin.Droid.Views
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame, true)]
    [Register("githubxamarin.droid.views.EventsFragment")]
    public class EventsFragment : MvxFragment<EventsViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            return this.BindingInflate(Resource.Layout.EventsView, null);
        }

        public override async void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
        }
    }
}