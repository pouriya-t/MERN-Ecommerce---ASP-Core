using Domain.Models.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.UserAccessor
{
    public interface IUserAccessor
    {
        Task<ApplicationUser> GetUserAsync();
        Task<IList<string>> GetRolesAsync(ApplicationUser user);
    }
}
