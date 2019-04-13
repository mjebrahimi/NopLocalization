using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace NopLocalization.Internal
{
    public class RequestLanguageResolver : ILanguageResolver
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public RequestLanguageResolver(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string GetCurrentLanguageCode()
        {
            var requestCultureFeature = _contextAccessor?.HttpContext?.Features?.Get<IRequestCultureFeature>();
            return requestCultureFeature?.RequestCulture?.Culture?.TwoLetterISOLanguageName;
        }
    }
}
