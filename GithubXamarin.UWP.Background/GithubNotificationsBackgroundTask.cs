using Octokit;
using System;
using Windows.ApplicationModel.Background;
using Windows.Security.Credentials;
using Windows.Storage;
using Microsoft.QueryStringDotNET;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;
using System.Threading.Tasks;
using Humanizer;
using Plugin.SecureStorage;

namespace GithubXamarin.UWP.Background
{
    public sealed class GithubNotificationsBackgroundTask : IBackgroundTask
    {
        private BackgroundTaskDeferral _deferral;
        private string _toastTitle;
        private string _toastContent;
        private string _toastLogo;
        private Credentials _passwordCredential;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();

            var localSettingsValues = ApplicationData.Current.LocalSettings.Values;
            WinSecureStorageBase.StoragePassword = "12345";

            //Octokit
            var client = new GitHubClient(new ProductHeaderValue("gitit"));
            if (CrossSecureStorage.Current.HasKey("OAuthToken"))
            {
                _passwordCredential = new Credentials(CrossSecureStorage.Current.GetValue("OAuthToken"));
                client.Credentials = new Credentials(_passwordCredential.Password);
                var notificationRequest = new NotificationsRequest
                {
                    Since =
                        DateTimeOffset.Now.Subtract(new TimeSpan(0,
                            int.Parse(localSettingsValues["BackgroundTaskTime"].ToString()), 0))
                };
                var notifications = await client.Activity.Notifications.GetAllForCurrent(notificationRequest);
                foreach (var notification in notifications)
                {
                    _toastTitle = $"{notification.Subject.Title}";
                    _toastContent = $"in {notification.Repository.FullName} ({Convert.ToDateTime(notification.UpdatedAt).Humanize()})";
                    #region Toast-Notification Payload
                    //Reference: https://blogs.msdn.microsoft.com/tiles_and_toasts/2015/07/08/quickstart-sending-a-local-toast-notification-and-handling-activations-from-it-windows-10/

                    //Body of toast
                    var toastVisual = new ToastVisual()
                    {
                        BindingGeneric = new ToastBindingGeneric()
                        {
                            Children =
                                {
                                    new AdaptiveText() {Text = _toastTitle},
                                    new AdaptiveText() {Text = _toastContent},
                                }
                        }
                    };

                    //Interactive buttons to Toast
                    var toastActions = new ToastActionsCustom()
                    {
                        Buttons =
                            {
                                new ToastButton("Mark As Read",new QueryString()
                                {
                                    {"action", "markAsRead" }
                                }.ToString())
                                {
                                    ActivationType = ToastActivationType.Background
                                }
                            }
                    };

                    var toastContent = new ToastContent()
                    {
                        Visual = toastVisual,
                        Actions = toastActions
                    };

                    #endregion

                    var toast = new ToastNotification(toastContent.GetXml()) {Tag = "1"};
                    ToastNotificationManager.CreateToastNotifier().Show(toast);
                }
            }
            _deferral.Complete();
        }
    }
}
