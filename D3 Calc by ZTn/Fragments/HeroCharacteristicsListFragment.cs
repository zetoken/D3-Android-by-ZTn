using System;
using System.Collections.Generic;
using System.Reflection;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using ZTn.BNet.D3.Heroes;
using ZTnDroid.D3Calculator.Adapters;
using ZTnDroid.D3Calculator.Helpers;
using ZTnDroid.D3Calculator.Storage;

using Fragment = Android.Support.V4.App.Fragment;

namespace ZTnDroid.D3Calculator.Fragments
{
    public class HeroCharacteristicsListFragment : ZTnFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            Console.WriteLine("HeroCharacteristicsListFragment: OnCreate");
            base.OnCreate(savedInstanceState);

            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Console.WriteLine("HeroCharacteristicsListFragment: OnCreateView");
            View view = inflater.Inflate(Resource.Layout.ViewHero, container, false);

            updateView(view);

            return view;
        }

        private void updateView(View view)
        {
            Console.WriteLine("HeroCharacteristicsListFragment: updateHeroView");

            Hero hero = D3Context.getInstance().hero;
            if (hero != null)
            {
                ListView heroStatsListView = view.FindViewById<ListView>(Resource.Id.heroStatsListView);
                List<IListItem> characteristicsAttr = new List<IListItem>()
                {
                    new SectionHeaderListItem(Resources.GetString(Resource.String.Progress)),
                    new AttributeListItem(Resources.GetString(Resource.String.lastUpdated), hero.lastUpdated),
                    new AttributeListItem(Resources.GetString(Resource.String.heroClass), hero.heroClass, Activity),
                    new AttributeListItem(Resources.GetString(Resource.String.level), hero.level),
                    new AttributeListItem(Resources.GetString(Resource.String.paragon), hero.paragonLevel),

                    new SectionHeaderListItem(Resources.GetString(Resource.String.KillsLifetime)),
                    new AttributeListItem(Resources.GetString(Resource.String.elites), hero.kills.elites),

                    new SectionHeaderListItem(Resources.GetString(Resource.String.attributes)),
                    new AttributeListItem(Resources.GetString(Resource.String.dexterity), hero.stats.dexterity),
                    new AttributeListItem(Resources.GetString(Resource.String.intelligence), hero.stats.intelligence),
                    new AttributeListItem(Resources.GetString(Resource.String.strength), hero.stats.strength),
                    new AttributeListItem(Resources.GetString(Resource.String.vitality), hero.stats.vitality),

                    new SectionHeaderListItem(Resources.GetString(Resource.String.damages)),
                    new AttributeListItem(Resources.GetString(Resource.String.damage), hero.stats.damage),
                    new AttributePercentListItem(Resources.GetString(Resource.String.criticChance), hero.stats.critChance),
                    new AttributePercentListItem(Resources.GetString(Resource.String.criticDamage), hero.stats.critDamage),
                    new AttributePercentListItem(Resources.GetString(Resource.String.attackSpeed) , hero.stats.attackSpeed),

                    new SectionHeaderListItem(Resources.GetString(Resource.String.life)),
                    new AttributeListItem(Resources.GetString(Resource.String.life), hero.stats.life),
                    new AttributeListItem(Resources.GetString(Resource.String.lifeOnHit), hero.stats.lifeOnHit),
                    new AttributePercentListItem(Resources.GetString(Resource.String.lifeSteal), hero.stats.lifeSteal),
                    new AttributeListItem(Resources.GetString(Resource.String.lifePerKill), hero.stats.lifePerKill),

                    new SectionHeaderListItem(Resources.GetString(Resource.String.defense)),
                    new AttributeListItem(Resources.GetString(Resource.String.armor), hero.stats.armor),
                    new AttributeListItem(Resources.GetString(Resource.String.arcaneResist), hero.stats.arcaneResist),
                    new AttributeListItem(Resources.GetString(Resource.String.coldResist), hero.stats.coldResist),
                    new AttributeListItem(Resources.GetString(Resource.String.fireResist), hero.stats.fireResist),
                    new AttributeListItem(Resources.GetString(Resource.String.lightningResist), hero.stats.lightningResist),
                    new AttributeListItem(Resources.GetString(Resource.String.physicalResist), hero.stats.physicalResist),
                    new AttributeListItem(Resources.GetString(Resource.String.poisonResist), hero.stats.poisonResist),

                    new SectionHeaderListItem(Resources.GetString(Resource.String.resources)),
                    new AttributeListItem(Resources.GetString(Resource.String.primaryResource), hero.stats.primaryResource),
                    new AttributeListItem(Resources.GetString(Resource.String.secondaryResource),hero.stats.secondaryResource),

                    new SectionHeaderListItem(Resources.GetString(Resource.String.bonuses)),
                    new AttributePercentListItem(Resources.GetString(Resource.String.goldFind), hero.stats.goldFind),
                    new AttributePercentListItem(Resources.GetString(Resource.String.magicFind), hero.stats.magicFind)
                };
                heroStatsListView.Adapter = new SectionedListAdapter(Activity, characteristicsAttr.ToArray());
            }
        }
    }
}