using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Dice_Bot.Modules;
using Dice_Bot;
using System.Threading;
using MoonSharp.Interpreter;

namespace Dice_Bot.Droid
{
    [Service]
    public class Bot_Services : Service
    {
        private  Timer _timer;
        public string JsonData = string.Empty;
        _site site { get; set; }
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {

            //var t = new Java.Lang.Thread(() => { Java.Lang.Thread.Sleep(2000); Modules._.core.AddMainLog = "test"; });
            //t.Start();
            Doing();
            return StartCommandResult.Sticky;

        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            _timer.Dispose();
            JsonData = string.Empty;
        }

        public void Doing()
        {
            _timer = new Timer((o)=> { if (!_.core.isBetBussy) System.Threading.Tasks.Task.Run(() => _.core.RunBet()); }, null, 0, 500);
        }
    }
}