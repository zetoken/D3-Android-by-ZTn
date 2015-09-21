using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZTn.BNet.BattleNet;
using ZTn.BNet.D3.Helpers;
using ZTn.Pcl.D3Calculator.Models;

namespace ZTn.Pcl.D3Calculator.Views
{
    public partial class AddBnetAccountPage : ContentPage
    {
        public BindableTask<ObservableCollection<string>> HostItems { get; set; }

        public int HostSelectedIndex { get; set; }

        public string BattleTagString { get; set; }

        static Host[] _hosts;

        public AddBnetAccountPage()
        {
            HostItems = new BindableTask<ObservableCollection<string>>(GetHostsItems());
            HostSelectedIndex = 0;

            InitializeComponent();

            BindingContext = this;
        }

        private Task<ObservableCollection<string>> GetHostsItems()
        {
            return Task.Run(() =>
            {
                if (_hosts == null)
                {
                    using (var stream = GetType().GetTypeInfo().Assembly.GetManifestResourceStream("ZTn.Pcl.D3Calculator.Resources.hosts.json"))
                    {
                        _hosts = stream.CreateFromJsonPersistentStream<Host[]>();
                    }
                }

                var hostItems = new ObservableCollection<string>();
                foreach (var hostItem in _hosts.Select(h => $"{h.Name} ({h.Url})"))
                {
                    hostItems.Add(hostItem);
                }

                return hostItems;
            });
        }

        private void OnSubmit(object sender, EventArgs e)
        {
            var account = new BnetAccount { BattleTag = BattleTagString, Host = _hosts[HostSelectedIndex].Url };

            BnetAccountsPage.Items.Add(account);
            var accounts = new BnetAccounts();
            accounts.InsertAccount(account);

            Navigation.PopAsync();
        }
    }
}
