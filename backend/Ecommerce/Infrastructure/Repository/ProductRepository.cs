using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Models.Product;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoContext _context;

        public ProductRepository(IMongoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Product>> GetProducts(int? page)
        {
            int limit = 4;

            var queryable = await _context.Products.AsQueryable().ToListAsync();


            page = (page < 0) ? 1 : page;

            var startRow = (page - 1) * limit;

            var totalPages = (int)Math.Ceiling(queryable.Count / (double)limit);



            var products = await _context.Products.Find(p => true)
                                       .SortByDescending(p => p.CreatedAt)
                                       .Limit(limit)
                                       .Skip(startRow).ToListAsync();


            return products;

            // Simple return products without filter page
            // return await _context.Products.Find(p => true).ToListAsync();
        }


        public async Task<Product> GetProduct(string id)
        {
            return await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> GetAllProductsCount()
        {
            return await _context.Products.AsQueryable().CountAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.ElemMatch(p => p.Name, name);

            return await _context.Products.Find(filter).ToListAsync();
        }

        public async Task CreateProduct(Product product)
        {
            await _context.Products.InsertOneAsync(product);
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updateResult = await _context.Products
                .ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);

            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context.Products.DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<Product>> FilterProducts(string keyword,
                        IDictionary<string, int> price, string category)
        {
            string searchToLower = keyword.ToLower();
            string searchToUpper = keyword.ToUpper();
            var upToFirstChar = searchToLower[0].ToString().ToUpper() + searchToLower.Substring(1);

            FilterDefinition<Product> filterByPrice = null;



            var filterByName = Builders<Product>.Filter.
                Where(p => p.Name.Contains(searchToLower) | p.Name.Contains(searchToUpper)
                | p.Name.Contains(keyword) | p.Name.Contains(upToFirstChar));

            // Search a word in description and name
            var filterByDescription = Builders<Product>.Filter.
                Where(p => p.Description.Contains(searchToLower) | p.Description.Contains(searchToUpper)
                | p.Description.Contains(keyword) | p.Description.Contains(upToFirstChar));

            var getProductCategory = Builders<Product>.Filter.Where(p => p.Category == category);

            if (price.ContainsKey("gt") || price.ContainsKey("lt")
                || price.ContainsKey("gte") || price.ContainsKey("lte"))
            {
                if (price.ContainsKey("lt") && price.ContainsKey("gt"))
                {
                    var lt = Builders<Product>.Filter.Lt(p => p.Price, price["lt"]);
                    var gt = Builders<Product>.Filter.Gt(p => p.Price, price["gt"]);

                    if (category != null)
                        return await _context.Products
                        .Find((getProductCategory) & (filterByName | filterByDescription) & (lt & gt)).ToListAsync();
                    else
                        return await _context.Products
                        .Find((filterByName | filterByDescription) & (lt & gt)).ToListAsync();
                }

                else if (price.ContainsKey("lte") && price.ContainsKey("gt"))
                {
                    var lte = Builders<Product>.Filter.Lt(p => p.Price, price["lte"]);
                    var gt = Builders<Product>.Filter.Gt(p => p.Price, price["gt"]);


                    if (category != null)
                        return await _context.Products
                        .Find((getProductCategory) & (filterByName | filterByDescription) & (lte & gt)).ToListAsync();
                    else
                        return await _context.Products
                        .Find((filterByName | filterByDescription) & (lte & gt)).ToListAsync();
                }

                else if (price.ContainsKey("lt") && price.ContainsKey("gte"))
                {
                    var lt = Builders<Product>.Filter.Lt(p => p.Price, price["lt"]);
                    var gte = Builders<Product>.Filter.Gt(p => p.Price, price["gte"]);


                    if (category != null)
                        return await _context.Products
                        .Find((getProductCategory) & (filterByName | filterByDescription) & (lt & gte)).ToListAsync();
                    else
                        return await _context.Products
                        .Find((filterByName | filterByDescription) & (lt & gte)).ToListAsync();
                }

                else if (price.ContainsKey("lte") && price.ContainsKey("gte"))
                {
                    var lte = Builders<Product>.Filter.Lt(p => p.Price, price["lte"]);
                    var gte = Builders<Product>.Filter.Gt(p => p.Price, price["gte"]);


                    if (category != null)
                        return await _context.Products
                        .Find((getProductCategory) & (filterByName | filterByDescription) & (lte & gte)).ToListAsync();
                    else
                        return await _context.Products
                        .Find((filterByName | filterByDescription) & (lte & gte)).ToListAsync();
                }

                else if (price.ContainsKey("gt") || price.ContainsKey("gte"))
                {
                    if (price.ContainsKey("gt"))
                        filterByPrice = Builders<Product>.Filter.Gt(p => p.Price, price["gt"]);
                    else
                        filterByPrice = Builders<Product>.Filter.Gte(p => p.Price, price["gte"]);

                    //return await _context.Products.Find(filterByPrice).ToListAsync();
                    if (category != null)
                        return await _context.Products
                        .Find((getProductCategory) & (filterByName | filterByDescription) & (filterByPrice)).ToListAsync();
                    else
                        return await _context.Products
                        .Find((filterByName | filterByDescription) & (filterByPrice)).ToListAsync();
                }

                else if (price.ContainsKey("lt") || price.ContainsKey("lte"))
                {
                    if (price.ContainsKey("lt"))
                        filterByPrice = Builders<Product>.Filter.Lt(p => p.Price, price["lt"]);
                    else
                        filterByPrice = Builders<Product>.Filter.Lte(p => p.Price, price["lte"]);

                    if (category != null)
                        return await _context.Products
                        .Find((getProductCategory) & (filterByName | filterByDescription) & (filterByPrice)).ToListAsync();
                    else
                        return await _context.Products
                        .Find((filterByName | filterByDescription) & (filterByPrice)).ToListAsync();
                }
            }
            if (category != null)
                return await _context.Products
                .Find((getProductCategory) & (filterByName | filterByDescription)).ToListAsync();
            else
                return await _context.Products
                .Find(filterByName | filterByDescription).ToListAsync();
        }

        //public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        //{
        //    FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, categoryName);

        //    return await _context.Products.Find(filter).ToListAsync();
        //}
    }
}
