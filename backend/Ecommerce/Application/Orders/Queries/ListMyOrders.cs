using Domain.Interfaces.Repositories;
using Domain.Interfaces.UserAccessor;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Orders.Queries
{
    public class ListMyOrders : IRequest<object>
    {


        public class Handler : IRequestHandler<ListMyOrders, object>
        {

            private readonly IOrderRepository _orderRepository;
            private readonly IUserAccessor _userAccessor;

            public Handler(IOrderRepository orderRepository, IUserAccessor userAccessor)
            {
                _orderRepository = orderRepository;
                _userAccessor = userAccessor;
            }

            public async Task<object> Handle(ListMyOrders query, CancellationToken cancellationToken)
            {
                var user = await _userAccessor.GetUserAsync();
                var orders = await _orderRepository.GetMyOrders(user.Id.ToString());
                return new { Success = true , Orders = orders};
            }
        }
    }
}
