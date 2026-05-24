using System;
using System.Security.Claims;
using ECommerce.Core.Domain.Entities;
using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.DTO;
using ECommerce.Core.ServiceContracts;

namespace ECommerce.Core.Services
{
    public class GetCartItemsService : IGetCartItemsService
    {
        private readonly ICartsRepository _cartRepository;
        private readonly IAuthService _authService;
        public GetCartItemsService(ICartsRepository cartRepository, IAuthService authService) 
        {
            _cartRepository = cartRepository;
            _authService = authService;
        }


        public async Task<List<AddToCartResponse>> GetAllCartItems(string? userId)
        {
            if (userId == null)
            {
                List<Carts> cartsItemsUserIdIsNull = await _cartRepository.GetAllCartItemsWithoutUserId();
                return cartsItemsUserIdIsNull.Select(temp => temp.ToAddToCartResponse())
                    .ToList();
            }

            List<Carts> cartsItems = await _cartRepository.GetAllCartItemsWithUserId(Guid.Parse(userId));
            return cartsItems.Select(temp => temp.ToAddToCartResponse())
                .ToList();
        }

        public async Task<bool> IsCartItemExist(Guid userId, Guid productId)
        {
            if (userId == Guid.Empty || productId == Guid.Empty)
                return false;

            bool isProductPresent = await _cartRepository.IsCartItemExist(userId,productId);

            if(isProductPresent)
                return true;
            return false;
        }
    }
}
