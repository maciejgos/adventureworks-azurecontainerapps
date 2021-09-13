using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdventureWorksCore.Api.Application.Models.Common;
using AdventureWorksCore.Infrastructure.Persistence;
using MediatR;

namespace AdventureWorksCore.Api.Application.Commands
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, ResultObject>
    {
        private readonly ApplicationDbContext _context;

        public DeleteProductCommandHandler(ApplicationDbContext context = default)
        {
            _context = context;
        }

        public async Task<ResultObject> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            if(_context == default || request.Id == 0)
                return ResultObject.CreateSuccessResult();

            var productToDelete = _context.Products.FirstOrDefault(p => p.Id == request.Id);
            _context.Products.Remove(productToDelete);
            await _context.SaveChangesAsync(CancellationToken.None);

            return ResultObject.CreateSuccessResult();
        }
    }

    public class DeleteProductCommand : IRequest<ResultObject>
    {
        public DeleteProductCommand(int id)
        {
            Id = id;
        }

        public int Id { get;}
    }
}
