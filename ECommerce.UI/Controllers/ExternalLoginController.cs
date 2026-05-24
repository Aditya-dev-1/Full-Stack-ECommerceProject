using ECommerce.Core.Domain.IdentityEntities;
using ECommerce.Core.DTO;
using ECommerce.Core.ServiceContracts;
using ECommerceApplication.Controllers;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using ServiceContracts;
using Services;

namespace ECommerce.UI.Controllers
{
    [Route("[controller]")]
    public class ExternalLoginController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IProductsGetterService _productsGetterService;
        private readonly ICategoriesGetterService _categoriesGetterService;

        public ExternalLoginController(IAuthService authService, IProductsGetterService productsGetterService, ICategoriesGetterService categoriesGetterService)
        {
            _authService = authService;
            _productsGetterService = productsGetterService;
            _categoriesGetterService = categoriesGetterService;
        }

        [IgnoreAntiforgeryToken] //This will ignore the checking of AntyForgeryToken for this action method
        [HttpPost]
        [Route("signin-google")]
        public async Task<IActionResult> GoogleLogin([FromForm] string credential)
        {
            //  Validating the Google token
              GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(credential);
              IdentityResult? result = await _authService.Register(payload);

              List<ProductResponse> products = await _productsGetterService.GetAllProducts();
              ViewBag.Categories = await _categoriesGetterService.GetAllCategories();

            if (result == null)
            {
                  bool isLogin = await _authService.Login(payload);
                  if (isLogin)
                  {
                      return RedirectToAction(nameof(ProductsController.Index), "Products",products);
                  }

                  return Json("Login Failed! Please try again");

            }
            if (result.Succeeded)
            {
                    bool isLogin = await _authService.Login(payload);
                    if (isLogin)
                    {
                        return RedirectToAction(nameof(ProductsController.Index), "Products", products);
                    }

                    return Json("Login Failed! Please try again");
            }

              return Json("Login Failed! Please try again"); 

            
        }
    }
}
