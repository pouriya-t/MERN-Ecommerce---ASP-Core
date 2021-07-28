using Domain.Interfaces.EmailService;
using Domain.Interfaces.Jwt;
using Domain.Interfaces.Repositories;
using Domain.Models.EmailService;
using Domain.Models.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Org.BouncyCastle.Asn1.Ocsp;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries
{
    public class ResetPassword : IRequest<object>
    {

        public string Token { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }


        // https://code-maze.com/password-reset-aspnet-core-identity/

        public class Handler : IRequestHandler<ResetPassword, object>
        {

            private readonly UserManager<ApplicationUser> _userManager;
            private readonly IUserRepository _userRepository;
            private readonly IJwtGenerator _jwtGenerator;


            public Handler(UserManager<ApplicationUser> userManager, IUserRepository userRepository,
                IJwtGenerator jwtGenerator)
            {
                _userManager = userManager;
                _userRepository = userRepository;
                _jwtGenerator = jwtGenerator;
            }

            public async Task<object> Handle(ResetPassword request,CancellationToken cancellationToken)
            {
                var user = _userRepository.GetUserByTokenResetPassword(request.Token);

                if(user != null)
                {
                    var codeDecodedBytes = WebEncoders.Base64UrlDecode(request.Token);
                    var codeDecoded = Encoding.UTF8.GetString(codeDecodedBytes);
                    var resetPassResult = await _userManager.ResetPasswordAsync(user, codeDecoded, request.Password);
                    if (resetPassResult.Succeeded)
                    {
                        var token = _jwtGenerator.CreateToken(user);
                        return new { Success = true, Token = token.Result.Token , User = user };
                    }
                }

                return new { Success = false, Message = "Your request has a problem" };
                
            }
        }
    }
}
