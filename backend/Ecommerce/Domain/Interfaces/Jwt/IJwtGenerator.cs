using Domain.Models.User;
using System.Threading.Tasks;
using Domain.Models.JwtModels;

namespace Domain.Interfaces.Jwt
{
    public interface IJwtGenerator
    {
        Task<TokenModel> CreateToken(ApplicationUser user);
        string GenerateRefreshToken();
    }
}
