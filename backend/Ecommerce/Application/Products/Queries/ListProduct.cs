﻿using Domain.Interfaces.Repositories;
using Domain.Models.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products.Queries
{
    public class ListProduct : IRequest<object>
    {

        public string Keyword { get; set; }
        public string Category { get; set; }
        public int? Page { get; set; }
        public IDictionary<string, int> Price { get; set; }

        public class Handler : IRequestHandler<ListProduct, object>
        {
            private readonly IProductRepository _productRepository;

            public Handler(IProductRepository productRepository)
            {
                _productRepository = productRepository;
            }

            public async Task<object> Handle(ListProduct query, CancellationToken cancellationToken)
            {
                IEnumerable<Product> products;
                if (query.Keyword == null)
                {
                    products = await _productRepository.GetProducts(query.Page);
                }
                else
                {
                    products = await _productRepository.FilterProducts(query.Keyword, query.Price,query.Category);
                }
                int countAllProducts = await _productRepository.GetAllProductsCount();
                return new { Success = true, ProductsCount = countAllProducts, ResPerPage = products.Count(), Products = products };
            }
        }
    }
}
