using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Models.Order;
using Domain.Models.Product;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IMongoContext _context;

        public OrderRepository(IMongoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

   

        public async Task<IEnumerable<Order>> GetOrders()
        {
            return await _context.Orders.Find(p => true).SortByDescending(p => p.CreatedAt).ToListAsync();
        }

        public async Task<Order> GetOrder(string id)
        {
            return await _context.Orders.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Order>> GetMyOrders(string id)
        {
            return await _context.Orders.Find(p => p.User == id)
                .SortByDescending(p => p.CreatedAt).ToListAsync();
        }

        public async Task CreateOrder(Order order)
        {
            await _context.Orders.InsertOneAsync(order);
        }


        public async Task<bool> UpdateOrder(Order order)
        {
            var updateResult = await _context.Orders
                .ReplaceOneAsync(filter: g => g.Id == order.Id, replacement: order);

            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> DeleteOrder(string id)
        {
            FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context.Orders.DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

    }
}
