using Android.App;
using Android.Widget;
using Android.OS;
using NLua;
using Newtonsoft.Json;

using System.Threading;
using System.IO;
using System.Net.Http;
using System.Net;
using System;
using System.Collections.Generic;


namespace DiceBot
{
    [Activity(Label = "Main", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/Theme.Custom")]
    public class MainActivity : Activity
    {
        Lua context = new Lua();
        Button btLogin, startBet, stopBet;
        TextView tx1;
        EditText etProgram;
        bool onDoBet = false;

        HttpClientHandler ClientHandlr;
        HttpClient Client;
        string sessionCookie = string.Empty;
        Random r = new Random();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource

            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
            ActionBar.SetDisplayShowTitleEnabled(false);
            SetContentView(Resource.Layout.Main);
            context.LoadCLRPackage();
            context["instance"] = this;

            btLogin = FindViewById<Button>(Resource.Id.btnLogin);
            startBet = FindViewById<Button>(Resource.Id.btnStart);
            stopBet = FindViewById<Button>(Resource.Id.btnStop);

            tx1 = FindViewById<TextView>(Resource.Id.textView1);

            etProgram = FindViewById<EditText>(Resource.Id.editText1);

            btLogin.Click += async (sender, e) =>
            {
                ThreadPool.QueueUserWorkItem(o => Login("tx113", "Asdfghjkl"));
            };

            startBet.Click += async (sender, e) =>
            {
                if (onDoBet)
                    return;

                _.tdata.programs = etProgram.Text;
                //ThreadPool.QueueUserWorkItem(o => DoTask());
            };

            stopBet.Click += async (sender, e) =>
            {
                //context.DoString("stopnow = true;");                
            };

            ActionBar.Tab tab = ActionBar.NewTab();
            //tab.SetText(Resources.GetString(Resource.String.tab1_text));
            tab.SetText("Home");
            //tab.SetIcon(Resource.Drawable.tab1_icon);
            tab.TabSelected += (sender, args) =>
            {
                // Do something when tab is selected
                //SetContentView(Resource.Layout.Main);
            };

            ActionBar.AddTab(tab);

            tab = ActionBar.NewTab();
            //tab.SetText(Resources.GetString(Resource.String.tab2_text));
            tab.SetText("Code");
            //tab.SetIcon(Resource.Drawable.tab2_icon);
            tab.TabSelected += (sender, args) => {
                // Do something when tab is selected
                //SetContentView(Resource.Layout.Login);
            };

            ActionBar.AddTab(tab);
        }

        async void DoTask()
        {
            context.Close();
            context["balance"] = _.udata.balance;
            context["win"] = false;
            context["profit"] = _.udata.profit;
            context["currentprofit"] = _.tdata.currentProfit;
            context["currentstreak"] = _.tdata.currentStreak;
            context["previousbet"] = _.tdata.previousbet;
            context["bets"] = _.udata.wins + _.udata.losses;
            context["wins"] = _.udata.wins;
            context["losses"] = _.udata.losses;
            context["nextbet"] = _.tdata.previousbet;
            context["chance"] = _.tdata.chance;
            context["bethigh"] = _.tdata.betHigh;
            //context["lastbet"] = _.tdata.lastBet;
            context["currencies"] = _.udata.currencies;
            context["enablezz"] = _.tdata.enablezz;
            //context["wagered"] = 
            context["site"] = _.udata.site;

            //context.DoString(_.tdata.programs);

            etProgram.Text = "hhh\r\n"+_.tdata.programs;

            while (onDoBet)
            {
                Thread.Sleep(2000);
                try
                {
                    context.DoString("dobet()");
                }
                catch (Exception)
                {
                    onDoBet = false;
                    break;
                }
                if ((bool)context["done"] || (bool)context["stopnow"])
                    onDoBet = false;

                PlaceBet((bool)context["bethigh"], (decimal)context["nextbet"], (decimal)context["chance"]);
            }
        }

        async void Login(string Username, string Password, string twofa = "")
        {
            RunOnUiThread(() => tx1.Text = "load");
            ClientHandlr = new HttpClientHandler { UseCookies = true, AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip }; ;
            Client = new HttpClient(ClientHandlr) { BaseAddress = new Uri("https://www.999doge.com/api/web.aspx") };
            Client.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("gzip"));
            Client.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("deflate"));

            List<KeyValuePair<string, string>> pairs = new List<KeyValuePair<string, string>>();
            pairs.Add(new KeyValuePair<string, string>("a", "Login"));
            pairs.Add(new KeyValuePair<string, string>("key", "fe5d6b6be76d40759cdf3b22d10f5ffb"));
            if (twofa != "" && twofa != null) pairs.Add(new KeyValuePair<string, string>("Totp", twofa));
            pairs.Add(new KeyValuePair<string, string>("Username", Username));
            pairs.Add(new KeyValuePair<string, string>("Password", Password));
            
            FormUrlEncodedContent Content = new FormUrlEncodedContent(pairs);
            Login_999dice result = new Login_999dice();
            try
            {
                using (var response = await Client.PostAsync("", Content))
                {
                    var json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<Login_999dice>(json);
                }
            }
            catch (AggregateException e)
            {
                return;
            }

            string shows="";
            shows += "Cookies: "+ result.SessionCookie + "\r\n";
            shows += "Balance: " + (result.Balance / 100000000.0m).ToString() + "\r\n";
            shows += "Raw Balance: " + (result.Balance).ToString() + "\r\n";
            shows += "Profit: " + (result.Profit / 100000000.0m).ToString() + "\r\n";
            shows += "Bet: " + result.BetCount + "\r\n";
            shows += "Win: " + result.BetWinCount + "\r\n";
            shows += "Lose: " + result.BetLoseCount + "\r\n";
            RunOnUiThread(() => tx1.Text = shows);
            sessionCookie = result.SessionCookie;
        }

