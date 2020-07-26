using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Dice_Bot.ViewModels
{
	public class AboutViewModel : BaseViewModel
	{
		public AboutViewModel()
		{
			Title = "About Me";

			OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://fb.com/Stupid-Dice-Bot-204719566750191/?ref=page_internal")));
		}

		/// <summary>
		/// Command to open browser to xamarin.com
		/// </summary>
		public ICommand OpenWebCommand { get; }
	}
}
