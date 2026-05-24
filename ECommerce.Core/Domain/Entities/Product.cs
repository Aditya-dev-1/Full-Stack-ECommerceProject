using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ECommerce.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    /// <summary>
    /// Products Class Represents Different Products In A Category 
    /// </summary>
    public class Product
    {
        [Key] //PrimaryKey
        public Guid Id { get; set; }

        public Guid CategoryId { get; set; }

        [StringLength(400)]
        public string Product_Name { get; set; }

        public string Description { get; set; }

        [Precision(18,2)]
        public decimal Price { get; set; }

        public int qty_in_stock { get; set; }

        [StringLength(1000)]
        public string Product_Image_Url {  get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Categories { get; set; } // NavigationProperty To categories as One Product->One Category 

        public ICollection<Carts> CartItems { get; set; } = new List<Carts>();    // a product will be in one cart of the user
    }
}
