using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Reflection;
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

        static List<IListItem> GetPartialListViewForActiveSkill(String headerText, ActiveSkill active, D3Picture icon)
        {
            var list = new List<IListItem>
            {
                new SectionHeaderListItem(headerText)
            };

            if (active.skill != null)
                list.Add(new SkillListItem(active.skill, icon));
            if (active.rune != null)
                list.Add(new RuneListItem(active.rune));

            return list;
        }

        static List<IListItem> GetPartialListViewForPassiveSkill(String headerText, PassiveSkill passive, D3Picture icon)
        {
            var list = new List<IListItem>
            {
                new SectionHeaderListItem(headerText)
            };

            if (passive.skill != null)
                list.Add(new SkillListItem(passive.skill, icon));

            return list;
        }

        private void UpdateView(View view)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            var hero = D3Context.Instance.hero;
            var icons = D3Context.Instance.Icons;
            if (hero != null)
            {
                var heroSkillsListView = view.FindViewById<ListView>(Resource.Id.heroSkillsListView);
                var skillsAttr = new List<IListItem>();

                skillsAttr.AddRange(GetPartialListViewForActiveSkill(Resources.GetString(Resource.String.skill) + " L", hero.skills.active[0], icons.ActiveSkill1));
                skillsAttr.AddRange(GetPartialListViewForActiveSkill(Resources.GetString(Resource.String.skill) + " R", hero.skills.active[1], icons.ActiveSkill2));
                skillsAttr.AddRange(GetPartialListViewForActiveSkill(Resources.GetString(Resource.String.skill) + " 1", hero.skills.active[2], icons.ActiveSkill3));
                skillsAttr.AddRange(GetPartialListViewForActiveSkill(Resources.GetString(Resource.String.skill) + " 2", hero.skills.active[3], icons.ActiveSkill4));
                skillsAttr.AddRange(GetPartialListViewForActiveSkill(Resources.GetString(Resource.String.skill) + " 3", hero.skills.active[4], icons.ActiveSkill5));
                skillsAttr.AddRange(GetPartialListViewForActiveSkill(Resources.GetString(Resource.String.skill) + " 4", hero.skills.active[5], icons.ActiveSkill6));

                skillsAttr.AddRange(GetPartialListViewForPassiveSkill(Resources.GetString(Resource.String.passiveSkill), hero.skills.passive[0], icons.PassiveSkill1));
                skillsAttr.AddRange(GetPartialListViewForPassiveSkill(Resources.GetString(Resource.String.passiveSkill), hero.skills.passive[1], icons.PassiveSkill2));
                skillsAttr.AddRange(GetPartialListViewForPassiveSkill(Resources.GetString(Resource.String.passiveSkill), hero.skills.passive[2], icons.PassiveSkill3));

                heroSkillsListView.Adapter = new ListAdapter(Activity, skillsAttr.ToArray());
            }
        }
    }
}