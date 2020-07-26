using System;
using System.Collections.Generic;
using System.Text;

namespace Dice_Bot.Modules
{
    
    public class Login_Freebitcoin : Balance_Freebitcoin
    {

    }

    public class Balance_Freebitcoin
    {
        public long wagered { get; set; }
        public long rolls_played { get; set; }
        public decimal lottery_spent { get; set; }
        public string status { get; set; }
        public decimal jackpot_winnings { get; set; }
        public decimal jackpot_spent { get; set; }
        public decimal reward_points { get; set; }
        public decimal balance { get; set; }
        public decimal total_winnings { get; set; }
        public decimal dice_profit { get; set; }
    }

    public class Bet_Freebitcoin
    {

    }
}
