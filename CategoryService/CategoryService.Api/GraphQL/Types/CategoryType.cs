//using HotChocolate.Types;
//using CategoryService.Application.DTO;

//namespace CategoryService.Api.GraphQL.Types
//{
//    // 1. MENU CATEGORY MASK (Keeps all fields, hides all lists)
//    public class FlatCategoryType : ObjectType<CategoryDto>
//    {
//        protected override void Configure(IObjectTypeDescriptor<CategoryDto> descriptor)
//        {
//            descriptor.Name("MenuCategory");
//            descriptor.Ignore(c => c.SecondLevelCategories);
//            descriptor.Ignore(c => c.ThirdLevelCategories);
//            descriptor.Ignore(c => c.FourthLevelCategories);
//            // Keeps Flag_Menu, CategoryDisplayName, CategoryUrl, etc.
//        }
//    }


//    // 2. CATEGORY TREE MASKS (Only CategoryId & CategoryName)
//    public class CategoryLevel1Type : ObjectType<CategoryDto>
//    {
//        protected override void Configure(IObjectTypeDescriptor<CategoryDto> descriptor)
//        {
//            descriptor.Name("TopLevelCategory");
//            descriptor.Field(c => c.SecondLevelCategories).Type<ListType<CategoryLevel2Type>>();

//            // HIDE UNWANTED LISTS
//            descriptor.Ignore(c => c.ThirdLevelCategories);
//            descriptor.Ignore(c => c.FourthLevelCategories);

//            // HIDE UNWANTED FIELDS (Clean up Banana Cake Pop)
//            descriptor.Ignore(c => c.CategoryDisplayName);
//            descriptor.Ignore(c => c.CategoryUrl);
//            descriptor.Ignore(c => c.Flag_Menu);
//        }
//    }

//    public class CategoryLevel2Type : ObjectType<CategoryDto>
//    {
//        protected override void Configure(IObjectTypeDescriptor<CategoryDto> descriptor)
//        {
//            descriptor.Name("SecondLevelCategory");
//            descriptor.Field(c => c.ThirdLevelCategories).Type<ListType<CategoryLevel3Type>>();

//            descriptor.Ignore(c => c.SecondLevelCategories);
//            descriptor.Ignore(c => c.FourthLevelCategories);

//            // HIDE UNWANTED FIELDS
//            descriptor.Ignore(c => c.CategoryDisplayName);
//            descriptor.Ignore(c => c.CategoryUrl);
//            descriptor.Ignore(c => c.Flag_Menu);
//        }
//    }

//    public class CategoryLevel3Type : ObjectType<CategoryDto>
//    {
//        protected override void Configure(IObjectTypeDescriptor<CategoryDto> descriptor)
//        {
//            descriptor.Name("ThirdLevelCategory");
//            descriptor.Field(c => c.FourthLevelCategories).Type<ListType<CategoryLevel4Type>>();

//            descriptor.Ignore(c => c.SecondLevelCategories);
//            descriptor.Ignore(c => c.ThirdLevelCategories);

//            // HIDE UNWANTED FIELDS
//            descriptor.Ignore(c => c.CategoryDisplayName);
//            descriptor.Ignore(c => c.CategoryUrl);
//            descriptor.Ignore(c => c.Flag_Menu);
//        }
//    }

//    public class CategoryLevel4Type : ObjectType<CategoryDto>
//    {
//        protected override void Configure(IObjectTypeDescriptor<CategoryDto> descriptor)
//        {
//            descriptor.Name("FourthLevelCategory");

//            descriptor.Ignore(c => c.SecondLevelCategories);
//            descriptor.Ignore(c => c.ThirdLevelCategories);
//            descriptor.Ignore(c => c.FourthLevelCategories);

//            // HIDE UNWANTED FIELDS
//            descriptor.Ignore(c => c.CategoryDisplayName);
//            descriptor.Ignore(c => c.CategoryUrl);
//            descriptor.Ignore(c => c.Flag_Menu);
//        }
//    }

//    public class CategoryHierarchyDtoType : ObjectType<CategoryHierarchyDto>
//    {
//        protected override void Configure(IObjectTypeDescriptor<CategoryHierarchyDto> descriptor)
//        {
//            descriptor.Name("CategoryHierarchy");
//            descriptor.Field(c => c.CategoryTree).Name("categoryTree").Type<ListType<CategoryLevel1Type>>();
//        }
//    }

//    // 3. ALL CATEGORIES MASKS (CategoryId, CategoryName, & CategoryUrl)

//    public class TopCategoryHierarchyType : ObjectType<CategoryHierarchyDto>
//    {
//        protected override void Configure(IObjectTypeDescriptor<CategoryHierarchyDto> descriptor)
//        {
//            descriptor.Name("TopCategoryHierarchy");
//            descriptor.Field(c => c.CategoryTree).Name("categoryTree").Type<ListType<TopCategoryLevel1Type>>();
//        }
//    }

//    public class TopCategoryLevel1Type : ObjectType<CategoryDto>
//    {
//        protected override void Configure(IObjectTypeDescriptor<CategoryDto> descriptor)
//        {
//            descriptor.Name("TopCategoryFirstLevel");
//            descriptor.Field(c => c.SecondLevelCategories).Type<ListType<TopCategoryLevel2Type>>();

//            descriptor.Ignore(c => c.ThirdLevelCategories);
//            descriptor.Ignore(c => c.FourthLevelCategories);

//            // HIDE UNWANTED FIELDS (Keeps CategoryUrl!)
//            descriptor.Ignore(c => c.CategoryDisplayName);
//            descriptor.Ignore(c => c.Flag_Menu);
//        }
//    }

//    public class TopCategoryLevel2Type : ObjectType<CategoryDto>
//    {
//        protected override void Configure(IObjectTypeDescriptor<CategoryDto> descriptor)
//        {
//            descriptor.Name("TopCategorySecondLevel");

//            descriptor.Ignore(c => c.SecondLevelCategories);
//            descriptor.Ignore(c => c.ThirdLevelCategories);
//            descriptor.Ignore(c => c.FourthLevelCategories);

//            // HIDE UNWANTED FIELDS (Keeps CategoryUrl!)
//            descriptor.Ignore(c => c.CategoryDisplayName);
//            descriptor.Ignore(c => c.Flag_Menu);
//        }
//    }
//}