using Domain.Interfaces.Repositories;
using Domain.Interfaces.UserAccessor;
using Domain.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Security.UserAccessor
{
    public class UserAccessor : IUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserAccessor(IHttpContextAccessor httpContextAccessor,
                            IUserRepository userRepository,UserManager<ApplicationUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public async Task<ApplicationUser> GetUserAsync()
        {
            var userName = _httpContextAccessor.HttpContext.User.Identity.Name;
            var user = await _userRepository.GetUserAsync(userName);
            return user;
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }
    }
}
