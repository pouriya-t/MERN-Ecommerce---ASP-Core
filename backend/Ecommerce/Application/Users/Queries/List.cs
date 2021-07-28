using Domain.Interfaces.Repositories;
using Domain.Models.User;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries
{
    public class List : IRequest<IEnumerable<ApplicationUser>>
    {

        public class Handler : IRequestHandler<List, IEnumerable<ApplicationUser>>
        {
            private readonly IUserRepository _userRepository;

            public Handler(IUserRepository userRepository)
            {
                _userRepository = userRepository;
            }

            public async Task<IEnumerable<ApplicationUser>> Handle(List query, CancellationToken cancellationToken)
            {
                return await _userRepository.GetUsers();
            }
        }
    }
}
