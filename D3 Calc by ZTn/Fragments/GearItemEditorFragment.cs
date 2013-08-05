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
using ZTn.BNet.D3.Items;
using ZTnDroid.D3Calculator.Helpers;
using ZTnDroid.D3Calculator.Storage;
using Fragment = Android.Support.V4.App.Fragment;

namespace ZTnDroid.D3Calculator.Fragments
{
    public class GearItemEditorFragment : Fragment
    {
        LayoutInflater layoutInflater;

        #region >> Constants

        static readonly List<int> attributeResources = new List<int>()
        {
            Resource.String.dexterity,
            Resource.String.intelligence, 
            Resource.String.strength, 
            Resource.String.vitality 
        };
        static readonly List<int> damageResources = new List<int>()
        {
            Resource.String.attackSpeed,
            Resource.String.damage,
            Resource.String.criticChance,
            Resource.String.criticDamage
        };
        static readonly List<int> defenseResources = new List<int>()
        { 
            Resource.String.armor, 
            Resource.String.allResists,
            Resource.String.arcaneResist, 
            Resource.String.coldResist, 
            Resource.String.fireResist, 
            Resource.String.lightningResist, 
            Resource.String.physicalResist, 
            Resource.String.poisonResist ,
            Resource.String.lifeBonusPerGlobe,
            Resource.String.lifeOnHit,
            Resource.String.lifeRegenPerSecond,
            Resource.String.lifeSteal
        };
        static readonly List<int> socketsResources = new List<int>()
        {
        };

        #endregion

        List<String> attributeLabels;
        List<String> damageLabels;
        List<String> defenseLabels;
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
            attributeLabels = attributeResources.Select(id => Resources.GetString(id)).ToList();
            damageLabels = damageResources.Select(id => Resources.GetString(id)).ToList();
            defenseLabels = defenseResources.Select(id => Resources.GetString(id)).ToList();
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

            ItemAttributes editingItemAttr = D3Context.instance.editingItem.attributesRaw;

            // Attributes
            LinearLayout layoutAttributes = view.FindViewById<LinearLayout>(Resource.Id.layoutAttributes);
            layoutAttributes.FindViewById<ImageButton>(Resource.Id.add).Click +=
                (Object sender, EventArgs e) => layoutAttributes.AddView(createRowView(attributeLabels, attributeResources), layoutAttributes.ChildCount - 1);

            createRowView(layoutAttributes, attributeLabels, attributeResources, Resource.String.dexterity, editingItemAttr.dexterityItem);
            createRowView(layoutAttributes, attributeLabels, attributeResources, Resource.String.intelligence, editingItemAttr.intelligenceItem);
            createRowView(layoutAttributes, attributeLabels, attributeResources, Resource.String.strength, editingItemAttr.strengthItem);
            createRowView(layoutAttributes, attributeLabels, attributeResources, Resource.String.vitality, editingItemAttr.vitalityItem);

            // Damages
            LinearLayout layoutDamage = view.FindViewById<LinearLayout>(Resource.Id.layoutDamage);
            layoutDamage.FindViewById<ImageButton>(Resource.Id.add).Click +=
                (Object sender, EventArgs e) => layoutDamage.AddView(createRowView(damageLabels, damageResources), layoutDamage.ChildCount - 1);

            createRowView(layoutDamage, damageLabels, damageResources, Resource.String.damage, editingItemAttr.damageWeaponMin_Physical);
            createRowView(layoutDamage, damageLabels, damageResources, Resource.String.attackSpeed, editingItemAttr.attacksPerSecondItem);
            createRowView(layoutDamage, damageLabels, damageResources, Resource.String.criticChance, editingItemAttr.critDamagePercent);
            createRowView(layoutDamage, damageLabels, damageResources, Resource.String.criticDamage, editingItemAttr.critPercentBonusCapped);

            // Defense
            LinearLayout layoutDefense = view.FindViewById<LinearLayout>(Resource.Id.layoutDefense);
            layoutDefense.FindViewById<ImageButton>(Resource.Id.add).Click +=
                (Object sender, EventArgs e) => layoutDefense.AddView(createRowView(defenseLabels, defenseResources), layoutDefense.ChildCount - 1);

