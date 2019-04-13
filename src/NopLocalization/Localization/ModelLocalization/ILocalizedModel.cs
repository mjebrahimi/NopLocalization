namespace NopLocalization
{
    /// <summary>
    /// Represent a model that can be localized and specify mapping between localized model and entity
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public interface ILocalizedModel<TModel, TEntity>
        where TModel : class, ILocalizedModel<TModel, TEntity> //Must be itself
        where TEntity : class, ILocalizable
    {
        int Id { get; set; }
    }
}
