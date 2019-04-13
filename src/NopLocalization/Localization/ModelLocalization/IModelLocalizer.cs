using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NopLocalization
{
    public interface IModelLocalizer
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
        Task<TModel> LocalizeAsync<TModel, TEntity>(ILocalizedModel<TModel, TEntity> model, CancellationToken cancellationToken = default) where TModel : class, ILocalizedModel<TModel, TEntity> where TEntity : class, ILocalizable;

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
        Task<TModel> LocalizeAsync<TModel, TEntity>(ILocalizedModel<TModel, TEntity> model, bool getOriginalValueIfNotFound, CancellationToken cancellationToken = default) where TModel : class, ILocalizedModel<TModel, TEntity> where TEntity : class, ILocalizable;

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
        Task<TModel> LocalizeAsync<TModel, TEntity>(ILocalizedModel<TModel, TEntity> model, LocalizationDepth depth, CancellationToken cancellationToken = default) where TModel : class, ILocalizedModel<TModel, TEntity> where TEntity : class, ILocalizable;
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
        Task<TModel> LocalizeAsync<TModel, TEntity>(ILocalizedModel<TModel, TEntity> model, bool getOriginalValueIfNotFound, LocalizationDepth depth, CancellationToken cancellationToken = default) where TModel : class, ILocalizedModel<TModel, TEntity> where TEntity : class, ILocalizable;
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
        Task<TModel> LocalizeAsync<TModel, TEntity>(ILocalizedModel<TModel, TEntity> model, int languageId, CancellationToken cancellationToken = default) where TModel : class, ILocalizedModel<TModel, TEntity> where TEntity : class, ILocalizable;

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
        Task<TModel> LocalizeAsync<TModel, TEntity>(ILocalizedModel<TModel, TEntity> model, int languageId, bool getOriginalValueIfNotFound, CancellationToken cancellationToken = default) where TModel : class, ILocalizedModel<TModel, TEntity> where TEntity : class, ILocalizable;

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
        Task<TModel> LocalizeAsync<TModel, TEntity>(ILocalizedModel<TModel, TEntity> model, int languageId, LocalizationDepth depth, CancellationToken cancellationToken = default) where TModel : class, ILocalizedModel<TModel, TEntity> where TEntity : class, ILocalizable;
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
        Task<TModel> LocalizeAsync<TModel, TEntity>(ILocalizedModel<TModel, TEntity> model, int languageId, bool getOriginalValueIfNotFound, LocalizationDepth depth, CancellationToken cancellationToken = default) where TModel : class, ILocalizedModel<TModel, TEntity> where TEntity : class, ILocalizable;
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
        Task<IEnumerable<TModel>> LocalizeAsync<TModel, TEntity>(IEnumerable<ILocalizedModel<TModel, TEntity>> models, CancellationToken cancellationToken = default) where TModel : class, ILocalizedModel<TModel, TEntity> where TEntity : class, ILocalizable;

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
        Task<IEnumerable<TModel>> LocalizeAsync<TModel, TEntity>(IEnumerable<ILocalizedModel<TModel, TEntity>> models, bool getOriginalValueIfNotFound, CancellationToken cancellationToken = default) where TModel : class, ILocalizedModel<TModel, TEntity> where TEntity : class, ILocalizable;

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
        Task<IEnumerable<TModel>> LocalizeAsync<TModel, TEntity>(IEnumerable<ILocalizedModel<TModel, TEntity>> models, LocalizationDepth depth, CancellationToken cancellationToken = default) where TModel : class, ILocalizedModel<TModel, TEntity> where TEntity : class, ILocalizable;
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
        Task<IEnumerable<TModel>> LocalizeAsync<TModel, TEntity>(IEnumerable<ILocalizedModel<TModel, TEntity>> models, bool getOriginalValueIfNotFound, LocalizationDepth depth, CancellationToken cancellationToken = default) where TModel : class, ILocalizedModel<TModel, TEntity> where TEntity : class, ILocalizable;
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
        Task<IEnumerable<TModel>> LocalizeAsync<TModel, TEntity>(IEnumerable<ILocalizedModel<TModel, TEntity>> models, int languageId, CancellationToken cancellationToken = default) where TModel : class, ILocalizedModel<TModel, TEntity> where TEntity : class, ILocalizable;

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
        Task<IEnumerable<TModel>> LocalizeAsync<TModel, TEntity>(IEnumerable<ILocalizedModel<TModel, TEntity>> models, int languageId, bool getOriginalValueIfNotFound, CancellationToken cancellationToken = default) where TModel : class, ILocalizedModel<TModel, TEntity> where TEntity : class, ILocalizable;

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
        Task<IEnumerable<TModel>> LocalizeAsync<TModel, TEntity>(IEnumerable<ILocalizedModel<TModel, TEntity>> models, int languageId, LocalizationDepth depth, CancellationToken cancellationToken = default) where TModel : class, ILocalizedModel<TModel, TEntity> where TEntity : class, ILocalizable;
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
        Task<IEnumerable<TModel>> LocalizeAsync<TModel, TEntity>(IEnumerable<ILocalizedModel<TModel, TEntity>> models, int languageId, bool getOriginalValueIfNotFound, LocalizationDepth depth, CancellationToken cancellationToken = default) where TModel : class, ILocalizedModel<TModel, TEntity> where TEntity : class, ILocalizable;
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
        Task AddLocalesAsync<TModel, TEntity, TLocalizedModel>(ILocalizedModelForEdit<TModel, TEntity, TLocalizedModel> model, CancellationToken cancellationToken = default) where TModel : class, ILocalizedModelForEdit<TModel, TEntity, TLocalizedModel> where TEntity : class, ILocalizable where TLocalizedModel : class, ILocalizedLocaleModel;

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
        Task AddLocalesAsync<TModel, TEntity, TLocalizedModel>(TEntity entity, ILocalizedModelForEdit<TModel, TEntity, TLocalizedModel> model, CancellationToken cancellationToken = default) where TModel : class, ILocalizedModelForEdit<TModel, TEntity, TLocalizedModel> where TEntity : class, ILocalizable where TLocalizedModel : class, ILocalizedLocaleModel;

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
        Task AddLocalesAsync<TModel, TEntity, TLocalizedModel>(TEntity entity, ILocalizedModelForEdit<TModel, TEntity, TLocalizedModel> model, LocalizationDepth depth, CancellationToken cancellationToken = default) where TModel : class, ILocalizedModelForEdit<TModel, TEntity, TLocalizedModel> where TEntity : class, ILocalizable where TLocalizedModel : class, ILocalizedLocaleModel;
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
        Task SaveLocalesAsync<TModel, TEntity, TLocalizedModel>(TEntity entity, ILocalizedModelForEdit<TModel, TEntity, TLocalizedModel> model, CancellationToken cancellationToken = default) where TModel : class, ILocalizedModelForEdit<TModel, TEntity, TLocalizedModel> where TEntity : class, ILocalizable where TLocalizedModel : class, ILocalizedLocaleModel;


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
        Task SaveLocalesAsync<TModel, TEntity, TLocalizedModel>(TEntity entity, ILocalizedModelForEdit<TModel, TEntity, TLocalizedModel> model, LocalizationDepth depth, CancellationToken cancellationToken = default) where TModel : class, ILocalizedModelForEdit<TModel, TEntity, TLocalizedModel> where TEntity : class, ILocalizable where TLocalizedModel : class, ILocalizedLocaleModel;
        #endregion
    }
}