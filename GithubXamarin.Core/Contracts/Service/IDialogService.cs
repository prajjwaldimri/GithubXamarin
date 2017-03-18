using System.Threading.Tasks;

namespace GithubXamarin.Core.Contracts.Service
{
    public interface IDialogService
    {
        Task ShowDialogASync(string message, string title);
    }
}
