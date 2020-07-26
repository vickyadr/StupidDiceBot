using Dice_Bot.Helpers;
using Dice_Bot.Models;
using Dice_Bot.Services;

using Xamarin.Forms;

namespace Dice_Bot.ViewModels
{
	public class BaseViewModel : ObservableObject
	{
		/// <summary>
		/// Get the azure service instance
		/// </summary>
		public IDataStore<LogsItem> DataStore => DependencyService.Get<IDataStore<LogsItem>>();

        public IDataStore<mainLog> MainStore => DependencyService.Get<IDataStore<mainLog>>();

        public IDataStore<setting> SettingStore => DependencyService.Get<IDataStore<setting>>();

        bool isBusy = false;
		public bool IsBusy
		{
			get { return isBusy; }
			set { SetProperty(ref isBusy, value); }
		}

        bool isSettingBusy = false;
        public bool IsSettingBusy
        {
            get { return isSettingBusy; }
            set { SetProperty(ref isSettingBusy, value); }
        }

        bool isMainBusy = false;
        public bool IsMainBusy
        {
            get { return isMainBusy; }
            set { SetProperty(ref isMainBusy, value); }
        }
        /// <summary>
        /// Private backing field to hold the title
        /// </summary>
        string title = string.Empty;
		/// <summary>
		/// Public property to set and get the title of the item
		/// </summary>
		public string Title
		{
			get { return title; }
			set { SetProperty(ref title, value); }
		}
	}
}

