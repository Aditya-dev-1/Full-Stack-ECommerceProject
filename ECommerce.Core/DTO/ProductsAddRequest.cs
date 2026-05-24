using System;
using System.ComponentModel.DataAnnotations;
using Entities;
using Microsoft.AspNetCore.Http;
using Services.Helpers.CustomValidations;



namespace ECommerce.Core.DTO
{
    /// <summary>
    /// Acts as DTO to add a Product 
    /// </summary>
    public class ProductsAddRequest
    {
        [Required(ErrorMessage ="Product Name can't be blank")]
        [StringLength(40)]
        public string? Product_Name { get; set; }

        [Required(ErrorMessage = "Product Description is required")]
        [StringLength(600)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Product Price can't be blank")]
        [DataType(DataType.Currency)]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "Products quantity is required")]
        public int? qty_in_stock { get; set; }


        [Required(ErrorMessage = "Please Select a category of product")]
        public Guid? CategoryId { get; set; }

        [Required(ErrorMessage = "Product image  is required")]
        [ValidatingFileType]
        public IFormFile? Product_Image{ get; set; }

        public string? ImagePath_url { get; set; }

        public int? Size { get; set; }

        public string? Color {  get; set; }

        public string? Material { get; set; }

        public Product ToProduct()
        {
            return new Product()
            {
                Product_Name = Product_Name,
                Description = Description,
                Price = Price.Value,
                CategoryId = CategoryId.Value,
                qty_in_stock = qty_in_stock.Value,
                Product_Image_Url = ImagePath_url,
             
            };
        }
    }
}
