using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using ZTn.BNet.D3.Medias;
using ZTn.BNet.D3.Skills;

namespace ZTnDroid.D3Calculator.Adapters
{
    public class SkillListItem : IListItem
    {
        public Skill skill;
        public D3Picture icon;

        public SkillListItem(Skill skill)
        {
            this.skill = skill;
        }

        public SkillListItem(Skill skill, D3Picture icon)
        {
            this.skill = skill;
            this.icon = icon;
        }

        public int getLayoutResource()
        {
            return Resource.Layout.SkillListItem;
        }

        public void updateHeroView(View view)
        {
            view.FindViewById<TextView>(Resource.Id.skillName).Text = skill.name;
            view.FindViewById<TextView>(Resource.Id.skillDescription).Text = skill.description;
            if (icon != null)
            {
                Bitmap bitmap = Android.Graphics.BitmapFactory.DecodeByteArray(icon.bytes, 0, (int)icon.bytes.Length);
                view.FindViewById<ImageView>(Resource.Id.imageSkill).SetImageBitmap(bitmap);
            }
        }
    }
}
