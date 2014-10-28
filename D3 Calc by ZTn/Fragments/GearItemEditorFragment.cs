using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using ZTn.BNet.D3.Calculator.Gems;
using ZTn.BNet.D3.Calculator.Helpers;
using ZTn.BNet.D3.Items;
using ZTnDroid.D3Calculator.Helpers;
using ZTnDroid.D3Calculator.Storage;
using Fragment = Android.Support.V4.App.Fragment;

namespace ZTnDroid.D3Calculator.Fragments
{
    public class GearItemEditorFragment : Fragment
    {
        private LayoutInflater layoutInflater;

        #region >> Constants

        private static readonly Field[] AttributeFields =
        {
            new Field(Resource.String.dexterity, "dexterityItem"),
            new Field(Resource.String.intelligence, "intelligenceItem"),
            new Field(Resource.String.strength, "strengthItem"),
            new Field(Resource.String.vitality, "vitalityItem")
        };

        private static readonly Field[] ItemDamageFields =
        {
            new Field(Resource.String.criticChance, "critPercentBonusCapped") { Percent = true },
            new Field(Resource.String.criticDamage, "critDamagePercent") { Percent = true },
            new Field(Resource.String.attackSpeed, "attacksPerSecondPercent") { Percent = true },
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
            new Field(Resource.String.damagePercent_Arcane, "damageTypePercentBonus_Arcane") { Percent = true },
            new Field(Resource.String.damagePercent_Cold, "damageTypePercentBonus_Cold") { Percent = true },
            new Field(Resource.String.damagePercent_Fire, "damageTypePercentBonus_Fire") { Percent = true },
            new Field(Resource.String.damagePercent_Holy, "damageTypePercentBonus_Holy") { Percent = true },
            new Field(Resource.String.damagePercent_Lightning, "damageTypePercentBonus_Lightning") { Percent = true },
            new Field(Resource.String.damagePercent_Physical, "damageTypePercentBonus_Physical") { Percent = true },
            new Field(Resource.String.damagePercent_Poison, "damageTypePercentBonus_Poison") { Percent = true }
        };

        private static readonly Field[] WeaponDamageFields =
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
            new Field(Resource.String.weaponPercent_Arcane, "damageWeaponPercentBonus_Arcane"),
            new Field(Resource.String.weaponPercent_Cold, "damageWeaponPercentBonus_Cold"),
            new Field(Resource.String.weaponPercent_Fire, "damageWeaponPercentBonus_Fire"),
            new Field(Resource.String.weaponPercent_Holy, "damageWeaponPercentBonus_Holy"),
            new Field(Resource.String.weaponPercent_Lightning, "damageWeaponPercentBonus_Lightning"),
            new Field(Resource.String.weaponPercent_Physical, "damageWeaponPercentBonus_Physical"),
            new Field(Resource.String.weaponPercent_Poison, "damageWeaponPercentBonus_Poison")
        };

        private static readonly Field[] SkillBonusFields =
        {
            new Field(Resource.String.damageDealtPercent_Arcane, "damageDealtPercentBonusArcane") { Percent = true },
            new Field(Resource.String.damageDealtPercent_Cold, "damageDealtPercentBonusCold") { Percent = true },
            new Field(Resource.String.damageDealtPercent_Fire, "damageDealtPercentBonusFire") { Percent = true },
            new Field(Resource.String.damageDealtPercent_Holy, "damageDealtPercentBonusHoly") { Percent = true },
            new Field(Resource.String.damageDealtPercent_Lightning, "damageDealtPercentBonusLightning") { Percent = true },
            new Field(Resource.String.damageDealtPercent_Physical, "damageDealtPercentBonusPhysical") { Percent = true },
            new Field(Resource.String.damageDealtPercent_Poison, "damageDealtPercentBonusPoison") { Percent = true },
            new Field(Resource.String.damagePercentBonusVsElites, "damagePercentBonusVsElites") { Percent = true },
            new Field(Resource.String.powerCooldownReductionPercentAll, "powerCooldownReductionPercentAll") { Percent = true }
        };

