using HotChocolate.Types;
using CategoryService.Application.DTO;

namespace CategoryService.Api.GraphQL.Types
{
    public class SecondLevelCategoryType : ObjectType<SecondLevelCategory>
    {
        protected override void Configure(IObjectTypeDescriptor<SecondLevelCategory> descriptor)
        {
            descriptor.Description("Level 2 Category Data.");
        }
    }
}