﻿using Application.Errors;
using Domain.Interfaces.Repositories;
using MediatR;
using MongoDB.Bson;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products.Queries
{
    public class DetailProduct : IRequest<object>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<DetailProduct, object>
        {
            private readonly IProductRepository _productRepository;

            public Handler(IProductRepository productRepository)
            {
                _productRepository = productRepository;
            }

            public async Task<object> Handle(DetailProduct query, CancellationToken cancellationToken)
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
