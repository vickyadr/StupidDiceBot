using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DiceBot
{
    public class _
    {
        public static userData udata;
        public static TempData tdata;
        public _()
        {
            udata= new userData();
            tdata = new TempData();
        }
    }
    public class TempData
    {
        public bool win;
        public decimal currentProfit;
        public decimal currentStreak;
        public decimal previousbet;
        public decimal nextbet;
        public decimal chance;
        public bool betHigh;
        public decimal lastBet;
        public bool enablezz;
        public string programs;
    }

    public class userData
    {
        //General
        public string sessionId { get; set; }
        public string seedCurrent { get; set; }
        public string seedNext { get; set; }
        public string hashCurrent { get; set; }
        public string hashNext { get; set; }
        public string balance { get; set; }

        //--
        public string site { get; set; }
        public string currencies { get; set; }
        public string lastBet { get; set; }
        public int wins { get; set; }
        public int losses { get; set; }
        public decimal bets { get; set; }
        public decimal profit { get; set; }
    }
}