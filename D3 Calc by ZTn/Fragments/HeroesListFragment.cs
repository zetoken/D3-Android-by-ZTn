using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using ZTn.BNet.BattleNet;
using ZTn.BNet.D3;
using ZTn.BNet.D3.Careers;
using ZTn.BNet.D3.DataProviders;
using ZTn.BNet.D3.Heroes;
using ZTnDroid.D3Calculator.Adapters;
using ZTnDroid.D3Calculator.Helpers;
using ZTnDroid.D3Calculator.Storage;
using Fragment = Android.Support.V4.App.Fragment;

namespace ZTnDroid.D3Calculator.Fragments
{
    public class HeroesListFragment : Fragment
    {
        String battleTag;
        String host;

        Career career;

        public override void OnCreate(Bundle savedInstanceState)
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            base.OnCreate(savedInstanceState);

            RetainInstance = true;

            SetHasOptionsMenu(true);

            battleTag = D3Context.getInstance().battleTag;
            host = D3Context.getInstance().host;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            View view = inflater.Inflate(Resource.Layout.ViewCareer, container, false);

            ListView listView = view.FindViewById<ListView>(Resource.Id.HeroesListView);
            listView.ItemClick += (Object sender, Android.Widget.AdapterView.ItemClickEventArgs args) =>
            {
                HeroSummary heroSummary = ((HeroSummariesListAdapter)listView.Adapter).getHeroSummaryAt(args.Position);
                D3Context.getInstance().heroSummary = heroSummary;

                Intent viewHeroIntent = new Intent(Activity, typeof(ViewHeroActivity));

                StartActivity(viewHeroIntent);
            };

            Activity.Title = battleTag;

            deferredFetchAndUpdateCareer(D3Context.getInstance().onlineMode);

            return view;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            base.OnCreateOptionsMenu(menu, inflater);

            Activity.MenuInflater.Inflate(Resource.Menu.ViewCareerActivity, menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            switch (item.ItemId)
            {
                case Resource.Id.DeleteContent:
                    deleteCareer();
                    return true;

                case Resource.Id.RefreshContent:
                    deferredFetchAndUpdateCareer(OnlineMode.Online);
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void deferredFetchAndUpdateCareer(OnlineMode online)
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            ProgressDialog progressDialog = null;

            if (online == OnlineMode.Online)
                progressDialog = ProgressDialog.Show(Activity, Resources.GetString(Resource.String.LoadingCareer), Resources.GetString(Resource.String.WaitWhileRetrievingData), true);

            new Thread(new ThreadStart(() =>
            {
                try
                {
                    fetchCareer(online);
                    Activity.RunOnUiThread(() =>
                    {
                        if (online == OnlineMode.Online)
                            progressDialog.Dismiss();
                        updateCareerView();
                    });
                }
                catch (ZTn.BNet.D3.DataProviders.FileNotInCacheException)
                {
                    Activity.RunOnUiThread(() =>
                    {
                        if (online == OnlineMode.Online)
                            progressDialog.Dismiss();
                        Toast.MakeText(Activity, "Career not in cache" + System.Environment.NewLine + "Please use refresh action", ToastLength.Long).Show();
                    });
                }
                catch (Exception exception)
                {
                    Activity.RunOnUiThread(() =>
                    {
                        if (online == OnlineMode.Online)
                            progressDialog.Dismiss();
                        Toast.MakeText(Activity, Resources.GetString(Resource.String.ErrorOccuredWhileRetrievingData), ToastLength.Long).Show();
                        Console.WriteLine(exception);
                    });
                }
            })).Start();
        }

        private void deleteCareer()
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            Toast.MakeText(Activity, "Career will be removed...", ToastLength.Short);
            D3Context.getInstance().dbAccounts.delete(battleTag, host);
            Activity.Finish();
        }

        private void fetchCareer(OnlineMode online)
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            D3Api.host = host;
            DataProviders.CacheableDataProvider dataProvider = (DataProviders.CacheableDataProvider)D3Api.dataProvider;
            dataProvider.onlineMode = online;

            try
            {
                career = Career.getCareerFromBattleTag(new BattleTag(battleTag));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                career = null;
                throw exception;
            }
            finally
            {
                dataProvider.onlineMode = D3Context.getInstance().onlineMode;
            }
        }

        private void updateCareerView()
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            if (career != null)
            {
                ListView killsListView = Activity.FindViewById<ListView>(Resource.Id.killsLifetimeListView);
                List<IListItem> attributes = new List<IListItem>()
                {
                    new SectionHeaderListItem(Resources.GetString(Resource.String.KillsLifetime)),
                    new AttributeListItem(Resources.GetString(Resource.String.elites), career.kills.elites.ToString()),
                    new AttributeListItem(Resources.GetString(Resource.String.KilledMonsters), career.kills.monsters.ToString()),
                    new AttributeListItem(Resources.GetString(Resource.String.KilledHardcore), career.kills.hardcoreMonsters.ToString())
                };
                killsListView.Adapter = new SectionedListAdapter(Activity, attributes.ToArray());

                if (career.heroes != null)
                {
                    ListView listView = Activity.FindViewById<ListView>(Resource.Id.HeroesListView);
                    listView.Adapter = new HeroSummariesListAdapter(Activity, career.heroes);
                }
            }
        }
    }
}