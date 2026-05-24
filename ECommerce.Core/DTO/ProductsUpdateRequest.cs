using System;
using System.ComponentModel.DataAnnotations;
using Entities;
using Microsoft.AspNetCore.Http;
using Services.Helpers.CustomValidations;


namespace ECommerce.Core.DTO
{
    /// <summary>
    /// DTO class for updating any product 
    /// </summary>
    public class ProductsUpdateRequest
    {
        [Required(ErrorMessage ="Product Id is required")]
        public Guid? ProductId { get; set; }


        [Required(ErrorMessage = "Product Name can't be blank")]
        [StringLength(40)]
        public string? Product_Name { get; set; }

        [Required(ErrorMessage = "Product Description is required")]
        [StringLength(600)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Product Price can't be blank")]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "Available products quantity is required")]
        public int? qty_in_stock { get; set; }

        [Required(ErrorMessage = "Please Select a category of product")]
        public Guid? CategoryId { get; set; }

        public string? Product_Image_Url { get; set; }

        [ValidatingFileType(isRequiredForNew:false)]
        public IFormFile? Product_Image { get; set; }

        public int? Size { get; set; }

        public string? Color { get; set; }

        public string? Material { get; set; }


        public Product ToProduct()
        {
            return new Product()
            {
                Product_Name = Product_Name,
                Product_Image_Url = Product_Image_Url,
                Price = Price.Value,
                Description = Description,
                qty_in_stock = qty_in_stock.Value,
             
            };
        }
    }
}
