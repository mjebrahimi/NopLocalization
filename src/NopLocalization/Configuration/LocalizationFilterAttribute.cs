using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace NopLocalization
{
    /// <summary>
    /// Execute <see cref="LocalizationPipeline"/> middleware pipeline to execute UseRequestLocalization
    /// </summary>
    public class LocalizationFilterAttribute : MiddlewareFilterAttribute
    {
        public LocalizationFilterAttribute()
            : base(typeof(LocalizationPipeline))
        {
            Order = int.MinValue;
        }
    }

    /// <summary>
    /// A middleware pipeline to execute UseRequestLocalization
    /// </summary>
    public class LocalizationPipeline
    {
        public void Configure(IApplicationBuilder app, IOptions<RequestLocalizationOptions> options)
        {
            app.UseRequestLocalization(options.Value);
        }
    }
}
