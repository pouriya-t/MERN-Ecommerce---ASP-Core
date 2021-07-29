using Application.DTO.User;
using Application.Errors;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.UserAccessor;
using Domain.Models.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands
{
    public class AdminDelete : IRequest<object>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Role { get; set; }


        public class Handler : IRequestHandler<AdminDelete, object>
        {
            private readonly IUserRepository _userRepository;

            public Handler(IUserRepository userRepository)
            {
                _userRepository = userRepository;
            }

            public async Task<object> Handle(AdminDelete command, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetUserByIdAsync(command.Id);

                if (user != null)
                {
                    await _userRepository.DeleteUser(command.Id);
                    return new { Success = true };
                }
                return new { Success = false };
            }
        }
    }
}
