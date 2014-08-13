using ZTn.BNet.D3.Heroes;

namespace ZTnDroid.D3Calculator.Helpers
{
    static class HeroClassExtension
    {
        public static string Translate(this HeroClass heroClass)
        {
            switch (heroClass)
            {
                case HeroClass.Barbarian:
                    return D3Calc.Instance.Resources.GetString(Resource.String.barbarian);
                case HeroClass.Crusader:
                    return D3Calc.Instance.Resources.GetString(Resource.String.crusader);
                case HeroClass.DemonHunter:
                    return D3Calc.Instance.Resources.GetString(Resource.String.demonHunter);
                case HeroClass.Monk:
                    return D3Calc.Instance.Resources.GetString(Resource.String.monk);
                case HeroClass.WitchDoctor:
                    return D3Calc.Instance.Resources.GetString(Resource.String.witchDoctor);
                case HeroClass.Wizard:
                    return D3Calc.Instance.Resources.GetString(Resource.String.wizard);
                default:
                    return "Unknown";
            }
        }

        public static int GetPortraitResource(this HeroClass heroClass, HeroGender gender)
        {
            switch (heroClass)
            {
                case HeroClass.Barbarian:
                    return (gender == HeroGender.Male ? Resource.Drawable.barbarian_male : Resource.Drawable.barbarian_female);
                case HeroClass.Crusader:
                    return (gender == HeroGender.Male ? Resource.Drawable.crusader_male : Resource.Drawable.crusader_female);
                case HeroClass.DemonHunter:
                    return (gender == HeroGender.Male ? Resource.Drawable.demonhunter_male : Resource.Drawable.demonhunter_female);
                case HeroClass.Monk:
                    return (gender == HeroGender.Male ? Resource.Drawable.monk_male : Resource.Drawable.monk_female);
                case HeroClass.WitchDoctor:
                    return (gender == HeroGender.Male ? Resource.Drawable.witchdoctor_male : Resource.Drawable.witchdoctor_female);
                case HeroClass.Wizard:
                    return (gender == HeroGender.Male ? Resource.Drawable.wizard_male : Resource.Drawable.wizard_female);
                default:
                    return Resource.Drawable.ic_launcher;
            }
        }
    }
}
