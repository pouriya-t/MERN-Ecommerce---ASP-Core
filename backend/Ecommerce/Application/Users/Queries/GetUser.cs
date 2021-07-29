using Application.DTO.User;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.UserAccessor;
using Domain.Models.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries
{
    public class GetUser : IRequest<object>
    {

        public string Id { get; set; }

        public class Handler : IRequestHandler<GetUser, object>
        {
            private readonly IUserRepository _userRepository;
            private readonly IUserAccessor _userAccessor;

            public Handler(IUserRepository userRepository, IUserAccessor userAccessor)
            {
                _userRepository = userRepository;
                _userAccessor = userAccessor;
            }

            public async Task<object> Handle(GetUser query, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetUserByIdAsync(query.Id);
                if(user != null)
                {
                    var role = await _userAccessor.GetRolesAsync(user);
                    return new { Success = true, User = new UserDto(user, role) };
                }
                return new { Success = false, Message = "User not found" };
            }
        }
    }
}
