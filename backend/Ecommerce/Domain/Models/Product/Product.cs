using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using Domain.Models.ImageModel;

namespace Domain.Models.Product
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public double? Price { get; set; }

        public string Description { get; set; }

        public double Ratings { get; set; } = 0;

        public ICollection<Image> Images { get; set; }

        public string Category { get; set; }

        public string Seller { get; set; }

        public double Stock { get; set; }

        public int NumOfReviews { get; set; } = 0;

        public ICollection<Review> Reviews { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [BsonRepresentation(BsonType.ObjectId)]
        public string User { get; set; }

    }
}
