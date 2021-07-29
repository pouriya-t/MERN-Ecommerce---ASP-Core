using Application.DTO.User;
using Application.Errors;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.UserAccessor;
using Domain.Models.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands
{
    public class AdminUpdate : IRequest<object>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Role { get; set; }


        public class Handler : IRequestHandler<AdminUpdate, object>
        {
            private readonly IUserRepository _userRepository;
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly IUserAccessor _userAccessor;

            public Handler(IUserRepository userRepository, UserManager<ApplicationUser> userManager,
                IUserAccessor userAccessor)
            {
                _userRepository = userRepository;
                _userManager = userManager;
                _userAccessor = userAccessor;
            }

            public async Task<object> Handle(AdminUpdate command, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetUserByIdAsync(command.Id);

                if (user != null)
                {
                    user.Name = command.Name ?? user.Name;
                    user.Email = command.Email ?? user.Email;


                    if (command.Role != "" || command.Role != null)
                    {
                        var roles = await _userAccessor.GetRolesAsync(user);
                        string role = "";
                        foreach (var item in roles)
                        {
                            role = item;
                        }
                        if (role != "")
                        {
                            await _userManager.RemoveFromRoleAsync(user, role);
                        }

                        string newRole = command.Role.ToLower();
                        char[] a = newRole.ToCharArray();
                        a[0] = char.ToUpper(a[0]);
                        string capitlizedFirstLetter = new string(a);

                        await _userManager.AddToRoleAsync(user, capitlizedFirstLetter);
                    }
                    await _userRepository.UpdateUser(user);
                    return new { Success = true };
                    
                }
                return new { Success = false };
            }
        }
    }
}
