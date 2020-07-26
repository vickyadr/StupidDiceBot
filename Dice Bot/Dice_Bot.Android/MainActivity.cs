using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace Dice_Bot.Droid
{
    [Activity(Label = "Stupid Dice Bot", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        static MainActivity instance {get; set;}

        public static MainActivity Instance { get { return instance; } }

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            instance = this;
            LoadApplication(new App());
        }

        public void Start_BetService()
        {
            StartService(new Intent(this, typeof(Bot_Services)));
        }

        public void Stop_BetService()
        {
            StopService(new Intent(this, typeof(Bot_Services)));
        }

    }
}