using System.Globalization;

namespace NopLocalization.Internal
{
    public class CultureLanguageResolver : ILanguageResolver
    {
        public string GetCurrentLanguageCode()
        {
            return CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            //return Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
        }
    }
}
