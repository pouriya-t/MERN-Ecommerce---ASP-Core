using Domain.Interfaces;
using Domain.Product;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;

namespace API.Context
{
    public class MongoContext : IMongoContext
    {

        public MongoContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));

            Products = database.GetCollection<Product>(configuration.GetValue<string>("DatabaseSettings:ProductCollection"));
            ProductSeedData.SeedData(Products);
        }

        public IMongoCollection<Product> Products { get; }

    }
}