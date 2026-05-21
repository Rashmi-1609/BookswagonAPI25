using CategoryService.Domain.Entities;
using CategoryService.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CategoryService.Domain.Interfaces
{
    public interface ICategoryRepository
    {   
        
        Task<List<CategoryTree>> GetCategoryTreeAsync();

        
        Task<List<Category>> GetMenuCategoriesAsync();


        Task<List<TopLevelCategory>> GetTopLevelCategoriesAsync();


        /// <summary>
        /// Asynchronously retrieves a list of user menu items grouped by category for the specified section.
        /// </summary>
        /// <param name="sectionName">The name of the section for which to retrieve grouped user menu items. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of grouped menu results
        /// for the specified section. The list is empty if no menu items are found.</returns>
       //Task<List<GroupedMenuResult>> GetGroupedUserMenuAsync(string sectionName);


        /// <summary>
        /// Asynchronously retrieves a list of the top product categories for the specified section.
        /// </summary>
        /// <param name="sectionName">The name of the section for which to retrieve top product categories. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of top product categories
        /// for the specified section. The list will be empty if no categories are found.</returns>
       // Task<List<TopCategoryResult>> GetTopProductCategoryAsync(string sectionName);

        /// <summary>
        /// Asynchronously retrieves a list of the top authors based on user activity.
        /// </summary>
        /// <param name="topCount">The maximum number of top authors to return. Must be greater than zero.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of top author results,
        /// limited to the specified count. The list will be empty if no authors are found.</returns>
        //Task<List<TopAuthorResult>> GetTopAuthorsUserAsync(int topCount);
    }
}