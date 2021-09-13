namespace AdventureWorksCore.Api.Application.Models
{
    public class ProductListModel
    {
        public ProductListModel()
        {
        }

        public string Name { get; internal set; }
        public string ProductNumber { get; internal set; }
        public string ProductLine { get; internal set; }
        public int Id { get; internal set; }
    }
}
