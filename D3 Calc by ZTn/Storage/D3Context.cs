using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ZTn.BNet.BattleNet;
using ZTn.BNet.D3.Heroes;

namespace ZTnDroid.D3Calculator.Storage
{
    public class D3Context
    {
        public String battleTag;
        public String host;
        public Boolean onlineMode;

        public HeroSummary heroSummary;
        public Hero hero;

        public AccountsDB dbAccounts;

        static D3Context instance;

        public static D3Context getInstance()
        {
            if (instance == null)
                instance = new D3Context();
            return instance;
        }
    }
}