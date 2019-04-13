using NopLocalization.Internal;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NopLocalization
{
    public static class ModelLocalizationExtensions
    {
        #region LocalizeAsync

        #region Single Item

        #region Current LanguageId

        #region Overloads
        /// <summary>
        /// Localizes all properties on an <see cref="ILocalizedModel{TModel, TEntity}"/> item with current language
        /// and depth=<see cref="LocalizationDepth.Shallow"/> and getOriginalValueIfNotFound=true
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="model"></param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized model</returns>
        public static async Task<TModel> LocalizeAsync<TModel, TEntity>(this ILocalizedModel<TModel, TEntity> model,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModel<TModel, TEntity>
            where TEntity : class, ILocalizable
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                return await serviceScope.ServiceProvider.GetRequiredService<IModelLocalizer>()
                    .LocalizeAsync(model, cancellationToken);
            }
        }

        /// <summary>
        /// Localizes all properties on an <see cref="ILocalizedModel{TModel, TEntity}"/> item with current language
        /// and depth=<see cref="LocalizationDepth.Shallow"/>
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="model"></param>
        /// <param name="getOriginalValueIfNotFound">A value indicating whether to return default value (if localized is not found)</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized model</returns>
        public static async Task<TModel> LocalizeAsync<TModel, TEntity>(this ILocalizedModel<TModel, TEntity> model,
            bool getOriginalValueIfNotFound,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModel<TModel, TEntity>
            where TEntity : class, ILocalizable
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                return await serviceScope.ServiceProvider.GetRequiredService<IModelLocalizer>()
                    .LocalizeAsync(model, getOriginalValueIfNotFound, cancellationToken);
            }
        }

        /// <summary>
        /// Localizes all properties on an <see cref="ILocalizedModel{TModel, TEntity}"/> item with current language
        /// and getOriginalValueIfNotFound=true
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="model"></param>
        /// <param name="depth">Depth of localization</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized model</returns>
        public static async Task<TModel> LocalizeAsync<TModel, TEntity>(this ILocalizedModel<TModel, TEntity> model,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModel<TModel, TEntity>
            where TEntity : class, ILocalizable
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                return await serviceScope.ServiceProvider.GetRequiredService<IModelLocalizer>()
                    .LocalizeAsync(model, depth, cancellationToken);
            }
        }
        #endregion

        /// <summary>
        /// Localizes all properties on an <see cref="ILocalizedModel{TModel, TEntity}"/> item with current language
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="model"></param>
        /// <param name="getOriginalValueIfNotFound">A value indicating whether to return default value (if localized is not found)</param>
        /// <param name="depth">Depth of localization</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized model</returns>
        public static async Task<TModel> LocalizeAsync<TModel, TEntity>(this ILocalizedModel<TModel, TEntity> model,
            bool getOriginalValueIfNotFound,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModel<TModel, TEntity>
            where TEntity : class, ILocalizable
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                return await serviceScope.ServiceProvider.GetRequiredService<IModelLocalizer>()
                    .LocalizeAsync(model, getOriginalValueIfNotFound, depth, cancellationToken);
            }
        }
        #endregion

        #region Specific LanguageId

        #region Overloads
        /// <summary>
        /// Localizes all properties an <see cref="ILocalizedModel{TModel, TEntity}"/> item with specified language id and
        /// depth=<see cref="LocalizationDepth.Shallow"/> and getOriginalValueIfNotFound=true
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="model"></param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized model</returns>
        public static async Task<TModel> LocalizeAsync<TModel, TEntity>(this ILocalizedModel<TModel, TEntity> model,
            int languageId,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModel<TModel, TEntity>
            where TEntity : class, ILocalizable
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                return await serviceScope.ServiceProvider.GetRequiredService<IModelLocalizer>()
                    .LocalizeAsync(model, languageId, cancellationToken);
            }
        }

        /// <summary>
        /// Localizes all properties an <see cref="ILocalizedModel{TModel, TEntity}"/> item with specified language id and
        /// depth=<see cref="LocalizationDepth.Shallow"/>
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="model"></param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="getOriginalValueIfNotFound">A value indicating whether to return default value (if localized is not found)</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized model</returns>
        public static async Task<TModel> LocalizeAsync<TModel, TEntity>(this ILocalizedModel<TModel, TEntity> model,
            int languageId,
            bool getOriginalValueIfNotFound,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModel<TModel, TEntity>
            where TEntity : class, ILocalizable
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                return await serviceScope.ServiceProvider.GetRequiredService<IModelLocalizer>()
                    .LocalizeAsync(model, languageId, getOriginalValueIfNotFound, cancellationToken);
            }
        }

        /// <summary>
        /// Localizes all properties an <see cref="ILocalizedModel{TModel, TEntity}"/> item with specified language id and
        /// getOriginalValueIfNotFound=true
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="model"></param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="depth">Depth of localization</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized model</returns>
        public static async Task<TModel> LocalizeAsync<TModel, TEntity>(this ILocalizedModel<TModel, TEntity> model,
            int languageId,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModel<TModel, TEntity>
            where TEntity : class, ILocalizable
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                return await serviceScope.ServiceProvider.GetRequiredService<IModelLocalizer>()
                    .LocalizeAsync(model, languageId, depth, cancellationToken);
            }
        }
        #endregion

        /// <summary>
        /// Localizes all properties an <see cref="ILocalizedModel{TModel, TEntity}"/> item with specified language id
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="model"></param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="getOriginalValueIfNotFound">A value indicating whether to return default value (if localized is not found)</param>
        /// <param name="depth">Depth of localization</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized model</returns>
        public static async Task<TModel> LocalizeAsync<TModel, TEntity>(this ILocalizedModel<TModel, TEntity> model,
            int languageId,
            bool getOriginalValueIfNotFound,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModel<TModel, TEntity>
            where TEntity : class, ILocalizable
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                return await serviceScope.ServiceProvider.GetRequiredService<IModelLocalizer>()
                    .LocalizeAsync(model, languageId, getOriginalValueIfNotFound, depth, cancellationToken);
            }
        }
        #endregion

        #endregion

        #region IEnumerable

        #region Current LanguageId

        #region Overloads
        /// <summary>
        /// Localizes all properties on each <see cref="ILocalizedModel{TModel, TEntity}"/> item in a collection with current language
        /// and depth=<see cref="LocalizationDepth.Shallow"/> and getOriginalValueIfNotFound=true
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="models"></param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized models</returns>
        public static async Task<IEnumerable<TModel>> LocalizeAsync<TModel, TEntity>(this IEnumerable<ILocalizedModel<TModel, TEntity>> models,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModel<TModel, TEntity>
            where TEntity : class, ILocalizable
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                return await serviceScope.ServiceProvider.GetRequiredService<IModelLocalizer>()
                    .LocalizeAsync(models, cancellationToken);
            }
        }

        /// <summary>
        /// Localizes all properties on each <see cref="ILocalizedModel{TModel, TEntity}"/> item in a collection with current language
        /// and depth=<see cref="LocalizationDepth.Shallow"/>
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="models"></param>
        /// <param name="getOriginalValueIfNotFound">A value indicating whether to return default value (if localized is not found)</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized models</returns>
        public static async Task<IEnumerable<TModel>> LocalizeAsync<TModel, TEntity>(IEnumerable<ILocalizedModel<TModel, TEntity>> models,
            bool getOriginalValueIfNotFound,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModel<TModel, TEntity>
            where TEntity : class, ILocalizable
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                return await serviceScope.ServiceProvider.GetRequiredService<IModelLocalizer>()
                    .LocalizeAsync(models, getOriginalValueIfNotFound, cancellationToken);
            }
        }

        /// <summary>
        /// Localizes all properties on each <see cref="ILocalizedModel{TModel, TEntity}"/> item in a collection with current language
        /// and getOriginalValueIfNotFound=true
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="models"></param>
        /// <param name="depth">Depth of localization</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized models</returns>
        public static async Task<IEnumerable<TModel>> LocalizeAsync<TModel, TEntity>(this IEnumerable<ILocalizedModel<TModel, TEntity>> models,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModel<TModel, TEntity>
            where TEntity : class, ILocalizable
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                return await serviceScope.ServiceProvider.GetRequiredService<IModelLocalizer>()
                    .LocalizeAsync(models, depth, cancellationToken);
            }
        }
        #endregion

        /// <summary>
        /// Localizes all properties on each <see cref="ILocalizedModel{TModel, TEntity}"/> item in a collection with current language
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="models"></param>
        /// <param name="getOriginalValueIfNotFound">A value indicating whether to return default value (if localized is not found)</param>
        /// <param name="depth">Depth of localization</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized models</returns>
        public static async Task<IEnumerable<TModel>> LocalizeAsync<TModel, TEntity>(this IEnumerable<ILocalizedModel<TModel, TEntity>> models,
            bool getOriginalValueIfNotFound,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModel<TModel, TEntity>
            where TEntity : class, ILocalizable
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                return await serviceScope.ServiceProvider.GetRequiredService<IModelLocalizer>()
                    .LocalizeAsync(models, getOriginalValueIfNotFound, cancellationToken);
            }
        }
        #endregion

        #region Specific LanguageId

        #region Overloads
        /// <summary>
        /// Localizes all properties on each <see cref="ILocalizedModel{TModel, TEntity}"/> item in a collection with specified language id and
        /// depth=<see cref="LocalizationDepth.Shallow"/> and getOriginalValueIfNotFound=true
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="models"></param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized models</returns>
        public static async Task<IEnumerable<TModel>> LocalizeAsync<TModel, TEntity>(this IEnumerable<ILocalizedModel<TModel, TEntity>> models,
            int languageId,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModel<TModel, TEntity>
            where TEntity : class, ILocalizable
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                return await serviceScope.ServiceProvider.GetRequiredService<IModelLocalizer>()
                    .LocalizeAsync(models, languageId, cancellationToken);
            }
        }

        /// <summary>
        /// Localizes all properties on each <see cref="ILocalizedModel{TModel, TEntity}"/> item in a collection with specified language id and
        /// depth=<see cref="LocalizationDepth.Shallow"/>
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="models"></param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="getOriginalValueIfNotFound">A value indicating whether to return default value (if localized is not found)</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized models</returns>
        public static async Task<IEnumerable<TModel>> LocalizeAsync<TModel, TEntity>(this IEnumerable<ILocalizedModel<TModel, TEntity>> models,
            int languageId,
            bool getOriginalValueIfNotFound,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModel<TModel, TEntity>
            where TEntity : class, ILocalizable
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                return await serviceScope.ServiceProvider.GetRequiredService<IModelLocalizer>()
                    .LocalizeAsync(models, languageId, getOriginalValueIfNotFound, cancellationToken);
            }
        }

        /// <summary>
        /// Localizes all properties on each <see cref="ILocalizedModel{TModel, TEntity}"/> item in a collection with specified language id and
        /// getOriginalValueIfNotFound=true
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="models"></param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="depth">Depth of localization</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized models</returns>
        public static async Task<IEnumerable<TModel>> LocalizeAsync<TModel, TEntity>(this IEnumerable<ILocalizedModel<TModel, TEntity>> models,
            int languageId,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModel<TModel, TEntity>
            where TEntity : class, ILocalizable
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                return await serviceScope.ServiceProvider.GetRequiredService<IModelLocalizer>()
                    .LocalizeAsync(models, languageId, depth, cancellationToken);
            }
        }
        #endregion

        /// <summary>
        /// Localizes all properties on each <see cref="ILocalizedModel{TModel, TEntity}"/> item in a collection with specified language id
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="models"></param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="getOriginalValueIfNotFound">A value indicating whether to return default value (if localized is not found)</param>
        /// <param name="depth">Depth of localization</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized models</returns>
        public static async Task<IEnumerable<TModel>> LocalizeAsync<TModel, TEntity>(this IEnumerable<ILocalizedModel<TModel, TEntity>> models,
            int languageId,
            bool getOriginalValueIfNotFound,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModel<TModel, TEntity>
            where TEntity : class, ILocalizable
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                return await serviceScope.ServiceProvider.GetRequiredService<IModelLocalizer>()
                    .LocalizeAsync(models, languageId, getOriginalValueIfNotFound, depth, cancellationToken);
            }
        }
        #endregion

        #endregion

        #endregion

        #region AddLocalesAsync
        /// <summary>
        /// Add empty LocalizedModel item to Locales property of an ILocalizedModelForEdit model to create
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TLocalizedModel"></typeparam>
        /// <param name="model"></param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns></returns>
        public static async Task AddLocalesAsync<TModel, TEntity, TLocalizedModel>(
            this ILocalizedModelForEdit<TModel, TEntity, TLocalizedModel> model,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModelForEdit<TModel, TEntity, TLocalizedModel>
            where TEntity : class, ILocalizable
            where TLocalizedModel : class, ILocalizedLocaleModel
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                await serviceScope.ServiceProvider.GetRequiredService<IModelLocalizer>()
                   .AddLocalesAsync(model, cancellationToken);
            }
        }

        /// <summary>
        /// Add empty LocalizedModel item to Locales property of an ILocalizedModelForEdit model to edit
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TLocalizedModel"></typeparam>
        /// <param name="entity"></param>
        /// <param name="model"></param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns></returns>
        public static async Task AddLocalesAsync<TModel, TEntity, TLocalizedModel>(this TEntity entity,
            ILocalizedModelForEdit<TModel, TEntity, TLocalizedModel> model,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModelForEdit<TModel, TEntity, TLocalizedModel>
            where TEntity : class, ILocalizable
            where TLocalizedModel : class, ILocalizedLocaleModel
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                await serviceScope.ServiceProvider.GetRequiredService<IModelLocalizer>()
                   .AddLocalesAsync(entity, model, cancellationToken);
            }
        }

        /// <summary>
        /// Add empty LocalizedModel item to Locales property of an ILocalizedModelForEdit model to edit
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TLocalizedModel"></typeparam>
        /// <param name="entity"></param>
        /// <param name="model"></param>
        /// <param name="depth">Depth of localization</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns></returns>
        public static async Task AddLocalesAsync<TModel, TEntity, TLocalizedModel>(this TEntity entity,
            ILocalizedModelForEdit<TModel, TEntity, TLocalizedModel> model,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModelForEdit<TModel, TEntity, TLocalizedModel>
            where TEntity : class, ILocalizable
            where TLocalizedModel : class, ILocalizedLocaleModel
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                await serviceScope.ServiceProvider.GetRequiredService<IModelLocalizer>()
                   .AddLocalesAsync(entity, model, depth, cancellationToken);
            }
        }
        #endregion

        #region SaveLocalesAsync
        /// <summary>
        /// Save all localized properties of each LocalizedModel item in Locales collection property of model
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TLocalizedModel"></typeparam>
        /// <param name="entity"></param>
        /// <param name="model"></param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns></returns>
        public static async Task SaveLocalesAsync<TModel, TEntity, TLocalizedModel>(this TEntity entity,
            ILocalizedModelForEdit<TModel, TEntity, TLocalizedModel> model,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModelForEdit<TModel, TEntity, TLocalizedModel>
            where TEntity : class, ILocalizable
            where TLocalizedModel : class, ILocalizedLocaleModel
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                await serviceScope.ServiceProvider.GetRequiredService<IModelLocalizer>()
                   .SaveLocalesAsync(entity, model, cancellationToken);
            }
        }

        /// <summary>
        /// Save all localized properties of each LocalizedModel item in Locales collection property of model
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TLocalizedModel"></typeparam>
        /// <param name="entity"></param>
        /// <param name="model"></param>
        /// <param name="depth">Depth of localization</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns></returns>
        public static async Task SaveLocalesAsync<TModel, TEntity, TLocalizedModel>(this TEntity entity,
            ILocalizedModelForEdit<TModel, TEntity, TLocalizedModel> model,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModelForEdit<TModel, TEntity, TLocalizedModel>
            where TEntity : class, ILocalizable
            where TLocalizedModel : class, ILocalizedLocaleModel
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                await serviceScope.ServiceProvider.GetRequiredService<IModelLocalizer>()
                   .SaveLocalesAsync(entity, model, depth, cancellationToken);
            }
        }
        #endregion
    }
}
