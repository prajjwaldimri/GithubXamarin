using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubXamarin.Core.Contracts.Service
{
    public interface IDialogService
    {
        Task ShowSimpleDialogAsync(string message, string title);
        Task<bool> ShowBooleanDialogAsync(string message, string title);
    }
}
