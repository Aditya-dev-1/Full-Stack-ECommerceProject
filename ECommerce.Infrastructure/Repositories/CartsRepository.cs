using System;
using ECommerce.Core.Domain.Entities;
using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.DTO;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
    public class CartsRepository : ICartsRepository
    {
        private readonly ApplicationDbContext _db;
        public CartsRepository(ApplicationDbContext db) 
        {
            _db = db;
        }

        public async Task<Carts> AddItemToCart(Carts cart)
        {
           await _db.Carts.AddAsync(cart);
            await _db.SaveChangesAsync();
            return cart;
        }

        public async Task<List<Carts>> GetAllCartItemsWithUserId(Guid userId)
        {
            return await _db.Carts
                .Include(t=>t.Products)
                .Where(t=>t.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Carts>> GetAllCartItemsWithoutUserId()
        {
            return await _db.Carts
                .Include(t => t.Products)
                .Where(t=>t.UserId==Guid.Empty)
                .ToListAsync();
        }

        public async Task<Carts?> GetCartItemByCartId(Guid cartId)
        { 
          return await _db.Carts
                .Include(t=>t.Products)
                .FirstOrDefaultAsync(temp=>temp.Id==cartId);
        }

        public async Task<bool> RemoveItemFromCartByCartId(Guid cartId)
        {
            _db.Carts.RemoveRange(_db.Carts.Where(t=>t.Id==cartId));
           int rowsDeleted =  await _db.SaveChangesAsync();
            return rowsDeleted > 0;
        }

        public async Task<Carts> UpdateCartItemQuantity(Carts cart,int updatedQuantity)
        {
            Carts? mathcingCart = await _db.Carts.Include(t=>t.Products).FirstOrDefaultAsync(temp=>temp.Id== cart.Id);

            if (mathcingCart == null)
            {
                return cart;
            }
            mathcingCart.Quantity += updatedQuantity;
            await _db.SaveChangesAsync();
            if(mathcingCart.Quantity == 0)
            {
               await RemoveItemFromCartByCartId(mathcingCart.Id);
            }
            return mathcingCart;
        }

        public async Task<bool> IsCartItemExist(Guid userId, Guid productId)
        {
          Carts? cart =  await _db.Carts.FirstOrDefaultAsync(t => t.UserId == userId && t.ProductId == productId);
            if(cart == null)
                return false;
            return true;
        }

        public async Task<Carts?> GetcartItemByUserIdProductId(Guid userId, Guid productId)
        {
           return await _db.Carts.FirstOrDefaultAsync(temp=>temp.UserId == userId && temp.ProductId==productId);
            
        }
    }
}
