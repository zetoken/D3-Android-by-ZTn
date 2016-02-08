﻿using System;
using Xamarin.Forms;
using ZTn.BNet.D3.DataProviders;
using ZTn.Pcl.D3Calculator.Models;
using ZTn.Pcl.D3Calculator.ViewModels;

namespace ZTn.Pcl.D3Calculator.Views
{
    public partial class CareerPage : ContentPage
    {
        private readonly CareersViewModel _viewModel;

        public CareerPage(BnetAccount account)
        {
            _viewModel = new CareersViewModel(account);

            Title = account.BattleTag;

            InitializeComponent();

            BindingContext = _viewModel;
        }

        private void OnRefresh(object sender, EventArgs e)
        {
            _viewModel.RefreshCareer(FetchMode.Online);
        }

        private void OnSelection(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }

            var heroListViewData = e.SelectedItem as HeroData;
            if (heroListViewData != null)
            {
                Navigation.PushAsync(new HeroPage(_viewModel.Account, heroListViewData.Hero));
            }

            var listView = (ListView)sender;
            listView.SelectedItem = null;
        }
    }
}
