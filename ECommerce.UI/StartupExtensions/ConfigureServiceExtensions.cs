using ECommerce.Core.Domain.IdentityEntities;
using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.DTO;
using ECommerce.Core.ServiceContracts;
using ECommerce.Core.Services;
using ECommerce.Infrastructure.Repositories;
using Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using RespositoryContracts;
using ServiceContracts;
using Services;

namespace ECommerceApplication.StartupExtensions
{
    public static class ConfigureServiceExtensions
    {

        public static void ConfigureServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddControllersWithViews( options =>
            {
                //Adding ValidateAntiForgeryToken at global level for all post action methods to prevent from XSRF(a type of malicious request made to our server to steal data) attacks .The server will verify whether the request is made to it is  valid or not by verifying antiforgerytoken (server willl create on run time with hash algo SHA256(session Id + salt))

                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

                services.AddScoped<IProductsRepository,ProductsRepositories>();
            services.AddScoped<ICategoriesRepository, CategoriesRepository>();
            services.AddScoped<ICartsRepository,CartsRepository>();

            services.AddScoped<IProductsAdderService,ProductsAdderService>();
            services.AddScoped<IProductsDeleteService, ProductsDeleteService>();
            services.AddScoped<IProductsGetterService, ProductsGetterService>();
            services.AddScoped<IProductsUpdateService, ProductsUpdateService>();
            services.AddScoped<ICategoriesGetterService, CategoriesGetterService>();
            services.AddScoped<IImageAdderService, ImageAdderService>();
            services.AddScoped<IImageDeleterService, ImageDeleterService>();
            services.AddScoped<IImageUpdaterService, ImageUpdaterService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAddCartItemsService, AddCartItemsService>();
            services.AddScoped<IGetCartItemsService, GetCartItemsService>();
            services.AddScoped<IRemoveCartItemService, RemoveCartItemService>();
            services.AddScoped<IUpdateProductQuantityInCart, UpdateProductQuantityInCart>();
            services.AddScoped<IEmailSenderService, EmailSenderService>();
            services.Configure<SMTPConfigOptions>(configuration.GetSection("SMTPConfig"));

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                //Password Complexity
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredUniqueChars = 3; // a password must have 3 diff. characters eg aditya it has 5 unique characters

                options.SignIn.RequireConfirmedEmail = true;
            })
             
            .AddEntityFrameworkStores<ApplicationDbContext>()
            
            .AddDefaultTokenProviders()
            
            .AddUserStore<UserStore<ApplicationUser,ApplicationRole,ApplicationDbContext,Guid>>()
            
            .AddRoleStore<RoleStore<ApplicationRole,ApplicationDbContext,Guid>>();


            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme; // by this the user will be logged in as the cookie will generatd and stored in browser and for all request browser will send it to server will check it is valid or not (simple means like identity do the user will be logged in to web app until the cokie will be remobved from browser memory and that will happen when user log out)
                                                                                          
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme; // this line means when the user clicks on google login button it will redirect him to google login page.    
            }).AddGoogle(options =>
                       {
                           IConfigurationSection googleAuthNSection =
                           configuration.GetSection("Authentication:Google");
                           options.ClientId = googleAuthNSection["ClientId"];
                           options.ClientSecret = googleAuthNSection["ClientSecret"];
                       });
        }
    }
}
