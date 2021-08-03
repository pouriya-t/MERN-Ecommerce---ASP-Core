using Application.Errors;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Jwt;
using Domain.Models.User;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.DTO.User;
using Microsoft.AspNetCore.Http;
using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using Infrastructure.Helpers;
using Domain.Interfaces.PhotoAccessor;
using Domain.Models.ImageModel;

namespace Application.Users.Commands
{
    public class RegisterUser : IRequest<object>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public IFormFile Avatar { get; set; }

        public class Handler : IRequestHandler<RegisterUser, object>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly IUserRepository _userRepository;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly IPhotoAccessor _photoAccessor;

            

            public Handler(UserManager<ApplicationUser> userManager, IUserRepository userRepository
                            , IJwtGenerator jwtGenerator, IPhotoAccessor photoAccessor)
            {
                _userManager = userManager;
                _userRepository = userRepository;
                _jwtGenerator = jwtGenerator;
                _photoAccessor = photoAccessor;
            }

            public async Task<object> Handle(RegisterUser request, CancellationToken cancellationToken)
            {
                if (await _userRepository.AnyUser(request.Email))
                    throw new RestException(HttpStatusCode.BadRequest, "Email already exists");

                var user = new ApplicationUser
                {
                    Email = request.Email,
                    UserName = request.Email,
                    Name = request.Name
                };

                var photoUploadResult = _photoAccessor.AddPhoto(request.Avatar);

                
                var photo = new Image
                {
                    PublicId = photoUploadResult.PublicId,
                    Url = photoUploadResult.Url
                };

                user.Avatar = photo;

                var createUser = await _userManager.CreateAsync(user, request.Password);
                var createRole = await _userManager.AddToRoleAsync(user, SD.User);
                var role = await _userManager.GetRolesAsync(user);

                if (createUser.Succeeded && createRole.Succeeded)
                {
                    var token = _jwtGenerator.CreateToken(user);
                    return new
                    {
                        Success = true,
                        Token = token.Result.Token,
                        ValidTo = token.Result.ValidTo,
                        User = new UserDto(user, role)
                    };
                }
                throw new Exception("Your request has a problem");
            }
        }
    }
}
