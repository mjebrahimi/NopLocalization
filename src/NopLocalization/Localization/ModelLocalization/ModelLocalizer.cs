using NopLocalization.Internal;
using AutoMapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NopLocalization
{
    public class ModelLocalizer : IModelLocalizer
    {
        #region Fields
        protected readonly ILocalizedPropertyRepository _localizedPropertyRepository;
        protected readonly IEntityLocalizer _entityLocalizer;
        protected readonly ILanguageRepository _languageRepository;
        protected readonly ILanguageService _languageService;
        protected readonly IModelLocalizedPropertyResolver _modelLocalizedPropertyResolver;
        protected readonly LocalizationOptions _localizationOptions;
        protected readonly IMapper _mapper;
        #endregion

        public ModelLocalizer(
            ILocalizedPropertyRepository localizedPropertyRepository,
            IEntityLocalizer entityLocalizer,
            ILanguageRepository languageRepository,
            ILanguageService languageService,
            IModelLocalizedPropertyResolver modelLocalizedPropertyResolver,
            IMapper mapper,
            IOptionsSnapshot<LocalizationOptions> localizationOptions)
        {
            _localizedPropertyRepository = localizedPropertyRepository;
            _entityLocalizer = entityLocalizer;
            _languageRepository = languageRepository;
            _languageService = languageService;
            _modelLocalizedPropertyResolver = modelLocalizedPropertyResolver;
            _mapper = mapper;
            _localizationOptions = localizationOptions.Value;
        }

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
        #endregion

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
        public virtual Task<TModel> LocalizeAsync<TModel, TEntity>(ILocalizedModel<TModel, TEntity> model,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModel<TModel, TEntity>
            where TEntity : class, ILocalizable
        {
            return LocalizeAsync(model, true, LocalizationDepth.Shallow, cancellationToken);
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
        public virtual Task<TModel> LocalizeAsync<TModel, TEntity>(ILocalizedModel<TModel, TEntity> model,
            bool getOriginalValueIfNotFound,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModel<TModel, TEntity>
            where TEntity : class, ILocalizable
        {
            return LocalizeAsync(model, getOriginalValueIfNotFound, LocalizationDepth.Shallow, cancellationToken);
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
        public virtual Task<TModel> LocalizeAsync<TModel, TEntity>(ILocalizedModel<TModel, TEntity> model,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModel<TModel, TEntity>
            where TEntity : class, ILocalizable
        {
            return LocalizeAsync(model, true, depth, cancellationToken);
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
        public virtual async Task<TModel> LocalizeAsync<TModel, TEntity>(ILocalizedModel<TModel, TEntity> model,
            bool getOriginalValueIfNotFound,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModel<TModel, TEntity>
            where TEntity : class, ILocalizable
        {
            if (model == null) return null;

            var language = await _languageService.GetCurrentLanguageCachedAsync(cancellationToken);

            if (!language.LanguageCode.Equals(_localizationOptions.DefaultLanguage, StringComparison.OrdinalIgnoreCase))
                await LocalizeAsync(model, language.Id, getOriginalValueIfNotFound, depth, cancellationToken);

            return (TModel)model;
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
        public virtual Task<TModel> LocalizeAsync<TModel, TEntity>(ILocalizedModel<TModel, TEntity> model,
            int languageId,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModel<TModel, TEntity>
            where TEntity : class, ILocalizable
        {
            return LocalizeAsync(model, languageId, true, LocalizationDepth.Shallow, cancellationToken);
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
        public virtual Task<TModel> LocalizeAsync<TModel, TEntity>(ILocalizedModel<TModel, TEntity> model,
            int languageId,
            bool getOriginalValueIfNotFound,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModel<TModel, TEntity>
            where TEntity : class, ILocalizable
        {
            return LocalizeAsync(model, languageId, getOriginalValueIfNotFound, LocalizationDepth.Shallow, cancellationToken);
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
        public virtual Task<TModel> LocalizeAsync<TModel, TEntity>(ILocalizedModel<TModel, TEntity> model,
            int languageId,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModel<TModel, TEntity>
            where TEntity : class, ILocalizable
        {
            return LocalizeAsync(model, languageId, true, depth, cancellationToken);
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
        public virtual async Task<TModel> LocalizeAsync<TModel, TEntity>(ILocalizedModel<TModel, TEntity> model,
            int languageId,
            bool getOriginalValueIfNotFound,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModel<TModel, TEntity>
            where TEntity : class, ILocalizable
        {
            if (model == null) return null;

            await LocalizeLocalizedPropertiesAsync(model, languageId, getOriginalValueIfNotFound, depth, cancellationToken);
            await LocalizeNavigationPropertiesAsync(model, languageId, getOriginalValueIfNotFound, depth, cancellationToken);

            return (TModel)model;
        }
        #endregion

        protected virtual async Task LocalizeLocalizedPropertiesAsync<TModel, TEntity>(ILocalizedModel<TModel, TEntity> model, int languageId,
            bool getOriginalValueIfNotFound,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModel<TModel, TEntity>
            where TEntity : class, ILocalizable
        {
            if (model == null) return;

            var type = model.GetType();
            var localizedPropertyInfo = _modelLocalizedPropertyResolver.GetLocalizedPropertyInfo(type);

            foreach (var propInfo in localizedPropertyInfo.LocalizedProperties)
            {
                var entityName = propInfo.EntityProperty.ReflectedType.Name;
                var propertyName = propInfo.EntityProperty.Name;
                var entityId = (int)propInfo.ModelPropertyId.GetValue(model);

                var valueStr = await _localizedPropertyRepository.GetLocalizedValueAsync(entityName, propertyName, entityId, languageId, _localizationOptions.LoadAllLocalizedProperties, cancellationToken);
                var value = GetValue(valueStr, propInfo.ModelProperty.PropertyType, getOriginalValueIfNotFound, () => propInfo.ModelProperty.GetValue(model, null));
                propInfo.ModelProperty.SetValue(model, value, null);
            }
        }

        protected virtual async Task LocalizeNavigationPropertiesAsync<TModel, TEntity>(ILocalizedModel<TModel, TEntity> model, int languageId,
            bool getOriginalValueIfNotFound,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModel<TModel, TEntity>
            where TEntity : class, ILocalizable
        {
            if (model == null) return;
            if (depth == LocalizationDepth.Shallow)
                return;
            if (depth == LocalizationDepth.OneLevel)
                depth = LocalizationDepth.Shallow;

            var type = model.GetType();
            var localizedPropertyInfo = _modelLocalizedPropertyResolver.GetLocalizedPropertyInfo(type);

            var assosicatedProps = localizedPropertyInfo.AssosicatedProperties
                .Select(p => p.GetValue(model, null))
                .Where(p => p != null);

            foreach (dynamic value in assosicatedProps)
            {
                //TODO: check for inherit of ILocalizedModel => prop.PropertyType.GetInterface(typeof(ILocalizedModel<,>)) != null
                await LocalizeAsync(value, languageId, getOriginalValueIfNotFound, depth, cancellationToken);
            }

            var collectionProps = localizedPropertyInfo.CollectionProperties
                .Select(p => p.GetValue(model, null) as IEnumerable<object>)
                .Where(p => p != null && p.Any());

            foreach (var collection in collectionProps)
            {
                foreach (dynamic item in collection.Where(p => p != null))
                    await LocalizeAsync(item, languageId, getOriginalValueIfNotFound, depth, cancellationToken);
            }
        }
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
        public virtual Task<IEnumerable<TModel>> LocalizeAsync<TModel, TEntity>(IEnumerable<ILocalizedModel<TModel, TEntity>> models,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModel<TModel, TEntity>
            where TEntity : class, ILocalizable
        {
            return LocalizeAsync(models, true, LocalizationDepth.Shallow, cancellationToken);
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
        public virtual Task<IEnumerable<TModel>> LocalizeAsync<TModel, TEntity>(IEnumerable<ILocalizedModel<TModel, TEntity>> models,
            bool getOriginalValueIfNotFound,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModel<TModel, TEntity>
            where TEntity : class, ILocalizable
        {
            return LocalizeAsync(models, getOriginalValueIfNotFound, LocalizationDepth.Shallow, cancellationToken);
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
        public virtual Task<IEnumerable<TModel>> LocalizeAsync<TModel, TEntity>(IEnumerable<ILocalizedModel<TModel, TEntity>> models,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModel<TModel, TEntity>
            where TEntity : class, ILocalizable
        {
            return LocalizeAsync(models, true, depth, cancellationToken);
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
        public virtual async Task<IEnumerable<TModel>> LocalizeAsync<TModel, TEntity>(IEnumerable<ILocalizedModel<TModel, TEntity>> models,
            bool getOriginalValueIfNotFound,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModel<TModel, TEntity>
            where TEntity : class, ILocalizable
        {
            if (models?.Any() != true) return null;

            var language = await _languageService.GetCurrentLanguageCachedAsync(cancellationToken);

            if (!language.LanguageCode.Equals(_localizationOptions.DefaultLanguage, StringComparison.OrdinalIgnoreCase))
            {
                foreach (var model in models)
                    await LocalizeAsync(models, language.Id, getOriginalValueIfNotFound, depth, cancellationToken);
            }

            return models.Cast<TModel>();
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
        public virtual Task<IEnumerable<TModel>> LocalizeAsync<TModel, TEntity>(IEnumerable<ILocalizedModel<TModel, TEntity>> models,
            int languageId,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModel<TModel, TEntity>
            where TEntity : class, ILocalizable
        {
            return LocalizeAsync(models, languageId, true, LocalizationDepth.Shallow, cancellationToken);
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
        public virtual Task<IEnumerable<TModel>> LocalizeAsync<TModel, TEntity>(IEnumerable<ILocalizedModel<TModel, TEntity>> models,
            int languageId,
            bool getOriginalValueIfNotFound,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModel<TModel, TEntity>
            where TEntity : class, ILocalizable
        {
            return LocalizeAsync(models, languageId, getOriginalValueIfNotFound, LocalizationDepth.Shallow, cancellationToken);
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
        public virtual Task<IEnumerable<TModel>> LocalizeAsync<TModel, TEntity>(IEnumerable<ILocalizedModel<TModel, TEntity>> models,
            int languageId,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModel<TModel, TEntity>
            where TEntity : class, ILocalizable
        {
            return LocalizeAsync(models, languageId, true, depth, cancellationToken);
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
        public virtual async Task<IEnumerable<TModel>> LocalizeAsync<TModel, TEntity>(IEnumerable<ILocalizedModel<TModel, TEntity>> models,
            int languageId,
            bool getOriginalValueIfNotFound,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModel<TModel, TEntity>
            where TEntity : class, ILocalizable
        {
            if (models?.Any() != true) return null;

            foreach (var model in models)
                await LocalizeAsync(models, languageId, getOriginalValueIfNotFound, depth, cancellationToken);

            return models.Cast<TModel>();
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
        public virtual async Task AddLocalesAsync<TModel, TEntity, TLocalizedModel>(
            ILocalizedModelForEdit<TModel, TEntity, TLocalizedModel> model,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModelForEdit<TModel, TEntity, TLocalizedModel>
            where TEntity : class, ILocalizable
            where TLocalizedModel : class, ILocalizedLocaleModel
        {
            var list = (await _languageRepository.GetAllCachedAsync(cancellationToken))
                .Where(p => !p.LanguageCode.Equals(_localizationOptions.DefaultLanguage, StringComparison.OrdinalIgnoreCase))
                .Select(p => p.Id);

            foreach (var languageId in list)
            {
                var locale = Activator.CreateInstance<TLocalizedModel>();
                locale.LanguageId = languageId;
                model.Locales.Add(locale);
            }
        }

        /// <summary>
        /// Add LocalizedModel item to Locales property of an ILocalizedModelForEdit model to edit
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TLocalizedModel"></typeparam>
        /// <param name="entity"></param>
        /// <param name="model"></param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns></returns>
        public virtual Task AddLocalesAsync<TModel, TEntity, TLocalizedModel>(TEntity entity,
            ILocalizedModelForEdit<TModel, TEntity, TLocalizedModel> model,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModelForEdit<TModel, TEntity, TLocalizedModel>
            where TEntity : class, ILocalizable
            where TLocalizedModel : class, ILocalizedLocaleModel
        {
            return AddLocalesAsync(entity, model, LocalizationDepth.Shallow, cancellationToken);
        }

        /// <summary>
        /// Add LocalizedModel item to Locales property of an ILocalizedModelForEdit model to edit
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TLocalizedModel"></typeparam>
        /// <param name="entity"></param>
        /// <param name="model"></param>
        /// <param name="depth">Depth of localization</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns></returns>
        public virtual async Task AddLocalesAsync<TModel, TEntity, TLocalizedModel>(TEntity entity,
            ILocalizedModelForEdit<TModel, TEntity, TLocalizedModel> model,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModelForEdit<TModel, TEntity, TLocalizedModel>
            where TEntity : class, ILocalizable
            where TLocalizedModel : class, ILocalizedLocaleModel
        {
            var defaultLanguage = _localizationOptions.DefaultLanguage;

            var list = (await _languageRepository.GetAllCachedAsync(cancellationToken))
                .Where(p => !p.LanguageCode.Equals(defaultLanguage, StringComparison.OrdinalIgnoreCase))
                .Select(p => p.Id);

            foreach (var languageId in list)
            {
                var locale = await GetLocalizedModelAsync<TEntity, TLocalizedModel>(entity, languageId, depth, cancellationToken);
                model.Locales.Add(locale);
            }
        }

        protected virtual async Task<TLocalizedModel> GetLocalizedModelAsync<TEntity, TLocalizedModel>(TEntity entity,
            int languageId,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TEntity : class, ILocalizable
            where TLocalizedModel : ILocalizedLocaleModel
        {
            entity.NotNull(nameof(entity));

            var result = default(TLocalizedModel);

            if (languageId > 0)
            {
                var localizedEntity = await _entityLocalizer.LocalizeAsync(entity, languageId, false, depth, cancellationToken);
                result = _mapper.Map<TEntity, TLocalizedModel>(localizedEntity);
                result.LanguageId = languageId;
            }

            return result;
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
        public virtual async Task SaveLocalesAsync<TModel, TEntity, TLocalizedModel>(TEntity entity,
            ILocalizedModelForEdit<TModel, TEntity, TLocalizedModel> model,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModelForEdit<TModel, TEntity, TLocalizedModel>
            where TEntity : class, ILocalizable
            where TLocalizedModel : class, ILocalizedLocaleModel
        {
            foreach (var locale in model.Locales)
            {
                var localizedEntity = _mapper.Map<TLocalizedModel, TEntity>(locale);
                localizedEntity.Id = entity.Id;
                await _entityLocalizer.SaveLocalizedAsync(localizedEntity, locale.LanguageId, cancellationToken);
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
        public virtual async Task SaveLocalesAsync<TModel, TEntity, TLocalizedModel>(TEntity entity,
            ILocalizedModelForEdit<TModel, TEntity, TLocalizedModel> model,
            LocalizationDepth depth,
            CancellationToken cancellationToken = default)
            where TModel : class, ILocalizedModelForEdit<TModel, TEntity, TLocalizedModel>
            where TEntity : class, ILocalizable
            where TLocalizedModel : class, ILocalizedLocaleModel
        {
            foreach (var locale in model.Locales)
            {
                var localizedEntity = _mapper.Map<TLocalizedModel, TEntity>(locale);
                localizedEntity.Id = entity.Id;
                await _entityLocalizer.SaveLocalizedAsync(localizedEntity, locale.LanguageId, depth, cancellationToken);
            }
        }
        #endregion

        #region Other Localized Extention
        ///// <summary>
        ///// Get localized property of setting
        ///// </summary>
        ///// <typeparam name="TSettings">Settings type</typeparam>
        ///// <param name="settings">Settings</param>
        ///// <param name="keySelector">Key selector</param>
        ///// <param name="languageId">Language identifier</param>
        ///// <param name="returnDefaultValue">A value indicating whether to return default value (if localized is not found)</param>
        ///// <param name="ensureTwoPublishedLanguages">A value indicating whether to ensure that we have at least two published languages; otherwise, load only default value</param>
        ///// <returns>Localized property</returns>
        //public virtual string GetLocalizedSetting<TSettings>(TSettings settings, Expression<Func<TSettings, string>> keySelector,
        //    int languageId, bool returnDefaultValue = true)
        //    where TSettings : ISettings, new()
        //{
        //    var key = _settingService.GetSettingKey(settings, keySelector);

        //    //we do not support localized settings per store (overridden store settings)
        //    var setting = _settingService.GetSetting(key);
        //    if (setting == null)
        //        return null;

        //    return GetLocalized(setting, x => x.Value, languageId, returnDefaultValue);
        //}

        ///// <summary>
        ///// Save localized property of setting
        ///// </summary>
        ///// <typeparam name="TSettings">Settings type</typeparam>
        ///// <param name="settings">Settings</param>
        ///// <param name="keySelector">Key selector</param>
        ///// <param name="languageId">Language identifier</param>
        ///// <param name="value">Localized value</param>
        ///// <returns>Localized property</returns>
        //public virtual void SaveLocalizedSetting<TSettings>(TSettings settings, Expression<Func<TSettings, string>> keySelector,
        //    int languageId, string value) where TSettings : ISettings, new()
        //{
        //    var key = _settingService.GetSettingKey(settings, keySelector);

        //    //we do not support localized settings per store (overridden store settings)
        //    var setting = _settingService.GetSetting(key);
        //    if (setting == null)
        //        return;

        //    _localizedEntityService.SaveLocalizedValue(setting, x => x.Value, value, languageId);
        //}

        ///// <summary>
        ///// Get localized value of enum
        ///// </summary>
        ///// <typeparam name="TEnum">Enum type</typeparam>
        ///// <param name="enumValue">Enum value</param>
        ///// <param name="languageId">Language identifier; pass null to use the current working language</param>
        ///// <returns>Localized value</returns>
        //public virtual string GetLocalizedEnum<TEnum>(TEnum enumValue, int? languageId = null) where TEnum : struct
        //{
        //    if (!typeof(TEnum).IsEnum)
        //        throw new ArgumentException("T must be an enumerated type");

        //    //localized value
        //    var resourceName = $"{NopLocalizationDefaults.EnumLocaleStringResourcesPrefix}{typeof(TEnum)}.{enumValue}";
        //    var result = GetResource(resourceName, languageId ?? _workContext.WorkingLanguage.Id, false, string.Empty, true);

        //    //set default value if required
        //    if (string.IsNullOrEmpty(result))
        //        result = StringHelper.ConvertEnum(enumValue.ToString()); //Humanize name of enum value

        //    return result;
        //}
        #endregion
    }
}