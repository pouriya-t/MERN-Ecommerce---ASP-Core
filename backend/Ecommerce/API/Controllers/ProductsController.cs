using Application.Products.Commands;
using Application.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class ProductsController : ControllerBase
    {
        // https://www.ezzylearning.net/tutorial/implement-cqrs-pattern-in-asp-net-core-5
        // https://github.com/ghulamabbas2/shopit

        private IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // /api/v1/products?keyword=charmount&price[gt]=20&price[lt]=27
        // /api/v1/products?page=1
        // /api/v1/products
        [HttpGet("products")]
        public async Task<IActionResult> GetProducts(int? page = 1,string keyword = null, 
            [FromQuery(Name = "price")] IDictionary<string, int> price = null)
        {
            return Ok(await _mediator.Send(new List() { Keyword = keyword, Price = price , Page = page }));
        }

        [HttpGet("product/{id}")]
        public async Task<IActionResult> GetProduct(string id)
        {
            return Ok(await _mediator.Send(new Detail() { Id = id }));
        }

        [HttpPost("admin/product/new")]
        public async Task<IActionResult> Create(Create command)
        {
            return CreatedAtAction("Create", await _mediator.Send(command));
        }

        [HttpPut("admin/product/{id}")]
        public async Task<IActionResult> Edit(string id, Edit command)
        {
            command.Id = id;
            return Ok(await _mediator.Send(command));
        }

        [HttpDelete("admin/product/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            return Ok(await _mediator.Send(new Delete() { Id = id }));
        }
    }
}
