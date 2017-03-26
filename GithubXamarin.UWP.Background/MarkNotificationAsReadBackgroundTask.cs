using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;
using Octokit;
using Plugin.SecureStorage;

namespace GithubXamarin.UWP.Background
{
    public sealed class MarkNotificationAsReadBackgroundTask : IBackgroundTask
    {
        private BackgroundTaskDeferral _deferral;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();

            var details = taskInstance.TriggerDetails as ToastNotificationActionTriggerDetail;

            if (details != null)
            {
                var arguments = details.Argument.Split('=');
                var notificationId = int.Parse(arguments[2]);
                WinSecureStorageBase.StoragePassword = "12345";

                //Octokit
                var client = new GitHubClient(new ProductHeaderValue("gitit"));
                if (CrossSecureStorage.Current.HasKey("OAuthToken"))
                {
                    client.Credentials = new Credentials(CrossSecureStorage.Current.GetValue("OAuthToken"));

                    var notificationsClient = new NotificationsClient(new ApiConnection(client.Connection));
                    await notificationsClient.MarkAsRead(notificationId);
                }

            }

            _deferral.Complete();
        }
    }
}
