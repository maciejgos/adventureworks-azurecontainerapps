using System.Threading;
using System.Threading.Tasks;
using AdventureWorksCore.Api.Application.Commands;
using AdventureWorksCore.Api.Application.Models.Common;
using Xunit;

namespace AdventureWorksCore.Tests.Commands
{
    public class DeleteProductCommandTests
    {
        public DeleteProductCommandTests()
        {
        }

        [Fact]
        public async Task DeleteProductReturnResultObject()
        {
            // Arrange
            var command = new DeleteProductCommand(0);
            var handler = new DeleteProductCommandHandler();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<ResultObject>(result);
        }

        [Fact]
        public async Task SuccessfullDeleteProductReturnSuccessResultObject()
        {
            // Arrange
            var command = new DeleteProductCommand(0);
            var handler = new DeleteProductCommandHandler();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
        }
    }
}
