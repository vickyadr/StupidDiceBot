using Dice_Bot.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dice_Bot.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Strategy : ContentPage
	{
        public string strategy { get; set; }
		public Strategy ()
		{
			InitializeComponent ();
            Title = "Learning Task";
            string temp = "--Write bid strategy here\r\nchance = 49.5 --sets your chance for placing a bet\r\n\r\nnextbet = 0.00000100 --sets your first bet.\r\nbethigh = true --bet high when true, bet low when false\r\nfunction dobet()\r\nend";
            _.udata.programs = strategy =_.ldata.LoadPref("strategy", temp);
            BindingContext = this;
		}

        private void Editor_TextChanged(object sender, TextChangedEventArgs e)
        {
            _.udata.programs = strategy;
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HelpPage());
        }

        private void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {
            _.ldata.SavePref("strategy", strategy);
        }
    }
}
