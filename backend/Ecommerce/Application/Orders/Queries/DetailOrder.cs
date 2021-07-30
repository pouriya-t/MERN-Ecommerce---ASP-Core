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
    public class DetailOrder : IRequest<object>
    {

        public string Id { get; set; }

        public class Handler : IRequestHandler<DetailOrder, object>
        {

            private readonly IOrderRepository _orderRepository;
            public Handler(IOrderRepository orderRepository)
            {
                _orderRepository = orderRepository;
            }

            public async Task<object> Handle(DetailOrder query, CancellationToken cancellationToken)
            {
                var order = await _orderRepository.GetOrder(query.Id);
                return new { Success = true, Order = order };
            }
        }
    }
}
