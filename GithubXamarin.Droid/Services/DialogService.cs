using System.Threading.Tasks;
using Android.App;
using Android.Support.Design.Widget;
using GithubXamarin.Core.Contracts.Service;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;

namespace GithubXamarin.Droid.Services
{
    public class DialogService : IDialogService
    {
        public async Task ShowPopupAsync(string message)
        {
            await Task.Run(() =>
            {
                var view = CurrentActivity.FindViewById(Android.Resource.Id.Content);
                Snackbar.Make(view, message, Snackbar.LengthShort)
                .Show();
            });
        }

        private Activity CurrentActivity => Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

        public Task ShowSimpleDialogAsync(string message, string title)
        {
            return Task.Run(() =>
            {
                Application.SynchronizationContext.Post(ignored =>
                {
                    var builder = new AlertDialog.Builder(CurrentActivity);
                    builder.SetTitle(title);
                    builder.SetMessage(message);
                    builder.SetPositiveButton("Close", delegate { });
                    builder.Create().Show();
                }, null);
            });
        }

        public async Task<bool> ShowBooleanDialogAsync(string message, string title)
        {
            bool? result = null;
            Task.Run(() =>
            {
                Application.SynchronizationContext.Post(ignored =>
                {
                    var builder = new AlertDialog.Builder(CurrentActivity);
                    builder.SetTitle(title);
                    builder.SetMessage(message);
                    builder.SetCancelable(true);
                    builder.SetPositiveButton("Yup", (sender, args) =>
                    {
                        result = true;
                    });
                    builder.SetNegativeButton("Nope", (sender, args) => { result = false; });
                    builder.Create().Show();
                }, null);
            });

            // HACK: Waits for user to accept any option. Without this delay the method will return null.
            while (!(result.HasValue))
            {
                await Task.Delay(1000);
            }

            return result.Value;
        }

        public Task ShowMarkdownDialogAsync(string markdown, string title)
        {
            return Task.Run(() =>
            {
                Application.SynchronizationContext.Post(ignored =>
                {
                    var builder = new AlertDialog.Builder(CurrentActivity);
                    builder.SetTitle(title);
                    builder.SetMessage(markdown);
                    builder.SetPositiveButton("Close", delegate { });
                    builder.Create().Show();
                }, null);
            });
        }
    }
}