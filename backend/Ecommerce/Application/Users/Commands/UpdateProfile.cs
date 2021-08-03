using Application.DTO.User;
using Application.Errors;
using Domain.Interfaces.Jwt;
using Domain.Interfaces.PhotoAccessor;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.UserAccessor;
using Domain.Models.ImageModel;
using Domain.Models.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands
{
    public class UpdateProfile : IRequest<object>
    {
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public IFormFile Avatar { get; set; }

        public class Handler : IRequestHandler<UpdateProfile, object>
        {
            private readonly IUserAccessor _userAccessor;
            private readonly IUserRepository _userRepository;
            private readonly IPhotoAccessor _photoAccessor;

            public Handler(IUserRepository userRepository, IUserAccessor userAccessor, IPhotoAccessor photoAccessor)
            {
                _userRepository = userRepository;
                _userAccessor = userAccessor;
                _photoAccessor = photoAccessor;
            }

            public async Task<object> Handle(UpdateProfile command, CancellationToken cancellationToken)
            {
                var user = await _userAccessor.GetUserAsync();
                var role = await _userAccessor.GetRolesAsync(user);

                var checkUserByEmail = await _userRepository.AnyUser(command.Email);

                if (checkUserByEmail)
                {
                    user.Name = command.Name ?? user.Name;
                    user.Email = command.Email ?? user.Email;
                    user.NormalizedEmail = command.Email.ToUpper() ?? user.NormalizedEmail;
                    if (command.Avatar != null)
                    {
                        if (user.Avatar != null)
                        {
                            var imageId = user.Avatar.PublicId;
                            _photoAccessor.DeletePhoto(imageId);
                        }

                        var photoUploadResult = _photoAccessor.AddPhoto(command.Avatar);

                        var photo = new Image
                        {
                            PublicId = photoUploadResult.PublicId,
                            Url = photoUploadResult.Url
                        };

                        user.Avatar = photo;
                    }
                    var success = await _userRepository.UpdateUser(user);
                    if (success)
                    {
                        return new { Success = true, User = new UserDto(user, role) };
                    }
                }
                throw new RestException(HttpStatusCode.BadRequest, "Sometimes went wrong");
            }
        }
    }
}
