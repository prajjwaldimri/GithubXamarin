using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using GithubXamarin.Core.ViewModels;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Shared.Attributes;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Support.V7.RecyclerView;

namespace GithubXamarin.Droid.Views
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame, true)]
    [Register("githubxamarin.droid.views.IssueFragment")]
    public class IssueFragment : MvxFragment<IssueViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            HasOptionsMenu = true;
            return this.BindingInflate(Resource.Layout.IssueView, null);
        }

        public override async void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            await ViewModel.Refresh();

            var issueLabelsContainer = Activity.FindViewById<MvxRecyclerView>(Resource.Id.issueLabelContainer);
            issueLabelsContainer.SetLayoutManager(new LinearLayoutManager(Context, LinearLayoutManager.Horizontal, false));
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.issue_menu, menu);
            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.issue_refresh:
                    ViewModel.Refresh();
                    break;
                case Resource.Id.issue_edit:
                    ViewModel.GoToNewIssueView();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}