using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products.Commands
{
    public class Delete : IRequest<object>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<Delete, object>
        {
            private readonly IProductRepository _productRepository;

            public Handler(IProductRepository productRepository)
            {
                _productRepository = productRepository;
            }

            public async Task<object> Handle(Delete query, CancellationToken cancellationToken)
            {
                var success = await _productRepository.DeleteProduct(query.Id);
                if (success)
                {
                    return new { Success = true, Message = "Product is deleted." };
                }

                throw new Exception("some problem");
            }
        }
    }
}
