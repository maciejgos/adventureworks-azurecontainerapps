using AdventureWorksCore.Shared;

namespace AdventureWorksCore.Core.Domain
{
    public abstract class ProductSubCategory : BaseEntity
    {
        public string Name { get; set; }
        public ProductCategory ProductCategory { get; set; }
    }
}