using HotChocolate.Types;
using CategoryService.Application.DTO;

namespace CategoryService.Api.GraphQL.Types
{
    public class TopLevelCategoryType : ObjectType<TopLevelCategory>
    {
        protected override void Configure(IObjectTypeDescriptor<TopLevelCategory> descriptor)
        {
            descriptor.Description("Level 1 Category Data.");
        }
    }
}