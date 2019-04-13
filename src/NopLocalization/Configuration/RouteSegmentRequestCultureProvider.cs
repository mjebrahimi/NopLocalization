using NopLocalization.Internal;
using Microsoft.AspNetCore.Localization;
using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace NopLocalization
{
    public class RouteSegmentRequestCultureProvider : RequestCultureProvider
    {
        public int CultureSegmentIndex { get; set; } = 0;

        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            httpContext.NotNull(nameof(httpContext));

            var culture = httpContext.Request.Path.Value.TrimStart('/').Split('/')[CultureSegmentIndex];

            var supported = Options.SupportedCultures
                .Any(p => p.TwoLetterISOLanguageName.Equals(culture, StringComparison.OrdinalIgnoreCase));

            if (!supported)
                return NullProviderCultureResult;

            var providerResultCulture = new ProviderCultureResult(culture, culture);
            return Task.FromResult(providerResultCulture);
        }
    }
}
