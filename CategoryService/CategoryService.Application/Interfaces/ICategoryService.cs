using System.Threading.Tasks;
using CategoryService.Application.DTO;

namespace CategoryService.Application.Interfaces
{
    public interface ICategoryService
    {
       
        Task<CategoryHierarchy> GetCategoryTreeAsync();


        
        Task<CategoryHierarchy> GetTopLevelCategoriesAsync();


        
        Task<List<FlatMenuCategory>> GetMenuCategoriesAsync();


        /// <summary>
        /// Asynchronously retrieves a list of menu sections that match the specified section name.
        /// </summary>
        /// <param name="sectionName">The name of the menu section to retrieve. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of menu section data
        /// transfer objects that match the specified section name. The list is empty if no matching sections are found.</returns>
        //Task<List<MenuSectionDto>> GetMenuCategoryAsync(string sectionName);
    }
}