using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using ZTn.BNet.D3.Heroes;

namespace ZTnDroid.D3Calculator.Adapters
{
    public class HeroSummariesListAdapter : BaseAdapter
    {
        Context context;
        HeroSummary[] heroes;

        public HeroSummariesListAdapter(Context context, HeroSummary[] heroes)
        {
            this.context = context;
            this.heroes = heroes;
        }

        public override int Count
        {
            get { return heroes.Length; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view;
            if (convertView == null)
            {
                view = ((Activity)context).LayoutInflater.Inflate(Resource.Layout.HeroesListItem, parent, false);
            }
            else
            {
                view = convertView;
            }

            HeroSummary hero = heroes[position];
            view.FindViewById<TextView>(Resource.Id.heroName).Text = heroes[position].name;
            view.FindViewById<TextView>(Resource.Id.heroClass).Text = hero.heroClass;
            view.FindViewById<TextView>(Resource.Id.heroLevel).Text = String.Format("{0}", hero.level);
            view.FindViewById<TextView>(Resource.Id.heroParagon).Text = String.Format("+{0}", hero.paragonLevel);
            view.FindViewById<TextView>(Resource.Id.heroHardcore).Text = (hero.hardcore ? "hardcore" : "");

            return view;
        }

        public HeroSummary getHeroSummaryAt(int position)
        {
            return heroes[position];
        }
    }
}