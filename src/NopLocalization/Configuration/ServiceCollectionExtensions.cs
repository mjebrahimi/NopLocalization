using NopLocalization.Internal;
using AutoMapper;
using CacheManager.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;

namespace NopLocalization
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add <see cref="LocalizationFilterAttribute"/> to <paramref name="options"/> to execute UseRequestLocalization in MVC by <see cref="LocalizationPipeline"/> middleware
        /// </summary>
        /// <param name="options"></param>
        public static void AddLocalizationFilter(this MvcOptions options)
        {
            options.Filters.Add<LocalizationFilterAttribute>();
        }

        /// <summary>
        /// Adds Localization (RequestLocalizationMiddleware) to the <see cref="IApplicationBuilder"/> request execution pipeline
        /// to automatically set culture information for requests based on information provided by the client.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseNopLocalization(this IApplicationBuilder app)
        {
            ApplicationServiceProvider.ServiceProvider = app.ApplicationServices;

            var options = app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;

            #region Routing
            //app.UseRouter(routes =>
            //{
            //    routes.MapMiddlewareRoute("{culture=" + options.DefaultRequestCulture.Culture + "}/{*mvcRoute}", subApp =>
            //    {
            //        subApp.UseRequestLocalization(options);
            //        subApp.UseMvc(mvcRoutes =>
            //        {
            //            mvcRoutes.MapRoute(
            //                name: "default",
            //                template: "{culture=" + options.DefaultRequestCulture.Culture + "}/{controller=Home}/{action=Index}/{id?}");
            //        });
            //    });
            //});

            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "LocalizedDefault",
            //        template: "{culture:culture=" + options.DefaultRequestCulture.Culture + "}/{controller=Home}/{action=Index}/{id?}"
            //    );
            //    routes.MapRoute(
            //        name: "CatchAll",
            //        template: "{*catchall}",
            //        defaults: new { controller = "Home", action = "RedirectToDefaultLanguage", culture = "en" });
            //});
            #endregion

            app.UseRequestLocalization(options);

            return app;
        }

        /// <summary>
        /// Adds Localization services to the <see cref="IServiceCollection"/> with assemblies=AppDomain.CurrentDomain.GetAssemblies()
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddNopLocalization(this IServiceCollection services, Action<LocalizationOptions> configureOptions)
        {
            return AddNopLocalization(services, configureOptions, null, AppDomain.CurrentDomain.GetAssemblies());
        }

        /// <summary>
        /// Adds Localization services to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IServiceCollection AddNopLocalization(this IServiceCollection services, Action<LocalizationOptions> configureOptions, params Assembly[] assemblies)
        {
            return AddNopLocalization(services, configureOptions, null, assemblies);
        }

        /// <summary>
        /// Adds Localization services to the <see cref="IServiceCollection"/>. and AddAutoMapper
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <param name="configAction"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IServiceCollection AddNopLocalization(this IServiceCollection services, Action<LocalizationOptions> configureOptions,
            Action<IServiceProvider, IMapperConfigurationExpression> configAction, params Assembly[] assemblies)
        {
            var localizationOptions = new LocalizationOptions();
            configureOptions(localizationOptions);
            services.Configure(configureOptions);

            services.Configure<RequestLocalizationOptions>(options =>
            {
                localizationOptions.DefaultLanguage.NotNull(nameof(localizationOptions.DefaultLanguage));
                localizationOptions.OtherLanguages.NotNull(nameof(localizationOptions.OtherLanguages));

                var supportedCultures = localizationOptions.OtherLanguages.Concat(new[] { localizationOptions.DefaultLanguage })
                    .Select(i => new CultureInfo(i)).ToList();

                options.DefaultRequestCulture = new RequestCulture(localizationOptions.DefaultLanguage);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                //options.RequestCultureProviders.Clear();
                //var requestProvider = new RouteDataRequestCultureProvider() { Options = options };

                var requestProvider = new RouteSegmentRequestCultureProvider() { Options = options, CultureSegmentIndex = localizationOptions.CultureSegmentIndex };
                options.RequestCultureProviders.Insert(0, requestProvider);

                localizationOptions.ConfigureRequestLocalizationOptions?.Invoke(options);
            });

            services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add("culture", typeof(CultureRouteConstraint));
            });

            services.AddLocalization(opt => opt.ResourcesPath = "Resources");

            switch (localizationOptions.CacheMode)
            {
                case CacheMode.MemoryCache:
                    services.AddMemoryCacheManager();
                    break;
                case CacheMode.RedisCacheWithProtoBuf:
                    services.AddRedisWithProtoBufCacheManager();
                    break;
            }

            TypeConverterRegistrar.RegisterTypeConverters();

            services.AddServices();

            services.AddAutoMapper((serviceProvider, mapperConfigExpression) =>
            {
                mapperConfigExpression.AddProfile(new MappingProfile(assemblies));
                //mapperConfigExpression.ValidateInlineMaps = false;
                //mapperConfigExpression.CreateMissingTypeMaps = true;

                configAction?.Invoke(serviceProvider, mapperConfigExpression);
            }, assemblies);

            return services;
        }

        private static void AddServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.TryAddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.TryAddScoped<ILocalizedPropertyCacheInvalidator, LocalizedPropertyCacheInvalidator>();

            services.TryAddScoped<ILanguageRepository, LanguageRepository>();
            services.TryAddScoped<ILocalizedPropertyRepository, LocalizedPropertyRepository>();
            services.TryAddScoped<ILanguageRepository, LanguageRepository>();

            services.TryAddScoped<ILanguageResolver, CultureLanguageResolver>();
            //services.TryAddScoped<ILanguageResolver, RequestLanguageResolver>();
            //services.TryAddScoped<ILanguageResolver, CookieLanguageResolver>();
            //services.TryAddScoped<ILanguageResolver, DefaultLanguageResolver>();

            services.TryAddScoped<ILanguageService, LanguageService>();
            services.TryAddScoped<IEntityLocalizer, EntityLocalizer>();
            services.TryAddScoped<IModelLocalizer, ModelLocalizer>();

            services.TryAddSingleton<IEntityLocalizedPropertyInfoResolver, EntityLocalizedPropertyInfoResolver>();
            services.TryAddSingleton<IModelLocalizedPropertyResolver, ModelLocalizedPropertyResolver>();
        }

        private static void AddMemoryCacheManager(this IServiceCollection services)
        {
            #region CacheManager.Microsoft.Extensions.Configuration

            //// using the new overload which adds a singleton of the configuration to services and the configure method to add logging
            //// TODO: still not 100% happy with the logging part
            //services.AddCacheManagerConfiguration( cfg => cfg.WithMicrosoftLogging(services));

            //// uses a refined configurastion (this will not log, as we added the MS Logger only to the configuration above
            //services.AddCacheManager<int>(configure: builder => builder.WithJsonSerializer());

            //// creates a completely new configuration for this instance (also not logging)
            //services.AddCacheManager<DateTime>(inline => inline.WithDictionaryHandle());

            //// any other type will be this. Configurastion used will be the one defined by AddCacheManagerConfiguration earlier.
            //services.AddCacheManager();
            #endregion

            var options = services.BuildServiceProvider().GetRequiredService<IOptions<LocalizationOptions>>().Value;

            services.AddSingleton(typeof(ICacheManagerConfiguration),
                new ConfigurationBuilder()
                    //.WithJsonSerializer()
                    .WithMicrosoftMemoryCacheHandle(instanceName: "MemoryCache")
                    .WithExpiration(ExpirationMode.Absolute, TimeSpan.FromMinutes(options.CacheTimeInMinutes))
                    .DisablePerformanceCounters()
                    .DisableStatistics()
                    .Build());
            services.AddSingleton(typeof(ICacheManager<>), typeof(BaseCacheManager<>));
        }

        private static void AddRedisWithProtoBufCacheManager(this IServiceCollection services)
        {
            var options = services.BuildServiceProvider().GetRequiredService<IOptions<LocalizationOptions>>().Value;

            //var jss = new JsonSerializerSettings
            //{
            //    NullValueHandling = NullValueHandling.Ignore,
            //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //};

            services.AddSingleton(typeof(ICacheManagerConfiguration),
                new ConfigurationBuilder()
                    .WithProtoBufSerializer()
                    //.WithJsonSerializer(serializationSettings: jss, deserializationSettings: jss)
                    .WithUpdateMode(CacheUpdateMode.Up)
                    .WithRedisConfiguration("Redis", config =>
                    {
                        var arr = options.RedisCachingConnectionString.Split(',')[0].Split(':');
                        config.WithAllowAdmin()
                            .WithDatabase(0)
                            .WithEndpoint(arr[0], Convert.ToInt32(arr));
                    })
                    .WithMaxRetries(100)
                    .WithRetryTimeout(50)
                    .WithRedisCacheHandle("Redis")
                    .WithExpiration(ExpirationMode.Absolute, TimeSpan.FromMinutes(options.CacheTimeInMinutes))
                    .DisablePerformanceCounters()
                    .DisableStatistics()
                    .Build());
            services.AddSingleton(typeof(ICacheManager<>), typeof(BaseCacheManager<>));
        }
    }
}
