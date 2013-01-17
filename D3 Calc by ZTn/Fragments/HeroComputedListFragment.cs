using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ZTn.BNet.BattleNet;
using ZTn.BNet.D3;
using ZTn.BNet.D3.Heroes;
using ZTn.BNet.D3.Items;
using ZTnDroid.D3Calculator.Adapters;
using ZTnDroid.D3Calculator.Storage;
using ZTn.BNet.D3.Calculator;
using System.Reflection;
using ZTn.BNet.D3.Calculator.Sets;

using Fragment = Android.Support.V4.App.Fragment;
using ZTnDroid.D3Calculator.Helpers;

namespace ZTnDroid.D3Calculator.Fragments
{
    public class HeroComputedListFragment : ZTnFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            base.OnCreate(savedInstanceState);

            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            View view = inflater.Inflate(Resource.Layout.ViewHero, container, false);

            updateView(view);

            return view;
        }

        private ZTn.BNet.D3.Calculator.D3Calculator getCalculator()
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            Hero hero = D3Context.getInstance().hero;
            HeroItems heroItems = D3Context.getInstance().heroItems;

            // Retrieve weared items from the GUI
            List<Item> items = new List<Item>();
            if (heroItems.bracers != null)
                items.Add((Item)heroItems.bracers);
            if (heroItems.feet != null)
                items.Add((Item)heroItems.feet);
            if (heroItems.hands != null)
                items.Add((Item)heroItems.hands);
            if (heroItems.head != null)
                items.Add((Item)heroItems.head);
            if (heroItems.leftFinger != null)
                items.Add((Item)heroItems.leftFinger);
            if (heroItems.legs != null)
                items.Add((Item)heroItems.legs);
            if (heroItems.neck != null)
                items.Add((Item)heroItems.neck);
            if (heroItems.rightFinger != null)
                items.Add((Item)heroItems.rightFinger);
            if (heroItems.shoulders != null)
                items.Add((Item)heroItems.shoulders);
            if (heroItems.torso != null)
                items.Add((Item)heroItems.torso);
            if (heroItems.waist != null)
                items.Add((Item)heroItems.waist);

            if (D3Context.getInstance().setBonus != null)
                items.Add(D3Context.getInstance().setBonus);

            if (heroItems.mainHand == null)
                heroItems.mainHand = new Item(new ItemAttributes());
            if (heroItems.offHand == null)
                heroItems.offHand = new Item(new ItemAttributes());

            ZTn.BNet.D3.Calculator.D3Calculator d3Calculator = new ZTn.BNet.D3.Calculator.D3Calculator(hero, (Item)heroItems.mainHand, (Item)heroItems.offHand, items.ToArray());

            return d3Calculator;
        }

        private void updateView(View view)
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            Hero hero = D3Context.getInstance().hero;
            HeroItems heroItems = D3Context.getInstance().heroItems;
            if (hero != null && heroItems != null)
            {
                ZTn.BNet.D3.Calculator.D3Calculator d3Calculator = getCalculator();
                double dps = d3Calculator.getHeroDPS(new List<ZTn.BNet.D3.Calculator.Skills.D3SkillModifier>(), new List<ZTn.BNet.D3.Calculator.Skills.D3SkillModifier>());

                ItemAttributes attr = d3Calculator.heroItemStats.attributesRaw;

                ListView heroStatsListView = view.FindViewById<ListView>(Resource.Id.heroStatsListView);
                List<IListItem> characteristicsAttr = new List<IListItem>()
                {
                    new SectionHeaderListItem(Resources.GetString(Resource.String.Progress)),
                    new AttributeListItem(Resources.GetString(Resource.String.heroClass), hero.heroClass, Activity),
                    new AttributeListItem(Resources.GetString(Resource.String.level), hero.level),
                    new AttributeListItem(Resources.GetString(Resource.String.paragon), hero.paragonLevel),

                    new SectionHeaderListItem(Resources.GetString(Resource.String.attributes)),
                    new AttributeListItem(Resources.GetString(Resource.String.dexterity), d3Calculator.getHeroDexterity()),
                    new AttributeListItem(Resources.GetString(Resource.String.intelligence), d3Calculator.getHeroIntelligence()),
                    new AttributeListItem(Resources.GetString(Resource.String.strength), d3Calculator.getHeroStrength()),
                    new AttributeListItem(Resources.GetString(Resource.String.vitality), d3Calculator.getHeroVitality()),

                    new SectionHeaderListItem(Resources.GetString(Resource.String.damages)),
                    new AttributeListItem(Resources.GetString(Resource.String.damage), dps),
                };

                if (attr.critPercentBonusCapped != null)
                    characteristicsAttr.Add(new AttributePercentListItem(Resources.GetString(Resource.String.criticChance), attr.critPercentBonusCapped));
                if (attr.critDamagePercent != null)
                    characteristicsAttr.Add(new AttributePercentListItem(Resources.GetString(Resource.String.criticDamage), attr.critDamagePercent));

                characteristicsAttr.AddRange(new List<IListItem>()
                {
                    new AttributePercentListItem(Resources.GetString(Resource.String.attackSpeed) , d3Calculator.getActualAttackSpeed()),

                    new SectionHeaderListItem(Resources.GetString(Resource.String.life)),
                    new AttributeListItem(Resources.GetString(Resource.String.life), d3Calculator.getHeroHitpoints())
                });

                if (attr.hitpointsOnHit != null)
                    characteristicsAttr.Add(new AttributeListItem(Resources.GetString(Resource.String.lifeOnHit), attr.hitpointsOnHit));
                if (attr.stealHealthPercent != null)
                    characteristicsAttr.Add(new AttributePercentListItem(Resources.GetString(Resource.String.lifeSteal), attr.stealHealthPercent));
                if (attr.hitpointsOnKill != null)
                    characteristicsAttr.Add(new AttributeListItem(Resources.GetString(Resource.String.lifePerKill), attr.hitpointsOnKill));
                if (attr.healthGlobeBonusHealth != null)
                    characteristicsAttr.Add(new AttributeListItem(Resources.GetString(Resource.String.lifeBonusPerGlobe), attr.healthGlobeBonusHealth));
                if (attr.hitpointsRegenPerSecond != null)
                    characteristicsAttr.Add(new AttributeListItem(Resources.GetString(Resource.String.lifeRegenPerSecond), attr.hitpointsRegenPerSecond));

                characteristicsAttr.AddRange(new List<IListItem>()
                {
                    new AttributeListItem(Resources.GetString(Resource.String.effectiveHitpoints), d3Calculator.getHeroEffectiveHitpoints(hero.level+3)),
                    new AttributeListItem(Resources.GetString(Resource.String.EHP_DPS), d3Calculator.getHeroEffectiveHitpoints(hero.level+3) * d3Calculator.getHeroDPS() / 1000000),
                    
                    new SectionHeaderListItem(Resources.GetString(Resource.String.defense)),
                    new AttributeListItem(Resources.GetString(Resource.String.dodge), d3Calculator.getHeroDodge()),
                    new AttributeListItem(Resources.GetString(Resource.String.armor), d3Calculator.getHeroArmor()),
                    new AttributeListItem(Resources.GetString(Resource.String.arcaneResist), d3Calculator.getHeroResistance("Arcane")),
                    new AttributeListItem(Resources.GetString(Resource.String.coldResist), d3Calculator.getHeroResistance("Cold")),
                    new AttributeListItem(Resources.GetString(Resource.String.fireResist), d3Calculator.getHeroResistance("Fire")),
                    new AttributeListItem(Resources.GetString(Resource.String.lightningResist), d3Calculator.getHeroResistance("Lightning")),
                    new AttributeListItem(Resources.GetString(Resource.String.physicalResist), d3Calculator.getHeroResistance("Physical")),
                    new AttributeListItem(Resources.GetString(Resource.String.poisonResist), d3Calculator.getHeroResistance("Poison")),

                    new SectionHeaderListItem(Resources.GetString(Resource.String.bonuses))
                });

                if (attr.goldFind != null)
                    characteristicsAttr.Add(new AttributePercentListItem(Resources.GetString(Resource.String.goldFind), attr.goldFind));
                if (attr.magicFind != null)
                    characteristicsAttr.Add(new AttributePercentListItem(Resources.GetString(Resource.String.magicFind), attr.magicFind));

                heroStatsListView.Adapter = new SectionedListAdapter(Activity, characteristicsAttr.ToArray());
            }
        }
    }
}