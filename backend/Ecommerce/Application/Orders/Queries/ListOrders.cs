using Domain.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Orders.Queries
{
    public class ListOrders : IRequest<object>
    {


        public class Handler : IRequestHandler<ListOrders, object>
        {

            private readonly IOrderRepository _orderRepository;
            public Handler(IOrderRepository orderRepository)
            {
                _orderRepository = orderRepository;
            }

            public async Task<object> Handle(ListOrders query, CancellationToken cancellationToken)
            {
                var orders = await _orderRepository.GetOrders();
                double totalAmount = 0.0;
                foreach (var total in orders)
                {
                    totalAmount += total.ItemsPrice;
                }
                return new { Success = true, TotalAmount = totalAmount, Orders = orders };
            }
        }
    }
}
