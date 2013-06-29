using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

using ZTnDroid.D3Calculator.Helpers;

using Fragment = Android.Support.V4.App.Fragment;

namespace ZTnDroid.D3Calculator.Fragments
{
    public class GearItemEditorFragment : Fragment
    {
        #region >> Fragment

        /// <inheritdoc/>
        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            switch (requestCode)
            {
                //case ADD_NEW_ACCOUNT:
                //    switch (resultCode)
                //    {
                //        case -1:
                //            break;

                //        default:
                //            break;
                //    }
                //    break;

                default:
                    break;
            }

            base.OnActivityResult(requestCode, resultCode, data);
        }

        /// <inheritdoc/>
        public override void OnCreate(Bundle savedInstanceState)
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            base.OnCreate(savedInstanceState);

            SetHasOptionsMenu(true);
        }

        /// <inheritdoc/>
        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            // Activity.MenuInflater.Inflate(Resource.Menu.GearItemEditorActivity, menu);

            base.OnCreateOptionsMenu(menu, inflater);
        }

        /// <inheritdoc/>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            View view = inflater.Inflate(Resource.Layout.GearItemEditor, container, false);

            LayoutInflater layoutInflater = (LayoutInflater)Activity.GetSystemService(Context.LayoutInflaterService);

            LinearLayout layoutAttributes = view.FindViewById<LinearLayout>(Resource.Id.layoutAttributes);
            layoutAttributes.AddView(layoutInflater.Inflate(Resource.Layout.GearItemRowEditor, null), layoutAttributes.ChildCount - 1);

            LinearLayout layoutResistances = view.FindViewById<LinearLayout>(Resource.Id.layoutResistances);
            layoutResistances.AddView(layoutInflater.Inflate(Resource.Layout.GearItemRowEditor, null), layoutResistances.ChildCount - 1);

            return view;
        }

        /// <inheritdoc/>
        public override void OnDestroyView()
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            base.OnDestroyView();
        }

        #endregion
    }
}