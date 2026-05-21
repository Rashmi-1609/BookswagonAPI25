using System;
using System.Collections.Generic;

namespace CategoryService.Application.DTO
{
    public class CategoryHierarchy
    {
        public List<TopLevelCategory> categoryTree { get; set; } = new();
    }

    public class TopLevelCategory
    {
        public string? CategoryName { get; set; }
        public Int32 CategoryId { get; set; }
        public string? CategoryUrl { get; set; }
        public List<SecondLevelCategory>? SecondLevelCategories { get; set; } = new();
    }

    public class SecondLevelCategory
    {
        public string? CategoryName { get; set; }
        public Int32 CategoryID { get; set; } 
        public string? CategoryUrl { get; set; }
        public List<ThirdLevelCategory>? ThirdLevelCategories { get; set; } = new();
    }

    public class ThirdLevelCategory
    {
        public string?  CategoryName { get; set; }
        public Int32 CategoryId { get; set; }
        public string? CategoryUrl { get; set; }
        public List<FourthLevelCategory>? FourthLevelCategories { get; set; } = new();
    }

    public class FourthLevelCategory
    {
        public string? CategoryName { get; set; }
        public Int32 CategoryId { get; set; }
        public string? CategoryUrl { get; set; }
    }

    // for GetCategoryMenu API
    public class FlatMenuCategory
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryDisplayName { get; set; }
        public string? CategoryUrl { get; set; }
        public bool? Flag_Menu { get; set; }
    }
}