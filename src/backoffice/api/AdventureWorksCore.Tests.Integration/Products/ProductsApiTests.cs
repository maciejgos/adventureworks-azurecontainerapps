using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using AdventureWorksCore.Api;
using AdventureWorksCore.Api.Application.Commands;
using AdventureWorksCore.Api.Application.Models;
using AdventureWorksCore.Api.Application.Models.Common;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace AdventureWorksCore.Tests.Integration
{
    public class ProductsApiTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        private readonly WebApplicationFactory<Startup> _factory;

        public ProductsApiTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get_ProductsEndpointReturnSuccessAndContent()
        {
            // Arrange
            var client = _factory.CreateClient();
            var url = "api/products";

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var collection = JsonSerializer.Deserialize<IEnumerable<ProductListModel>>(jsonString);

            Assert.True(response.IsSuccessStatusCode);
            Assert.IsType<List<ProductListModel>>(collection);
            Assert.NotEmpty(collection);
        }

        [Fact]
        public async Task Get_ProductEndpointReturnSuccessAndContent()
        {
            // Arrange
            var client = _factory.CreateClient();
            var url = "api/products/1";

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ProductModel>(jsonString);

            Assert.True(response.IsSuccessStatusCode);
            Assert.IsType<ProductModel>(result);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Post_ProductEndpointReturnSuccess()
        {
            // Arrange
            var client = _factory.CreateClient();
            var url = "api/products";
            var number = new System.Random().Next().ToString().Substring(0, 4);

            var command = new CreateProductCommand
            {
                ProductName = string.Concat("Test_", number),
                ProductNumber = string.Concat("AR-", number),
                ProductLine = "R",
                Class = "H",
                ProductColor = "Color",
                DaysToManufacture = 5,
                FinishedGoodsFlag = true,
                ListPrice = 12.5m,
                StandardCost = 25.5m,
                MakeFlag = true,
                ReorderPoint = 1,
                SafetyStockLevel = 1,
                SellStartDate = System.DateTime.UtcNow.AddYears(-1),
                Size = "M",
                SizeUnitMeasureCode = "CM",
                Style = "U",
                WeightUnitMeasureCode = "LB"
            };
            var content = new StringContent(JsonSerializer.Serialize(command), System.Text.Encoding.Default, "application/json");

            // Act
            var response = await client.PostAsync(url, content);

            // Assert
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ResultModel>(jsonString, _serializerOptions);

            Assert.True(response.IsSuccessStatusCode);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task Put_ProductEndpointReturnSuccess()
        {
            // Arrange
            var client = _factory.CreateClient();
            var url = "api/products/1";
            var number = new System.Random().Next().ToString().Substring(0, 4);

            var command = new UpdateProductCommand
            {
                Id = 1,
                Name = string.Concat("Test_", number),
                ProductNumber = string.Concat("AR-", number),
                ProductLine = "R",
                Class = "H",
                Color = "Color",
                DaysToManufacture = 5,
                FinishedGoodsFlag = true,
                ListPrice = 12.5m,
                StandardCost = 25.5m,
                MakeFlag = true,
                ReorderPoint = 1,
                SafetyStockLevel = 1,
                SellStartDate = System.DateTime.UtcNow.AddYears(-1),
                Size = "M",
                SizeUnitMeasureCode = "CM",
                Style = "U",
                WeightUnitMeasureCode = "LB"
            };
            var content = new StringContent(JsonSerializer.Serialize(command), System.Text.Encoding.Default, "application/json");

            // Act
            var response = await client.PutAsync(url, content);

            // Assert
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ResultModel>(jsonString, _serializerOptions);

            Assert.True(response.IsSuccessStatusCode);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task Delete_ProductEndpointReturnSuccess()
        {
            // Arrange
            var client = _factory.CreateClient();

            var allProductsUrl = "api/products";
            var allProductsResponse = await client.GetAsync(allProductsUrl);
            allProductsResponse.EnsureSuccessStatusCode();
            var allProductsJson = await allProductsResponse.Content.ReadAsStringAsync();
            var collection = JsonSerializer.Deserialize<IEnumerable<ProductListModel>>(allProductsJson);

            var url = string.Concat("api/products/", collection.LastOrDefault().Id.ToString());

            // Act
            var response = await client.DeleteAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ResultModel>(jsonString, _serializerOptions);

            Assert.True(allProductsResponse.IsSuccessStatusCode);
            Assert.True(result.Success);
        }
    }

    class ResultModel
    {
        public bool Success { get; set; }
    }
}
