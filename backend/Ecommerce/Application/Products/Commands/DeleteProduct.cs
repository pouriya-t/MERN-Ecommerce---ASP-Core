using Domain.Interfaces.PhotoAccessor;
using Domain.Interfaces.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products.Commands
{
    public class DeleteProduct : IRequest<object>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<DeleteProduct, object>
        {
            private readonly IProductRepository _productRepository;
            private readonly IPhotoAccessor _photoAccessor;

            public Handler(IProductRepository productRepository, IPhotoAccessor photoAccessor)
            {
                _productRepository = productRepository;
                _photoAccessor = photoAccessor;
            }

            public async Task<object> Handle(DeleteProduct query, CancellationToken cancellationToken)
            {
                var product = await _productRepository.GetProduct(query.Id);

                foreach (var image in product.Images)
                {
                    _photoAccessor.DeletePhoto(image.PublicId);
                }
                var success = await _productRepository.DeleteProduct(product.Id);
                if (success)
                {
                    return new { Success = true, Message = "Product is deleted." };
                }

                throw new Exception("some problem");
            }
        }
    }
}
