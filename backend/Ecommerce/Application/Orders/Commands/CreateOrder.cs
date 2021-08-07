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
    public class CreateOrder : IRequest<object>
    {

        public ShippingInfo ShippingInfo { get; set; }

        public string User { get; set; }

        public ICollection<OrderItems> OrderItems { get; set; }

        public PaymentInfo PaymentInfo { get; set; }

        public DateTime PaidAt { get; set; }

        public double ItemsPrice { get; set; } = 0.0;

        public double TaxPrice { get; set; } = 0.0;

        public double ShippingPrice { get; set; } = 0.0;

        public double TotalPrice { get; set; } = 0.0;

        public string OrderStatus { get; set; } = "Processing";

        public DateTime DeliveredAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;


        public class Handler : IRequestHandler<CreateOrder, object>
        {
            private readonly IOrderRepository _orderRepository;

            public Handler(IOrderRepository orderRepository)
            {
                _orderRepository = orderRepository;
            }
            public async Task<object> Handle(CreateOrder request, CancellationToken cancellationToken)
            {
                //double totalPrice = 0;
                //foreach (var price in request.OrderItems)
                //{
                //    totalPrice += (price.Price * price.Quantity);
                //}
                var order = new Order
                {
                    ShippingInfo = request.ShippingInfo,
                    User = request.User,
                    OrderItems = request.OrderItems,
                    PaymentInfo = request.PaymentInfo,
                    PaidAt = request.PaidAt,
                    ItemsPrice = request.ItemsPrice,
                    TaxPrice = request.TaxPrice,
                    ShippingPrice = request.ShippingPrice,
                    TotalPrice = request.TotalPrice,
                    OrderStatus = request.OrderStatus,
                    DeliveredAt = request.DeliveredAt,
                    CreatedAt = request.CreatedAt
                };

                await _orderRepository.CreateOrder(order);

                return new { success = true, Order = order };
            }
        }
    }
}
