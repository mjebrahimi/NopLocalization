using Microsoft.Extensions.Options;

namespace NopLocalization.Internal
{
    public class DefaultLanguageResolver : ILanguageResolver
    {
        private readonly LocalizationOptions _localizationOptions;

        public DefaultLanguageResolver(IOptionsSnapshot<LocalizationOptions> localizationOptions)
        {
            _localizationOptions = localizationOptions.Value;
        }

        public string GetCurrentLanguageCode()
        {
            return _localizationOptions.DefaultLanguage;
        }
    }
}
