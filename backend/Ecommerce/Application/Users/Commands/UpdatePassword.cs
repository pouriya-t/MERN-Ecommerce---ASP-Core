using Application.DTO.User;
using Application.Errors;
using Domain.Interfaces.Jwt;
using Domain.Interfaces.UserAccessor;
using Domain.Models.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands
{
    public class UpdatePassword : IRequest<UserDto>
    {
        public string OldPassword { get; set; }
        public string Password { get; set; }


        public class Handler : IRequestHandler<UpdatePassword, UserDto>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly IUserAccessor _userAccessor;
            private readonly IJwtGenerator _jwtGenerator;

            public Handler(UserManager<ApplicationUser> userManager, IUserAccessor userAccessor,
                IJwtGenerator jwtGenerator)
            {
                _userManager = userManager;
                _userAccessor = userAccessor;
                _jwtGenerator = jwtGenerator;
            }

            public async Task<UserDto> Handle(UpdatePassword command, CancellationToken cancellationToken)
            {
                var user = await _userAccessor.GetUserAsync();
                var result = await _userManager.ChangePasswordAsync(user, command.OldPassword, command.Password);
                var role = await _userAccessor.GetRolesAsync(user);

                if (result.Succeeded)
                {
                    var token = _jwtGenerator.CreateToken(user);
                    user.RefreshToken = token.Result.Token;
                    return new UserDto(user, role);
                }
                else
                {
                    throw new RestException(HttpStatusCode.NoContent, "Old password and new password not matched");
                }
                throw new RestException(HttpStatusCode.BadRequest, "Sometimes went wrong");
            }
        }
    }
}
