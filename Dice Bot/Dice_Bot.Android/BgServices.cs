using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;

namespace Dice_Bot.Modules
{
    [Service]
    public class BgServices : Service
    {
        static readonly string TAG = "X:" + typeof(BgServices).Name;
        static readonly int TimerWait = 4000;
        Timer timer;
        bool isStarted = false;
        DateTime startTime;

        public override void OnCreate()
        {
            base.OnCreate();
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            Log.Debug(TAG, $"OnStartCommand called at {startTime}, flags={flags}, startid={startId}");

            if (isStarted)
            {
                TimeSpan runtime = DateTime.UtcNow.Subtract(startTime);
                Log.Debug(TAG, $"This service was already started, it's been running for {runtime:c}");
            }
            else
            {
                startTime = DateTime.UtcNow;
                Log.Debug(TAG, $"Starting the service {startTime}");
                timer = new Timer(HandleTimerCallback, startTime, 0, TimerWait);
                isStarted = true;
            }
            return base.OnStartCommand(intent, flags, startId);
        }

        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }

        public override void OnDestroy()
        {
            timer.Dispose();
            timer = null;
            isStarted = false;

            TimeSpan runtime = DateTime.UtcNow.Subtract(startTime);
            Log.Debug(TAG, $"Simple services destroyed at {DateTime.UtcNow} after runnung for {runtime:c}");
            base.OnDestroy();
        }

        void HandleTimerCallback(object state)
        {
            TimeSpan runtime = DateTime.UtcNow.Subtract(startTime);
            Log.Debug(TAG, $"This service has been running for {runtime:c} since ${state}");
            _.core.AddMainLog = $"This service has been running for {runtime:c} since ${state}";

        }

    }

    public class ServiceActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var intent = new Intent(ApplicationContext, typeof(BgServices));
            var source = PendingIntent.GetBroadcast(ApplicationContext, 0, intent, 0);

        }
    }
}
