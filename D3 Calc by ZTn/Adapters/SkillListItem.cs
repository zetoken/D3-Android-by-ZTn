using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Views;
using Android.Widget;
using ZTn.BNet.D3.Skills;

namespace ZTnDroid.D3Calculator.Adapters
{
    public class SkillListItem : IListItem
    {
        public Skill skill;

        public SkillListItem(Skill skill)
        {
            this.skill = skill;
        }

        public int getLayoutResource()
        {
            return Resource.Layout.SkillListItem;
        }

        public void updateHeroView(View view)
        {
            view.FindViewById<TextView>(Resource.Id.skillName).Text = skill.name;
            view.FindViewById<TextView>(Resource.Id.skillDescription).Text = skill.description;
            //view.FindViewById<ImageView>(Resource.Id.imageSkill)
        }
    }
}
