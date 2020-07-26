using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Dice_Bot.Modules
{
    class _Freebitcoin : _site
    {
        private int retrycount;
        private CookieContainer Cookies = new CookieContainer();
        private string csrf = "";
        private string accesstoken = "";
        private bool undone = false;
        public _Freebitcoin()
        { }

        public override async Task Login(string user, string pass, string twofa)
        {
            if (undone)
            {
                _.udata.Output("login", new Status { isError = true, message = "Sorry, Undercontruction this site unstable may fix on next update" });
                return;
            }

            try
            {
                Cookies = new CookieContainer();
                ClientHandlr = new HttpClientHandler { UseCookies = true, AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip, CookieContainer = Cookies }; ;
                Client = new HttpClient(ClientHandlr) { BaseAddress = new Uri("https://freebitco.in/") };
                Client.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("gzip"));
                Client.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("deflate"));

                using (var resp = await Client.GetAsync(""))
                {
                    string s1 = "";
                    if (resp.IsSuccessStatusCode)
                        s1 = string.Empty;//s1 = await resp.Content.ReadAsStringAsync();
                    else if (resp.StatusCode == HttpStatusCode.ServiceUnavailable)
                    {
                        s1 = await resp.Content.ReadAsStringAsync();
                        //Task.Factory.StartNew(() =>
                        //{
                        //Android.Widget.Toast.MakeText("freebitcoin has their cloudflare protection on HIGH\n\nThis will cause a slight delay in logging in. Please allow up to a minute.");
                        //});

                        if (!Cloudflare.doCFThing(s1, Client, ClientHandlr, 0, "freebitco.in"))
                        {

                            //finishedlogin(false);
                            _.udata.Output("login", new Status { isError = true, message = "Please try again later." });
                            return;
                        }
                    }
                }

                foreach (Cookie x in Cookies.GetCookies(new Uri("https://freebitco.in")))
                {
                    if (x.Name == "csrf_token")
                    {
                        csrf = x.Value;
                    }
                }

                List<KeyValuePair<string, string>> pairs = new List<KeyValuePair<string, string>>();
                pairs.Add(new KeyValuePair<string, string>("csrf_token", csrf));
                pairs.Add(new KeyValuePair<string, string>("op", "login_new"));
                pairs.Add(new KeyValuePair<string, string>("btc_address", user));
                pairs.Add(new KeyValuePair<string, string>("password", pass));
                pairs.Add(new KeyValuePair<string, string>("tfa_code", twofa));

                FormUrlEncodedContent Content = new FormUrlEncodedContent(pairs);
                Login_Freebitcoin result = new Login_Freebitcoin();

                try
                {
                    string s = string.Empty;
                    using (var cflare = await Client.PostAsync("", Content))
                    {

                        if (!cflare.IsSuccessStatusCode)
                        {
                            _.udata.Output("login", new Status { isError = true, message = "Unexcepted error." });
                            return;
                        }

                        s = await cflare.Content.ReadAsStringAsync();
                    }

                    string[] messages = s.Split(':');
                    if (messages.Length > 2)
                    {
                        accesstoken = messages[2];
                        Cookies.Add(new Cookie("btc_address", messages[1], "/", "freebitco.in"));
                        Cookies.Add(new Cookie("password", accesstoken, "/", "freebitco.in"));
                        Cookies.Add(new Cookie("have_account", "1", "/", "freebitco.in"));
                        using (var resp = await Client.GetAsync("https://freebitco.in/cgi-bin/api.pl?op=get_user_stats"))
                        {
                            if (resp.IsSuccessStatusCode)
                                result = JsonConvert.DeserializeObject<Login_Freebitcoin>(await resp.Content.ReadAsStringAsync());
                            else
                            {
                                _.udata.Output("login", new Status { isError = true, message = "Login failed, please try again later." });
                                return;
                            }
                        }
                    }
                    else
                    {
                        _.udata.Output("login", new Status { isError = true, message = "Wrong email or password." });
                        return;
                    }
                }
                catch (AggregateException e)
                {
                    Console.Write(e.Message);
                    _.udata.Output("login", new Status { isError = true, message = "An error has occurred." });
                    return;
                }

                if (result != null)
                {
                    _.udata.balance = result.balance / 100000000m;
                    _.udata.bets = result.rolls_played;
                    _.udata.profit = result.dice_profit / 100000000m;
                    _.udata.wagered = result.wagered / 100000000m;

                    //GetBalance();

                    _.udata.Output("login", new Status { isError = false, message = "Login Succeed." });
                }
                else
                    _.udata.Output("login", new Status { isError = true, message = "Wrong email or password." });

                return;

            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                //_.udata.Output("login", new Status { isError = true, message = "Request time out." });
                _.udata.Output("login", new Status { isError = true, message = e.Message });
                return;
            }
        }

        public override async Task<bool> PlaceBet(bool High, decimal amount, decimal chance, string guid = "")
        {
            if (!_.udata.isRun)
            {
                return await Task.FromResult(false);
            }

            if (chance == 0)
            {
                _.udata.Output("placebet", new Status { isError = true, message = "Chance too low." });
                return await Task.FromResult(false);
            }
            if (amount == 0)
            {
                _.udata.Output("placebet", new Status { isError = true, message = "Bet amount too low." });
                return await Task.FromResult(false);
            }

            string ClientSeed = string.Empty;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZqwertyuiopasdfghjklzxcvbnm1234567890";
            for (int i = 0; i < 16; i++)
                ClientSeed += chars[r.Next(chars.Length)];

            string Params = string.Format(System.Globalization.NumberFormatInfo.InvariantInfo, "m={0}&client_seed={1}&jackpot=0&stake={2}&multiplier={3}&rand={4}&csrf_token={5}",
                    High ? "hi" : "lo", ClientSeed, amount, 95m / chance, r.Next(0, 9999999) / 10000000m, csrf);
            //https://freebitco.in/cgi-bin/bet.pl?m=hi&client_seed=wACFTfmLoZMHYNgE&jackpot=0&stake=0.00000001&multiplier=2.00&rand=0.7162693865123573&csrf_token=Zu67cND63vAL
            //string tmps = Content.ReadAsStringAsync().Result;
            //Bet_999dice result = new Bet_999dice();
            //_.ldata.SavePref("ssss", Params);
            try
            {
                using (var response = await Client.GetAsync("https://freebitco.in/cgi-bin/bet.pl?" + Params))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        string[] msgs = result.Split(':');
                        if (msgs.Length > 2)
                        {
                            /*
                                1. Success code (s1)
                                2. Result (w/l)
                                3. Rolled number
                                4. User balance
                                5. Amount won or lost (always positive). If 2. is l, then amount is subtracted from balance else if w it is added.
                                6. Redundant (can ignore)
                                7. Server seed hash for next roll
                                8. Client seed of previous roll
                                9. Nonce for next roll
                                10. Server seed for previous roll
                                11. Server seed hash for previous roll
                                12. Client seed again (can ignore)
                                13. Previous nonce
                                14. Jackpot result (1 if won 0 if not won)
                                15. Redundant (can ignore)
                                16. Jackpot amount won (0 if lost)
                                17. Bonus account balance after bet
                                18. Bonus account wager remaining
                                19. Max. amount of bonus eligible
                                20. Max bet
                                21. Account balance before bet
                                22. Account balance after bet
                                23. Bonus account balance before bet
                                24. Bonus account balance after bet
                            */
                            Models.LogsItem tmp = new Models.LogsItem
                            {
                                Time = DateTime.Now.ToLongTimeString(),
                                BetId = _.udata.bets.ToString(),
                                Amount = amount,
                                Chance = chance,
                                ClientSeed = ClientSeed,
                                IsHigh = High,
                                Profit = msgs[1] == "w" ? decimal.Parse(msgs[4]) : -decimal.Parse(msgs[4], System.Globalization.NumberFormatInfo.InvariantInfo),
                                ServerHash = msgs[10],
                                ServerSeed = msgs[9],
                                CurrentBalance = decimal.Parse(msgs[3], System.Globalization.NumberFormatInfo.InvariantInfo),
                                IsWin = (msgs[1] =="w"),
                                Roll = decimal.Parse(msgs[2], System.Globalization.NumberFormatInfo.InvariantInfo) / 100.0m
                            };

                            _.udata.balance = tmp.CurrentBalance;
                            if (tmp.IsWin) _.udata.wins++; else _.udata.losses++;
                            _.udata.bets++;
                            _.udata.wagered += amount;
                            _.udata.profit += tmp.Profit;
                            _.core.AddLogs(tmp);

                        }
                        else if(msgs.Length > 0)
                        {
                            //20 - too low balance
                            if (msgs.Length > 1)
                            {
                                if (msgs[1] == "20")
                                {
                                    _.udata.Output("placebet", new Status { isError = true, message = "[Error] Balance too low." });
                                    return await Task.FromResult(false);
                                }
                            }
                            else
                            {
                                _.udata.Output("placebet", new Status { isError = true, message = "[Error] Site returned unknown error." });
                                return await Task.FromResult(false);
                            }
                        }

                    }
                }
            }
            catch (AggregateException e)
            {
                Console.Write(e);
                if (retrycount++ < 3)
                {
                    _.udata.Output("placebet", new Status { isError = true, message = "send_rebet" });
                    return await Task.FromResult(false);
                }
            }

            if (_.udata.streakType == _.udata.win)
                _.udata.streakCurrent++;
            else
                _.udata.streakCurrent = 1;

            retrycount = 0;
            return await Task.FromResult(true);
        }

        public override Task Register()
        {
            throw new NotImplementedException();
        }

        void GetBalance()
        {
            if (_.udata.sessionId != "" && _.udata.sessionId != null)
            {
                Balance_Freebitcoin result = new Balance_Freebitcoin();
                using (var response = Client.GetStringAsync("https://freebitco.in/cgi-bin/api.pl?op=get_user_stats"))
                {
                    try
                    {
                        result = JsonConvert.DeserializeObject<Balance_Freebitcoin>(response.Result);
                    }
                    catch (AggregateException e)
                    {
                        //if (e.InnerException.Message.Contains("ssl"))
                        //{
                        //    GetBalance();
                        //    return;
                        //}
                    }
                }

                try
                {
                    _.udata.balance = result.balance / 100000000.0m;
                    _.udata.wagered = result.wagered / 100000000.0m;
                    _.udata.profit = result.dice_profit / 100000000.0m; ;
                    _.udata.bets = result.rolls_played;
                    //_.udata.wins = result;
                    //_.udata.losses = result.TotalLose;
                }
                catch
                {

                }
            }
        }

        public override Task ReLogin(string Json)
        {
            throw new NotImplementedException();
        }
    }
}
