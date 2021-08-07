using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Models.Order
{
    public class PaymentInfo
    {
        public string Id { get; set; }

        public string Status { get; set; }
    }
}