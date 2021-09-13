using System;
using System.Threading;
using System.Threading.Tasks;
using AdventureWorksCore.Api.Application.Models.Common;
using AdventureWorksCore.Core.Domain;
using AdventureWorksCore.Infrastructure.Persistence;
using MediatR;

namespace AdventureWorksCore.Api.Application.Commands
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ResultObject>
    {
        private readonly ApplicationDbContext _context;

        public CreateProductCommandHandler(ApplicationDbContext context = default)
        {
            _context = context;
        }

        public async Task<ResultObject> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            if (_context == default)
                return ResultObject.CreateSuccessResult();

            var product = new Product
            {
                Name = request.ProductName,
                ProductNumber = request.ProductNumber,
                MakeFlag = request.MakeFlag,
                FinishedGoodsFlag = request.FinishedGoodsFlag,
                Color = request.ProductColor,
                SafetyStockLevel = request.SafetyStockLevel,
                ReorderPoint = request.ReorderPoint,
                StandardCost = request.StandardCost,
                ListPrice = request.ListPrice,
                Size = request.Size,
                SizeUnitMeasureCode = request.SizeUnitMeasureCode,
                WeightUnitMeasureCode = request.WeightUnitMeasureCode,
                Weight = request.Weight,
                DaysToManufacture = request.DaysToManufacture,
                ProductLine = request.ProductLine,
                Class = request.Class,
                Style = request.Style,
                SellStartDate = request.SellStartDate,
                SellEndDate = request.SellEndDate,
                DiscontinuedDate = request.DiscontinuedDate,
                ModifiedDate = System.DateTime.UtcNow,
                RowGuid = System.Guid.NewGuid()
            };

            await _context.Products.AddAsync(product, CancellationToken.None);
            await _context.SaveChangesAsync(CancellationToken.None);

            return ResultObject.CreateSuccessResult();
        }
    }

    public class CreateProductCommand : IRequest<ResultObject>
    {
        public string ProductName { get; set; }
        public string ProductNumber { get; set; }
        public bool MakeFlag { get; set; }
        public bool FinishedGoodsFlag { get; set; }
        public string ProductColor { get; set; }
        public short SafetyStockLevel { get; set; }
        public short ReorderPoint { get; set; }
        public decimal StandardCost { get; set; }
        public decimal ListPrice { get; set; }
        public string Size { get; set; }
        public string SizeUnitMeasureCode { get; set; }
        public string WeightUnitMeasureCode { get; set; }
        public decimal? Weight { get; set; }
        public int DaysToManufacture { get; set; }
        public string ProductLine { get; set; }
        public string Class { get; set; }
        public string Style { get; set; }
        public DateTime SellStartDate { get; set; }
        public DateTime? SellEndDate { get; set; }
        public DateTime? DiscontinuedDate { get; set; }
    }
}
