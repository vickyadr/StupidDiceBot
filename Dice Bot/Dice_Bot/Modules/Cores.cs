using Dice_Bot.Models;
using Dice_Bot.Modules;
using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Dice_Bot.Modules
{
    public class _
    {
        public static userData udata = new userData();
        public static Cores core = Cores.Instance;
        public static LocalData ldata = new LocalData();
        public static readonly Dictionary<string, SiteData> sdata = new Dictionary<string, SiteData>
        {
            { "999 Dice", new SiteData { Name="999 Dice", loginAs="Username", Currencies = new Dictionary<string, string> { { "Bitcoin","btc" }, { "Dogecoin", "doge" }, { "Litecoin", "ltc" }, { "Ethereum", "eth" }, { "Monero", "xmr" } }, SiteScript = new _999dice(), UseCurrency = true } },
            { "FreeBitcoin", new SiteData { Name="FreeBitcoin", loginAs="Email", Currencies = new Dictionary<string, string> { { "Bitcoin", "btc" } }, SiteScript = new _Freebitcoin(), UseCurrency = false } }
        };
    }

    public class Cores
    {

        private static readonly Cores instance = new Cores();

        public _site SetSite { set { _s = _.udata.siteUsed = value; } }
        private Thread betThread { get; set; }

        private Script lua { get; set; }
        public string programs { get; private set; }

        private bool first = true;
        private bool isRebet = false;
        private int scriptdelay = 0;
        public bool isBetBussy = false;
        //private Lua lua = new Lua();
        //lua.Clear();
        //    LuaGlobalPortable g = lua.CreateEnvironment();

        private _site _s { get; set; }

        public static Cores Instance
        {
            get
            {
                return instance;
            }
        }

        static Cores() { }

        private Cores() { }
       
        public void Start()
        {
            
            if (_.udata.sessionId == null || _s == null)
                return;

            if (Views.Main.mainData.StartBot == "Waiting...")
            {
                //betThread.Abort();
                Droid.MainActivity.Instance.Stop_BetService();
                //Views.Main.mainData.StartBot = "Teach Bot Now !!!";
                _.core.UpdateButton = e_.ButtonStart.Stopped;
                _.core.AddMainLog = "[Learn] Bot take a nap";
                return;
            }

            try
            {
                lua = new Script();
                _.core.AddMainLog = "[Learn] Starting....";
                programs = _.udata.programs;
                Thread.Sleep(100);
                SetLua();
                lua.DoString(string.Format(@"{0}", programs));
                Thread.Sleep(100);
                _.udata.isRun = true;
                scriptdelay = 0;
                isBetBussy = false;
                isRebet = false;
                first = true;
                lua = new Script();

                lua.DoString(@"dxstop = false " +
                    "function stop() " +
                    "dxstop = not(dxstop) " +
                    "end");
                LuaSet("delaynext", 0);
                LuaSet("delaynow", 0);

                Droid.MainActivity.Instance.Start_BetService();
                //betThread = new Thread(new ThreadStart(DoTask));
                //betThread.Start();
                //Task.Run(async () => await DoTask());
            }
            catch (SyntaxErrorException e)//catch (ScriptRuntimeException ex) 
            {
                _.core.AddMainLog = string.Format("[Error] {0}", e.DecoratedMessage);
                _.udata.isRun = false;
                return;
            }

        }

        public void RunBet()
        {
            isBetBussy = true;

            if (!isRebet)
            {
                try
                {
                    SetLua();
                    if (first)
                    {
                        lua.DoString(string.Format(@"{0}", programs));
                        Views.Main.mainData.StartBot = "Take a Nap";
                        first = false;
                    }
                    else
                    {
                        //Thread.Sleep(_.udata.sleepTime);
                        LuaSet("delaynow", LuaNumber("delaynext"));
                        LuaSet("delaynext", 0);
                        lua.DoString("dobet()");
                    }
                    Thread.Sleep(100);
                    _.core.AddMainLog = string.Format("[Bet] Place {0:0.########} {1} chance {2:0.##} at {3}", LuaDecimal("nextbet"), _.udata.currencies, LuaDecimal("chance"), (LuaBool("bethigh")) ? "high" : "low");
                    lua.Options.DebugPrint = s => { _.core.AddMainLog = "[Print] " + s; };
                    scriptdelay = LuaInt("delaynow");

                    if (scriptdelay > 0)
                    {
                        Thread.Sleep(scriptdelay);
                        LuaSet("delaynow", 0);
                        _.core.AddMainLog = string.Format("[Bet] Delay {0}ms", scriptdelay);
                    }


                    Thread.Sleep(50);
                }
                catch (ScriptRuntimeException e)
                {
                    _.core.AddMainLog = string.Format("[Error] {0}", e.Message);
                    _.udata.isRun = false;
                    isBetBussy = false;
                }

            }
            else
            {

                Thread.Sleep(50);
                isRebet = false;
            }

            try
            {

                if (LuaBool("dxstop") || !_.udata.isRun)
                {
                    Droid.MainActivity.Instance.Stop_BetService();

                    if (!_.udata.stopnow && _.udata.isRun)
                    {
                        //Views.Main.mainData.StartBot = "Teach Bot Now !!!";
                        _.core.UpdateButton = e_.ButtonStart.Stopped;
                        _.core.AddMainLog = "[Learn] Bot take a nap";
                    }
                    _.udata.isRun = false;
                }

                if (_.udata.isRun)
                {
                    if (_s.PlaceBet(LuaBool("bethigh"), LuaDecimal("nextbet"), LuaDecimal("chance")).Result)
                    {
                        _.core.UpdateMain();
                        Thread.Sleep(50);
                        _.udata.previousbet = LuaDecimal("nextbet");
                        _.udata.betHigh = LuaBool("bethigh");
                        _.udata.chance = LuaDecimal("chance");
                        SetLua();
                    }
                    else
                    {

                        string s = string.Empty;
                        if (_.udata.Output("placebet", out s))
                        {
                            if (s == "send_rebet")
                                isRebet = true;
                            else if (s != null)
                            {
                                _.core.AddMainLog = string.Format("[Error] {0}", s);
                                lua.DoString("stop()");
                            }
                        }

                    }

                }
                    
                isBetBussy = false;

                if (LuaBool("dxstop") || !_.udata.isRun)
                {
                    Droid.MainActivity.Instance.Stop_BetService();

                    if (!_.udata.stopnow && _.udata.isRun)
                    {
                        //Views.Main.mainData.StartBot = "Teach Bot Now !!!";
                        _.core.UpdateButton = e_.ButtonStart.Stopped;
                        _.core.AddMainLog = "[Learn] Bot take a nap";
                    }
                    _.udata.isRun = false;
                }

            }
            catch (ScriptRuntimeException e)
            {
                _.core.AddMainLog = string.Format("[Error] {0}", e.Message);
                _.udata.isRun = false;
                isBetBussy = false;
                Droid.MainActivity.Instance.Stop_BetService();
                //Views.Main.mainData.StartBot = "Teach Bot Now !!!";
                _.core.UpdateButton = e_.ButtonStart.Stopped;
                _.core.AddMainLog = "[Learn] Bot take a nap";
            }
        }

        public void Stop()
        {
            if (_s == null || Views.Main.mainData.StartBot == "Waiting...")
                return;

            if (_.udata.stopnow)
            {
                //betThread.Abort();
                _.udata.isRun = false;
                Droid.MainActivity.Instance.Stop_BetService();
            }
            else
            {
                _.core.AddMainLog = "[Learn] Stop on bet complete";
                //Views.Main.mainData.StartBot = "Waiting...";
                _.core.UpdateButton = e_.ButtonStart.Waiting;
                lua.DoString("stop()");
                return;
            }

            //Views.Main.mainData.StartBot = "Teach Bot Now !!!";
            _.core.UpdateButton = e_.ButtonStart.Stopped;
            _.core.AddMainLog = "[Learn] Bot take a nap";
        }

        public async Task Login(string user, string pass, string twofa) {

            if (_s == null)
                return;
            await _s.Login(user, pass, twofa);
        }

        void SetLua()
        {
            LuaSet("balance", _.udata.balance);
            LuaSet("profit", _.udata.profit);
            LuaSet("currentstreak", (_.udata.streakType) ? _.udata.streakCurrent : -(_.udata.streakCurrent));
            LuaSet("previousbet", _.udata.previousbet);
            LuaSet("nextbet", _.udata.previousbet);
            LuaSet("chance", _.udata.chance);
            LuaSet("bethigh", _.udata.betHigh);
            LuaSet("bets", (_.udata.wins + _.udata.losses));
            LuaSet("wins", _.udata.wins);
            LuaSet("losses", _.udata.losses);
            LuaSet("currency", _.udata.currencies);
            LuaSet("wagered", _.udata.wagered);
            LuaSet("win", _.udata.win);
            //context["currencies"] = _.udata.currencies;
            //context["enablersc"] = EnableReset;
            //context["enablezz"] = EnableProgZigZag;
            //context["site"] = _.sdata.name;
        }

        void LuaSet(string key, string val)
        {
            if (val != null)
                lua.DoString(string.Format("{0} = \"{1}\"", key, val));
        }

        void LuaSet(string key, int val)
        {
            lua.DoString(string.Format("{0} = {1}", key, val));
        }

        void LuaSet(string key, long val)
        {
            lua.DoString(string.Format("{0} = {1}", key, val));
        }

        void LuaSet(string key, decimal val)
        {
            lua.DoString(string.Format("{0} = {1}", key, val));
        }

        void LuaSet(string key, double val)
        {
            lua.DoString(string.Format("{0} = {1}", key, val));
        }

        void LuaSet(string key, bool val)
        {
            lua.DoString(string.Format("{0} = {1}", key, ((val) ? "true":"false")));
        }

        double LuaNumber(string v)
        {
            return lua.DoString("return "+v).Number;
        }

        int LuaInt(string v)
        {
            try { return (int)lua.DoString("return " + v).Number; }
            catch (Exception e) { Console.Write(e); return 0; }
        }

        decimal LuaDecimal(string v)
        {
            decimal result = 0;
            decimal.TryParse(string.Format("{0:0.########}", lua.DoString("return " + v).Number), out result);
            return result;
        }

        string LuaString(string v)
        {
            return lua.DoString("return " + v).String;
        }

        bool LuaBool(string v)
        {
            return lua.DoString("return " + v).Boolean;
        }

        public string AddMainLog { set { MessagingCenter.Send(this, "WiteTask", new mainLog { Id = Guid.NewGuid().ToString(), Message = value, Time = DateTime.Now }); } }

        public void UpdateMain()
        {
            MessagingCenter.Send(this, "UpdateMain");
        }

        public e_.ButtonStart UpdateButton { set {   MessagingCenter.Send(this, "UpdateButton", value); } }

        public void AddLogs(LogsItem item)
        {
            MessagingCenter.Send(this, "AddLogs", item);
        }

        public void SetSetting(setting item)
        {
            MessagingCenter.Send(this, "UpdateSetting", item);
            if(item.Name == "Bet sleep")
            {
                int i = 0;
                if (int.TryParse(item.Val.ToString(), out i))
                    _.udata.sleepTime = i;
            }else if(item.Name == "Stop mode")
                _.udata.stopnow = (item.Val.ToString() == "force");
        }
    }
}
