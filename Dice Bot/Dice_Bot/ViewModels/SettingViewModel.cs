using Dice_Bot.Helpers;
using Dice_Bot.Modules;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Dice_Bot.ViewModels
{
    public class SettingViewModel : BaseViewModel
    {
        public ObservableRangeCollection<Models.setting> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public SettingViewModel()
        {
            Title = "Bot's Garage";

            Items = new ObservableRangeCollection<Models.setting>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<Cores, Models.setting>(this, "UpdateSetting", async (obj, args) =>
            {
                args.Id = Guid.NewGuid().ToString();
                await SettingStore.UpdateItemAsync(args);
                _.ldata.SavePref(args.Name, args.Val.ToString());
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsSettingBusy)
                return;

            IsSettingBusy = true;
            try
            {
                Items.Clear();
                var items = await SettingStore.GetItemsAsync(true);
                Items.ReplaceRange(items);
                foreach (var item in items)
                {
                    if (item.Name == "Bet sleep")
                    {
                        int i = 0;
                        if (int.TryParse(item.Val.ToString(), out i))
                            _.udata.sleepTime = i;
                    }
                    else if (item.Name == "Stop mode")
                        _.udata.stopnow = (item.Val.ToString() == "force");
                }
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
                IsSettingBusy = false;
            }
        }
    }
}
