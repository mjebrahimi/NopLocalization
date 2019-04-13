using NopLocalization.Internal;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace NopLocalization
{
    /// <summary>
    /// Represent a model that can be localized and specify mapping between localized model and entity
    /// </summary>
    /// <typeparam name="TLocalizedModel"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class LocalizedModel<TLocalizedModel, TEntity> : ILocalizedModel<TLocalizedModel, TEntity>, IHaveMapping
        where TLocalizedModel : LocalizedModel<TLocalizedModel, TEntity>
        where TEntity : class, ILocalizable
    {
        protected static Lazy<IMapper> Mapper = new Lazy<IMapper>(() => ApplicationServiceProvider.ServiceProvider.GetRequiredService<IMapper>());
        public virtual int Id { get; set; }

        public virtual TEntity ToEntity()
        {
            return Mapper.Value.Map<TLocalizedModel, TEntity>(CastToDerivedClass(this));
        }

        public virtual TEntity ToEntity(TEntity entity)
        {
            return Mapper.Value.Map(CastToDerivedClass(this), entity);
        }

        public static TLocalizedModel FromEntity(TEntity model)
        {
            return Mapper.Value.Map<TEntity, TLocalizedModel>(model);
        }

        protected TLocalizedModel CastToDerivedClass(LocalizedModel<TLocalizedModel, TEntity> baseInstance)
        {
            //TODO: Check this cast
            return (TLocalizedModel)baseInstance;
            //return Mapper.Map<TLocalizedModel>(baseInstance);
        }

        /// <summary>
        /// </summary>
        /// <param name="config"></param>
        void IHaveMapping.CreateMapping(Profile profile)
        {

            var mappingExpression = profile.CreateMap<TLocalizedModel, TEntity>();

            //TODO: verify is necessary
            //var modelType = typeof(TLocalizedModel);
            //var entityType = typeof(TEntity);
            //Ignore mapping to any property of source (like Post.Categroy) that dose not contains in destination (like PostDto)
            //foreach (var property in entityType.GetProperties())
            //{
            //    if (modelType.GetProperty(property.Name) == null)
            //        mappingExpression.ForMember(property.Name, opt => opt.Ignore());
            //}

            CustomMapping(mappingExpression.ReverseMap());
        }

        public virtual void CustomMapping(IMappingExpression<TEntity, TLocalizedModel> mapping)
        {
        }
    }
}
