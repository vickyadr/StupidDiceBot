using System;

namespace Dice_Bot.Models
{
    public class LogsItem : BaseDataObject
	{
		string betId = string.Empty;
		public string BetId
		{
			get { return betId; }
			set { SetProperty(ref betId, value); }
		}

        string time = string.Empty;
		public string Time
		{
			get { return time; }
			set { SetProperty(ref time, value); }
		}

        decimal amount = 0;
        public decimal Amount
        {
            get { return amount; }
            set { SetProperty(ref amount, value); }
        }

        bool isHigh = false;
        public bool IsHigh
        {
            get { return isHigh; }
            set { SetProperty(ref isHigh, value); }
        }

        public string High { get { return (isHigh) ? "High" : "Low";} }

        bool isWin = false;
        public bool IsWin
        {
            get { return isWin; }
            set { SetProperty(ref isWin, value); }
        }

        public bool IsLose
        {
            get { return !isWin; }
        }

        public string Win { get { return (isWin) ? "(Win)" : "(Lose)"; } }

        decimal chance = 0;
        public decimal Chance
        {
            get { return chance; }
            set { SetProperty(ref chance, value); }
        }

        decimal roll = 0;
        public decimal Roll
        {
            get { return roll; }
            set { SetProperty(ref roll, value); }
        }

        decimal profit = 0;
        public decimal Profit
        {
            get { return profit; }
            set { SetProperty(ref profit, value); }
        }

        decimal currentbalance = 0;
        public decimal CurrentBalance
        {
            get { return currentbalance; }
            set { SetProperty(ref currentbalance, value); }
        }

        public string cBalance
        {
            get { return "("+currentbalance+")"; }
        }

        string serverhash = string.Empty;
        public string ServerHash
        {
            get { return serverhash; }
            set { SetProperty(ref serverhash, value); }
        }

        string serverseed = string.Empty;
        public string ServerSeed
        {
            get { return serverseed; }
            set { SetProperty(ref serverseed, value); }
        }

        string clientseed = string.Empty;
        public string ClientSeed
        {
            get { return clientseed; }
            set { SetProperty(ref clientseed, value); }
        }
    }
}
