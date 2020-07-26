using Dice_Bot.Helpers;
using Dice_Bot.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dice_Bot.ViewModels
{
    public class HelpViewModel : BaseViewModel
    {
        public ObservableRangeCollection<HelpItem> Items { get; set; }

        public HelpViewModel() {
            Title = "Bot's Librarry";
            Addlib();
        }

        private void Addlib()
        {
            Items = new ObservableRangeCollection<HelpItem>();

            List<HelpItem> item = new List<HelpItem>
            {
                new HelpItem { Name = "balance : double [RO]", Description="return current balance" },
                new HelpItem { Name = "bets : int [RO]", Description="return total bet counter" },
                new HelpItem { Name = "chance : double [RW]", Description="return last bet chance, set current bet chance" },
                new HelpItem { Name = "currentstreak : int [RO]", Description="return currents streak" },
                new HelpItem { Name = "delaynext : int [RW]", Description="add delay in ms after current bet" },
                new HelpItem { Name = "delaynow : int [RW]", Description="add delay in ms on current bet" },
                new HelpItem { Name = "loses : int [RO]", Description="return total loses counter" },
                new HelpItem { Name = "nextbet : double [RW]", Description="return last bet amount, set current bet amount" },
                new HelpItem { Name = "profit : double [RO]", Description="return current profit" },
                new HelpItem { Name = "stop() [RO]", Description="make bot have a nap" },
                new HelpItem { Name = "win : bool [RO]", Description="return true if last bet is win" },
                new HelpItem { Name = "wins : int [RO]", Description="return total win counter" }
            };

            foreach (var i in item)
                Items.Add(i);
        }

    }
}
