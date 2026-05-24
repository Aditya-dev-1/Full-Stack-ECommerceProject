using System;
using System.Security.Claims;
using ECommerce.Core.DTO;

namespace ECommerce.Core.ServiceContracts
{
    public interface IGetCartItemsService
    {
        /// <summary>
        /// Gives all the cart items of the specific user based on user  
        /// </summary>
        /// <param name="user">The user which cart items will be returns </param>
        /// <returns>Returns all the cart items of the specific user based on user </returns>
        Task<List<AddToCartResponse>> GetAllCartItems(string? userId);


        /// <summary>
        /// Checks whether the product exist in the specific user cart or not 
        /// </summary>
        /// <param name="userId">The user which cart will be check</param>
        /// <param name="productId">The product to check</param>
        /// <returns>Returns true if the the specific product is present in the current logged in user; otherwise false</returns>
        Task<bool> IsCartItemExist(Guid userId, Guid productId);
    }
}
