using System.Collections.Generic;

namespace NopLocalization.Sample.Controllers
{
    public class CategoryModel : ILocalizedModel<CategoryModel, Category>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ProductModel> Products { get; set; }
    }

    public class ProductModel : ILocalizedModel<ProductModel, Product>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        //public CategoryModel Category { get; set; }
    }
}
