using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Preferences;
using Android.Support.V4.App;
using Humanizer;
using Octokit;
using Plugin.Connectivity;
using Plugin.SecureStorage;
using Credentials = Octokit.Credentials;
using Notification = Octokit.Notification;

namespace GithubXamarin.Droid.Services
{
    [Service]
    public class GithubNotificationsService : IntentService
    {
        private string _title;
        private string _text;
        private string _toastLogo;
        private Credentials _passwordCredential;
        private const string _lastShowedNotificationKey = "LastShowedNotificationUpdationTime";

        public GithubNotificationsService() : base("GithubNotificationsService")
        {

        }

        protected override void OnHandleIntent(Intent intent)
        {
            if (!CrossConnectivity.Current.IsConnected)
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
                        DateTimeOffset.Now.Subtract(new TimeSpan(1, 0, 0, 0))
                    };

                    var serverNotifications = client.Activity.Notifications.GetAllForCurrent(notificationRequest).Result;
                    if (serverNotifications.Count <= 0) return;

                    var latestUpdatedAt = DateTime.Parse(serverNotifications[0].UpdatedAt);
                    IEnumerable<Octokit.Notification> notifications = new List<Notification>(0);

                    var prefs = PreferenceManager.GetDefaultSharedPreferences(ApplicationContext);
                    var prefsEditor = prefs.Edit();
                    if (prefs.Contains(_lastShowedNotificationKey))
                    {
                        var localUpdatedAt = DateTime.Parse(prefs.GetString(_lastShowedNotificationKey, "null"));
                        notifications = from notification in serverNotifications
                                        where latestUpdatedAt > localUpdatedAt
                                        select notification;
                    }
                    prefsEditor.PutString(_lastShowedNotificationKey, latestUpdatedAt.ToString());
                    prefsEditor.Apply();
                    foreach (var notification in notifications)
                    {
                        _title = $"{notification.Subject.Title}";
                        _text =
                            $"in {notification.Repository.FullName} ({Convert.ToDateTime(notification.UpdatedAt).Humanize()})";

                        var builder = new NotificationCompat.Builder(this)
                            .SetSmallIcon(Resource.Drawable.ic_stat_newstorelogo_scale_400)
                            .SetContentTitle(_title)
                            .SetContentText(_text);

                        var notificationManager = (NotificationManager)GetSystemService(Context.NotificationService);
                        notificationManager.Notify(1, builder.Build());
                    }
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}