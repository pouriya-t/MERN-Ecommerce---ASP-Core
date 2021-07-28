using Domain.Interfaces.EmailService;
using Domain.Interfaces.Repositories;
using Domain.Models.EmailService;
using Domain.Models.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries
{
    public class ForgotPassword : IRequest<object>
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }


        // https://code-maze.com/password-reset-aspnet-core-identity/

        public class Handler : IRequestHandler<ForgotPassword, object>
        {

            private readonly UserManager<ApplicationUser> _userManager;
            private readonly IEmailSender _emailSender;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IUserRepository _userRepository;


            public Handler(UserManager<ApplicationUser> userManager, IEmailSender emailSender,
                IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
            {
                _userManager = userManager;
                _emailSender = emailSender;
                _httpContextAccessor = httpContextAccessor;
                _userRepository = userRepository;
            }

            public async Task<object> Handle(ForgotPassword request,CancellationToken cancellationToken)
            {

                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                    return new { Success = false, Message = "Email not found" };

                //var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                byte[] tokenGeneratedBytes = Encoding.UTF8.GetBytes(token);
                var codeEncoded = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);

                user.ResetPasswordToken = codeEncoded;
                user.ResetPasswordExpire = DateTime.Now.AddHours(2);

                await _userRepository.UpdateUser(user);

                string currentUrl = _httpContextAccessor.HttpContext.Request.Host.Value;

                var content = $"<h2>Reset your password</h2><br /><br /><h4>Your token:</h4><br />" +
                    $"<a href='https://{currentUrl}/api/v1/password/reset/{codeEncoded}'>Click Here for go to url</a>";


                var message = new Message(new string[] { user.Email }, "Reset password token", content, null);
                await _emailSender.SendEmailAsync(message);

                return new { Success = true, Message = $"Email sent to {request.Email}" };
            }
        }
    }
}
