using Domain.Models.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries
{
    public class Logout : IRequest<bool>
    {


        public class Handler : IRequestHandler<Logout, bool>
        {

            private readonly SignInManager<ApplicationUser> _signInManager;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(SignInManager<ApplicationUser> signInManager,
                IHttpContextAccessor httpContextAccessor)
            {
                _signInManager = signInManager;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<bool> Handle(Logout request, CancellationToken cancellationToken)
            {
                bool isSignedIn = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
                if (isSignedIn)
                {
                    await _signInManager.SignOutAsync();
                    return true;
                }
                return false;
            }
        }

    }
}
