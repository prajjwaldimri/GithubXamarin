using Android.Content;

namespace GithubXamarin.Droid.Services
{
    [BroadcastReceiver(Enabled = true, Exported = false, Process = ":remote")]
    public class AlarmBroadcastReciever : BroadcastReceiver
    {
        public static int RequestCode = 18721;
        public override void OnReceive(Context context, Intent intent)
        {
            // https://github.com/codepath/android_guides/wiki/Starting-Background-Services#using-with-alarmmanager-for-periodic-tasks
            var i = new Intent(context, typeof(GithubNotificationsService));
            context.StartService(i);
        }
    }
}