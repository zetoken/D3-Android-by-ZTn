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
using ZTn.BNet.D3.Calculator.Helpers;
using ZTnDroid.D3Calculator.Helpers;
using ZTnDroid.D3Calculator.Storage;
using Fragment = Android.Support.V4.App.Fragment;
using ZTn.BNet.D3.Calculator.Gems;
using ZTn.BNet.D3.Helpers;

namespace ZTnDroid.D3Calculator.Fragments
{
    public class GearItemEditorFragment : Fragment
    {
        LayoutInflater layoutInflater;

        #region >> Constants

        static readonly Field[] attributeFields = 
        {
            new Field(Resource.String.dexterity, "dexterityItem"),
            new Field(Resource.String.intelligence, "intelligenceItem"), 
            new Field(Resource.String.strength, "strengthItem"), 
            new Field(Resource.String.vitality , "vitalityItem")
        };
        static readonly Field[] itemDamageFields = 
        {
            new Field(Resource.String.criticChance, "critPercentBonusCapped") { percent = true },
            new Field(Resource.String.criticDamage, "critDamagePercent") { percent = true },
            new Field(Resource.String.attackSpeed, "attacksPerSecondPercent") { percent = true },
            new Field(Resource.String.arcaneMin, "damageMin_Arcane"),
            new Field(Resource.String.arcaneMax, "damageDelta_Arcane", "damageMin_Arcane"),
            new Field(Resource.String.coldMin, "damageMin_Cold"),
            new Field(Resource.String.coldMax, "damageDelta_Cold", "damageMin_Cold"),
            new Field(Resource.String.fireMin, "damageMin_Fire"),
            new Field(Resource.String.fireMax, "damageDelta_Fire", "damageMin_Fire"),
            new Field(Resource.String.holyMin, "damageMin_Holy"),
            new Field(Resource.String.holyMax, "damageDelta_Holy", "damageMin_Holy"),
            new Field(Resource.String.lightningMin, "damageMin_Lightning"),
            new Field(Resource.String.lightningMax, "damageDelta_Lightning", "damageMin_Lightning"),
            new Field(Resource.String.physicalMin, "damageMin_Physical"),
            new Field(Resource.String.physicalMax, "damageDelta_Physical", "damageMin_Physical"),
            new Field(Resource.String.poisonMin, "damageMin_Poison"),
            new Field(Resource.String.poisonMax, "damageDelta_Poison", "damageMin_Poison"),
            new Field(Resource.String.damagePercent_Arcane, "damageTypePercentBonus_Arcane") { percent = true },
            new Field(Resource.String.damagePercent_Cold, "damageTypePercentBonus_Cold") { percent = true },
            new Field(Resource.String.damagePercent_Fire, "damageTypePercentBonus_Fire") { percent = true },
            new Field(Resource.String.damagePercent_Holy, "damageTypePercentBonus_Holy") { percent = true },
            new Field(Resource.String.damagePercent_Lightning, "damageTypePercentBonus_Lightning") { percent = true },
            new Field(Resource.String.damagePercent_Physical, "damageTypePercentBonus_Physical") { percent = true },
            new Field(Resource.String.damagePercent_Poison, "damageTypePercentBonus_Poison") { percent = true },
        };
        static readonly Field[] weaponDamageFields = 
        {
            new Field(Resource.String.attacksPerSecond, "attacksPerSecondItem"),
            new Field(Resource.String.arcaneMinWeapon, "damageWeaponMin_Arcane"),
            new Field(Resource.String.arcaneMaxWeapon, "damageWeaponDelta_Arcane", "damageWeaponMin_Arcane"),
            new Field(Resource.String.coldMinWeapon, "damageWeaponMin_Cold"),
            new Field(Resource.String.coldMaxWeapon, "damageWeaponDelta_Cold", "damageWeaponMin_Cold"),
            new Field(Resource.String.fireMinWeapon, "damageWeaponMin_Fire"),
            new Field(Resource.String.fireMaxWeapon, "damageWeaponDelta_Fire", "damageWeaponMin_Fire"),
            new Field(Resource.String.holyMinWeapon, "damageWeaponMin_Holy"),
            new Field(Resource.String.holyMaxWeapon, "damageWeaponDelta_Holy", "damageWeaponMin_Holy"),
            new Field(Resource.String.lightningMinWeapon, "damageWeaponMin_Lightning"),
            new Field(Resource.String.lightningMaxWeapon, "damageWeaponDelta_Lightning", "damageWeaponMin_Lightning"),
            new Field(Resource.String.physicalMinWeapon, "damageWeaponMin_Physical"),
            new Field(Resource.String.physicalMaxWeapon, "damageWeaponDelta_Physical", "damageWeaponMin_Physical"),
            new Field(Resource.String.poisonMinWeapon, "damageWeaponMin_Poison"),
            new Field(Resource.String.poisonMaxWeapon, "damageWeaponDelta_Poison", "damageWeaponMin_Poison"),
        };
        static readonly Field[] defenseFields = 
        { 
            new Field(Resource.String.armor, "armorItem"),
            new Field(Resource.String.allResists, "resistance_All"),
            new Field(Resource.String.arcaneResist, "resistance_Arcane"),
            new Field(Resource.String.coldResist, "resistance_Cold"),
            new Field(Resource.String.fireResist, "resistance_Fire"),
            new Field(Resource.String.lightningResist, "resistance_Lightning"),
            new Field(Resource.String.physicalResist, "resistance_Physical"),
            new Field(Resource.String.poisonResist, "resistance_Poison"),
            new Field(Resource.String.life, "hitpointsPercent") { percent = true },
            new Field(Resource.String.lifeBonusPerGlobe, "healthGlobeBonusHealth"),
            new Field(Resource.String.lifeOnHit, "hitpointsOnHit"),
            new Field(Resource.String.lifeRegenPerSecond, "hitpointsRegenPerSecond"),
            new Field(Resource.String.lifeSteal, "stealHealthPercent") { percent = true },
        };
        static /*readonly*/ Field[] socketFields = 
        {
        };

