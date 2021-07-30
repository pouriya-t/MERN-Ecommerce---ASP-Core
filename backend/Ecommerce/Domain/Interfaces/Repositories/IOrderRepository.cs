using Domain.Models.Order;
using Domain.Models.Product;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetOrders();
        Task<Order> GetOrder(string id);
        Task<IEnumerable<Order>> GetMyOrders(string id);
        Task CreateOrder(Order order);
        Task<bool> UpdateOrder(Order order);
        Task<bool> DeleteOrder(string id);
    }
}