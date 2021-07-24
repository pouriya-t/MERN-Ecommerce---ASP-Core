using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IMongoContext
    {
        IMongoCollection<Product.Product> Products { get; }
    }
}