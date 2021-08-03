using Application.DTO.User;
using Application.Users.Commands;
using Application.Users.Queries;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = SD.Admin)]
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _mediator.Send(new ListUsers()));
        }

        [Authorize(Roles = SD.Admin)]
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            return Ok(await _mediator.Send(new GetUser() { Id = id }));
        }

        [HttpPost("register")]
        public async Task<ActionResult<object>> Register([FromForm] RegisterUser command)
        {
            return CreatedAtAction("Register", await _mediator.Send(command));
        }

        [HttpPost("login")]
        public async Task<ActionResult<object>> Login(Login query)
        {
            var getUser = await _mediator.Send(query);
            SetTokenCookie(getUser.RefreshToken);
            return Ok(new { Success = true, Token = getUser.RefreshToken, User = getUser });
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult> Profile()
        {
            return Ok(await _mediator.Send(new ProfileUser()));
        }

        [Authorize]
        [HttpPut("me/update")]
        public async Task<ActionResult> UpdateProfile([FromForm]UpdateProfile command)
        {
            return Ok(await _mediator.Send(command));
        }


        [Authorize(Roles = SD.Admin)]
        [HttpPut("admin/user/{id}")]
        public async Task<ActionResult> AdminUpdate(string id, AdminUpdate command)
        {
            command.Id = id;
            return Ok(await _mediator.Send(command));
        }

        [Authorize(Roles = SD.Admin)]
        [HttpDelete("admin/user/{id}")]
        public async Task<ActionResult> AdminDelete(string id, AdminDelete command)
        {
            command.Id = id;
            return Ok(await _mediator.Send(command));
        }

        [Authorize]
        [HttpGet("logout")]
        public async Task<ActionResult> Logout()
        {
            bool signOut = await _mediator.Send(new Logout());
            if (signOut)
            {
                DeleteCookie();
                return Ok(new { Success = true, Message = "Logged out" });
            }
            return NotFound(new { Success = false, Message = "Problem" });
        }

        [HttpPost("password/forgot")]
        public async Task<ActionResult> ForgotPassword([FromForm] ForgotPassword command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut("password/reset/{*token}")]
        public async Task<ActionResult> ResetPassword(string token,[FromForm] ResetPassword command)
        {
            command.Token = token;
            return Ok(await _mediator.Send(command));
        }

        [HttpPut("password/update")]
        public async Task<ActionResult> UpdatePassword([FromForm]UpdatePassword command)
        {
            var user = await _mediator.Send(command);
            SetTokenCookie(user.RefreshToken);
            return Ok(new { Success = true, Token = user.RefreshToken, User = user });
        }

        // set and remove cookie
        private void SetTokenCookie(string refreshToken)
        {
            Response.Cookies.Append("token", refreshToken);
        }



        private void DeleteCookie()
        {
            Response.Cookies.Delete("token");
        }

        //private void ExpireToken(string refreshToken)
        //{

        //    var cookieOptions = new CookieOptions
        //    {
        //        HttpOnly = true,
        //        Expires = DateTime.Now
        //    };
        //}

        //public async Task<ActionResult<User>> RefreshToken(Application.User.RefreshToken.Command command)
        //{
        //    command.RefreshToken = Request.Cookies["refreshToken"];
        //    var user = await Mediator.Send(command);
        //    SetTokenCookie(user.RefreshToken);
        //    return user;
        //}

    }
}
