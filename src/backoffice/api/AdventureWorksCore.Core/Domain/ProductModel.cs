using AdventureWorksCore.Shared;

namespace AdventureWorksCore.Core.Domain
{
    public abstract class ProductModel : BaseEntity
    {
        public string Name { get; set; }
        public string CatalogDescription { get; set; }
        public string Instructions { get; set; }
    }
}