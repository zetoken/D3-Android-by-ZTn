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
                new AttributeListItem(Resource.String.heroClass, hero.heroClass.Translate().CapitalizeFirstLetter()),
                new AttributeListItem(Resource.String.level, hero.Level),
                new AttributeListItem(Resource.String.paragon, hero.ParagonLevel),

                new SectionHeaderListItem(Resource.String.KillsLifetime),
                new AttributeListItem(Resource.String.elites, hero.Kills.elites),

                new SectionHeaderListItem(Resource.String.attributes),
                new AttributeListItem(Resource.String.dexterity, hero.Stats.dexterity),
                new AttributeListItem(Resource.String.intelligence, hero.Stats.intelligence),
                new AttributeListItem(Resource.String.strength, hero.Stats.strength),
                new AttributeListItem(Resource.String.vitality, hero.Stats.vitality),

                new SectionHeaderListItem(Resource.String.damages),
                new AttributeListItem(Resource.String.damage, hero.Stats.damage),
                new AttributePercentListItem(Resource.String.criticChance, hero.Stats.critChance),
                new AttributePercentListItem(Resource.String.criticDamage, hero.Stats.critDamage),
                new AttributePercentListItem(Resource.String.attackSpeed , hero.Stats.attackSpeed),

                new SectionHeaderListItem(Resource.String.life),
                new AttributeListItem(Resource.String.life, hero.Stats.life),
                new AttributeListItem(Resource.String.lifeOnHit, hero.Stats.lifeOnHit),
                new AttributePercentListItem(Resource.String.lifeSteal, hero.Stats.lifeSteal),
                new AttributeListItem(Resource.String.lifePerKill, hero.Stats.lifePerKill),

                new SectionHeaderListItem(Resource.String.defense),
                new AttributeListItem(Resource.String.armor, hero.Stats.armor),
                new AttributeListItem(Resource.String.arcaneResist, hero.Stats.arcaneResist),
                new AttributeListItem(Resource.String.coldResist, hero.Stats.coldResist),
                new AttributeListItem(Resource.String.fireResist, hero.Stats.fireResist),
                new AttributeListItem(Resource.String.lightningResist, hero.Stats.lightningResist),
                new AttributeListItem(Resource.String.physicalResist, hero.Stats.physicalResist),
                new AttributeListItem(Resource.String.poisonResist, hero.Stats.poisonResist),

                new SectionHeaderListItem(Resource.String.resources),
                new AttributeListItem(Resource.String.primaryResource, hero.Stats.primaryResource),
                new AttributeListItem(Resource.String.secondaryResource,hero.Stats.secondaryResource),

                new SectionHeaderListItem(Resource.String.bonuses),
                new AttributePercentListItem(Resource.String.goldFind, hero.Stats.goldFind),
                new AttributePercentListItem(Resource.String.magicFind, hero.Stats.magicFind)
            };
            heroStatsListView.Adapter = new ListAdapter(Activity, characteristicsAttr.ToArray());
        }
    }
}