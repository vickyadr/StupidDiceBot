using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Dice_Bot.Models;

using Xamarin.Forms;

[assembly: Dependency(typeof(Dice_Bot.Services.MockDataStore))]
namespace Dice_Bot.Services
{
    public class MockDataStore : IDataStore<LogsItem>
    {
        bool isInitialized;
        List<LogsItem> items;

        public async Task<bool> AddItemAsync(LogsItem item)
        {
            await InitializeAsync();

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(LogsItem item)
        {
            await InitializeAsync();

            var _item = items.Where((LogsItem arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(_item);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(LogsItem item)
        {
            await InitializeAsync();

            var _item = items.Where((LogsItem arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(_item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync()
        {
            await InitializeAsync();

            items.Clear();

            return await Task.FromResult(true);
        }

        public async Task<LogsItem> GetItemAsync(string id)
        {
            await InitializeAsync();

            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<LogsItem>> GetItemsAsync(bool forceRefresh = false)
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

            items = new List<LogsItem>();
            var _items = new List<LogsItem>();
            //{
            //    new LogsItem { Id = Guid.NewGuid().ToString(), Amount = 0.000012m, IsHigh = false, BetId = "66433019921", Profit = 0.0000012m, Chance = 49.00m, IsWin = true, ClientSeed="18309798361",
            //                    Roll = 32m, Time= DateTime.Now.ToLongTimeString(), ServerHash="3138937041", ServerSeed = "1111123313", CurrentBalance= 0.00001m
            //    },
            //    new LogsItem { Id = Guid.NewGuid().ToString(), Amount = 0.00001m, IsHigh = true, BetId = "6643301244", Profit = -0.000001m, Chance = 49.00m, IsWin = false, ClientSeed="183097313121",
            //                    Roll = 44m, Time= DateTime.Now.ToLongTimeString(), ServerHash="3138937041", ServerSeed = "11111232341", CurrentBalance= 0.000009m
            //    }
            //};

            foreach (LogsItem item in _items)
            {
                items.Add(item);
            }
            isInitialized = true;
        }
    }

}
