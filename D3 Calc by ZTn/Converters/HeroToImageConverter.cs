using System;
using System.Globalization;
using Xamarin.Forms;
using ZTn.BNet.D3.Heroes;

namespace ZTn.Pcl.D3Calculator.Converters
{
    public class HeroToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var hero = value as HeroSummary;

            if (hero == null)
            {
                return null;
            }

            switch (hero.HeroClass)
            {
                case HeroClass.Barbarian:
                    return (hero.Gender == HeroGender.Male ? "barbarian_male.png" : "barbarian_female.png");
                case HeroClass.Crusader:
                    return (hero.Gender == HeroGender.Male ? "crusader_male.png" : "crusader_female.png");
                case HeroClass.DemonHunter:
                    return (hero.Gender == HeroGender.Male ? "demonhunter_male.png" : "demonhunter_female.png");
                case HeroClass.Monk:
                    return (hero.Gender == HeroGender.Male ? "monk_male.png" : "monk_female.png");
                case HeroClass.WitchDoctor:
                    return (hero.Gender == HeroGender.Male ? "witchdoctor_male.png" : "witchdoctor_female.png");
                case HeroClass.Wizard:
                    return (hero.Gender == HeroGender.Male ? "wizard_male.png" : "wizard_female.png");
                default:
                    return "icon.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
