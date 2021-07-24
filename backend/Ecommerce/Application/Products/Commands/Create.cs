using Domain.Interfaces;
using Domain.Product;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products.Commands
{
    public class Create : IRequest<object>
    {

        public string Name { get; set; }
        public double? Price { get; set; }
        public string Description { get; set; }
        public double Ratings { get; set; }
        public ICollection<Image> Images { get; set; }
        public string Category { get; set; }
        public string Seller { get; set; }
        public double Stock { get; set; }
        public int NumOfReviews { get; set; }
        public ICollection<Review> Reviews { get; set; }


        public class Handler : IRequestHandler<Create, object>
        {
            private readonly IProductRepository _productRepository;
            public Handler(IProductRepository productRepository)
            {
                _productRepository = productRepository;
            }
            public async Task<object> Handle(Create request, CancellationToken cancellationToken)
            {
                var product = new Product
                {
                    Name = request.Name,
                    Price = request.Price,
                    Description = request.Description,
                    Ratings = request.Ratings,
                    Images = request.Images,
                    Category = request.Category,
                    Seller = request.Seller,
                    Stock = request.Stock,
                    NumOfReviews = request.NumOfReviews,
                    Reviews = request.Reviews
                };

                await _productRepository.CreateProduct(product);

                return new { success = true, Product = product  };
            }
        }
    }
}
