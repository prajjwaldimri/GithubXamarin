using System;
using System.Threading.Tasks;

namespace GithubXamarin.Core.Contracts.Service
{
    public interface IShareService
    {
        Task ShareTextAsync(string text, string title);
        Task ShareLinkAsync(Uri link, string title);
    }
}
