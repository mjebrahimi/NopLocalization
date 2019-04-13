using CacheManager.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCore.Localization
{
    public class EntityLocalizer
    {
        //private readonly ICacheManager<LocalizedPropertyCached> _cacheManager;
        private readonly ICacheManager<string> _cacheManagerString;
        private readonly LocalizedPropertyRepository _localizedPropertyRepository;
        private readonly LanguageRepository _languageRepository;
        private readonly LocalizationOptions _localizationOptions;

        #region Constants
        /// <summary>
        /// Key for caching a property
        /// </summary>
        /// <remarks>
        /// {0} : entity name
        /// {1} : entity id
        /// {2} : property name
        /// {3} : language id
        /// </remarks>
        public const string CacheKey = "LocalizedProperty-{0}-{1}-{2}-{3}";
        #endregion

        #region Constructor
        public EntityLocalizer(
            //ICacheManager<List<LocalizedPropertyCached>> cacheManagerList,
            ICacheManager<string> cacheManagerString,
            LocalizedPropertyRepository localizedPropertyRepository,
            LanguageRepository languageRepository,
            IOptionsSnapshot<LocalizationOptions> localizationOptions)
        {
            //_cacheManagerList = cacheManagerList;
            _cacheManagerString = cacheManagerString;
            _localizedPropertyRepository = localizedPropertyRepository;
            _languageRepository = languageRepository;
            _localizationOptions = localizationOptions.Value;
        }
        #endregion

        #region GetLocalizedValue
        /// <summary>
        /// Find localized value by type
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <typeparam name="TPropType">TPropType</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="keySelector">Key selector</param>
        /// <param name="languageId">Language ID</param>
        /// <returns>return localized value as TPropType</returns>
        public virtual async Task<TPropType> GetLocalizedValueAsync<T, TPropType>(T entity, Expression<Func<T, TPropType>> keySelector, int languageId,
            CancellationToken cancellationToken, bool returnOriginalValueIfNotFound = true)
            where T : class, ILocalizable
            where TPropType : IConvertible
        {
            entity.NotNull(nameof(entity));

            var propInfo = keySelector.GetPropInfo();

            //load localized value (check whether it's a cacheable entity. In such cases we load its original entity type)
            var entityName = entity.GetUnproxiedEntityType().Name;
            var propertyName = propInfo.Name;

            var localeValue = await GetLocalizedValueAsync(entityName, propertyName, entity.Id, languageId, cancellationToken);

            //set default value if required
            if (localeValue.HasValue(false))
                return localeValue.ConvertTo<TPropType>();

            if (!returnOriginalValueIfNotFound)
                return default;

            var getOriginalValue = keySelector.Compile();
            return getOriginalValue(entity);
        }

        /// <summary>
        /// Find localized value
        /// </summary>
        /// <typeparam name="T">type of value</typeparam>
        /// <param name="entityName">Entity name</param>
        /// <param name="propertyName">Property name</param>
        /// <param name="entityId">Entity identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>return localized value as string</returns>
        protected virtual Task<string> GetLocalizedValueAsync(string entityName, string propertyName, int entityId, int languageId, Func<List<LocalizedProperty>> getSource, CancellationToken cancellationToken)
        {
            if (languageId == 0)
                throw new ArgumentOutOfRangeException(nameof(languageId), "Language ID should not be 0");

            var a = _cacheManagerString.Get("key", "region", () =>
            {
                //TODO : Check
                return null;
            });

            var key = string.Format(CacheKey, entityName, entityId, propertyName, languageId);
            if (_localizationOptions.LoadAllLocalizedProperties)
            {
                return _cacheManagerString.GetAsync(key, LocalizedPropertyRepository.CacheRegion, async () =>
                {
                    //load all records (we know they are cached)
                    var source = await _localizedPropertyRepository.GetAllCachedAsync(cancellationToken);
                    var localeValue = source
                        .Where(p => p.LanguageId == languageId && p.EntityId == entityId && p.EntityName == entityName && p.PropertyName == propertyName)
                        .Select(p => p.LocaleValue).FirstOrDefault();
                    //little hack here. nulls aren't cacheable so set it to ""
                    return localeValue ?? "";
                });
            }
            else
            {
                //gradual loading
                return _cacheManagerString.GetAsync(key, LocalizedPropertyRepository.CacheRegion, async () =>
                {
                    var localeValue = getSource()
                        .Where(p => p.LanguageId == languageId && p.EntityId == entityId && p.EntityName == entityName && p.PropertyName == propertyName)
                        .Select(p => p.LocaleValue).FirstOrDefault();
                    //little hack here. nulls aren't cacheable so set it to ""
                    return localeValue ?? "";
                });
            }
        }

        /// <summary>
        /// Find localized entity
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="languageId">Language ID</param>
        /// <param name="getOriginalValueIfNotFound">A value indicating whether to return default value (if localized is not found)</param>
        public virtual async Task<T> GetLocalizedValueAsync<T>(T entity, int languageId,
            CancellationToken cancellationToken, bool getOriginalValueIfNotFound = true) where T : class, ILocalizable
        {
            //TODO: set language id from language resolver (int? languageId = null)
            entity.NotNull(nameof(entity));

            //load localized value (check whether it's a cacheable entity. In such cases we load its original entity type)
            var entityType = entity.GetUnproxiedEntityType();
            var props = entityType.GetProperties()
                .Where(p => p.HasAttribute<LocalizedAttribute>() && p.CanRead && p.CanWrite);

            var propNames = props.Select(p => p.Name);
            var source = await _localizedPropertyRepository.TableNoTracking
                .Where(p => p.LanguageId == languageId && p.EntityId == entity.Id && p.EntityName == entityType.Name && propNames.Contains(p.PropertyName))
                .ToListAsync(cancellationToken);

            foreach (var prop in props)
            {
                var valueStr = await GetLocalizedValueAsync(entityType.Name, prop.Name, entity.Id, languageId, () => source, cancellationToken);

                var value = GetValue(valueStr, prop);

                prop.SetValue(entity, value, null);
            }
            return entity;

            object GetValue(string localeValue, PropertyInfo prop)
            {
                //set default value if required
                if (localeValue.HasValue(false))
                    return localeValue.ConvertTo(prop.PropertyType);

                if (!getOriginalValueIfNotFound)
                    return default;

                return prop.GetValue(entity, null);
            }
        }
        #endregion

        #region SaveLocalizedValue
        /// <summary>
        /// Save localized value
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <typeparam name="TPropType">TPropType</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="keySelector">Key selector</param>
        /// <param name="localeValue">Locale value</param>
        /// <param name="languageId">Language ID</param>
        public virtual async Task SaveLocalizedValueAsync<T, TPropType>(T entity, Expression<Func<T, TPropType>> keySelector, TPropType localeValue, int languageId, CancellationToken cancellationToken)
            where T : class, ILocalizable
            where TPropType : IConvertible
        {
            entity.NotNull(nameof(entity));

            var propInfo = keySelector.GetPropInfo();
            //load localized value (check whether it's a cacheable entity. In such cases we load its original entity type)
            var entityName = entity.GetUnproxiedEntityType().Name;
            var propertyName = propInfo.Name;

            var source = await _localizedPropertyRepository.Table
                .Where(p => p.EntityName == entityName && p.PropertyName == propertyName && p.EntityId == entity.Id && p.LanguageId == languageId)
                .ToListAsync(cancellationToken);

            await SaveLocalizedValueAsync(source, entityName, entity.Id, propertyName, localeValue, languageId, cancellationToken);

            await _localizedPropertyRepository.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Save localized value
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="entityName">Entity name</param>
        /// <param name="entityId">Entity identifier</param>
        /// <param name="propertyName">Property name</param>
        /// <param name="localeValue">Locale value</param>
        /// <param name="languageId">Language identifier</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="languageId"/></exception>
        protected virtual async Task SaveLocalizedValueAsync<T>(List<LocalizedProperty> source, string entityName, int entityId, string propertyName, T localeValue, int languageId, CancellationToken cancellationToken)
            where T : IConvertible
        {
            if (languageId == 0)
                throw new ArgumentOutOfRangeException(nameof(languageId), "Language ID should not be 0");

            var prop = source
                .Where(p => p.EntityName == entityName && p.PropertyName == propertyName && p.EntityId == entityId && p.LanguageId == languageId)
                .FirstOrDefault();

            var localeValueStr = localeValue.ConvertTo<string>(); //localeValue.ToString();
            if (prop != null)
            {
                if (localeValueStr.HasValue())
                {
                    //update
                    prop.LocaleValue = localeValueStr;
                    _localizedPropertyRepository.Entities.Update(prop);
                    //await _localizedPropertyRepository.UpdateAsync(prop, cancellationToken);
                }
                else
                {
                    //delete
                    _localizedPropertyRepository.Entities.Remove(prop);
                    //await _localizedPropertyRepository.DeleteAsync(prop, cancellationToken);
                }
            }
            else
            {
                if (localeValueStr.HasValue())
                {
                    //insert
                    prop = new LocalizedProperty
                    {
                        EntityId = entityId,
                        LanguageId = languageId,
                        PropertyName = propertyName,
                        EntityName = entityName,
                        LocaleValue = localeValueStr
                    };
                    await _localizedPropertyRepository.Entities.AddAsync(prop);
                    //await _localizedPropertyRepository.AddAsync(prop, cancellationToken);
                }
            }
        }

        /// <summary>
        /// Save localized value for entity
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="saveNonLocalizedProperty">saveNonLocalizedProperty</param>
        public virtual async Task SaveLocalizedValueAsync<T>(T entity, int languageId, CancellationToken cancellationToken) where T : class, ILocalizable
        {
            entity.NotNull(nameof(entity));

            //load localized value (check whether it's a cacheable entity. In such cases we load its original entity type)
            var entityType = entity.GetUnproxiedEntityType();
            var props = entityType.GetProperties()
                .Where(p => p.HasAttribute<LocalizedAttribute>() && p.CanRead && p.CanWrite);

            var propNames = props.Select(p => p.Name);
            var source = await _localizedPropertyRepository.Table
                .Where(p => p.EntityName == entityType.Name && propNames.Contains(p.PropertyName) && p.EntityId == entity.Id && p.LanguageId == languageId)
                .ToListAsync(cancellationToken);

            foreach (var prop in props)
            {
                //Duck typing is not supported in C#. That's why we're using dynamic type
                dynamic value = prop.GetValue(entity, null);
                await SaveLocalizedValueAsync(source, entityType.Name, entity.Id, prop.Name, value, languageId, cancellationToken);
            }
            await _localizedPropertyRepository.SaveChangesAsync(cancellationToken);
        }
        #endregion
    }
}