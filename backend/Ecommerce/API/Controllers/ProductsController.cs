using Application.Products;
using Application.Products.Commands;
using Application.Products.Queries;
using Domain.Interfaces;
using Domain.Product;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpGet("products")]
        public async Task<IActionResult> GetProducts()
        {
            return Ok(await _mediator.Send(new List()));
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
