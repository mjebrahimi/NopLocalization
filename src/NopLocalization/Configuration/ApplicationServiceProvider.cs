using Microsoft.Extensions.DependencyInjection;
using System;

namespace NopLocalization.Internal
{
    /// <summary>
    /// Application's IServiceProvider.
    /// </summary>
    internal static class ApplicationServiceProvider
    {
        /// <summary>
        /// Access point of the application's IServiceProvider.
        /// </summary>
        internal static IServiceProvider ServiceProvider { get; set; }

        internal static IServiceScope CreateServiceScope() => ServiceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
    }
}
