using Domain.Models.Amount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    [Route("api/v1")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public PaymentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("stripeapi")]
        public IActionResult GetStripeApiKey()
        {
            string StripeApiKey = _configuration["Stripe:PublishableKey"];
            return Ok(new { StripeApiKey = StripeApiKey });
        }

        [HttpPost("payment/process")]
        public IActionResult PaymentProcess([FromBody] AmountModel amountModel)
        {

            var options = new PaymentIntentCreateOptions
            {
                Amount = (int)amountModel.Amount,
                Currency = "usd",
                PaymentMethodTypes = new List<string>
                {
                  "card",
                },
            };
            var service = new PaymentIntentService();
            var createdService = service.Create(options);

            if (createdService != null)
            {
                return Ok(new { Success = true, client_secret = createdService.ClientSecret });
            }


            return BadRequest(new { Message = "some times occuring a problem" });
        }
    }
}
