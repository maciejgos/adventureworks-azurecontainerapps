using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdventureWorksCore.Api.Application.Models;
using AdventureWorksCore.Infrastructure.Persistence;
using MediatR;

namespace AdventureWorksCore.Api.Application.Queries
{
    public class GetProductsListQueryHandler : IRequestHandler<GetProductsListQuery, IEnumerable<ProductListModel>>
    {
        private readonly ApplicationDbContext _context;

        public GetProductsListQueryHandler(ApplicationDbContext context = default)
        {
            _context = context;
        }

        public Task<IEnumerable<ProductListModel>> Handle(GetProductsListQuery request, CancellationToken cancellationToken)
        {
            if (_context == default)
                return Task.FromResult(new List<ProductListModel>().AsEnumerable());

            var results = _context.Products?.Take(10);
            var response = results?.Select(r => new ProductListModel
            {
                Id = r.Id,
                Name = r.Name,
                ProductNumber = r.ProductNumber,
                ProductLine = r.ProductLine
            }).AsEnumerable();

            return Task.FromResult(response);
        }
    }

    public class GetProductsListQuery : IRequest<IEnumerable<ProductListModel>>
    {
        public GetProductsListQuery()
        {

        }
    }
}
