using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using System;
using ZTn.BNet.D3.Heroes;
using ZTnDroid.D3Calculator.Helpers;

namespace ZTnDroid.D3Calculator.Adapters
{
    public class HeroSummariesListAdapter : BaseAdapter
    {
        readonly Context context;
        readonly HeroSummary[] heroes;

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
                view = ((Activity)context).LayoutInflater.Inflate(Resource.Layout.HeroesListItem, parent, false);
            else
                view = convertView;

            HeroSummary hero = heroes[position];
            view.FindViewById<TextView>(Resource.Id.heroName).Text = heroes[position].name;
            view.FindViewById<TextView>(Resource.Id.heroClass).Text = hero.heroClass.Translate().CapitalizeFirstLetter();
            view.FindViewById<TextView>(Resource.Id.heroLevel).Text = String.Format("{0}", hero.level);
            view.FindViewById<TextView>(Resource.Id.heroParagon).Text = String.Format("+{0}", hero.paragonLevel);
            view.FindViewById<TextView>(Resource.Id.heroHardcore).Text = (hero.hardcore ? "hardcore" : "");
            view.FindViewById<TextView>(Resource.Id.heroLastUpdated).Text = hero.lastUpdated.ToString("dd/MM/yyyy HH:mm");

            int imageResource = hero.heroClass.GetPortraitResource(hero.gender);
            view.FindViewById<ImageView>(Resource.Id.imageClass).SetImageResource(imageResource);

            return view;
        }

        public HeroSummary GetHeroSummaryAt(int position)
        {
            return heroes[position];
        }
    }
}