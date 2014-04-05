using System.Reflection;
using Android.App;
using Android.OS;
using ZTnDroid.D3Calculator.Fragments;
using ZTnDroid.D3Calculator.Helpers;

namespace ZTnDroid.D3Calculator
{
    [Activity(Label = "@string/ViewCareerActivityLabel")]
    public class ViewCareerActivity : ZTnFragmentActivity
    {
        private static HeroesListFragment _heroesListFragment;

        #region >> FragmentActivity

        /// <inheritdoc />
        protected override void OnCreate(Bundle savedInstanceState)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            base.OnCreate(savedInstanceState);

            Application.SetTheme(Android.Resource.Style.ThemeHolo);

            SetContentView(Resource.Layout.FragmentContainer);

            ActionBar.SetDisplayHomeAsUpEnabled(true);

            if (savedInstanceState == null)
            {
                _heroesListFragment = new HeroesListFragment();
                SupportFragmentManager
                    .BeginTransaction()
                    .Add(Resource.Id.fragment_container, _heroesListFragment)
                    .Commit();
            }
        }

        #endregion
    }
}