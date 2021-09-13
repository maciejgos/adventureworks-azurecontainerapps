using System.Threading;
using System.Threading.Tasks;
using AdventureWorksCore.Api.Application.Commands;
using AdventureWorksCore.Api.Application.Models.Common;
using Xunit;

namespace AdventureWorksCore.Tests.Commands
{
    public class UpdateProductCommandTests
    {
        public UpdateProductCommandTests()
        {
        }

        [Fact]
        public async Task UpdateProductReturnResultObject()
        {
            // Arrange
            var command = new UpdateProductCommand();
            var handler = new UpdateProductCommandHandler();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<ResultObject>(result);
        }

        [Fact]
        public async Task UpdateProductReturnSuccessResultObject()
        {
            // Arrange
            var command = new UpdateProductCommand();
            var handler = new UpdateProductCommandHandler();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
        }
    }
}
