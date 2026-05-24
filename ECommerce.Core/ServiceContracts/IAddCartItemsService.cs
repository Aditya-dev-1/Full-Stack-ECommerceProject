using System;
using ECommerce.Core.DTO;

namespace ECommerce.Core.ServiceContracts
{
    public interface IAddCartItemsService
    {
        /// <summary>
        /// Adds the item in cart 
        /// </summary>
        /// <param name="addToCart">The product to add</param>
        /// <returns></returns>
        Task<AddToCartResponse> AddItemToCart(AddToCartRequest addToCart);
    }
}
