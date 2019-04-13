using NopLocalization.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace NopLocalization
{
    public static class EntityLocalizationExtensions
    {
        #region GetLocalizedValueAsync

        #region Current LanguageId
        /// <summary>
        /// Find localized value with getOriginalValueIfNotFound=true
        /// </summary>
        /// <typeparam name="TEntity">Type</typeparam>
        /// <typeparam name="TPropType">TPropType</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="keySelector">Key selector</param>
        /// <returns>return localized value as TPropType</returns>
        public static async Task<TPropType> GetLocalizedValueAsync<TEntity, TPropType>(this TEntity entity,
            Expression<Func<TEntity, TPropType>> keySelector,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                return await serviceScope.ServiceProvider.GetRequiredService<IEntityLocalizer>()
                    .GetLocalizedValueAsync(entity, keySelector, cancellationToken);
            }
        }

        /// <summary>
        /// Find localized value
        /// </summary>
        /// <typeparam name="TEntity">Type</typeparam>
        /// <typeparam name="TPropType">TPropType</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="keySelector">Key selector</param>
        /// <param name="getOriginalValueIfNotFound">Return original value if notFound</param>
        /// <returns>return localized value as TPropType</returns>
        public static async Task<TPropType> GetLocalizedValueAsync<TEntity, TPropType>(this TEntity entity,
            Expression<Func<TEntity, TPropType>> keySelector,
            bool getOriginalValueIfNotFound,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                return await serviceScope.ServiceProvider.GetRequiredService<IEntityLocalizer>()
                    .GetLocalizedValueAsync(entity, keySelector, getOriginalValueIfNotFound, cancellationToken);
            }
        }
        #endregion

        #region Specific LanguageId
        /// <summary>
        /// Find localized value with getOriginalValueIfNotFound=true
        /// </summary>
        /// <typeparam name="TEntity">Type</typeparam>
        /// <typeparam name="TPropType">TPropType</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="keySelector">Key selector</param>
        /// <param name="languageId">Language ID</param>
        /// <returns>return localized value as TPropType</returns>
        public static async Task<TPropType> GetLocalizedValueAsync<TEntity, TPropType>(this TEntity entity,
            Expression<Func<TEntity, TPropType>> keySelector,
            int languageId,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                return await serviceScope.ServiceProvider.GetRequiredService<IEntityLocalizer>()
                    .GetLocalizedValueAsync(entity, keySelector, languageId, cancellationToken);
            }
        }

        /// <summary>
        /// Find localized value
        /// </summary>
        /// <typeparam name="TEntity">Type</typeparam>
        /// <typeparam name="TPropType">TPropType</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="keySelector">Key selector</param>
        /// <param name="languageId">Language ID</param>
        /// <param name="getOriginalValueIfNotFound">Return original value if notFound</param>
        /// <returns>return localized value as TPropType</returns>
        public static async Task<TPropType> GetLocalizedValueAsync<TEntity, TPropType>(this TEntity entity,
            Expression<Func<TEntity, TPropType>> keySelector,
            int languageId,
            bool getOriginalValueIfNotFound,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                return await serviceScope.ServiceProvider.GetRequiredService<IEntityLocalizer>()
                    .GetLocalizedValueAsync(entity, keySelector, languageId, getOriginalValueIfNotFound, cancellationToken);
            }
        }
        #endregion

        #endregion

        #region LocalizeAsync

        #region Single Item

        #region Current LanguageId

        #region Overloads
        /// <summary>
        /// Localizes all properties on an <see cref="ILocalizable"/> item with current language and depth=<see cref="LocalizationDepth.Shallow"/> and getOriginalValueIfNotFound=true
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized entity</returns>
        public static async Task<TEntity> LocalizeAsync<TEntity>(this TEntity entity,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                return await serviceScope.ServiceProvider.GetRequiredService<IEntityLocalizer>()
                    .LocalizeAsync(entity, cancellationToken);
            }
        }

        /// <summary>
        /// Localizes all properties on an <see cref="ILocalizable"/> item with current language and depth=<see cref="LocalizationDepth.Shallow"/>
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="getOriginalValueIfNotFound">A value indicating whether to return default value (if localized is not found)</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized entity</returns>
        public static async Task<TEntity> LocalizeAsync<TEntity>(this TEntity entity,
            bool getOriginalValueIfNotFound,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                return await serviceScope.ServiceProvider.GetRequiredService<IEntityLocalizer>()
                    .LocalizeAsync(entity, getOriginalValueIfNotFound, cancellationToken);
            }
        }

        /// <summary>
        /// Localizes all properties on an <see cref="ILocalizable"/> item with current language and getOriginalValueIfNotFound=true
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="depth">Depth of localization</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized entity</returns>
        public static async Task<TEntity> LocalizeAsync<TEntity>(this TEntity entity,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                return await serviceScope.ServiceProvider.GetRequiredService<IEntityLocalizer>()
                    .LocalizeAsync(entity, depth, cancellationToken);
            }
        }
        #endregion

        /// <summary>
        /// Localizes all properties on an <see cref="ILocalizable"/> item with current language
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="getOriginalValueIfNotFound">A value indicating whether to return default value (if localized is not found)</param>
        /// <param name="depth">Depth of localization</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized entity</returns>
        public static async Task<TEntity> LocalizeAsync<TEntity>(this TEntity entity,
            bool getOriginalValueIfNotFound,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                return await serviceScope.ServiceProvider.GetRequiredService<IEntityLocalizer>()
                    .LocalizeAsync(entity, getOriginalValueIfNotFound, depth, cancellationToken);
            }
        }
        #endregion

        #region Specific LanguageId

        #region Overloads
        /// <summary>
        /// Localizes all properties on an <see cref="ILocalizable"/> item with specified language id and depth=<see cref="LocalizationDepth.Shallow"/> and getOriginalValueIfNotFound=true
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized entity</returns>
        public static async Task<TEntity> LocalizeAsync<TEntity>(this TEntity entity,
            int languageId,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                return await serviceScope.ServiceProvider.GetRequiredService<IEntityLocalizer>()
                    .LocalizeAsync(entity, languageId, cancellationToken);
            }
        }

        /// <summary>
        /// Localizes all properties on an <see cref="ILocalizable"/> item with specified language id and with depth=<see cref="LocalizationDepth.Shallow"/>
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="getOriginalValueIfNotFound">A value indicating whether to return default value (if localized is not found)</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized entity</returns>
        public static async Task<TEntity> LocalizeAsync<TEntity>(this TEntity entity,
            int languageId,
            bool getOriginalValueIfNotFound,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                return await serviceScope.ServiceProvider.GetRequiredService<IEntityLocalizer>()
                    .LocalizeAsync(entity, languageId, getOriginalValueIfNotFound, cancellationToken);
            }
        }

        /// <summary>
        /// Localizes all properties on an <see cref="ILocalizable"/> item with specified language id and with getOriginalValueIfNotFound=true
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="depth">Depth of localization</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized entity</returns>
        public static async Task<TEntity> LocalizeAsync<TEntity>(this TEntity entity,
            int languageId,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                return await serviceScope.ServiceProvider.GetRequiredService<IEntityLocalizer>()
                    .LocalizeAsync(entity, languageId, depth, cancellationToken);
            }
        }
        #endregion

        /// <summary>
        /// Localizes all properties on an <see cref="ILocalizable"/> item with specified language id
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="getOriginalValueIfNotFound">A value indicating whether to return default value (if localized is not found)</param>
        /// <param name="depth">Depth of localization</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized entity</returns>
        public static async Task<TEntity> LocalizeAsync<TEntity>(this TEntity entity,
            int languageId,
            bool getOriginalValueIfNotFound,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                return await serviceScope.ServiceProvider.GetRequiredService<IEntityLocalizer>()
                    .LocalizeAsync(entity, languageId, getOriginalValueIfNotFound, depth, cancellationToken);
            }
        }
        #endregion

        #endregion

        #region IEnumerable

        #region Current LanguageId

        #region Overloads
        /// <summary>
        /// Localizes all properties on each <see cref="ILocalizable"/> item in a collection with current language and depth=<see cref="LocalizationDepth.Shallow"/> and getOriginalValueIfNotFound=true
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized entitis</returns>
        public static async Task<IEnumerable<TEntity>> LocalizeAsync<TEntity>(this IEnumerable<TEntity> entities,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                return await serviceScope.ServiceProvider.GetRequiredService<IEntityLocalizer>()
                    .LocalizeAsync(entities, cancellationToken);
            }
        }

        /// <summary>
        /// Localizes all properties on each <see cref="ILocalizable"/> item in a collection with current language and depth=<see cref="LocalizationDepth.Shallow"/>
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <param name="getOriginalValueIfNotFound">A value indicating whether to return default value (if localized is not found)</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized entitis</returns>
        public static async Task<IEnumerable<TEntity>> LocalizeAsync<TEntity>(this IEnumerable<TEntity> entities,
            bool getOriginalValueIfNotFound,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                return await serviceScope.ServiceProvider.GetRequiredService<IEntityLocalizer>()
                    .LocalizeAsync(entities, getOriginalValueIfNotFound, cancellationToken);
            }
        }

        /// <summary>
        /// Localizes all properties on each <see cref="ILocalizable"/> item in a collection with current language and getOriginalValueIfNotFound=true
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <param name="depth">Depth of localization</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized entitis</returns>
        public static async Task<IEnumerable<TEntity>> LocalizeAsync<TEntity>(this IEnumerable<TEntity> entities,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                return await serviceScope.ServiceProvider.GetRequiredService<IEntityLocalizer>()
                    .LocalizeAsync(entities, depth, cancellationToken);
            }
        }
        #endregion

        /// <summary>
        /// Localizes all properties on each <see cref="ILocalizable"/> item in a collection with current language
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <param name="getOriginalValueIfNotFound">A value indicating whether to return default value (if localized is not found)</param>
        /// <param name="depth">Depth of localization</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized entitis</returns>
        public static async Task<IEnumerable<TEntity>> LocalizeAsync<TEntity>(this IEnumerable<TEntity> entities,
            bool getOriginalValueIfNotFound,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                return await serviceScope.ServiceProvider.GetRequiredService<IEntityLocalizer>()
                    .LocalizeAsync(entities, getOriginalValueIfNotFound, depth, cancellationToken);
            }
        }
        #endregion

        #region Specific LanguageId

        #region Overloads
        /// <summary>
        /// Localizes all properties on each <see cref="ILocalizable"/> item in a collection with specified language id and depth=<see cref="LocalizationDepth.Shallow"/> and getOriginalValueIfNotFound=true
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized entitis</returns>
        public static async Task<IEnumerable<TEntity>> LocalizeAsync<TEntity>(this IEnumerable<TEntity> entities,
            int languageId,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                return await serviceScope.ServiceProvider.GetRequiredService<IEntityLocalizer>()
                    .LocalizeAsync(entities, languageId, cancellationToken);
            }
        }

        /// <summary>
        /// Localizes all properties on each <see cref="ILocalizable"/> item in a collection with specified language id and depth=<see cref="LocalizationDepth.Shallow"/>
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="getOriginalValueIfNotFound">A value indicating whether to return default value (if localized is not found)</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized entitis</returns>
        public static async Task<IEnumerable<TEntity>> LocalizeAsync<TEntity>(this IEnumerable<TEntity> entities,
            int languageId,
            bool getOriginalValueIfNotFound,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                return await serviceScope.ServiceProvider.GetRequiredService<IEntityLocalizer>()
                    .LocalizeAsync(entities, languageId, getOriginalValueIfNotFound, cancellationToken);
            }
        }

        /// <summary>
        /// Localizes all properties on each <see cref="ILocalizable"/> item in a collection with specified language id and getOriginalValueIfNotFound=true
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="depth">Depth of localization</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized entitis</returns>
        public static async Task<IEnumerable<TEntity>> LocalizeAsync<TEntity>(this IEnumerable<TEntity> entities,
            int languageId,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                return await serviceScope.ServiceProvider.GetRequiredService<IEntityLocalizer>()
                    .LocalizeAsync(entities, languageId, depth, cancellationToken);
            }
        }
        #endregion

        /// <summary>
        /// Localizes all properties on each <see cref="ILocalizable"/> item in a collection with specified language id
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="getOriginalValueIfNotFound">A value indicating whether to return default value (if localized is not found)</param>
        /// <param name="depth">Depth of localization</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized entitis</returns>
        public static async Task<IEnumerable<TEntity>> LocalizeAsync<TEntity>(this IEnumerable<TEntity> entities,
            int languageId,
            bool getOriginalValueIfNotFound,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                return await serviceScope.ServiceProvider.GetRequiredService<IEntityLocalizer>()
                    .LocalizeAsync(entities, languageId, getOriginalValueIfNotFound, depth, cancellationToken);
            }
        }
        #endregion

        #endregion

        #endregion

        #region SaveLocalizedValueAsync
        /// <summary>
        /// Save localized value
        /// </summary>
        /// <typeparam name="TEntity">Type</typeparam>
        /// <typeparam name="TPropType">TPropType</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="keySelector">Key selector</param>
        /// <param name="localeValue">Locale value</param>
        /// <param name="languageId">Language ID</param>
        public static async Task SaveLocalizedValueAsync<TEntity, TPropType>(this TEntity entity,
            Expression<Func<TEntity, TPropType>> keySelector,
            TPropType localeValue,
            int languageId,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                await serviceScope.ServiceProvider.GetRequiredService<IEntityLocalizer>()
                   .SaveLocalizedValueAsync(entity, keySelector, localeValue, languageId, cancellationToken);
            }
        }

        /// <summary>
        /// Save localized entity with depth=<see cref="LocalizationDepth.Shallow"/>
        /// </summary>
        /// <typeparam name="TEntity">Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="languageId">Language identifier</param>
        public static async Task SaveLocalizedAsync<TEntity>(this TEntity entity,
            int languageId,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                await serviceScope.ServiceProvider.GetRequiredService<IEntityLocalizer>()
                   .SaveLocalizedAsync(entity, languageId, cancellationToken);
            }
        }

        /// <summary>
        /// Save localized entity
        /// </summary>
        /// <typeparam name="TEntity">Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="depth">Depth of localization</param>
        public static async Task SaveLocalizedAsync<TEntity>(this TEntity entity,
            int languageId,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
            {
                await serviceScope.ServiceProvider.GetRequiredService<IEntityLocalizer>()
                   .SaveLocalizedAsync(entity, languageId, depth, cancellationToken);
            }
        }
        #endregion
    }
}
