using Domain.Interfaces.Repositories;
using Domain.Models.Order;
using MediatR;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Orders.Commands
{
    public class EditOrder : IRequest<object>
    {

        public string Id { get; set; }

        public string User { get; set; }

        public double ItemsPrice { get; set; }

        public double TaxPrice { get; set; }

        public double ShippingPrice { get; set; }

        public string Status { get; set; }


        public class Handler : IRequestHandler<EditOrder, object>
        {
            private readonly IOrderRepository _orderRepository;

            public Handler(IOrderRepository orderRepository)
            {
                _orderRepository = orderRepository;
            }
            public async Task<object> Handle(EditOrder request, CancellationToken cancellationToken)
            {

                var order = await _orderRepository.GetOrder(request.Id);

                order.User = request.User ?? order.User;
                if (request.ItemsPrice.ToString() != "" || request.ItemsPrice.ToString() != null)
                {
                    order.ItemsPrice = request.ItemsPrice;
                }
                if (request.TaxPrice.ToString() != "" || request.TaxPrice.ToString() != null)
                {
                    order.TaxPrice = request.TaxPrice;
                }
                if (request.ShippingPrice.ToString() != "" || request.ShippingPrice.ToString() != null)
                {
                    order.ShippingPrice = request.ShippingPrice;
                }
                order.OrderStatus = request.Status ?? order.OrderStatus;                



                await _orderRepository.UpdateOrder(order);

                return new { success = true, Order = order };
            }
        }
    }
}
