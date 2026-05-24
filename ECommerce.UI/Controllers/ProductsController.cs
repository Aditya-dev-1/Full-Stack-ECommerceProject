using System.Runtime.CompilerServices;
using System.Security;
using System.Security.AccessControl;
using ECommerce.Core.DTO;
using ECommerce.Core.ServiceContracts;
using ECommerceApplication.Filters.AcionFilter;
using Entities;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;

namespace ECommerceApplication.Controllers
{
    [Route("[controller]")]
    public class ProductsController : Controller
    {
        private readonly ICategoriesGetterService _categoriesGetterService;
        private readonly IProductsGetterService _productsGetterService;
        private readonly IAddCartItemsService _addCartItemsService;
        private readonly IGetCartItemsService _getCartItemsService;
        private readonly IRemoveCartItemService _removeCartItemService;
        private readonly IUpdateProductQuantityInCart _updateProductQuantityInCart;
        private readonly IAuthService _authService;
        public ProductsController(ICategoriesGetterService categoriesGetterService,IProductsGetterService productsGetterService, IAddCartItemsService addCartItemsService, IAuthService authService, IGetCartItemsService getCartItemsService, IRemoveCartItemService removeCartItemService, IUpdateProductQuantityInCart updateProductQuantityInCart)
        {
            _categoriesGetterService = categoriesGetterService;
            _productsGetterService = productsGetterService;
            _addCartItemsService = addCartItemsService;
            _getCartItemsService = getCartItemsService;
            _removeCartItemService = removeCartItemService;
            _updateProductQuantityInCart = updateProductQuantityInCart;
            _authService = authService;
        }


        [Route("/")]
        [Route("[action]")]
        public async Task<IActionResult> Index(string? searchString)
        {
            List<ProductResponse> products = await _productsGetterService.GetFilteredProducts(searchString);

            ViewBag.Categories = await _categoriesGetterService.GetAllCategories();
            ViewBag.searchString = searchString;
            return View(products);
        }


        [Route("[action]/{productId:guid}")]
        public async Task<IActionResult> ProductDetails(Guid productId)
        {
           string? userId =  await _authService.GetUserId(HttpContext.User);
            ProductResponse? product = await _productsGetterService.GetProductByProductId(productId);
            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }

            if (userId != null)
                ViewBag.IsCartItemExist = await _getCartItemsService.IsCartItemExist(Guid.Parse(userId), productId);
            else
                ViewBag.IsCartItemExist = false;
            return View(product);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> AddItemToCart(AddToCartRequest cartRequest)
        {
            List<ProductResponse> products = await _productsGetterService.GetAllProducts();

            string? userId =  await _authService.GetUserId(HttpContext.User);
            if (userId!= null)
                cartRequest.UserId = Guid.Parse(userId);
            else
                cartRequest.UserId = null;
            
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage);
                return View(nameof(ProductsController.Index), products);
            }

           await _addCartItemsService.AddItemToCart(cartRequest);
          ProductResponse product = await _productsGetterService.GetProductByProductId(cartRequest.ProductId);

            if(userId!=null)
                 ViewBag.IsCartItemExist = await _getCartItemsService.IsCartItemExist(Guid.Parse(userId), product.ProductId.Value);
            else
                ViewBag.IsCartItemExist = false;
            return View(nameof(ProductsController.ProductDetails),product);
        }


        [Route("[action]")]
        public async Task<IActionResult> CartItems()
        {
            decimal TotalPrice = 0;
           var userId = await _authService.GetUserId(HttpContext.User);
            List<AddToCartResponse> cartItems = await _getCartItemsService.GetAllCartItems(userId);

            foreach(var item in cartItems)
            {
                TotalPrice += item.ProductPrice*item.Quantity;
            }
            ViewBag.TotalPrice = TotalPrice;
            return View(cartItems);
        }


        [Route("[action]/{cartId:guid}")]
        public async Task<IActionResult> DeleteCartItem(Guid cartId)
        {
            decimal TotalPrice = 0;
            var userId = await _authService.GetUserId(HttpContext.User);

            bool isDeleted =  await _removeCartItemService.RemoveCartItem(cartId);

            List<AddToCartResponse> cartItems = await _getCartItemsService.GetAllCartItems(userId);
            foreach (var item in cartItems)
            {
                TotalPrice += item.ProductPrice;
            }
            ViewBag.TotalPrice = TotalPrice;

            if (isDeleted)
                return View(nameof(ProductsController.CartItems),cartItems);
            return View(nameof(ProductsController.CartItems),cartItems); 
        }

        
        [HttpPost]
        [IgnoreAntiforgeryToken]
        [Route("[action]")]
        public async Task<IActionResult> UpdateItemQuantity(AddToCartRequest cartRequest,int change)
        {
            decimal TotalPrice = 0;

            string? userId = await _authService.GetUserId(HttpContext.User);
            if(userId == null)
            {
                List<ProductResponse> products = await _productsGetterService.GetAllProducts();

                ViewBag.Categories = await _categoriesGetterService.GetAllCategories();
                return RedirectToAction(nameof(ProductsController.Index));
            }
              

            if (!ModelState.IsValid)
            {
                List<AddToCartResponse> cartItems = await _getCartItemsService.GetAllCartItems(userId);
                ViewBag.Errors = ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage);
                return RedirectToAction(nameof(ProductsController.CartItems), cartItems);
            }

            await _updateProductQuantityInCart.UpdateProductQuantityInCartItem(change, Guid.Parse(userId), cartRequest.ProductId.Value);

            List<AddToCartResponse> updatedCartItems = await _getCartItemsService.GetAllCartItems(userId);

            foreach (var item in updatedCartItems)
            {
                TotalPrice += item.ProductPrice*item.Quantity;
            }
            ViewBag.TotalPrice = TotalPrice;

            return RedirectToAction(nameof(ProductsController.CartItems), updatedCartItems);
        }
    }
}
