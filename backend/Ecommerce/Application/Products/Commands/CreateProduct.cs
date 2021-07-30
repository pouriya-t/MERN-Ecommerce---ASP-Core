using Domain.Interfaces.Repositories;
using Domain.Interfaces.UserAccessor;
using Domain.Models.Product;
using MediatR;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products.Commands
{
    public class CreateProduct : IRequest<object>
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter product price")]
        [Range(typeof(double), "0", "99999", ErrorMessage = "Product price cannot exceed 5 digit")]
        public double? Price { get; set; }

        [Required(ErrorMessage = "Please enter product description")]
        public string Description { get; set; }

        public double Ratings { get; set; } = 0;

        public ICollection<Image> Images { get; set; }

        [Required(ErrorMessage = "Please select category")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Please product seller")]
        public string Seller { get; set; }

        [Required(ErrorMessage = "Please enter product stock")]
        [Range(typeof(double), "0", "99999", ErrorMessage = "Product stock cannot exceed 5 digit")]
        public double Stock { get; set; }

        public int NumOfReviews { get; set; }

        [Required]
        public ICollection<Review> Reviews { get; set; }


        public class Handler : IRequestHandler<CreateProduct, object>
        {
            private readonly IProductRepository _productRepository;
            private readonly IUserAccessor _userAccessor;

            public Handler(IProductRepository productRepository, IUserAccessor userAccessor)
            {
                _productRepository = productRepository;
                _userAccessor = userAccessor;
            }
            public async Task<object> Handle(CreateProduct request, CancellationToken cancellationToken)
            {
                var user = _userAccessor.GetUserAsync();

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
                    Reviews = request.Reviews,
                    User = user.Id.ToString()
                };
                
                await _productRepository.CreateProduct(product);

                return new { success = true, Product = product  };
            }
        }
    }
}
