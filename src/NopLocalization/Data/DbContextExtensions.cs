using NopLocalization.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NopLocalization
{
    public static class DbContextExtensions
    {
        public static async Task<List<LocalizedProperty>> RemoveLocalizedPropertiesAsync(this DbContext dbContext, CancellationToken cancellationToken)
        {
            var removedEntities = dbContext.ChangeTracker.Entries<ILocalizable>()
                .Where(p => p.State == EntityState.Deleted)
                .Select(p => new
                {
                    EntityId = p.Entity.Id,
                    EntityName = p.Entity.GetType().GetUnproxiedEntityType().Name
                }).ToList();

            if (removedEntities.Count == 0)
                return new List<LocalizedProperty>();

            var localizedProperties = dbContext.Set<LocalizedProperty>();

            var removedProperties = await localizedProperties.Where(p => removedEntities.Any(x => x.EntityName == p.EntityName && x.EntityId == p.EntityId)).ToListAsync();

            localizedProperties.RemoveRange(removedProperties);

            return removedProperties;
        }

        public static List<LocalizedProperty> RemoveLocalizedProperties(this DbContext dbContext)
        {
            var removedEntities = dbContext.ChangeTracker.Entries<ILocalizable>()
                .Where(p => p.State == EntityState.Deleted)
                .Select(p => new
                {
                    EntityId = p.Entity.Id,
                    EntityName = p.Entity.GetType().GetUnproxiedEntityType().Name
                }).ToList();

            if (removedEntities.Count == 0)
                return new List<LocalizedProperty>();

            var localizedProperties = dbContext.Set<LocalizedProperty>();

            var removedProperties = localizedProperties.Where(p => removedEntities.Any(x => x.EntityName == p.EntityName && x.EntityId == p.EntityId)).ToList();

            localizedProperties.RemoveRange(removedProperties);

            return removedProperties;
        }

        public static void InvalidateLocalizedPropertiesCache(this DbContext dbContext, List<LocalizedProperty> removedProperties)
        {
            var serviceProvider = dbContext.GetInfrastructure();
            var localizedPropertyCacheUpdater = serviceProvider.GetRequiredService<ILocalizedPropertyCacheInvalidator>();

            localizedPropertyCacheUpdater.InvalidateCache(removedProperties, CacheInvalidationOperation.Delete);
        }

        public static void AddLocalization(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LocalizedProperty>().ToTable("LocalizedProperties");
            modelBuilder.Entity<Language>().ToTable("Languages").HasIndex(p => p.LanguageCode).IsUnique();
        }
    }
}
