using System;
using System.ComponentModel.DataAnnotations;
using ECommerce.Core.Domain.Entities;

namespace ECommerce.Core.DTO
{
    public class AddToCartRequest
    {
        public Guid? UserId { get; set; }
        [Required]
        public Guid? ProductId { get; set; }
        public int Quantity { get; set; } = 1;

        public Carts ToCart()
        {
            return new Carts()
            {
                UserId = UserId.Value,
                ProductId = ProductId.Value,
                Quantity = Quantity
            };
        }
    }
}
