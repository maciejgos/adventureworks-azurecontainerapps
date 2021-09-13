using AdventureWorksCore.Shared;

namespace AdventureWorksCore.Core.Domain
{
    public abstract class ProductCategory : BaseEntity
    {
        public string Name { get; set; }
    }
}