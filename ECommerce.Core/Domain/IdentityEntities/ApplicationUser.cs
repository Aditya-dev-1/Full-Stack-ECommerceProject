using System;
using ECommerce.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Core.Domain.IdentityEntities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ICollection<Carts> Carts { get; set; } = new List<Carts>();  // one user can have many cart items 
    }
}
