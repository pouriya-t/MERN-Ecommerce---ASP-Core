using Application.DTO.User;
using Application.Errors;
using Domain.Interfaces.Jwt;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.UserAccessor;
using Domain.Models.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands
{
    public class UpdateProfile : IRequest<object>
    {
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }


        public class Handler : IRequestHandler<UpdateProfile, object>
        {
            private readonly IUserAccessor _userAccessor;
            private readonly IUserRepository _userRepository;

            public Handler(IUserRepository userRepository, IUserAccessor userAccessor)
            {
                _userRepository = userRepository;
                _userAccessor = userAccessor;
            }

            public async Task<object> Handle(UpdateProfile command, CancellationToken cancellationToken)
            {
                var user = await _userAccessor.GetUserAsync();
                var role = await _userAccessor.GetRolesAsync(user);

                var checkUserByEmail = await _userRepository.AnyUser(command.Email);

                if (!checkUserByEmail)
                {
                    user.Name = command.Name ?? user.Name;
                    user.Email = command.Email ?? user.Email;
                    var success = await _userRepository.UpdateUser(user);
                    if (success)
                    {
                        return new { Success = true, User = new UserDto(user,role) };
                    }
                }
                throw new RestException(HttpStatusCode.BadRequest, "Sometimes went wrong");
            }
        }
    }
}
