using System;
using ECommerce.Core.DTO;

namespace ECommerce.Core.ServiceContracts
{
    public interface IUpdateProductQuantityInCart
    {
        /// <summary>
        /// Updates the product quantity in cart of a specific user
        /// </summary>
        /// <param name="updatedQuantity">The quantity to update</param>
        /// <param name="userId">The user which cart product quanity to update</param>
        /// <param name="productId">The product to update quantity</param>
        /// <returns>Returns the updated cart items details</returns>
        Task<AddToCartResponse> UpdateProductQuantityInCartItem(int updatedQuantity,Guid userId,Guid productId);
    }
}
