using Application.Orders.Commands;
using Application.Orders.Queries;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1")]
    public class OrderController : ControllerBase
    {

        private IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("order/new")]
        public async Task<IActionResult> Create(CreateOrder command)
        {
            return CreatedAtAction("Create", await _mediator.Send(command));
        }

        [Authorize(Roles = SD.Admin)]
        [HttpGet("admin/orders")]
        public async Task<IActionResult> GetOrders()
        {
            return Ok(await _mediator.Send(new ListOrders()));
        }

        [HttpGet("order/{id}")]
        public async Task<IActionResult> GetOrder(string id)
        {
            return Ok(await _mediator.Send(new DetailOrder { Id = id }));
        }

        [HttpGet("orders/me")]
        public async Task<IActionResult> GetMyOrders()
        {
            return Ok(await _mediator.Send(new ListMyOrders()));
        }

        [Authorize(Roles = SD.Admin)]
        [HttpPut("admin/order/{id}")]
        public async Task<IActionResult> EditOrder(string id,[FromForm] EditOrder command)
        {
            command.Id = id;
            return Ok(await _mediator.Send(command));
        }

        [Authorize(Roles = SD.Admin)]
        [HttpDelete("admin/order/{id}")]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            return Ok(await _mediator.Send(new DeleteOrder { Id = id }));
        }
    }
}
