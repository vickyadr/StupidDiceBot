using Dice_Bot.Modules;
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
	public partial class Settings : ContentPage
	{
        SettingViewModel viewModel;
        string sname { get; set; }
		public Settings ()
		{
			InitializeComponent ();
            BindingContext = viewModel = new SettingViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        private void piker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (piker.SelectedIndex > -1 && sname != null)
            {
                Models.setting seting = new Models.setting { Name = sname, Val = piker.Items[piker.SelectedIndex] };
                _.core.SetSetting(seting);
                sname = string.Empty;
            }
        }

        private void ItemsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ListView lv = this.FindByName<ListView>("ItemsListView");
            lv.SelectedItem = -1;
            piker.Items.Clear();
            var item = e.Item as Models.setting;
            sname = piker.Title = item.Name;
            foreach (object i in item.piker)
                piker.Items.Add(i.ToString());
            piker.Focus();
        }
    }
}
