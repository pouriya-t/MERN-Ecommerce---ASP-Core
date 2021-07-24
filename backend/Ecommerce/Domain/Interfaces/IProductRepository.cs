﻿using Domain.Product;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product.Product>> GetProducts();

        Task<Product.Product> GetProduct(string id);

        Task<IEnumerable<Product.Product>> GetProductByName(string name);

        //Task<IEnumerable<Product.Product>> GetProductByCategory(string categoryName);

        Task CreateProduct(Product.Product product);

        Task<bool> UpdateProduct(Product.Product product);

        Task<bool> DeleteProduct(string id);
    }
}