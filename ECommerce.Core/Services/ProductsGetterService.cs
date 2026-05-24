using System;
using Entities;
using RepositoryContracts;
using ServiceContracts;
using ECommerce.Core.DTO;
using System.Net.Http.Headers;

namespace Services
{
    public class ProductsGetterService : IProductsGetterService
    {
        private readonly IProductsRepository _productsRepository;
        private readonly ICategoriesGetterService _categoriesGetterService;

        public ProductsGetterService(IProductsRepository productsRepository, ICategoriesGetterService categoriesGetterService)
        {
            _productsRepository = productsRepository;
            _categoriesGetterService = categoriesGetterService;
        }

        public async Task<List<ProductResponse>> GetAllProducts()
        {
           List<Product> products =  await _productsRepository.GetAllProducts();
            return products.Select(temp => temp.ToProductResponse()).ToList();
        }

        public async Task<List<ProductResponse>> GetFilteredProducts(string? searchString)
        {
            if(searchString == null)
            {
                List<ProductResponse> products = await GetAllProducts();
                return products;
            }

            List<Product> allProducts = await _productsRepository.GetFilteredProducts(searchString);

            return allProducts
                .Select(temp => temp.ToProductResponse())
                .ToList();
        }

        public async Task<ProductResponse?> GetProductByProductId(Guid? productID)
        {
            if (productID == null)
            {
                return null;
            }

            Product? matchingProduct = await _productsRepository.GetProductById(productID.Value);

            if (matchingProduct == null)
            {
                return null;
            }

            return matchingProduct.ToProductResponse();
        }

        public async Task<List<ProductResponse>?> GetProductsBasedOnCategoryId(Guid? categoryID)
        {
           if(categoryID == null)
           {
                return null;
           }

          CategoryResponse? category = await _categoriesGetterService.GetCategoryByCategoryId(categoryID.Value);

            if(category == null)
            {
                return null;
            }

           List<Product> products = await _productsRepository.GetProductsBasedOnCategoryId(categoryID.Value);

           if (products == null)
           {
                return null;
           }

            return products.Select(temp => temp.ToProductResponse()).ToList();

        }
    }
}