            createRowView(layoutDefense, defenseLabels, defenseResources, Resource.String.armor, editingItemAttr.armorItem);
            createRowView(layoutDefense, defenseLabels, defenseResources, Resource.String.allResists, editingItemAttr.resistance_All);
            createRowView(layoutDefense, defenseLabels, defenseResources, Resource.String.arcaneResist, editingItemAttr.resistance_Arcane);
            createRowView(layoutDefense, defenseLabels, defenseResources, Resource.String.coldResist, editingItemAttr.resistance_Cold);
            createRowView(layoutDefense, defenseLabels, defenseResources, Resource.String.fireResist, editingItemAttr.resistance_Fire);
            createRowView(layoutDefense, defenseLabels, defenseResources, Resource.String.lightningResist, editingItemAttr.resistance_Lightning);
            createRowView(layoutDefense, defenseLabels, defenseResources, Resource.String.physicalResist, editingItemAttr.resistance_Physical);
            createRowView(layoutDefense, defenseLabels, defenseResources, Resource.String.poisonResist, editingItemAttr.resistance_Poison);
            createRowView(layoutDefense, defenseLabels, defenseResources, Resource.String.lifeRegenPerSecond, editingItemAttr.hitpointsRegenPerSecond);
            createRowView(layoutDefense, defenseLabels, defenseResources, Resource.String.lifeOnHit, editingItemAttr.hitpointsOnHit);
            createRowView(layoutDefense, defenseLabels, defenseResources, Resource.String.lifeSteal, editingItemAttr.stealHealthPercent);
            createRowView(layoutDefense, defenseLabels, defenseResources, Resource.String.lifeBonusPerGlobe, editingItemAttr.healthGlobeBonusHealth);

            // Sockets
            LinearLayout layoutSockets = view.FindViewById<LinearLayout>(Resource.Id.layoutSockets);
            layoutSockets.FindViewById<ImageButton>(Resource.Id.add).Click +=
                (Object sender, EventArgs e) => layoutSockets.AddView(createRowView(sockets, socketsResources), layoutSockets.ChildCount - 1);

            return view;
        }

        /// <inheritdoc/>
        public override void OnDestroyView()
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            base.OnDestroyView();
        }

        #endregion

        void createRowView(LinearLayout layout, List<string> labels, List<int> resources, int selectedResource, ItemValueRange value)
        {
            if (value != null && value.min != 0)
                layout.AddView(createRowView(labels, resources, selectedResource, value), layout.ChildCount - 1);
        }

        View createRowView(List<String> texts, List<int> resources)
        {
            // Get the view from inflater
            View rowView = layoutInflater.Inflate(Resource.Layout.GearItemRowEditor, null);

            rowView.Tag = new JavaLangObject<List<int>>(resources);

            // Set "remove row" action
            rowView.FindViewById<ImageButton>(Resource.Id.remove).Click +=
                (Object sender, EventArgs e) =>
                {
                    ImageButton removeButton = (ImageButton)sender;
                    View parent = (View)removeButton.Parent;
                    LinearLayout parentContainer = (LinearLayout)parent.Parent;
                    parentContainer.RemoveView(parent);
                };
            rowView.FindViewById<Spinner>(Resource.Id.attributeName).Adapter = new ArrayAdapter<String>(Activity, Android.Resource.Layout.SimpleSpinnerItem, texts);

            return rowView;
        }

        View createRowView(List<String> texts, List<int> resources, int selected, ItemValueRange value)
        {
            View rowView = createRowView(texts, resources);

            int index = 0;
            for (; index < resources.Count; index++)
                if (resources[index] == selected)
                    break;

            rowView.FindViewById<Spinner>(Resource.Id.attributeName).SetSelection(index);
            rowView.FindViewById<EditText>(Resource.Id.attributeValue).Text = value.min.ToString();

            return rowView;
        }
    }
}