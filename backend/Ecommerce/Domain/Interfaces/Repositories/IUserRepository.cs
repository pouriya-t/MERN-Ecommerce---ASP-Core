using Domain.Models.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetUsers();
        ApplicationUser GetUser(string userName);
        ApplicationUser GetUserByTokenResetPassword(string tokenResetPassword);
        Task<bool> AnyUser(string userName);
        Task<bool> UpdateUser(ApplicationUser user);
    }
}