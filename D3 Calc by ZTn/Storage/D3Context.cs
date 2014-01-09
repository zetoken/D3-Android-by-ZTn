using System;
using System.Collections.Generic;
using ZTn.BNet.D3.DataProviders;
using ZTn.BNet.D3.Heroes;
using ZTn.BNet.D3.Items;

namespace ZTnDroid.D3Calculator.Storage
{
    public class D3Context
    {
        public String BattleTag;
        public String Host;
        public OnlineMode FetchMode;

        public HeroSummary CurrentHeroSummary;
        public Hero CurrentHero;

        public HeroItems CurrentHeroItems;

        public Item ActivatedSetBonus;
        public List<Set> ActivatedSets;

        public IconsContainer Icons;

        public Item EditingItem;

        public AccountsDB DBAccounts;

        static D3Context instance;

        public static D3Context Instance
        {
            get
            {
                if (instance == null)
                    instance = new D3Context();
                return instance;
            }
        }
    }
}