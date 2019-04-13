using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace NopLocalization.Sample.Controllers
{
    public class AppDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        public AppDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.AddLocalization();
            modelBuilder.Entity<Category>().HasData(new Category
            {
                Id = 1,
                Name = "Category1"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                Id = 1,
                CategoryId = 1,
                Name = "Product1",
            });
        }

        public override int SaveChanges()
        {
            var removedProperties =  this.RemoveLocalizedProperties();

            var result =  base.SaveChanges();

            this.InvalidateLocalizedPropertiesCache(removedProperties);

            return result;
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            var removedProperties = this.RemoveLocalizedProperties();

            var result = base.SaveChanges(acceptAllChangesOnSuccess);

            this.InvalidateLocalizedPropertiesCache(removedProperties);

            return result;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var removedProperties = await this.RemoveLocalizedPropertiesAsync(cancellationToken);

            var result = await base.SaveChangesAsync(cancellationToken);

            this.InvalidateLocalizedPropertiesCache(removedProperties);

            return result;
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            var removedProperties = await this.RemoveLocalizedPropertiesAsync(cancellationToken);

            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            this.InvalidateLocalizedPropertiesCache(removedProperties);

            return result;
        }
    }
}
