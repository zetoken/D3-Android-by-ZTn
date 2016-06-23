using System;
using Xamarin.Forms;
using ZTn.BNet.D3.DataProviders;
using ZTn.BNet.D3.Heroes;
using ZTn.Pcl.D3Calculator.Models;
using ZTn.Pcl.D3Calculator.ViewModels;

namespace ZTn.Pcl.D3Calculator.Views
{
    public partial class HeroPage : TabbedPage
    {
        public HeroViewModel ViewModel { get; }

        public HeroPage(BnetAccount account, HeroSummary heroSummary)
        {
            ViewModel = new HeroViewModel(this, account, heroSummary);

            InitializeComponent();

            Title = $"{heroSummary.Name} @ {account.BattleTag}";

            BindingContext = ViewModel;
        }

        private void OnRefresh(object sender, EventArgs eventArgs)
        {
            ViewModel.RefreshHero(FetchMode.Online);
        }
    }
}
