using System;

using Dice_Bot.Models;
using Dice_Bot.ViewModels;

using Xamarin.Forms;

namespace Dice_Bot.Views
{
	public partial class Logs : ContentPage
	{
		ItemsViewModel viewModel;

		public Logs()
		{
			InitializeComponent();
			BindingContext = viewModel = new ItemsViewModel();
		}

		async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
		{
			var item = args.SelectedItem as LogsItem;
			if (item == null)
				return;
            
            // Manually deselect item
            ItemsListView.SelectedItem = null;

            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));
		}

		void Clear_Clicked(object sender, EventArgs e)
		{

            ///************************************* START OF TESTING AREA *********************************************///
            //Modules._.core.AddLogs(new LogsItem { Amount = 0.000002m, BetId = "11298267", Chance = 27.5m, ClientSeed = "oeuiheiowndowinxaoinxamxoiwiiwiwopqpoowoooiewoproi", CurrentBalance = 0.001m, IsHigh = false, IsWin = true, Profit = 0.00005m, Roll = 32.24m, ServerHash = "hd98w8e88w8ejdjjjckkckkdkkppdoojfodjf", ServerSeed = "1398139103983131319313093000489", Time = DateTime.Now.ToLongTimeString() });
            //Modules._.core.AddLogs(new LogsItem { Amount = 0.000001m, BetId = "11298309", Chance = 49.5m, ClientSeed = "oeuiheiowndowinxaoinxamxoiwiiwiwopqpoowoooiewoproi", CurrentBalance = 0.001m, IsHigh = true, IsWin = false, Profit = -0.000001m, Roll = 22.32m, ServerHash = "hd98w8e88w8ejdjjjckkckkdkkppdoojfodjf", ServerSeed = "1398139103983131319313093000489", Time = DateTime.Now.ToLongTimeString() });
            //return;
            ///************************************** END OF TESTING AREA *********************************************///
            MessagingCenter.Send(this, "ClearLog");

		}

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}
