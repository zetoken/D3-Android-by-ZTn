using System;
using System.Collections.Generic;
using System.Reflection;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using ZTn.BNet.D3.Heroes;
using ZTn.BNet.D3.Medias;
using ZTn.BNet.D3.Skills;
using ZTnDroid.D3Calculator.Adapters;
using ZTnDroid.D3Calculator.Helpers;
using ZTnDroid.D3Calculator.Storage;
using Fragment = Android.Support.V4.App.Fragment;

namespace ZTnDroid.D3Calculator.Fragments
{
    public class HeroSkillsListFragment : ZTnFragment
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

            View view = inflater.Inflate(Resource.Layout.ViewHeroSkills, container, false);

            updateView(view);

            return view;
        }

        List<IListItem> getPartialListViewForActiveSkill(String headerText, ActiveSkill active, D3Picture icon)
        {
            List<IListItem> list = new List<IListItem>();

            list.Add(new SectionHeaderListItem(headerText));

            if (active.skill != null)
                list.Add(new SkillListItem(active.skill, icon));
            if (active.rune != null)
                list.Add(new RuneListItem(active.rune));

            return list;
        }

        List<IListItem> getPartialListViewForPassiveSkill(String headerText, PassiveSkill passive, D3Picture icon)
        {
            List<IListItem> list = new List<IListItem>();

            list.Add(new SectionHeaderListItem(headerText));
            if (passive.skill != null)
                list.Add(new SkillListItem(passive.skill, icon));

            return list;
        }

        private void updateView(View view)
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            Hero hero = D3Context.instance.hero;
            IconsContainer icons = D3Context.instance.icons;
            if (hero != null)
            {
                ListView heroSkillsListView = view.FindViewById<ListView>(Resource.Id.heroSkillsListView);
                List<IListItem> skillsAttr = new List<IListItem>();

                skillsAttr.AddRange(getPartialListViewForActiveSkill(Resources.GetString(Resource.String.skill) + " L", hero.skills.active[0], icons.activeSkill1));
                skillsAttr.AddRange(getPartialListViewForActiveSkill(Resources.GetString(Resource.String.skill) + " R", hero.skills.active[1], icons.activeSkill2));
                skillsAttr.AddRange(getPartialListViewForActiveSkill(Resources.GetString(Resource.String.skill) + " 1", hero.skills.active[2], icons.activeSkill3));
                skillsAttr.AddRange(getPartialListViewForActiveSkill(Resources.GetString(Resource.String.skill) + " 2", hero.skills.active[3], icons.activeSkill4));
                skillsAttr.AddRange(getPartialListViewForActiveSkill(Resources.GetString(Resource.String.skill) + " 3", hero.skills.active[4], icons.activeSkill5));
                skillsAttr.AddRange(getPartialListViewForActiveSkill(Resources.GetString(Resource.String.skill) + " 4", hero.skills.active[5], icons.activeSkill6));

                skillsAttr.AddRange(getPartialListViewForPassiveSkill(Resources.GetString(Resource.String.passiveSkill), hero.skills.passive[0], icons.passiveSkill1));
                skillsAttr.AddRange(getPartialListViewForPassiveSkill(Resources.GetString(Resource.String.passiveSkill), hero.skills.passive[1], icons.passiveSkill2));
                skillsAttr.AddRange(getPartialListViewForPassiveSkill(Resources.GetString(Resource.String.passiveSkill), hero.skills.passive[2], icons.passiveSkill3));

                heroSkillsListView.Adapter = new SectionedListAdapter(Activity, skillsAttr.ToArray());
            }
        }
    }
}