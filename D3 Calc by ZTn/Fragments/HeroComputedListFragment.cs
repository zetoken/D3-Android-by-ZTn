using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Android.OS;
using Android.Views;
using Android.Widget;
using ZTn.BNet.D3.Calculator.Skills;
using ZTn.BNet.D3.Helpers;
using ZTn.BNet.D3.Items;
using ZTnDroid.D3Calculator.Adapters;
using ZTnDroid.D3Calculator.Adapters.Delegated;
using ZTnDroid.D3Calculator.Helpers;
using ZTnDroid.D3Calculator.Storage;

namespace ZTnDroid.D3Calculator.Fragments
{
    public class HeroComputedListFragment : UpdatableFragment
    {
        #region >> ZTnFragment

        /// <inheritdoc/>
        public override void OnCreate(Bundle savedInstanceState)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            base.OnCreate(savedInstanceState);

            RetainInstance = true;
        }

        /// <inheritdoc/>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            var view = inflater.Inflate(Resource.Layout.ViewHero, container, false);

            UpdateView(view);

            return view;
        }

        #endregion

        private static ZTn.BNet.D3.Calculator.D3Calculator GetCalculator()
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            var hero = D3Context.Instance.CurrentHero;
            var heroItems = D3Context.Instance.CurrentHeroItems;

            // Retrieve worn items
            var items = new List<Item>
            {
                (Item)heroItems.Bracers,
                (Item)heroItems.Feet,
                (Item)heroItems.Hands,
                (Item)heroItems.Head,
                (Item)heroItems.LeftFinger,
                (Item)heroItems.Legs,
                (Item)heroItems.Neck,
                (Item)heroItems.RightFinger,
                (Item)heroItems.Shoulders,
                (Item)heroItems.Torso,
                (Item)heroItems.Waist,
                D3Context.Instance.ActivatedSetBonus
            };
            items = items.Where(i => i != null)
                .Select(i => i.DeepClone())
                .ToList();

            if (heroItems.MainHand == null)
            {
                heroItems.MainHand = new Item(new ItemAttributes());
            }
            if (heroItems.OffHand == null)
            {
                heroItems.OffHand = new Item(new ItemAttributes());
            }

            var d3Calculator = new ZTn.BNet.D3.Calculator.D3Calculator(hero, ((Item)heroItems.MainHand).DeepClone(), ((Item)heroItems.OffHand).DeepClone(), items.ToArray());

            return d3Calculator;
        }

        private void UpdateView(View view)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            var hero = D3Context.Instance.CurrentHero;
            var heroItems = D3Context.Instance.CurrentHeroItems;

            if (hero == null || heroItems == null)
            {
                return;
            }

            var d3Calculator = GetCalculator();
            var dps = d3Calculator.GetHeroDps(new List<ID3SkillModifier>(), new List<ID3SkillModifier>())
                .Min;

            var attr = d3Calculator.HeroStatsItem.AttributesRaw;

            var heroStatsListView = view.FindViewById<ListView>(Resource.Id.heroStatsListView);
            var characteristicsAttr = new List<IListItem>
            {
                new SectionHeaderListItem(Resource.String.Progress),
                new AttributeListItem(Resource.String.heroClass, hero.HeroClass),
                new AttributeListItem(Resource.String.level, hero.Level)
            };

            AddAttributeIntegerListItem(characteristicsAttr, Resource.String.paragon, hero.ParagonLevel);

            characteristicsAttr.Add(new SectionHeaderListItem(Resource.String.attributes));

            AddAttributeIntegerListItem(characteristicsAttr, Resource.String.dexterity, d3Calculator.GetHeroDexterity());
            AddAttributeIntegerListItem(characteristicsAttr, Resource.String.intelligence, d3Calculator.GetHeroIntelligence());
            AddAttributeIntegerListItem(characteristicsAttr, Resource.String.strength, d3Calculator.GetHeroStrength());
            AddAttributeIntegerListItem(characteristicsAttr, Resource.String.vitality, d3Calculator.GetHeroVitality());

            characteristicsAttr.Add(new SectionHeaderListItem(Resource.String.damages));

            characteristicsAttr.Add(new AttributeListItem(Resource.String.damage, dps));
            AddAttributePercentListItem(characteristicsAttr, Resource.String.criticChance, attr.critPercentBonusCapped + 0.05);
            AddAttributePercentListItem(characteristicsAttr, Resource.String.criticDamage, attr.critDamagePercent + 1);

            AddAttributeListItem(characteristicsAttr, Resource.String.attackSpeed, d3Calculator.GetActualAttackSpeed());

            characteristicsAttr.Add(new SectionHeaderListItem(Resource.String.skills));

            AddAttributeElementaryDamageListItem(characteristicsAttr, Resource.String.damageDealtPercent_Arcane, attr.damageDealtPercentBonusArcane, dps);
            AddAttributeElementaryDamageListItem(characteristicsAttr, Resource.String.damageDealtPercent_Cold, attr.damageDealtPercentBonusCold, dps);
            AddAttributeElementaryDamageListItem(characteristicsAttr, Resource.String.damageDealtPercent_Fire, attr.damageDealtPercentBonusFire, dps);
            AddAttributeElementaryDamageListItem(characteristicsAttr, Resource.String.damageDealtPercent_Holy, attr.damageDealtPercentBonusHoly, dps);
            AddAttributeElementaryDamageListItem(characteristicsAttr, Resource.String.damageDealtPercent_Lightning, attr.damageDealtPercentBonusLightning, dps);
            AddAttributeElementaryDamageListItem(characteristicsAttr, Resource.String.damageDealtPercent_Physical, attr.damageDealtPercentBonusPhysical, dps);
            AddAttributeElementaryDamageListItem(characteristicsAttr, Resource.String.damageDealtPercent_Poison, attr.damageDealtPercentBonusPoison, dps);

            AddAttributeElementaryDamageListItem(characteristicsAttr, Resource.String.damagePercentBonusVsElites, attr.damagePercentBonusVsElites, dps);

            AddAttributeElementaryDamageListItem(characteristicsAttr, Resource.String.damageDealtPercentVsElites_Arcane, attr.damageDealtPercentBonusArcane, attr.damagePercentBonusVsElites, dps);
            AddAttributeElementaryDamageListItem(characteristicsAttr, Resource.String.damageDealtPercentVsElites_Cold, attr.damageDealtPercentBonusCold, attr.damagePercentBonusVsElites, dps);
            AddAttributeElementaryDamageListItem(characteristicsAttr, Resource.String.damageDealtPercentVsElites_Fire, attr.damageDealtPercentBonusFire, attr.damagePercentBonusVsElites, dps);
            AddAttributeElementaryDamageListItem(characteristicsAttr, Resource.String.damageDealtPercentVsElites_Holy, attr.damageDealtPercentBonusHoly, attr.damagePercentBonusVsElites, dps);
            AddAttributeElementaryDamageListItem(characteristicsAttr, Resource.String.damageDealtPercentVsElites_Lightning, attr.damageDealtPercentBonusLightning, attr.damagePercentBonusVsElites, dps);
            AddAttributeElementaryDamageListItem(characteristicsAttr, Resource.String.damageDealtPercentVsElites_Physical, attr.damageDealtPercentBonusPhysical, attr.damagePercentBonusVsElites, dps);
            AddAttributeElementaryDamageListItem(characteristicsAttr, Resource.String.damageDealtPercentVsElites_Poison, attr.damageDealtPercentBonusPoison, attr.damagePercentBonusVsElites, dps);

            AddAttributePercentListItem(characteristicsAttr, Resource.String.powerCooldownReductionPercentAll, attr.powerCooldownReductionPercentAll);

            characteristicsAttr.AddRange(new List<IListItem>
            {
                new SectionHeaderListItem(Resource.String.life),
                new AttributeListItem(Resource.String.life, d3Calculator.GetHeroHitpoints())
            });

            AddAttributeIntegerListItem(characteristicsAttr, Resource.String.lifeOnHit, attr.hitpointsOnHit);
            AddAttributeIntegerListItem(characteristicsAttr, Resource.String.lifeSteal, attr.stealHealthPercent);
            AddAttributeIntegerListItem(characteristicsAttr, Resource.String.lifePerKill, attr.hitpointsOnKill);
            AddAttributeIntegerListItem(characteristicsAttr, Resource.String.lifeBonusPerGlobe, attr.healthGlobeBonusHealth);
            AddAttributeIntegerListItem(characteristicsAttr, Resource.String.lifeRegenPerSecond, attr.hitpointsRegenPerSecond);
            AddAttributeIntegerListItem(characteristicsAttr, Resource.String.effectiveHitpoints, Math.Round(d3Calculator.GetHeroEffectiveHitpoints(hero.Level)));
            AddAttributeIntegerListItem(characteristicsAttr, Resource.String.EHP_DPS, Math.Round((d3Calculator.GetHeroEffectiveHitpoints(hero.Level) * d3Calculator.GetHeroDps()).Min / 1000000));

            characteristicsAttr.AddRange(new List<IListItem>
            {
                new SectionHeaderListItem(Resource.String.defense),
                new AttributeListItem(Resource.String.dodge, d3Calculator.GetHeroDodge()),
                new AttributeMitigationListItem(Resource.String.armor, d3Calculator.GetHeroArmor(), d3Calculator.GetHeroDamageReduction_Armor(hero.Level)),
                new AttributeMitigationListItem(Resource.String.arcaneResist, d3Calculator.GetHeroResistance("Arcane"), d3Calculator.GetHeroDamageReduction(hero.Level,"Arcane")),
                new AttributeMitigationListItem(Resource.String.coldResist, d3Calculator.GetHeroResistance("Cold"), d3Calculator.GetHeroDamageReduction(hero.Level,"Cold")),
                new AttributeMitigationListItem(Resource.String.fireResist, d3Calculator.GetHeroResistance("Fire"), d3Calculator.GetHeroDamageReduction(hero.Level,"Fire")),
                new AttributeMitigationListItem(Resource.String.lightningResist, d3Calculator.GetHeroResistance("Lightning"), d3Calculator.GetHeroDamageReduction(hero.Level,"Lightning")),
                new AttributeMitigationListItem(Resource.String.physicalResist, d3Calculator.GetHeroResistance("Physical"), d3Calculator.GetHeroDamageReduction(hero.Level,"Physical")),
                new AttributeMitigationListItem(Resource.String.poisonResist, d3Calculator.GetHeroResistance("Poison"), d3Calculator.GetHeroDamageReduction(hero.Level,"Poison")),
                new SectionHeaderListItem(Resource.String.bonuses)
            });

            AddAttributePercentListItem(characteristicsAttr, Resource.String.goldFind, attr.goldFind);
            AddAttributePercentListItem(characteristicsAttr, Resource.String.magicFind, attr.magicFind);

            heroStatsListView.Adapter = new ListAdapter(Activity, characteristicsAttr.ToArray());
        }

        private static void AddAttributePercentListItem(List<IListItem> listItems, int resource, ItemValueRange valueRange)
        {
            if (valueRange == null)
            {
                return;
            }

            listItems.Add(new AttributePercentListItem(resource, valueRange));
        }

        private static void AddAttributeElementaryDamageListItem(List<IListItem> listItems, int resource, ItemValueRange elementaryDamageIncrease, double dps)
        {
            if (elementaryDamageIncrease == null)
            {
                return;
            }

            listItems.Add(new AttributeElementaryDamageListItem(resource, elementaryDamageIncrease, dps));
        }

        private static void AddAttributeElementaryDamageListItem(List<IListItem> listItems, int resource, ItemValueRange elementaryDamageIncrease, ItemValueRange bonusVsElites, double dps)
        {
            if (elementaryDamageIncrease == null || bonusVsElites == null)
            {
                return;
            }

            listItems.Add(new AttributeElementaryDamageListItem(resource, (1 + elementaryDamageIncrease) * (1 + bonusVsElites) - 1, dps));
        }

        private static void AddAttributeIntegerListItem(List<IListItem> listItems, int resource, ItemValueRange valueRange)
        {
            if (valueRange == null)
            {
                return;
            }

            listItems.Add(new AttributeListItem(resource, (long)Math.Round(valueRange.Min)));
        }

        private static void AddAttributeIntegerListItem(List<IListItem> listItems, int resource, double value)
        {
            listItems.Add(new AttributeListItem(resource, (long)Math.Round(value)));
        }

        private static void AddAttributeListItem(List<IListItem> listItems, int resource, ItemValueRange valueRange)
        {
            if (valueRange == null)
            {
                return;
            }

            listItems.Add(new AttributeListItem(resource, valueRange));
        }
    }
}