        #endregion

        #region >> Fields

        List<String> attributeLabels;
        List<String> itemDamageLabels;
        List<String> defenseLabels;
        List<String> socketLabels;
        List<String> weaponDamageLabels;

        LinearLayout layoutAttributes;
        LinearLayout layoutItemDamage;
        LinearLayout layoutWeaponDamage;
        LinearLayout layoutDefense;
        LinearLayout layoutSockets;

        public static KnownGems knownGems = KnownGems.getKnownGemsFromJsonStream(D3Calc.instance.Assets.Open("d3gem.json"));

        #endregion

        #region >> Fragment

        /// <inheritdoc/>
        public override void OnCreate(Bundle savedInstanceState)
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            base.OnCreate(savedInstanceState);

            SetHasOptionsMenu(true);

            // Get inflater service
            layoutInflater = (LayoutInflater)Activity.GetSystemService(Context.LayoutInflaterService);

            // Build localized strings
            attributeLabels = attributeFields.Select(o => o.label).ToList();
            itemDamageLabels = itemDamageFields.Select(o => o.label).ToList();
            defenseLabels = defenseFields.Select(o => o.label).ToList();
            weaponDamageLabels = weaponDamageFields.Select(o => o.label).ToList();

            socketLabels = new List<string>();
            socketLabels.Add("( no gem )");
            socketLabels.AddRange(knownGems.getGemsForItem(D3Context.instance.editingItem).Select(g => g.attributes[0]));
        }

        /// <inheritdoc/>
        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            Activity.MenuInflater.Inflate(Resource.Menu.GearItemEditorFragment, menu);

