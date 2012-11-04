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

            updateHeroSkills(view);

            return view;
        }

        public void updateHeroSkills(View view)
        {
            Console.WriteLine("HeroSkillsListFragment: updateHeroSkills");

            Hero hero = D3Context.getInstance().hero;

            if (hero != null)
            {
                Console.WriteLine("Hero OK");
                ListView heroSkillsListView = view.FindViewById<ListView>(Resource.Id.heroSkillsListView);
                List<IListItem> skillsAttr = new List<IListItem>()
                {
                    new SectionHeaderListItem(Resources.GetString(Resource.String.skill)+" G"),
                    new SkillListItem(hero.skills.active[0].skill),
                    new SectionHeaderListItem(Resources.GetString(Resource.String.skill)+" R"),
                    new SkillListItem(hero.skills.active[1].skill),
                    new SectionHeaderListItem(Resources.GetString(Resource.String.skill)+" 1"),
                    new SkillListItem(hero.skills.active[2].skill),
                    new SectionHeaderListItem(Resources.GetString(Resource.String.skill)+" 2"),
                    new SkillListItem(hero.skills.active[3].skill),
                    new SectionHeaderListItem(Resources.GetString(Resource.String.skill)+" 3"),
                    new SkillListItem(hero.skills.active[4].skill),
                    new SectionHeaderListItem(Resources.GetString(Resource.String.skill)+" 4"),
                    new SkillListItem(hero.skills.active[5].skill),
                };
                heroSkillsListView.Adapter = new SectionedListAdapter(Activity, skillsAttr.ToArray());
            }
        }
    }
}