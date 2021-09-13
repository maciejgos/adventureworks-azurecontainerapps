using System.Collections.Generic;
using System.Threading.Tasks;
using AdventureWorksCore.Api.Application.Commands;
using AdventureWorksCore.Api.Application.Models;
using AdventureWorksCore.Api.Application.Models.Common;
using AdventureWorksCore.Api.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AdventureWorksCore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductListModel>>> Get()
        {
            var request = new GetProductsListQuery();
            var response = await _mediator.Send(request);

            return new OkObjectResult(response);
        }

        // GET: api/Products/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<ActionResult<ProductModel>> Get(int id)
        {
            var request = new GetProductQuery(id);
            var response = await _mediator.Send(request);

            return new OkObjectResult(response);
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<ResultObject>> Post([FromBody] CreateProductCommand command)
        {
            var response = await _mediator.Send(command);
            return new OkObjectResult(response);
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ResultObject>> Put(int id, [FromBody] UpdateProductCommand command)
        {
            var response = await _mediator.Send(command);
            return new OkObjectResult(response);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResultObject>> Delete(int id)
        {
            var command = new DeleteProductCommand(id);
            var response = await _mediator.Send(command);
            return new OkObjectResult(response);
        }
    }
}
