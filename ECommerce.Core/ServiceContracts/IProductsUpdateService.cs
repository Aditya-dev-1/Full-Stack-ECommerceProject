using ECommerce.Core.DTO;

namespace ServiceContracts
{
    public interface IProductsUpdateService
    {
        /// <summary>
        /// Update the existing product 
        /// </summary>
        /// <param name="productUpdateRequest">Contains Updated details of product</param>
        /// <returns>Returns the updated details of product</returns>
        Task<ProductResponse> UpdateProduct(ProductsUpdateRequest productUpdateRequest);
    }
}
