using Application.Errors;
using Domain.Interfaces;
using Domain.Product;
using MediatR;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
                if (ObjectId.TryParse(query.Id, out _))
                {
                    var product = await _productRepository.GetProduct(query.Id);
                    if(product != null)
                        return new { Success = true , Product = product };
                }

                throw new RestException(HttpStatusCode.NotFound,"Product was not found");

            }
        }
    }
}
