using Application.DTO.User;
using Domain.Interfaces.UserAccessor;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries
{
    public class Profile : IRequest<object>
    {

        public class Handler : IRequestHandler<Profile, object>
        {
            private readonly IUserAccessor _userAccessor;

            public Handler(IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
            }

            public async Task<object> Handle(Profile query, CancellationToken cancellationToken)
            {
                var user = await _userAccessor.GetUserAsync();
                var roles = await _userAccessor.GetRolesAsync(user);

                return new { Success = true, User = new UserDto(user, roles) };
            }
        }
    }
}
