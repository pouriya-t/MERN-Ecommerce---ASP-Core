using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Domain.Models.Order
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public ShippingInfo ShippingInfo { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string User { get; set; }

        public ICollection<OrderItems> OrderItems { get; set; }

        public PaymentInfo PaymentInfo { get; set; }

        public DateTime PaidAt { get; set; }

        public double ItemsPrice { get; set; } = 0.0;

        public double TaxPrice { get; set; } = 0.0;

        public double ShippingPrice { get; set; } = 0.0;

        public double TotalPrice { get; set; } = 0.0;

        public string OrderStatus { get; set; } = "Processing";

        public DateTime DeliveredAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
