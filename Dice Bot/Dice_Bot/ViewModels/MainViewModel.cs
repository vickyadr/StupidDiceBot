using Dice_Bot.Helpers;
using Dice_Bot.Models;
using Dice_Bot.Modules;
using Dice_Bot.Views;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Dice_Bot.ViewModels
{
	public class MainViewModel : BaseViewModel
	{
        public main mainData { get; set; }
        public ObservableRangeCollection<mainLog> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public MainViewModel()
		{
			Title = "Training Room";
            mainData = new main();
            Items = new ObservableRangeCollection<mainLog>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<Cores>(this, "UpdateMain", async (obj) =>
            {
                await UpdateMain();
            });

            MessagingCenter.Subscribe<Cores, e_.ButtonStart>(this, "UpdateButton", async (obj,e) =>
            {
                await UpdateButton(e);
            });

            MessagingCenter.Subscribe<Cores, mainLog>(this, "WiteTask", async (obj, task) =>
            {
                var _item = task as mainLog;
                _item.Id = Guid.NewGuid().ToString();
                Items.Insert(0, _item);
                //Items.Add(_item);

                var getset = await SettingStore.GetItemAsync("Max bot log");
                int count = 0, icount = Items.Count;
                int.TryParse(getset.Val.ToString(), out count);

                if (icount > count && count != 0)
                {
                    for (int i = icount-1; i >= count; i--)
                    {
                        //await MainStore.DeleteItemAsync(new mainLog());
                        Items.RemoveAt(i);
                    }
                }

                //await MainStore.AddItemAsync(_item);
            });
            Random r = new Random();
            /*TESTING AREA*/

        }

        async Task UpdateButton(e_.ButtonStart e)
        {
            mainData.IsLogin = _.udata.isLogin;
            if (!_.udata.isLogin)
            {
                mainData.StartBot = "LOCKED";
                return;
            }

            switch (e)
            {
                case e_.ButtonStart.Started:
                    mainData.StartBot = "Take a Nap";
                    break;
                case e_.ButtonStart.Waiting:
                    mainData.StartBot = "Waiting...";
                    break;
                case e_.ButtonStart.Stopped:
                    mainData.StartBot = "Teach Bot Now !!!";
                    break;
                default:
                    break;
            }
        }

        async Task UpdateMain()
        {
            try
            {
                mainData.Balance = _.udata.balance;
                mainData.Profit = _.udata.profit;
                mainData.Wagered = _.udata.wagered;
                mainData.Wins = _.udata.wins;
                mainData.Loses = _.udata.losses;
                mainData.Bets = _.udata.losses + _.udata.wins;
                mainData.IsUp = _.udata.streakType;
                mainData.IsDown = !_.udata.streakType;
                mainData.Currency = _.udata.coinname;
                i = 15;
                await HideIndicator();
            }
            catch
            {
                System.Threading.Thread.Sleep(100);
                await UpdateMain();
            }
        }

        int i = 0;

        async Task HideIndicator()
        {
            while (i > 1)
            {
                System.Threading.Thread.Sleep(100);
                i--;
            }
            mainData.IsDown = mainData.IsUp = false;
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsMainBusy)
                return;

            IsMainBusy = true;
            try
            {
                Items.Clear();
                var items = await MainStore.GetItemsAsync(true);
                Items.ReplaceRange(items);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessagingCenter.Send(new MessagingCenterAlert
                {
                    Title = "Error",
                    Message = "Unable to load items.",
                    Cancel = "OK"
                }, "message");
            }
            finally
            {
                IsMainBusy = false;
            }
        }
    }
}
