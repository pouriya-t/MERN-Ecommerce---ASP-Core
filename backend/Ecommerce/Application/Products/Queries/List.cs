using Domain.Interfaces;
using Domain.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products.Queries
{
    public class List : IRequest<object>
    {
        public class Handler : IRequestHandler<List, object>
        {
            private readonly IProductRepository _productRepository;

            public Handler(IProductRepository productRepository)
            {
                _productRepository = productRepository;
            }

            public async Task<object> Handle(List query, CancellationToken cancellationToken)
            {
                var products = await _productRepository.GetProducts();
                
                return new { Success = true , Count = products.Count() , Products = products };
            }
        }
    }
}
