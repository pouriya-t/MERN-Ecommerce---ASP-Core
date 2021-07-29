using Domain.Models.User;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTO.User
{
    public class UserDto
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public Avatar Avatar { get; set; }

        public string Role { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public DateTime CreatedAt { get; set; }

        [JsonIgnore]
        public string RefreshToken { get; set; }

        public UserDto(ApplicationUser user, IEnumerable<string> roles)
        {
            foreach (var role in roles)
            {
                Role = role;
            }
            _id = user.Id.ToString();
            Avatar = user.Avatar;
            Name = user.Name;
            Email = user.Email;
            CreatedAt = user.CreatedAt;
            RefreshToken = user.RefreshToken;
        }
    }

}
