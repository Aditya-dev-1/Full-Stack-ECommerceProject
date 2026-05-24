using System;
using Entities;
using RepositoryContracts;
using ServiceContracts;

namespace Services
{
    public class ProductsDeleteService : IProductsDeleteService
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IImageDeleterService _imageDeleterService;

        public ProductsDeleteService(IProductsRepository productsRepository ,IImageDeleterService imageDeleterService)
        {
            _productsRepository = productsRepository;
            _imageDeleterService = imageDeleterService;
        }

        public async Task<bool> DeleteProduct(Guid? productID)
        {
            if(productID == null)
            {
               throw new ArgumentNullException(nameof(productID));
            }

            Product? matchinProduct = await _productsRepository.GetProductById(productID.Value);

            if(matchinProduct == null)
            {
                return false;
            }

            //callling imageDeleter() to delete image from wwwroot
          bool isImageDeleted =  await _imageDeleterService.ImageDeleter(matchinProduct.Product_Image_Url);
            if (!isImageDeleted)
                return false;

            await _productsRepository.DeleteProductByProductId(matchinProduct.Id);

            return true;
        }
    }
}
