using Android.OS;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using System.Reflection;
using ZTnDroid.D3Calculator.Adapters;
using ZTnDroid.D3Calculator.Adapters.Delegated;
using ZTnDroid.D3Calculator.Helpers;
using ZTnDroid.D3Calculator.Storage;

namespace ZTnDroid.D3Calculator.Fragments
{
    public class HeroCharacteristicsListFragment : UpdatableFragment
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

        private void UpdateView(View view)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            var hero = D3Context.Instance.CurrentHero;

            if (hero == null)
            {
                return;
            }

            var heroStatsListView = view.FindViewById<ListView>(Resource.Id.heroStatsListView);
            var characteristicsAttr = new List<IListItem>
            {
                new SectionHeaderListItem(Resource.String.Progress),
                new AttributeListItem(Resource.String.lastUpdated, hero.LastUpdated),
                new AttributeListItem(Resource.String.heroClass, hero.HeroClass.Translate().CapitalizeFirstLetter()),
                new AttributeListItem(Resource.String.level, hero.Level),
                new AttributeListItem(Resource.String.paragon, hero.ParagonLevel),

                new SectionHeaderListItem(Resource.String.KillsLifetime),
                new AttributeListItem(Resource.String.elites, hero.Kills.Elites),

                new SectionHeaderListItem(Resource.String.attributes),
                new AttributeListItem(Resource.String.dexterity, hero.Stats.Dexterity),
                new AttributeListItem(Resource.String.intelligence, hero.Stats.Intelligence),
                new AttributeListItem(Resource.String.strength, hero.Stats.Strength),
                new AttributeListItem(Resource.String.vitality, hero.Stats.Vitality),
                new AttributeListItem(Resource.String.healing, hero.Stats.Healing),
                new AttributeListItem(Resource.String.toughness, hero.Stats.Toughness),

                new SectionHeaderListItem(Resource.String.damages),
                new AttributeListItem(Resource.String.damage, hero.Stats.Damage),
                new AttributePercentListItem(Resource.String.criticChance, hero.Stats.CritChance),
                new AttributePercentListItem(Resource.String.criticDamage, hero.Stats.CritDamage),
                new AttributePercentListItem(Resource.String.attackSpeed , hero.Stats.AttackSpeed),

                new SectionHeaderListItem(Resource.String.life),
                new AttributeListItem(Resource.String.life, hero.Stats.Life),
                new AttributeListItem(Resource.String.lifeOnHit, hero.Stats.LifeOnHit),
                new AttributePercentListItem(Resource.String.lifeSteal, hero.Stats.LifeSteal),
                new AttributeListItem(Resource.String.lifePerKill, hero.Stats.LifePerKill),

                new SectionHeaderListItem(Resource.String.defense),
                new AttributeListItem(Resource.String.armor, hero.Stats.Armor),
                new AttributeListItem(Resource.String.arcaneResist, hero.Stats.ArcaneResist),
                new AttributeListItem(Resource.String.coldResist, hero.Stats.ColdResist),
                new AttributeListItem(Resource.String.fireResist, hero.Stats.FireResist),
                new AttributeListItem(Resource.String.lightningResist, hero.Stats.LightningResist),
                new AttributeListItem(Resource.String.physicalResist, hero.Stats.PhysicalResist),
                new AttributeListItem(Resource.String.poisonResist, hero.Stats.PoisonResist),

                new SectionHeaderListItem(Resource.String.resources),
                new AttributeListItem(Resource.String.primaryResource, hero.Stats.PrimaryResource),
                new AttributeListItem(Resource.String.secondaryResource,hero.Stats.SecondaryResource),

                new SectionHeaderListItem(Resource.String.bonuses),
                new AttributePercentListItem(Resource.String.goldFind, hero.Stats.GoldFind),
                new AttributePercentListItem(Resource.String.magicFind, hero.Stats.MagicFind)
            };
            heroStatsListView.Adapter = new ListAdapter(Activity, characteristicsAttr.ToArray());
        }
    }
}