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
