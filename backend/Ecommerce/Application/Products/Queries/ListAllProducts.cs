using Domain.Interfaces.Repositories;
using Domain.Models.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products.Queries
{
    public class ListAllProducts : IRequest<object>
    {

        public class Handler : IRequestHandler<ListAllProducts, object>
        {
            private readonly IProductRepository _productRepository;

            public Handler(IProductRepository productRepository)
            {
                _productRepository = productRepository;
            }

            public async Task<object> Handle(ListAllProducts query, CancellationToken cancellationToken)
            {
                var products = await _productRepository.GetAllProductsAsync();
                return new { Success = true, Products = products };
            }
        }
    }
}
