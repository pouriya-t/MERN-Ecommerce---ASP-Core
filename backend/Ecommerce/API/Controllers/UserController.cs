using Application.Users.Commands;
using Application.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class UserController : ControllerBase
    {

        private IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _mediator.Send(new List()));
        }

        [HttpPost("register")]
        public async Task<ActionResult<object>> Register(Register command)
        {
            return CreatedAtAction("Register", await _mediator.Send(command));
        }

        [HttpPost("login")]
        public async Task<ActionResult<object>> Login(Login query)
        {
            var getUser = await _mediator.Send(query);
            SetTokenCookie(getUser.RefreshToken);
            return Ok(new { Success = true, Token = getUser.RefreshToken });
        }

        [HttpPost("logout")]
        public async Task<ActionResult> Logout(Logout query)
        {
            bool signOut = await _mediator.Send(query);
            if (signOut)
            {
                DeleteCookie();
                return Ok(new { Success = true, Message = "Logged out" });
            }
            return NotFound(new { Success = false, Message = "Problem" });
        }

        [HttpPost("password/forgot")]
        public async Task<ActionResult> Forgot(ForgotPassword command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut("password/reset/{*token}")]
        public async Task<ActionResult> Reset(string token,ResetPassword command)
        {
            command.Token = token;
            return Ok(await _mediator.Send(command));
        }




        // set and remove cookie
        private void SetTokenCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("token", refreshToken, cookieOptions);
        }

        private void DeleteCookie()
        {
            Response.Cookies.Delete("token");
        }
    }
}
