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
using ZTn.BNet.D3.Careers;
using ZTn.BNet.D3.Heroes;
using ZTnDroid.D3Calculator.Adapters;
using ZTnDroid.D3Calculator.Storage;

namespace ZTnDroid.D3Calculator.Fragments
{
    public class HeroesListFragment : Fragment
    {
        String battleTag;
        String host;

        Career career;

        public override void OnCreate(Bundle savedInstanceState)
        {
            Console.WriteLine("HeroesListFragment: OnCreate");
            base.OnCreate(savedInstanceState);

            SetHasOptionsMenu(true);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Console.WriteLine("HeroesListFragment: OnCreate");
            View view = inflater.Inflate(Resource.Layout.ViewCareer, container, false);
            battleTag = Activity.Intent.GetStringExtra("battleTag");
            host = Activity.Intent.GetStringExtra("host");

            battleTag = D3Context.getInstance().battleTag;
            host = D3Context.getInstance().host;

            ListView listView = view.FindViewById<ListView>(Resource.Id.HeroesListView);
            listView.ItemClick += (Object sender, Android.Widget.AdapterView.ItemClickEventArgs args) =>
            {
                HeroSummary heroSummary = ((HeroSummariesListAdapter)listView.Adapter).getHeroSummaryAt(args.Position);
                D3Context.getInstance().heroSummary = heroSummary;

                Intent viewHeroIntent = new Intent(Activity, typeof(ViewHeroActivity));

                StartActivity(viewHeroIntent);
            };

            Activity.Title = battleTag;

            deferredFetchAndUpdateCareer(false);

            return view;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnCreateOptionsMenu(menu, inflater);

            Activity.MenuInflater.Inflate(Resource.Menu.ViewCareerActivity, menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.DeleteContent:
                    deleteCareer();
                    return true;

                case Resource.Id.RefreshContent:
                    deferredFetchAndUpdateCareer(true);
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void deferredFetchAndUpdateCareer(Boolean online)
        {
            ProgressDialog progressDialog = null;

            if (online)
                progressDialog = ProgressDialog.Show(Activity, "Loading Career", "Please wait while retrieving data", true);

            new Thread(new ThreadStart(() =>
            {
                try
                {
                    fetchCareer(online);
                    Activity.RunOnUiThread(() =>
                    {
                        if (online)
                            progressDialog.Dismiss();
                        updateCareerView();
                    });
                }
                catch (ZTn.BNet.D3.DataProviders.FileNotInCacheException)
                {
                    Activity.RunOnUiThread(() =>
                    {
                        if (online)
                            progressDialog.Dismiss();
                        Toast.MakeText(Activity, "Career not in cache" + System.Environment.NewLine + "Please use refresh action", ToastLength.Long).Show();
                    });
                }
                catch (Exception exception)
                {
                    Activity.RunOnUiThread(() =>
                    {
                        if (online)
                            progressDialog.Dismiss();
                        Toast.MakeText(Activity, "An error occured when retrieving the career", ToastLength.Long).Show();
                        Console.WriteLine(exception);
                    });
                }
            })).Start();
        }

        private void deleteCareer()
        {
            Toast.MakeText(Activity, "Career will be removed...", ToastLength.Short);
            D3Context.getInstance().dbAccounts.delete(battleTag, host);
            Activity.Finish();
        }

        private void fetchCareer(Boolean online)
        {
            D3Api.host = host;
            DataProviders.CacheableDataProvider dataProvider = (DataProviders.CacheableDataProvider)D3Api.dataProvider;
            dataProvider.online = online;

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
                dataProvider.online = false;
            }
        }

        private void updateCareerView()
        {
            if (career != null)
            {
                ListView killsListView = Activity.FindViewById<ListView>(Resource.Id.killsLifetimeListView);
                List<AttributeListItem> attributes = new List<AttributeListItem>()
                {
                    new AttributeListItem("elites", career.kills.elites.ToString()),
                    new AttributeListItem("monsters", career.kills.monsters.ToString()),
                    new AttributeListItem("hardcore", career.kills.hardcoreMonsters.ToString())
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