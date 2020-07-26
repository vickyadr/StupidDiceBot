using System;

using Dice_Bot.Models;

using Xamarin.Forms;
using Dice_Bot.Modules;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using Android.Content;
using Android.App;

namespace Dice_Bot.Views
{
	public partial class Login : ContentPage
    {
		public login loginItem  { get; set; }
        Button b;
        Picker p, p2;
        List<string> currency = new List<string>();
        public Login()
		{
			InitializeComponent();

            b = this.FindByName<Button>("login");
            p = this.FindByName<Picker>("site");
            p2 = this.FindByName<Picker>("siteCurrencies");
            
            foreach (var item in _.sdata)
            {
                p.Items.Add(item.Key);
            }
            loginItem = new login();
            loginItem.LoginAs = "Login Id";
            BindingContext = this;
		}


        async void Login_Clicked(object sender, EventArgs e)
		{
            ///************************************* START OF TESTING AREA *********************************************///
            //Modules.Notif n = new Notif();
            //return;
            ///************************************** END OF TESTING AREA *********************************************///
            loginItem.IsLogining = true;
            if (_.udata.isRun)
            {
                loginItem.Message = "I cant handle multi task, i'm on learning now.";
                loginItem.IsMessage = true;
                return;
            }

            if (loginItem.Site == -1)
            {
                loginItem.Message = "Where must i go, no targeted site here.";
                loginItem.IsMessage = true;
                return;
            }

            if (loginItem.UseCurrencies && loginItem.Currencies == -1)
            {
                loginItem.Message = "What currencies must i play on ?";
                loginItem.IsMessage = true;
                return;
            }

            if (loginItem.Username.Length < 1)
            {
                loginItem.Message = "Sorry, security ask me my username to pass.";
                loginItem.IsMessage = true;
                return;
            }

            if (loginItem.Password.Length < 1)
            {
                loginItem.Message = "There is password on a wall what must i do ?.";
                loginItem.IsMessage = true;
                return;
            }

            //Main.mainData.IsLogin = false;
            _.udata.isLogin = false;
            loginItem.IsLoading = true;
            loginItem.IsMessage = false;
            _.udata.Reset();
            _.core.SetSite = _.sdata[p.Items[loginItem.Site]].SiteScript;
            string tmp= _.udata.currencies, tmp2 = _.udata.coinname;

            //Write to log
            _.core.AddMainLog = string.Format("[Login] Trying to enter {0} {1}", p.Items[p.SelectedIndex], (loginItem.UseCurrencies) ? "via "+ p2.Items[loginItem.Currencies] : "");

            _.udata.currencies = (loginItem.UseCurrencies) ? currency[loginItem.Currencies] : "";
            _.udata.coinname = (loginItem.UseCurrencies) ? p2.Items[loginItem.Currencies] : p2.Items[0];
            await Task.Run(() => _.core.Login(loginItem.Username, loginItem.Password, loginItem.FACode));
            _.core.UpdateButton = e_.ButtonStart.Stopped;
            _.core.UpdateMain();
            loginItem.IsLoading = false;

            string message = string.Empty;
            if (!_.udata.Output("login", out message))
            {
                //Main.mainData.IsLogin = true;
                _.udata.isLogin = true;
                _.core.UpdateButton = e_.ButtonStart.Stopped;
                _.core.AddMainLog = string.Format("[Login] Bot now in the room !");
            }else {
                _.core.AddMainLog = string.Format("[Login] Kicked by security !");
                _.udata.currencies = tmp;
                _.udata.coinname = tmp2;
            }

            loginItem.Message = message;
            loginItem.IsMessage = true;
        }
    
        private async void Button_Clicked(object sender, EventArgs e)
        {
            loginItem.IsMessage = false;
            loginItem.Message = null;
            loginItem.IsLogining = false;
            loginItem.IsLoading = false;
            if(_.udata.isLogin)
                await Navigation.PopToRootAsync();
        }

        private void _SelectedIndexChanged(object sender, EventArgs e)
        {
            string s = p.Items[p.SelectedIndex];
            loginItem.UseCurrencies = (_.sdata[s].UseCurrency);
            p2.Items.Clear();
            currency.Clear();
            loginItem.LoginAs = _.sdata[s].loginAs;
            foreach (var item in _.sdata[s].Currencies)
            {
                p2.Items.Add(item.Key);
                currency.Add(item.Value);
            }
        }
    }
}