using System;
using ECommerce.Core.Domain.Entities;
using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.DTO;
using ECommerce.Core.ServiceContracts;

namespace ECommerce.Core.Services
{
    public class UpdateProductQuantityInCart : IUpdateProductQuantityInCart
    {
        private readonly ICartsRepository _cartsRepository;

        public UpdateProductQuantityInCart(ICartsRepository cartsRepository)
        {
            _cartsRepository = cartsRepository;
        }

        public async Task<AddToCartResponse> UpdateProductQuantityInCartItem(int updatedQuantity, Guid userId, Guid productId)
        {
            if(userId == Guid.Empty || productId == Guid.Empty || updatedQuantity==0)
            {
                throw new ArgumentNullException(nameof(productId),nameof(userId));
            }

            Carts? cart = await _cartsRepository.GetcartItemByUserIdProductId(userId, productId);
            if (cart == null)
                throw new ArgumentNullException("cart Item does not exist for the current user");

            Carts updatedCart = await _cartsRepository.UpdateCartItemQuantity(cart, updatedQuantity);
            return updatedCart.ToAddToCartResponse();


        }
    }
}
