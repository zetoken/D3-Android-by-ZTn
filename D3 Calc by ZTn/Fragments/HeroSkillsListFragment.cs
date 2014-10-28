using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Android.OS;
using Android.Views;
using Android.Widget;
using ZTn.BNet.D3.Medias;
using ZTn.BNet.D3.Skills;
using ZTnDroid.D3Calculator.Adapters;
using ZTnDroid.D3Calculator.Adapters.Delegated;
using ZTnDroid.D3Calculator.Helpers;
using ZTnDroid.D3Calculator.Storage;

namespace ZTnDroid.D3Calculator.Fragments
{
    public class HeroSkillsListFragment : UpdatableFragment
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

            var view = inflater.Inflate(Resource.Layout.ViewHeroSkills, container, false);

            UpdateView(view);

            return view;
        }

        #endregion

        private static List<IListItem> GetPartialListViewForActiveSkill(String headerText, ActiveSkill active, D3Picture icon)
        {
            var list = new List<IListItem>
            {
                new SectionHeaderListItem(headerText)
            };

            if (active.Skill != null)
            {
                list.Add(new SkillListItem(active.Skill, icon));
            }
            if (active.Rune != null)
            {
                list.Add(new RuneListItem(active.Rune));
            }

            return list;
        }

        private static List<IListItem> GetPartialListViewForPassiveSkill(String headerText, PassiveSkill passive, D3Picture icon)
        {
            var list = new List<IListItem>
            {
                new SectionHeaderListItem(headerText)
            };

            if (passive.Skill != null)
            {
                list.Add(new SkillListItem(passive.Skill, icon));
            }

            return list;
        }

        private void UpdateView(View view)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            var hero = D3Context.Instance.CurrentHero;

            if (hero == null)
            {
                return;
            }

            var icons = D3Context.Instance.Icons;
            var heroSkillsListView = view.FindViewById<ListView>(Resource.Id.heroSkillsListView);
            var skillsAttr = new List<IListItem>();

            skillsAttr.AddRange(GetPartialListViewForActiveSkill(Resources.GetString(Resource.String.skill) + " L", hero.Skills.Active[0], icons.ActiveSkill1));
            skillsAttr.AddRange(GetPartialListViewForActiveSkill(Resources.GetString(Resource.String.skill) + " R", hero.Skills.Active[1], icons.ActiveSkill2));
            skillsAttr.AddRange(GetPartialListViewForActiveSkill(Resources.GetString(Resource.String.skill) + " 1", hero.Skills.Active[2], icons.ActiveSkill3));
            skillsAttr.AddRange(GetPartialListViewForActiveSkill(Resources.GetString(Resource.String.skill) + " 2", hero.Skills.Active[3], icons.ActiveSkill4));
            skillsAttr.AddRange(GetPartialListViewForActiveSkill(Resources.GetString(Resource.String.skill) + " 3", hero.Skills.Active[4], icons.ActiveSkill5));
            skillsAttr.AddRange(GetPartialListViewForActiveSkill(Resources.GetString(Resource.String.skill) + " 4", hero.Skills.Active[5], icons.ActiveSkill6));

            var passiveCount = hero.Skills.Passive.Count();
            if (passiveCount >= 1)
                skillsAttr.AddRange(GetPartialListViewForPassiveSkill(Resources.GetString(Resource.String.passiveSkill) + " 1", hero.Skills.Passive[0], icons.PassiveSkill1));
            if (passiveCount >= 2)
                skillsAttr.AddRange(GetPartialListViewForPassiveSkill(Resources.GetString(Resource.String.passiveSkill) + " 2", hero.Skills.Passive[1], icons.PassiveSkill2));
            if (passiveCount >= 3)
                skillsAttr.AddRange(GetPartialListViewForPassiveSkill(Resources.GetString(Resource.String.passiveSkill) + " 3", hero.Skills.Passive[2], icons.PassiveSkill3));
            if (passiveCount >= 4)
                skillsAttr.AddRange(GetPartialListViewForPassiveSkill(Resources.GetString(Resource.String.passiveSkill) + " 4", hero.Skills.Passive[3], icons.PassiveSkill4));

            heroSkillsListView.Adapter = new ListAdapter(Activity, skillsAttr.ToArray());
        }
    }
}