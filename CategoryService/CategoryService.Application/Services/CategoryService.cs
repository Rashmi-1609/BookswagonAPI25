using CategoryService.Application.DTO;
using CategoryService.Application.Interfaces;
using CategoryService.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CategoryService.Application.Services
{
    public class CategoryService(ICategoryRepository repository) : ICategoryService
    {
        // 1. GetMenuCategories (Flat list, O(N))
        public async Task<List<FlatMenuCategory>> GetMenuCategoriesAsync()
        {
            var categories = await repository.GetMenuCategoriesAsync();

            return categories.Select(c => new FlatMenuCategory
            {
                CategoryId = c.CategoryId,
                CategoryDisplayName = c.CategoryDisplayName == "Uncategorized" ? "General" : c.CategoryDisplayName,
                CategoryName = c.CategoryName == "Uncategorized" ? "General" : c.CategoryName,
                CategoryUrl = c.CategoryUrl,
                Flag_Menu = c.Flag_Menu
            }).ToList();
        }

        // 2. GetTopLevelCategory (GetAllCategories - GroupBy is O(N))
        public async Task<CategoryHierarchy> GetTopLevelCategoriesAsync()
        {
            var rawData = await repository.GetTopLevelCategoriesAsync();
            var lstCategoryTree = new List<TopLevelCategory>();

            var groupedData = rawData.GroupBy(r => r.Level1_ID);

            foreach (var group in groupedData)
            {
                var firstItem = group.First();

                var level1 = new TopLevelCategory
                {
                    CategoryName = firstItem.Category_DisplayName,
                    CategoryId = firstItem.ID_CategoryMapping,
                    CategoryUrl = firstItem.CategoryUrl,

                    SecondLevelCategories = group.Skip(1).Select(sub => new SecondLevelCategory
                    {
                        CategoryName = sub.Category_DisplayName,
                        CategoryID = sub.ID_CategoryMapping,
                        CategoryUrl = sub.CategoryUrl
                    }).ToList()
                };

                lstCategoryTree.Add(level1);
            }

            return new CategoryHierarchy { categoryTree = lstCategoryTree };
        }

        // 3. GetCategoryTree (Highly optimized ToLookup approach - O(N))
        public async Task<CategoryHierarchy> GetCategoryTreeAsync()
        {
            var rawTree = await repository.GetCategoryTreeAsync();

            // Create memory hash tables (O(N) operation)
            var level2Lookup = rawTree.Where(c => c.CategoryLevel == 2).ToLookup(c => c.LevelId1);
            var level3Lookup = rawTree.Where(c => c.CategoryLevel == 3).ToLookup(c => c.LevelId2);
            var level4Lookup = rawTree.Where(c => c.CategoryLevel == 4).ToLookup(c => c.LevelId3);

            // Declarative mapping using the lookups
            var TopLevelCategories = rawTree
                .Where(c => c.CategoryLevel == 1 && c.CategoryMappingId != 1)
                .Select(l1 => new TopLevelCategory
                {
                    CategoryId = l1.CategoryMappingId,
                    CategoryName = l1.ChildName,

                    SecondLevelCategories = level2Lookup[l1.CategoryMappingId].Select(l2 => new SecondLevelCategory
                    {
                        CategoryID = l2.CategoryMappingId,
                        CategoryName = l2.ChildName,

                        ThirdLevelCategories = level3Lookup[l2.CategoryMappingId].Select(l3 => new ThirdLevelCategory
                        {
                            CategoryId = l3.CategoryMappingId,
                            CategoryName = l3.ChildName,

                            // Note: Using ForthLevelCategories to match your DTO schema
                            FourthLevelCategories = level4Lookup[l3.CategoryMappingId].Select(l4 => new FourthLevelCategory
                            {
                                CategoryId = l4.CategoryMappingId,
                                CategoryName = l4.ChildName
                            }).ToList()
                        }).ToList()
                    }).ToList()
                }).ToList();

            return new CategoryHierarchy { categoryTree = TopLevelCategories };
        }
    }
}