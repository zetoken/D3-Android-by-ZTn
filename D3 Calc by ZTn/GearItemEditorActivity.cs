using Android.App;
using Android.OS;
using System.Reflection;
using ZTnDroid.D3Calculator.Fragments;
using ZTnDroid.D3Calculator.Helpers;

namespace ZTnDroid.D3Calculator
{
    [Activity(Label = "@string/GearItemEditorActivityLabel")]
    public class GearItemEditorActivity : ZTnFragmentActivity
    {
        public const int ItemEdit = 1;

        #region >> Fragment

        /// <inheritdoc/>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.FragmentContainer);

            ActionBar.SetDisplayHomeAsUpEnabled(true);

            SupportFragmentManager
                .BeginTransaction()
                .Add(Resource.Id.fragment_container, new GearItemEditorFragment())
                .Commit();
        }

        #endregion
    }
}