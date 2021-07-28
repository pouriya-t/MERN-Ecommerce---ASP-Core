using Domain.Interfaces;
using Domain.Models.Product;
using Domain.Models.User;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Persistence.Context
{
    public class MongoContext : IMongoContext
    {
        private readonly IConfiguration _configuration;

        public IMongoCollection<Product> Products { get; }
        public IMongoCollection<ApplicationUser> Users { get; }

        public MongoContext(IConfiguration configuration)
        {
            _configuration = configuration;

            var client = new MongoClient(_configuration["DatabaseSettings:ConnectionString"]);
            var database = client.GetDatabase(_configuration["DatabaseSettings:DatabaseName"]);

            Products = database.GetCollection<Product>(_configuration["DatabaseSettings:ProductCollection"]);
            Users = database.GetCollection<ApplicationUser>(_configuration["DatabaseSettings:UserCollection"]);

            SeedData.SeedProducts(Products);
        }

        // another way to create database table and for how to see it's working , please go to UserRepository.cs

        //public IMongoCollection<T> GetCollection<T>(string tableName)
        //{
        //    var client = new MongoClient(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        //    var database = client.GetDatabase(_configuration.GetValue<string>("DatabaseSettings:DatabaseName"));
        //    return database.GetCollection<T>(tableName);
        //}


        // if we are implement this class in startup , write this codes
        //public IMongoCollection<Product> Products { get; }

        //public MongoContext(IConfiguration configuration)
        //{
        //    var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        //    var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));

        //    Products = database.GetCollection<Product>(configuration.GetValue<string>("DatabaseSettings:ProductCollection"));
        //    SeedData.SeedProducts(Products);
        //}
    }
}