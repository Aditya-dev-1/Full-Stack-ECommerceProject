using Entities;
using RepositoryContracts;
using ServiceContracts;
using ECommerce.Core.DTO;
using Services.Helpers;

namespace Services
{
    public class ProductsAdderService : IProductsAdderService
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IImageAdderService _imageAdderService;

        public ProductsAdderService(IProductsRepository productsRepository,IImageAdderService imageAdderService)
        {
            _productsRepository = productsRepository;
            _imageAdderService = imageAdderService;
        }

        public async Task<ProductResponse> AddProduct(ProductsAddRequest? productAddRequest)
        {
            if(productAddRequest == null)
            {
                throw new ArgumentNullException(nameof(productAddRequest));
            }
            if (productAddRequest.Product_Image==null)
            {
                throw new ArgumentException(nameof(productAddRequest.Product_Image));
            }


            //calling image service method
            string filePath = await _imageAdderService.ImageAdder(productAddRequest.Product_Image);
            productAddRequest.ImagePath_url = filePath;

            //Validations
            ValidationHelper.ModelValidator(productAddRequest);
            

            Product product = productAddRequest.ToProduct();
            product.Id = Guid.NewGuid();
            

            await _productsRepository.AddProduct(product);

            return product.ToProductResponse();
        }

        
    }
}
