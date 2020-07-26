using Dice_Bot.Modules;
using Dice_Bot.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dice_Bot.Views
{
	public partial class Main : ContentPage
	{
        public MainViewModel viewModel;
        public static Models.main mainData { set; get; }
        public Main ()
		{
			InitializeComponent ();
            BindingContext = viewModel = new MainViewModel();
            mainData = viewModel.mainData;

            if (_.udata.isLogin)
            {
                _.core.UpdateButton = (_.udata.isRun) ? e_.ButtonStart.Started : e_.ButtonStart.Stopped;
                _.core.UpdateMain();
            }
        }

        private async void Login_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Login());
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if(!_.udata.isRun)
                _.core.Start();
            else
                _.core.Stop();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}
