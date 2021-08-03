using AspNetCore.Identity.Mongo.Model;
using Domain.Models.ImageModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Domain.Models.User
{
    public class ApplicationUser : MongoUser<ObjectId>
    {
        public string Name { get; set; }

        public Image Avatar { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string ResetPasswordToken { get; set; }
        
        public DateTime ResetPasswordExpire{ get; set; }

        [BsonIgnore]
        [JsonIgnore]
        public string RefreshToken { get; set; }
    }
}
