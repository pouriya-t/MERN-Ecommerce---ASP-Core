using Application.DTO.User;
using Application.Errors;
using Domain.Interfaces.Jwt;
using Domain.Interfaces.Repositories;
using Domain.Models.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries
{
    public class Login : IRequest<UserDto>
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public class Handler : IRequestHandler<Login, UserDto>
        {

            private readonly UserManager<ApplicationUser> _userManager;
            private readonly SignInManager<ApplicationUser> _signInManager;
            private readonly IJwtGenerator _jwtGenerator;

            public Handler(UserManager<ApplicationUser> userManager,
                            SignInManager<ApplicationUser> signInManager, 
                            IJwtGenerator jwtGenerator)
            {
                _jwtGenerator = jwtGenerator;
                _signInManager = signInManager;
                _userManager = userManager;
            }

            public async Task<UserDto> Handle(Login request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                if (user == null)
                    throw new RestException(HttpStatusCode.NotFound);

                var role = await _userManager.GetRolesAsync(user);

                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

                if (result.Succeeded)
                {
                    var token = _jwtGenerator.CreateToken(user);
                    user.RefreshToken = token.Result.Token;
                    return new UserDto(user,role);
                }
                throw new Exception("Your request has a problem");
            }
        }
    }
}
