using HotChocolate.Types;
using CategoryService.Application.DTO;

namespace CategoryService.Api.GraphQL.Types
{
    public class FlatMenuCategoryType : ObjectType<FlatMenuCategory>
    {
        protected override void Configure(IObjectTypeDescriptor<FlatMenuCategory> descriptor)
        {
            // We rename the schema type to "Category" so the frontend queries don't break
            descriptor.Name("Category");
            descriptor.Description("A flat list of standard menu categories.");
        }
    }
}