using System;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using RepositoryContracts;

namespace Repositories
{
    public class ProductsRepositories : IProductsRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductsRepositories(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Product> AddProduct(Product product)
        {
            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteProductByProductId(Guid productId)
        {
            _db.Products.RemoveRange(_db.Products.Where(temp => temp.Id == productId));
            int rowsDeletd = await _db.SaveChangesAsync();
            return rowsDeletd > 0;

        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await _db.Products.ToListAsync();
        }

        public async Task<List<Product>> GetFilteredProducts(string searchString)
        {
           return await _db.Products
                .Where(product=> EF.Functions.Like(product.Product_Name.ToLower(),$"%{searchString.ToLower()}%"))
                .ToListAsync();
        }

        public async Task<Product?> GetProductById(Guid productId)
        {
           return await _db.Products.Include(t=>t.Categories).FirstOrDefaultAsync(temp => temp.Id == productId);
            
        }

        public async Task<List<Product>> GetProductsBasedOnCategoryId(Guid categoryId)
        {
           return await _db.Products.Where(temp=>temp.Categories.Id == categoryId).Include(t=>t.Categories).ToListAsync();
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            Product? matchingProduct = await _db.Products
                .FirstOrDefaultAsync(temp => temp.Id == product.Id);

            if(matchingProduct == null)
            {
                return product;
            }

            matchingProduct.Product_Name = product.Product_Name;
            matchingProduct.Product_Image_Url = product.Product_Image_Url;
            matchingProduct.Description = product.Description;
            matchingProduct.Price = product.Price;
            matchingProduct.qty_in_stock = product.qty_in_stock;

            await _db.SaveChangesAsync();
            return matchingProduct;
        }
    }
}
