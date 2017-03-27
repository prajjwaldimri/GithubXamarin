using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Microsoft.QueryStringDotNET;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;
using Humanizer;
using Plugin.SecureStorage;
using Notification = Octokit.Notification;

namespace GithubXamarin.UWP.Background
{
    public sealed class GithubNotificationsBackgroundTask : IBackgroundTask
    {
        private BackgroundTaskDeferral _deferral;
        private string _toastTitle;
        private string _toastContent;
        private string _toastLogo;
        private const string _tokenKey = "OAuthToken";
        private const string _lastShowedNotificationKey = "LastShowedNotificationUpdationTime";

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();

            var localSettingsValues = ApplicationData.Current.LocalSettings.Values;
            WinSecureStorageBase.StoragePassword = "12345";

            //Octokit
            var client = new GitHubClient(new ProductHeaderValue("gitit"));
            if (CrossSecureStorage.Current.HasKey(_tokenKey))
            {
                client.Credentials = new Credentials(CrossSecureStorage.Current.GetValue(_tokenKey));
                var notificationRequest = new NotificationsRequest
                {
                    Since =
                        DateTimeOffset.Now.Subtract(new TimeSpan(1, 0, 0, 0))
                };

                var serverNotifications = await client.Activity.Notifications.GetAllForCurrent(notificationRequest);
                if (serverNotifications.Count <= 0) return;

                var latestUpdatedAt = DateTime.Parse(serverNotifications[0].UpdatedAt);
                IEnumerable<Octokit.Notification> notifications = new List<Notification>(0);


                if (localSettingsValues.ContainsKey(_lastShowedNotificationKey))
                {
                    var localUpdatedAt = DateTime.Parse(localSettingsValues[_lastShowedNotificationKey].ToString());
                    notifications = from notification in serverNotifications
                                    where latestUpdatedAt > localUpdatedAt
                                    select notification;
                }
                localSettingsValues[_lastShowedNotificationKey] = latestUpdatedAt.ToString();
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
                                    {"action", "markAsRead" },
                                    {"notificationId", notification.Id }
                                }.ToString())
                                {
                                    ActivationType = ToastActivationType.Background
                                }
                            }
                    };

                    var toastContent = new ToastContent()
                    {
                        Visual = toastVisual,
                        Actions = toastActions,

                        Launch = new QueryString()
                        {
                            {"notificationId", notification.Id }
                        }.ToString()
                    };

                    #endregion

                    #region Tile Payload

                    // https://blogs.msdn.microsoft.com/tiles_and_toasts/2015/06/30/adaptive-tile-templates-schema-and-documentation/

                    var tileContent = new TileContent()
                    {
                        Visual = new TileVisual()
                        {
                            Branding = TileBranding.NameAndLogo,
                            TileMedium = new TileBinding()
                            {
                                Content = new TileBindingContentAdaptive()
                                {
                                    Children =
                                    {
                                        new AdaptiveGroup()
                                        {
                                            Children =
                                            {
                                                new AdaptiveSubgroup()
                                                {
                                                    Children =
                                                    {
                                                        new AdaptiveText()
                                                        {
                                                            Text = _toastTitle,
                                                            HintWrap = true,
                                                            HintStyle = AdaptiveTextStyle.Body
                                                        },
                                                        new AdaptiveText()
                                                        {
                                                            Text = _toastContent,
                                                            HintWrap = true,
                                                            HintStyle = AdaptiveTextStyle.CaptionSubtle
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            },
                            TileWide = new TileBinding()
                            {
                                Content = new TileBindingContentAdaptive()
                                {
                                    Children =
                                    {
                                        CreateGroup(_toastTitle, _toastContent)
                                    }
                                }
                            },
                            TileLarge = new TileBinding()
                            {
                                Content = new TileBindingContentAdaptive()
                                {
                                    Children =
                                    {
                                        CreateGroup(_toastTitle, _toastContent)
                                    }
                                }
                            }
                        }
                    };

                    #endregion

                    var toast = new ToastNotification(toastContent.GetXml()) { Tag = "1" };
                    ToastNotificationManager.CreateToastNotifier().Show(toast);

                    // Update tile
                    var tileNotification = new TileNotification(tileContent.GetXml());
                    TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
                }
            }
            _deferral.Complete();
        }

        private static AdaptiveGroup CreateGroup(string title, string body)
        {
            return new AdaptiveGroup()
            {
                Children =
                {
                    new AdaptiveSubgroup()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = title,
                                HintWrap = true,
                                HintStyle = AdaptiveTextStyle.Subtitle
                            },
                            new AdaptiveText()
                            {
                                Text = body,
                                HintWrap = true,
                                HintStyle = AdaptiveTextStyle.CaptionSubtle
                            }
                        }
                    }
                }
            };
        }
    }
}
