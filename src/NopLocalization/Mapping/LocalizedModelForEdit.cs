using NopLocalization.Internal;
using AutoMapper;
using System.Collections.Generic;

namespace NopLocalization
{
    /// <summary>
    /// It has Locale models in several languages for editing and specify mapping between model and entity
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TLocalizedModel"></typeparam>
    public abstract class LocalizedModelForEdit<TModel, TEntity, TLocalizedModel> : ILocalizedModelForEdit<TModel, TEntity, TLocalizedModel>, IHaveMapping
        where TModel : LocalizedModelForEdit<TModel, TEntity, TLocalizedModel>
        where TEntity : class, ILocalizable
        where TLocalizedModel : ILocalizedLocaleModel
    {
        public virtual int Id { get; set; }
        public virtual List<TLocalizedModel> Locales { get; set; }

        public virtual TEntity ToEntity()
        {
            return Mapper.Map<TModel, TEntity>(CastToDerivedClass(this));
        }

        public virtual TEntity ToEntity(TEntity entity)
        {
            return Mapper.Map(CastToDerivedClass(this), entity);
        }

        public static TModel FromEntity(TEntity model)
        {
            return Mapper.Map<TEntity, TModel>(model);
        }

        protected TModel CastToDerivedClass(LocalizedModelForEdit<TModel, TEntity, TLocalizedModel> baseInstance)
        {
            //TODO: Check this cast
            return (TModel)baseInstance;
            //return Mapper.Map<TLocalizedModel>(baseInstance);
        }

        void IHaveMapping.CreateMapping(Profile profile)
        {
            var modelMapping = profile.CreateMap<TModel, TEntity>();
            var localeModelMapping = profile.CreateMap<TLocalizedModel, TEntity>();

            //TODO: verify is necessary
            //var modelType = typeof(TLocalizedModel);
            //var entityType = typeof(TEntity);
            //var localeModelType = typeof(TLocalizedLocaleModel);
            ////Ignore mapping to any property of source (like Post.Categroy) that dose not contains in destination (like PostDto)
            //foreach (var property in entityType.GetProperties())
            //{
            //    if (modelType.GetProperty(property.Name) == null)
            //        modelMapping.ForMember(property.Name, opt => opt.Ignore());
            //}
            //foreach (var property in entityType.GetProperties())
            //{
            //    if (localeModelType.GetProperty(property.Name) == null)
            //        localeModelMapping.ForMember(property.Name, opt => opt.Ignore());
            //}

            CustomMappings(modelMapping.ReverseMap(), localeModelMapping.ReverseMap());
        }

        public virtual void CustomMappings(IMappingExpression<TEntity, TModel> modelMapping,
           IMappingExpression<TEntity, TLocalizedModel> localeModelMapping)
        {
        }
    }
}
