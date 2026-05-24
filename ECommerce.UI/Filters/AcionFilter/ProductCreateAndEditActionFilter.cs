using ECommerceApplication.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ECommerce.Core.DTO;

namespace ECommerceApplication.Filters.AcionFilter
{
    public class ProductCreateAndEditActionFilter : IAsyncActionFilter
    {
        private readonly ICategoriesGetterService _categoriesGetterService;
        public ProductCreateAndEditActionFilter(ICategoriesGetterService categoriesGetterService)
        {
            _categoriesGetterService = categoriesGetterService;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.Controller is ProductsController productsController)
            {
                if (!productsController.ModelState.IsValid)
                {
                    List<CategoryResponse> categories = await _categoriesGetterService.GetAllCategories();
                   productsController.ViewBag.Categories = categories.Select(temp => new SelectListItem()
                    {
                        Text = temp.Cat_Name,
                        Value = temp.CategoryId.ToString(),
                    });

                    productsController.ViewBag.Errors = productsController.ModelState.Values.SelectMany(temp => temp.Errors).Select(err => err.ErrorMessage);

                    var productRequest = context.ActionArguments["productsRequest"]; 
                    context.Result = productsController.View(productRequest); //short-circuitng (stopping the execution of action method if there are model errors). 
                }
                else
                {
                    await next(); //calling subsequent filter if present or action method 
                }
            }
            else
            {
                await next(); //calling subsequent filter if present or action method 
            }
            
        }
    }
}
