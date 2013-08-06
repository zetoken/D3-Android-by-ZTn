using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using ZTnDroid.D3Calculator.Fragments;
using ZTnDroid.D3Calculator.Helpers;

namespace ZTnDroid.D3Calculator
{
    [Activity(Label = "@string/GearItemEditorActivityLabel")]
    public class GearItemEditorActivity : ZTnFragmentActivity
    {
        public const int ITEM_EDIT = 1;

        #region >> Fragment

        /// <inheritdoc/>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.FragmentContainer);

            ActionBar.SetDisplayHomeAsUpEnabled(true);

            SupportFragmentManager
                .BeginTransaction()
                .Add(Resource.Id.fragment_container, new GearItemEditorFragment())
                .Commit();
        }

        public override void Finish()
        {
            SetResult(Result.Ok, new Intent());

            base.Finish();
        }

        #endregion
    }
}