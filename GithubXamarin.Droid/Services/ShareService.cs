using System;
using System.Threading.Tasks;
using GithubXamarin.Core.Contracts.Service;

namespace GithubXamarin.Droid.Services
{
    public class ShareService : IShareService
    {
        public async Task ShareTextAsync(string text, string title)
        {
            throw new NotImplementedException();
        }

        public async Task ShareLinkAsync(Uri link, string title)
        {
            throw new NotImplementedException();
        }
    }
}