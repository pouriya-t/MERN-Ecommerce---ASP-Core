using Domain.Interfaces.Repositories;
using Domain.Interfaces.UserAccessor;
using Domain.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Security.UserAccessor
{
    public class UserAccessor : IUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;

        public UserAccessor(IHttpContextAccessor httpContextAccessor,
                            IUserRepository userRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        public ApplicationUser GetUser()
        {
            var userName = _httpContextAccessor.HttpContext.User.Identity.Name;
            var user = _userRepository.GetUser(userName);
            return user;
        }
    }
}
