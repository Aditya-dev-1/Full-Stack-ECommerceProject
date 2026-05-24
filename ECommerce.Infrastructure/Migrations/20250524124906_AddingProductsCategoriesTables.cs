using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ECommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingProductsCategoriesTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Cat_Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Product_Name = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    qty_in_stock = table.Column<int>(type: "int", nullable: false),
                    Product_Image_Url = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Cat_Name", "Status" },
                values: new object[,]
                {
                    { new Guid("12df2d0c-f849-4aae-99dc-393f4b4443dd"), "Mobile", true },
                    { new Guid("2a51063a-0d41-4784-97fc-0facbc3675b7"), "Men's Shirts", true },
                    { new Guid("99798cf8-a700-4254-b641-960a278ba9e1"), "Men's Pents", true },
                    { new Guid("d026bf3d-5e98-46fd-986e-82c97da687bf"), "Men's Shoes", true },
                    { new Guid("ed419975-8b09-4553-bb59-43b3ed382adb"), "Laptop", true }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "Price", "Product_Image_Url", "Product_Name", "qty_in_stock" },
                values: new object[,]
                {
                    { new Guid("51d09fd8-b2f5-4054-a05b-3d16688e89a9"), new Guid("2a51063a-0d41-4784-97fc-0facbc3675b7"), "Round Neck", 274.00m, "/Images/colorblock-roundneck-tshirt.jpg", "Men Colorblock Round Neck Pure Cotton T-Shirt", 10 },
                    { new Guid("6117ca3d-1ff8-4ddf-a414-f75ef8952b23"), new Guid("ed419975-8b09-4553-bb59-43b3ed382adb"), "This HP Victus Gaming Laptop 15 includes the Intel Core i5-13420H processor, offering power and efficiency.This laptop also provides vivid details, featuring 75 W TGP for sustained power, NVIDIA DLSS 3 AI Tensor Core, NVIDIA Ada Lovelace Architecture", 77800.00m, "/Images/HP Victus.jpg", "HP Victus Intel Core i5 13th Gen 13420H", 20 },
                    { new Guid("6408796b-56b0-4c2f-9ade-004c8d7c2624"), new Guid("d026bf3d-5e98-46fd-986e-82c97da687bf"), "Casual Sneaker Outdoor Shoes For Boys And Men", 391.00m, "/Images/casual-sneakers.jpg", "Casual Sneaker", 40 },
                    { new Guid("6fda0a9a-fa97-4f5a-9d70-5dca4a75414e"), new Guid("12df2d0c-f849-4aae-99dc-393f4b4443dd"), "iPhone 13.boasts an advanced dual-camera system that allows you to click mesmerising pictures with immaculate clarity.Furthermore, the lightning-fast A15 Bionic chip allows for seamless multitasking, elevating your performance to a new dimension", 44999.00m, "/Images/iphone-13.jpg", "Apple IPhone 13", 10 },
                    { new Guid("7e323a5d-0d1d-49f5-80c4-a2bb4c152f6a"), new Guid("ed419975-8b09-4553-bb59-43b3ed382adb"), "The Vivobook 15, a multipurpose laptop with a 12th-generation Intel U-Series CPU, DDR4 memory, and PCIe 3.0 SSD storage, delivers remarkable performance for daily work.Its three-sided NanoEdge slim-bezel display offers breathtaking images, and its Dirac-designed audio system produces rich, immersive sound.", 50990.00m, "/Images/ASUS-Vivobook-15.png", "ASUS Vivobook 15 ", 15 },
                    { new Guid("aacee317-8fff-4a95-8047-c2b5d97c1cb6"), new Guid("12df2d0c-f849-4aae-99dc-393f4b4443dd"), "iPhone 16. Built for Apple Intelligence.Featuring Camera Control. 48 MP Fusion camera.Five vibrant colours.And A18 chip.", 69999.00m, "/Images/iphone-16.jpg", "Apple IPhone 16", 10 },
                    { new Guid("cba35257-d692-4b76-b033-c2078b0b013b"), new Guid("2a51063a-0d41-4784-97fc-0facbc3675b7"), "Men Collar T-Shirt", 253.00m, "/Images/men-tshirt.jpg", "Men Solid Collar Cotton T-Shirt", 8 },
                    { new Guid("e1147aab-745b-4063-9fb5-99991357a1ad"), new Guid("d026bf3d-5e98-46fd-986e-82c97da687bf"), "WHITE HIGH PREMIUM QUALITY Running Shoes For Men", 478.00m, "/Images/white-sneakers.jpg", "White Shoes", 90 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
