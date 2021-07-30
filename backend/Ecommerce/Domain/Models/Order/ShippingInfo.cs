using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Models.Order
{
    public class ShippingInfo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        public string Address { get; set; }
        
        public string City { get; set; }
        
        public string PhoneNo { get; set; }

        public string PostalCode{ get; set; }

        public string Country { get; set; }

    }
}