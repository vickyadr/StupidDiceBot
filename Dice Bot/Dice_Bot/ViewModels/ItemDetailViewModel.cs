using Dice_Bot.Models;

namespace Dice_Bot.ViewModels
{
	public class ItemDetailViewModel : BaseViewModel
	{
		public LogsItem Item { get; set; }
		public ItemDetailViewModel(LogsItem item = null)
		{
			Title = "Details Bet "+item.BetId;
			Item = item;
		}

		int quantity = 1;
		public int Quantity
		{
			get { return quantity; }
			set { SetProperty(ref quantity, value); }
		}
	}
}