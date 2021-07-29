using Domain.Models.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetUsers();
        Task<ApplicationUser> GetUserAsync(string userName);
        Task<ApplicationUser> GetUserByIdAsync(string id);
        Task<ApplicationUser> GetUserByTokenResetPasswordAsync(string tokenResetPassword);
        Task<bool> AnyUser(string userName);
        Task<bool> UpdateUser(ApplicationUser user);
        Task<bool> DeleteUser(string id);
    }
}