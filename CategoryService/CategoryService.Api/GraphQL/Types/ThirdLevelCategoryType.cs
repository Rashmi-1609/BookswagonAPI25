using HotChocolate.Types;
using CategoryService.Application.DTO;

namespace CategoryService.Api.GraphQL.Types
{
    public class ThirdLevelCategoryType : ObjectType<ThirdLevelCategory>
    {
        protected override void Configure(IObjectTypeDescriptor<ThirdLevelCategory> descriptor)
        {
            descriptor.Description("Level 3 Category Data.");
        }
    }
}