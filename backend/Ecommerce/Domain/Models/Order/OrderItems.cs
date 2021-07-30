using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Models.Order
{
    public class OrderItems
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        public string Name { get; set; }

        public double Quantity { get; set; }

        public string Image { get; set; }

        public double Price { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string User { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Product { get; set; }
    }
}