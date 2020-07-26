using System;
using System.Collections.Generic;
using System.Text;

namespace Dice_Bot.Modules
{
    public class Balance_999dice
    {
        public decimal Balance { get; set; }
        public long TotalBets { get; set; }
        public long TotalWins { get; set; }
        public long TotalLose { get { return TotalBets - TotalWins; } }
        public decimal Wagered { get { return TotalPayOut - TotalPayIn; } }

        public decimal TotalPayIn { get; set; }
        public decimal TotalPayOut { get; set; }
        public decimal TotalProfit { get { return TotalPayIn + TotalPayOut; } }
    }

    public class Register_999dice
    {
        public string AccountCookie { get; set; }
        public string SessionCookie { get; set; }
        public long Accountid { get; set; }
        public int MaxBetBatchSize { get; set; }
        public string ClientSeed { get; set; }
        public string DepositAddress { get; set; }
    }

    public class Login_999dice : Register_999dice
    {
        public decimal Balance { get; set; }
        public string Email { get; set; }
        public string EmergenctAddress { get; set; }
        public long BetCount { get; set; }
        public long BetWinCount { get; set; }
        public long BetLoseCount { get { return BetCount - BetWinCount; } }
        public decimal BetPayIn { get; set; }
        public decimal BetPayOut { get; set; }
        public decimal Profit { get { return BetPayIn + BetPayOut; } }
        public decimal Wagered { get { return BetPayOut - BetPayIn; } }

        public decimal TotalPayIn { get; set; }
        public decimal TotalPayOut { get; set; }
        public decimal TotalProfit { get { return TotalPayIn + TotalPayOut; } }

        public long TotalBets { get; set; }
        public long TotalWins { get; set; }
        public long TotalLoseCount { get { return TotalBets - TotalWins; } }
    }

    public class Hash_999dice
    {
        public string Hash { get; set; }
    }
    public class Deposit_999dice
    {
        public string Address { get; set; }
    }
    public class Bet_999dice
    {
        public string BetId { get; set; }
        public decimal PayOut { get; set; }
        public decimal Secret { get; set; }
        public decimal StartingBalance { get; set; }
        public string ServerSeed { get; set; }
        public string Next { get; set; }

        public int ChanceTooHigh { get; set; }
        public int ChanceTooLow { get; set; }
        public int InsufficientFunds { get; set; }
        public int NoPossibleProfit { get; set; }
        public int MaxPayoutExceeded { get; set; }
    }
}
