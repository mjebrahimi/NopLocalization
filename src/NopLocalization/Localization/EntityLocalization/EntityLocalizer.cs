using NopLocalization.Internal;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace NopLocalization
{
    public class EntityLocalizer : IEntityLocalizer
    {
        #region Fields
        protected readonly ILocalizedPropertyRepository _localizedPropertyRepository;
        protected readonly ILanguageService _languageService;
        protected readonly IEntityLocalizedPropertyInfoResolver _localiedPropertiesInfoResolver;
        protected readonly LocalizationOptions _localizationOptions;
        #endregion

        #region Constructor
        public EntityLocalizer(
            ILocalizedPropertyRepository localizedPropertyRepository,
            ILanguageService languageService,
            IEntityLocalizedPropertyInfoResolver localiedPropertiesInfoResolver,
            IOptionsSnapshot<LocalizationOptions> localizationOptions)
        {
            _localizedPropertyRepository = localizedPropertyRepository;
            _languageService = languageService;
            _localiedPropertiesInfoResolver = localiedPropertiesInfoResolver;
            _localizationOptions = localizationOptions.Value;
        }
        #endregion

        #region Utilities
        protected object GetValue(string localeValue, Type propertyType, bool getOriginalValueIfNotFound, Func<object> originalValueFactory)
        {
            //set default value if required
            if (localeValue.HasValue(false))
                return localeValue.ConvertTo(propertyType);

            if (getOriginalValueIfNotFound)
                return originalValueFactory();

            return default;
        }

        protected void CheckLocalizedAttribute(PropertyInfo propInfo)
        {
            if (!propInfo.HasAttribute<LocalizedAttribute>(true))
                throw new ArgumentException($"Property '{propInfo.Name}' has not LocalizedAttribute.", propInfo.Name);
        }
        #endregion

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
        public virtual Task<TPropType> GetLocalizedValueAsync<TEntity, TPropType>(TEntity entity,
            Expression<Func<TEntity, TPropType>> keySelector,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            return GetLocalizedValueAsync(entity, keySelector, true, cancellationToken);
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
        public virtual async Task<TPropType> GetLocalizedValueAsync<TEntity, TPropType>(TEntity entity,
            Expression<Func<TEntity, TPropType>> keySelector,
            bool getOriginalValueIfNotFound,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            entity.NotNull(nameof(entity));
            keySelector.NotNull(nameof(keySelector));

            var propInfo = keySelector.GetPropertyInfo();
            CheckLocalizedAttribute(propInfo);

            var language = await _languageService.GetCurrentLanguageCachedAsync(cancellationToken);

            string localeValue = null;
            if (language.LanguageCode.Equals(_localizationOptions.DefaultLanguage, StringComparison.OrdinalIgnoreCase))
            {
                var entityName = entity.GetType().GetUnproxiedEntityType().Name;
                var propertyName = propInfo.Name;

                localeValue = await _localizedPropertyRepository.GetLocalizedValueAsync(entityName, propertyName, entity.Id, language.Id, _localizationOptions.LoadAllLocalizedProperties, cancellationToken);
            }

            return (TPropType)GetValue(localeValue, propInfo.PropertyType, getOriginalValueIfNotFound, () =>
            {
                var getOriginalValue = keySelector.Compile();
                return getOriginalValue(entity);
            });
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
        public virtual Task<TPropType> GetLocalizedValueAsync<TEntity, TPropType>(TEntity entity,
            Expression<Func<TEntity, TPropType>> keySelector,
            int languageId,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            return GetLocalizedValueAsync(entity, keySelector, languageId, true, cancellationToken);
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
        public virtual async Task<TPropType> GetLocalizedValueAsync<TEntity, TPropType>(TEntity entity,
            Expression<Func<TEntity, TPropType>> keySelector,
            int languageId,
            bool getOriginalValueIfNotFound,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            entity.NotNull(nameof(entity));
            keySelector.NotNull(nameof(keySelector));

            var propInfo = keySelector.GetPropertyInfo();
            CheckLocalizedAttribute(propInfo);

            var entityName = entity.GetType().GetUnproxiedEntityType().Name;
            var propertyName = propInfo.Name;

            var localeValue = await _localizedPropertyRepository.GetLocalizedValueAsync(entityName, propertyName, entity.Id, languageId, _localizationOptions.LoadAllLocalizedProperties, cancellationToken);

            return (TPropType)GetValue(localeValue, propInfo.PropertyType, getOriginalValueIfNotFound, () =>
            {
                var getOriginalValue = keySelector.Compile();
                return getOriginalValue(entity);
            });
        }
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
        public virtual Task<TEntity> LocalizeAsync<TEntity>(TEntity entity,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            return LocalizeAsync(entity, true, LocalizationDepth.Shallow, cancellationToken);
        }

        /// <summary>
        /// Localizes all properties on an <see cref="ILocalizable"/> item with current language and depth=<see cref="LocalizationDepth.Shallow"/>
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="getOriginalValueIfNotFound">A value indicating whether to return default value (if localized is not found)</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized entity</returns>
        public virtual Task<TEntity> LocalizeAsync<TEntity>(TEntity entity,
            bool getOriginalValueIfNotFound,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            return LocalizeAsync(entity, getOriginalValueIfNotFound, LocalizationDepth.Shallow, cancellationToken);
        }

        /// <summary>
        /// Localizes all properties on an <see cref="ILocalizable"/> item with current language and getOriginalValueIfNotFound=true
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="depth">Depth of localization</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized entity</returns>
        public virtual Task<TEntity> LocalizeAsync<TEntity>(TEntity entity,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            return LocalizeAsync(entity, true, depth, cancellationToken);
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
        public virtual async Task<TEntity> LocalizeAsync<TEntity>(TEntity entity,
            bool getOriginalValueIfNotFound,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            if (entity == null) return null;

            var language = await _languageService.GetCurrentLanguageCachedAsync(cancellationToken);

            if (!language.LanguageCode.Equals(_localizationOptions.DefaultLanguage, StringComparison.OrdinalIgnoreCase))
                await LocalizeAsync(entity, language.Id, getOriginalValueIfNotFound, depth, cancellationToken);

            return entity;
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
        public virtual Task<TEntity> LocalizeAsync<TEntity>(TEntity entity,
            int languageId,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            return LocalizeAsync(entity, languageId, true, LocalizationDepth.Shallow, cancellationToken);
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
        public virtual Task<TEntity> LocalizeAsync<TEntity>(TEntity entity,
            int languageId,
            bool getOriginalValueIfNotFound,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            return LocalizeAsync(entity, languageId, getOriginalValueIfNotFound, LocalizationDepth.Shallow, cancellationToken);
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
        public virtual Task<TEntity> LocalizeAsync<TEntity>(TEntity entity,
            int languageId,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            return LocalizeAsync(entity, languageId, true, depth, cancellationToken);
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
        public virtual async Task<TEntity> LocalizeAsync<TEntity>(TEntity entity,
            int languageId,
            bool getOriginalValueIfNotFound,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            if (entity == null) return null;

            await LocalizeLocalizedPropertiesAsync(entity, languageId, getOriginalValueIfNotFound, depth, cancellationToken);
            await LocalizeNavigationPropertiesAsync(entity, languageId, getOriginalValueIfNotFound, depth, cancellationToken);

            return entity;
        }
        #endregion

        protected virtual async Task LocalizeLocalizedPropertiesAsync<TEntity>(TEntity entity,
            int languageId,
            bool getOriginalValueIfNotFound,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            if (entity == null) return;

            var entityType = entity.GetType().GetUnproxiedEntityType();
            var localizedPropertyInfo = _localiedPropertiesInfoResolver.GetLocalizedPropertyInfo(entityType);

            foreach (var prop in localizedPropertyInfo.LocalizedProperties)
            {
                var valueStr = await _localizedPropertyRepository.GetLocalizedValueAsync(entityType.Name, prop.Name, entity.Id, languageId, _localizationOptions.LoadAllLocalizedProperties, cancellationToken);
                var value = GetValue(valueStr, prop.PropertyType, getOriginalValueIfNotFound, () => prop.GetValue(entity, null));
                prop.SetValue(entity, value, null);
            }
        }

        protected virtual async Task LocalizeNavigationPropertiesAsync<TEntity>(TEntity entity,
            int languageId,
            bool getOriginalValueIfNotFound,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            if (entity == null) return;
            if (depth == LocalizationDepth.Shallow)
                return;
            if (depth == LocalizationDepth.OneLevel)
                depth = LocalizationDepth.Shallow;

            var entityType = entity.GetType().GetUnproxiedEntityType();
            var localizedPropertyInfo = _localiedPropertiesInfoResolver.GetLocalizedPropertyInfo(entityType);

            var assosicatedProps = localizedPropertyInfo.AssosicatedProperties
                .Select(p => p.GetValue(entity, null) as ILocalizable)
                .Where(p => p != null);
            foreach (var value in assosicatedProps)
            {
                if (value.GetType().IsProxy())
                    throw new InvalidOperationException("Localize lazy-loaded entity only allowed with LocalizationDepth.Shallow depth.");

                await LocalizeAsync(value, languageId, getOriginalValueIfNotFound, depth, cancellationToken);
            }

            var collectionProps = localizedPropertyInfo.CollectionProperties
                .Select(p => p.GetValue(entity, null) as IEnumerable<ILocalizable>)
                .Where(p => p != null && p.Any());
            foreach (var collection in collectionProps)
            {
                if (collection.First()?.GetType().IsProxy() == true)
                    throw new InvalidOperationException("Localize lazy-loaded entity only allowed with LocalizationDepth.Shallow depth.");

                foreach (var item in collection.Where(p => p != null))
                    await LocalizeAsync(item, languageId, getOriginalValueIfNotFound, depth, cancellationToken);
            }
        }
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
        public virtual Task<IEnumerable<TEntity>> LocalizeAsync<TEntity>(IEnumerable<TEntity> entities,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            return LocalizeAsync(entities, true, LocalizationDepth.Shallow, cancellationToken);
        }

        /// <summary>
        /// Localizes all properties on each <see cref="ILocalizable"/> item in a collection with current language and depth=<see cref="LocalizationDepth.Shallow"/>
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <param name="getOriginalValueIfNotFound">A value indicating whether to return default value (if localized is not found)</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized entitis</returns>
        public virtual Task<IEnumerable<TEntity>> LocalizeAsync<TEntity>(IEnumerable<TEntity> entities,
            bool getOriginalValueIfNotFound,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            return LocalizeAsync(entities, getOriginalValueIfNotFound, LocalizationDepth.Shallow, cancellationToken);
        }

        /// <summary>
        /// Localizes all properties on each <see cref="ILocalizable"/> item in a collection with current language and getOriginalValueIfNotFound=true
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <param name="depth">Depth of localization</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>Return localized entitis</returns>
        public virtual Task<IEnumerable<TEntity>> LocalizeAsync<TEntity>(IEnumerable<TEntity> entities,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            return LocalizeAsync(entities, true, depth, cancellationToken);
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
        public virtual async Task<IEnumerable<TEntity>> LocalizeAsync<TEntity>(IEnumerable<TEntity> entities,
            bool getOriginalValueIfNotFound,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            if (entities?.Any() != true) return null;

            var language = await _languageService.GetCurrentLanguageCachedAsync(cancellationToken);

            if (!language.LanguageCode.Equals(_localizationOptions.DefaultLanguage, StringComparison.OrdinalIgnoreCase))
            {
                foreach (var entity in entities)
                    await LocalizeAsync(entity, language.Id, getOriginalValueIfNotFound, depth, cancellationToken);
            }

            return entities;
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
        public virtual Task<IEnumerable<TEntity>> LocalizeAsync<TEntity>(IEnumerable<TEntity> entities,
            int languageId,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            return LocalizeAsync(entities, languageId, true, LocalizationDepth.Shallow, cancellationToken);
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
        public virtual Task<IEnumerable<TEntity>> LocalizeAsync<TEntity>(IEnumerable<TEntity> entities,
            int languageId,
            bool getOriginalValueIfNotFound,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            return LocalizeAsync(entities, languageId, getOriginalValueIfNotFound, LocalizationDepth.Shallow, cancellationToken);
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
        public virtual Task<IEnumerable<TEntity>> LocalizeAsync<TEntity>(IEnumerable<TEntity> entities,
            int languageId,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            return LocalizeAsync(entities, languageId, true, depth, cancellationToken);
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
        public virtual async Task<IEnumerable<TEntity>> LocalizeAsync<TEntity>(IEnumerable<TEntity> entities,
            int languageId,
            bool getOriginalValueIfNotFound,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            if (entities?.Any() != true) return null;

            foreach (var entity in entities)
                await LocalizeAsync(entity, languageId, getOriginalValueIfNotFound, depth, cancellationToken);

            return entities;
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
        public virtual Task SaveLocalizedValueAsync<TEntity, TPropType>(TEntity entity,
            Expression<Func<TEntity, TPropType>> keySelector,
            TPropType localeValue,
            int languageId,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            entity.NotNull(nameof(entity));
            keySelector.NotNull(nameof(keySelector));

            var propInfo = keySelector.GetPropertyInfo();
            CheckLocalizedAttribute(propInfo);

            var entityName = entity.GetType().GetUnproxiedEntityType().Name;

            return _localizedPropertyRepository.
                SaveLocalizedValueAsync(entityName, entity.Id, propInfo.Name, localeValue, languageId, cancellationToken);
        }

        /// <summary>
        /// Save localized entity with depth=<see cref="LocalizationDepth.Shallow"/>
        /// </summary>
        /// <typeparam name="TEntity">Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="languageId">Language identifier</param>
        public virtual Task SaveLocalizedAsync<TEntity>(TEntity entity,
            int languageId,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            return SaveLocalizedAsync(entity, languageId, LocalizationDepth.Shallow, cancellationToken);
        }

        /// <summary>
        /// Save localized entity
        /// </summary>
        /// <typeparam name="TEntity">Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="depth">Depth of localization</param>
        public virtual async Task SaveLocalizedAsync<TEntity>(TEntity entity,
            int languageId,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            entity.NotNull(nameof(entity));

            await SaveLocalizedPropertiesAsync(entity, languageId, depth, cancellationToken);
            await SaveNavigationPropertiesAsync(entity, languageId, depth, cancellationToken);
        }

        protected virtual async Task SaveLocalizedPropertiesAsync<TEntity>(TEntity entity,
            int languageId,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            entity.NotNull(nameof(entity));

            var entityType = entity.GetType().GetUnproxiedEntityType();
            var localizedPropertyInfo = _localiedPropertiesInfoResolver.GetLocalizedPropertyInfo(entityType);

            foreach (var prop in localizedPropertyInfo.LocalizedProperties)
            {
                //TODO: check Duck typing is not supported in C#. That's why we're using dynamic type
                var value = prop.GetValue(entity, null);
                await _localizedPropertyRepository.
                    SaveLocalizedValueAsync(entityType.Name, entity.Id, prop.Name, value, languageId, cancellationToken);
            }
        }

        protected virtual async Task SaveNavigationPropertiesAsync<TEntity>(TEntity entity,
            int languageId,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
        {
            entity.NotNull(nameof(entity));
            if (depth == LocalizationDepth.Shallow)
                return;
            if (depth == LocalizationDepth.OneLevel)
                depth = LocalizationDepth.Shallow;

            var entityType = entity.GetType().GetUnproxiedEntityType();
            var localizedPropertyInfo = _localiedPropertiesInfoResolver.GetLocalizedPropertyInfo(entityType);

            var assosicatedProps = localizedPropertyInfo.AssosicatedProperties
                .Select(p => p.GetValue(entity, null) as ILocalizable)
                .Where(p => p != null);
            foreach (var value in assosicatedProps)
            {
                if (value.GetType().IsProxy())
                    throw new InvalidOperationException("Localize lazy-loaded entity only allowed with LocalizationDepth.Shallow depth.");

                await SaveLocalizedAsync(value, languageId, depth, cancellationToken);
            }

            var collectionProps = localizedPropertyInfo.CollectionProperties
                .Select(p => p.GetValue(entity, null) as IEnumerable<ILocalizable>)
                .Where(p => p != null && p.Any());
            foreach (var collection in collectionProps)
            {
                if (collection.First()?.GetType().IsProxy() == true)
                    throw new InvalidOperationException("Localize lazy-loaded entity only allowed with LocalizationDepth.Shallow depth.");

                foreach (var item in collection.Where(p => p != null))
                    await SaveLocalizedAsync(item, languageId, depth, cancellationToken);
            }
        }
        #endregion
    }
}