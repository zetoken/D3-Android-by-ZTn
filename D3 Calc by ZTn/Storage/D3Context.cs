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
using ZTn.BNet.D3.Items;
using ZTn.BNet.D3.Medias;

namespace ZTnDroid.D3Calculator.Storage
{
    public class D3Context
    {
        public String battleTag;
        public String host;
        public Boolean onlineMode;

        public HeroSummary heroSummary;
        public Hero hero;

        public HeroItems heroItems;

        public AccountsDB dbAccounts;

        static D3Context instance;

        public D3Picture activeSkill1Icon;
        public D3Picture activeSkill2Icon;
        public D3Picture activeSkill3Icon;
        public D3Picture activeSkill4Icon;
        public D3Picture passiveSkill1Icon;
        public D3Picture passiveSkill2Icon;
        public D3Picture passiveSkill3Icon;

        public D3Picture headIcon;
        public D3Picture torsoIcon;
        public D3Picture feetIcon;
        public D3Picture handsIcon;
        public D3Picture shouldersIcon;
        public D3Picture legsIcon;
        public D3Picture bracersIcon;
        public D3Picture mainHandIcon;
        public D3Picture offHandIcon;
        public D3Picture waistIcon;
        public D3Picture rightFingerIcon;
        public D3Picture leftFingerIcon;
        public D3Picture neckIcon;

        public static D3Context getInstance()
        {
            if (instance == null)
                instance = new D3Context();
            return instance;
        }
    }
}