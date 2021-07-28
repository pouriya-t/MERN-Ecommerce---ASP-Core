using Application.Errors;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Jwt;
using Domain.Models.User;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands
{
    public class Register : IRequest<object>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public class Handler : IRequestHandler<Register, object>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly IUserRepository _userRepository;
            private readonly IJwtGenerator _jwtGenerator;

            public Handler(UserManager<ApplicationUser> userManager, IUserRepository userRepository
                            , IJwtGenerator jwtGenerator)
            {
                _userManager = userManager;
                _userRepository = userRepository;
                _jwtGenerator = jwtGenerator;
            }

            public async Task<object> Handle(Register request, CancellationToken cancellationToken)
            {
                if (await _userRepository.AnyUser(request.Email))
                    throw new RestException(HttpStatusCode.BadRequest, "Email already exists");

                var user = new ApplicationUser
                {
                    Email = request.Email,
                    UserName = request.Email,
                    Name = request.Name
                };

                var createUser = await _userManager.CreateAsync(user, request.Password);
                var createRole = await _userManager.AddToRoleAsync(user, SD.User);

                if (createUser.Succeeded && createRole.Succeeded)
                {
                    var token = _jwtGenerator.CreateToken(user);
                    return new 
                    { 
                        Success = true, 
                        Token = token.Result.Token , 
                        ValidTo = token.Result.ValidTo , 
                        User = user 
                    };
                }
                throw new Exception("Your request has a problem");
            }
        }
    }
}
