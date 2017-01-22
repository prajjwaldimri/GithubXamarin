using Octokit;
using System;
using System.Net.NetworkInformation;
using Windows.ApplicationModel.Background;
using Windows.Security.Credentials;
using Windows.Storage;
using Microsoft.QueryStringDotNET;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;

namespace GithubXamarin.UWP.Background
{
    public sealed class GithubNotificationsBackgroundTask : IBackgroundTask
    {
        private BackgroundTaskDeferral _deferral;
        private string _toastTitle;
        private string _toastContent;
        private string _toastLogo;
        private Octokit.Credentials _passwordCredential;

        public void Run(IBackgroundTaskInstance taskInstance)   
        {
            _deferral = taskInstance.GetDeferral();

            if (NetworkInterface.GetIsNetworkAvailable())
            {
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

                //Octokit
                GitHubClient client;
                client = new GitHubClient(new ProductHeaderValue("githubuwp"));
                VaultAccessTokenRetriever();
                if (_passwordCredential != null)
                {
                    client.Credentials = new Credentials(_passwordCredential.Password);
                    var notificationRequest = new NotificationsRequest();
                    notificationRequest.Since = DateTimeOffset.Now.Subtract(new TimeSpan(0, 20, 0));
                    var notifications = client.Activity.Notifications.GetAllForCurrent(notificationRequest).Result;
                    foreach (var notification in notifications)
                    {
                        _toastTitle = $"{notification.Subject.Title}";
                        _toastContent = $"in {notification.Repository.FullName} at {notification.UpdatedAt}";
                        var toast = new ToastNotification(toastContent.GetXml());
                        toast.Tag = "1";
                        toast.Group = "GitItNotifications";
                        ToastNotificationManager.CreateToastNotifier().Show(toast);
                    }
                }
            }

            _deferral.Complete();
        }

        //<summary>
        //Retrieves the OAuth AccessToken from the PasswordVault
        //</summary>
        //<returns>A Credentials object</returns>
        public void VaultAccessTokenRetriever()
        {
            var vault = new PasswordVault();
            if (!ApplicationData.Current.RoamingSettings.Values.ContainsKey("IsLoggedIn"))
                _passwordCredential = null;
            try
            {
                if (vault.FindAllByResource("GithubAccessToken") != null)
                {
                    _passwordCredential = new Credentials(vault.Retrieve("GithubAccessToken", "Github").Password);
                }
            }
            catch (Exception)
            {
                _passwordCredential = null;
            }
        }

    }
}
