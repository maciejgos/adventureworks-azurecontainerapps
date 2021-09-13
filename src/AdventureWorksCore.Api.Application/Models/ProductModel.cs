using System;
namespace AdventureWorksCore.Api.Application.Models
{
    public class ProductModel
    {
        public ProductModel()
        {
        }

        public int Id { get; internal set; }
        public string Name { get; internal set; }
        public decimal StandardCost { get; internal set; }
        public decimal ListPrice { get; internal set; }
    }
}
