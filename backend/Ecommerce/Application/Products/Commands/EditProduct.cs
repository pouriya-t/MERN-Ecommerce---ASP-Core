using Domain.Interfaces.Repositories;
using Domain.Models.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Domain.Models.ImageModel;
using Microsoft.AspNetCore.Http;
using Domain.Interfaces.PhotoAccessor;

namespace Application.Products.Commands
{
    public class EditProduct : IRequest<object>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public double? Price { get; set; }

        public string Description { get; set; }

        public IFormFileCollection Images { get; set; }

        public string Category { get; set; }

        public string Seller { get; set; }

        public double? Stock { get; set; }



        public class Handler : IRequestHandler<EditProduct, object>
        {
            private readonly IProductRepository _productRepository;
            private readonly IPhotoAccessor _photoAccessor;

            public Handler(IProductRepository productRepository, IPhotoAccessor photoAccessor)
            {
                _productRepository = productRepository;
                _photoAccessor = photoAccessor;
            }

            public async Task<object> Handle(EditProduct command, CancellationToken cancellationToken)
            {
                var product = await _productRepository.GetProduct(command.Id);

                product.Name = command.Name ?? product.Name;
                product.Price = command.Price ?? product.Price;
                product.Description = command.Description ?? product.Description;
                product.Category = command.Category ?? product.Category;
                product.Seller = command.Seller ?? product.Seller;
                product.Stock = command.Stock ?? product.Stock;

                if(command.Images != null)
                {
                    if(product.Images != null)
                    {
                        foreach (var image in product.Images)
                        {
                            _photoAccessor.DeletePhoto(image.PublicId);
                        }
                        product.Images = new List<Image>();
                    }

                    foreach (var image in command.Images)
                    {
                        var photoUploadResult = _photoAccessor.AddPhoto(image);
                        var photo = new Image
                        {
                            PublicId = photoUploadResult.PublicId,
                            Url = photoUploadResult.Url
                        };
                        product.Images.Add(photo);
                    }
                }

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
