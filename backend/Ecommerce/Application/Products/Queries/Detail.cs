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
    public class Detail : IRequest<object>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<Detail, object>
        {
            private readonly IProductRepository _productRepository;

            public Handler(IProductRepository productRepository)
            {
                _productRepository = productRepository;
            }

            public async Task<object> Handle(Detail query, CancellationToken cancellationToken)
            {
                var product = await _productRepository.GetProduct(query.Id);
                return new { Success = true , Product = product };
            }
        }
    }
}
