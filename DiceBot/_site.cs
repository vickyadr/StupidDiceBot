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
    /// <summary>
    /// Abstact Factorry Pattern
    /// </summary>
    public abstract class _site
    {
        public abstract decimal balance { get; set; }
        public abstract decimal profit { get; set; }
        public abstract decimal wins { get; set; }
        public abstract decimal looses { get; set; }
        public abstract string programs { get; set; }

        public abstract void Login(string user, string pass, string twofa = "");
        public abstract void Register();
        public abstract void PlaceBet(bool High, decimal amount, decimal chance, string guid = "");
    }
}