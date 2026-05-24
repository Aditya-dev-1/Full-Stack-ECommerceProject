using System;
using ECommerce.Core.Domain.Entities;

namespace ECommerce.Core.DTO
{
    public class AddToCartResponse
    {
        public Guid CartId {  get; set; }   
        public Guid? UserId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal ProductPrice { get; set; }
        public string Product_Image_Path { get; set; }
        public string ProductName { get; set; }
    }

    public static class CartExtensions
    {
        public static AddToCartResponse ToAddToCartResponse(this Carts cart)
        {
            return new AddToCartResponse
            {
                CartId = cart.Id,
                UserId = cart.UserId.Value,
                ProductId = cart.Products.Id,
                Quantity = cart.Quantity,
                ProductPrice = cart.Products.Price,
                Product_Image_Path = cart.Products.Product_Image_Url,
                ProductName = cart.Products.Product_Name,
            };
        }
    }
}
