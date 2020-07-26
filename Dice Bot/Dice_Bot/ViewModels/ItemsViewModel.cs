using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Dice_Bot.Helpers;
using Dice_Bot.Models;
using Dice_Bot.Views;

using Xamarin.Forms;

namespace Dice_Bot.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableRangeCollection<LogsItem> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ItemsViewModel()
        {
            Title = "Table of Destiny";
            Items = new ObservableRangeCollection<LogsItem>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<Modules.Cores, LogsItem>(this, "AddLogs", async (obj, item) =>
            {
                var _item = item as LogsItem;
                _item.Id = Guid.NewGuid().ToString();
                //Items.Add(_item);
                Items.Insert(0, _item);

                var getset = await SettingStore.GetItemAsync("Max bet log");
                int count = 0, icount = Items.Count;
                int.TryParse(getset.Val.ToString(), out count);

                if (icount > count && count != 0)
                {
                    //for (int i = 0; i <= (icount - count); i++)
                    for (int i= icount; i>=count; i--)
                    {
                        //setting s = new setting { Name = Items[i].Id };
                        //await SettingStore.DeleteItemAsync(s);
                        //await SettingStore.DeleteItemAsync(new setting());
                        Items.RemoveAt(0);
                    }
                }

                //await DataStore.AddItemAsync(_item);
            });

            MessagingCenter.Subscribe<Logs>(this, "ClearLog", async (obj) =>
            {
                await DataStore.DeleteItemAsync();
                Items.Clear();
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {

                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
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
                IsBusy = false;
            }
        }

    }
}