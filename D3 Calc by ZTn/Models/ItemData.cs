using System.Linq;
using Xamarin.Forms;
using ZTn.BNet.D3.Items;
using ZTn.Pcl.D3Calculator.Resources;

namespace ZTn.Pcl.D3Calculator.Models
{
    internal class ItemData : IControlData
    {
        public string Armor => Stringify(Item?.Armor);

        public Color Color => Colorify(Item?.DisplayColor);

        public string Dps => Stringify(Item?.Dps);

        public bool HasPrimaryAttributes => Item?.Attributes?.Primary != null && Item.Attributes.Primary.Any();
        public bool HasPrimary1 => Item?.Attributes?.Primary != null && Item.Attributes.Primary.Length >= 1;
        public bool HasPrimary2 => Item?.Attributes?.Primary != null && Item.Attributes.Primary.Length >= 2;
        public bool HasPrimary3 => Item?.Attributes?.Primary != null && Item.Attributes.Primary.Length >= 3;
        public bool HasPrimary4 => Item?.Attributes?.Primary != null && Item.Attributes.Primary.Length >= 4;
        public bool HasPrimary5 => Item?.Attributes?.Primary != null && Item.Attributes.Primary.Length >= 5;
        public bool HasPrimary6 => Item?.Attributes?.Primary != null && Item.Attributes.Primary.Length >= 6;

        public bool HasSecondaryAttributes => Item?.Attributes?.Secondary != null && Item.Attributes.Secondary.Any();
        public bool HasSecondary1 => Item?.Attributes?.Secondary != null && Item.Attributes.Secondary.Length >= 1;
        public bool HasSecondary2 => Item?.Attributes?.Secondary != null && Item.Attributes.Secondary.Length >= 2;
        public bool HasSecondary3 => Item?.Attributes?.Secondary != null && Item.Attributes.Secondary.Length >= 3;
        public bool HasSecondary4 => Item?.Attributes?.Secondary != null && Item.Attributes.Secondary.Length >= 4;
        public bool HasSecondary5 => Item?.Attributes?.Secondary != null && Item.Attributes.Secondary.Length >= 5;
        public bool HasSecondary6 => Item?.Attributes?.Secondary != null && Item.Attributes.Secondary.Length >= 6;

        public bool HasPassiveAttributes => Item?.Attributes?.Passive != null && Item.Attributes.Passive.Any();
        public bool HasPassive1 => Item?.Attributes?.Passive != null && Item.Attributes.Passive.Length >= 1;
        public bool HasPassive2 => Item?.Attributes?.Passive != null && Item.Attributes.Passive.Length >= 2;

        public ImageSource Icon => ImageSource.FromFile("icon.png");

        public bool IsArmor => Item?.Armor != null;

        public bool IsWeapon => Item?.Dps != null;

        public Item Item { get; }

        public ItemPosition Position { get; }

        public string Passive1 => HasPassive1 ? Item.Attributes.Passive[0].Text : "";
        public string Passive2 => HasPassive2 ? Item.Attributes.Passive[1].Text : "";

        public string Primary1 => HasPrimary1 ? Item.Attributes.Primary[0].Text : "";
        public string Primary2 => HasPrimary2 ? Item.Attributes.Primary[1].Text : "";
        public string Primary3 => HasPrimary3 ? Item.Attributes.Primary[2].Text : "";
        public string Primary4 => HasPrimary4 ? Item.Attributes.Primary[3].Text : "";
        public string Primary5 => HasPrimary5 ? Item.Attributes.Primary[4].Text : "";
        public string Primary6 => HasPrimary6 ? Item.Attributes.Primary[5].Text : "";

        public string Secondary1 => HasSecondary1 ? Item.Attributes.Secondary[0].Text : "";
        public string Secondary2 => HasSecondary2 ? Item.Attributes.Secondary[1].Text : "";
        public string Secondary3 => HasSecondary3 ? Item.Attributes.Secondary[2].Text : "";
        public string Secondary4 => HasSecondary4 ? Item.Attributes.Secondary[3].Text : "";
        public string Secondary5 => HasSecondary5 ? Item.Attributes.Secondary[4].Text : "";
        public string Secondary6 => HasSecondary6 ? Item.Attributes.Secondary[5].Text : "";

        public Color Passive1Color => HasPassive1 ? Colorify(Item.Attributes.Passive[0].Color) : Colors.DefaultText;
        public Color Passive2Color => HasPassive2 ? Colorify(Item.Attributes.Passive[1].Color) : Colors.DefaultText;

        public Color Primary1Color => HasPrimary1 ? Colorify(Item.Attributes.Primary[0].Color) : Colors.DefaultText;
        public Color Primary2Color => HasPrimary2 ? Colorify(Item.Attributes.Primary[1].Color) : Colors.DefaultText;
        public Color Primary3Color => HasPrimary3 ? Colorify(Item.Attributes.Primary[2].Color) : Colors.DefaultText;
        public Color Primary4Color => HasPrimary4 ? Colorify(Item.Attributes.Primary[3].Color) : Colors.DefaultText;
        public Color Primary5Color => HasPrimary5 ? Colorify(Item.Attributes.Primary[4].Color) : Colors.DefaultText;
        public Color Primary6Color => HasPrimary6 ? Colorify(Item.Attributes.Primary[5].Color) : Colors.DefaultText;

        public Color Secondary1Color => HasSecondary1 ? Colorify(Item.Attributes.Secondary[0].Color) : Colors.DefaultText;
        public Color Secondary2Color => HasSecondary2 ? Colorify(Item.Attributes.Secondary[1].Color) : Colors.DefaultText;
        public Color Secondary3Color => HasSecondary3 ? Colorify(Item.Attributes.Secondary[2].Color) : Colors.DefaultText;
        public Color Secondary4Color => HasSecondary4 ? Colorify(Item.Attributes.Secondary[3].Color) : Colors.DefaultText;
        public Color Secondary5Color => HasSecondary5 ? Colorify(Item.Attributes.Secondary[4].Color) : Colors.DefaultText;
        public Color Secondary6Color => HasSecondary6 ? Colorify(Item.Attributes.Secondary[5].Color) : Colors.DefaultText;

        #region >> Constructors

        public ItemData(Item itemItem, ItemPosition position)
        {
            Position = position;
            Item = itemItem;
        }

        #endregion

        private static Color Colorify(string stringColor)
        {
            switch (stringColor)
            {
                case "orange":
                    return Colors.Legendary;
                case "blue":
                    return Colors.Magic;
                case "green":
                    return Colors.Set;
                case "yellow":
                    return Colors.Rare;
                case "white":
                    return Colors.Trash;
                case "gray":
                    return Colors.Normal;
                default:
                    return Colors.Normal;
            }
        }

        private static string Stringify(ItemValueRange value)
        {
            if (value == null)
            {
                return "";
            }

            if (value.Min == value.Max)
            {
                return $"{value.Min:N0}";
            }

            return $"{value}";
        }
    }
}
