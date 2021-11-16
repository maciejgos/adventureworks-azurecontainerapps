using System.Collections.Generic;
using System.Threading.Tasks;
using AdventureWorksCore.Api.Application.Models;
using AdventureWorksCore.Api.Application.Queries;
using Xunit;
using System.Threading;

namespace AdventureWorksCore.Tests.Queries
{
    public class GetProductQueryTests
    {
        public GetProductQueryTests()
        {
        }

        [Fact]
        public async Task CanGetCollectionOfProductListModelType()
        {
            // Arrange
            var query = new GetProductsListQuery();
            var handler = new GetProductsListQueryHandler();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsType<List<ProductListModel>>(result);
        }

        [Fact]
        public async Task CanGetDetailsOfProductModelType()
        {
            // Arrange
            var query = new GetProductQuery(id: 1);
            var handler = new GetProductQueryHandler();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsType<ProductModel>(result);
        }
    }
}
