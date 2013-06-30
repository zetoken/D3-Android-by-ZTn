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
using ZTnDroid.D3Calculator.Storage;
using Fragment = Android.Support.V4.App.Fragment;

namespace ZTnDroid.D3Calculator.Fragments
{
    public class GearItemEditorFragment : Fragment
    {
        LayoutInflater layoutInflater;

        #region >> Constants

        static readonly List<int> attributesResources = new List<int>() { Resource.String.dexterity, Resource.String.intelligence, Resource.String.strength, Resource.String.vitality };
        static readonly List<int> defensesResources = new List<int>() { Resource.String.armor, Resource.String.arcaneResist, Resource.String.coldResist, Resource.String.fireResist, Resource.String.lightningResist, Resource.String.physicalResist, Resource.String.poisonResist };
        static readonly List<int> socketsResources = new List<int>() { };

        #endregion

        List<String> attributes;
        List<String> defenses;
        List<String> sockets;

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

            // Get inflater service
            layoutInflater = (LayoutInflater)Activity.GetSystemService(Context.LayoutInflaterService);

            // Build localized strings
            attributes = attributesResources.Select(id => Resources.GetString(id)).ToList();
            defenses = defensesResources.Select(id => Resources.GetString(id)).ToList();
            sockets = socketsResources.Select(id => Resources.GetString(id)).ToList();
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

            view.FindViewById<TextView>(Resource.Id.gearItemName).Text = D3Context.instance.editingItem.name;

            // Attributes
            LinearLayout layoutAttributes = view.FindViewById<LinearLayout>(Resource.Id.layoutAttributes);
            layoutAttributes.FindViewById<ImageButton>(Resource.Id.add).Click +=
                (Object sender, EventArgs e) => layoutAttributes.AddView(createRowView(attributes), layoutAttributes.ChildCount - 1);

            // Defense
            LinearLayout layoutDefense = view.FindViewById<LinearLayout>(Resource.Id.layoutDefense);
            layoutDefense.FindViewById<ImageButton>(Resource.Id.add).Click +=
                (Object sender, EventArgs e) => layoutDefense.AddView(createRowView(defenses), layoutDefense.ChildCount - 1);

            // Sockets
            LinearLayout layoutSockets = view.FindViewById<LinearLayout>(Resource.Id.layoutSockets);
            layoutSockets.FindViewById<ImageButton>(Resource.Id.add).Click +=
                (Object sender, EventArgs e) => layoutSockets.AddView(createRowView(sockets), layoutSockets.ChildCount - 1);

            return view;
        }

        /// <inheritdoc/>
        public override void OnDestroyView()
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            base.OnDestroyView();
        }

        #endregion

        View createRowView(List<String> list)
        {
            // Get the view from inflater
            View rowView = layoutInflater.Inflate(Resource.Layout.GearItemRowEditor, null);

            // Set "remove row" action
            rowView.FindViewById<ImageButton>(Resource.Id.remove).Click +=
                (Object sender, EventArgs e) =>
                {
                    ImageButton removeButton = (ImageButton)sender;
                    View parent = (View)removeButton.Parent;
                    LinearLayout parentContainer = (LinearLayout)parent.Parent;
                    parentContainer.RemoveView(parent);
                };
            rowView.FindViewById<Spinner>(Resource.Id.attributeName).Adapter = new ArrayAdapter<String>(Activity, Android.Resource.Layout.SimpleSpinnerItem, list);

            return rowView;
        }
    }
}