        private static readonly Field[] DefenseFields =
        {
            new Field(Resource.String.armor, "armorItem"),
            new Field(Resource.String.allResists, "resistance_All"),
            new Field(Resource.String.arcaneResist, "resistance_Arcane"),
            new Field(Resource.String.coldResist, "resistance_Cold"),
            new Field(Resource.String.fireResist, "resistance_Fire"),
            new Field(Resource.String.lightningResist, "resistance_Lightning"),
            new Field(Resource.String.physicalResist, "resistance_Physical"),
            new Field(Resource.String.poisonResist, "resistance_Poison"),
            new Field(Resource.String.life, "hitpointsPercent") { Percent = true },
            new Field(Resource.String.lifeBonusPerGlobe, "healthGlobeBonusHealth"),
            new Field(Resource.String.lifeOnHit, "hitpointsOnHit"),
            new Field(Resource.String.lifeRegenPerSecond, "hitpointsRegenPerSecond"),
            new Field(Resource.String.lifeSteal, "stealHealthPercent") { Percent = true }
        };

        #endregion

        #region >> Fields

        private List<String> attributeLabels;
        private List<String> itemDamageLabels;
        private List<String> defenseLabels;
        private List<String> socketLabels;
        private List<String> weaponDamageLabels;
        private List<String> skillBonusLabels;

        private LinearLayout layoutAttributes;
        private LinearLayout layoutItemDamage;
        private LinearLayout layoutWeaponDamage;
        private LinearLayout layoutDefense;
        private LinearLayout layoutSkillBonus;
        private LinearLayout layoutSockets;

        public static KnownGems KnownGems = KnownGems.GetKnownGemsFromJsonStream(D3Calc.Instance.Assets.Open("d3gem.json"));

        #endregion

        #region >> Fragment

        /// <inheritdoc/>
        public override void OnCreate(Bundle savedInstanceState)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            base.OnCreate(savedInstanceState);

            HasOptionsMenu = true;

            // Get inflater service
            layoutInflater = (LayoutInflater)Activity.GetSystemService(Context.LayoutInflaterService);

            // Build localized strings
            attributeLabels = AttributeFields.Select(o => o.Label)
                .ToList();
            itemDamageLabels = ItemDamageFields.Select(o => o.Label)
                .ToList();
            defenseLabels = DefenseFields.Select(o => o.Label)
                .ToList();
            weaponDamageLabels = WeaponDamageFields.Select(o => o.Label)
                .ToList();
            skillBonusLabels = SkillBonusFields.Select(o => o.Label)
                .ToList();

