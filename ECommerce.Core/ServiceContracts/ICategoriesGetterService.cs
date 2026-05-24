using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using ECommerce.Core.DTO;

namespace ServiceContracts
{
    public interface ICategoriesGetterService
    {
        /// <summary>
        /// Returns list of all categories from data store
        /// </summary>
        /// <returns>Returns list of all categories</returns>
        Task<List<CategoryResponse>> GetAllCategories();


        /// <summary>
        /// Search for category 
        /// </summary>
        /// <param name="categoryId">the category to be search</param>
        /// <returns>Returns the category based on id</returns>
        Task<CategoryResponse?> GetCategoryByCategoryId(Guid? categoryId);
    }
}
