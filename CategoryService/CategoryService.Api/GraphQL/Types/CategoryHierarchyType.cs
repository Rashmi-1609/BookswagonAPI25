using HotChocolate.Types;
using CategoryService.Application.DTO;

namespace CategoryService.Api.GraphQL.Types
{
    public class CategoryHierarchyType : ObjectType<CategoryHierarchy>
    {
        protected override void Configure(IObjectTypeDescriptor<CategoryHierarchy> descriptor)
        {
            descriptor.Description("The root wrapper containing the full category tree structure.");

            // Ensures the GraphQL schema explicitly names this array 'categoryTree'
            descriptor.Field(c => c.categoryTree).Name("categoryTree");
        }
    }
}