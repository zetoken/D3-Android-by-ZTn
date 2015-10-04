using System;
using System.Linq;
using Xamarin.Forms;
using ZTn.Pcl.D3Calculator.Models;
using ZTn.Pcl.D3Calculator.ViewModels;

namespace ZTn.Pcl.D3Calculator.Views
{
    public partial class BnetAccountEditorPage : ContentPage
    {
        private readonly BnetAccountEditorViewModel _viewModel;
        private BnetAccount _account;

        public BnetAccountEditorPage() : this(null)
        {
        }

        public BnetAccountEditorPage(BnetAccount account)
        {
            _account = account;

            InitializeComponent();

            _viewModel = new BnetAccountEditorViewModel();

            if (account == null)
            {
                _viewModel.HostSelectedIndex = 0;
            }
            else
            {
                _viewModel.BattleTagString = account.BattleTag;

                var hostInList = App.Hosts
                    .Select((h, i) => new { Host = h, Index = i })
                    .FirstOrDefault(v => v.Host.Url == account.Host);

                if (hostInList != null)
                {
                    _viewModel.HostSelectedIndex = hostInList.Index;
                }
            }

            BindingContext = _viewModel;
        }

        private void OnSubmit(object sender, EventArgs e)
        {
            var accounts = new BnetAccounts();

            if (_account == null)
            {
                _account = new BnetAccount { BattleTag = _viewModel.BattleTagString, Host = App.Hosts[_viewModel.HostSelectedIndex].Url };

                BnetAccountsPage.Accounts.Add(_account);

                accounts.InsertAccount(_account);
            }
            else
            {
                _account.BattleTag = _viewModel.BattleTagString;
                _account.Host = App.Hosts[_viewModel.HostSelectedIndex].Url;

                var accountInList = BnetAccountsPage.Accounts
                    .Select((a, i) => new { Account = a, Index = i })
                    .FirstOrDefault(v => v.Account.Id == _account.Id);

                if (accountInList != null)
                {
                    BnetAccountsPage.Accounts[accountInList.Index] = _account;
                }

                accounts.UpdateAccount(_account);
            }

            Navigation.PopAsync();
        }
    }
}
