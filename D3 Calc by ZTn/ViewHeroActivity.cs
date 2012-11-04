using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ZTn.BNet.BattleNet;
using ZTn.BNet.D3;
using ZTn.BNet.D3.Heroes;
using ZTnDroid.D3Calculator.Adapters;
using ZTnDroid.D3Calculator.Fragments;
using ZTnDroid.D3Calculator.Storage;

namespace ZTnDroid.D3Calculator
{
    [Activity(Label = "View Hero", Theme = "@android:style/Theme.Holo", Icon = "@drawable/icon")]
    public class ViewHeroActivity : Activity
    {
        ActionBar.Tab tabCharacteristics;
        ActionBar.Tab tabGear;
        ActionBar.Tab tabSkills;

        protected override void OnCreate(Bundle bundle)
        {
            Console.WriteLine("ViewHeroActivity: OnCreate");
            base.OnCreate(bundle);

            this.Application.SetTheme(Android.Resource.Style.ThemeHolo);

            SetContentView(Resource.Layout.FragmentContainer);

            this.Title = D3Context.getInstance().heroSummary.name;
            this.ActionBar.Subtitle = D3Context.getInstance().battleTag;

            ActionBar.SetDisplayHomeAsUpEnabled(true);

            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;

            tabCharacteristics = ActionBar.NewTab();
            tabCharacteristics.SetText(Resources.GetString(Resource.String.details));
            tabCharacteristics.SetTabListener(new SimpleTabListener<HeroCharacteristicsListFragment>());
            ActionBar.AddTab(tabCharacteristics);

            tabGear = ActionBar.NewTab();
            tabGear.SetText(Resources.GetString(Resource.String.gear));
            tabGear.SetTabListener(new SimpleTabListener<HeroGearListFragment>());
            ActionBar.AddTab(tabGear);

            tabSkills = ActionBar.NewTab();
            tabSkills.SetText(Resources.GetString(Resource.String.skills));
            tabSkills.SetTabListener(new SimpleTabListener<HeroSkillsListFragment>());
            ActionBar.AddTab(tabSkills);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}