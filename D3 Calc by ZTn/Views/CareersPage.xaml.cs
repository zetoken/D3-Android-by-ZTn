using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZTn.BNet.BattleNet;
using ZTn.BNet.D3;
using ZTn.BNet.D3.Careers;
using ZTn.BNet.D3.Heroes;
using ZTn.Pcl.D3Calculator.Models;
using ZTn.Pcl.D3Calculator.ViewModels;

namespace ZTn.Pcl.D3Calculator.Views
{
    public partial class CareersPage : ContentPage
    {
        private readonly BnetAccount _account;
        private CareersViewModel _viewModel;

        public CareersPage(BnetAccount account)
        {
            _account = account;

            _viewModel = new CareersViewModel(account);

            Title = account.BattleTag;

            InitializeComponent();

            BindingContext = _viewModel;
        }

        private void OnRefresh(object sender, EventArgs e)
        {
            _viewModel = new CareersViewModel(_account);

            BindingContext = _viewModel;
        }

        private void OnSelection(object sender, SelectedItemChangedEventArgs e)
        {
            // TODO
        }
    }
}
