using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Dice_Bot.Models;

using Xamarin.Forms;

[assembly: Dependency(typeof(Dice_Bot.Services.MainDataStore))]
namespace Dice_Bot.Services
{
    public class MainDataStore : IDataStore<mainLog>
    {
        bool isInitialized;
        List<mainLog> items;

        public async Task<bool> AddItemAsync(mainLog item)
        {
            await InitializeAsync();

            items.Add(item);
            //items.Insert(0, item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(mainLog item)
        {
            await InitializeAsync();

            var _item = items.Where((mainLog arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(_item);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(mainLog item)
        {
            await InitializeAsync();

            var _item = items.Where((mainLog arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(_item);
            //items.RemoveAt(0);

            return await Task.FromResult(true);
        }

        public async Task<mainLog> GetItemAsync(string id)
        {
            await InitializeAsync();

            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<mainLog>> GetItemsAsync(bool forceRefresh = false)
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

            items = new List<mainLog>();
            var _items = new List<mainLog>
            {
                new mainLog { Id = Guid.NewGuid().ToString(), Message = "[Talk] Hello... am i a bot ?", Time = DateTime.Now },                
            };

            foreach (mainLog item in _items)
            {
                items.Add(item);
            }

            isInitialized = true;
        }

        public Task<bool> DeleteItemAsync()
        {
            throw new NotImplementedException();
        }
    }

}
