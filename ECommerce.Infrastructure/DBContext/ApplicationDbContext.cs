using System;
using System.Text.Json;
using ECommerce.Core.Domain.Entities;
using ECommerce.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,Guid>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }


        public DbSet<Product> Products { get; set; }    

        public DbSet<Category> Categories { get; set; }

        public DbSet<Carts> Carts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<Category>().ToTable("Categories");
            modelBuilder.Entity<Carts>().ToTable("Carts");


            //Sedding data into Categories Table
            string categoriesJson = File.ReadAllText("Categories.json");
            List<Category> categories = JsonSerializer.Deserialize<List<Category>>(categoriesJson);

            foreach (Category category in categories)
            {
                modelBuilder.Entity<Category>().HasData(category);
            }

            //Sedding data into Products Table
            string productsJson = File.ReadAllText("Products.json");
            List<Product> products = JsonSerializer.Deserialize<List<Product>>(productsJson);

            foreach (Product product in products)
            {
                modelBuilder.Entity<Product>().HasData(product);
            }

        }
    }
}
