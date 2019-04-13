using System.Collections.Generic;

namespace NopLocalization
{
    /// <summary>
    /// It has Locale models in several languages for editing and specify mapping between model and entity
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TLocalizedModel"></typeparam>
    public interface ILocalizedModelForEdit<TModel, TEntity, TLocalizedModel>
        where TModel : class, ILocalizedModelForEdit<TModel, TEntity, TLocalizedModel> //Must be itself
        where TEntity : class, ILocalizable
        where TLocalizedModel : ILocalizedLocaleModel
    {
        int Id { get; set; }
        List<TLocalizedModel> Locales { get; set; }
    }

    public interface ILocalizedLocaleModel
    {
        int LanguageId { get; set; }
    }
}
