using Dice_Bot.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dice_Bot.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HelpPage : ContentPage
	{
        HelpViewModel viewModel;
        public HelpPage ()
		{
			InitializeComponent ();

            BindingContext = viewModel = new HelpViewModel();
        }

        private void ItemsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ListView lv = this.FindByName<ListView>("ItemsListView");
            lv.SelectedItem = -1;
        }
    }
}
