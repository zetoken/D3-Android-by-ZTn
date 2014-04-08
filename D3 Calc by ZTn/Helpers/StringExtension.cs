using System.Linq;

namespace ZTnDroid.D3Calculator.Helpers
{
    static class StringExtension
    {
        public static string CapitalizeFirstLetter(this string text)
        {
            if (!text.Any())
            {
                return string.Empty;
            }

            var chars = text.ToCharArray();
            chars[0] = char.ToUpper(text[0]);

            return new string(chars);
        }
    }
}
