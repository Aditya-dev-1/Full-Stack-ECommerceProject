using System;
using Entities;
using RepositoryContracts;
using ServiceContracts;
using ECommerce.Core.DTO;
using Services.Helpers;

namespace Services
{
    public class ProductsUpdateService : IProductsUpdateService
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IImageUpdaterService _imageUpdaterService;

        public ProductsUpdateService(IProductsRepository productsRepository, IImageUpdaterService imageUpdaterService)
        {
            _productsRepository = productsRepository;
            _imageUpdaterService = imageUpdaterService;
        }

        public async Task<ProductResponse> UpdateProduct(ProductsUpdateRequest productUpdateRequest)
        {
            if (productUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(productUpdateRequest));
            }

            //Validations
            ValidationHelper.ModelValidator(productUpdateRequest);

            //Checking Product Id
            Product? matchingProduct = await _productsRepository.GetProductById(productUpdateRequest.ProductId.Value);

            if (matchingProduct == null)
            {
                throw new ArgumentException("Product Id does not exist");
            }

            if(productUpdateRequest.Product_Image != null&&productUpdateRequest.Product_Image.Length!=0)
            {
                string? new_Image_Url = await _imageUpdaterService.ImageUpdater(productUpdateRequest.Product_Image, productUpdateRequest.Product_Image_Url);

                productUpdateRequest.Product_Image_Url = new_Image_Url;
            }

           

            matchingProduct.Price = productUpdateRequest.Price.Value;
            matchingProduct.Product_Name = productUpdateRequest.Product_Name;
            matchingProduct.qty_in_stock = productUpdateRequest.qty_in_stock.Value;
            matchingProduct.Description = productUpdateRequest.Description;
            matchingProduct.Product_Image_Url = productUpdateRequest.Product_Image_Url;
            
            await _productsRepository.UpdateProduct(matchingProduct);

            return matchingProduct.ToProductResponse();
        }
    }
}
