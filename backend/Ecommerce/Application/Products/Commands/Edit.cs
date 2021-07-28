using Domain.Interfaces.Repositories;
using Domain.Models.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Application.Products.Commands
{
    public class Edit : IRequest<object>
    {

        public string Id { get; set; }

        public string Name { get; set; }

        [Range(typeof(double), "0", "99999", ErrorMessage = "Product price cannot exceed 5 digit")]
        public double? Price { get; set; }

        public string Description { get; set; }

        public double? Ratings { get; set; }

        public ICollection<Image> Images { get; set; }

        public string Category { get; set; }

        public string Seller { get; set; }

        [Range(typeof(double), "0", "99999", ErrorMessage = "Product stock cannot exceed 5 digit")]
        public double? Stock { get; set; }

        public int? NumOfReviews { get; set; }

        public ICollection<Review> Reviews { get; set; }



        public class Handler : IRequestHandler<Edit, object>
        {
            private readonly IProductRepository _productRepository;

            public Handler(IProductRepository productRepository)
            {
                _productRepository = productRepository;
            }

            public async Task<object> Handle(Edit command, CancellationToken cancellationToken)
            {
                var product = await _productRepository.GetProduct(command.Id);

                product.Name = command.Name ?? product.Name;
                product.Price = command.Price ?? product.Price;
                product.Description = command.Description ?? product.Description;
                product.Ratings = command.Ratings ?? product.Ratings;
                product.Images = command.Images ?? product.Images;
                product.Category = command.Category ?? product.Category;
                product.Seller = command.Seller ?? product.Seller;
                product.Stock = command.Stock ?? product.Stock;
                product.NumOfReviews = command.NumOfReviews ?? product.NumOfReviews;
                product.Reviews = command.Reviews ?? product.Reviews;

                var success = await _productRepository.UpdateProduct(product);

                if (success)
                {
                    return new { Success = true, Product = product };
                }

                throw new Exception("some problem");
            }
        }
    }
}
