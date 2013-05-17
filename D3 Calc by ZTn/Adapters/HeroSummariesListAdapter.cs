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
                view = ((Activity)context).LayoutInflater.Inflate(Resource.Layout.HeroesListItem, parent, false);
            else
                view = convertView;

            HeroSummary hero = heroes[position];
            view.FindViewById<TextView>(Resource.Id.heroName).Text = heroes[position].name;
            view.FindViewById<TextView>(Resource.Id.heroClass).Text = hero.heroClass.ToString();
            view.FindViewById<TextView>(Resource.Id.heroLevel).Text = String.Format("{0}", hero.level);
            view.FindViewById<TextView>(Resource.Id.heroParagon).Text = String.Format("+{0}", hero.paragonLevel);
            view.FindViewById<TextView>(Resource.Id.heroHardcore).Text = (hero.hardcore ? "hardcore" : "");
            view.FindViewById<TextView>(Resource.Id.heroLastUpdated).Text = hero.lastUpdated.ToString("dd/MM/yyyy HH:mm");

            int imageResource = Resource.Drawable.Icon;
            switch (hero.heroClass)
            {
                case HeroClass.Barbarian:
                    imageResource = (hero.gender == HeroGender.Male ? Resource.Drawable.barbarian_male : Resource.Drawable.barbarian_female);
                    break;
                case HeroClass.DemonHunter:
                    imageResource = (hero.gender == HeroGender.Male ? Resource.Drawable.demonhunter_male : Resource.Drawable.demonhunter_female);
                    break;
                case HeroClass.Monk:
                    imageResource = (hero.gender == HeroGender.Male ? Resource.Drawable.monk_male : Resource.Drawable.monk_female);
                    break;
                case HeroClass.WitchDoctor:
                    imageResource = (hero.gender == HeroGender.Male ? Resource.Drawable.witchdoctor_male : Resource.Drawable.witchdoctor_female);
                    break;
                case HeroClass.Wizard:
                    imageResource = (hero.gender == HeroGender.Male ? Resource.Drawable.wizard_male : Resource.Drawable.wizard_female);
                    break;
                default:
                    break;
            }
            view.FindViewById<ImageView>(Resource.Id.imageClass).SetImageResource(imageResource);

            return view;
        }

        public HeroSummary getHeroSummaryAt(int position)
        {
            return heroes[position];
        }
    }
}