using System;
using System.Threading;
using System.Threading.Tasks;
using AdventureWorksCore.Api.Application.Commands;
using AdventureWorksCore.Api.Application.Models.Common;
using Xunit;

namespace AdventureWorksCore.Tests.Commands
{
    public class CreateProductCommandTests
    {
        public CreateProductCommandTests()
        {
        }

        [Fact]
        public async Task CreateProductReturnResultObject()
        {
            // Arrange
            var command = new CreateProductCommand();
            var handler = new CreateProductCommandHandler();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<ResultObject>(result);
        }

        [Fact]
        public async Task SuccessfullCreateProductReturnSuccessResultObject()
        {
            // Arrange
            var command = new CreateProductCommand();
            var handler = new CreateProductCommandHandler();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
        }
    }
}
