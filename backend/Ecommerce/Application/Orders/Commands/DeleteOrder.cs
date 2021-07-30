using Application.Errors;
using Domain.Interfaces.Repositories;
using Domain.Models.Order;
using MediatR;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Orders.Commands
{
    public class DeleteOrder : IRequest<object>
    {

        public string Id { get; set; }



        public class Handler : IRequestHandler<DeleteOrder, object>
        {
            private readonly IOrderRepository _orderRepository;

            public Handler(IOrderRepository orderRepository)
            {
                _orderRepository = orderRepository;
            }
            public async Task<object> Handle(DeleteOrder request, CancellationToken cancellationToken)
            {

                var order = await _orderRepository.DeleteOrder(request.Id);
                if (order)
                {
                    return new { success = true };
                }
                throw new RestException(HttpStatusCode.BadRequest, "ID not found or your request has a problem");
            }
        }
    }
}
