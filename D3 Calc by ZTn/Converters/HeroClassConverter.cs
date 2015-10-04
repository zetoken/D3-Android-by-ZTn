using System;
using System.Globalization;
using Xamarin.Forms;
using ZTn.BNet.D3.Heroes;
using ZTn.Pcl.D3Calculator.Resources;

namespace ZTn.Pcl.D3Calculator.Converters
{
    public class HeroClassConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var heroClass = (HeroClass)value;

            switch (heroClass)
            {
                case HeroClass.Barbarian:
                    return Lang.Barbarian;
                case HeroClass.Crusader:
                    return Lang.Crusader;
                case HeroClass.DemonHunter:
                    return Lang.DemonHunter;
                case HeroClass.Monk:
                    return Lang.Monk;
                case HeroClass.WitchDoctor:
                    return Lang.WitchDoctor;
                case HeroClass.Wizard:
                    return Lang.Wizard;
                default:
                    return Lang.Unknown;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
