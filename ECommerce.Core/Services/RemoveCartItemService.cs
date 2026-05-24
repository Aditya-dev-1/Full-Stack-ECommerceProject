using System;
using ECommerce.Core.Domain.Entities;
using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.ServiceContracts;

namespace ECommerce.Core.Services
{
    public class RemoveCartItemService : IRemoveCartItemService
    {
        private readonly ICartsRepository _cartsRepository;

        public RemoveCartItemService(ICartsRepository cartsRepository)
        {
            _cartsRepository = cartsRepository;
        }

        public async Task<bool> RemoveCartItem(Guid cartId)
        {
            if(cartId == Guid.Empty)
                throw new ArgumentNullException(nameof(cartId));

           Carts? matchingCart = await _cartsRepository.GetCartItemByCartId(cartId);
           if(matchingCart == null)
                return false;

           bool isDeleted = await _cartsRepository.RemoveItemFromCartByCartId(matchingCart.Id);
            if (isDeleted)
                return true;
            return false;
        }
    }
}
