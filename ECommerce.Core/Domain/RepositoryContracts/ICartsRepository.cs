using System;
using ECommerce.Core.Domain.Entities;
using ECommerce.Core.DTO;

namespace ECommerce.Core.Domain.RepositoryContracts
{
    public interface ICartsRepository
    {
        /// <summary>
        /// Add item to the cart to the data store
        /// </summary>
        /// <param name="cart">the cart to add</param>
        /// <returns>Returns the added cart item details</returns>
        public Task<Carts> AddItemToCart(Carts cart);


        /// <summary>
        /// Deletes the cart item from the data store
        /// </summary>
        /// <param name="cartId">The cart item to remove </param>
        /// <returns>Returns true if deleted otherwise false</returns>
        public Task<bool> RemoveItemFromCartByCartId(Guid cartId);


        /// <summary>
        /// Searches for the cart item in the data store
        /// </summary>
        /// <param name="cartId">The cart item to search</param>
        /// <returns>Returns the cart items details if exist</returns>
        public Task<Carts?> GetCartItemByCartId(Guid cartId);


        /// <summary>
        /// Searches for the cart item in the data store based on userId and productId
        /// </summary>
        /// <param name="userId">The user  which cart item to get</param>
        /// <param name="productId">The product to get from cart</param>
        /// <returns>Returns the cart items details if exist</returns>
        public Task<Carts?> GetcartItemByUserIdProductId(Guid userId ,Guid productId);


        /// <summary>
        /// Updates the item quantity in cart
        /// </summary>
        /// <param name="cart">The cart item to update</param>
        /// <param name="updatedQuantity">The quantity to update</param>
        /// <returns>Returns updated cart items details</returns>
        public Task<Carts> UpdateCartItemQuantity(Carts cart,int updatedQuantity);



        /// <summary>
        /// Returns all carts items from data store (when user is not authenticated)
        /// </summary>
        /// <returns>Returns all carts items from data store</returns>
        public Task<List<Carts>> GetAllCartItemsWithoutUserId();


        /// <summary>
        /// Returns all carts items from data store (when user is  authenticated)
        /// </summary>
        /// <param name="userId">The user which cart items to return</param>
        /// <returns>Returns all carts items from data store</returns>
        public Task<List<Carts>> GetAllCartItemsWithUserId(Guid userId);


        /// <summary>
        /// Checks whether the product exist in the specific user cart or not 
        /// </summary>
        /// <param name="userId">The user which cart will be check</param>
        /// <param name="productId">The product to check</param>
        /// <returns>Returns true if the the specific product is present in the current logged in user; otherwise false</returns>
        Task<bool> IsCartItemExist(Guid userId, Guid productId);
    }
}
