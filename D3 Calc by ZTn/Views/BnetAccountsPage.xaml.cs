using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using ZTn.Pcl.D3Calculator.Models;

namespace ZTn.Pcl.D3Calculator.Views
{
    public partial class BnetAccountsPage : ContentPage
    {
        public static ObservableCollection<BnetAccount> Items { get; set; }
        private static BnetAccounts _accounts;

        public BnetAccountsPage()
        {
            _accounts = new BnetAccounts();
            Items = new ObservableCollection<BnetAccount>(_accounts.GetAllAccounts());

            InitializeComponent();
        }

        #region >> Page event handlers

        private void OnSelection(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }

            var account = (BnetAccount)e.SelectedItem;
            DisplayAlert("Item Selected", account.BattleTag, "Ok");

            var listView = (ListView)sender;
            listView.SelectedItem = null;
        }

        private void OnRefresh(object sender, EventArgs e)
        {
            var listView = (ListView)sender;

            var itemList = Items.Reverse().ToList();
            Items.Clear();
            foreach (var item in itemList)
            {
                Items.Add(item);
            }

            listView.IsRefreshing = false;
        }

        private void OnTap(object sender, ItemTappedEventArgs e)
        {
            var account = (BnetAccount)e.Item;
            DisplayAlert("Item Tapped", $"{account}", "Ok");
        }

        private void OnEdit(object sender, EventArgs e)
        {
            var item = (MenuItem)sender;
            DisplayAlert("More Context Action", item.CommandParameter + " more context action", "OK");
        }

        private void OnDelete(object sender, EventArgs e)
        {
            var item = (MenuItem)sender;
            var account = (BnetAccount)item.CommandParameter;

            _accounts.DeleteAccount(account);

            Items.Remove(account);
        }

        private void OnAddAccount(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddBnetAccountPage());
        }

        #endregion
    }
}
