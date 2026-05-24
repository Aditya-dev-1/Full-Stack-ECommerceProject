using Entities;

namespace RepositoryContracts
{
    public interface IProductsRepository
    {
        /// <summary>
        /// Add Product in the data store
        /// </summary>
        /// <param name="product">product to add in data store</param>
        /// <returns>Returns the added product details</returns>
        Task<Product> AddProduct(Product product);


        /// <summary>
        /// Updates the product in the data store
        /// </summary>
        /// <param name="product">product to update</param>
        /// <returns>Returns the updated product details</returns>
        Task<Product> UpdateProduct(Product product);


        /// <summary>
        /// Delete the product from the data store
        /// </summary>
        /// <param name="productId">The product to delete</param>
        /// <returns>Returns true if product is deleted ; otherwise false</returns>
        Task<bool> DeleteProductByProductId(Guid productId);


        /// <summary>
        /// Search for the product based on the product id 
        /// </summary>
        /// <param name="productId">the product id to search</param>
        /// <returns>Returns the product from the data store</returns>
        Task<Product?> GetProductById(Guid productId);


        /// <summary>
        /// Returns All products based on category id
        /// </summary>
        /// <param name="categoryId">the id based on which the products will be returned </param>
        /// <returns>Returns the list of products based on category id</returns>
        Task<List<Product>> GetProductsBasedOnCategoryId(Guid categoryId);

        /// <summary>
        /// Returns all products from the data store
        /// </summary>
        /// <returns>Returns all products</returns>
        Task<List<Product>> GetAllProducts();

        /// <summary>
        /// Returns all products which matches with given  searchString from the data store
        /// </summary>
        /// <param name="searchString">search String to search</param>
        /// <returns>Returns all matching products based on searchString from the data store</returns>
        Task<List<Product>> GetFilteredProducts(string searchString);
    }
}
