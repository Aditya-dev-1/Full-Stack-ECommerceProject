using System;
using ECommerce.Core.Domain.Entities;
using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.DTO;
using ECommerce.Core.ServiceContracts;
using Services.Helpers;

namespace ECommerce.Core.Services
{
    public class AddCartItemsService : IAddCartItemsService
    {
        private readonly ICartsRepository _cartsRepository;
        public AddCartItemsService(ICartsRepository cartsRepository) 
        {
            _cartsRepository = cartsRepository;
        }

        public async Task<AddToCartResponse> AddItemToCart(AddToCartRequest addToCart)
        {
            if(addToCart == null)
                throw new ArgumentNullException(nameof(addToCart));

            ValidationHelper.ModelValidator(addToCart);

            Carts cart = addToCart.ToCart();
            cart.Id = Guid.NewGuid();

            await _cartsRepository.AddItemToCart(cart);

            return cart.ToAddToCartResponse();
        }
    }
}
