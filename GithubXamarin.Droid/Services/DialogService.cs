using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GithubXamarin.Core.Contracts.Service;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;

namespace GithubXamarin.Droid.Services
{
    public class DialogService : IDialogService
    {
        protected Activity CurrentActivity => Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

        public Task ShowSimpleDialogAsync(string message, string title)
        {
            return Task.Run(() =>
            {
                Alert(message, title);
            });
        }

        private void Alert(string message, string title)
        {
            Application.SynchronizationContext.Post(ignored =>
            {
                var builder = new AlertDialog.Builder(CurrentActivity);
                builder.SetTitle(title);
                builder.SetMessage(message);
                builder.SetPositiveButton("Close", delegate { });
                builder.Create().Show();
            }, null);
        }

        public async Task<bool> ShowBooleanDialogAsync(string message, string title)
        {
            throw new NotImplementedException();
        }
    }
}