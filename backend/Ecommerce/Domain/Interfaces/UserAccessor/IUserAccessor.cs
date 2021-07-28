using Domain.Models.User;
using System.Threading.Tasks;

namespace Domain.Interfaces.UserAccessor
{
    public interface IUserAccessor
    {
        ApplicationUser GetUser();
    }
}
