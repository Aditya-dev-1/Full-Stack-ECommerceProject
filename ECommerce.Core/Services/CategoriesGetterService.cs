using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using RepositoryContracts;
using RespositoryContracts;
using ServiceContracts;
using ECommerce.Core.DTO;

namespace Services
{
    public class CategoriesGetterService : ICategoriesGetterService
    {
        private readonly ICategoriesRepository _categoriesRepository;
        public CategoriesGetterService(ICategoriesRepository categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
        }

        public async Task<List<CategoryResponse>> GetAllCategories()
        {
            List<Category> categories = await _categoriesRepository.GetAllCategories();
            return categories.Select(temp=>temp.ToCategoryResponse()).ToList();
        }

        public async Task<CategoryResponse?> GetCategoryByCategoryId(Guid? categoryId)
        {
           if(categoryId == null)
           {
               return null;
           }

           Category? matchingCategory =  await _categoriesRepository.GetCategoryByCategoryId(categoryId.Value);

            if(matchingCategory == null)
            {
                return null;
            }
            return matchingCategory.ToCategoryResponse();
        }
    }
}
