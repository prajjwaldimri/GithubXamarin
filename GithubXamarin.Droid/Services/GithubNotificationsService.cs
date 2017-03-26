using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Humanizer;
using Octokit;
using Plugin.Connectivity;
using Plugin.SecureStorage;
using Credentials = Octokit.Credentials;

namespace GithubXamarin.Droid.Services
{
    [Service]
    public class GithubNotificationsService : IntentService
    {
        private string _title;
        private string _text;
        private string _toastLogo;
        private Credentials _passwordCredential;

        public GithubNotificationsService() : base("GithubNotificationsService")
        {

        }

        protected override void OnHandleIntent(Intent intent)
        {
            if (!(CrossConnectivity.Current.IsRemoteReachable("www.google.com").Result))
                return;
            try
            {
                var client = new GitHubClient(new ProductHeaderValue("gitit"));

                if (CrossSecureStorage.Current.HasKey("OAuthToken"))
                {
                    _passwordCredential = new Credentials(CrossSecureStorage.Current.GetValue("OAuthToken"));
                    client.Credentials = new Credentials(_passwordCredential.Password);
                    var notificationRequest = new NotificationsRequest
                    {
                        Since =
                            DateTimeOffset.Now.Subtract(new TimeSpan(0,
                                15, 0))
                    };
                    var notifications = client.Activity.Notifications.GetAllForCurrent(notificationRequest).Result;
                    foreach (var notification in notifications)
                    {
                        _title = $"{notification.Subject.Title}";
                        _text =
                            $"in {notification.Repository.FullName} ({Convert.ToDateTime(notification.UpdatedAt).Humanize()})";

                        var builder = new NotificationCompat.Builder(this)
                            .SetSmallIcon(Resource.Drawable.ic_stat_newstorelogo_scale_400)
                            .SetContentTitle(_title)
                            .SetContentText(_text);

                        var notificationManager = (NotificationManager) GetSystemService(Context.NotificationService);
                        notificationManager.Notify(1, builder.Build());
                    }
                }
            }
            catch (HttpRequestException e)
            {
                
            }
        }
    }
}