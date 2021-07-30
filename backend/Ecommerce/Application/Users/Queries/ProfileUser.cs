using Application.DTO.User;
using Domain.Interfaces.UserAccessor;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries
{
    public class ProfileUser : IRequest<object>
    {

        public class Handler : IRequestHandler<ProfileUser, object>
        {
            private readonly IUserAccessor _userAccessor;

            public Handler(IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
            }

            public async Task<object> Handle(ProfileUser query, CancellationToken cancellationToken)
            {
                var user = await _userAccessor.GetUserAsync();
                var roles = await _userAccessor.GetRolesAsync(user);

                return new { Success = true, User = new UserDto(user, roles) };
            }
        }
    }
}
