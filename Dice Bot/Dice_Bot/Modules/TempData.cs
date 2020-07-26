using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Dice_Bot.Modules
{

    public class userData
    {
        //General
        public string sessionId { get; set; }
        public string seedCurrent { get; set; }
        public string seedNext { get; set; }
        public string hashCurrent { get; set; }
        public string hashNext { get; set; }
        public decimal balance { get; set; }
        public string currencies { get; set; }
        public string coinname { get; set; }

        //--stats
        public string site { get; set; }
        public long lastBet { get; set; }
        public long wins { get; set; }
        public long losses { get; set; }
        public long bets { get; set; }
        public decimal profit { get; set; }
        public decimal wagered { get; set; }

        //--use on
        public bool win { get; set; }
        public decimal currentProfit { get; set; }
        public decimal previousbet { get; set; }
        public decimal nextbet { get; set; }
        public decimal chance { get; set; }
        public bool betHigh { get; set; }
        public bool enablezz { get; set; }
        public int streakCurrent { get; set; }
        public bool streakType { get; set; }

        //--other
        public string programs { get; set; }
        public bool isRun { get; set; }
        public int sleepTime { get; set; }
        public bool stopnow { get; set; }
        public string loginReceived { get; set; }
        public _site siteUsed { get; set; }
        public bool isLogin { get; set; }

        private Dictionary<string, Status> output = new Dictionary<string, Status>();

        /// <summary>
        /// Return true if error
        /// </summary>
        /// <param name="sKey">Key</param>
        /// <param name="st"></param>
        /// <returns></returns>
        public bool Output(string sKey, out string st)
        {
            st = string.Empty;
            bool result = false;
            if (output.ContainsKey(sKey))
            {
                st = output[sKey].message;
                result = output[sKey].isError;
                output.Remove(sKey);
            }
            return result;
        }

        public bool Output(string sKey)
        {
            bool result = false;
            if (output.ContainsKey(sKey))
            {
                output.Remove(sKey);
                result = output[sKey].isError;
            }
            return result;
        }

        public void Output(string sKey, Status sVal, bool force = false)
        {
            if (!output.ContainsKey(sKey))
            {
                output.Add(sKey, sVal);
            }
            else if (force)
            {
                output.Remove(sKey);
                output.Add(sKey, sVal);
            }
        }

        public void Reset()
        {
            sessionId = string.Empty;
            seedCurrent = string.Empty;
            seedNext = string.Empty;
            hashCurrent = string.Empty;
            hashNext = string.Empty;
            balance = 0;
            currencies = string.Empty;

            //--stats
            site = string.Empty;
            lastBet = 0;
            wins = 0;
            losses = 0;
            bets = 0;
            profit = 0;
            wagered = 0;

            //--use on
            win = false;
            currentProfit = 0;
            streakCurrent = 0;
            previousbet = 0;
            nextbet = 0;
            chance = 0;
        }
    }

    public class Status
    {
        public string message { get; set; }
        public bool isError { get; set; }
    }
}
