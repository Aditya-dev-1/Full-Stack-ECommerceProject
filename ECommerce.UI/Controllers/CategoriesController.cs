using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ECommerce.Core.DTO;

namespace ECommerceApplication.Controllers
{
    [Route("[controller]")]
    public class CategoriesController : Controller
    {
        private readonly IProductsGetterService _productsGetterService;
        public CategoriesController(IProductsGetterService productsGetterService) 
        { 
          _productsGetterService = productsGetterService;
        }


        [Route("[action]/{categoryId:guid}")]
        public async Task<IActionResult>  Category(Guid categoryId)
        {
            List<ProductResponse>? products = await _productsGetterService.GetProductsBasedOnCategoryId(categoryId);
            if(products == null)
            {
                return RedirectToAction("Index", "Products");
            }
            
            ViewBag.CategoryName = products[0]?.CategoryName;
            return View(products);
        }
    }
}
