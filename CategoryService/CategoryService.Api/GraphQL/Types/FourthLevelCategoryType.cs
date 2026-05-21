using HotChocolate.Types;
using CategoryService.Application.DTO;

namespace CategoryService.Api.GraphQL.Types
{
    public class FourthLevelCategoryType : ObjectType<FourthLevelCategory>
    {
        protected override void Configure(IObjectTypeDescriptor<FourthLevelCategory> descriptor)
        {
            descriptor.Description("Level 4 Category Data. This is the maximum depth.");
        }
    }
}