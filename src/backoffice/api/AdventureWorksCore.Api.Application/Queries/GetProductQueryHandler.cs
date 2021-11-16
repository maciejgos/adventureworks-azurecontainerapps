using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdventureWorksCore.Api.Application.Models;
using AdventureWorksCore.Infrastructure.Persistence;
using MediatR;

namespace AdventureWorksCore.Api.Application.Queries
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductModel>
    {
        private readonly ApplicationDbContext _context;

        public GetProductQueryHandler(ApplicationDbContext context = default) => _context = context;

        public Task<ProductModel> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            if (_context == default)
                return Task.FromResult(new ProductModel());

            var query = _context.Products.Where(e => e.Id == request.ProductId);
            var result = query.FirstOrDefault();
            var response = new ProductModel
            {
                Id = result.Id,
                Name = result.Name,
                StandardCost = result.StandardCost,
                ListPrice = result.ListPrice
            };

            return Task.FromResult(response);
        }
    }

    public class GetProductQuery : IRequest<ProductModel>
    {
        public int ProductId { get; }

        public GetProductQuery(int id)
        {
            ProductId = id;
        }
    }
}
