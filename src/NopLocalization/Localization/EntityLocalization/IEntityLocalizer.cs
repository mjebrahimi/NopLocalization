using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace NopLocalization
{
    public interface IEntityLocalizer
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
        Task<TPropType> GetLocalizedValueAsync<TEntity, TPropType>(TEntity entity, Expression<Func<TEntity, TPropType>> keySelector, CancellationToken cancellationToken = default) where TEntity : class, ILocalizable;

        /// <summary>
        /// Find localized value
        /// </summary>
        /// <typeparam name="TEntity">Type</typeparam>
        /// <typeparam name="TPropType">TPropType</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="keySelector">Key selector</param>
        /// <param name="getOriginalValueIfNotFound">Return original value if notFound</param>
        /// <returns>return localized value as TPropType</returns>
        Task<TPropType> GetLocalizedValueAsync<TEntity, TPropType>(TEntity entity, Expression<Func<TEntity, TPropType>> keySelector, bool getOriginalValueIfNotFound, CancellationToken cancellationToken = default) where TEntity : class, ILocalizable;
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
        Task<TPropType> GetLocalizedValueAsync<TEntity, TPropType>(TEntity entity, Expression<Func<TEntity, TPropType>> keySelector, int languageId, CancellationToken cancellationToken = default) where TEntity : class, ILocalizable;

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
        Task<TPropType> GetLocalizedValueAsync<TEntity, TPropType>(TEntity entity, Expression<Func<TEntity, TPropType>> keySelector, int languageId, bool getOriginalValueIfNotFound, CancellationToken cancellationToken = default) where TEntity : class, ILocalizable;
        #endregion

        #endregion

        #region LocalizeAsync

        #region Single Item

        #region Current LanguageId

        #region Overloads
        //TODO: complete document description
        // <summary>
        // Decorates all registered services of type <typeparamref name="TService"/>
        // using the <paramref name="decorator"/> function.
        // </summary>
        // <typeparam name="TService">The type of services to decorate.</typeparam>
        // <param name="services">The services to add to.</param>
        // <param name="decorator">The decorator function.</param>
        // <exception cref="MissingTypeRegistrationException">If no service of <typeparamref name="TService"/> has been registered.</exception>
        // <exception cref="ArgumentNullException">If either the <paramref name="services"/>
        // or <paramref name="decorator"/> arguments are <see langword="null"/>.</exception>

        /// <summary>
        /// Localizes all properties on an <see cref="ILocalizable"/> item with current language and depth=<see cref="LocalizationDepth.Shallow"/> and getOriginalValueIfNotFound=true
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized entity</returns>
        Task<TEntity> LocalizeAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class, ILocalizable;

        /// <summary>
        /// Localizes all properties on an <see cref="ILocalizable"/> item with current language and depth=<see cref="LocalizationDepth.Shallow"/>
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="getOriginalValueIfNotFound">A value indicating whether to return default value (if localized is not found)</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized entity</returns>
        Task<TEntity> LocalizeAsync<TEntity>(TEntity entity, bool getOriginalValueIfNotFound, CancellationToken cancellationToken = default) where TEntity : class, ILocalizable;

        /// <summary>
        /// Localizes all properties on an <see cref="ILocalizable"/> item with current language and getOriginalValueIfNotFound=true
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="depth">Depth of localization</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized entity</returns>
        Task<TEntity> LocalizeAsync<TEntity>(TEntity entity, LocalizationDepth depth, CancellationToken cancellationToken = default) where TEntity : class, ILocalizable;
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
        Task<TEntity> LocalizeAsync<TEntity>(TEntity entity, bool getOriginalValueIfNotFound, LocalizationDepth depth, CancellationToken cancellationToken = default) where TEntity : class, ILocalizable;
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
        Task<TEntity> LocalizeAsync<TEntity>(TEntity entity, int languageId, CancellationToken cancellationToken = default) where TEntity : class, ILocalizable;

        /// <summary>
        /// Localizes all properties on an <see cref="ILocalizable"/> item with specified language id and with depth=<see cref="LocalizationDepth.Shallow"/>
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="getOriginalValueIfNotFound">A value indicating whether to return default value (if localized is not found)</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized entity</returns>
        Task<TEntity> LocalizeAsync<TEntity>(TEntity entity, int languageId, bool getOriginalValueIfNotFound, CancellationToken cancellationToken = default) where TEntity : class, ILocalizable;

        /// <summary>
        /// Localizes all properties on an <see cref="ILocalizable"/> item with specified language id and with getOriginalValueIfNotFound=true
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="depth">Depth of localization</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized entity</returns>
        Task<TEntity> LocalizeAsync<TEntity>(TEntity entity, int languageId, LocalizationDepth depth, CancellationToken cancellationToken = default) where TEntity : class, ILocalizable;
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
        Task<TEntity> LocalizeAsync<TEntity>(TEntity entity, int languageId, bool getOriginalValueIfNotFound, LocalizationDepth depth, CancellationToken cancellationToken = default) where TEntity : class, ILocalizable;
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
        Task<IEnumerable<TEntity>> LocalizeAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class, ILocalizable;

        /// <summary>
        /// Localizes all properties on each <see cref="ILocalizable"/> item in a collection with current language and depth=<see cref="LocalizationDepth.Shallow"/>
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <param name="getOriginalValueIfNotFound">A value indicating whether to return default value (if localized is not found)</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized entitis</returns>
        Task<IEnumerable<TEntity>> LocalizeAsync<TEntity>(IEnumerable<TEntity> entities, bool getOriginalValueIfNotFound, CancellationToken cancellationToken = default) where TEntity : class, ILocalizable;

        /// <summary>
        /// Localizes all properties on each <see cref="ILocalizable"/> item in a collection with current language and getOriginalValueIfNotFound=true
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <param name="depth">Depth of localization</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized entitis</returns>
        Task<IEnumerable<TEntity>> LocalizeAsync<TEntity>(IEnumerable<TEntity> entities, LocalizationDepth depth, CancellationToken cancellationToken = default) where TEntity : class, ILocalizable;
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
        Task<IEnumerable<TEntity>> LocalizeAsync<TEntity>(IEnumerable<TEntity> entities, bool getOriginalValueIfNotFound, LocalizationDepth depth, CancellationToken cancellationToken = default) where TEntity : class, ILocalizable;
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
        Task<IEnumerable<TEntity>> LocalizeAsync<TEntity>(IEnumerable<TEntity> entities, int languageId, CancellationToken cancellationToken = default) where TEntity : class, ILocalizable;

        /// <summary>
        /// Localizes all properties on each <see cref="ILocalizable"/> item in a collection with specified language id and depth=<see cref="LocalizationDepth.Shallow"/>
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="getOriginalValueIfNotFound">A value indicating whether to return default value (if localized is not found)</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized entitis</returns>
        Task<IEnumerable<TEntity>> LocalizeAsync<TEntity>(IEnumerable<TEntity> entities, int languageId, bool getOriginalValueIfNotFound, CancellationToken cancellationToken = default) where TEntity : class, ILocalizable;

        /// <summary>
        /// Localizes all properties on each <see cref="ILocalizable"/> item in a collection with specified language id and getOriginalValueIfNotFound=true
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="depth">Depth of localization</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized entitis</returns>
        Task<IEnumerable<TEntity>> LocalizeAsync<TEntity>(IEnumerable<TEntity> entities, int languageId, LocalizationDepth depth, CancellationToken cancellationToken = default) where TEntity : class, ILocalizable;
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
        Task<IEnumerable<TEntity>> LocalizeAsync<TEntity>(IEnumerable<TEntity> entities, int languageId, bool getOriginalValueIfNotFound, LocalizationDepth depth, CancellationToken cancellationToken = default) where TEntity : class, ILocalizable;
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
        Task SaveLocalizedValueAsync<TEntity, TPropType>(TEntity entity, Expression<Func<TEntity, TPropType>> keySelector, TPropType localeValue, int languageId, CancellationToken cancellationToken = default) where TEntity : class, ILocalizable;

        /// <summary>
        /// Save localized entity with depth=<see cref="LocalizationDepth.Shallow"/>
        /// </summary>
        /// <typeparam name="TEntity">Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="languageId">Language identifier</param>
        Task SaveLocalizedAsync<TEntity>(TEntity entity, int languageId, CancellationToken cancellationToken = default) where TEntity : class, ILocalizable;

        /// <summary>
        /// Save localized entity
        /// </summary>
        /// <typeparam name="TEntity">Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="depth">Depth of localization</param>
        Task SaveLocalizedAsync<TEntity>(TEntity entity, int languageId, LocalizationDepth depth, CancellationToken cancellationToken = default) where TEntity : class, ILocalizable;
        #endregion
    }
}