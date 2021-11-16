using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdventureWorksCore.Api.Application.Models.Common;
using AdventureWorksCore.Infrastructure.Persistence;
using MediatR;

namespace AdventureWorksCore.Api.Application.Commands
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ResultObject>
    {
        private readonly ApplicationDbContext _context;

        public UpdateProductCommandHandler(ApplicationDbContext context = default)
        {
            _context = context;
        }

        public async Task<ResultObject> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            if(_context == default)
                return ResultObject.CreateSuccessResult();

            var productToUpdate = _context.Products.FirstOrDefault(p => p.Id == request.Id);

            productToUpdate.Name = request.Name;
            productToUpdate.Class = request.Class;
            productToUpdate.Color = request.Color;
            productToUpdate.DaysToManufacture = request.DaysToManufacture;
            productToUpdate.DiscontinuedDate = request.DiscontinuedDate;
            productToUpdate.FinishedGoodsFlag = request.FinishedGoodsFlag;
            productToUpdate.ListPrice = request.ListPrice;
            productToUpdate.MakeFlag = request.MakeFlag;
            productToUpdate.ProductLine = request.ProductLine;
            productToUpdate.ProductNumber = request.ProductNumber;
            productToUpdate.ReorderPoint = request.ReorderPoint;
            productToUpdate.SafetyStockLevel = request.SafetyStockLevel;
            productToUpdate.SellEndDate = request.SellEndDate;
            productToUpdate.SellStartDate = request.SellStartDate;
            productToUpdate.Size = request.Size;
            productToUpdate.SizeUnitMeasureCode = request.SizeUnitMeasureCode;
            productToUpdate.StandardCost = request.StandardCost;
            productToUpdate.Style = request.Style;
            productToUpdate.Weight = request.Weight;
            productToUpdate.WeightUnitMeasureCode = request.WeightUnitMeasureCode;

            await _context.SaveChangesAsync(CancellationToken.None);

            return ResultObject.CreateSuccessResult();
        }
    }

    public class UpdateProductCommand : IRequest<ResultObject>
    {
        public string Name { get; set; }
        public string ProductNumber { get; set; }
        public string ProductLine { get; set; }
        public string Class { get; set; }
        public string Color { get; set; }
        public int DaysToManufacture { get; set; }
        public bool FinishedGoodsFlag { get; set; }
        public decimal ListPrice { get; set; }
        public decimal StandardCost { get; set; }
        public bool MakeFlag { get; set; }
        public short ReorderPoint { get; set; }
        public short SafetyStockLevel { get; set; }
        public DateTime SellStartDate { get; set; }
        public string Size { get; set; }
        public string SizeUnitMeasureCode { get; set; }
        public string Style { get; set; }
        public string WeightUnitMeasureCode { get; set; }
        public int Id { get; set; }
        public DateTime? DiscontinuedDate { get; set; }
        public DateTime? SellEndDate { get; set; }
        public decimal? Weight { get; set; }
    }
}
