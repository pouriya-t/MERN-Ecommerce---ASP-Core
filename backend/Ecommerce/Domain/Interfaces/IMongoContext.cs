using Domain.Models.Product;
using Domain.Models.User;
using MongoDB.Driver;

namespace Domain.Interfaces
{
    public interface IMongoContext
    {
        IMongoCollection<Product> Products { get; }
        IMongoCollection<ApplicationUser> Users { get; }
    }
}