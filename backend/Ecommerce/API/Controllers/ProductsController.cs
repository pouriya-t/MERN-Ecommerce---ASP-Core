using Application.Products.Commands;
using Application.Products.Queries;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        public async Task<IActionResult> GetProducts(int? page = 1, string keyword = null,
            [FromQuery(Name = "price")] IDictionary<string, int> price = null, string category = null)
        {
            return Ok(await _mediator.Send(new ListProduct() { Keyword = keyword, Price = price, Page = page, Category = category }));
        }

        [HttpGet("product/{id}")]
        public async Task<IActionResult> GetProduct(string id)
        {
            return Ok(await _mediator.Send(new DetailProduct() { Id = id }));
        }

        [Authorize(Roles = SD.Admin)]
        [HttpPost("admin/product/new")]
        public async Task<IActionResult> Create(CreateProduct command)
        {
            return CreatedAtAction("Create", await _mediator.Send(command));
        }

        [Authorize]
        [HttpPut("review")]
        public async Task<ActionResult> UserReview([FromForm] UserReview command)
        {
            return Ok(await _mediator.Send(command));
        }

        [Authorize(Roles = SD.Admin)]
        [HttpGet("reviews")]
        public async Task<ActionResult> GetReview(string id = null)
        {
            return Ok(await _mediator.Send(new ListReview { Id = id }));
        }

        [Authorize(Roles = SD.Admin)]
        [HttpDelete("reviews")]
        public async Task<ActionResult> DeleteReview(string productId, string id)
        {
            return Ok(await _mediator.Send(new DeleteReview { ProductId = productId, Id = id }));
        }

        [Authorize(Roles = SD.Admin)]
        [HttpPut("admin/product/{id}")]
        public async Task<IActionResult> Edit(string id, EditProduct command)
        {
            command.Id = id;
            return Ok(await _mediator.Send(command));
        }

        [Authorize(Roles = SD.Admin)]
        [HttpDelete("admin/product/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            return Ok(await _mediator.Send(new DeleteProduct() { Id = id }));
        }
    }
}
