using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Dice_Bot.Models;

using Xamarin.Forms;

[assembly: Dependency(typeof(Dice_Bot.Services.SettingDataStore))]
namespace Dice_Bot.Services
{
    public class SettingDataStore : IDataStore<setting>
    {
        bool isInitialized;
        List<setting> items;

        public async Task<bool> AddItemAsync(setting item)
        {
            await InitializeAsync();

            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(setting item)
        {
            await InitializeAsync();
            var _item = items.Where((setting arg) => arg.Name == item.Name).FirstOrDefault();
            _item.Val = item.Val;

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(setting item)
        {
            await InitializeAsync();

            var _item = items.Where((setting arg) => arg.Name == item.Name).FirstOrDefault();
            items.Remove(_item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync()
        {
            await InitializeAsync();

            items.Clear();

            return await Task.FromResult(true);
        }

        public async Task<setting> GetItemAsync(string name)
        {
            await InitializeAsync();

            return await Task.FromResult(items.FirstOrDefault(s => s.Name == name));
        }

        public async Task<IEnumerable<setting>> GetItemsAsync(bool forceRefresh = false)
        {
            await InitializeAsync();

            return await Task.FromResult(items);
        }

        public Task<bool> PullLatestAsync()
        {
            return Task.FromResult(true);
        }


        public Task<bool> SyncAsync()
        {
            return Task.FromResult(true);
        }

        public async Task InitializeAsync()
        {
            if (isInitialized)
                return;

            items = new List<setting>();
            var _items = new List<setting>//();
            {
                new setting { Id = Guid.NewGuid().ToString(), Name = "Max bot log", Format="Maximum log {0}, if reached old log will deleted.", Val=20, piker = new object[] {10,20,30,40,50} },
                new setting { Id = Guid.NewGuid().ToString(), Name = "Max bet log", Format="Maximum log {0}, if reached old log will deleted.", Val=20, piker = new object[] {20,50,80,200,"Unlimited"} },
                new setting { Id = Guid.NewGuid().ToString(), Name = "Bet sleep", Format="Delay between bet {0}ms", Val=1200, piker = new object[] {500,1000,1500,2000,2500} },
                new setting { Id = Guid.NewGuid().ToString(), Name = "Stop mode", Format="Stop button will {0} stop task", Val="force", piker = new object[] {"force", "wait"} },
                new setting { Id = Guid.NewGuid().ToString(), Name = "Login chache", Format="Resume without re-login \"{0}\" (yet available, verry buggy)", Val="Disable", piker = new object[] {"Enable", "Disable"} },
                new setting { Id = Guid.NewGuid().ToString(), Name = "Show notification bar", Format="{0} (yet available)", Val="Disable", piker = new object[] {"Disable", "Enable"} },
            };

            foreach (setting item in _items)
            {
                item.Val= Modules._.ldata.LoadPref(item.Name, item.Val.ToString());
                items.Add(item);
            }
            isInitialized = true;
        }
    }

}