        string nextH;

        async void PlaceBet(bool High, decimal amount, decimal chance, string guid="")
        {
            List<KeyValuePair<string, string>> pairs = new List<KeyValuePair<string, string>>();
            FormUrlEncodedContent Content = new FormUrlEncodedContent(pairs);

            //if (nextH == "" && nextH != null)
            //{
            //    pairs = new List<KeyValuePair<string, string>>();
            //    pairs.Add(new KeyValuePair<string, string>("a", "GetServerSeedHash"));
            //    pairs.Add(new KeyValuePair<string, string>("s", sessionCookie));

            //    Content = new FormUrlEncodedContent(pairs);
            //    using (var response = await Client.PostAsync("", Content))
            //    {
            //        try
            //        {
            //            var json = await response.Content.ReadAsStringAsync();
            //            nextH = JsonConvert.DeserializeObject<Hash_999dice>(json).Hash;
            //        }
            //        catch (AggregateException e)
            //        {
            //            if (e.InnerException.Message.Contains("ssl"))
            //            {
            //                //PlaceBet(true);
            //                return;
            //            }
            //        }
            //    }
            //}

            string ClientSeed = r.Next(0, int.MaxValue).ToString();
            pairs = new List<KeyValuePair<string, string>>();
            pairs.Add(new KeyValuePair<string, string>("a", "PlaceBet"));
            pairs.Add(new KeyValuePair<string, string>("s", sessionCookie));
            pairs.Add(new KeyValuePair<string, string>("PayIn", ((long)((decimal)amount * 100000000m)).ToString("0", System.Globalization.NumberFormatInfo.InvariantInfo)));
            pairs.Add(new KeyValuePair<string, string>("Low", (High ? 999999 - (int)chance : 0).ToString(System.Globalization.NumberFormatInfo.InvariantInfo)));
            pairs.Add(new KeyValuePair<string, string>("High", (High ? 999999 : (int)chance).ToString(System.Globalization.NumberFormatInfo.InvariantInfo)));
            pairs.Add(new KeyValuePair<string, string>("ClientSeed", ClientSeed));
            pairs.Add(new KeyValuePair<string, string>("Currency", "doge"));//Currency));
            pairs.Add(new KeyValuePair<string, string>("ProtocolVersion", "2"));

            Content = new FormUrlEncodedContent(pairs);
            //string tmps = Content.ReadAsStringAsync().Result;
            Bet_999dice betResult = new Bet_999dice();
            string res = "";
            using (var response = await Client.PostAsync("", Content))
            {

                try
                {
                    var json = await response.Content.ReadAsStringAsync();
                    res = json;
                    betResult = JsonConvert.DeserializeObject<Bet_999dice>(json);
                }
                catch (AggregateException e)
                {
                    //Parent.DumpLog(e.InnerException.Message, 0);
                    //if (retrycount++ < 3)
                    //{
                    //    PlaceBetThread(High);
                    //    return;
                    //}
                    //if (e.InnerException.Message.Contains("ssl"))
                    //{
                    //    PlaceBetThread(High);
                    //    return;
                    //}
                    //else
                    //{
                    //    Parent.updateStatus("An error has occurred");
                    //}
                }
            }

            string shows = res;
            //shows += "ID: " + betResult.BetId + "\r\n";
            //shows += "Payout: " + (betResult.PayOut / 100000000.0m).ToString() + "\r\n";
            RunOnUiThread(() => tx1.Text = shows);
        }

    }
}

