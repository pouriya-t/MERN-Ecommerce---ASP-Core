using Application.Errors;
using Domain.Interfaces.Repositories;
using Domain.Models.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products.Commands
{
    public class DeleteReview : IRequest<object>
    {

        public string Id { get; set; }
        public string ProductId { get; set; }

        public class Handler : IRequestHandler<DeleteReview, object>
        {
            private readonly IProductRepository _productRepository;

            public Handler(IProductRepository productRepository)
            {
                _productRepository = productRepository;
            }

            public async Task<object> Handle(DeleteReview command, CancellationToken cancellationToken)
            {
                var product = await _productRepository.GetProduct(command.ProductId);

                if (product != null)
                {
                    var review = product.Reviews.FirstOrDefault(r => r.Id == command.Id);

                    product.Reviews.Remove(review);

                    var success = await _productRepository.UpdateProduct(product);
                    if (success)
                    {
                        return new { Success = true };
                    }
                }

                throw new RestException(HttpStatusCode.NotFound, "Your Id not found or your request has a problem");

            }
        }
    }
}
