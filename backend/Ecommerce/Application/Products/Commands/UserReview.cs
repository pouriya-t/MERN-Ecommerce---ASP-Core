using Application.Errors;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.UserAccessor;
using Domain.Models.Product;
using MediatR;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products.Commands
{
    public class UserReview : IRequest<object>
    {
        public double Rating { get; set; }

        public string Comment { get; set; }

        public string ProductId { get; set; }


        public class Handler : IRequestHandler<UserReview, object>
        {
            private readonly IUserAccessor _userAccessor;
            private readonly IProductRepository _productRepository;

            public Handler(IProductRepository productRepository, IUserAccessor userAccessor)
            {
                _productRepository = productRepository;
                _userAccessor = userAccessor;
            }

            public async Task<object> Handle(UserReview command, CancellationToken cancellationToken)
            {
                var user = await _userAccessor.GetUserAsync();

                var product = await _productRepository.GetProduct(command.ProductId);

                if (product != null)
                {
                    var review = new Review
                    {
                        User = user.Id.ToString(),
                        Name = user.Name,
                        Rating = command.Rating,
                        Comment = command.Comment
                    };

                    if(product.Reviews == null)
                    {
                        product.Reviews = new List<Review>();
                    }

                    product.Reviews.Add(review);

                    var success = await _productRepository.UpdateProduct(product);

                    if (success)
                    {
                        return new { Success = true };
                    }
                }
                
                throw new RestException(HttpStatusCode.BadRequest, "Sometimes went wrong");
            }
        }
    }
}