            socketLabels = new List<string> { "( no gem )" };
            foreach (var gem in KnownGems.GetGemsForItem(D3Context.Instance.EditingItem))
            {
                var text = string.Empty;
                if (gem.Attributes.Primary != null && gem.Attributes.Primary.Any())
                {
                    text = gem.Attributes.Primary[0].Text;
                }
                if (gem.Attributes.Secondary != null && gem.Attributes.Secondary.Any())
                {
                    text = gem.Attributes.Secondary[0].Text;
                }
                if (gem.Attributes.Passive != null && gem.Attributes.Passive.Any())
                {
                    text = gem.Attributes.Passive[0].Text;
                }
                socketLabels.Add(text);
            }
        }

        /// <inheritdoc/>
        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            Activity.MenuInflater.Inflate(Resource.Menu.GearItemEditorFragment, menu);

            base.OnCreateOptionsMenu(menu, inflater);
        }

        /// <inheritdoc/>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            var view = inflater.Inflate(Resource.Layout.GearItemEditor, container, false);

            view.FindViewById<TextView>(Resource.Id.gearItemName)
                .Text = D3Context.Instance.EditingItem.Name;

            var editingItem = D3Context.Instance.EditingItem;
            editingItem.Attributes = new ItemTextAttributes { Primary = new[] { new ItemTextAttribute { Text = Application.Context.Resources.GetString(Resource.String.EditedItem) } } };

            // Attributes
            layoutAttributes = view.FindViewById<LinearLayout>(Resource.Id.layoutAttributes);
            layoutAttributes.FindViewById<ImageButton>(Resource.Id.add)
                .Click += (sender, e) => layoutAttributes.AddView(CreateRowView(attributeLabels, AttributeFields), layoutAttributes.ChildCount - 1);

            foreach (var descriptor in AttributeFields)
            {
                CreateAttributeRowView(descriptor);
            }

            // Damages
            layoutItemDamage = view.FindViewById<LinearLayout>(Resource.Id.layoutItemDamage);
            layoutItemDamage.FindViewById<ImageButton>(Resource.Id.add)
                .Click += (sender, e) => layoutItemDamage.AddView(CreateRowView(itemDamageLabels, ItemDamageFields), layoutItemDamage.ChildCount - 1);

            foreach (var descriptor in ItemDamageFields)
            {
                CreateItemDamageRowView(descriptor);
            }

            // Damage Weapons
            layoutWeaponDamage = view.FindViewById<LinearLayout>(Resource.Id.layoutWeaponDamage);
            layoutWeaponDamage.FindViewById<ImageButton>(Resource.Id.add)
                .Click += (sender, e) => layoutWeaponDamage.AddView(CreateRowView(weaponDamageLabels, WeaponDamageFields), layoutWeaponDamage.ChildCount - 1);

            foreach (var descriptor in WeaponDamageFields)
            {
                CreateWeaponDamageRowView(descriptor);
            }

            // Defense
            layoutDefense = view.FindViewById<LinearLayout>(Resource.Id.layoutDefense);
            layoutDefense.FindViewById<ImageButton>(Resource.Id.add)
                .Click += (sender, e) => layoutDefense.AddView(CreateRowView(defenseLabels, DefenseFields), layoutDefense.ChildCount - 1);

            foreach (var descriptor in DefenseFields)
            {
                CreateDefenseRowView(descriptor);
            }

            // Skills
            layoutSkillBonus = view.FindViewById<LinearLayout>(Resource.Id.layoutSkillBonus);
            layoutSkillBonus.FindViewById<ImageButton>(Resource.Id.add)
                .Click += (sender, e) => layoutSkillBonus.AddView(CreateRowView(skillBonusLabels, SkillBonusFields), layoutSkillBonus.ChildCount - 1);

            foreach (var descriptor in SkillBonusFields)
            {
                CreateSkillBonusRowView(descriptor);
            }

            // Sockets
            layoutSockets = view.FindViewById<LinearLayout>(Resource.Id.layoutSockets);
            layoutSockets.FindViewById<ImageButton>(Resource.Id.add)
                .Click += (sender, e) => layoutSockets.AddView(CreateSocketRowView(socketLabels, KnownGems.GetGemsForItem(D3Context.Instance.EditingItem)), layoutSockets.ChildCount - 1);

            var itemGems = D3Context.Instance.EditingItem.Gems;
            if (itemGems != null)
            {
                foreach (var socketedGem in itemGems)
                {
                    layoutSockets.AddView(CreateSocketRowView(socketLabels, KnownGems.GetGemsForItem(D3Context.Instance.EditingItem), socketedGem.Item), layoutSockets.ChildCount - 1);
                }
            }

            return view;
        }

        /// <inheritdoc/>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            switch (item.ItemId)
            {
                case Resource.Id.CancelEditedItem:
                    Activity.SetResult(Result.Canceled, new Intent());
                    Activity.Finish();
                    return true;

                case Resource.Id.SubmitEditedItem:
                    D3Context.Instance.EditingItem.AttributesRaw = new ItemAttributes();
                    UpdateEditedItem(layoutAttributes);
                    UpdateEditedItem(layoutDefense);
                    UpdateEditedItem(layoutItemDamage);
                    UpdateEditedItem(layoutWeaponDamage);
                    UpdateEditedItem(layoutSkillBonus);
                    UpdateEditedItemSockets(layoutSockets);
                    D3Context.Instance.EditingItem.UpdateStats();
                    Activity.SetResult(Result.Ok, new Intent());
                    Activity.Finish();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        #endregion

        #region >> Inner Classes

        private class Field
        {
            public readonly int Id;
            private readonly string targetAttribute;
            private readonly string refAttribute;
            public readonly string Label;
            public bool Percent;

            public Field(int id, string attribute)
                : this(id, attribute, null)
            {
            }

            public Field(int id, string targetAttribute, string refAttribute)
            {
                Percent = false;
                Id = id;
                this.targetAttribute = targetAttribute;
                this.refAttribute = refAttribute;
                Label = Application.Context.Resources.GetString(id);
            }

            public ItemValueRange GetValue(ItemAttributes attributes)
            {
                ItemValueRange value;

                if (refAttribute == null)
                {
                    value = attributes.GetAttributeByName(targetAttribute);
                }
                else
                {
                    value = attributes.GetAttributeByName(targetAttribute) + attributes.GetAttributeByName(refAttribute);
                }

                if (Percent)
                {
                    value *= 100;
                }
                return value;
            }

            public void SetValue(ItemAttributes attributes, double value)
            {
                if (Percent)
                {
                    value /= 100;
                }

                if (refAttribute == null)
                {
                    attributes.SetAttributeByName(targetAttribute, new ItemValueRange(value));
                }
                else
                {
                    attributes.SetAttributeByName(targetAttribute, new ItemValueRange(value) - attributes.GetAttributeByName(refAttribute));
                }
            }

            #region >> Object

            /// <inheritdoc />
            public override string ToString()
            {
                return "[Id:" + Id + " attribute:" + targetAttribute + " label:" + Label + "]";
            }

            #endregion
        }

        #endregion

        private void CreateAttributeRowView(Field field)
        {
            var value = field.GetValue(D3Context.Instance.EditingItem.AttributesRaw);
            if (value == null || value.IsZero())
            {
                return;
            }

            layoutAttributes.AddView(CreateRowView(attributeLabels, AttributeFields, field, value), layoutAttributes.ChildCount - 1);
        }

        private void CreateItemDamageRowView(Field field)
        {
            var value = field.GetValue(D3Context.Instance.EditingItem.AttributesRaw);
            if (value == null || value.IsZero())
            {
                return;
            }

            layoutItemDamage.AddView(CreateRowView(itemDamageLabels, ItemDamageFields, field, value), layoutItemDamage.ChildCount - 1);
        }

        private void CreateWeaponDamageRowView(Field field)
        {
            var value = field.GetValue(D3Context.Instance.EditingItem.AttributesRaw);
            if (value == null || value.IsZero())
            {
                return;
            }

            layoutWeaponDamage.AddView(CreateRowView(weaponDamageLabels, WeaponDamageFields, field, value), layoutWeaponDamage.ChildCount - 1);
        }

        private void CreateDefenseRowView(Field field)
        {
            var value = field.GetValue(D3Context.Instance.EditingItem.AttributesRaw);
            if (value == null || value.IsZero())
            {
                return;
            }

            layoutDefense.AddView(CreateRowView(defenseLabels, DefenseFields, field, value), layoutDefense.ChildCount - 1);
        }

        private void CreateSkillBonusRowView(Field field)
        {
            var value = field.GetValue(D3Context.Instance.EditingItem.AttributesRaw);
            if (value == null || value.IsZero())
            {
                return;
            }

            layoutSkillBonus.AddView(CreateRowView(skillBonusLabels, SkillBonusFields, field, value), layoutSkillBonus.ChildCount - 1);
        }

        private View CreateRowView(List<String> labels, Field[] fields)
        {
            // Get the view from inflater
            var rowView = layoutInflater.Inflate(Resource.Layout.GearItemRowEditor, null);

            // Set "remove row" action
            rowView.FindViewById<ImageButton>(Resource.Id.remove)
                .Click += (sender, e) =>
                {
                    var removeButton = (ImageButton)sender;
                    var parent = (View)removeButton.Parent;
                    var parentContainer = (LinearLayout)parent.Parent;
                    parentContainer.RemoveView(parent);
                };

            var spinner = rowView.FindViewById<Spinner>(Resource.Id.attributeName);
            spinner.Adapter = new ArrayAdapter<String>(Activity, Android.Resource.Layout.SimpleSpinnerItem, labels);
            spinner.ItemSelected += (sender, e) => { ((View)((Spinner)sender).Parent).Tag = new JavaLangObject<Field>(fields[e.Position]); };

            return rowView;
        }

        private View CreateRowView(List<String> labels, Field[] fields, Field selected, ItemValueRange value)
        {
            var rowView = CreateRowView(labels, fields);

            rowView.Tag = new JavaLangObject<Field>(selected);

            var index = 0;
            for (; index < fields.Length; index++)
            {
                if (fields[index].Id == selected.Id)
                {
                    break;
                }
            }

            if (index != fields.Length)
            {
                rowView.FindViewById<Spinner>(Resource.Id.attributeName)
                    .SetSelection(index);
                rowView.FindViewById<EditText>(Resource.Id.attributeValue)
                    .Text = value.Min.ToString();
            }

            return rowView;
        }

        private View CreateSocketRowView(List<String> labels, List<Item> gems)
        {
            // Get the view from inflater
            var rowView = layoutInflater.Inflate(Resource.Layout.GearSocketRowEditor, null);

            // Set "remove row" action
            rowView.FindViewById<ImageButton>(Resource.Id.remove)
                .Click +=
                (sender, e) =>
                {
                    var removeButton = (ImageButton)sender;
                    var parent = (View)removeButton.Parent;
                    var parentContainer = (LinearLayout)parent.Parent;
                    parentContainer.RemoveView(parent);
                };

            var spinner = rowView.FindViewById<Spinner>(Resource.Id.socketLabel);
            spinner.Adapter = new ArrayAdapter<String>(Activity, Android.Resource.Layout.SimpleSpinnerItem, labels);
            spinner.ItemSelected +=
                (sender, e) =>
                {
                    // Note: spinner has the additional "no gem" choice
                    ((View)((Spinner)sender).Parent).Tag = (e.Position >= 1 ? new JavaLangObject<Item>(gems[e.Position - 1]) : null);
                };

            return rowView;
        }

        private View CreateSocketRowView(List<String> labels, List<Item> gems, ItemSummary selected)
        {
            var rowView = CreateSocketRowView(labels, gems);

            var index = 0;
            for (; index < gems.Count; index++)
            {
                if (gems[index].Id == selected.Id)
                {
                    break;
                }
            }

            if (index != gems.Count)
            {
                // Note: spinner has the additional "no gem" choice
                rowView.FindViewById<Spinner>(Resource.Id.socketLabel)
                    .SetSelection(index + 1);
            }

            return rowView;
        }

        private static void UpdateEditedItem(ViewGroup layout)
        {
            var numberDecimalSeparator = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
            var attr = new ItemAttributes();
            for (var index = 0; index < layout.ChildCount; index++)
            {
                var view = layout.GetChildAt(index);
                var editText = view.FindViewById<EditText>(Resource.Id.attributeValue);
                if (editText != null)
                {
                    var tag = (JavaLangObject<Field>)(view.Tag);
                    Console.WriteLine(tag.Value + " = " + editText.Text);

                    var text = editText.Text.Replace(numberDecimalSeparator == "," ? "." : ",", numberDecimalSeparator);

                    double value;
                    if (Double.TryParse(text, out value))
                    {
                        tag.Value.SetValue(attr, value);
                    }
                }
            }
            D3Context.Instance.EditingItem.AttributesRaw += attr;
        }

        private static void UpdateEditedItemSockets(ViewGroup layout)
        {
            var editedGems = new List<SocketedGem>();
            for (var index = 0; index < layout.ChildCount; index++)
            {
                var view = layout.GetChildAt(index);
                if (view.FindViewById<Spinner>(Resource.Id.socketLabel) != null)
                {
                    var tag = (JavaLangObject<Item>)(view.Tag);
                    if (tag != null)
                    {
                        Console.WriteLine(tag.Value);
                        editedGems.Add(new SocketedGem(tag.Value));
                    }
                }
            }
            D3Context.Instance.EditingItem.Gems = editedGems.ToArray();
        }
    }
}