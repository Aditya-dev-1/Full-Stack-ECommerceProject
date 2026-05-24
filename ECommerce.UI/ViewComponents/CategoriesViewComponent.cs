using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ECommerce.Core.DTO;

namespace ECommerceApplication.ViewComponents
{
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly ICategoriesGetterService _categoriesGetterService;

        public CategoriesViewComponent(ICategoriesGetterService categoriesGetterService)
        {
            _categoriesGetterService = categoriesGetterService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<CategoryResponse> categories = await _categoriesGetterService.GetAllCategories();
            return View(categories);
        }
        
    }
}
