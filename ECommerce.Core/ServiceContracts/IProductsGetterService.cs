using ECommerce.Core.DTO;

namespace ServiceContracts
{
    public interface IProductsGetterService
    {
        /// <summary>
        ///  Gives all products based on categoryId from the database
        /// </summary>
        /// <param name="categoryID">CategoryId baesd on which products will be returned</param>
        /// <returns>Returns all the produts based on  category id</returns>
        Task<List<ProductResponse>?> GetProductsBasedOnCategoryId(Guid? categoryID);


        /// <summary>
        /// Search for product based on id
        /// </summary>
        /// <param name="productID">the product to be search</param>
        /// <returns>Retuns product based on id</returns>
        Task<ProductResponse?> GetProductByProductId(Guid? productID);


        /// <summary>
        /// Returns all products from the data store
        /// </summary>
        /// <returns>Returns all products</returns>
        Task<List<ProductResponse>> GetAllProducts();

        /// <summary>
        /// Returns all products which matches with given  searchString
        /// </summary>
        /// <param name="searchString">search String to search</param>
        /// <returns>Returns all matching products based on searchString</returns>
        Task<List<ProductResponse>> GetFilteredProducts(string? searchString);
    }
}
