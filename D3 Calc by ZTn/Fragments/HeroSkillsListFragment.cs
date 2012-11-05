using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using ZTn.BNet.D3.Heroes;
using ZTn.BNet.D3.Skills;
using ZTnDroid.D3Calculator.Adapters;
using ZTnDroid.D3Calculator.Storage;

namespace ZTnDroid.D3Calculator.Fragments
{
    public class HeroSkillsListFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            Console.WriteLine("HeroSkillsListFragment: OnCreate");
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Console.WriteLine("HeroSkillsListFragment: OnCreateView");
            View view = inflater.Inflate(Resource.Layout.ViewHeroSkills, container, false);

            updateHeroView(view);

            return view;
        }

        List<IListItem> getPartialListViewForActiveSkill(String headerText, ActiveSkill active)
        {
            List<IListItem> list = new List<IListItem>();

            list.Add(new SectionHeaderListItem(headerText));
            if (active.skill != null)
                list.Add(new SkillListItem(active.skill));
            if (active.rune != null)
                list.Add(new RuneListItem(active.rune));

            return list;
        }

        List<IListItem> getPartialListViewForPassiveSkill(String headerText, PassiveSkill passive)
        {
            List<IListItem> list = new List<IListItem>();

            list.Add(new SectionHeaderListItem(headerText));
            if (passive.skill != null)
                list.Add(new SkillListItem(passive.skill));

            return list;
        }

        public void updateHeroView(View view)
        {
            Console.WriteLine("HeroSkillsListFragment: updateHeroSkills");

            Hero hero = D3Context.getInstance().hero;
            if (hero != null)
            {
                ListView heroSkillsListView = view.FindViewById<ListView>(Resource.Id.heroSkillsListView);

                List<IListItem> skillsAttr = new List<IListItem>();

                skillsAttr.AddRange(getPartialListViewForActiveSkill(Resources.GetString(Resource.String.skill) + " L", hero.skills.active[0]));
                skillsAttr.AddRange(getPartialListViewForActiveSkill(Resources.GetString(Resource.String.skill) + " R", hero.skills.active[1]));
                skillsAttr.AddRange(getPartialListViewForActiveSkill(Resources.GetString(Resource.String.skill) + " 1", hero.skills.active[2]));
                skillsAttr.AddRange(getPartialListViewForActiveSkill(Resources.GetString(Resource.String.skill) + " 2", hero.skills.active[3]));
                skillsAttr.AddRange(getPartialListViewForActiveSkill(Resources.GetString(Resource.String.skill) + " 3", hero.skills.active[4]));
                skillsAttr.AddRange(getPartialListViewForActiveSkill(Resources.GetString(Resource.String.skill) + " 4", hero.skills.active[5]));

                skillsAttr.AddRange(getPartialListViewForPassiveSkill(Resources.GetString(Resource.String.passiveSkill), hero.skills.passive[0]));
                skillsAttr.AddRange(getPartialListViewForPassiveSkill(Resources.GetString(Resource.String.passiveSkill), hero.skills.passive[1]));
                skillsAttr.AddRange(getPartialListViewForPassiveSkill(Resources.GetString(Resource.String.passiveSkill), hero.skills.passive[2]));

                heroSkillsListView.Adapter = new SectionedListAdapter(Activity, skillsAttr.ToArray());
            }
        }
    }
}