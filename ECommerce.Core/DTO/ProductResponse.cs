using System;
using Entities;
using Microsoft.AspNetCore.Http;


namespace ECommerce.Core.DTO
{
    /// <summary>
    /// DTO class which is used as return type of Service class methods
    /// </summary>
    public class ProductResponse
    {
        public Guid? ProductId { get; set; }

        public string? Product_Name { get; set; }

        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public Guid? CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public int? qty_in_stock { get; set; }

        public IFormFile? Product_Image { get; set; }

        public string? Product_Image_Path {  get; set; }

        public int? Size { get; set; }

        public string? Color { get; set; }

        public string? Material { get; set; }

        public ProductsUpdateRequest ToProductUpdateRequest()
        {
            return new ProductsUpdateRequest()
            {
                Price = Price,
                Product_Name = Product_Name,
                Description = Description,
                qty_in_stock = qty_in_stock,
                Product_Image = Product_Image,
                CategoryId = CategoryId,
                ProductId = ProductId,
                Product_Image_Url = Product_Image_Path
            };
        }
    }

    public static class ProductExtensions
    {
        public static ProductResponse ToProductResponse(this Product product)
        {
            return new ProductResponse()
            {
                ProductId = product.Id,
                Product_Name = product.Product_Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                Product_Image_Path = product.Product_Image_Url,
                CategoryName = product.Categories?.Cat_Name,
                qty_in_stock= product.qty_in_stock,
            };
        }
    }
}