            base.OnCreateOptionsMenu(menu, inflater);
        }

        /// <inheritdoc/>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            View view = inflater.Inflate(Resource.Layout.GearItemEditor, container, false);

            view.FindViewById<TextView>(Resource.Id.gearItemName).Text = D3Context.instance.editingItem.name;

            Item editingItem = D3Context.instance.editingItem;
            ItemAttributes editingItemAttr = D3Context.instance.editingItem.attributesRaw;
            editingItem.attributes = new String[] { D3Calc.Context.Resources.GetString(Resource.String.EditedItem) };

            // Attributes
            layoutAttributes = view.FindViewById<LinearLayout>(Resource.Id.layoutAttributes);
            layoutAttributes.FindViewById<ImageButton>(Resource.Id.add).Click +=
                (Object sender, EventArgs e) => layoutAttributes.AddView(createRowView(attributeLabels, attributeFields), layoutAttributes.ChildCount - 1);

            foreach (Field descriptor in attributeFields)
            {
                createAttributeRowView(descriptor);
            }

            // Damages
            layoutItemDamage = view.FindViewById<LinearLayout>(Resource.Id.layoutItemDamage);
            layoutItemDamage.FindViewById<ImageButton>(Resource.Id.add).Click +=
                (Object sender, EventArgs e) => layoutItemDamage.AddView(createRowView(itemDamageLabels, itemDamageFields), layoutItemDamage.ChildCount - 1);

            foreach (Field descriptor in itemDamageFields)
            {
                createItemDamageRowView(descriptor);
            }

            // Damage Weapons
            layoutWeaponDamage = view.FindViewById<LinearLayout>(Resource.Id.layoutWeaponDamage);
            layoutWeaponDamage.FindViewById<ImageButton>(Resource.Id.add).Click +=
                (Object sender, EventArgs e) => layoutWeaponDamage.AddView(createRowView(weaponDamageLabels, weaponDamageFields), layoutWeaponDamage.ChildCount - 1);

            foreach (Field descriptor in weaponDamageFields)
            {
                createWeaponDamageRowView(descriptor);
            }

            // Defense
            layoutDefense = view.FindViewById<LinearLayout>(Resource.Id.layoutDefense);
            layoutDefense.FindViewById<ImageButton>(Resource.Id.add).Click +=
                (Object sender, EventArgs e) => layoutDefense.AddView(createRowView(defenseLabels, defenseFields), layoutDefense.ChildCount - 1);

            foreach (Field descriptor in defenseFields)
            {
                createDefenseRowView(descriptor);
            }

            // Sockets
            layoutSockets = view.FindViewById<LinearLayout>(Resource.Id.layoutSockets);
            layoutSockets.FindViewById<ImageButton>(Resource.Id.add).Click +=
                (Object sender, EventArgs e) => layoutSockets.AddView(createSocketRowView(socketLabels, knownGems.getGemsForItem(D3Context.instance.editingItem)), layoutSockets.ChildCount - 1);

            SocketedGem[] itemGems = D3Context.instance.editingItem.gems;
            if (itemGems != null)
            {
                foreach (SocketedGem socketedGem in itemGems)
                {
                    layoutSockets.AddView(createSocketRowView(socketLabels, knownGems.getGemsForItem(D3Context.instance.editingItem), socketedGem.item), layoutSockets.ChildCount - 1);
                }
            }

            return view;
        }

        /// <inheritdoc/>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            switch (item.ItemId)
            {
                case Resource.Id.CancelEditedItem:
                    this.Activity.SetResult(Result.Canceled, new Intent());
                    this.Activity.Finish();
                    return true;

                case Resource.Id.SubmitEditedItem:
                    D3Context.instance.editingItem.attributesRaw = new ItemAttributes();
                    updateEditedItem(layoutAttributes);
                    updateEditedItem(layoutDefense);
                    updateEditedItem(layoutItemDamage);
                    updateEditedItem(layoutWeaponDamage);
                    updateEditedItemSockets(layoutSockets);
                    D3Context.instance.editingItem.UpdateStats();
                    this.Activity.SetResult(Result.Ok, new Intent());
                    this.Activity.Finish();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        #endregion

        #region >> Inner Classes

        class Field
        {
            public int id;
            public string targetAttribute;
            public string refAttribute;
            public string label;
            public bool percent = false;

            public Field(int id, string attribute)
                : this(id, attribute, null)
            {
            }

            public Field(int id, string targetAttribute, string refAttribute)
            {
                this.id = id;
                this.targetAttribute = targetAttribute;
                this.refAttribute = refAttribute;
                this.label = D3Calc.Context.Resources.GetString(id);
            }

            public ItemValueRange getValue(ItemAttributes attributes)
            {
                ItemValueRange value;

                if (refAttribute == null)
                    value = attributes.getAttributeByName(targetAttribute);
                else
                    value = attributes.getAttributeByName(targetAttribute) + attributes.getAttributeByName(refAttribute);

                if (percent)
                    value *= 100;
                return value;
            }

            public void setValue(ItemAttributes attributes, double value)
            {
                if (percent)
                    value /= 100;

                if (refAttribute == null)
                    attributes.setAttributeByName(targetAttribute, new ItemValueRange(value));
                else
                    attributes.setAttributeByName(targetAttribute, new ItemValueRange(value) - attributes.getAttributeByName(refAttribute));
            }

            #region >> Object

            /// <inheritdoc />
            public override string ToString()
            {
                return "[id:" + id + " attribute:" + targetAttribute + " label:" + label + "]";
            }

            #endregion
        }

        #endregion

        void createAttributeRowView(Field field)
        {
            ItemValueRange value = field.getValue(D3Context.instance.editingItem.attributesRaw);
            if (value != null && value.min != 0)
                layoutAttributes.AddView(createRowView(attributeLabels, attributeFields, field, value), layoutAttributes.ChildCount - 1);
        }

        void createItemDamageRowView(Field field)
        {
            ItemValueRange value = field.getValue(D3Context.instance.editingItem.attributesRaw);
            if (value != null && value.min != 0)
                layoutItemDamage.AddView(createRowView(itemDamageLabels, itemDamageFields, field, value), layoutItemDamage.ChildCount - 1);
        }

        void createWeaponDamageRowView(Field field)
        {
            ItemValueRange value = field.getValue(D3Context.instance.editingItem.attributesRaw);
            if (value != null && value.min != 0)
                layoutWeaponDamage.AddView(createRowView(weaponDamageLabels, weaponDamageFields, field, value), layoutWeaponDamage.ChildCount - 1);
        }

        void createDefenseRowView(Field field)
        {
            ItemValueRange value = field.getValue(D3Context.instance.editingItem.attributesRaw);
            if (value != null && value.min != 0)
                layoutDefense.AddView(createRowView(defenseLabels, defenseFields, field, value), layoutDefense.ChildCount - 1);
        }

        void createRowView(LinearLayout layout, List<string> labels, Field[] fields, Field selected, ItemValueRange value)
        {
            if (value != null && value.min != 0)
                layout.AddView(createRowView(labels, fields, selected, value), layout.ChildCount - 1);
        }

        View createRowView(List<String> labels, Field[] fields)
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

            Spinner spinner = rowView.FindViewById<Spinner>(Resource.Id.attributeName);
            spinner.Adapter = new ArrayAdapter<String>(Activity, Android.Resource.Layout.SimpleSpinnerItem, labels);
            spinner.ItemSelected +=
                (Object sender, AdapterView.ItemSelectedEventArgs e) =>
                {
                    ((View)((Spinner)sender).Parent).Tag = new JavaLangObject<Field>(fields[e.Position]);
                };

            return rowView;
        }

        View createRowView(List<String> labels, Field[] fields, Field selected, ItemValueRange value)
        {
            View rowView = createRowView(labels, fields);

            rowView.Tag = new JavaLangObject<Field>(selected);

            int index = 0;
            for (; index < fields.Length; index++)
                if (fields[index].id == selected.id)
                    break;

            if (index != fields.Length)
            {
                rowView.FindViewById<Spinner>(Resource.Id.attributeName).SetSelection(index);
                rowView.FindViewById<EditText>(Resource.Id.attributeValue).Text = value.min.ToString();
            }

            return rowView;
        }

        View createSocketRowView(List<String> labels, List<Item> gems)
        {
            // Get the view from inflater
            View rowView = layoutInflater.Inflate(Resource.Layout.GearSocketRowEditor, null);

            // Set "remove row" action
            rowView.FindViewById<ImageButton>(Resource.Id.remove).Click +=
                (Object sender, EventArgs e) =>
                {
                    ImageButton removeButton = (ImageButton)sender;
                    View parent = (View)removeButton.Parent;
                    LinearLayout parentContainer = (LinearLayout)parent.Parent;
                    parentContainer.RemoveView(parent);
                };

            Spinner spinner = rowView.FindViewById<Spinner>(Resource.Id.socketLabel);
            spinner.Adapter = new ArrayAdapter<String>(Activity, Android.Resource.Layout.SimpleSpinnerItem, labels);
            spinner.ItemSelected +=
                (Object sender, AdapterView.ItemSelectedEventArgs e) =>
                {
                    Console.WriteLine("OK: " + gems[e.Position]);
                    // Note: spinner has the additional "no gem" choice
                    if (e.Position >= 1)
                        ((View)((Spinner)sender).Parent).Tag = new JavaLangObject<Item>(gems[e.Position - 1]);
                    else
                        ((View)((Spinner)sender).Parent).Tag = null;
                };

            return rowView;
        }

        View createSocketRowView(List<String> labels, List<Item> gems, ItemSummary selected)
        {
            View rowView = createSocketRowView(labels, gems);

            int index = 0;
            for (; index < gems.Count; index++)
                if (gems[index].id == selected.id)
                    break;

            if (index != gems.Count)
            {
                // Note: spinner has the additional "no gem" choice
                rowView.FindViewById<Spinner>(Resource.Id.socketLabel).SetSelection(index + 1);
            }

            return rowView;
        }

        void updateEditedItem(LinearLayout layout)
        {
            String numberDecimalSeparator = System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
            ItemAttributes attr = new ItemAttributes();
            for (int index = 0; index < layout.ChildCount; index++)
            {
                View view = layout.GetChildAt(index);
                EditText editText = view.FindViewById<EditText>(Resource.Id.attributeValue);
                if (editText != null)
                {
                    JavaLangObject<Field> tag = (JavaLangObject<Field>)(view.Tag);
                    Console.WriteLine(tag.value + " = " + editText.Text);

                    string text;
                    if (numberDecimalSeparator == ",")
                        text = editText.Text.Replace(".", numberDecimalSeparator);
                    else
                        text = editText.Text.Replace(",", numberDecimalSeparator);

                    double value;
                    if (Double.TryParse(text, out value))
                    {
                        tag.value.setValue(attr, value);
                    }
                }
            }
            D3Context.instance.editingItem.attributesRaw += attr;
        }

        void updateEditedItemSockets(LinearLayout layout)
        {
            List<SocketedGem> editedGems = new List<SocketedGem>();
            for (int index = 0; index < layout.ChildCount; index++)
            {
                View view = layout.GetChildAt(index);
                if (view.FindViewById<Spinner>(Resource.Id.socketLabel) != null)
                {
                    JavaLangObject<Item> tag = (JavaLangObject<Item>)(view.Tag);
                    if (tag != null)
                    {
                        Console.WriteLine(tag.value);
                        editedGems.Add(new SocketedGem(tag.value));
                    }
                }
            }
            D3Context.instance.editingItem.gems = editedGems.ToArray();
        }
    }
}