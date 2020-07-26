using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Dice_Bot.Modules
{
    public class _999dice : _site
    {
        int retrycount;
        public _999dice()
        { }

        public override async Task Login(string user, string pass, string twofa)
        {
            try
            {
                ClientHandlr = new HttpClientHandler { UseCookies = true, AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip }; ;
                Client = new HttpClient(ClientHandlr) { BaseAddress = new Uri("https://www.999dice.com/api/web.aspx") };
                Client.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("gzip"));
                Client.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("deflate"));

                List<KeyValuePair<string, string>> pairs = new List<KeyValuePair<string, string>>();
                pairs.Add(new KeyValuePair<string, string>("a", "Login"));
                pairs.Add(new KeyValuePair<string, string>("key", "fe5d6b6be76d40759cdf3b22d10f5ffb"));
                if (twofa != "" && twofa != null) pairs.Add(new KeyValuePair<string, string>("Totp", twofa));
                pairs.Add(new KeyValuePair<string, string>("Username", user));
                pairs.Add(new KeyValuePair<string, string>("Password", pass));

                FormUrlEncodedContent Content = new FormUrlEncodedContent(pairs);
                Login_999dice result = new Login_999dice();
            

            try
            {
                using (var response = await Client.PostAsync("", Content))
                {
                    var json = await response.Content.ReadAsStringAsync();
                    _.udata.loginReceived = json;
                    result = JsonConvert.DeserializeObject<Login_999dice>(json);
                }
            }
            catch (AggregateException e)
            {
                Console.Write(e.Message);
                    _.udata.Output("login", new Status { isError = true, message = "An error has occurred." });
                    return;
            }
            
                if (result.SessionCookie != null)
                {
                    _.udata.sessionId = result.SessionCookie;

                    _.udata.bets = result.BetCount;
                    _.udata.wins = result.BetWinCount;
                    _.udata.losses = result.BetLoseCount;

                    _.udata.balance = result.Balance / 100000000.0m;
                    _.udata.profit = result.TotalProfit / 100000000.0m;
                    _.udata.wagered = result.Wagered / 100000000.0m;

                    GetBalance();

                    _.udata.seedNext = result.ClientSeed;

                    _.udata.Output("login", new Status { isError = false, message = "Login Succeed." });
                }else
                    _.udata.Output("login", new Status { isError = true, message = "Wrong username or password." });
                return;

            } catch (Exception e) {
                Console.Write(e.Message);
                _.udata.Output("login", new Status { isError = true, message = "Request time out." });
                return;
            }
        }

        public override async Task ReLogin(string json)
        {
            Login_999dice result = new Login_999dice();
            result = JsonConvert.DeserializeObject<Login_999dice>(json);
            if (result.SessionCookie != null)
            {
                _.udata.sessionId = result.SessionCookie;

                _.udata.bets = result.BetCount;
                _.udata.wins = result.BetWinCount;
                _.udata.losses = result.BetLoseCount;

                _.udata.balance = result.Balance / 100000000.0m;
                _.udata.profit = result.TotalProfit / 100000000.0m;
                _.udata.wagered = result.Wagered / 100000000.0m;

            }

            return;
        }

        public override async Task<bool> PlaceBet(bool High, decimal amount, decimal chance, string guid = "")
        {
            if (!_.udata.isRun)
                return await Task.FromResult(false);

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
            List<KeyValuePair<string, string>> pairs = new List<KeyValuePair<string, string>>();
            FormUrlEncodedContent Content = new FormUrlEncodedContent(pairs);

            string ClientSeed = r.Next(0, int.MaxValue).ToString();
            chance = (999999.0m) * (chance / 100.0m);

            pairs = new List<KeyValuePair<string, string>>();
            pairs.Add(new KeyValuePair<string, string>("a", "PlaceBet"));
            pairs.Add(new KeyValuePair<string, string>("s", _.udata.sessionId));
            pairs.Add(new KeyValuePair<string, string>("PayIn", ((long)((decimal)amount * 100000000m)).ToString("0", System.Globalization.NumberFormatInfo.InvariantInfo)));
            pairs.Add(new KeyValuePair<string, string>("Low", (High ? 999999 - (int)chance : 0).ToString(System.Globalization.NumberFormatInfo.InvariantInfo)));
            pairs.Add(new KeyValuePair<string, string>("High", (High ? 999999 : (int)chance).ToString(System.Globalization.NumberFormatInfo.InvariantInfo)));
            pairs.Add(new KeyValuePair<string, string>("ClientSeed", ClientSeed));
            pairs.Add(new KeyValuePair<string, string>("Currency", _.udata.currencies));
            pairs.Add(new KeyValuePair<string, string>("ProtocolVersion", "2"));
            Content = new FormUrlEncodedContent(pairs);
            //string tmps = Content.ReadAsStringAsync().Result;
            Bet_999dice result = new Bet_999dice();

            try
            {
                using (var response = await Client.PostAsync("", Content))
                {
                    try
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<Bet_999dice>(json);
                    }
                    catch (AggregateException e)
                    {
                        //Console.Write(e);
                        //Parent.DumpLog(e.InnerException.Message, 0);
                        retrycount++;

                        if (retrycount < 3)
                        {
                            //PlaceBetThread(High);
                            //await PlaceBet(High, amount, chance);
                            //_.udata.Output("placebet", new Status { isError = true, message= "Fail... trying to place bet again." });
                            _.udata.Output("placebet", new Status { isError = true, message = "send_rebet" });
                            return await Task.FromResult(false);
                        }
                        if (e.InnerException.Message.Contains("ssl"))
                        {
                            //rebet
                            //PlaceBetThread(High);
                            //await PlaceBet(High, amount, chance);
                            _.udata.Output("placebet", new Status { isError = true, message = "send_rebet" });
                            return await Task.FromResult(false);
                        }
                        else
                        {
                            //Parent.updateStatus("An error has occurred");
                            _.udata.Output("placebet", new Status { isError = true, message = "send_rebet" });
                            _.core.AddMainLog = "[Error] An error has occurred.";
                            return await Task.FromResult(false);
                        }
                    }
                }
            }
            catch (AggregateException e)
            {
                Console.Write(e);
                retrycount++;
                if (retrycount < 3)
                {
                    //_.core.AddMainLog = "[Error] Fail... trying to place bet again.";
                    //PlaceBetThread(High);
                    //await PlaceBet(High, amount, chance);
                    _.udata.Output("placebet", new Status { isError = true, message = "send_rebet" });
                    return await Task.FromResult(false);
                }
            }

            _.udata.balance = ((result.StartingBalance + result.PayOut)/ 100000000m) - amount;
            _.udata.profit += (result.PayOut/ 100000000m) - amount;
            _.udata.win = (result.PayOut > 0);
            _.udata.bets++;

            if (_.udata.win)
                _.udata.wins+=1;
            else
                _.udata.losses+=1;

            if (_.udata.streakType == _.udata.win)
                _.udata.streakCurrent++;
            else
            {
                _.udata.streakCurrent = 1;
                _.udata.streakType = _.udata.win;
            }

            _.udata.wagered += amount;

            Models.LogsItem tmp = new Models.LogsItem();
            tmp.Roll = result.Secret / 10000m;
            tmp.Chance = chance * 100m / 999999m;
            tmp.ClientSeed = ClientSeed;
            tmp.BetId = result.BetId;
            tmp.Amount = amount;
            tmp.IsHigh = High;
            tmp.IsWin = _.udata.win;//(((tmp.Roll > (99.99m - chance)) && High) || ((tmp.Roll < chance) && !High));
            tmp.Profit = (result.PayOut / 100000000m) - amount;
            tmp.CurrentBalance = _.udata.balance;
            tmp.ServerSeed = result.ServerSeed;
            //tmp.ServerHash = 
            tmp.Time = DateTime.Now.ToLongTimeString();

            _.core.AddLogs(tmp);

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
                List<KeyValuePair<string, string>> pairs = new List<KeyValuePair<string, string>>();
                pairs.Add(new KeyValuePair<string, string>("a", "GetBalance"));
                pairs.Add(new KeyValuePair<string, string>("s", _.udata.sessionId));
                pairs.Add(new KeyValuePair<string, string>("Currency", _.udata.currencies));
                pairs.Add(new KeyValuePair<string, string>("Stats", "1"));
                FormUrlEncodedContent Content = new FormUrlEncodedContent(pairs);
                Balance_999dice result = new Balance_999dice();
                using (var response = Client.PostAsync("", Content))
                {
                    try
                    {
                        result = JsonConvert.DeserializeObject<Balance_999dice>(response.Result.Content.ReadAsStringAsync().Result);
                    }
                    catch (AggregateException e)
                    {
                        if (e.InnerException.Message.Contains("ssl"))
                        {
                            GetBalance();
                            return;
                        }
                    }
                }

                try
                {
                    _.udata.balance = result.Balance / 100000000.0m;
                    _.udata.wagered = -result.TotalPayIn / 100000000.0m;
                    _.udata.profit = result.TotalProfit / 100000000.0m; ;
                    _.udata.bets = result.TotalBets;
                    _.udata.wins = result.TotalWins;
                    _.udata.losses = result.TotalLose;
                }
                catch
                {

                }
            }
        }
    }
}
