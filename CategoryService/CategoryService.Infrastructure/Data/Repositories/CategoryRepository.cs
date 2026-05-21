using CategoryService.Domain.Entities;
using CategoryService.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using CategoryService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CategoryService.Infrastructure.Data.Repositories
{
    // Primary Constructor injects 'dbContext' directly into class scope
    public class CategoryRepository(AppDbContext dbContext) : ICategoryRepository
    {

        /// <inheritdoc />
        public async Task<List<CategoryTree>> GetCategoryTreeAsync()
        {
            // Using the injected 'dbContext' parameter directly
            // .ToListAsync() -> select * from table
            return await dbContext.CategoryTrees.ToListAsync();
        }



        /// <inheritdoc />
        public async Task<List<Category>> GetMenuCategoriesAsync()
        {
            // Note: AsNoTracking() is great for read-only queries (performance boost)
            return await dbContext.Categories
                .AsNoTracking()
                .Where(c => c.Flag_Menu && c.CategoryId != 1 && c.FlagActive)
                .ToListAsync();
        }



        /// <inheritdoc />
        public async Task<List<TopLevelCategory>> GetTopLevelCategoriesAsync()
        {
            // This safely executes the SP and instantly maps the columns to your C# class
            return await dbContext.TopLevelCategoryResults
                .FromSqlRaw("EXEC Sproc_GetTopLevelCategory")
                .ToListAsync();
        }



        //fetch standard menu items for the requested section
        /// <inheritdoc />
        //public async Task<List<GroupedMenuResult>> GetGroupedUserMenuAsync(string sectionName)
        //{
        //    var param = new SqlParameter("@sectionName", sectionName);
        //    return await dbContext.GroupedMenuResults
        //        .FromSqlRaw("EXEC GetGroupedUserMenu @sectionName", param)
        //        .ToListAsync();
        //}



        //fetch most popular sub-categories for that section
        /// <inheritdoc />
        //public async Task<List<TopCategoryResult>> GetTopProductCategoryAsync(string sectionName)
        //{
        //    var param = new SqlParameter("@categoryName", sectionName);
        //    return await dbContext.TopCategoryResults
        //        .FromSqlRaw("EXEC Sproc_GetTopProductCategoryNew @categoryName", param)
        //        .ToListAsync();
        //}



        //fetch most popular authors across entire platform
        /// <inheritdoc />
        //public async Task<List<TopAuthorResult>> GetTopAuthorsUserAsync(int topCount)
        //{
        //    var param = new SqlParameter("@TopCount", topCount);
        //    return await dbContext.TopAuthorResults
        //        .FromSqlRaw("EXEC Sproc_GetTopAuthorsUser @TopCount", param)
        //        .ToListAsync();
        //}
    }
}