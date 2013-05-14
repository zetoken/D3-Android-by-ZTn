using System;
using System.Collections.Generic;
using ZTn.BNet.D3.DataProviders;
using ZTn.BNet.D3.Heroes;
using ZTn.BNet.D3.Items;

namespace ZTnDroid.D3Calculator.Storage
{
    public class D3Context
    {
        public String battleTag;
        public String host;
        public OnlineMode onlineMode;

        public HeroSummary heroSummary;
        public Hero hero;

        public HeroItems heroItems;

        public Item activatedSetBonus;
        public List<Set> activatedSets;

        public IconsContainer icons;

        public AccountsDB dbAccounts;

        static D3Context _instance;

        public static D3Context instance
        {
            get
            {
                if (_instance == null)
                    _instance = new D3Context();
                return _instance;
            }
        }
    }
}