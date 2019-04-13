using System.Collections.Generic;

namespace NopLocalization.Sample.Controllers
{
    public class Category : ILocalizable
    {
        public int Id { get; set; }
        [Localized]
        public string Name { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }

    public class Product : ILocalizable
    {
        public int Id { get; set; }
        [Localized]
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
