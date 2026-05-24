using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ECommerce.Core.Domain.IdentityEntities;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Core.Domain.Entities
{
    //But for real app two tables can be better Cart and CartItems.
    public class Carts
    {
        [Key]
        public Guid Id { get; set; }
        public int Quantity { get; set; } = 1;
        public Guid ProductId { get; set; }
        public Guid? UserId { get; set; }

        //Navigation Properties
        [ForeignKey(nameof(UserId))]
        public ApplicationUser? Users { get; set; }  // one user can add multiple products so there will be mutiple insertion in cart table from one user so one-to-many 

        [ForeignKey(nameof(ProductId))]
        public Product Products { get; set; } // a same  produt can be added by mutiple users one-to-many 

    }
}
