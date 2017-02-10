using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubXamarin.Core.Contracts.Service
{
    public interface IDialogService
    {
        Task ShowDialogASync(string message, string title);
    }
}
