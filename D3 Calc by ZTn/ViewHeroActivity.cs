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
        String battleTag;
        String host;
        HeroSummary heroSummary;

        ActionBar.Tab tabCharacteristics;
        ActionBar.Tab tabGear;
        ActionBar.Tab tabSkills;

        protected override void OnCreate(Bundle bundle)
        {
            Console.WriteLine("ViewHeroActivity: OnCreate");
            base.OnCreate(bundle);

            this.Application.SetTheme(Android.Resource.Style.ThemeHolo);

            battleTag = D3Context.getInstance().battleTag;
            host = D3Context.getInstance().host;
            heroSummary = D3Context.getInstance().heroSummary;

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

            D3Context.getInstance().hero = null;
            deferredFetchHero(D3Context.getInstance().onlineMode);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.ViewHeroActivity, menu);

            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;

                case Resource.Id.RefreshContent:
                    deferredFetchHero(true);
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void deferredFetchHero(Boolean online)
        {
            ProgressDialog progressDialog = null;

            if (online)
                progressDialog = ProgressDialog.Show(this, "Loading Hero", "Please wait while retrieving data", true);

            new Thread(new ThreadStart(() =>
            {
                try
                {
                    D3Context.getInstance().hero = fetchHero(online);
                    RunOnUiThread(() =>
                    {
                        if (online)
                            progressDialog.Dismiss();
                        ActionBar.SetSelectedNavigationItem(0);
                    });
                }
                catch (ZTn.BNet.D3.DataProviders.FileNotInCacheException)
                {
                    RunOnUiThread(() =>
                    {
                        if (online)
                            progressDialog.Dismiss();
                        Toast.MakeText(this, "Hero not in cache" + System.Environment.NewLine + "Please use refresh action", ToastLength.Long).Show();
                    });
                }
                catch (Exception exception)
                {
                    RunOnUiThread(() =>
                    {
                        if (online)
                            progressDialog.Dismiss();
                        Toast.MakeText(this, "An error occured when retrieving the hero", ToastLength.Long).Show();
                        Console.WriteLine(exception);
                    });
                }
            })).Start();
        }

        private Hero fetchHero(Boolean online)
        {
            Console.WriteLine("ViewHeroActivity: fetchHero");
            Hero hero = null;

            D3Api.host = host;
            DataProviders.CacheableDataProvider dataProvider = (DataProviders.CacheableDataProvider)D3Api.dataProvider;
            dataProvider.online = online;

            try
            {
                hero = Hero.getHeroFromHeroId(new BattleTag(battleTag), heroSummary.id);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                hero = null;
                throw exception;
            }
            finally
            {
                dataProvider.online = D3Context.getInstance().onlineMode;
            }

            return hero;
        }
    }
}