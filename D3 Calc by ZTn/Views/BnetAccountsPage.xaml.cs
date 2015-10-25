using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZTn.Pcl.D3Calculator.Models;

namespace ZTn.Pcl.D3Calculator.Views
{
    public partial class BnetAccountsPage : ContentPage
    {
        private static BnetAccounts _accounts;

        public static ObservableCollection<BnetAccount> Accounts { get; set; }

        public string CareersTitle => D3Calculator.Resources.Lang.Careers.ToUpper();

        public BnetAccountsPage()
        {
            _accounts = new BnetAccounts();
            Accounts = new ObservableCollection<BnetAccount>(_accounts.GetAllAccounts());

            InitializeComponent();

            BindingContext = this;
        }

        #region >> Page event handlers

        private void OnSelection(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }

            var account = (BnetAccount)e.SelectedItem;

            Navigation.PushAsync(new CareersPage(account));

            var listView = (ListView)sender;
            listView.SelectedItem = null;
        }

        private void OnRefresh(object sender, EventArgs e)
        {
            var listView = (ListView)sender;

            var itemList = Accounts.Reverse().ToList();
            Accounts.Clear();
            foreach (var item in itemList)
            {
                Accounts.Add(item);
            }

            listView.IsRefreshing = false;
        }

        private async void OnEdit(object sender, EventArgs e)
        {
            var item = (MenuItem)sender;
            var account = (BnetAccount)item.CommandParameter;

            // Workaround for a Xamarin 1.5+ issue (latest stable ok: 1.4.4)
            await Task.Yield();

            Navigation.PushAsync(new BnetAccountEditorPage(account));
        }

        private void OnDelete(object sender, EventArgs e)
        {
            var item = (MenuItem)sender;
            var account = (BnetAccount)item.CommandParameter;

            _accounts.DeleteAccount(account);

            Accounts.Remove(account);
        }

        private void OnAddAccount(object sender, EventArgs e)
        {
            Navigation.PushAsync(new BnetAccountEditorPage());
        }

        #endregion
    }
}
