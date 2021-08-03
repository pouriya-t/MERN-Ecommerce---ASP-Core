using Domain.Models.ImageModel;
using Domain.Models.User;
using Microsoft.AspNetCore.Http;

namespace Domain.Interfaces.PhotoAccessor
{
     public interface IPhotoAccessor
    {
         Image AddPhoto(IFormFile file);
         string DeletePhoto(string publicId);
    }
}