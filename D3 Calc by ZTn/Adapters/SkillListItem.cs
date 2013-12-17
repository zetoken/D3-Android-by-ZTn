using Android.Graphics;
using Android.Views;
using Android.Widget;
using System;
using ZTn.BNet.D3.Medias;
using ZTn.BNet.D3.Skills;
using ZTnDroid.D3Calculator.Adapters.Delegated;

namespace ZTnDroid.D3Calculator.Adapters
{
    public class SkillListItem : IListItem
    {
        public Skill Skill;
        public D3Picture Icon;

        #region >> Constructors

        public SkillListItem(Skill skill)
        {
            Skill = skill;
        }

        public SkillListItem(Skill skill, D3Picture icon)
        {
            Skill = skill;
            Icon = icon;
        }

        #endregion

        #region >> IListItem

        /// <inheritdoc/>
        public int GetLayoutResource()
        {
            return Resource.Layout.SkillListItem;
        }

        /// <inheritdoc/>
        public bool IsEnabled()
        {
            return false;
        }

        /// <inheritdoc/>
        public void RemoveView(View view)
        {
        }

        /// <inheritdoc/>
        public void UpdateView(View view, Boolean recycled)
        {
            view.FindViewById<TextView>(Resource.Id.skillName).Text = Skill.name;
            view.FindViewById<TextView>(Resource.Id.skillDescription).Text = Skill.description;
            if (Icon != null)
            {
                Bitmap bitmap = BitmapFactory.DecodeByteArray(Icon.bytes, 0, Icon.bytes.Length);
                view.FindViewById<ImageView>(Resource.Id.imageSkill).SetImageBitmap(bitmap);
            }
        }

        #endregion
    }
}
