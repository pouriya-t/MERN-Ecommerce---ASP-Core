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

namespace Application.Products.Queries
{
    public class ListReview : IRequest<object>
    {

        public string Id { get; set; }

        public class Handler : IRequestHandler<ListReview, object>
        {
            private readonly IProductRepository _productRepository;

            public Handler(IProductRepository productRepository)
            {
                _productRepository = productRepository;
            }

            public async Task<object> Handle(ListReview query, CancellationToken cancellationToken)
            {
                var product = await _productRepository.GetProduct(query.Id);

                if (product != null)
                {

                    //List<Review> reviews = new List<Review>();

                    //foreach (var review in product.Reviews)
                    //{
                    //    reviews.Add(review);
                    //}

                    return new { Success = true, Reviews = product.Reviews };
                }

                throw new RestException(HttpStatusCode.NotFound, "Your Id not found or your request has a problem");

            }
        }
    }
}
