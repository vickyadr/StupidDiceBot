using Android.App;
using Android;
using Android.Content;

namespace Dice_Bot.Modules
{
    public class Notif : Activity
    {
        Notification.Builder builder { get; set; }

        public Notif()
        {
            builder = new Notification.Builder(this).SetContentTitle("DiceBot").SetContentText("Profit = 0.0001").SetSmallIcon(Resource.Drawable.ButtonPlus);
            Notification notif = builder.Build();
            NotificationManager notifManager = GetSystemService(Context.NotificationService) as NotificationManager;
            const int notifid = 0;
            notifManager.Notify(notifid, notif);
        }
    }
}
