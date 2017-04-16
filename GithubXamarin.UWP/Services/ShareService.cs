using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using GithubXamarin.Core.Contracts.Service;

namespace GithubXamarin.UWP.Services
{
    public class ShareService : IShareService
    {
        private string PlainText;
        private string Title;
        private Uri Link;


        public ShareService()
        {
            var dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += DataTransferManager_DataRequested;
        }
        public Task ShareTextAsync(string text, string title)
        {
            PlainText = text;
            Title = title;
            DataTransferManager.ShowShareUI();
            return Task.CompletedTask;
        }

        public Task ShareLinkAsync(Uri link, string title)
        {
            Link = link;
            Title = title;
            DataTransferManager.ShowShareUI();
            return Task.CompletedTask;
        }

        private void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            if (!(string.IsNullOrWhiteSpace(PlainText)))
            {
                args.Request.Data.SetText(PlainText);
            }
            else if (!(string.IsNullOrWhiteSpace(Link.ToString())))
            {
                args.Request.Data.SetWebLink(Link);
            }

            args.Request.Data.Properties.Title = Title;
        }
    }
}
