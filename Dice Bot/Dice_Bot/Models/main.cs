using System;

namespace Dice_Bot.Models
{
    public class main : BaseDataObject
    {
        bool isLogin = false;
        string startBot = "Locked";

        public bool IsLogin
        {
            get { return isLogin; }
            set
            {
                SetProperty(ref isLogin, value);
                StartBot = (isLogin) ? "Teach Bot Now !!!" : "Locked";
            }
        }

        public string StartBot
        {
            get { return startBot; }
            set { SetProperty(ref startBot, value); }
        }

        decimal balance = 0;
        public decimal Balance
        {
            get { return balance; }
            set { SetProperty(ref balance, value); }
        }

        decimal profit = 0;
        public decimal Profit
        {
            get { return profit; }
            set { SetProperty(ref profit, value); }
        }

        decimal wager = 0;
        public decimal Wagered
        {
            get { return wager; }
            set { SetProperty(ref wager, value); }
        }

        long bet = 0;
        public long Bets
        {
            get { return bet; }
            set { SetProperty(ref bet, value); }
        }

        long win = 0;
        public long Wins
        {
            get { return win; }
            set { SetProperty(ref win, value); }
        }

        long lose = 0;
        public long Loses
        {
            get { return lose; }
            set { SetProperty(ref lose, value); }
        }

        bool isUp = false;

        public bool IsUp
        {
            get { return isUp; }
            set { SetProperty(ref isUp, value); }
        }

        bool isDown = false;
        public bool IsDown
        {
            get { return isDown; }
            set { SetProperty(ref isDown, value); }
        }

        string currency = string.Empty;

        public string Currency
        {
            get { return currency; }
            set { SetProperty(ref currency, value.ToUpper()); }
        }
    }

    public class mainLog : BaseDataObject
    {
        string mesg = string.Empty;
        public string Message
        {
            get { return mesg; }
            set { SetProperty(ref mesg, value); }
        }

        DateTime time = new DateTime();
        public DateTime Time
        {
            get { return time; }
            set {
                SetProperty(ref time, value);
                sTime = string.Format("[{0}]", value.ToLongTimeString());
            }
        }

        string sTime = string.Empty;
        public string STime
        {
            get { return sTime; }
            set { SetProperty(ref sTime, value); }
        }
    }
}
