﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Models.Order
{
    public class PaymentInfo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        public string Status { get; set; }
    }
}