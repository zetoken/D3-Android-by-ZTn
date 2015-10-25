using System;
using Xamarin.Forms;
using ZTn.BNet.D3.DataProviders;
using ZTn.BNet.D3.Heroes;
using ZTn.Pcl.D3Calculator.Models;
using ZTn.Pcl.D3Calculator.ViewModels;

namespace ZTn.Pcl.D3Calculator.Views
{
    public partial class HeroItemsPage : ContentPage
    {
        private readonly HeroViewModel _viewModel;

        public HeroItemsPage(BnetAccount bnetAccount, HeroSummary heroSummary)
        {
            _viewModel = new HeroViewModel(bnetAccount, heroSummary);

            Title = $"{heroSummary.Name} @ {bnetAccount.BattleTag}";

            InitializeComponent();

            BindingContext = _viewModel;
        }

        private void OnRefresh(object sender, EventArgs e)
        {
            _viewModel.RefreshHero(FetchMode.Online);
        }
    }
}
