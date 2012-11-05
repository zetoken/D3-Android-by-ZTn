using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Views;
using Android.Widget;
using ZTn.BNet.D3.Skills;

namespace ZTnDroid.D3Calculator.Adapters
{
    public class RuneListItem : IListItem
    {
        public Rune rune;

        public RuneListItem(Rune rune)
        {
            this.rune = rune;
        }

        public int getLayoutResource()
        {
            return Resource.Layout.RuneListItem;
        }

        public void updateHeroView(View view)
        {
            view.FindViewById<TextView>(Resource.Id.runeName).Text = rune.name;
            view.FindViewById<TextView>(Resource.Id.runeDescription).Text = rune.description;
            //view.FindViewById<ImageView>(Resource.Id.imageSkill)
        }
    }
}
