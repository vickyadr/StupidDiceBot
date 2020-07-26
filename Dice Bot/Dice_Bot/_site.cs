using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Dice_Bot
{
    /// <summary>
    /// Abstact Factorry Pattern
    /// </summary>
    public abstract class _site
    {
        protected HttpClientHandler ClientHandlr;
        protected HttpClient Client;
        protected string sessionCookie = string.Empty;
        protected Random r = new Random();

        public abstract Task Login(string user, string pass, string twofa);
        public abstract Task ReLogin(string Json);
        public abstract Task Register();
        public abstract Task<bool> PlaceBet(bool High, decimal amount, decimal chance, string guid = "");
    }
}
