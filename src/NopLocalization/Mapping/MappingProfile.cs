using NopLocalization.Internal;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NopLocalization
{
    public class MappingProfile : Profile
    {
        public MappingProfile(IEnumerable<Assembly> assemblies)
        {
            var types = assemblies
                .Where(p => !p.IsDynamic)
                .SelectMany(p => p.GetExportedTypes())
                .Where(type => type.IsClass && !type.IsAbstract).ToList();

            AddCreateMapping(types);
            AddLocalizedModelMapping(types);
            AddLocalizedModelForEditMapping(types);

            AddEntityCacheMapping();

            #region Comment
            //MapperConfiguration => Singleton
            //IMapper => Scoped

            //var mce = new MapperConfigurationExpression();
            //mce.CreateMap<Foo, Bar>();
            //var config = new MapperConfiguration(mce);

            //var config = new MapperConfiguration(cfg =>
            //{
            //    cfg.ConstructServicesUsing(ApplicationServiceProvider.ServiceProvider.GetRequiredService);
            //    //cfg.CreateMap<Foo, Bar>();
            //    //cfg.AddProfile<FooProfile>();
            //    //cfg.AddProfiles(assmebly1, assembly2, ...);
            //    //cfg.AddProfiles("Foo.UI", "Foo.Core", ...);
            //    //cfg.AddProfiles(typeof(HomeController), typeof(Entity, ...);
            //});
            //config.AssertConfigurationIsValid();
            //config.CompileMappings();

            //IMapper mapper = config.CreateMapper(ApplicationServiceProvider.ServiceProvider.GetRequiredService);
            //IMapper mapper = new Mapper(config);
            //IMapper mapper = new Mapper(config, ApplicationServiceProvider.ServiceProvider.GetRequiredService);
            #endregion
        }

        private void AddEntityCacheMapping()
        {
            CreateMap<Language, LanguageCached>();
            CreateMap<LocalizedProperty, LocalizedPropertyCached>();
        }

        private void AddCreateMapping(IEnumerable<Type> allTypes)
        {
            var types = allTypes
                .Where(type => type.IsInheritFrom<IHaveMapping>());

            foreach (var type in types)
            {
                var item = (IHaveMapping)Activator.CreateInstance(type);
                item.CreateMapping(this);
            }
        }

        private void AddLocalizedModelMapping(IEnumerable<Type> allTypes)
        {
            var types = allTypes
                .Select(type => type.GetInterface(typeof(ILocalizedModel<,>)))
                .Where(type => type != null);

            foreach (var type in types)
            {
                var arguments = type.GetGenericArguments();
                CreateMap(arguments[0], arguments[1]).ReverseMap();
            }
        }

        private void AddLocalizedModelForEditMapping(IEnumerable<Type> allTypes)
        {
            var types = allTypes
                .Select(type => type.GetInterface(typeof(ILocalizedModelForEdit<,,>)))
                .Where(type => type != null);

            foreach (var type in types)
            {
                var arguments = type.GetGenericArguments();
                CreateMap(arguments[0], arguments[1]).ReverseMap();
                CreateMap(arguments[2], arguments[1]).ReverseMap();
            }
        }
    }
}
