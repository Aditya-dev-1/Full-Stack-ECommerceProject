using ECommerce.Core.DTO;

namespace ServiceContracts
{
    public interface IProductsAdderService
    {
        /// <summary>
        /// Adds a product to the database
        /// </summary>
        /// <param name="productAddRequest">Contains the product details to add</param>
        /// <returns>Returns the added product details</returns>
        Task<ProductResponse> AddProduct(ProductsAddRequest? productAddRequest);
        
    }
}
