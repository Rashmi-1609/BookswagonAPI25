using System.Threading.Tasks;
using HotChocolate;
using CategoryService.Application.DTO;
using CategoryService.Application.Interfaces;
//using CategoryService.Api.GraphQL.Types; 

namespace CategoryService.Api.GraphQL.Queries
{   
    public class CategoryQuery
    {
        /// <summary>
        /// Asynchronously retrieves the category hierarchy as a tree structure.
        /// </summary>
        /// <param name="categoryService">The service used to access category data. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of category data transfer
        /// objects representing the category tree.</returns>
        
      // [GraphQLType(typeof(CategoryHierarchyDtoType))]
        public Task<CategoryHierarchy> GetCategoryTreeAsync([Service] ICategoryService categoryService)
            => categoryService.GetCategoryTreeAsync();



        /// <summary>
        /// Asynchronously retrieves a list of categories to be displayed in the menu.
        /// </summary>
        /// <param name="categoryService">The category service used to obtain menu category data. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of category data transfer
        /// objects for the menu. The list will be empty if no categories are available.</returns>

     //  [GraphQLType(typeof(ListType<FlatCategoryType>))]
        public Task<List<FlatMenuCategory>> GetCategoryMenuAsync([Service] ICategoryService categoryService)
            => categoryService.GetMenuCategoriesAsync();



        /// <summary>
        /// Asynchronously retrieves all top-level categories.
        /// </summary>
        /// <param name="categoryService">The service used to access category data. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of CategoryDto objects
        /// representing the top-level categories. The list will be empty if no categories are found.</returns>
    //   [GraphQLType(typeof(TopCategoryHierarchyType))]
        public Task<CategoryHierarchy> GetAllCategoriesAsync([Service] ICategoryService categoryService)
            => categoryService.GetTopLevelCategoriesAsync();



        // check this method
        // Note: The argument 'string sectionName' comes FIRST, before the injected service.

        /// <summary>
        /// Asynchronously retrieves a list of menu categories for the specified section.
        /// </summary>
        /// <param name="sectionName">The name of the menu section for which to retrieve categories. Cannot be null or empty.</param>
        /// <param name="categoryService">The service used to access category data. This parameter is provided by dependency injection.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of menu category data
        /// transfer objects for the specified section. The list will be empty if no categories are found.</returns>
        //public Task<List<MenuSectionDto>> GetMenuCategoryAsync(
        //    string sectionName,
        //    [Service] ICategoryService categoryService)
        //{
        //    return categoryService.GetMenuCategoryAsync(sectionName);

        //}

    }
}