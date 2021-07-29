using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Models.User;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoContext _context;

        public UserRepository(IMongoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> AnyUser(string userName)
        {
            return await _context.Users.Find(p => p.Email == userName).AnyAsync();
        }


        public async Task<IEnumerable<ApplicationUser>> GetUsers()
        {
            return await _context.Users.Find(p => true).ToListAsync();
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            return await _context.Users.Find(p => p.Id == ObjectId.Parse(id)).FirstOrDefaultAsync();
        }

        public async Task<ApplicationUser> GetUserAsync(string userName)
        {
            return await _context.Users.Find(p => p.UserName == userName).FirstOrDefaultAsync();
        }

        public async Task<ApplicationUser> GetUserByTokenResetPasswordAsync(string tokenResetPassword)
        {
            return await _context.Users.Find(p => p.ResetPasswordToken == tokenResetPassword).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateUser(ApplicationUser user)
        {
            var updateResult = await _context.Users
                .ReplaceOneAsync(filter: g => g.Id == user.Id, replacement: user);

            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> DeleteUser(string id)
        {
            FilterDefinition<ApplicationUser> filter = Builders<ApplicationUser>.Filter.Eq(p => p.Id , ObjectId.Parse(id));

            DeleteResult deleteResult = await _context.Users.DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }


        //public async Task<Product> GetProduct(string id)
        //{
        //    return await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
        //}

        //public async Task<IEnumerable<Product>> GetProductByName(string name)
        //{
        //    FilterDefinition<Product> filter = Builders<Product>.Filter.ElemMatch(p => p.Name, name);

        //    return await _context.Products.Find(filter).ToListAsync();
        //}

        //public async Task CreateProduct(Product product)
        //{
        //    await _context.Products.InsertOneAsync(product);
        //}

        //public async Task<bool> UpdateProduct(Product product)
        //{
        //    var updateResult = await _context.Products
        //        .ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);

        //    return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        //}

        //public async Task<bool> DeleteProduct(string id)
        //{
        //    FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);

        //    DeleteResult deleteResult = await _context.Products.DeleteOneAsync(filter);

        //    return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        //}


        //public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        //{
        //    FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, categoryName);

        //    return await _context.Products.Find(filter).ToListAsync();
        //}
    }
}
