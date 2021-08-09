using Domain.Interfaces.PhotoAccessor;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.UserAccessor;
using Domain.Models.ImageModel;
using Domain.Models.Product;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products.Commands
{
    public class CreateProduct : IRequest<object>
    {

        public string Name { get; set; }

        public double? Price { get; set; }

        public string Description { get; set; }

        public IFormFileCollection Images { get; set; }

        public string Category { get; set; }

        public string Seller { get; set; }

        public double Stock { get; set; }


        public class Handler : IRequestHandler<CreateProduct, object>
        {
            private readonly IProductRepository _productRepository;
            private readonly IUserAccessor _userAccessor;
            private readonly IPhotoAccessor _photoAccessor;

            public Handler(IProductRepository productRepository, IUserAccessor userAccessor,
                IPhotoAccessor photoAccessor)
            {
                _productRepository = productRepository;
                _userAccessor = userAccessor;
                _photoAccessor = photoAccessor;
            }
            public async Task<object> Handle(CreateProduct request, CancellationToken cancellationToken)
            {
                var user = await _userAccessor.GetUserAsync();

                var product = new Product
                {
                    Name = request.Name,
                    Price = request.Price,
                    Description = request.Description,
                    Category = request.Category,
                    Seller = request.Seller,
                    Stock = request.Stock,
                    User = user.Id.ToString()
                };


                foreach (var image in request.Images)
                {
                    var photoUploadResult = _photoAccessor.AddPhoto(image);
                    var photo = new Image
                    {
                        PublicId = photoUploadResult.PublicId,
                        Url = photoUploadResult.Url
                    };
                    product.Images.Add(photo);
                }

                await _productRepository.CreateProduct(product);

                return new { success = true, Product = product };
            }
        }
    }
}
