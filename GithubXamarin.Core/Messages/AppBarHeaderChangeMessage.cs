using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Plugins.Messenger;

namespace GithubXamarin.Core.Messages
{
    public class AppBarHeaderChangeMessage : MvxMessage
    {
        public AppBarHeaderChangeMessage(object sender) : base(sender)
        {
        }

        public string HeaderTitle { get; set; }
    }
}
