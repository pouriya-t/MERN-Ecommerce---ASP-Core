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
    public class ListUsers : IRequest<object>
    {

        public class Handler : IRequestHandler<ListUsers, object>
        {
            private readonly IUserRepository _userRepository;
            private readonly IUserAccessor _userAccessor;

            public Handler(IUserRepository userRepository, IUserAccessor userAccessor)
            {
                _userRepository = userRepository;
                _userAccessor = userAccessor;
            }

            public async Task<object> Handle(ListUsers query, CancellationToken cancellationToken)
            {
                var users = await _userRepository.GetUsers();
                List<UserDto> userDtos = new List<UserDto>();

                foreach (var user in users)
                {
                    var role = await _userAccessor.GetRolesAsync(user);
                    var userFromDto = new UserDto(user, role);
                    userDtos.Add(userFromDto);
                }
                return new { Success = true, Users = userDtos };
            }
        }
    }
}
