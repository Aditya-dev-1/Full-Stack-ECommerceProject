using ECommerce.Core.DTO;

namespace ServiceContracts
{
    public interface IProductsDeleteService
    {
        /// <summary>
        /// Delete a product from the database
        /// </summary>
        /// <param name="productID">The ProductId of the product to  delete</param>
        /// <returns>Returns true if the product is deleted; otherwise false</returns>
        Task<bool> DeleteProduct(Guid? productID);
    }
}
