using Domain.Models.User;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Domain.Models.JwtModels
{
    public class RefreshToken
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public string Token { get; set; }

        public DateTime Expires { get; set; } = DateTime.UtcNow.AddDays(7);

        public bool IsExpired => DateTime.UtcNow >= Expires;

        public DateTime? Revoked { get; set; }

        public bool IsActive => Revoked == null & !IsExpired;
    }
}
