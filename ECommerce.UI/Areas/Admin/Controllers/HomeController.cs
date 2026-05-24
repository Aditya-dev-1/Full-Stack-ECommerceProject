using ECommerce.Core.DTO;
using ECommerceApplication.Filters.AcionFilter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using Services;

namespace ECommerce.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly ICategoriesGetterService _categoriesGetterService;
        private readonly IProductsAdderService _productsAdderService;
        private readonly IProductsUpdateService _productsUpdateService;
        private readonly IProductsDeleteService _productsDeleteService;
        private readonly IProductsGetterService _productsGetterService;

        public HomeController(ICategoriesGetterService categoriesGetterService, IProductsUpdateService productsUpdateService, IProductsAdderService productsAdderService, IProductsDeleteService productsDeleteService, IProductsGetterService productsGetterService)
        {
            _categoriesGetterService = categoriesGetterService;
            _productsAdderService = productsAdderService;
            _productsDeleteService = productsDeleteService;
            _productsUpdateService = productsUpdateService;
            _productsGetterService = productsGetterService;
        }

        [Route("[action]")]
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Create()
        {
            List<CategoryResponse> categories = await _categoriesGetterService.GetAllCategories();
            ViewBag.Categories = categories.Select(temp => new SelectListItem()
            {
                Text = temp.Cat_Name,
                Value = temp.CategoryId.ToString(),
            });
            return View();
        }


        [HttpPost]
        [Route("[action]")]
        [TypeFilter(typeof(ProductCreateAndEditActionFilter))]
        public async Task<IActionResult> Create(ProductsAddRequest productsRequest)
        {
            //calling AddProduct() of service class 
            await _productsAdderService.AddProduct(productsRequest);
            return RedirectToAction("Index", "Products");
        }



        [HttpGet]
        [Route("[action]/{productId:guid}")]
        public async Task<IActionResult> Delete(Guid productId)
        {
            ProductResponse? product = await _productsGetterService.GetProductByProductId(productId);

            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }
            return View(product);
        }


        [HttpPost]
        [Route("[action]/{productId:guid}")]
        public async Task<IActionResult> Delete(ProductsUpdateRequest productsUpdateRequest)
        {
            ProductResponse? productResponse = await _productsGetterService.GetProductByProductId(productsUpdateRequest.ProductId);

            if (productResponse == null)
            {
                return RedirectToAction("Index", "Products");
            }

            await _productsDeleteService.DeleteProduct(productResponse.ProductId);
            return RedirectToAction("Index", "Products");
        }


        [HttpGet]
        [Route("[action]/{productId:guid}")]
        public async Task<IActionResult> Edit(Guid productId)
        {
            ProductResponse? productsResponse = await _productsGetterService.GetProductByProductId(productId);

            if (productsResponse == null)
            {
                return RedirectToAction("Index", "Products");
            }

            ProductsUpdateRequest productsUpdateRequest = productsResponse.ToProductUpdateRequest();

            List<CategoryResponse> categories = await _categoriesGetterService.GetAllCategories();
            ViewBag.Categories = categories.Select(temp => new SelectListItem()
            {
                Text = temp.Cat_Name,
                Value = temp.CategoryId.ToString(),
            });

            ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(err => err.ErrorMessage).ToList();

            return View(productsUpdateRequest);
        }

        [HttpPost]
        [Route("[action]/{productId:guid}")]
        [TypeFilter(typeof(ProductCreateAndEditActionFilter))]
        public async Task<IActionResult> Edit(ProductsUpdateRequest productsRequest)
        {
            ProductResponse? productResponse = await _productsGetterService.GetProductByProductId(productsRequest.ProductId);
            if (productResponse == null)
            {
                return RedirectToAction("Index", "Products");
            }
            await _productsUpdateService.UpdateProduct(productsRequest);
            return RedirectToAction("Index", "Products");
        }


    }
}
